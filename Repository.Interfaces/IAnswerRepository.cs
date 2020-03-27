using Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Interfaces
{
    public interface IAnswerRepository : IDisposable
    {
        Answer GetAnswerByID(long id);
        IQueryable<Answer> GetUserAnswers(int id);
        IQueryable<Answer> GetUserPaidAnswers(int id);
        void InsertAnswer(Answer answer);
        void UpdateAnswer(Answer answer);
        void DeleteAnswer(Answer answer);
        List<Answer> GetAllAnswerForQuestionID(Guid questionID);
        void UpdateAnswerAttachments(List<Attachment> attachments);
        void Save();
    }
}
