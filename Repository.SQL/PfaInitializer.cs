using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Security;
using Domain.Models;
using WebMatrix.WebData;
using System.Linq;

namespace DAL
{
    public class PfaInitializer : DropCreateDatabaseIfModelChanges<PfaDb>
    {
        protected override void Seed(DAL.PfaDb context)
        {
            var statusCategories = new List<StatusCategory>
                {
                    new StatusCategory { Category = "Question"}, //1
                    new StatusCategory { Category = "Answer"} //2
                };
            statusCategories.ForEach(sc => context.StatusCategories.Add(sc));
            context.SaveChanges();

            var status = new List<Status>
                {
                    new Status { Name = "Open", StatusCategoryId = 1}, //1
                    new Status { Name = "Paid", StatusCategoryId = 1}, //2
                    new Status { Name = "Locked", StatusCategoryId = 1}, //3
                    new Status { Name = "Hidden", StatusCategoryId = 1}, //4
                    new Status { Name = "Unrelated", StatusCategoryId = 2}, //5
                    new Status { Name = "Accepted", StatusCategoryId = 2}, //6
                    new Status { Name = "Submitted", StatusCategoryId = 2} //7
                };
            status.ForEach(s => context.Status.Add(s));
            context.SaveChanges();

            var subjects = new List<Subject>
                {
                    new Subject { SubjectName = "Math"}, //1
                    new Subject { SubjectName = "square-root"}, //2
                    new Subject { SubjectName = "Android"}, //3
                    new Subject { SubjectName = "actionbarsherlock"}, //4
                    new Subject { SubjectName = "android-webview"}, //5
                    new Subject { SubjectName = "adroid-view-pager"}, //6
                    new Subject { SubjectName = "menuitem"}, //7
                    new Subject { SubjectName = "Physics"}, //8
                    new Subject { SubjectName = "quantum-gravity"}, //9
                    new Subject { SubjectName = "computer-science"}, //10
                    new Subject { SubjectName = "Hardware"}, //1
                    new Subject { SubjectName = "Monitor"}, //12
                    new Subject { SubjectName = "Helicopter"}, //13
                    new Subject { SubjectName = "Blade"}, //14
                    new Subject { SubjectName = "Home"}, //15
                    new Subject { SubjectName = "toilet"}, //16
                    new Subject { SubjectName = "Pipes"}, //17
                    new Subject { SubjectName = "Plug"}, //18
                    new Subject { SubjectName = "Flood"} //19
                };
            subjects.ForEach(s => context.Subjects.Add(s));
            context.SaveChanges();

            CreateRoleAndUsers();

            var questions = new List<Question>
                {
                    new Question 
                    { 
                        Title = "What is the square root of -1?", 
                        Description = "What is the square root of -1?", 
                        Amount = 10, 
                        StatusId = 1,
                        UserId = 2
                    }, //1
                    new Question 
                    { 
                        Title = "ViewPager and Menu-Items and WebViews", 
                        Description = "Is there a way to create my Menu Items for the actionbar in my MainActivity where my ViewPager is created so that way im only creating my webview items (back, forward and refresh buttons) once. Then inside my onOptions.... Then specify the menu items to be able to handle my webview.  All my fragments in my ViewPager are webviews, i just want 3 constant Actionbar items to control the webview's that is being shown. Instead of recreating them each time my viewpager loads another fragment. Any Idea's???", 
                        Amount = 20, 
                        StatusId = 1,
                        UserId = 2
                    }, //2
                    new Question 
                    { 
                        Title = "Vacuum catastophe", 
                        Description = "Why does the predicted mass of the quantum vacuum have little effect on the expansion of the universe?", 
                        Amount = 1000, 
                        StatusId = 1,
                        UserId = 3
                    }, //3
                    new Question 
                    { 
                        Title = "Quantum gravity", 
                        Description = "Can quantum mechanics and general relativity be realized as a fully consistent theory (perhaps as a quantum field theory)?[7] Is spacetime fundamentally continuous or discrete? Would a consistent theory involve a force mediated by a hypothetical graviton, or be a product of a discrete structure of spacetime itself (as in loop quantum gravity)? Are there deviations from the predictions of general relativity at very small or very large scales or in other extreme circumstances that flow from a quantum gravity theory?", 
                        Amount = 1000, 
                        StatusId = 1,
                        UserId = 3
                    }, //4
                    new Question 
                    { 
                        Title = "P versus NP problem", 
                        Description = "The P versus NP problem is a major unsolved problem in computer science. Informally, it asks whether every problem whose solution can be quickly verified by a computer can also be quickly solved by a computer. It was introduced in 1971 by Stephen Cook in his seminal paper 'The complexity of theorem proving procedures'[2] and is considered by many to be the most important open problem in the field.[3] It is one of the seven Millennium Prize Problems selected by the Clay Mathematics Institute to carry a US$ 1,000,000 prize for the first correct solution. The informal term quickly used above means the existence of an algorithm for the task that runs in polynomial time. The general class of questions for which some algorithm can provide an answer in polynomial time is called 'class' or just 'P'. For some questions, there is no known way to find an answer quickly, but if one is provided with information showing what the answer is, it may be possible to verify the answer quickly. The class of questions for which an answer can be verified in polynomial time is called NP. Consider the subset sum problem, an example of a problem that is easy to verify, but whose answer may be difficult to compute. Given a set of integers, does some nonempty subset of them sum to 0? For instance, does a subset of the set {−2, −3, 15, 14, 7, −10} add up to 0? The answer 'yes, because {−2, −3, −10, 15} add up to zero' can be quickly verified with three additions. However, there is no known algorithm to find such a subset in polynomial time (there is one, however, in exponential time, which consists of 2n-1 tries), and indeed such an algorithm can only exist if P = NP; hence this problem is in NP (quickly checkable) but not necessarily in P (quickly solvable). An answer to the P = NP question would determine whether problems that can be verified in polynomial time, like the subset-sum problem, can also be solved in polynomial time. If it turned out that P ≠ NP, it would mean that there are problems in NP (such as NP-complete problems) that are harder to compute than to verify: they could not be solved in polynomial time, but the answer could be verified in polynomial time. Aside from being an important problem in computational theory, a proof either way would have profound implications for mathematics, cryptography, algorithm research, artificial intelligence, game theory, multimedia processing and many other fields.", 
                        Amount = 1000000, 
                        StatusId = 1,
                        UserId = 4
                    }, //5
                    new Question 
                    { 
                        Title = "How to fix a monitor with a blue tint?", 
                        Description = "I have a computer with a blue tint on it and my monitor seems to changed language to dutch. I can see on my monitor options that the blue hue is turned up while red and green remain the same but i my monitor wont let me change the hue. also when my cord is not plugged in the entire screen is white. what do i do My monitor is aGateway FPD1730.", 
                        Amount = 20, 
                        StatusId = 1,
                        UserId = 4
                    }, //6
                    new Question 
                    { 
                        Title = "How to replace a helicopter blade?", 
                        Description = "Need to replace the Blade 400. It can be somewhat intimidating to a new pilot, especially when you look in the manual at the parts listing and see all of those parts. Willing to pay for step by step instructions to replace it", 
                        Amount = 50, 
                        StatusId = 1,
                        UserId = 5
                    }, //7
                    new Question 
                    { 
                        Title = "Flood in the 1st floor of my home?", 
                        Description = "Every time I flush the toilet, it overflow for a long period of time. I think something may be stuck in the pipes. I am willing to pay for step by step directions to identify the problem and fix it.", 
                        Amount = 15, 
                        StatusId = 1,
                        UserId = 5
                    } //8
                };
            questions.ForEach(s => context.Questions.Add(s));
            context.SaveChanges();

            questions[1].Subjects.Add(subjects[1]);
            questions[1].Subjects.Add(subjects[2]);
            questions[2].Subjects.Add(subjects[3]);
            questions[2].Subjects.Add(subjects[4]);
            questions[2].Subjects.Add(subjects[5]);
            questions[2].Subjects.Add(subjects[6]);
            questions[2].Subjects.Add(subjects[7]);
            questions[3].Subjects.Add(subjects[8]);
            questions[3].Subjects.Add(subjects[9]);
            questions[4].Subjects.Add(subjects[8]);
            questions[4].Subjects.Add(subjects[9]);
            questions[5].Subjects.Add(subjects[1]);
            questions[5].Subjects.Add(subjects[10]);
            questions[6].Subjects.Add(subjects[11]);
            questions[6].Subjects.Add(subjects[12]);
            questions[7].Subjects.Add(subjects[13]);
            questions[7].Subjects.Add(subjects[14]);
            questions[8].Subjects.Add(subjects[15]);
            questions[8].Subjects.Add(subjects[16]);
            questions[8].Subjects.Add(subjects[17]);
            questions[8].Subjects.Add(subjects[18]);
            questions[8].Subjects.Add(subjects[19]);
            context.SaveChanges();

            //for (int i = 0; i < 1000; i++)
            //{
            //    context.Restaurants.AddOrUpdate(r => r.Name,
            //        new Restaurant { Name = i.ToString(), City = "Nowhere", Country = "USA" });
            //}
        }

        private void CreateRoleAndUsers()
        {
            WebSecurity.InitializeDatabaseConnection("PfaDbSqlExpress",
                "UserProfile", "UserId", "UserName", autoCreateTables: true);

            var roles = (SimpleRoleProvider)Roles.Provider;
            var membership = (SimpleMembershipProvider)Membership.Provider;

            if (!roles.RoleExists("Admin"))
            {
                roles.CreateRole("Admin");
            }
            if (membership.GetUser("jvelazquez", false) == null)
            {
                membership.CreateUserAndAccount("jvelazquez", "P@ssword");
            }
            if (!roles.GetRolesForUser("jvelazquez").Contains("Admin"))
            {
                roles.AddUsersToRoles(new[] { "jvelazquez" }, new[] { "Admin" });
            }
            if (membership.GetUser("jvelazquez1", false) == null)
            {
                membership.CreateUserAndAccount("jvelazquez1", "P@ssword"); //UserId=2
            }
            if (membership.GetUser("jvelazquez2", false) == null)
            {
                membership.CreateUserAndAccount("jvelazquez2", "P@ssword");//UserId=3
            }
            if (membership.GetUser("jvelazquez3", false) == null)
            {
                membership.CreateUserAndAccount("jvelazquez3", "P@ssword");//UserId=4
            }
            if (membership.GetUser("jvelazquez4", false) == null)
            {
                membership.CreateUserAndAccount("jvelazquez4", "P@ssword");//UserId=5
            }

        }

    }
}
