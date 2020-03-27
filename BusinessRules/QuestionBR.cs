using AutoMapper;
using Domain.Constants;
using Domain.Exceptions;
using Domain.Models;
using Domain.Models.Entities;
using Domain.Models.TableEntities.SubjectTable;
using Domain.Models.ViewModel;
using ErrorChecking;
using Repository.Interfaces;
using Repository.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace BusinessRules
{
    public class QuestionBR
    {
        public QuestionBR() 
        {
            Mapper.CreateMap<CreateQuestionViewModel, Question>();
            Mapper.CreateMap<Question, QuestionDetailsViewModel>();
        }

        public Question CreateQuestion(CreateQuestionViewModel questionViewModel, IQuestionSubjectRepository questionSubjectRepository, IBlobRepository blobRepository)
        {
            CalculateFees(questionViewModel);
            Question questionModel = Mapper.Map<CreateQuestionViewModel, Question>(questionViewModel);
            BlobBR blobBr = new BlobBR();
            new SubjectBR().AddSubjectsToQuestionModelFromCommaDelimitedSubjects(questionViewModel, questionModel, questionSubjectRepository);
            new PaymentBR().AddFirstPaymentToQuestionModel(questionViewModel, questionModel);
            new MarketingBR().AddFirstCampaignToPaymentModel(questionViewModel, questionModel.QuestionPaymentDetails.FirstOrDefault());
            blobBr.AddAttachmentsToQuestionModel(questionViewModel, questionModel);

            questionModel.StatusId = StatusValues.QuestionRequested;
            questionModel.CreatedOn = DateTime.UtcNow;
            questionModel.UpdatedOn = DateTime.UtcNow;
            questionModel.UserId = questionViewModel.UserId;
            questionSubjectRepository.InsertQuestion(questionModel);
            questionSubjectRepository.Save();

            //questionViewModel.Description = new Ads().AppendTopAdAndDescription(questionViewModel.Description, questionModel.Id.ToString(), questionModel.Amount, questionModel.Title);

            try
            {
                blobBr.UploadFiles(questionViewModel, questionModel, blobRepository);
                questionSubjectRepository.Save();
            }
            catch(Exception ex)
            {
                questionModel.StatusId = StatusValues.ErrorUploadingFiles;
                questionSubjectRepository.UpdateQuestion(questionModel);
                throw ex;
            }

            return questionModel;
        }

        public Question UpdateQuestion(UpdateQuestionViewModel questionViewModel, Question question, IQuestionRepository questionRepository, IBlobRepository blobRepository)
        {
            BlobBR blobBr = new BlobBR();
            blobBr.AddAttachmentsToQuestionModel(questionViewModel, question);
            questionRepository.UpdateQuestion(question);
            blobBr.UploadFiles(questionViewModel, question, blobRepository);
            question.UpdatedOn = DateTime.UtcNow;
            questionRepository.UpdateQuestion(question);
            return question;
        }

        public void SaveToTables(Question questionModel)
        {
            Guid questionID = Guid.NewGuid();
            List<SubjectQuestion> subjectQuestion = new List<SubjectQuestion>();
            questionModel.Subjects.ToList().ForEach(s => subjectQuestion.Add(new SubjectQuestion() { SubjectName = s.SubjectName, QuestionID = questionID }));
        }

        public Question PrevalidateQuestion(ValidateQuestionViewModel validateQuestionModel, IQuestionRepository questionRepository)
        {
            Question questionModel = questionRepository.GetQuestionByID(validateQuestionModel.QuestionId);
            new QuestionErrorCheckingBR().ValidateIfQuestionCanBePrevalidated(questionModel, validateQuestionModel.IdOfUserTryingToMakeUpdate);
            questionModel.StatusId = StatusValues.WaitingForPaymentNotification;
            questionModel.QuestionPaymentDetails.Where(r => r.PaymentId == validateQuestionModel.PaymentId).FirstOrDefault().Payment.StatusId = StatusValues.WaitingForPaymentNotification;
            questionRepository.UpdateQuestion(questionModel);
            return questionModel;
        }

        public CreateQuestionViewModel CalculateFees(CreateQuestionViewModel questionViewModel)
        {
            Decimal percentageFee = General.ChargePercentageFee;
            Decimal fixFee = General.ChargeFixFee;

            questionViewModel.Amount = Math.Round(questionViewModel.Amount, 2);
            questionViewModel.MarketingBudgetPerDay = Math.Round((decimal)questionViewModel.MarketingBudgetPerDay, 2);
            questionViewModel.Fee = ((questionViewModel.Amount + questionViewModel.MarketingBudgetPerDay) * percentageFee) + fixFee;

            questionViewModel.Fee = questionViewModel.Fee > General.MaxChargeFee 
                ? General.MaxChargeFee 
                : Math.Round(Convert.ToDecimal(questionViewModel.Fee), 2);

            questionViewModel.TotalMarketingBudget = questionViewModel.MarketingBudgetPerDay * questionViewModel.NumberOfCampaignDays;
            questionViewModel.Total = questionViewModel.Amount + questionViewModel.TotalMarketingBudget + questionViewModel.Fee;
            questionViewModel.Total = Math.Round(Convert.ToDecimal(questionViewModel.Total), 2);

            return questionViewModel;
        }

        public Question AddQuestionComment(Guid questionId, string userName, string comment, IQuestionRepository questionRepository, IBlobRepository blobRepository)
        {
            Question questionModel = questionRepository.GetQuestionByID(questionId);
            if (questionModel == null) throw new RequestNotFoundException(string.Format("Question id: {0}", questionId));
            if (string.IsNullOrWhiteSpace(comment)) throw new Exception(string.Format("Question id: {0}. Empty comment provided", questionId));

            var blobPathAndName = new QuestionUrlsAndPaths().GetQuestionCommentsPath(questionId);
            string newComments = string.Format(Html.COMMENTS, userName, DateTime.UtcNow, comment);
            string oldComments = blobRepository.GetHtmlFileContent(blobPathAndName, StorageValues.COMMENTS_CONTAINER);
            newComments += oldComments;

            blobRepository.AddUpdateHtmlFileContent(blobPathAndName, newComments, StorageValues.COMMENTS_CONTAINER);
            Task.Factory.StartNew(() => new EmailBR().CommentNotifications(questionModel.Id, new QuestionRepository(), userName, CommentType.QuestionComment, comment));

            return questionModel;
        }

        public QuestionDetailsViewModel GetQuestionDetailsModel(Guid questionId, IQuestionRepository questionRepository, IBlobRepository blobRepository, DateTime utcNow, int currentUserId, List<string> currentUserRoles)
        {
            Question question = questionRepository.GetQuestionByID(questionId);
            QuestionUrlsAndPaths qUrlsAndPath = new QuestionUrlsAndPaths();
            AnswerUrlsAndPaths aUrlsAndPath = new AnswerUrlsAndPaths();

            if (question == null) throw new RequestNotFoundException(string.Format("Question id: {0}", questionId));
            QuestionDetailsViewModel questionDetailsViewModel = new QuestionDetailsViewModel();
            questionDetailsViewModel = Mapper.Map<Question, QuestionDetailsViewModel>(question);
            questionDetailsViewModel.EmailAddressOfUserWhoPostedQuestion = question.User.Email;
            questionDetailsViewModel.Comments = blobRepository.GetHtmlFileContent(qUrlsAndPath.GetQuestionCommentsPath(questionId), StorageValues.COMMENTS_CONTAINER);
            questionDetailsViewModel.Description = blobRepository.GetHtmlFileContent(qUrlsAndPath.GetQuestionDescriptionPath(questionId), StorageValues.DESCRIPTION_CONTAINER);
            questionDetailsViewModel.IsQuestionOpenForAnswers = StatusList.CONFIRM_PAYMENT_STATUS.Contains(questionDetailsViewModel.StatusId) ? true : false;
            questionDetailsViewModel.UserId = question.User.Id;
            questionDetailsViewModel.CanTheQuestionBeUpdated = new UpdateQuestionBR().CanTheQuestionBeUpdated(question, currentUserId, currentUserRoles);

            Parallel.ForEach<Answer>(questionDetailsViewModel.Answers, answer =>
            {
                answer.Description = blobRepository.GetHtmlFileContent(aUrlsAndPath.GetAnswerDescriptionPath(questionId, answer.Id), StorageValues.DESCRIPTION_CONTAINER);
                answer.Comments = blobRepository.GetHtmlFileContent(aUrlsAndPath.GetAnswerCommentsPath(questionId, answer.Id), StorageValues.COMMENTS_CONTAINER);
            });

            if (currentUserId == question.UserId)
                new MarketingBR().GetQuestionActiveMarketingCampaignSummary(question, questionDetailsViewModel, utcNow);
            return questionDetailsViewModel;
        }

        public UpdateQuestionViewModel GetUpdateQuestionDetailsModel(Guid questionId, IQuestionRepository questionRepository, IBlobRepository blobRepository, DateTime utcNow, int currentUserId)
        {
            Question question = questionRepository.GetQuestionByID(questionId);
            QuestionUrlsAndPaths qUrlsAndPath = new QuestionUrlsAndPaths();

            if (question == null) throw new RequestNotFoundException(string.Format("Question id: {0}", questionId));
            UpdateQuestionViewModel questionDetailsViewModel = new UpdateQuestionViewModel();
            questionDetailsViewModel.Title = question.Title;
            questionDetailsViewModel.QuestionID = question.Id;
            questionDetailsViewModel.Amount = question.Amount;
            questionDetailsViewModel.Attachments = question.Attachments.ToList();
            questionDetailsViewModel.Description = blobRepository.GetHtmlFileContent(qUrlsAndPath.GetQuestionDescriptionPath(questionId), StorageValues.DESCRIPTION_CONTAINER);
            questionDetailsViewModel.UserId = question.User.Id;
            questionDetailsViewModel.CreatedOn = question.CreatedOn;
            questionDetailsViewModel.Subjects = question.Subjects.ToList();

            return questionDetailsViewModel;
        }
    }
}
