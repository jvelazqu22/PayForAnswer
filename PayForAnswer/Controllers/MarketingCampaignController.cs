using BusinessRules;
using Domain.Models.Entities;
using Domain.Models.ViewModel;
using Repository.Interfaces;
using Repository.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace PayForAnswer.Controllers.Admin
{
    //[Authorize(Roles = Role.Admin)]
    public class MarketingCampaignController : BootstrapBaseController
    {
        //
        // GET: /MarketingCampaign/
        [Authorize]
        public ActionResult MyHistory(Guid id)
        {
            List<QuestionMarketingHistoryViewModel> questionMarketingHistoryViewModelList = new List<QuestionMarketingHistoryViewModel>();
            using (var paymentRepository = new PaymentRepository())
                questionMarketingHistoryViewModelList = new MarketingBR().GetQuestionMarketingHistoryViewModelList(id, paymentRepository, WebSecurity.CurrentUserId);
            return View(questionMarketingHistoryViewModelList.ToList());
        }


        public ActionResult NotStartedList()
        {
            List<MarketingCampaignViewModel> marketingCampaignViewModelList = new List<MarketingCampaignViewModel>();

            using (var marketingRepository = new MarketingRepository())
                marketingCampaignViewModelList = new MarketingBR().GetMarketingCampaignViewModelList(marketingRepository.GetMarketingCampaignsList());
            return View(marketingCampaignViewModelList);
        }

        public ActionResult MostRecentStartedMarketingCampaigns()
        {
            List<MarketingCampaignViewModel> marketingCampaignViewModelList = new List<MarketingCampaignViewModel>();
            using (var marketingRepository = new MarketingRepository())
                marketingCampaignViewModelList = new MarketingBR().GetMarketingCampaignViewModelList(marketingRepository.Get100MostRecentStartedMarketingCampaignsList());
            return View(marketingCampaignViewModelList);
        }

        //
        // GET: /MarketingCampaign/Edit/5

        public ActionResult Edit(long id = 0)
        {
            MarketingCampaignViewModel marketingCampaignViewModel = new MarketingCampaignViewModel();
            var marketingRepository = new MarketingRepository();
            marketingCampaignViewModel = new MarketingBR().GetMarketingCampaignViewModelById(marketingRepository, id);

            ViewBag.StatusId = new SelectList(marketingRepository.GetCampaignStatus(),
                "Id", "DisplayName", marketingCampaignViewModel.StatusId);

            return View(marketingCampaignViewModel);
        }

        //
        // POST: /MarketingCampaign/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MarketingCampaignViewModel marketingCampaignViewModel)
        {
            UserProfile user = new UserProfile() { Id = WebSecurity.CurrentUserId, UserName = WebSecurity.CurrentUserName };
            IMarketingRepository marketingRepository = new MarketingRepository();
            try
            {
                if (ModelState.IsValid)
                {
                    new MarketingBR().UpdateMarketingStatusModel(marketingCampaignViewModel, user, marketingRepository);
                    return RedirectToAction("NotStartedList");
                }
                return View(marketingCampaignViewModel);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var clientValues = (MarketingCampaign)entry.Entity;
                var databaseValues = (MarketingCampaign)entry.GetDatabaseValues().ToObject();

                if (databaseValues.PerDayBudget != clientValues.PerDayBudget)
                    ModelState.AddModelError("PerDayBudget", "Current value: " + String.Format("{0:c}", databaseValues.PerDayBudget));
                if (databaseValues.NumberOfDaysToRun != clientValues.NumberOfDaysToRun)
                    ModelState.AddModelError("NumberOfDaysToRun", "Current value: " + String.Format("{0:c}", databaseValues.NumberOfDaysToRun));
                if (databaseValues.UsedBudget != clientValues.UsedBudget)
                    ModelState.AddModelError("UsedBudget", "Current value: " + String.Format("{0:c}", databaseValues.UsedBudget));
                if (databaseValues.StatusId != clientValues.StatusId)
                    ModelState.AddModelError("StatusId", "Current value: " + marketingRepository.GetCampaignStatusByID((int)databaseValues.StatusId).DisplayName);
                if (databaseValues.StartDate != clientValues.StartDate)
                    ModelState.AddModelError("StartDate", "Current value: " + String.Format("{0:d}", databaseValues.StartDate));
                if (databaseValues.EndDate != clientValues.EndDate)
                    ModelState.AddModelError("EndDate", "Current value: " + String.Format("{0:d}", databaseValues.EndDate));

                Error("The record you attempted to edit "
                    + "was modified by another user after you got the original value. The "
                    + "edit operation was canceled and the current values in the database "
                    + "have been displayed. If you still want to edit this record, click "
                    + "the Save button again. Otherwise click the Back to List hyperlink.");

                marketingCampaignViewModel.RowVersion = databaseValues.RowVersion;
                using(var paymentRepository = new PaymentRepository())
                marketingCampaignViewModel.QuestionTitle = paymentRepository.GetPaymentDetailByID(databaseValues.QuestionPaymentDetailID).Question.Title;
            }
            catch (DataException ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }

            ViewBag.StatusId = new SelectList(marketingRepository.GetCampaignStatus(),
                "Id", "DisplayName", marketingCampaignViewModel.StatusId);

            return View(marketingCampaignViewModel);
        }
    }
}