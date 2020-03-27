using AutoMapper;
using Domain.App_GlobalResources;
using Domain.Constants;
using Domain.Models;
using Domain.Models.Calculation;
using Domain.Models.Entities;
using Domain.Models.Entities.Identity;
using Domain.Models.ViewModel;
using ErrorChecking;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessRules
{
    public class MarketingBR
    {
        public MarketingBR()
        {
            Mapper.CreateMap<CreateQuestionViewModel, MarketingCampaign>()
                .ForMember(dest => dest.PerDayBudget, opt => opt.MapFrom(src => src.MarketingBudgetPerDay))
                .ForMember(dest => dest.NumberOfDaysToRun, opt => opt.MapFrom(src => src.NumberOfCampaignDays));

            Mapper.CreateMap<AddMoneyViewModel, MarketingCampaign>()
                .ForMember(dest => dest.PerDayBudget, opt => opt.MapFrom(src => src.MarketingBudgetPerDay))
                .ForMember(dest => dest.NumberOfDaysToRun, opt => opt.MapFrom(src => src.NumberOfCampaignDays));
        }

        public void UpdateMarketingStatusModel(MarketingCampaignViewModel marketingCampaignViewModel, ApplicationUser user, IMarketingRepository marketingRepository)
        {
            MarketingCampaign dbMarketingCampaign = marketingRepository.GetMarketingCampaignByID(marketingCampaignViewModel.QuestionPaymentDetailID);

            if (marketingCampaignViewModel.StatusId == CampaignStatus.CampaignReadyToBeStarted)
                dbMarketingCampaign.CampaignManagerId = null;
            else
                dbMarketingCampaign.CampaignManagerId = user.Id;

            if(marketingCampaignViewModel.StatusId == CampaignStatus.CampaignStarted)
            {
                dbMarketingCampaign.StartDate = marketingCampaignViewModel.StartDate.HasValue ? marketingCampaignViewModel.StartDate : DateTime.UtcNow;
                dbMarketingCampaign.EndDate = new MarketingBR().GetEndDate(dbMarketingCampaign, DateTime.UtcNow);
            }
            dbMarketingCampaign.StatusId = marketingCampaignViewModel.StatusId;
            dbMarketingCampaign.RowVersion = marketingCampaignViewModel.RowVersion;
            dbMarketingCampaign.UpdatedOn = DateTime.UtcNow;
            dbMarketingCampaign.UpdatedBy = user.UserName;

            marketingRepository.UpdateCampaign(dbMarketingCampaign);
        }

        public void AddFirstCampaignToPaymentModel(CreateQuestionViewModel questionViewModel, QuestionPaymentDetail paymentModel)
        {
            MarketingCampaign marketingCampaign = Mapper.Map<CreateQuestionViewModel, MarketingCampaign>(questionViewModel);
            marketingCampaign.SearchKeywords.Add(new SearchKeyword() { Keywords = questionViewModel.GoogleSearchKeywords1 });
            marketingCampaign.SearchKeywords.Add(new SearchKeyword() { Keywords = questionViewModel.GoogleSearchKeywords2 });
            marketingCampaign.SearchKeywords.Add(new SearchKeyword() { Keywords = questionViewModel.GoogleSearchKeywords3 });
            marketingCampaign.SearchKeywords.Add(new SearchKeyword() { Keywords = questionViewModel.GoogleSearchKeywords4 });
            marketingCampaign.SearchKeywords.Add(new SearchKeyword() { Keywords = questionViewModel.GoogleSearchKeywords5 });

            marketingCampaign.UsedBudget = 0;
            marketingCampaign.StatusId = StatusValues.WaitingForPaymentNotification;
            marketingCampaign.CreatedOn = DateTime.UtcNow;
            marketingCampaign.CreatedBy = Role.PayForAnswer;
            marketingCampaign.UpdatedOn = DateTime.UtcNow;
            marketingCampaign.UpdatedBy = Role.PayForAnswer;
            paymentModel.MarketingCampaign = marketingCampaign;
        }

        public MarketingCampaign CreateMarketingCampaign(AddMoneyViewModel addMoneyViewModel)
        {
            MarketingCampaign marketingCampaign = Mapper.Map<AddMoneyViewModel, MarketingCampaign>(addMoneyViewModel);
            marketingCampaign.UsedBudget = 0;
            marketingCampaign.StatusId = StatusValues.WaitingForPaymentNotification;
            marketingCampaign.CreatedOn = DateTime.UtcNow;
            marketingCampaign.CreatedBy = addMoneyViewModel.UserName;
            marketingCampaign.UpdatedOn = DateTime.UtcNow;
            marketingCampaign.UpdatedBy = addMoneyViewModel.UserName;

            return marketingCampaign;
        }

        public void GetQuestionActiveMarketingCampaignSummary(Question question, MarketingSummaryBase marketingSummary, DateTime utcNow)
        {
            List<MarketingCampaign> allQuestionMarketingCampaigns = GetAllQuerstionMarketingCampaigns(question);
            List<MarketingCampaign> allQuestionActiveMarketingCampaigns = GetAllQuestionActiveMarketingCampaigns(allQuestionMarketingCampaigns, utcNow);
            List<MarketingCampaignTotals> questionMarketingCampaignTotals = GetQuestionActiveCampaignsTotals(allQuestionActiveMarketingCampaigns, utcNow);

            marketingSummary.TotalActiveCampaigns = allQuestionActiveMarketingCampaigns.Count;
            if (marketingSummary.TotalActiveCampaigns > 0)
            {
                marketingSummary.MarketingBudgetLeft = questionMarketingCampaignTotals.Sum(g => g.TotalBudgetLeft);
                marketingSummary.LongestCampaignEndDate = questionMarketingCampaignTotals.Max(g => g.CampaignEndDate);
                marketingSummary.LongestCampaignEndDateString = marketingSummary.LongestCampaignEndDate.ToString() + CommonResources.UTC;
            }
            else
            {
                marketingSummary.MarketingBudgetLeft = 0;
                marketingSummary.LongestCampaignEndDate = null;
                marketingSummary.LongestCampaignEndDateString = CommonResources.NotAvailable;
            }
        }

        public List<MarketingCampaignTotals> GetQuestionActiveCampaignsTotals(List<MarketingCampaign> allQuestionActiveMarketingCampaigns, DateTime utcNow)
        {
            if (allQuestionActiveMarketingCampaigns.Count == 0) return new List<MarketingCampaignTotals>();
            List<MarketingCampaignTotals> marketingCampaignsTotals = new List<MarketingCampaignTotals>();

            foreach (var campaign in allQuestionActiveMarketingCampaigns)
            {
                if(campaign.StatusId != StatusValues.WaitingForPaymentNotification && campaign.NumberOfDaysToRun != 0 && campaign.PerDayBudget != 0)
                {
                    DateTime endDate = (DateTime)GetEndDate(campaign, utcNow);
                    MarketingCampaignTotals campaignTotals = new MarketingCampaignTotals();
                    campaignTotals.BudgetPerDay = campaign.PerDayBudget;
                    campaignTotals.DaysLeft = DaysLeftBeforeCampaignEnds(campaign, utcNow);
                    campaignTotals.TotalBudgetLeft = campaignTotals.BudgetPerDay * (decimal)campaignTotals.DaysLeft;
                    campaignTotals.CampaignEndDate = endDate ;
                    marketingCampaignsTotals.Add(campaignTotals);
                }
            }
            return marketingCampaignsTotals;
        }

        public int DaysLeftBeforeCampaignEnds(MarketingCampaign campaign, DateTime utcNow)
        {
            DateTime? endDate = GetEndDate(campaign, utcNow);
            if (endDate == null && campaign.NumberOfDaysToRun != 0 && campaign.PerDayBudget != 0) return campaign.NumberOfDaysToRun;
            if (endDate == null && (campaign.NumberOfDaysToRun == 0 || campaign.PerDayBudget == 0)) return 0;
            if (utcNow >= endDate) return 0;

            return (int) ((DateTime)endDate - utcNow).TotalDays;
            //return Math.Truncate(100 * ((DateTime)endDate - utcNow).TotalDays) / 100;
        }

        public DateTime? GetEndDate(MarketingCampaign campaign, DateTime utcNow)
        {
            if (campaign.StatusId == StatusValues.WaitingForPaymentNotification || campaign.NumberOfDaysToRun == 0 || campaign.PerDayBudget == 0)
                return null;

            DateTime startDate, endDate;
            if (campaign.StartDate == null)
                endDate = utcNow.AddDays(campaign.NumberOfDaysToRun);
            else 
            {
                startDate = (DateTime)campaign.StartDate;
                endDate = campaign.EndDate.HasValue ? (DateTime)campaign.EndDate : startDate.AddDays(campaign.NumberOfDaysToRun);
            }

            return endDate;
        }

        public List<MarketingCampaign> GetAllQuerstionMarketingCampaigns(Question question)
        {
            List<MarketingCampaign> allMarketingCampaigns = new List<MarketingCampaign>();
            List<long> questionPaymentDetailIds = question.QuestionPaymentDetails.Select(r => r.QuestionPaymentDetailID).ToList();
            List<QuestionPaymentDetail> questionPaymentDetails = question.QuestionPaymentDetails.ToList();
            foreach(var paymentDetail in questionPaymentDetails)
                if (paymentDetail.MarketingCampaign != null)
                    allMarketingCampaigns.Add(paymentDetail.MarketingCampaign);

            return allMarketingCampaigns;
        }

        public List<MarketingCampaign> GetAllQuestionActiveMarketingCampaigns(List<MarketingCampaign> allMarketingCampaigns, DateTime utcNow)
        {
            List<MarketingCampaign> allQuestionActiveMarketingCampaigns = new List<MarketingCampaign>();
            foreach (var campaign in allMarketingCampaigns)
            {
                if (campaign.StatusId == StatusValues.WaitingForPaymentNotification)
                    continue;

                if (campaign.StartDate == null)
                {
                    allQuestionActiveMarketingCampaigns.Add(campaign);
                    continue;
                }
                else
                {
                    DateTime startDate = (DateTime)campaign.StartDate;
                    DateTime endDate = campaign.EndDate.HasValue ? (DateTime)campaign.EndDate : startDate.AddDays(campaign.NumberOfDaysToRun);

                    if (endDate <= utcNow)
                        continue;
                    else
                        allQuestionActiveMarketingCampaigns.Add(campaign);
                }
            }
            return allQuestionActiveMarketingCampaigns;
        }

        public List<QuestionMarketingHistoryViewModel> GetQuestionMarketingHistoryViewModelList(Guid questionId, IPaymentRepository paymentRepository, int currentUserId)
        {
            List<QuestionPaymentDetail> questionPaymentDetailList = paymentRepository.GetPaymentDetailListByQuestionID(questionId);
            //new MarketingErrorCheckingBR().ValidateUser(questionPaymentDetailList.FirstOrDefault(), currentUserId);
            List<QuestionMarketingHistoryViewModel> questionMarketingHistoryList = new List<QuestionMarketingHistoryViewModel>();

            foreach (var record in questionPaymentDetailList)
            {
                QuestionMarketingHistoryViewModel marketingHistory = new QuestionMarketingHistoryViewModel();
                marketingHistory.QuestionID = questionId;
                marketingHistory.QuestionTitle = record.Question.Title;
                marketingHistory.TotalMarketingBudget = record.TotalMarketingBudget;
                marketingHistory.QuestionAmountIncrease = record.QuestionAmountIncrease;
                marketingHistory.QuestionAmountBeforeIncrease = record.QuestionAmountBeforeIncrease;
                marketingHistory.QuestionPaymentDetailID = record.QuestionPaymentDetailID;
                if (record.MarketingCampaign != null)
                {
                    marketingHistory.CratedOn = record.MarketingCampaign.CreatedOn;
                    marketingHistory.CreatedOnString = marketingHistory.CratedOn + CommonResources.UTC;
                    marketingHistory.NumberOfDaysToRun = record.MarketingCampaign.NumberOfDaysToRun;
                    marketingHistory.PerDayBudget = record.MarketingCampaign.PerDayBudget;
                    marketingHistory.StartDateString = record.MarketingCampaign.StartDate != null
                        ? record.MarketingCampaign.StartDate + CommonResources.UTC : CommonResources.NotStartedYet;
                    marketingHistory.EndDateString = record.MarketingCampaign.EndDate != null
                        ? record.MarketingCampaign.EndDate + CommonResources.UTC : CommonResources.NotStartedYet;
                    marketingHistory.Status = record.MarketingCampaign.Status.DisplayName;
                }
                else
                {
                    marketingHistory.CratedOn = record.CreatedOn;
                    marketingHistory.CreatedOnString = record.CreatedOn + CommonResources.UTC;
                    marketingHistory.NumberOfDaysToRun = 0;
                    marketingHistory.PerDayBudget = 0;
                    marketingHistory.StartDateString = CommonResources.NotAvailable;
                    marketingHistory.EndDateString = CommonResources.NotAvailable;
                    marketingHistory.Status = StatusList.CONFIRM_PAYMENT_STATUS.Contains((int)record.Payment.StatusId)
                        ? CommonResources.Processed : CommonResources.NoPaymentReceived;
                }
                questionMarketingHistoryList.Add(marketingHistory);
            }

            return questionMarketingHistoryList.OrderByDescending(d => d.CratedOn).ToList();
        }

        public List<MarketingCampaignViewModel> GetMarketingCampaignViewModelList(List<MarketingCampaign> marketingCampaignsList)
        {
            List<MarketingCampaignViewModel> marketingCampaignViewModelList = new List<MarketingCampaignViewModel>();
            foreach (var campaign in marketingCampaignsList)
            {
                MarketingCampaignViewModel marketingCampaignViewModel = new MarketingCampaignViewModel();
                marketingCampaignViewModel.QuestionPaymentDetailID = campaign.QuestionPaymentDetailID;
                marketingCampaignViewModel.QuestionTitle = campaign.QuestionPaymentDetail.Question.Title;
                marketingCampaignViewModel.PerDayBudget = campaign.PerDayBudget;
                marketingCampaignViewModel.NumberOfDaysToRun = campaign.NumberOfDaysToRun;
                marketingCampaignViewModel.TotalMarketingBudget = campaign.QuestionPaymentDetail.TotalMarketingBudget;
                marketingCampaignViewModel.Amount = campaign.QuestionPaymentDetail.Question.Amount;
                marketingCampaignViewModel.QuestionAmountIncrease = campaign.QuestionPaymentDetail.QuestionAmountIncrease;
                marketingCampaignViewModel.StartDate = campaign.StartDate;
                marketingCampaignViewModel.StartDateString = campaign.StartDate == null ? "Not started yet" : campaign.StartDate.ToString();
                marketingCampaignViewModel.EndDate = campaign.EndDate;
                marketingCampaignViewModel.EndDateString = campaign.EndDate == null ? "Not started yet" : campaign.EndDate.ToString();
                marketingCampaignViewModel.QuestionID = campaign.QuestionPaymentDetail.QuestionId;
                marketingCampaignViewModel.QuestionPosterUserName = campaign.QuestionPaymentDetail.Question.User.UserName;
                marketingCampaignViewModel.QuestionPosterEmailAddress = campaign.QuestionPaymentDetail.Question.User.Email;
                marketingCampaignViewModel.QuestionSubjects = campaign.QuestionPaymentDetail.Question.Subjects.Select(s => s.SubjectName).ToList();
                if (campaign.User != null)
                {
                    marketingCampaignViewModel.CampaignManagerUserName = campaign.User.UserName;
                    marketingCampaignViewModel.CampaignManagerId = (int)campaign.CampaignManagerId;
                }
                else
                {
                    marketingCampaignViewModel.CampaignManagerUserName = null;
                    marketingCampaignViewModel.CampaignManagerId = 0;
                }
                marketingCampaignViewModel.CreatedOn = campaign.CreatedOn;
                marketingCampaignViewModel.CreatedOnString = campaign.CreatedOn.ToString();
                marketingCampaignViewModel.Status = campaign.Status.DisplayName;
                marketingCampaignViewModelList.Add(marketingCampaignViewModel);
            }

            return marketingCampaignViewModelList;
        }

        public MarketingCampaignViewModel GetMarketingCampaignViewModelById(IMarketingRepository marketingRepository, long questionPaymentDetailID)
        {
            MarketingCampaign marketingCampaign = marketingRepository.GetMarketingCampaignByID(questionPaymentDetailID);
            MarketingCampaignViewModel marketingCampaignViewModel = new MarketingCampaignViewModel();
            marketingCampaignViewModel.QuestionPaymentDetailID = marketingCampaign.QuestionPaymentDetailID;
            marketingCampaignViewModel.QuestionTitle = marketingCampaign.QuestionPaymentDetail.Question.Title;
            marketingCampaignViewModel.PerDayBudget = marketingCampaign.PerDayBudget;
            marketingCampaignViewModel.NumberOfDaysToRun = marketingCampaign.NumberOfDaysToRun;
            marketingCampaignViewModel.TotalMarketingBudget = marketingCampaign.QuestionPaymentDetail.TotalMarketingBudget;
            marketingCampaignViewModel.Amount = marketingCampaign.QuestionPaymentDetail.Question.Amount;
            marketingCampaignViewModel.QuestionAmountIncrease = marketingCampaign.QuestionPaymentDetail.QuestionAmountIncrease;
            marketingCampaignViewModel.QuestionAmountBeforeIncrease = marketingCampaign.QuestionPaymentDetail.QuestionAmountBeforeIncrease;
            marketingCampaignViewModel.StartDate = marketingCampaign.StartDate;
            marketingCampaignViewModel.StartDateString = marketingCampaign.StartDate == null ? "Not started yet" : marketingCampaign.StartDate.ToString() + CommonResources.UTC;
            marketingCampaignViewModel.EndDate = marketingCampaign.EndDate;
            marketingCampaignViewModel.EndDateString = marketingCampaign.EndDate == null ? "Not started yet" : marketingCampaign.EndDate.ToString() + CommonResources.UTC;
            marketingCampaignViewModel.QuestionID = marketingCampaign.QuestionPaymentDetail.QuestionId;
            marketingCampaignViewModel.QuestionPosterUserName = marketingCampaign.QuestionPaymentDetail.Question.User.UserName;
            marketingCampaignViewModel.QuestionPosterEmailAddress = marketingCampaign.QuestionPaymentDetail.Question.User.Email;
            marketingCampaignViewModel.QuestionSubjects = marketingCampaign.QuestionPaymentDetail.Question.Subjects.Select(s => s.SubjectName).ToList();
            marketingCampaignViewModel.SearchKeywords = marketingCampaign.SearchKeywords.ToList();
            if (marketingCampaign.User != null)
            {
                marketingCampaignViewModel.CampaignManagerUserName = marketingCampaign.User.UserName;
                marketingCampaignViewModel.CampaignManagerId = (int)marketingCampaign.CampaignManagerId;
            }
            else
            {
                marketingCampaignViewModel.CampaignManagerUserName = null;
                marketingCampaignViewModel.CampaignManagerId = 0;
            }
            marketingCampaignViewModel.CreatedOn = marketingCampaign.CreatedOn;
            marketingCampaignViewModel.CreatedOnString = marketingCampaign.CreatedOn.ToString();
            marketingCampaignViewModel.Status = marketingCampaign.Status.DisplayName;
            marketingCampaignViewModel.StatusId = (int)marketingCampaign.StatusId;
            marketingCampaignViewModel.RowVersion = marketingCampaign.RowVersion;

            return marketingCampaignViewModel;
        }
    }
}
