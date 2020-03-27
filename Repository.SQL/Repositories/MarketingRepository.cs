using Domain.Constants;
using Domain.Models.Entities;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Repository.SQL
{
    public class MarketingRepository : IMarketingRepository
    {
        private PfaDb context;

        public MarketingRepository()
        {
            this.context = new PfaDb();
        }

        public MarketingRepository(PfaDb context)
        {
            this.context = context;
        }

        public MarketingCampaign GetMarketingCampaignByID(long id)
        {
            return context.MarketingCampaigns
                .Include(m => m.QuestionPaymentDetail)
                .Where(r => r.QuestionPaymentDetailID == id).FirstOrDefault();
        }

        public List<MarketingCampaign> GetMarketingCampaignsListByStatus(List<int> statusList)
        {
            return context.MarketingCampaigns
                    .Where(c => statusList.Contains((int)c.StatusId))
                    .Include(m => m.QuestionPaymentDetail)
                    .Include(m => m.User)
                    .Include(m => m.Status).ToList();
        }

        public List<MarketingCampaign> GetTopMostRecentStartedMarketingCampaignsList(int campaignStatus, int recordsToTake)
        {
            return context.MarketingCampaigns
                    .OrderByDescending(m => m.StartDate)
                    .Where(c => c.StatusId == CampaignStatus.CampaignStarted)
                    .Include(m => m.QuestionPaymentDetail)
                    .Include(m => m.User)
                    .Include(m => m.Status)
                    .Take(recordsToTake).ToList();
        }

        public void InsertCampaign(MarketingCampaign campaign)
        {
            context.MarketingCampaigns.Add(campaign);
        }

        public void UpdateCampaign(MarketingCampaign campaign)
        {
            context.Entry(campaign).State = EntityState.Modified;
            context.SaveChanges();
        }

        public IQueryable<Status> GetCampaignStatus()
        {
            return context.Status.Where(s => StatusList.CAMPAIGN_STATUS.Contains(s.Id));
        }

        public Status GetCampaignStatusByID(int statusId)
        {
            return context.Status.Where(m => m.Id == statusId).FirstOrDefault();
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
