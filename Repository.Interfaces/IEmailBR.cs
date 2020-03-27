using Domain.Models;
using System.Collections.Generic;

namespace Repository.Interfaces
{
    public interface IEmailBR
    {
        void SendNewQuestionToAllUsersWithRelatedSubject(Question questionModel, ICollection<Subject> Subjects);
        void Send(EmailModel emailModel);
    }
}
