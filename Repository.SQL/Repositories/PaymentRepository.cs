using Domain.Constants;
using Domain.Models.Entities;
using Domain.Models.Entities.Identity;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Repository.SQL
{
    public class PaymentRepository : IPaymentRepository
    {
        private PfaDb context;

        public PaymentRepository()
        {
            this.context = new PfaDb();
        }

        public PaymentRepository(PfaDb context)
        {
            this.context = context;
        }

        public QuestionPaymentDetail GetPaymentDetailByID(long id)
        {
            return context.QuestionPaymentDetails
                .Include(qpd => qpd.Question)
                .Include(qpd => qpd.Payment)
                .Include(qpd => qpd.MarketingCampaign)
                .Where(r => r.QuestionPaymentDetailID == id).FirstOrDefault();
        }
        public QuestionPaymentDetail GetPaymentDetailByPaymentID(long id)
        {
            return context.QuestionPaymentDetails
                .Include(qpd => qpd.Question)
                .Include(qpd => qpd.Payment)
                .Include(qpd => qpd.MarketingCampaign)
                .Where(r => r.PaymentId == id).FirstOrDefault();
        }
        public List<QuestionPaymentDetail> GetPaymentDetailListByQuestionID(Guid questionId)
        {
            return context.QuestionPaymentDetails
                .Include(qpd => qpd.Question)
                .Include(qpd => qpd.Payment)
                .Include(qpd => qpd.MarketingCampaign)
                .Where(r => r.QuestionId == questionId).ToList();
        }

        public void InsertPaymentDetail(QuestionPaymentDetail paymentDetail)
        {
            context.QuestionPaymentDetails.Add(paymentDetail);
        }

        public void UpdatePaymentDetail(QuestionPaymentDetail paymentDetail)
        {
            context.Entry(paymentDetail).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void UpdatePayment(Payment payment)
        {
            context.Entry(payment).State = EntityState.Modified;
            context.SaveChanges();
        }

        public List<ApplicationUser> GetAllCampaignManagers()
        {
            var campaignManagerRoleID = context.Roles.Where(r => r.Name == Role.CampaignManager).First().Id;
            return context.Users.Where(u => u.Roles.Select(r => r.RoleId).Contains(campaignManagerRoleID)).ToList();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
