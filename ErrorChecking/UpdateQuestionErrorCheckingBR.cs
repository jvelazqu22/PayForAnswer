using Domain.App_GlobalResources;
using Domain.Constants;
using Domain.Exceptions;
using Domain.Models;
using Domain.Models.Entities;
using Domain.Models.Helper;
using Domain.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ErrorChecking
{
    public class UpdateQuestionErrorCheckingBR : ErrorCheckingBR
    {
        public Error CanTheQuestionBeUpdated(UpdateQuestionViewModel questionViewModel)
        {
            if (string.IsNullOrEmpty(questionViewModel.Description))
                return new Error() { ErrorFound = true, Message = CommonResources.MsgErrorMissingDescription };

            double numberOfBytesInDescription = Encoding.UTF8.GetByteCount(questionViewModel.Description);
            if (numberOfBytesInDescription >= StorageSizeLimits.MAX_DESCRIPTION_SIZE)
                return new Error() { ErrorFound = true, Message = CommonResources.MaxStorageDescriptionSizeReached };

            Error error = new AttachFilesErrorChecking().AttachmentsExceedUploadLimit(new List<Attachment>(), questionViewModel.Files, questionViewModel.FilesToBeUploaded, questionViewModel.Amount, AttachmentType.QUESTION_ATTACHMENT);
            if ( error.ErrorFound ) return error;

            if (IsDescriptionEmpty(questionViewModel.Description))
                return new Error() { ErrorFound = true, Message = CommonResources.MsgErrorMissingDescription };

            return new Error() { ErrorFound = false };
        }
    }
}
