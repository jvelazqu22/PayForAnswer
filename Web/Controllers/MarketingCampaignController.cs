using BusinessRules;
using Domain.App_GlobalResources;
using Domain.Constants;
using Domain.Exceptions;
using Domain.Models.Entities;
using Domain.Models.Entities.Identity;
using Domain.Models.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PagedList;
using Repository.Interfaces;
using Repository.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Utilities;

namespace Web.Controllers
{
    public class MarketingCampaignController : BaseController
    {
        public MarketingCampaignController() {}

        public MarketingCampaignController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        [Authorize(Roles = Role.CommaSeparateCMandAdminRoles)]
        public ActionResult GetQuestionHistory()
        {
            return View();
        }

        [Authorize]
        public ActionResult QuestionHistory(Guid? questionID, int? page = 1)
        {
            if(questionID == null)
            {
                Danger("Invalid question id", true);
                return View("GetQuestionHistory");
            }

            int userID = User.Identity.GetUserId<int>();
            var qRepository = new QuestionRepository();
            var userBR = new UserBR();
            bool didUserPostQuestion = userBR.DidUserPostQuestion(userID, (Guid)questionID, qRepository);
            qRepository.Dispose();
            List<string> userRoles = UserManager.GetRoles<ApplicationUser, int>(userID).ToList();
            bool isUserInRoles = userBR.IsUserInAllowedRoles(userRoles, Role.CmAndAdminRolesList);
            if (didUserPostQuestion || isUserInRoles)
            {
                List<QuestionMarketingHistoryViewModel> questionMarketingHistoryViewModelList = new List<QuestionMarketingHistoryViewModel>();
                ViewBag.MethodName = MethodBase.GetCurrentMethod().Name;
                int pageNumber = (page ?? 1);

                using (var paymentRepository = new PaymentRepository())
                    questionMarketingHistoryViewModelList = new MarketingBR().GetQuestionMarketingHistoryViewModelList((Guid)questionID, paymentRepository, userID);
                return View(questionMarketingHistoryViewModelList.ToPagedList(pageNumber, Size.MarketingCampaignsPageSize));
            }
            else
            {
                Danger(CommonResources.QuestionHistPermissionsErrorMsg, true);
                InvalidUserException ex = new InvalidUserException("Question ID: " + questionID + " currentUserId: " + userID);
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return View("Error");
            }
        }


        [Authorize(Roles = Role.CommaSeparateCMandAdminRoles)]
        public ActionResult NotStartedList(int? page = 1)
        {
            List<MarketingCampaignViewModel> marketingCampaignViewModelList = new List<MarketingCampaignViewModel>();
            ViewBag.MethodName = MethodBase.GetCurrentMethod().Name;

            using (var marketingRepository = new MarketingRepository())
                marketingCampaignViewModelList = new MarketingBR()
                    .GetMarketingCampaignViewModelList(
                        marketingRepository.GetMarketingCampaignsListByStatus(StatusList.CAMPAIGN_STARTED_AND_MANAGER_ASSIGEND_STATUS));

            int pageNumber = (page ?? 1);
            return View(marketingCampaignViewModelList.ToPagedList(pageNumber, Size.MarketingCampaignsPageSize));
        }

        [Authorize(Roles = Role.CommaSeparateCMandAdminRoles)]
        public ActionResult MostRecentStartedMarketingCampaigns(int? page = 1)
        {
            ViewBag.MethodName = MethodBase.GetCurrentMethod().Name;
            int pageNumber = (page ?? 1);
            List<MarketingCampaignViewModel> marketingCampaignViewModelList = new List<MarketingCampaignViewModel>();
            using (var marketingRepository = new MarketingRepository())
                marketingCampaignViewModelList = new MarketingBR()
                    .GetMarketingCampaignViewModelList(
                        marketingRepository.GetTopMostRecentStartedMarketingCampaignsList(CampaignStatus.CampaignStarted, Size.TopStartedMarketingCampaigns));

            return View(marketingCampaignViewModelList.ToPagedList(pageNumber, Size.MarketingCampaignsPageSize));
        }

        //
        // GET: /MarketingCampaign/Edit/5

        [Authorize(Roles = Role.CommaSeparateCMandAdminRoles)]
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
        [Authorize(Roles = Role.CommaSeparateCMandAdminRoles)]
        public ActionResult Edit(MarketingCampaignViewModel marketingCampaignViewModel)
        {
            ApplicationUser user = new ApplicationUser() { Id = User.Identity.GetUserId<int>(), UserName = User.Identity.GetUserName() };
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

                Danger("The record you attempted to edit "
                    + "was modified by another user after you got the original value. The "
                    + "edit operation was canceled and the current values in the database "
                    + "have been displayed. If you still want to edit this record, click "
                    + "the Save button again. Otherwise click the Back to List hyperlink.");

                marketingCampaignViewModel.RowVersion = databaseValues.RowVersion;
                using (var paymentRepository = new PaymentRepository())
                    marketingCampaignViewModel.QuestionTitle = paymentRepository.GetPaymentDetailByID(databaseValues.QuestionPaymentDetailID).Question.Title;
            }
            catch (DataException ex)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            ViewBag.StatusId = new SelectList(marketingRepository.GetCampaignStatus(),
                "Id", "DisplayName", marketingCampaignViewModel.StatusId);

            return View(marketingCampaignViewModel);
        }
    }
}