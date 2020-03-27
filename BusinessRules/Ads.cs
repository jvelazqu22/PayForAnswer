using Domain.App_GlobalResources;
using Domain.Constants;
using Domain.Models.Entities;

namespace BusinessRules
{
    public class Ads
    {
        public string AppendTopAdAndDescription(string description, string questionID, decimal questionAmount, string questionTitle)
        {
            string topAd = GetQuestionTopAd(questionID, questionAmount, questionTitle);
            return topAd + description;
        }

        public string GetQuestionTopAd(string questionID, decimal questionAmount, string questionTitle)
        {
            string questionUrl = Urls.QUESTION_URL + questionID;
            string firstLine = CommonResources.GoogleAdHeadlinePrefix + questionAmount + " " + 
                CommonResources.GoogleAdHeadlineSuffix + " - " + questionTitle;

            //string subjects = spaceDelimitedSubjects.Length > Size.GoogleDescrLineMaxCharacters
            //    ? spaceDelimitedSubjects.Substring(0, Size.GoogleDescrLineMaxCharacters)
            //    : spaceDelimitedSubjects;

            var param = new object[2] { questionUrl, firstLine };

            //return string.Format(Html.TOP_QUESTION_AD_NEW_WINDOW, param);
            return firstLine;
        }
    }
}
