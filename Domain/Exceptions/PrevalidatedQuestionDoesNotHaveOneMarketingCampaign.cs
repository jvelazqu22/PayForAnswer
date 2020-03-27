using System;

namespace Domain.Exceptions
{
    public class PrevalidatedQuestionDoesNotHaveOneMarketingCampaign : Exception
    {
        public PrevalidatedQuestionDoesNotHaveOneMarketingCampaign() { }
        public PrevalidatedQuestionDoesNotHaveOneMarketingCampaign(string message) : base(message) { }
    }
}
