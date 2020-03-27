using Domain.Constants;
using Domain.Models;
using Domain.Models.Entities;
using Domain.Models.Helper;
using Domain.Models.ViewModel;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Utilities;

namespace BusinessRules
{
    public class BlobBR
    {
        public void AddAttachmentsToQuestionModel(UploadFile questionViewModel, Question questionModel)
        {
            if(questionViewModel.Files != null)
                foreach (HttpPostedFileBase item in questionViewModel.Files)
                    if (item != null && Array.Exists(questionViewModel.FilesToBeUploaded.Split(','), s => s.Equals(item.FileName)))
                        if (item.ContentLength > 0)
                            questionModel.Attachments.Add(new Attachment() { Name = item.FileName, SizeInBytes = item.ContentLength });
        }

        public void AddAttachmentsToAnswerModel(NewAnswerViewModel answerViewModel, Answer answerModel)
        {
            if (answerViewModel.Files != null)
                foreach (HttpPostedFileBase item in answerViewModel.Files)
                if (item != null && Array.Exists(answerViewModel.FilesToBeUploaded.Split(','), s => s.Equals(item.FileName)))
                    if (item.ContentLength > 0)
                        answerModel.Attachments.Add(new Attachment() { Name = item.FileName, SizeInBytes = item.ContentLength });
        }

        public void UploadFiles(CreateQuestionViewModel questionViewModel, Question questionModel, IBlobRepository blobRepository)
        {
            List<string> attachmentNames = questionModel.Attachments.Select(a => a.Name).ToList();
            Parallel.ForEach<HttpPostedFileBase>(questionViewModel.Files, item =>
            {
                if (item != null && attachmentNames.Contains(item.FileName))
                {
                    Attachment attachment = questionModel.Attachments.Where(a => a.Name == item.FileName).FirstOrDefault();
                    UpdateQuestionAttachment(attachment, questionModel.Id);
                    blobRepository.UploadAStreamToABlob(item.InputStream, attachment.Path, StorageValues.ATTACHMENT_CONTAINER);
                }
            });

            QuestionUrlsAndPaths urlAndPaths = new QuestionUrlsAndPaths();
            string descriptionBlobPath = urlAndPaths.GetQuestionDescriptionPath(questionModel.Id);
            string commentBlobPath = urlAndPaths.GetQuestionCommentsPath(questionModel.Id);
            blobRepository.AddUpdateHtmlFileContent(descriptionBlobPath, questionViewModel.Description, StorageValues.DESCRIPTION_CONTAINER);
            blobRepository.AddUpdateHtmlFileContent(commentBlobPath, Html.COMMENTS_DEFAULT_VALUE, StorageValues.COMMENTS_CONTAINER);
            questionModel.DescriptionUrl = urlAndPaths.GetQuestionDescriptionPrimaryUrl(questionModel.Id);
            questionModel.CommentsUrl = urlAndPaths.GetQuestionCommentsPrimaryUrl(questionModel.Id);
        }

        public void UploadFiles(UpdateQuestionViewModel questionViewModel, Question questionModel, IBlobRepository blobRepository)
        {
            List<string> attachmentNames = questionModel.Attachments.Select(a => a.Name).ToList();
            if (questionViewModel.Files != null)
            {
                Parallel.ForEach<HttpPostedFileBase>(questionViewModel.Files, item =>
                {
                    if (item != null && attachmentNames.Contains(item.FileName))
                    {
                        Attachment attachment = questionModel.Attachments.Where(a => a.Name == item.FileName).FirstOrDefault();
                        UpdateQuestionAttachment(attachment, questionModel.Id);
                        blobRepository.UploadAStreamToABlob(item.InputStream, attachment.Path, StorageValues.ATTACHMENT_CONTAINER);
                    }
                });
            }

            QuestionUrlsAndPaths urlAndPaths = new QuestionUrlsAndPaths();
            string descriptionBlobPath = urlAndPaths.GetQuestionDescriptionPath(questionModel.Id);
            blobRepository.AddUpdateHtmlFileContent(descriptionBlobPath, questionViewModel.Description, StorageValues.DESCRIPTION_CONTAINER);
            questionModel.DescriptionUrl = urlAndPaths.GetQuestionDescriptionPrimaryUrl(questionModel.Id);
        }

        public void UploadFiles(NewAnswerViewModel answerViewModel, Answer answerModel, IBlobRepository blobRepository)
        {
            List<string> attachmentNames = answerModel.Attachments.Select(a => a.Name).ToList();

            if (answerViewModel.Files != null)
            {
                Parallel.ForEach<HttpPostedFileBase>(answerViewModel.Files, item =>
                {
                    if (item != null && attachmentNames.Contains(item.FileName))
                    {
                        Attachment attachment = answerModel.Attachments.Where(a => a.Name == item.FileName).FirstOrDefault();
                        UpdateAnswerAttachment(attachment, (Guid)answerModel.QuestionId, answerModel.Id);
                        blobRepository.UploadAStreamToABlob(item.InputStream, attachment.Path, StorageValues.ATTACHMENT_CONTAINER);
                    }
                });
            }

            AnswerUrlsAndPaths urlAndPaths = new AnswerUrlsAndPaths();
            string descriptionBlobPath = urlAndPaths.GetAnswerDescriptionPath((Guid)answerModel.QuestionId, answerModel.Id);
            string commentBlobPath = urlAndPaths.GetAnswerCommentsPath((Guid)answerModel.QuestionId, answerModel.Id);
            blobRepository.AddUpdateHtmlFileContent(descriptionBlobPath, answerViewModel.NewPostedAnswer, StorageValues.DESCRIPTION_CONTAINER);
            blobRepository.AddUpdateHtmlFileContent(commentBlobPath, Html.COMMENTS_DEFAULT_VALUE, StorageValues.COMMENTS_CONTAINER);
            answerModel.DescriptionUrl = urlAndPaths.GetAnswerDescriptionPrimaryUrl((Guid)answerModel.QuestionId, answerModel.Id);
            answerModel.CommentsUrl = urlAndPaths.GetAnswerCommentsPrimaryUrl((Guid)answerModel.QuestionId, answerModel.Id);
        }

        public void UpdateQuestionAttachment(Attachment attachment, Guid questionID)
        {
            QuestionUrlsAndPaths urlAndPaths = new QuestionUrlsAndPaths();
            attachment.Path = urlAndPaths.GetQuestionAttachmentPath(attachment, questionID);
            attachment.PrimaryUri = urlAndPaths.GetQuestionAttachmentPrimaryUrl(attachment, questionID);
            attachment.SecondaryUri = urlAndPaths.GetQuestionAttachmentSecondaryUrl(attachment, questionID);
            attachment.Container = StorageValues.ATTACHMENT_CONTAINER;
        }

        public void UpdateAnswerAttachment(Attachment attachment, Guid questionID, long answerID)
        {
            AnswerUrlsAndPaths urlAndPaths = new AnswerUrlsAndPaths();
            attachment.Path = urlAndPaths.GetAnswerAttachmentPath(attachment, questionID, answerID);
            attachment.PrimaryUri = urlAndPaths.GetAnswerAttachmentPrimaryUrl(attachment, questionID, answerID);
            attachment.SecondaryUri = urlAndPaths.GetAnswerAttachmentSecondaryUrl(attachment, questionID, answerID);
            attachment.Container = StorageValues.ATTACHMENT_CONTAINER;
        }

        public void DeleteUnacceptedQuestionAnswersAttachments(Answer acceptedAnswer, IAnswerRepository answerRepository, IBlobRepository blobRepository)
        {
            List<Answer> answerList = answerRepository.GetAllAnswerForQuestionID(acceptedAnswer.QuestionId);
            Answer answerToRemove = answerList.Where(a => a.Id == acceptedAnswer.Id).FirstOrDefault();
            if(answerToRemove != null)
            {
                answerList.Remove(answerToRemove);

                List<Attachment> answerAttachments = new List<Attachment>();
                List<string> blobNames = new List<string>();
                foreach (var answer in answerList)
                    foreach (var attachment in answer.Attachments)
                    {
                        blobNames.Add(new AnswerUrlsAndPaths().GetAnswerAttachmentPath(attachment, answer.QuestionId, answer.Id));
                        attachment.PrimaryUri = StorageValues.DELETED_HTML_URL;
                        answerAttachments.Add(attachment);
                    }
                answerRepository.UpdateAnswerAttachments(answerAttachments);

                Parallel.ForEach<string>(blobNames, blobName =>
                {
                    blobRepository.DeleteAttachment(blobName);
                });
            }
        }

        public void DeleteQuestionAttachment(Guid questionID, Attachment attachment, IBlobRepository blobRepository)
        {
            string blobName = new QuestionUrlsAndPaths().GetQuestionAttachmentPath(attachment, questionID);
            blobRepository.DeleteAttachment(blobName);
        }
    }
}
