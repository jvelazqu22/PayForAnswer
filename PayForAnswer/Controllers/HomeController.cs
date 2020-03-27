using Domain.Constants;
using PagedList;
using Repository.SQL;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace PayForAnswer.Controllers
{
    public class HomeController : BootstrapBaseController
    {
        private PfaDb db = new PfaDb();

        public ActionResult Index(int? page = 1)
        {
            ViewBag.MethodName = MethodBase.GetCurrentMethod().Name;
            //using (var db = new PfaDb())
            {
                var questions = db.Questions
                    .OrderByDescending(q => q.Amount)
                    .Where(q => q.StatusId == StatusValues.PayPalRedirectConfirmed || q.StatusId == StatusValues.PayPalIPNNotifyConfirmed)
                    .Where(q => !q.Answers.Select(a => a.StatusId == StatusValues.Accepted || a.StatusId == StatusValues.Paid).Contains(true))
                    .Include(q => q.Status)
                    .Take(Size.NumberOfQuestionsToDisplayOnTheMainPage);

                int pageNumber = (page ?? 1);
                return View(questions.ToPagedList(pageNumber, Size.QuestionPagerPageSize));
            }
        }

        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult Robots()
        {
            Response.ContentType = "text/plain";
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
