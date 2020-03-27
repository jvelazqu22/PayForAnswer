using System.Linq;
using System.Web.Mvc;
using PayForAnswer.DAL;
using System.Data;
using System.Data.Entity;

namespace PayForAnswer.Controllers
{
    public class QuestionsController : Controller
    {
        //
        // GET: /Questions/
        private PfaDb db = new PfaDb();

        public ActionResult Index()
        {
            var questions = db.Questions.Include(q => q.Status);
            return View(questions.ToList());
        }

        public ActionResult AllQuestions()
        {
            ViewBag.Message = "All Questions";
            return View("DisplayQuestions");
        }

        public ActionResult MyQuestions()
        {
            ViewBag.Message = "My Questions";
            return View("DisplayQuestions");
        }

        public ActionResult MyAnswers()
        {
            ViewBag.Message = "My Answers";
            return View("DisplayQuestions");
        }

        public ActionResult BySubject()
        {
            ViewBag.Message = "By Subject";
            return View("DisplayQuestions");
        }
    }
}
