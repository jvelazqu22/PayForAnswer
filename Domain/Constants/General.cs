using System;
using System.Configuration;

namespace Domain.Constants
{
    public static class General
    {
        public const string QuestionShortDescriptionAppendSymbol = " ...";
        public const string UTC = "UTC";
        public const int MaxNumberOfSmartSearchResults = 10;

        public static bool RunningInProduction
        {
            get { return Convert.ToBoolean(ConfigurationManager.AppSettings["RunningInProduction"]); }
        }

        public static int MaxFailedAccessAttemptsBeforeLockout
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["MaxFailedAccessAttemptsBeforeLockout"]); }
        }

        public static int LockingPeriodInMinutes
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["LockingPeriod"]); }
        }

        public static Decimal ChargePercentageFee
        {
            get { return Convert.ToDecimal(ConfigurationManager.AppSettings["ChargePercentageFee"]); }
        }

        public static Decimal ChargeFixFee
        {
            get { return Convert.ToDecimal(ConfigurationManager.AppSettings["ChargeFixFee"]); }
        }
        public static Decimal MaxChargeFee
        {
            get { return Convert.ToDecimal(ConfigurationManager.AppSettings["MaxChargeFee"]); }
        }

        public static Decimal MinimumQuestionAmount
        {
            get { return Convert.ToDecimal(ConfigurationManager.AppSettings["MinimumQuestionAmount"]); }
        }

        public static Decimal MinimumMarketingBudgetPerDay
        {
            get { return Convert.ToDecimal(ConfigurationManager.AppSettings["MinimumMarketingBudgetPerDay"]); }
        }

        public static int MinimumMarketingDays
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["MinimumMarketingDays"]); }
        }

        public static Decimal MinimumQuestionAmountIncrease
        {
            get { return Convert.ToDecimal(ConfigurationManager.AppSettings["MinimumQuestionAmountIncrease"]); }
        }

        public static double QuestionMoneyAmountStorageBlockSize
        {
            get { return Convert.ToDouble(ConfigurationManager.AppSettings["QuestionAmountStorageBlockSize"]); }
        }

        public static double MegabytesPerQuestionLevel
        {
            get { return Convert.ToDouble(ConfigurationManager.AppSettings["MegabytesPerQuestionLevel"]); }
        }

        public static int AnswerStorageMultiplier
        {
            get { return Convert.ToInt16(ConfigurationManager.AppSettings["AnswerStorageMultiplier"]); }
        }

        public static int MaxAttachmentSizeInGigabytes
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["MaxAttachmentSizeInGigabytes"]); }
        }

        public static int MaxDescriptionSizeInMegabytes
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["MaxDescriptionSizeInMegabytes"]); }
        }

        public static string Alphabet
        {
            get { return "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z"; }
        }
    }

    public static class CommentType
    {
        public const int QuestionComment = 1;
        public const int AnswerComment = 2;
    }

}
