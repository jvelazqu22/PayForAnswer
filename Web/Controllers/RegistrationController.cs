using BusinessRules;
using Domain.App_GlobalResources;
using Domain.Constants;
using Domain.Models.Helper;
using ErrorChecking;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Repository.SQL;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Utilities;

namespace Web.Controllers
{
    public class RegistrationController : BaseController
    {
        private ApplicationUserManager _userManager;

        public RegistrationController() {}

        public RegistrationController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }
        public ApplicationUserManager UserManager
        {
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
        //// GET: /Registration/
        [Authorize]
        public ActionResult Subjects()
        {
            return View();
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSubjects(string commaDelimitedSubjects = null)
        {
            List<string> cleanList = new List<string>();
            Error error = new SubjectsErrorChecking().IsUserSubjectListValid(commaDelimitedSubjects, ref cleanList);
            if (error.ErrorFound)
            {
                Danger(error.Message, true);
                return Json(new { url = Url.Action("Subjects", "Registration") });
            }
            else
            {
                new SubjectBR().AddUpdateUserSubjects(User.Identity.GetUserId<int>(), cleanList, new SubjectRepository());
                Success(CommonResources.SuccessRegistrationMsg, true);
                return Json(new { url = Url.Action("MySubjects", "Main") });
            }
        }

        [AllowAnonymous]
        public ActionResult ConfirmRegistration(int userId, string token)
        {
            if (userId > 0 && !String.IsNullOrEmpty(token))
            {
                IdentityResult result = UserManager.ConfirmEmail(userId, token);
                if (result.Succeeded)
                {
                    Success(CommonResources.MsgInfoRegConfirmation, true);
                    return RedirectToAction("Subjects", "Registration");
                }
                else
                    Danger(CommonResources.MsgErrorRegistration, true);
            }
            else
                Danger(CommonResources.MsgErrorRegComfirmationCodeMissing, true);

            ViewBag.Token = token;
            return RedirectToAction("Register", "Account");
        }

        [HttpGet]
        public ActionResult ConfirmationEmailSent(string confirmationMsg)
        {
            ViewBag.ConfirmationMessage = confirmationMsg;
            var emailLink = string.Format(Html.EMAIL_LINK_PLACE_HOLDER, Emails.SUPPORT_EMAIL_ADDRESS, Emails.SUPPORT_EMAIL_ADDRESS);
            ViewBag.ConfirmationMessage2 = string.Format(CommonResources.AddUsToYourAddressBookMsg, emailLink);
            return View();
        }
    }
}