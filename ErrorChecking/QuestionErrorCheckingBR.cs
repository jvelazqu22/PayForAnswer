using Domain.App_GlobalResources;
using Domain.Constants;
using Domain.Exceptions;
using Domain.Models;
using Domain.Models.Entities;
using Domain.Models.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ErrorChecking
{
    public class QuestionErrorCheckingBR : ErrorCheckingBR
    {
        Decimal minimumQuestionAmount = General.MinimumQuestionAmount;
        Decimal minimumMarketingBudget = General.MinimumMarketingBudgetPerDay;
        int minimumMarketingDays = General.MinimumMarketingDays;

        public Error CanTheQuestionBeCreated(CreateQuestionViewModel questionViewModel)
        {
            Error error = new SubjectsErrorChecking().IsQuestionSubjectListValid(questionViewModel.CommaDelimitedSubjects);
            if (error.ErrorFound) return error;

            if (string.IsNullOrEmpty(questionViewModel.Description))
                return new Error() { ErrorFound = true, Message = CommonResources.MsgErrorMissingDescription };

            double numberOfBytesInDescription = Encoding.UTF8.GetByteCount(questionViewModel.Description);
            if (numberOfBytesInDescription >= StorageSizeLimits.MAX_DESCRIPTION_SIZE)
                return new Error() { ErrorFound = true, Message = CommonResources.MaxStorageDescriptionSizeReached };

            error = new AttachFilesErrorChecking().AttachmentsExceedUploadLimit(new List<Attachment>(), questionViewModel.Files, questionViewModel.FilesToBeUploaded, questionViewModel.Amount, AttachmentType.QUESTION_ATTACHMENT);
            if ( error.ErrorFound ) return error;

            if (IsDescriptionEmpty(questionViewModel.Description))
                return new Error() { ErrorFound = true, Message = CommonResources.MsgErrorMissingDescription };

            if (questionViewModel.Amount < minimumQuestionAmount || questionViewModel.MarketingBudgetPerDay < minimumMarketingBudget ||
                    questionViewModel.NumberOfCampaignDays < minimumMarketingDays)
            {
                string msg = string.Format(CommonResources.MsgErrorInvalidQuestionAmount, minimumQuestionAmount, minimumMarketingBudget, minimumMarketingDays);
                return new Error() { ErrorFound = true, Message = msg };
            }

            if (string.IsNullOrEmpty(questionViewModel.GoogleSearchKeywords1))
                return new Error() { ErrorFound = true, Message = CommonResources.MsgErrorMissingGoogleSearchKeyword };
            if (string.IsNullOrEmpty(questionViewModel.GoogleSearchKeywords2))
                return new Error() { ErrorFound = true, Message = CommonResources.MsgErrorMissingGoogleSearchKeyword };
            if (string.IsNullOrEmpty(questionViewModel.GoogleSearchKeywords3))
                return new Error() { ErrorFound = true, Message = CommonResources.MsgErrorMissingGoogleSearchKeyword };
            if (string.IsNullOrEmpty(questionViewModel.GoogleSearchKeywords4))
                return new Error() { ErrorFound = true, Message = CommonResources.MsgErrorMissingGoogleSearchKeyword };
            if (string.IsNullOrEmpty(questionViewModel.GoogleSearchKeywords5))
                return new Error() { ErrorFound = true, Message = CommonResources.MsgErrorMissingGoogleSearchKeyword };

            return new Error() { ErrorFound = false };
        }

        public void ValidateIfQuestionCanBePrevalidated(Question questionModel, int UserIdOfUserTryingToPrevalidateQuestion)
        {
            string details = "Question id: " + questionModel.Id.ToString() + " - Question status: " + questionModel.StatusId +
                " - Question user id: " + questionModel.UserId + " - Id of user trying to make update: " + UserIdOfUserTryingToPrevalidateQuestion.ToString();

            if ( !(questionModel.StatusId == StatusValues.QuestionRequested || questionModel.StatusId == StatusValues.WaitingForPaymentNotification) )
                throw new QuestionPrevalidationException(details);

            if (UserIdOfUserTryingToPrevalidateQuestion != questionModel.UserId)
                throw new QuestionPrevalidationException(details);
        }
    }
}
