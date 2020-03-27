using Domain.Constants;
using Domain.Models.ViewModel;
using Domain.Models.Helper;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    [Authorize(Roles = Role.Admin)]
    public class AdminController : BaseController
    {
        //
        // GET: /Admin/

        public ActionResult Alert()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Alert(AlertViewModel model)
        {
            Alert alert = new Alert() { AlertStyle = model.AlertStyle, Message = model.Message, Dismissable = false }; 
            HttpContext.Application["TopAlertMsg"] = model.Message;
            HttpContext.Application["TopAlertType"] = model.AlertStyle;
            return RedirectToAction("Alert");
        }
    }
}