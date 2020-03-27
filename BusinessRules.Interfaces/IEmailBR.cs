using Domain.Models.Entities;
using Domain.Models.Entities.Identity;
using Domain.Models.Helper;
using System.Collections.Generic;

namespace BusinessRules.Interfaces
{
    public interface IEmailBR
    {
        void SendNewQuestionToAllUsersWithRelatedSubject(Question questionModel, ICollection<Subject> Subjects, int paymentType);
        void Send(EmailModel emailModel);
        void SendEmail(string to, string subject, string body);
        void NotifyAllCampaignManagers(List<ApplicationUser> campaignManagers);
    }
}
