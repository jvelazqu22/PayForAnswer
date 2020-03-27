using Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Interfaces
{
    public interface IMarketingRepository : IDisposable
    {
        MarketingCampaign GetMarketingCampaignByID(long id);
        List<MarketingCampaign> GetMarketingCampaignsListByStatus(List<int> statusList);
        List<MarketingCampaign> GetTopMostRecentStartedMarketingCampaignsList(int campaignStatus, int recordsToTake);
        void InsertCampaign(MarketingCampaign campaign);
        void UpdateCampaign(MarketingCampaign campaign);
        IQueryable<Status> GetCampaignStatus();
        Status GetCampaignStatusByID(int statusId);
        void Save();
    }
}
