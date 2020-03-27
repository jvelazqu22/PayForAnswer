using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Constants;

namespace PayForAnswer.Controllers
{
    [Authorize(Roles = Role.Admin)]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Alert()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Alert(string txAlert, string txAlertType)
        {
            HttpContext.Application["topAlert"] = txAlert;
            HttpContext.Application["topAlertType"] = txAlertType;
            return RedirectToAction("Alert");
        }
    }
}
