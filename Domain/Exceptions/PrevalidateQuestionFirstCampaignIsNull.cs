using System;

namespace Domain.Exceptions
{
    public class PrevalidateQuestionFirstCampaignIsNull : Exception
    {
        public PrevalidateQuestionFirstCampaignIsNull() { }
        public PrevalidateQuestionFirstCampaignIsNull(string message) : base(message) { }
    }
}
