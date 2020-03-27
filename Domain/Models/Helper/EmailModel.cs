using System.Collections.Generic;

namespace Domain.Models.Helper
{
    public class EmailModel
    {
        public EmailModel()
        {
            ListOfEmailAddresesToNotify = new List<string>();
            NotifyAllCampaignManagers = false;
        }

        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string EmailServer { get; set; }
        public string SenderName { get; set; }
        public bool NotifyAllCampaignManagers { get; set; }
        public List<string> ListOfEmailAddresesToNotify { get; set; }
    }
}
