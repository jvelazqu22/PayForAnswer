using Domain.Constants;
using Domain.Models.Entities;
using Domain.Models.ViewModel;
using Repository.SQL;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace PayForAnswer.Controllers
{
    public class UnauthSubjectController : Controller
    {
        private PfaDb db = new PfaDb();

        public ActionResult BySubject(int? subjectId, int? page = 1)
        {
            List<string> tempSubjectList = Session["SubjectList"] != null ? (List<string>)Session["SubjectList"] : new List<string>();
            QuestionsBySubjectModel questionsBySubjectModel = new QuestionsBySubjectModel();

            if (subjectId != null)
            {
                Subject subject = db.Subjects.Find(subjectId);
                if (tempSubjectList != null && tempSubjectList.Contains(subject.SubjectName))
                {
                    tempSubjectList.Remove(subject.SubjectName);
                    Session["SubjectList"] = tempSubjectList;
                }
            }

            IQueryable<Subject> subjects = db.Subjects.Where(s => tempSubjectList.Contains(s.SubjectName));

            var questions = db.Questions
                .Include(q => q.Subjects)
                .Include(q => q.Answers)
                .Include(q => q.Status)
                .Where(q => q.StatusId == StatusValues.PayPalRedirectConfirmed || q.StatusId == StatusValues.PayPalIPNNotifyConfirmed)
                .Where(q => !q.Answers.Select(a => a.StatusId == StatusValues.Accepted || a.StatusId == StatusValues.Paid).Contains(true))
                .Where(q => q.Subjects.Intersect(subjects).Any())
                .OrderByDescending(q => q.Amount);

            questionsBySubjectModel.Subjects = subjects.ToList();
            questionsBySubjectModel.Questions = questions;

            ViewBag.MethodName = MethodBase.GetCurrentMethod().Name;
            questionsBySubjectModel.PageNumber = (page ?? 1);

            return View(questionsBySubjectModel);
        }

        public ActionResult Subjects(string searchTerm = null, int? page = 1)
        {
            List<string> tempSubjectList = Session["SubjectList"] != null ? (List<string>)Session["SubjectList"] : new List<string>();
            Subject subject = db.Subjects.SingleOrDefault(s => s.SubjectName.Equals(searchTerm));

            if (subject != null)
            {
                if (tempSubjectList != null && !tempSubjectList.Contains(subject.SubjectName) && !string.IsNullOrWhiteSpace(subject.SubjectName))
                {
                    tempSubjectList.Add(subject.SubjectName);
                    Session["SubjectList"] = tempSubjectList;
                }
            }
            else
            {
                subject = new Subject { SubjectName = searchTerm };
                db.Subjects.Add(subject);
                db.SaveChanges();
            }

            QuestionsBySubjectModel questionsBySubjectModel = new QuestionsBySubjectModel();
            IQueryable<Subject> subjects = db.Subjects.Where(s => tempSubjectList.Contains(s.SubjectName));

            var questions = db.Questions
                .Include(q => q.Subjects)
                .Include(q => q.Answers)
                .Include(q => q.Status)
                .Where(q => q.StatusId == StatusValues.PayPalRedirectConfirmed || q.StatusId == StatusValues.PayPalIPNNotifyConfirmed)
                .Where(q => !q.Answers.Select(a => a.StatusId == StatusValues.Accepted || a.StatusId == StatusValues.Paid).Contains(true))
                .Where(q => q.Subjects.Intersect(subjects).Any())
                .OrderByDescending(q => q.Amount);

            questionsBySubjectModel.Subjects = subjects.ToList();
            questionsBySubjectModel.Questions = questions;
            ViewBag.MethodName = MethodBase.GetCurrentMethod().Name;
            questionsBySubjectModel.PageNumber = (page ?? 1);

            if (Request.IsAjaxRequest())
                return PartialView("~/Views/Subject/_BySubject", questionsBySubjectModel);

            return View("BySubject", questionsBySubjectModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (db != null)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}
