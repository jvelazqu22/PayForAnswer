using Domain.Constants;
using Domain.Models.Entities;
using Domain.Models.Entities.Identity;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Repository.SQL
{
    public class QuestionSubjectRepository : IQuestionSubjectRepository
    {
        private PfaDb context;
        private bool disposed = false;

        public QuestionSubjectRepository() 
        {
            context = new PfaDb();
        }

        public Subject GetSubjectBySubjectId(int subjectId)
        {
            return context.Subjects.Find(subjectId);
        }

        public Subject GetSubjectBySubjectName(string subjectName)
        {
            return context.Subjects.Where(s => s.SubjectName.ToLower() == subjectName.ToLower()).FirstOrDefault();
        }

        public void InsertQuestion(Question question)
        {
            context.Questions.Add(question);
        }

        public void UpdateQuestion(Question question)
        {
            context.Entry(question).State = EntityState.Modified;
            context.SaveChanges();
        }

        public ApplicationUser GetUserById(int userId)
        {
            return context.Users
                .Include(u => u.Subjects)
                .Where(u => u.Id == userId).FirstOrDefault();
        }

        public Subject GetSubjectByName(string searchTerm)
        {
            return context.Subjects.SingleOrDefault(s => s.SubjectName.Equals(searchTerm));
        }

        public void InsertSubject(Subject subject)
        {
            context.Subjects.Add(subject);
            context.SaveChanges();
        }

        public IQueryable<Subject> GetUserSubjects(int userId)
        {
            return context.Subjects.Where(s => s.Users.Select(u => u.Id == userId).Contains(true));
        }

        public IQueryable<Subject> GetUnRegUserSubjects(List<string> tempSubjectList)
        {
            return  context.Subjects.Where(s => tempSubjectList.Contains(s.SubjectName));
        }

        public IQueryable<Question> GetOpenQuestionsByUserSubjects(IQueryable<Subject> userSubjects)
        {
            return context.Questions
                .Include(q => q.Subjects)
                .Include(q => q.Answers)
                .Include(q => q.Status)
                .Where(q => q.StatusId == StatusValues.PayPalRedirectConfirmed || q.StatusId == StatusValues.PayPalIPNNotifyConfirmed)
                .Where(q => !q.Answers.Select(a => a.StatusId == StatusValues.Accepted || a.StatusId == StatusValues.Paid).Contains(true))
                .Where(q => q.Subjects.Intersect(userSubjects).Any())
                .OrderByDescending(q => q.Amount);
        }

        public void UpdateUser(ApplicationUser user)
        {
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
