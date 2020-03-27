using Domain.Models.Entities;
using Repository.Interfaces;
using System;
using System.Linq;
using System.Data.Entity;
using Domain.Constants;

namespace Repository.SQL
{
    public class QuestionRepository : IQuestionRepository
    {
        private PfaDb context;

        public QuestionRepository() { this.context = new PfaDb(); }

        public QuestionRepository(PfaDb context) { this.context = context; }

        public Question GetQuestionByID(Guid id) { return context.Questions.Find(id); }

        public void InsertQuestion(Question question) { context.Questions.Add(question); }

        public void UpdateQuestion(Question question)
        {
            context.Entry(question).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void DeleteAttachment(Attachment attachment)
        {
            context.Attachments.Remove(attachment);
            context.SaveChanges();
        }

        public IQueryable<Question> GetUserAcceptedQuestions(int userId)
        {
            return context.Questions
                .OrderByDescending(q => q.CreatedOn)
                .Include(q => q.Status)
                .Where(q => q.StatusId == StatusValues.Accepted || q.StatusId == StatusValues.AcceptedByRequester || q.StatusId == StatusValues.Paid)
                .Where(q => q.UserId == userId);
        }

        public IQueryable<Question> GetUserOpenQuestions(int userId)
        {
            return context.Questions
                .OrderByDescending(q => q.CreatedOn)
                .Include(q => q.Status)
                .Where(q => q.StatusId == StatusValues.PayPalRedirectConfirmed || q.StatusId == StatusValues.PayPalIPNNotifyConfirmed)
                .Where(q => !q.Answers.Select(a => a.StatusId == StatusValues.Accepted || a.StatusId == StatusValues.Paid).Contains(true))
                .Where(q => q.UserId == userId);
        }

        public IQueryable<Question> GetUserQuestions(int userId)
        {
            return context.Questions
                .OrderByDescending(q => q.CreatedOn)
                .Include(q => q.Status)
                .Where(q => q.UserId == userId);
        }

        public IQueryable<Question> GetAllOpenQuestions()
        {
            return context.Questions
                .OrderByDescending(q => q.Amount)
                .Where(q => q.StatusId == StatusValues.PayPalRedirectConfirmed || q.StatusId == StatusValues.PayPalIPNNotifyConfirmed)
                .Where(q => !q.Answers.Select(a => a.StatusId == StatusValues.Accepted || a.StatusId == StatusValues.Paid).Contains(true))
                .Include(q => q.Status);
        }

        public IQueryable<Question> GetTopQuestions(int size)
        {
            return context.Questions
                .OrderByDescending(q => q.Amount)
                .Where(q => q.StatusId == StatusValues.PayPalRedirectConfirmed || q.StatusId == StatusValues.PayPalIPNNotifyConfirmed)
                .Where(q => !q.Answers.Select(a => a.StatusId == StatusValues.Accepted || a.StatusId == StatusValues.Paid).Contains(true))
                .Include(q => q.Status)
                .Take(size);
        }

        public IQueryable<Question> GetAllPaidOrAcceptedQuestions()
        {
            return context.Questions
                .OrderByDescending(q => q.Amount)
                .Where(q => q.StatusId == StatusValues.Accepted || q.StatusId == StatusValues.Paid || q.StatusId == StatusValues.AcceptedByRequester)
                .Include(q => q.Status);
        }

        public void Save() { context.SaveChanges(); }

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
