using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Calculation
{
    public class MarketingCampaignTotals
    {
        public int DaysLeft { get; set; }
        public decimal BudgetPerDay { get; set; }
        public decimal TotalBudgetLeft { get; set; }
        public DateTime CampaignEndDate { get; set; }
    }
}
