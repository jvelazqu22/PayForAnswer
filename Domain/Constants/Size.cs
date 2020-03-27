
using System;
namespace Domain.Constants
{
    public static class Size
    {
        public const int MinNumberOfUserSubjects = 3;
        public const int MaxNumberOfUserSubjects = 500;
        public const int MinNumberOfQuestionSubjects = 1;
        public const int MaxNumberOfQuestionSubjects = 20;
        public const int MaxCharactersPerSubjectAllowed = 30;
        public const int QuestionShortDescriptionSize = 90;
        public const int QuestionPagerPageSize = 30;
        public const int HomePageQuestionPagerPageSize = 2;
        public const int GeneralPageSize = 30;
        public const int AnswerPagerPageSize = 15;
        public const int UserAnswersPagerPageSize = 30;
        public const int MarketingCampaignsPageSize = 30;
        public const int TopStartedMarketingCampaigns = 1000;
        public const int NumberOfQuestionsToDisplayOnTheMainPage = 100;
        public const int GoogleHeadLineMaxCharacters = 25;
        public const int GoogleDescrLineMaxCharacters = 35;
        public const int GoogleSearchKeyWordMaxCharacters = 100;
        public const int FeedbackTitleMaxCharacters = 100;
        public const int ParallelOperationThreadCount = 10;
        public const int MaxCommentCharacters = 1000;
        public const int DobYearsRange = 100;
        public const int MinimumAgeForRegistration = 18;
    }

    public static class DobRange
    {
        public static int MinDobYear
        {
            get { return DateTime.UtcNow.Year - Size.DobYearsRange; }
        }
        public static int MaxDobToAddToMinDobYear
        {
            get { return Size.DobYearsRange + 1; }
        }
    }
    public static class UserTimeLimits
    {
        public const int TokenExpirationInHoursFromNow = 24;
        public const int LogOffUserInMinutesFromNow = 30;
    }

    public static class StorageSize
    {
        public const double BytesInATerabyte = 1099511627776;
        public const double BytesInAGigabyte = 1073741824;
        public const double BytesInAMegabyte = 1048576;
        public const double BytesInAKilobyte = 1024;
    }

    public static class StorageSizeLimits
    {
        public static double MAX_ATTACHMENT_SIZE = StorageSize.BytesInAGigabyte * General.MaxAttachmentSizeInGigabytes;
        public static double MAX_DESCRIPTION_SIZE = StorageSize.BytesInAMegabyte * General.MaxDescriptionSizeInMegabytes;
    }

}
