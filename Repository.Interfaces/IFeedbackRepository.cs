using Domain.Models.Entities;
using Domain.Models.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Interfaces
{
    public interface IFeedbackRepository : IDisposable
    {
        void InsertFeedback(Feedback feedback);
        void UpdateFeedback(Feedback feedback);
        IQueryable<Feedback> GetFeedbackList();

        void Save();
    }
}
