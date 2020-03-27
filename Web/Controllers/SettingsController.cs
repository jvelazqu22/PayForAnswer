using BusinessRules;
using Domain.App_GlobalResources;
using Domain.Models.Account;
using Domain.Models.Entities.Identity;
using Domain.Models.Helper;
using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Utilities;

namespace Web.Controllers
{
    [Authorize]
    public class SettingsController : BaseController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ApplicationUserManager _userManager;

        public SettingsController() {}

        public SettingsController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<ActionResult> ENotifications()
        {
            ApplicationUser user = null;
            try
            {
                user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                Danger(CommonResources.ErrorMsgException,true);
            }

            return View(user);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ENotifications(ApplicationUser model)
        {
            try
            {
                ApplicationUser user = await UserManager.FindByIdAsync(model.Id);
                user.Notifications.NewQuestionRelatedToMySubjects = model.Notifications.NewQuestionRelatedToMySubjects;
                user.Notifications.NewAnswerToMyQuestion = model.Notifications.NewAnswerToMyQuestion;
                user.Notifications.NewComment = model.Notifications.NewComment;
                user.Notifications.QuestionStatusChangeWithMyAnswers = model.Notifications.QuestionStatusChangeWithMyAnswers;

                IdentityResult result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    Success(CommonResources.EmailPreferencesSuccessMsg, true);
                    return RedirectToAction("ENotifications", "Settings");
                }
                else
                    result.Errors.ToList().ForEach(e => Danger(e, true));

                return View(user);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                Danger(CommonResources.ErrorMsgException, true);
            }

            return RedirectToAction("ENotifications", "Settings");
        }

        [Authorize]
        public async Task<ActionResult> UserProfile()
        {
            ProfileViewModel model = null;
            try
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
                model = new ProfileViewModel() { UserID = user.Id, Email = user.Email, NewEmail = user.NewEmail, UserName = user.UserName };
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                Danger(CommonResources.ErrorMsgException, true);
                return View("Error");
            }

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserProfile(ProfileViewModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.NewEmail))
                {
                    Danger(CommonResources.MsgErrorEmailAddressIsEmpty, true);
                    return View(model);
                }
                if (UserManager.FindByEmail(model.NewEmail) != null)
                {
                    string msg = string.Format(CommonResources.TheNewEmailIsAlreadyTaken, model.NewEmail);
                    Danger(msg, true);
                    return View(model);
                }
                if (model.Email == model.NewEmail)
                {
                    Danger(CommonResources.NewEamilCannotBeTheSameAsCurrentMsg, true);
                    return View(model);
                }
                if (model.NewEmail != model.ConfirmNewEmail)
                {
                    string msg = string.Format(CommonResources.NewEmailAndConfirmNewEmailDoNotMatch, model.NewEmail, model.ConfirmNewEmail);
                    Danger(msg, true);
                    return View(model);
                }

                ApplicationUser user = await UserManager.FindByIdAsync(model.UserID);
                user.Email = model.NewEmail;

                IdentityResult result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    Success(CommonResources.EmailUpdatedSuccessMsg, true);
                    return RedirectToAction("UserProfile", "Settings" );
                }
                else
                    result.Errors.ToList().ForEach(e => Danger(e, true));

                return View(model);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                Danger(CommonResources.ErrorMsgException, true);
            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ForgotUserName()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotUserName(ForgotUserNameOrPasswordViewModel model)
        {
            try
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    Danger(CommonResources.ErrorMsgEmailAddressNotFound, true);
                    return View();
                }
                if (!(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    Danger(string.Format(CommonResources.MsgInfoCompleteRegistration, user.Email), true);
                    return View();
                }

                EmailBR emailBR = new EmailBR();
                EmailModel eModel = new EmailModel()
                {
                    From = CommonResources.FromAutoConfirmEmailAddress,
                    SenderName = CommonResources.PayForAnswer,
                    To = model.Email,
                    Subject = CommonResources.ForgotUserName,
                    Body = string.Format(CommonResources.ForgotUserNameEmailBody, user.UserName)
                };
                emailBR.Send(eModel);
                string msg = string.Format(CommonResources.MsgAttentionPwdResetEmailSent, model.Email);

                return RedirectToAction("ConfirmationEmailSent", "Registration", new { confirmationMsg = msg });
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                Danger(CommonResources.ErrorMsgException, true);
            }

            // If we got this far, something failed, redisplay form
            return View();
        }
    }
}
