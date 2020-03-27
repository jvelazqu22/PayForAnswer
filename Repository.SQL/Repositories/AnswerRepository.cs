using Domain.Constants;
using Domain.Models.Entities;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Repository.SQL
{
    public class AnswerRepository : IAnswerRepository
    {
        private PfaDb context;

        public AnswerRepository()
        {
            this.context = new PfaDb();
        }

        public AnswerRepository(PfaDb context)
        {
            this.context = context;
        }

        public Answer GetAnswerByID(long id)
        {
            return context.Answers
                .Include(a => a.User)
                .Include(a => a.Question)
                .Include(a => a.Status)
                .Include(a => a.Payment)
                .Where(a => a.Id == id).FirstOrDefault();
        }

        public IQueryable<Answer> GetUserAnswers(int id)
        {
            return context.Answers
                .Include(a => a.Question)
                .Include(a => a.Status)
                .OrderByDescending(a => a.CreatedOn)
                .Where(a => a.UserId == id);
        }

        public IQueryable<Answer> GetUserPaidAnswers(int id)
        {
            return context.Answers
                .Include(a => a.Question)
                .Include(a => a.Status)
                .OrderByDescending(a => a.CreatedOn)
                .Where(a => a.UserId == id && a.StatusId == StatusValues.Paid);
        }

        public List<Answer> GetAllAnswerForQuestionID(Guid questionID)
        {
            return context.Questions.Find(questionID).Answers.ToList();
        }

        public void InsertAnswer(Answer answer)
        {
            context.Answers.Add(answer);
            context.SaveChanges();
        }

        public void UpdateAnswer(Answer answer)
        {
            context.Entry(answer).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void DeleteAnswer(Answer answer)
        {
            context.Answers.Remove(answer);
            context.SaveChanges();
        }

        public void UpdateAnswerAttachments(List<Attachment> attachments)
        {
            foreach(var attachment in attachments)
                context.Entry(attachment).State = EntityState.Modified;

            context.SaveChanges();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
