using BusinessRules.Interfaces;
using Domain.App_GlobalResources;
using Domain.Constants;
using Domain.Exceptions;
using Domain.Models.Entities;
using Domain.Models.Entities.Identity;
using Domain.Models.Helper;
using Domain.Models.ViewModel;
using ErrorChecking;
using Repository.Interfaces;
using System;
using System.Threading.Tasks;
using Utilities;

namespace BusinessRules
{
    public class AnswerBR
    {
        public void UpdateAnswerStatusAndCheckIfPosterAnswerHisOwnQuestionAndIfTheQuestionStatusIdNeedsToBeUpdated(int statusId, Answer answerModel)
        {
            if (statusId == StatusValues.Accepted && answerModel.UserId == answerModel.Question.UserId)
                statusId = StatusValues.AcceptedByRequester;

            if (statusId == StatusValues.Accepted || statusId == StatusValues.AcceptedByRequester || statusId == StatusValues.Paid)
            {
                answerModel.Question.StatusId = statusId;
                if (statusId == StatusValues.Paid) answerModel.Payment.StatusId = statusId;
            }

            answerModel.DoesAnswerNeedsToBePaid = false;
            if (answerModel.StatusId != statusId && answerModel.StatusId != StatusValues.Paid && statusId == StatusValues.Accepted)
            {
                answerModel.DoesAnswerNeedsToBePaid = true;
                Payment payment = new Payment() { StatusId = StatusValues.PaymentCreatedButNotSentToPayPalYet, Total = answerModel.Question.Amount };
                answerModel.Payment = payment;
            }

            answerModel.StatusId = statusId;
        }

        public void IfStatusIsPaidSetFlagToSendEmailToWinner(Answer answerModel)
        {
            if (answerModel.StatusId == StatusValues.Paid)
                answerModel.SendEmailToWinner = true;
        }

        public void CheckIfUpdatesAreNeededAndMakeThem(int statusId, Answer answerModel, IAnswerRepository answerRepository)
        {
            answerModel.QuestionTitle = answerModel.Question.Title;
            answerModel.QuestionAmount = answerModel.Question.Amount;
            answerModel.EmailAddressOfUserWhoPostedAnswer = answerModel.User.Email;
            answerModel.SendEmailToWinner = false;
            if (answerModel.StatusId != statusId)
            {
                UpdateAnswerStatusAndCheckIfPosterAnswerHisOwnQuestionAndIfTheQuestionStatusIdNeedsToBeUpdated(statusId, answerModel);
                answerRepository.UpdateAnswer(answerModel);
                IfStatusIsPaidSetFlagToSendEmailToWinner(answerModel);
                answerModel.StatusName = answerModel.Status.DisplayName;
            }
        }

        public Answer UserUpdateAnswerStatus(long answerId, int userIdThatChangeAnswerStatus, int statusId, IAnswerRepository answerRepository)
        {
            Answer answerModel = answerRepository.GetAnswerByID(answerId);
            if (answerModel == null) throw new AnswerNotFoundException(string.Format(Errors.ANSWER_ID, answerId));
            int userIdWhoPostedQuestion = answerModel.Question.UserId;
            if (userIdWhoPostedQuestion != userIdThatChangeAnswerStatus)
                throw new UpdateAnswerStatusAttack(string.Format(Errors.INVALID_USER_ID_TRIED_TO_UPDATE_ANSWER,
                    answerId, userIdThatChangeAnswerStatus, userIdWhoPostedQuestion));
            CheckIfUpdatesAreNeededAndMakeThem(statusId, answerModel, answerRepository);
            return answerModel;
        }

        public Answer UpdateAnswerStatus(long answerId, int statusId, IAnswerRepository answerRepository)
        {
            Answer answerModel = answerRepository.GetAnswerByID(answerId);
            if (answerModel == null) throw new AnswerNotFoundException(string.Format("Answer id: ", answerId));
            CheckIfUpdatesAreNeededAndMakeThem(statusId, answerModel, answerRepository);
            return answerModel;
        }

        public Answer AddAnswerComment(long answerId, string userName, string comment, IAnswerRepository answerRepository, IBlobRepository blobRepository, IQuestionRepository questionRepository)
        {
            Answer answerModel = answerRepository.GetAnswerByID(answerId);
            if (answerModel == null) throw new AnswerNotFoundException(string.Format("Answer id: {0}", answerId));
            if (string.IsNullOrWhiteSpace(comment)) throw new Exception(string.Format("Answer id: {0}. Empty comment provided", answerId));

            var blobPathAndName = new AnswerUrlsAndPaths().GetAnswerCommentsPath((Guid)answerModel.QuestionId, answerModel.Id);
            string newComments = string.Format(Html.COMMENTS, userName, DateTime.UtcNow, comment);
            string oldComments = blobRepository.GetHtmlFileContent(blobPathAndName, StorageValues.COMMENTS_CONTAINER);
            newComments += oldComments;

            try
            {
                blobRepository.AddUpdateHtmlFileContent(blobPathAndName, newComments, StorageValues.COMMENTS_CONTAINER);
                Task.Factory.StartNew(() => new EmailBR().CommentNotifications(answerModel.Question.Id, questionRepository, userName, CommentType.AnswerComment, comment));
            }
            catch (Exception ex)
            {
                try
                {
                    blobRepository.AddUpdateHtmlFileContent(blobPathAndName, Html.COMMENTS_DEFAULT_VALUE, StorageValues.COMMENTS_CONTAINER);
                }
                catch(Exception)
                {
                    answerRepository.DeleteAnswer(answerModel);
                }
                throw ex;
            }

            return answerModel;
        }

        public void MarkAnswerAsPaidAndSendEmail(long answerId, IAnswerRepository answerRepository, IEmailBR emailBR)
        {
            Answer answerModel = UpdateAnswerStatus(answerId, StatusValues.Paid, answerRepository);
            if (answerModel.SendEmailToWinner)
            {
                string confirmationUrl;
                confirmationUrl = Urls.QUESTION_URL + answerModel.QuestionId.ToString();

                var confirmationLink = String.Format(Html.LINK_PLACE_HOLDER, confirmationUrl, answerModel.QuestionTitle);

                EmailModel emailModel = new EmailModel();
                emailModel.From = Emails.SUPPORT_EMAIL_ADDRESS;
                emailModel.SenderName = CommonResources.PayForAnswer;
                emailModel.To = answerModel.EmailAddressOfUserWhoPostedAnswer;
                emailModel.Subject = String.Format(CommonResources.EmailSubjectPaidAnswer, answerModel.QuestionAmount);
                emailModel.Body = String.Format(CommonResources.EmailBodyPaidAnswer,
                    confirmationLink, answerModel.QuestionAmount);
                emailBR.Send(emailModel);
            }
        }

        public Answer GetAnswerToPay(long answerId, IAnswerRepository answerRepository)
        {
            Answer answerModel = answerRepository.GetAnswerByID(answerId);
            answerModel.EmailAddressOfUserWhoPostedAnswer = answerModel.User.Email;
            answerModel.QuestionAmount = answerModel.Question.Amount;

            return answerModel;
        }

        public Answer SaveNewAnswerAndSendEmail(ApplicationUser user, NewAnswerViewModel newAnswerViewModel, IAnswerRepository answerRepository, IEmailBR emailBr, IBlobRepository blobRepository)
        {
            BlobBR blobBr = new BlobBR();
            Answer answerModel = new AnswerErrorCheckingBR().CheckForErrorsAndProceedIfThereAreNoExceptions(user.Id, newAnswerViewModel);
            blobBr.AddAttachmentsToAnswerModel(newAnswerViewModel, answerModel);
            answerRepository.InsertAnswer(answerModel);

            try
            {
                blobBr.UploadFiles(newAnswerViewModel, answerModel, blobRepository);
                answerRepository.Save();
            }
            catch(Exception ex)
            {
                answerRepository.DeleteAnswer(answerModel);
                throw ex;
            }

            if (newAnswerViewModel.NewAnswerToMyQuestion && newAnswerViewModel.QuestionUserID != answerModel.UserId)
            {
                string confirmationUrl = Urls.QUESTION_URL + answerModel.QuestionId.ToString();
                var confirmationLink = String.Format(Html.LINK_PLACE_HOLDER, confirmationUrl, newAnswerViewModel.QuestionTitle);
                var unsubscribeUrl = Urls.E_NOTIFICATIONS_URL;
                var unsubscribeLink = string.Format(Html.LINK_PLACE_HOLDER, unsubscribeUrl, CommonResources.Unsubscribe);

                EmailModel emailModel = new EmailModel();
                emailModel.From = CommonResources.EmailFromNewAnswer;
                emailModel.SenderName = CommonResources.PayForAnswer;
                emailModel.To = newAnswerViewModel.EmailAddressOfUserWhoPostedQuestion;
                emailModel.Subject = CommonResources.EmailSubjectNewAnswer;
                emailModel.Body = String.Format(CommonResources.EmailBodyNewAnswer, confirmationLink, unsubscribeLink);
                emailBr.Send(emailModel);
            }

            return answerModel;
        }

    }
}
