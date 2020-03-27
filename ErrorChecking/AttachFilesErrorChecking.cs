using Domain.App_GlobalResources;
using Domain.Constants;
using Domain.Models.Entities;
using Domain.Models.Helper;
using System;
using System.Collections.Generic;
using System.Web;

namespace ErrorChecking
{
    public class AttachFilesErrorChecking
    {
        public Error AttachmentsExceedUploadLimit(List<Attachment> existingAttachments, IEnumerable<HttpPostedFileBase> newAttachments, string commaSeparatedFilesToUpload, decimal questionAmount, int attachmentType)
        {
            double maxSizeBytesLimit = attachmentType == AttachmentType.QUESTION_ATTACHMENT
                ? GetStorageMaxSizeBytesForQuestion((double)questionAmount)
                : GetStorageMaxSizeBytesForAnswer((double)questionAmount);

            long byteCount = 0;
            existingAttachments.ForEach(a => byteCount += a.SizeInBytes);

            if(newAttachments != null)
                foreach (HttpPostedFileBase item in newAttachments)
                    if (item != null && Array.Exists(commaSeparatedFilesToUpload.Split(','), s => s.Equals(item.FileName)))
                        if (item.ContentLength > 0)
                            byteCount += item.ContentLength;

            if (byteCount >= StorageSizeLimits.MAX_ATTACHMENT_SIZE)
                return new Error() { ErrorFound = true, Message = CommonResources.MaxStorageAttachmentSizeReached };

            if (byteCount > maxSizeBytesLimit)
                return new Error() { ErrorFound = true, Message = GetExceededSizeLimitErrorMsg(maxSizeBytesLimit, attachmentType) };

            return new Error() { ErrorFound = false };
        }

        //public Error AttachmentsExceedUploadLimit(IEnumerable<HttpPostedFileBase> attachments, string commaSeparatedFilesToUpload, decimal questionAmount, int attachmentType)
        //{
        //    double maxSizeBytesLimit = attachmentType == AttachmentType.QUESTION_ATTACHMENT
        //        ? GetStorageMaxSizeBytesForQuestion((double)questionAmount)
        //        : GetStorageMaxSizeBytesForAnswer((double)questionAmount);

        //    long byteCount = 0;
        //    foreach (HttpPostedFileBase item in attachments)
        //        if (item != null && Array.Exists(commaSeparatedFilesToUpload.Split(','), s => s.Equals(item.FileName)))
        //            if (item.ContentLength > 0)
        //                byteCount += item.ContentLength;

        //    if ( byteCount >= StorageSizeLimits.MAX_ATTACHMENT_SIZE )
        //        return new Error() { ErrorFound = true, Message = CommonResources.MaxStorageAttachmentSizeReached };

        //    if (byteCount > maxSizeBytesLimit)
        //        return new Error() { ErrorFound = true, Message = GetExceededSizeLimitErrorMsg(maxSizeBytesLimit, attachmentType) };

        //    return new Error() { ErrorFound = false };
        //}

        public double GetStorageMaxSizeBytesForQuestion(double questionAmount)
        {
            int numberOfStorageBlocksInQuestion = 1;
            if (questionAmount > General.QuestionMoneyAmountStorageBlockSize)
                numberOfStorageBlocksInQuestion = (int)(questionAmount / General.QuestionMoneyAmountStorageBlockSize);

            return StorageSize.BytesInAMegabyte * General.MegabytesPerQuestionLevel * numberOfStorageBlocksInQuestion;
        }

        public double GetStorageMaxSizeBytesForAnswer(double questionAmount)
        {
            return GetStorageMaxSizeBytesForQuestion(questionAmount) * General.AnswerStorageMultiplier;
        }

        public string GetExceededSizeLimitErrorMsg(double maxSizeBytesLimit, int attachmentType)
        {
            double storageLimitInMegabytes = maxSizeBytesLimit / StorageSize.BytesInAMegabyte;
            string errorMsg = attachmentType == AttachmentType.QUESTION_ATTACHMENT
                ? string.Format(CommonResources.QuestionMaxStorageSizeErrorMsg, General.MegabytesPerQuestionLevel, General.QuestionMoneyAmountStorageBlockSize, storageLimitInMegabytes)
                : string.Format(CommonResources.AnswerMaxStorageSizeErrorMsg, storageLimitInMegabytes);

            return errorMsg;
        }
    }
}
