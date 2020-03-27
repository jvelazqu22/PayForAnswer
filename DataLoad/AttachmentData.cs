using Domain.Constants;
using Domain.Models.Entities;
using Repository.Blob;
using Repository.SQL;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace DataLoad
{
    public class AttachmentData
    {
        string path = @"C:\Code\PayForAnswer\Development\Repository.Blob\TestFiles\";
        public void AddAttachmentsToQuestions(List<Question> questions, PfaDb context, BlobRepository blobRepository)
        {
            Parallel.ForEach<Question>(questions, question =>
            {
                foreach (Attachment attachment in question.Attachments)
                {
                    var fileNameWithPath = path + attachment.Name;
                    var attachmentBloblPath = string.Format(StorageValues.QUESTION_ATTACHMENT_PATH_PLACE_HOLDER, question.Id,
                        attachment.ID, attachment.Name);
                    attachment.PrimaryUri = string.Format(StorageValues.QUESTION_ATTACHMENT_URL_PLACE_HOLDER, StorageValues.STORAGE_URL_PRIMARY,
                                                    StorageValues.ATTACHMENT_CONTAINER, question.Id, attachment.ID, HttpUtility.UrlPathEncode(attachment.Name));
                    blobRepository.UploadFileToBlob(attachmentBloblPath, fileNameWithPath, StorageValues.ATTACHMENT_CONTAINER);
                }
            });
            context.SaveChanges();
        }

        public void AddAttachmentsToAnswers(List<Answer> answers, PfaDb context, BlobRepository blobRepository)
        {
            Parallel.ForEach<Answer>(answers, answer =>
            {
                foreach (Attachment attachment in answer.Attachments)
                {
                    var fileNameWithPath = path + attachment.Name;
                    var attachmentBloblPath = string.Format(StorageValues.ANSWER_ATTACHMENT_PATH_PLACE_HOLDER, answer.QuestionId, answer.Id,
                                                            attachment.ID, attachment.Name);
                    attachment.PrimaryUri = string.Format(StorageValues.ANSWER_ATTACHMENT_URL_PLACE_HOLDER, StorageValues.STORAGE_URL_PRIMARY,
                                                    StorageValues.ATTACHMENT_CONTAINER, answer.QuestionId, answer.Id, attachment.ID,
                                                    HttpUtility.UrlPathEncode(attachment.Name));
                    blobRepository.UploadFileToBlob(attachmentBloblPath, fileNameWithPath, StorageValues.ATTACHMENT_CONTAINER);
                }
            });

            context.SaveChanges();
        }
    }
}
