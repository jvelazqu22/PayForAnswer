using Domain.Models.Entities;
using System;
using System.Linq;

namespace Repository.Interfaces
{
    public interface IQuestionRepository : IDisposable
    {
        Question GetQuestionByID(Guid id);
        void InsertQuestion(Question question);
        void UpdateQuestion(Question question);
        void DeleteAttachment(Attachment attachment);
        IQueryable<Question> GetUserAcceptedQuestions(int userId);
        IQueryable<Question> GetUserOpenQuestions(int userId);
        IQueryable<Question> GetTopQuestions(int size);
        IQueryable<Question> GetUserQuestions(int userId);
        IQueryable<Question> GetAllOpenQuestions();
        IQueryable<Question> GetAllPaidOrAcceptedQuestions();

        void Save();
    }
}
