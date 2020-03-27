using Domain.Models.Entities;
using Domain.Models.Entities.Identity;
using System;
using System.Collections.Generic;

namespace Repository.Interfaces
{
    public interface IPaymentRepository : IDisposable
    {
        QuestionPaymentDetail GetPaymentDetailByID(long id);
        QuestionPaymentDetail GetPaymentDetailByPaymentID(long id);
        List<QuestionPaymentDetail> GetPaymentDetailListByQuestionID(Guid questionId);
        void InsertPaymentDetail(QuestionPaymentDetail paymentDetail);
        void UpdatePaymentDetail(QuestionPaymentDetail paymentDetail);
        void UpdatePayment(Payment payment);
        List<ApplicationUser> GetAllCampaignManagers();

        void Save();
    }
}
