using BusinessRules;
using Domain.Models.Entities;
using Domain.Models.Entities.Identity;
using Repository.Blob;
using Repository.Interfaces;
using Repository.SQL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLoad
{
    public class Loader
    {
        static void Main(string[] args)
        {
            var context = new PfaDb();

            Console.WriteLine("Loading status...");
            new StatusData().InsertDefaultStatus(context);

            //Don't run this in a production enviroment.
            new Loader().AddTestData(context);
        }

        public void Test()
        {
            IPaymentRepository paymentRepository = new PaymentRepository();
            EmailBR emailBR = new EmailBR(); 
            List<ApplicationUser> campaignManagers = paymentRepository.GetAllCampaignManagers();
            emailBR.NotifyAllCampaignManagers(campaignManagers);
        }

        public void AddTestData(PfaDb context)
        {
            SubjectData sd = new SubjectData();
            var blobRepository = new BlobRepository();
            var commentData = new CommentData();
            var attachmentData = new AttachmentData();
            var questionData = new QuestionData();
            var answerData = new AnswerData();
            var descriptionData = new DescriptionData();

            Console.WriteLine("Loading roles and users...");
            new RolesAndUsers(context).AddDefaultData();

            Console.WriteLine("Loading subjects...");
            var subjects = sd.InsertTestSubjects(context);

            Console.WriteLine("Adding user to subjects...");
            var user = GetUserToAddToSubjects(context, "jvelazquez22h");
            sd.AddUserToSubjects(user, subjects, context);

            Console.WriteLine("Loading questions...");
            var questionPaymentDetails = new QuestionPaymentDetailData().GetTestDataToBeAdded(context);
            var questions = questionData.InsertTestQuestions(questionPaymentDetails, context, GetAttachmentList());

            Console.WriteLine("Loading question descriptions...");
            descriptionData.AddDescriptionToQuestions(blobRepository, questions, context);

            Console.WriteLine("Loading question comments...");
            var comments = commentData.GetTestCommentsToBeAdded();
            commentData.AddCommentsToQuestions(blobRepository, questions, comments, context);

            Console.WriteLine("Loading quesitons attachments...");
            attachmentData.AddAttachmentsToQuestions(questions, context, blobRepository);

            Console.WriteLine("Loading subjects to questions...");
            questionData.AddSubjectsToQuestions(questions, subjects, context);

            Console.WriteLine("Loading answers to questions...");
            var answers = answerData.InsertTestData(questions, context, GetAttachmentList());

            Console.WriteLine("Loading description to answers...");
            descriptionData.AddDescriptionToAnswers(blobRepository, answers, context);

            Console.WriteLine("Loading comments to answer...");
            commentData.AddCommentsToAnswers(blobRepository, answers, comments, context);

            Console.WriteLine("Loading attachments to answers...");
            attachmentData.AddAttachmentsToAnswers(answers, context, blobRepository);
        }

        public ApplicationUser GetUserToAddToSubjects(PfaDb context, string userName)
        {
            return context.Users.Where(u => u.UserName == userName).FirstOrDefault();
        }

        public List<Attachment> GetAttachmentList()
        {
            return new List<Attachment>() 
            { 
                new Attachment() { Name = "205730-GCU-Transfer Sectrion 7_23_13.pdf", SizeInBytes = 271468 },
                new Attachment() { Name = "ARB ROM.docx" , SizeInBytes = 128756 }
            };
        }
    }
}
