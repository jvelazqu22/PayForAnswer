namespace BusinessRulesTest
{
    using BusinessRules;
    using Domain.Models.Entities;
    using Domain.Models.Entities.Identity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Repository.Interfaces;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class EmailBRTest
    {
        [TestMethod]
        public void SuccessfullyGetAllUniqueEmailAddressOfUsersWithRelatedSubjectsInNewQuestion()
        {
            // Arrange
            ICollection<Subject> questionSubjects = new List<Subject>
                {
                    new Subject { Id = 1, SubjectName = "Math" }, //1
                    new Subject { Id = 2, SubjectName = "actionbarsherlock" }, //4
                    new Subject { Id = 3, SubjectName = "menuitem" }, //7
                };

            IEnumerable<ApplicationUser> users1 = new List<ApplicationUser>
                {
                    new ApplicationUser { Id = 1, Email = "jvelazquez1@hotmail.com" },
                    new ApplicationUser { Id = 2, Email = "jvelazquez1@hotmail.com" },
                    new ApplicationUser { Id = 3, Email = "jvelazquez3@hotmail.com" },
                    new ApplicationUser { Id = 4, Email = "jvelazquez4@hotmail.com" }
                };
            IEnumerable<ApplicationUser> users2 = new List<ApplicationUser>
                {
                    new ApplicationUser { Id = 5, Email = "jvelazquez5@hotmail.com" },
                    new ApplicationUser { Id = 6, Email = "jvelazquez6@hotmail.com" },
                    new ApplicationUser { Id = 7, Email = "jvelazquez7@hotmail.com" },
                    new ApplicationUser { Id = 1, Email = "jvelazquez1@hotmail.com" }
                };
            IEnumerable<ApplicationUser> users3 = new List<ApplicationUser>
                {
                    new ApplicationUser { Id = 9, Email = "jvelazquez9@hotmail.com" },
                    new ApplicationUser { Id = 10, Email = "jvelazquez10@hotmail.com" },
                    new ApplicationUser { Id = 11, Email = "jvelazquez11@hotmail.com" },
                    new ApplicationUser { Id = 2, Email = "jvelazquez2@hotmail.com" }
                };

            EmailBR emailBR = new EmailBR();
            Question questionModel = new Question();
            questionModel.UserId = 1; // User who posted the question.

            var subjectRepository = new Mock<ISubjectRepository>();
            subjectRepository.Setup(sr => sr.GetUsersBySubject(1)).Returns(users1);
            subjectRepository.Setup(sr => sr.GetUsersBySubject(2)).Returns(users2);
            subjectRepository.Setup(sr => sr.GetUsersBySubject(3)).Returns(users3);

            List<string> expectedListOfEmailsToNotify = new List<string>
            {
                "jvelazquez2@hotmail.com", "jvelazquez3@hotmail.com", "jvelazquez4@hotmail.com", "jvelazquez5@hotmail.com",
                "jvelazquez6@hotmail.com", "jvelazquez7@hotmail.com", "jvelazquez9@hotmail.com", "jvelazquez10@hotmail.com",
                "jvelazquez11@hotmail.com"
            };

            // Act
            List<string> listOfEmailAddresesToNotify =
                emailBR.GetListOfOptInEmailAddressThatHaveNewQuestionSubjects(questionModel, questionSubjects, 
                                                                        subjectRepository.Object);
            // Assert
            Assert.IsTrue(expectedListOfEmailsToNotify.Intersect(listOfEmailAddresesToNotify).Count() == 9);
        }
    }
}
