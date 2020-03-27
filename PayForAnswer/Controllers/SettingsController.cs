using BusinessRules;
using Domain.App_GlobalResources;
using Domain.Constants;
using Domain.Exceptions;
using Domain.Models;
using Domain.Models.Account;
using Domain.Models.Entities;
using log4net;
using Repository.SQL;
using System;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace PayForAnswer.Controllers
{
    public class SettingsController : BootstrapBaseController
    {
        private PfaDb db = new PfaDb();
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult ENotifications()
        {
            UserProfile userProfileModel = null;
            try
            {
                int userId = WebSecurity.CurrentUserId;
                userProfileModel = db.UserProfiles.Find(userId);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                ModelState.AddModelError("", CommonResources.ErrorMsgException);
            }

            return View(userProfileModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ENotifications(UserProfile newUserProfileModel)
        {
            try
            {
                UserProfile userProfileModel = db.UserProfiles.Find(newUserProfileModel.Id);
                userProfileModel.NewQuestionRelatedToMySubjectsNotification = newUserProfileModel.NewQuestionRelatedToMySubjectsNotification;
                userProfileModel.QuestionStatusChangeWithMyAnswers = newUserProfileModel.QuestionStatusChangeWithMyAnswers;
                userProfileModel.ReplyToMyQuestionNotification = newUserProfileModel.ReplyToMyQuestionNotification;

                db.Entry(userProfileModel).State = EntityState.Modified;
                db.SaveChanges();
                Success(CommonResources.EmailPreferencesSuccessMsg);
                return View(userProfileModel);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                ModelState.AddModelError("", CommonResources.ErrorMsgException);
            }

            return RedirectToAction("ENotifications", "Settings");
        }

        [Authorize]
        public ActionResult ConfirmEmail(string id)
        {
            UserProfile userProfileModel = null;
            try
            {
                if (User.Identity.Name == id)
                {
                    int userId = WebSecurity.GetUserId(id);
                    userProfileModel = db.UserProfiles.Find(userId);

                    if (userProfileModel.NewEmail == userProfileModel.Email)
                    {
                        Error(CommonResources.MsgErrorNewEmailAddressEqualsCurrent);
                        return RedirectToAction("MyProfile", "Settings");
                    }

                    userProfileModel.Email = userProfileModel.NewEmail;

                    db.Entry(userProfileModel).State = EntityState.Modified;
                    db.SaveChanges();
                    Success(CommonResources.EmailUpdatedSuccessMsg);

                    return RedirectToAction("MyProfile", "Settings");
                }
                else
                {
                    throw new Exception(MethodBase.GetCurrentMethod().Name + Errors.SELF_CREATED_EXCEPTION_MESSAGE_SUFIX);
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                ModelState.AddModelError("", CommonResources.ErrorMsgException);
            }

            return RedirectToAction("MyProfile", "Settings");
        }

        [Authorize]
        public ActionResult MyProfile()
        {
            UserProfile userProfileModel = null;
            try
            {
                int userId = WebSecurity.CurrentUserId;
                userProfileModel = db.UserProfiles.Find(userId);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                ModelState.AddModelError("", CommonResources.ErrorMsgException);
            }

            return View(userProfileModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyProfile(int UserId, string EmailAddress, string NewEmailAddress)
        {
            try
            {
                if (string.IsNullOrEmpty(NewEmailAddress))
                {
                    Error(CommonResources.MsgErrorEmailAddressIsEmpty);
                    return RedirectToAction("MyProfile", "Settings");
                }

                if (EmailAddress == NewEmailAddress)
                {
                    Error(CommonResources.MsgErrorNewEmailAddressEqualsCurrent);
                    return RedirectToAction("MyProfile", "Settings");
                }

                UserProfile userProfileModel = db.UserProfiles.Find(UserId);
                userProfileModel.NewEmail = NewEmailAddress;

                db.Entry(userProfileModel).State = EntityState.Modified;
                db.SaveChanges();
                EmailBR emailBR = new EmailBR();
                emailBR.SendChangeEmailNotification(userProfileModel);
                Attention(CommonResources.EmailUpdateConfirmationEmailSent + userProfileModel.NewEmail);
                return View(userProfileModel);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                ModelState.AddModelError("", CommonResources.ErrorMsgException);
            }

            return RedirectToAction("MyProfile", "Settings");
        }

        [AllowAnonymous]
        public ActionResult RequestPwdReset()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult RequestPwdReset(RegisterModel registerModel)
        {
            try
            {
                //TODO: if there is more than one user with the same email address, the user will get an error
                UserProfile userModel = (UserProfile)db.UserProfiles.FirstOrDefault(u => u.Email == registerModel.EmailAddress);

                if (userModel == null) 
                    throw new EmailAddressNotFoundException();
                
                string token = WebSecurity.GeneratePasswordResetToken(userModel.UserName, Size.TokenExpirationInMinutesFromNow);
                userModel.ChangePasswordToken = HttpUtility.UrlEncode(token);

                EmailBR emailBR = new EmailBR();
                emailBR.SendChangePasswordTokenEmail(userModel);
                Attention(String.Format(CommonResources.MsgAttentionPwdResetEmailSent, registerModel.EmailAddress));

                return RedirectToAction("RequestPwdReset", "Settings");
            }
            catch (EmailAddressNotFoundException)
            {
                ModelState.AddModelError("", CommonResources.ErrorMsgEmailAddressNotFound);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                ModelState.AddModelError("", CommonResources.ErrorMsgException);
            }

            // If we got this far, something failed, redisplay form
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPwd(string id)
        {

            if (string.IsNullOrEmpty(id))
            {
                Error(CommonResources.ResetPwdTokenMissing);
                return RedirectToAction("RequestPwdReset", "Settings");
            }

            ViewBag.Token = id;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPwd(string token, LocalPasswordModel localPasswordModel)
        {
            try
            {
                //TODO: This method will to execute more than one time with the same token
                // eventhoguh the pwd only gets reset the first time.
                WebSecurity.ResetPassword(token, localPasswordModel.ConfirmPassword);
                Success(CommonResources.MsgSuccessPwdChanged);

                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                ModelState.AddModelError("", CommonResources.MsgExceptionResetPwd);
            }

            // If we got this far, something failed, redisplay form
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (db != null)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}
