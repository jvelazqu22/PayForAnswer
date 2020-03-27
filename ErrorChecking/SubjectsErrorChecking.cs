using Domain.App_GlobalResources;
using Domain.Constants;
using Domain.Models.Helper;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ErrorChecking
{
    public class SubjectsErrorChecking
    {
        public Error IsUserSubjectListValid(string commaDelimitedSubjects, ref List<string> distinctSubjects)
        {
            string subjectSamples = CommonResources.SubjectSamples.ToLower();
            subjectSamples = Regex.Replace(subjectSamples, @"\s+", "");

            List<char> resutl = commaDelimitedSubjects.ToList();
            resutl.RemoveAll(c => c == ' ' || c == '\n');
            commaDelimitedSubjects = new string(resutl.ToArray());
            string[] subjects = commaDelimitedSubjects.Split(',');
            subjects = subjects.Where(s => s != "").ToArray();

            distinctSubjects = subjects.Distinct().ToList();
            bool duplicatesFound = subjects.Count() > distinctSubjects.Count() ? true : false;

            if (duplicatesFound)
                return new Error() { ErrorFound = true, Message = CommonResources.DuplicateSubjectErrorMgs };
            if (subjectSamples == commaDelimitedSubjects)
                return new Error() { ErrorFound = true, Message = CommonResources.ReplaceSampleSubjectsErrorMsg };
            if (subjects.Count() < Size.MinNumberOfUserSubjects)
                return new Error() { ErrorFound = true, Message = string.Format(CommonResources.MinSubjecsErrorMsg, Size.MinNumberOfUserSubjects) };
            if (subjects.Count() > Size.MaxNumberOfUserSubjects)
                return new Error() { ErrorFound = true, Message = string.Format(CommonResources.MaxSubjectsErrorMsg, Size.MaxNumberOfUserSubjects) };
            if (DoAnySubjectsExceedTheMaxCharactersPerSubjectAllowed(distinctSubjects))
                return new Error() { ErrorFound = true, Message = string.Format(CommonResources.ExceedMaxCharPerSubjectErrorMsg, Size.MaxCharactersPerSubjectAllowed) };

            return new Error() { ErrorFound = false };
        }

        public Error IsQuestionSubjectListValid(string commaDelimitedSubjects)
        {
            string subjectSamples = CommonResources.SubjectSamples.ToLower();
            subjectSamples = Regex.Replace(subjectSamples, @"\s+", "");

            if(string.IsNullOrWhiteSpace(commaDelimitedSubjects))
                return new Error() { ErrorFound = true, Message = string.Format(CommonResources.MinSubjecsErrorMsg, Size.MinNumberOfQuestionSubjects) };

            List<char> resutl = commaDelimitedSubjects.ToList();
            resutl.RemoveAll(c => c == ' ' || c == '\n');
            commaDelimitedSubjects = new string(resutl.ToArray());
            string[] subjects = commaDelimitedSubjects.Split(',');
            subjects = subjects.Where(s => s != "").ToArray();

            List<string> distinctSubjects = subjects.Distinct().ToList();
            bool duplicatesFound = subjects.Count() > distinctSubjects.Count() ? true : false;

            if (duplicatesFound)
                return new Error() { ErrorFound = true, Message = CommonResources.DuplicateSubjectErrorMgs };
            if (subjectSamples == commaDelimitedSubjects)
                return new Error() { ErrorFound = true, Message = CommonResources.ReplaceSampleSubjectsErrorMsg };
            if (subjects.Count() < Size.MinNumberOfQuestionSubjects)
                return new Error() { ErrorFound = true, Message = string.Format(CommonResources.MinSubjecsErrorMsg, Size.MinNumberOfQuestionSubjects) };
            if (subjects.Count() > Size.MaxNumberOfQuestionSubjects)
                return new Error() { ErrorFound = true, Message = string.Format(CommonResources.MaxSubjectsErrorMsg, Size.MaxNumberOfQuestionSubjects) };
            if (DoAnySubjectsExceedTheMaxCharactersPerSubjectAllowed(distinctSubjects))
                return new Error() { ErrorFound = true, Message = string.Format(CommonResources.ExceedMaxCharPerSubjectErrorMsg, Size.MaxCharactersPerSubjectAllowed) };

            return new Error() { ErrorFound = false };
        }

        public Error IsUnRegUserSubjectListValid(string commaDelimitedSubjects, ref List<string> distinctSubjects)
        {
            string subjectSamples = CommonResources.UnAuthDefaultSubjects.ToLower();
            subjectSamples = Regex.Replace(subjectSamples, @"\s+", "");

            distinctSubjects = GetSanatizedSubjectNamesList(commaDelimitedSubjects);
            return new Error() { ErrorFound = false };
        }

        public bool DoAnySubjectsExceedTheMaxCharactersPerSubjectAllowed(List<string> distinctSubjects)
        {
            foreach(var str in distinctSubjects)
            {
                if (str.Length > Size.MaxCharactersPerSubjectAllowed)
                    return true;
            }
            return false;
        }

        public List<string> GetSanatizedSubjectNamesList(string commaDelimitedSubjects)
        {
            List<char> resutl = commaDelimitedSubjects.ToList();
            resutl.RemoveAll(c => c == ' ' || c == '\n');
            commaDelimitedSubjects = new string(resutl.ToArray());
            string[] subjects = commaDelimitedSubjects.Split(',');
            subjects = subjects.Where(s => s != "").ToArray();
            return  subjects.Distinct().ToList();
        }
    }
}
