using Domain.Models.Entities;
using Domain.Models.Entities.Identity;
using Repository.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;

namespace Repository.SQL
{
    public class FeedbackRepository : IFeedbackRepository, IDisposable
    {
        private PfaDb context;
        private bool disposed = false;

        public FeedbackRepository()
        {
            this.context = new PfaDb();
        }

        public FeedbackRepository(PfaDb context)
        {
            this.context = context;
        }

        public void InsertFeedback(Feedback feedback)
        {
            context.Feedback.Add(feedback);
            context.SaveChanges();
        }

        public void UpdateFeedback(Feedback feedback)
        {
            context.Entry(feedback).State = EntityState.Modified;
            context.SaveChanges();
        }

        public IQueryable<Feedback> GetFeedbackList()
        {
            return context.Feedback.OrderByDescending(f => f.CreatedOn);
        }

        public void Save()
        {
            context.SaveChanges();
        }

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
