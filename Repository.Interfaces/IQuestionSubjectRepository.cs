using Domain.Models.Entities;
using Domain.Models.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Interfaces
{
    public interface IQuestionSubjectRepository : IDisposable
    {
        Subject GetSubjectBySubjectId(int subjectId);
        Subject GetSubjectBySubjectName(string subjectName);
        void InsertQuestion(Question question);
        void UpdateQuestion(Question question);
        ApplicationUser GetUserById(int userId);
        Subject GetSubjectByName(string subjectName);
        void InsertSubject(Subject subject);
        IQueryable<Subject> GetUserSubjects(int userId);
        IQueryable<Subject> GetUnRegUserSubjects(List<string> tempSubjectList);
        IQueryable<Question> GetOpenQuestionsByUserSubjects(IQueryable<Subject> userSubjects);
        void UpdateUser(ApplicationUser user);
        void Save();
    }
}
