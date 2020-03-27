using Domain.Constants;
using Domain.Models.Entities;
using Repository.SQL;
using System;
using System.Collections.Generic;

namespace DataLoad
{
    public class QuestionData
    {
        public List<Question> InsertTestQuestions(List<QuestionPaymentDetail> questionPaymentDetails, PfaDb context, List<Attachment> attachments)
        {
            var questions = new List<Question>
                {
                    new Question 
                    { 
                        Title = "What is the square root of -1?", 
                        Amount = 10, 
                        StatusId = StatusValues.PayPalIPNNotifyConfirmed,
                        UserId = 1,
                        CreatedOn = DateTime.UtcNow,
                        UpdatedOn = DateTime.UtcNow,
                        Subjects = new List<Subject>(),
                        QuestionPaymentDetails = new List<QuestionPaymentDetail> { questionPaymentDetails[0] }
                    }, //1
                    new Question 
                    { 
                        Title = "ViewPager, Menu-Items and WebViews", 
                        Amount = 10, 
                        StatusId = StatusValues.PayPalRedirectConfirmed,
                        UserId = 1,
                        CreatedOn = DateTime.UtcNow,
                        UpdatedOn = DateTime.UtcNow,
                        Subjects = new List<Subject>(),
                        QuestionPaymentDetails = new List<QuestionPaymentDetail> { questionPaymentDetails[1] }
                    }, //2
                    new Question 
                    { 
                        Title = "Vacuum catastophe", 
                        Amount = 10, 
                        StatusId = StatusValues.PayPalIPNNotifyConfirmed,
                        UserId = 2,
                        CreatedOn = DateTime.UtcNow,
                        UpdatedOn = DateTime.UtcNow,
                        Subjects = new List<Subject>(),
                        QuestionPaymentDetails = new List<QuestionPaymentDetail> { questionPaymentDetails[2] },
                    }, //3
                    new Question 
                    { 
                        Title = "Quantum gravity", 
                        Amount = 10, 
                        StatusId = StatusValues.PayPalRedirectConfirmed,
                        UserId = 2,
                        CreatedOn = DateTime.UtcNow,
                        UpdatedOn = DateTime.UtcNow,
                        Subjects = new List<Subject>(),
                        QuestionPaymentDetails = new List<QuestionPaymentDetail> { questionPaymentDetails[3] },
                    }, //4
                    new Question 
                    { 
                        Title = "P versus NP problem", 
                        Amount = 10, 
                        StatusId = StatusValues.PayPalIPNNotifyConfirmed,
                        UserId = 3,
                        CreatedOn = DateTime.UtcNow,
                        UpdatedOn = DateTime.UtcNow,
                        Subjects = new List<Subject>(),
                        QuestionPaymentDetails = new List<QuestionPaymentDetail> { questionPaymentDetails[4] },
                    }, //5
                    new Question 
                    { 
                        Title = "How to fix a monitor with a blue...", 
                        Amount = 6, 
                        StatusId = StatusValues.PayPalRedirectConfirmed,
                        UserId = 3,
                        CreatedOn = DateTime.UtcNow,
                        UpdatedOn = DateTime.UtcNow,
                        Subjects = new List<Subject>(),
                        QuestionPaymentDetails = new List<QuestionPaymentDetail> { questionPaymentDetails[5] },
                    }, //6
                    new Question 
                    { 
                        Title = "How to replace a helicopter ...", 
                        Amount = 7, 
                        StatusId = StatusValues.PayPalIPNNotifyConfirmed,
                        UserId = 4,
                        CreatedOn = DateTime.UtcNow,
                        UpdatedOn = DateTime.UtcNow,
                        Subjects = new List<Subject>(),
                        QuestionPaymentDetails = new List<QuestionPaymentDetail> { questionPaymentDetails[6] },
                    }, //7
                    new Question 
                    { 
                        Title = "Flood in the 1st floor of my ...", 
                        Amount = 8, 
                        StatusId = StatusValues.PayPalRedirectConfirmed,
                        UserId = 4,
                        CreatedOn = DateTime.UtcNow,
                        UpdatedOn = DateTime.UtcNow,
                        Subjects = new List<Subject>(),
                        QuestionPaymentDetails = new List<QuestionPaymentDetail> { questionPaymentDetails[7] },
                    } //8
                };
            foreach (var question in questions)
                attachments.ForEach(a => question.Attachments.Add(new Attachment() { Name = a.Name, SizeInBytes = a.SizeInBytes }));

            questions.ForEach(q => context.Questions.Add(q));
            context.SaveChanges();

            return questions;
        }

        public void AddSubjectsToQuestions(List<Question> questions, List<Subject> subjects, PfaDb context)
        {
            questions[0].Subjects.Add(subjects[0]);
            questions[0].Subjects.Add(subjects[1]);
            questions[1].Subjects.Add(subjects[2]);
            questions[1].Subjects.Add(subjects[3]);
            questions[1].Subjects.Add(subjects[4]);
            questions[1].Subjects.Add(subjects[5]);
            questions[1].Subjects.Add(subjects[6]);
            questions[2].Subjects.Add(subjects[7]);
            questions[2].Subjects.Add(subjects[8]);
            questions[3].Subjects.Add(subjects[7]);
            questions[3].Subjects.Add(subjects[8]);
            questions[4].Subjects.Add(subjects[0]);
            questions[4].Subjects.Add(subjects[9]);
            questions[5].Subjects.Add(subjects[10]);
            questions[5].Subjects.Add(subjects[11]);
            questions[6].Subjects.Add(subjects[12]);
            questions[6].Subjects.Add(subjects[13]);
            questions[7].Subjects.Add(subjects[14]);
            questions[7].Subjects.Add(subjects[15]);
            questions[7].Subjects.Add(subjects[16]);
            questions[7].Subjects.Add(subjects[17]);
            questions[7].Subjects.Add(subjects[18]);

            context.SaveChanges();
        }
    }
}
