using Domain.Models.Entities;
using Domain.Models.Entities.Identity;
using Repository.SQL;
using System.Collections.Generic;

namespace DataLoad
{
    public class SubjectData
    {
        public List<Subject> InsertTestSubjects(PfaDb context)
        {
            var subjects = new List<Subject>
                {
                    new Subject { SubjectName = "math", Users = new List<ApplicationUser>() }, //1
                    new Subject { SubjectName = "square-root", Users = new List<ApplicationUser>() }, //2
                    new Subject { SubjectName = "android", Users = new List<ApplicationUser>() }, //3
                    new Subject { SubjectName = "actionbarsherlock", Users = new List<ApplicationUser>() }, //4
                    new Subject { SubjectName = "android-webview", Users = new List<ApplicationUser>() }, //5
                    new Subject { SubjectName = "adroid-view-pager", Users = new List<ApplicationUser>() }, //6
                    new Subject { SubjectName = "menuitem", Users = new List<ApplicationUser>() }, //7
                    new Subject { SubjectName = "physics", Users = new List<ApplicationUser>() }, //8
                    new Subject { SubjectName = "quantum-gravity", Users = new List<ApplicationUser>() }, //9
                    new Subject { SubjectName = "computer-science", Users = new List<ApplicationUser>() }, //10
                    new Subject { SubjectName = "hardware", Users = new List<ApplicationUser>() }, //1
                    new Subject { SubjectName = "monitor", Users = new List<ApplicationUser>() }, //12
                    new Subject { SubjectName = "helicopter", Users = new List<ApplicationUser>() }, //13
                    new Subject { SubjectName = "blade", Users = new List<ApplicationUser>() }, //14
                    new Subject { SubjectName = "home", Users = new List<ApplicationUser>() }, //15
                    new Subject { SubjectName = "toilet", Users = new List<ApplicationUser>() }, //16
                    new Subject { SubjectName = "pipes", Users = new List<ApplicationUser>() }, //17
                    new Subject { SubjectName = "plug", Users = new List<ApplicationUser>() }, //18
                    new Subject { SubjectName = "flood", Users = new List<ApplicationUser>() } //19
                };
            subjects.ForEach(s => context.Subjects.Add(s));
            context.SaveChanges();

            return subjects;
        }

        public void AddUserToSubjects(ApplicationUser user, List<Subject> subjects, PfaDb context)
        {
            subjects[0].Users.Add(user); //Math
            subjects[3].Users.Add(user); //actionbarsherlock
            subjects[8].Users.Add(user); //quantum-gravity
            subjects[18].Users.Add(user); // Flood
            context.SaveChanges();
        }
    }
}
