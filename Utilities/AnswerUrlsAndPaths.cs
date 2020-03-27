using Domain.Constants;
using Domain.Models.Entities;
using System;
using System.Web;

namespace Utilities
{
    public class AnswerUrlsAndPaths
    {
        public string GetAnswerAttachmentPath(Attachment attachment, Guid questionID, long answerID)
        {
            return string.Format(StorageValues.ANSWER_ATTACHMENT_PATH_PLACE_HOLDER,
                                    questionID,
                                    answerID,
                                    attachment.ID,
                                    attachment.Name);
        }

        public string GetAnswerAttachmentPrimaryUrl(Attachment attachment, Guid questionID, long answerID)
        {
            return string.Format(StorageValues.ANSWER_ATTACHMENT_URL_PLACE_HOLDER,
                                    StorageValues.STORAGE_URL_PRIMARY,
                                    StorageValues.ATTACHMENT_CONTAINER,
                                    questionID,
                                    answerID,
                                    attachment.ID,
                                    HttpUtility.UrlPathEncode(attachment.Name));
        }

        public string GetAnswerAttachmentSecondaryUrl(Attachment attachment, Guid questionID, long answerID)
        {
            return string.Format(StorageValues.ANSWER_ATTACHMENT_URL_PLACE_HOLDER,
                                    StorageValues.STORAGE_URL_SECONDARY,
                                    StorageValues.ATTACHMENT_CONTAINER,
                                    questionID,
                                    answerID,
                                    attachment.ID,
                                    HttpUtility.UrlPathEncode(attachment.Name));
        }

        public string GetAnswerDescriptionPrimaryUrl(Guid questionID, long answerID)
        {
            return string.Format(StorageValues.ANSWER_DESCRIPTION_URL_PLACE_HOLDER,
                                    StorageValues.STORAGE_URL_PRIMARY,
                                    StorageValues.DESCRIPTION_CONTAINER,
                                    questionID,
                                    answerID,
                                    StorageValues.DESCRIPTION_FILE_NAME);
        }

        public string GetAnswerDescriptionPath(Guid questionID, long answerID)
        {
            return string.Format(StorageValues.ANSWER_DESCRIPTION_PATH_PLACE_HOLDER,
                                    questionID,
                                    answerID,
                                    StorageValues.DESCRIPTION_FILE_NAME);
        }

        public string GetAnswerCommentsPrimaryUrl(Guid questionID, long answerID)
        {
            return string.Format(StorageValues.ANSWER_COMMENT_URL_PLACE_HOLDER,
                                    StorageValues.STORAGE_URL_PRIMARY,
                                    StorageValues.COMMENTS_CONTAINER,
                                    questionID,
                                    answerID,
                                    StorageValues.COMMENTS_FILE_NAME);
        }

        public string GetAnswerCommentsPath(Guid questionID, long answerID)
        {
            return string.Format(StorageValues.ANSWER_COMMENT_PATH_PLACE_HOLDER,
                                    questionID, 
                                    answerID,
                                    StorageValues.COMMENTS_FILE_NAME);
        }
    }
}
