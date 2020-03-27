using Domain.Models.Entities;
using Domain.Models.Entities.Identity;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Repository.SQL
{
    public class SubjectRepository : ISubjectRepository, IDisposable
    {
        private PfaDb context;
        private bool disposed = false;

        public SubjectRepository()
        {
            this.context = new PfaDb();
        }

        public SubjectRepository(PfaDb context)
        {
            this.context = context;
        }

        public void AddSubject(Subject subject)
        {
            context.Subjects.Add(subject);
        }

        public Subject GetSubjectByName(string subjectName)
        {
            return context.Subjects.SingleOrDefault(s => s.SubjectName.Equals(subjectName));
        }

        public IEnumerable<ApplicationUser> GetUsersBySubject(long id)
        {
            return context.Subjects.Find(id).Users;
        }

        public ApplicationUser GetUserByID(int userID)
        {
            return context.Users
                    .Include(u => u.Subjects)
                    .Where(u => u.Id == userID).FirstOrDefault();
        }

        public void RemoveUserSubjects(ApplicationUser user, List<Subject> subjects)
        {
            foreach(var subject in subjects)
                user.Subjects.Remove(subject);
            context.Entry(user).State = EntityState.Modified;
            context.SaveChanges();
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
