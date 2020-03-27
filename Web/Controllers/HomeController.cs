using Domain.Constants;
using PagedList;
using Repository.SQL;
using System.Reflection;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index(int? page = 1)
        {
            ViewBag.MethodName = MethodBase.GetCurrentMethod().Name;
            var questions = new QuestionRepository().GetTopQuestions(Size.NumberOfQuestionsToDisplayOnTheMainPage);
            int pageNumber = (page ?? 1);
            return View(questions.ToPagedList(pageNumber, Size.QuestionPagerPageSize));
        }

        [Route("robots.txt")]
        public ActionResult Robots()
        {
            Response.ContentType = "text/plain";
            return View();
        }
    }
}