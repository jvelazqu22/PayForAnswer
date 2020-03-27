using Domain.Constants;
using Domain.Models.Entities;
using Repository.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLoad
{
    public class AnswerData
    {
        public List<Answer> InsertTestData(List<Question> questions, PfaDb context, List<Attachment> attachments)
        {
            var answers = new List<Answer>
                {
                    //Questions from user 1
                    new Answer {  QuestionId = questions[0].Id,  StatusId = StatusValues.AnswerSubmitted, UserId = 2, CreatedOn = DateTime.UtcNow }, //1
                    new Answer {  QuestionId = questions[0].Id,  StatusId = StatusValues.AnswerSubmitted, UserId = 3, CreatedOn = DateTime.UtcNow }, //2
                    new Answer {  QuestionId = questions[0].Id,  StatusId = StatusValues.AnswerSubmitted, UserId = 4, CreatedOn = DateTime.UtcNow }, //3
                    new Answer {  QuestionId = questions[1].Id,  StatusId = StatusValues.AnswerSubmitted, UserId = 2, CreatedOn = DateTime.UtcNow }, //4
                    new Answer {  QuestionId = questions[1].Id,  StatusId = StatusValues.AnswerSubmitted, UserId = 3, CreatedOn = DateTime.UtcNow }, //5
                    new Answer {  QuestionId = questions[1].Id,  StatusId = StatusValues.AnswerSubmitted, UserId = 4, CreatedOn = DateTime.UtcNow }, //6
                    //Questions from user 2
                    new Answer {  QuestionId = questions[2].Id,  StatusId = StatusValues.AnswerSubmitted, UserId = 3, CreatedOn = DateTime.UtcNow }, //7
                    new Answer {  QuestionId = questions[2].Id,  StatusId = StatusValues.AnswerSubmitted, UserId = 4, CreatedOn = DateTime.UtcNow }, //8
                    new Answer {  QuestionId = questions[2].Id,  StatusId = StatusValues.AnswerSubmitted, UserId = 1, CreatedOn = DateTime.UtcNow }, //9
                    new Answer {  QuestionId = questions[3].Id,  StatusId = StatusValues.AnswerSubmitted, UserId = 3, CreatedOn = DateTime.UtcNow }, //10
                    new Answer {  QuestionId = questions[3].Id,  StatusId = StatusValues.AnswerSubmitted, UserId = 4, CreatedOn = DateTime.UtcNow }, //11
                    new Answer {  QuestionId = questions[3].Id,  StatusId = StatusValues.AnswerSubmitted, UserId = 1, CreatedOn = DateTime.UtcNow }, //12
                    //Questions from user 3
                    new Answer {  QuestionId = questions[4].Id,  StatusId = StatusValues.AnswerSubmitted, UserId = 4, CreatedOn = DateTime.UtcNow }, //13
                    new Answer {  QuestionId = questions[4].Id,  StatusId = StatusValues.AnswerSubmitted, UserId = 1, CreatedOn = DateTime.UtcNow }, //14
                    new Answer {  QuestionId = questions[4].Id,  StatusId = StatusValues.AnswerSubmitted, UserId = 2, CreatedOn = DateTime.UtcNow }, //15
                    new Answer {  QuestionId = questions[5].Id,  StatusId = StatusValues.AnswerSubmitted, UserId = 4, CreatedOn = DateTime.UtcNow }, //16
                    new Answer {  QuestionId = questions[5].Id,  StatusId = StatusValues.AnswerSubmitted, UserId = 1, CreatedOn = DateTime.UtcNow }, //17
                    new Answer {  QuestionId = questions[5].Id,  StatusId = StatusValues.AnswerSubmitted, UserId = 2, CreatedOn = DateTime.UtcNow }, //18
                    //Questions from user 5
                    new Answer {  QuestionId = questions[6].Id,  StatusId = StatusValues.AnswerSubmitted, UserId = 1, CreatedOn = DateTime.UtcNow }, //19
                    new Answer {  QuestionId = questions[6].Id,  StatusId = StatusValues.AnswerSubmitted, UserId = 2, CreatedOn = DateTime.UtcNow }, //20
                    new Answer {  QuestionId = questions[6].Id,  StatusId = StatusValues.AnswerSubmitted, UserId = 3, CreatedOn = DateTime.UtcNow }, //21
                    new Answer {  QuestionId = questions[7].Id,  StatusId = StatusValues.AnswerSubmitted, UserId = 1, CreatedOn = DateTime.UtcNow }, //22
                    new Answer {  QuestionId = questions[7].Id,  StatusId = StatusValues.AnswerSubmitted, UserId = 2, CreatedOn = DateTime.UtcNow }, //23
                    new Answer {  QuestionId = questions[7].Id,  StatusId = StatusValues.AnswerSubmitted, UserId = 3, CreatedOn = DateTime.UtcNow } //24
                };

            foreach (var answer in answers)
                attachments.ForEach(a => answer.Attachments.Add( new Attachment() { Name = a.Name, SizeInBytes = a.SizeInBytes } ));

            answers.ForEach(a => context.Answers.Add(a));
            context.SaveChanges();

            return answers;
        }
    }
}
