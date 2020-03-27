using Domain.App_GlobalResources;
using Domain.Models;
using Domain.Models.Entities;
using Domain.Models.Entities.Identity;
using Domain.Models.ViewModel;
using ErrorChecking;
using Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace BusinessRules
{
    public class SubjectBR
    {
        public void AddSubjectsToQuestionModelFromCommaDelimitedSubjects(CreateQuestionViewModel questionViewModel, Question questionModel, IQuestionSubjectRepository questionSubjectRepository)
        {
            var subjectStrings = questionViewModel.CommaDelimitedSubjects.Split(',');
            foreach (string item in subjectStrings)
            {
                Subject subject = questionSubjectRepository.GetSubjectBySubjectName(item);
                if (subject == null)
                {
                    subject = new Subject();
                    subject.SubjectName = item;
                }
                questionModel.Subjects.Add(subject);
            }
        }

        public QuestionsBySubjectModel GetUserSubjectsAndQuestionsRelatedToSubjects(int userId, IQuestionSubjectRepository questionSubjecrRepository)
        {
            QuestionsBySubjectModel questionsBySubjectModel = new QuestionsBySubjectModel();
            IQueryable<Subject> userSubjects = questionSubjecrRepository.GetUserSubjects(userId);
            IQueryable<Question> questions = questionSubjecrRepository.GetOpenQuestionsByUserSubjects(userSubjects);
            questionsBySubjectModel.Subjects = userSubjects.ToList();
            questionsBySubjectModel.Questions = questions;
            questionsBySubjectModel.CommaSpaceDelimitedSubjects = string.Join(", ", questionsBySubjectModel.Subjects.Select(s => s.SubjectName).ToArray());

            return questionsBySubjectModel;
        }

        public QuestionsBySubjectModel GetUnRegUserSubjectsAndQuestionsRelatedToSubjects(List<string> tempSubjectList, IQuestionSubjectRepository questionSubjecrRepository)
        {
            QuestionsBySubjectModel questionsBySubjectModel = new QuestionsBySubjectModel();
            IQueryable<Subject> userSubjects = questionSubjecrRepository.GetUnRegUserSubjects(tempSubjectList);
            IQueryable<Question> questions = questionSubjecrRepository.GetOpenQuestionsByUserSubjects(userSubjects);
            questionsBySubjectModel.Subjects = userSubjects.ToList();
            questionsBySubjectModel.Questions = questions;
            questionsBySubjectModel.CommaSpaceDelimitedSubjects = string.Join(", ", tempSubjectList.ToArray());

            return questionsBySubjectModel;
        }

        public void AddUpdateUserSubjects(int userID, List<string> subjectNames, ISubjectRepository subjectRepository)
        {
            ApplicationUser user = subjectRepository.GetUserByID(userID);
            List<string> currentUserSubjects = user.Subjects.Select(s => s.SubjectName).ToList();
            int intersectCount = subjectNames.Intersect(currentUserSubjects).Count();
            if (intersectCount > 0 && intersectCount == subjectNames.Count() && intersectCount == currentUserSubjects.Count())
                return;

            List<Subject> subjectEntities = user.Subjects.ToList();
            if (subjectEntities.Count > 0)
                subjectRepository.RemoveUserSubjects(user, subjectEntities);

            foreach (var subjectName in subjectNames)
            {
                Subject subject = string.IsNullOrEmpty(subjectName) ? null : subjectRepository.GetSubjectByName(subjectName);
                if (subject != null && !string.IsNullOrEmpty(subjectName))
                {
                    user.Subjects.Add(subject);
                }
                else if (!string.IsNullOrEmpty(subjectName))
                {
                    subject = new Subject { SubjectName = subjectName };
                    subjectRepository.AddSubject(subject);
                    user.Subjects.Add(subject);
                }
            }
            subjectRepository.Save();
        }
    }
}
