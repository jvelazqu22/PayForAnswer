using Domain.Models.Entities;
using Domain.Models.Entities.Identity;
using System;
using System.Collections.Generic;

namespace Repository.Interfaces
{
    public interface ISubjectRepository : IDisposable
    {
        void AddSubject(Subject subject);
        Subject GetSubjectByName(string subjectName);
        IEnumerable<ApplicationUser> GetUsersBySubject(long id);
        ApplicationUser GetUserByID(int userID);
        void RemoveUserSubjects(ApplicationUser user, List<Subject> subjects);
        void Save();
    }
}
