using Domain.Constants;
using Domain.Models.Entities;
using System;
using System.Web;

namespace Utilities
{
    public class QuestionUrlsAndPaths
    {
        public string GetQuestionAttachmentPath(Attachment attachment, Guid questionID)
        {
            return string.Format(StorageValues.QUESTION_ATTACHMENT_PATH_PLACE_HOLDER,
                                    questionID.ToString(),
                                    attachment.ID.ToString(),
                                    attachment.Name);
        }

        public string GetQuestionAttachmentPrimaryUrl(Attachment attachment, Guid questionID)
        {
            return string.Format(StorageValues.QUESTION_ATTACHMENT_URL_PLACE_HOLDER,
                                    StorageValues.STORAGE_URL_PRIMARY,
                                    StorageValues.ATTACHMENT_CONTAINER,
                                    questionID.ToString(),
                                    attachment.ID.ToString(),
                                    HttpUtility.UrlPathEncode(attachment.Name));
        }

        public string GetQuestionAttachmentSecondaryUrl(Attachment attachment, Guid questionID)
        {
            return string.Format(StorageValues.QUESTION_ATTACHMENT_URL_PLACE_HOLDER,
                                    StorageValues.STORAGE_URL_SECONDARY,
                                    StorageValues.ATTACHMENT_CONTAINER,
                                    questionID.ToString(),
                                    attachment.ID.ToString(),
                                    HttpUtility.UrlPathEncode(attachment.Name));
        }

        public string GetQuestionDescriptionPrimaryUrl(Guid questionID)
        {
            return string.Format(StorageValues.QUESTION_DESCRIPTION_URL_PLACE_HOLDER,
                                    StorageValues.STORAGE_URL_PRIMARY,
                                    StorageValues.DESCRIPTION_CONTAINER,
                                    questionID.ToString(),
                                    StorageValues.DESCRIPTION_FILE_NAME);
        }

        public string GetQuestionDescriptionPath(Guid questionID)
        {
            return string.Format(StorageValues.QUESTION_DESCRIPTION_PATH_PLACE_HOLDER,
                                    questionID.ToString(),
                                    StorageValues.DESCRIPTION_FILE_NAME);
        }

        public string GetQuestionCommentsPrimaryUrl(Guid questionID)
        {
            return string.Format(StorageValues.QUESTION_COMMENT_URL_PLACE_HOLDER,
                                    StorageValues.STORAGE_URL_PRIMARY,
                                    StorageValues.COMMENTS_CONTAINER,
                                    questionID.ToString(),
                                    StorageValues.COMMENTS_FILE_NAME);
        }

        public string GetQuestionCommentsPath(Guid questionID)
        {
            return string.Format(StorageValues.QUESTION_COMMENT_PATH_PLACE_HOLDER,
                                    questionID.ToString(), 
                                    StorageValues.COMMENTS_FILE_NAME);
        }
    }
}
