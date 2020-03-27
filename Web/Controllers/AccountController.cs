using BusinessRules;
using CaptchaMvc.HtmlHelpers;
using Domain.App_GlobalResources;
using Domain.Constants;
using Domain.Models.Account;
using Domain.Models.Entities;
using Domain.Models.Entities.Identity;
using Domain.Models.Helper;
using ErrorChecking;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Utilities;


namespace Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private ApplicationUserManager _userManager;

        public AccountController() { }

        public AccountController(ApplicationUserManager userManager)
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

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                // find user by username first
                var user = await UserManager.FindByNameAsync(model.UserName);
                
                if(user == null)
                {
                    user = await UserManager.FindByEmailAsync(model.UserName);
                    if (user == null)
                    {
                        Danger(CommonResources.InvalidLogInTryAgainErrorMsg, true);
                        return View(model);
                    }
                }

                if (!user.EmailConfirmed)
                {
                    Danger(string.Format(CommonResources.MsgInfoCompleteRegistration, user.Email), true);
                    return View(model);
                }

                var validCredentials = await UserManager.FindAsync(user.UserName, model.Password);

                // When a user is lockedout, this check is done to ensure that even if the credentials are valid
                // the user can not login until the lockout duration has passed
                if (await UserManager.IsLockedOutAsync(user.Id))
                    Danger(string.Format(CommonResources.AccountLockedErrorMsg, General.LockingPeriodInMinutes), true);

                // if user is subject to lockouts and the credentials are invalid
                // record the failure and check if user is lockedout and display message, otherwise,
                // display the number of attempts remaining before lockout
                else if (await UserManager.GetLockoutEnabledAsync(user.Id) && validCredentials == null)
                {
                    // Record the failure which also may cause the user to be locked out
                    await UserManager.AccessFailedAsync(user.Id);
                    string message;

                    if (await UserManager.IsLockedOutAsync(user.Id))
                        message = string.Format(CommonResources.AccountLockedErrorMsg, General.LockingPeriodInMinutes);
                    else
                    {
                        int accessFailedCount = await UserManager.GetAccessFailedCountAsync(user.Id);
                        int attemptsLeft = General.MaxFailedAccessAttemptsBeforeLockout - accessFailedCount;
                        message = string.Format(CommonResources.InvalidLogInLeftAttemptsErrorMsg, attemptsLeft);
                    }
                    Danger(message, true);
                }
                else if (validCredentials == null)
                    Danger(CommonResources.InvalidLogInTryAgainErrorMsg, true);
                else
                {
                    await SignInAsync(user, model.RememberMe);
                    // When token is verified correctly, clear the access failed count used for lockout
                    await UserManager.ResetAccessFailedCountAsync(user.Id);
                    return RedirectToLocal(returnUrl);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            //if (!this.IsCaptchaValid("Invalid key value"))
            if (!this.IsCaptchaValid(CommonResources.InvalidSecurityKey))
            {
                Danger(CommonResources.InvalidSecurityKey, true);
                return View(model);
            }

            Error error = new UserErrorChecking().AreThereAnyRegistrationErrors(model);
            if (error.ErrorFound)
            {
                Danger(error.Message, true);
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                Danger(CommonResources.ErrorMsgException, true);
                return View(model);
            }

            var user = new ApplicationUser()
            {
                UserName = model.UserName,
                Email = model.EmailAddress,
                Notifications = new Notifications(),
                NewEmail = "",
                FullName = null,
                IsFemale = null,
                DoB = new DateTime(model.Year, model.Month, model.Day),
                AcceptedTermsConditionsAndPrivacyPolicy = model.TermsAndConditionsAccepted
            };
            IdentityResult result = await UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                string confirmationToken = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                var callbackUrl = Url.Action("ConfirmRegistration", "Registration", new { userId = user.Id, token = confirmationToken }, protocol: Request.Url.Scheme);
                var confirmationLink = String.Format(Html.LINK_PLACE_HOLDER, callbackUrl, CommonResources.lnkTxtConfirmReg);
                var subject = String.Format(CommonResources.EmailSubjectConfirmAcctReg, model.UserName);
                var emailLink = string.Format(Html.EMAIL_LINK_PLACE_HOLDER, Emails.SUPPORT_EMAIL_ADDRESS, Emails.SUPPORT_EMAIL_ADDRESS);
                var addUsToYourAddressBookMsg = string.Format(CommonResources.AddUsToYourAddressBookMsg, emailLink);
                
                var param = new object[3] { confirmationLink, callbackUrl, addUsToYourAddressBookMsg };
                var body = String.Format(CommonResources.EmailBodyConfirmAcctReg, param);

                EmailBR emailBR = new EmailBR();
                model.RegistrationToken = HttpUtility.UrlEncode(confirmationToken);
                emailBR.SendEmail(model.EmailAddress, subject, body);

                if (new UserBR().IsUserEmailAddressFromMicrosoft(model.EmailAddress))
                    Warning(CommonResources.RegistrationInJunkFolderMsg, true);

                string msg = string.Format(CommonResources.MsgAttentionRegEmailSent, model.EmailAddress, Emails.SUPPORT_EMAIL_ADDRESS);
                return RedirectToAction("ConfirmationEmailSent", "Registration", new { confirmationMsg = msg });
            }
            else
                result.Errors.ToList().ForEach(e => Danger(e, true));

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(int userId, string code)
        {
            if (userId > 0 || code == null) 
            {
                return View("Error");
            }

            IdentityResult result = await UserManager.ConfirmEmailAsync(userId, code);
            if (result.Succeeded)
            {
                return View("ConfirmEmail");
            }
            else
            {
                AddErrors(result);
                return View();
            }
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotUserNameOrPasswordViewModel model)
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

                string confirmationToken = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { code = confirmationToken }, protocol: Request.Url.Scheme);
                string confirmationLink = String.Format("<a href=\"{0}\">{1} </a>", callbackUrl, CommonResources.lnkTxtConfirmResetPwd);
                var subject = CommonResources.EmailSubjectResetPwd;
                var body = String.Format(CommonResources.EmailBodyResetPwd, user.UserName, confirmationLink, callbackUrl);

                EmailBR emailBR = new EmailBR();
                emailBR.SendEmail(user.Email, subject, body);
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

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
	
        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            if (code == null) 
            {
                Danger(CommonResources.ResetPwdTokenMissing, true);
                return View("Error");
            }
            return View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    Danger(CommonResources.EmailAddressNotFound, true);
                    return View();
                }
                IdentityResult result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
                if (result.Succeeded)
                {
                    Success(CommonResources.MsgSuccessPwdChanged, true);
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    AddErrors(result);
                    return View();
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId<int>(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
                await SignInAsync(user, isPersistent: false);
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
            if (message == ManageMessageId.ChangePasswordSuccess)
                Success(CommonResources.MsgSuccessPwdChanged,true);
            else if (message == ManageMessageId.SetPasswordSuccess)
                Success(CommonResources.PasswordHasBeenSetMsg, true);
            else if (message == ManageMessageId.RemoveLoginSuccess)
                Success(CommonResources.ExternalLoginRemovedMsg, true);
            else if (message == ManageMessageId.Error)
                Danger(CommonResources.ErrorMsgException, true);

            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId<int>(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId<int>(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId<int>(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };
                IdentityResult result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        
                        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                        // Send an email with this link
                        // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        // SendEmail(user.Email, callbackUrl, "Confirm your account", "Please confirm your account by clicking this link");
                        
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId<int>());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
                Danger(error, true);

            //foreach (var error in result.Errors)
            //    ModelState.AddModelError("", error);
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId<int>());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private void SendEmail(string email, string callbackUrl, string subject, string message)
        {
            // For information on sending mail, please visit http://go.microsoft.com/fwlink/?LinkID=320771
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}