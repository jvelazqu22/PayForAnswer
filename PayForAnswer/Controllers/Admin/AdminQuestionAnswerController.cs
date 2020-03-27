using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Models;
using PayForAnswer.DAL;

namespace PayForAnswer.Controllers.Admin
{
    public class AdminQuestionAnswerController : Controller
    {
        private PfaDb db = new PfaDb();

        //
        // GET: /AdminQuestionAnswer/

        public ActionResult Index()
        {
            var questionanswers = db.QuestionAnswers.Include(q => q.Answer).Include(q => q.Question);
            return View(questionanswers.ToList());
        }

        //
        // GET: /AdminQuestionAnswer/Details/5

        public ActionResult Details(int id = 0)
        {
            QuestionAnswer questionanswer = db.QuestionAnswers.Find(id);
            if (questionanswer == null)
            {
                return HttpNotFound();
            }
            return View(questionanswer);
        }

        //
        // GET: /AdminQuestionAnswer/Create

        public ActionResult Create()
        {
            ViewBag.AnswerId = new SelectList(db.Answers, "Id", "Description");
            ViewBag.QuestionId = new SelectList(db.Questions, "Id", "Title");
            return View();
        }

        //
        // POST: /AdminQuestionAnswer/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(QuestionAnswer questionanswer)
        {
            if (ModelState.IsValid)
            {
                db.QuestionAnswers.Add(questionanswer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AnswerId = new SelectList(db.Answers, "Id", "Description", questionanswer.AnswerId);
            ViewBag.QuestionId = new SelectList(db.Questions, "Id", "Title", questionanswer.QuestionId);
            return View(questionanswer);
        }

        //
        // GET: /AdminQuestionAnswer/Edit/5

        public ActionResult Edit(int id = 0)
        {
            QuestionAnswer questionanswer = db.QuestionAnswers.Find(id);
            if (questionanswer == null)
            {
                return HttpNotFound();
            }
            ViewBag.AnswerId = new SelectList(db.Answers, "Id", "Description", questionanswer.AnswerId);
            ViewBag.QuestionId = new SelectList(db.Questions, "Id", "Title", questionanswer.QuestionId);
            return View(questionanswer);
        }

        //
        // POST: /AdminQuestionAnswer/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(QuestionAnswer questionanswer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(questionanswer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AnswerId = new SelectList(db.Answers, "Id", "Description", questionanswer.AnswerId);
            ViewBag.QuestionId = new SelectList(db.Questions, "Id", "Title", questionanswer.QuestionId);
            return View(questionanswer);
        }

        //
        // GET: /AdminQuestionAnswer/Delete/5

        public ActionResult Delete(int id = 0)
        {
            QuestionAnswer questionanswer = db.QuestionAnswers.Find(id);
            if (questionanswer == null)
            {
                return HttpNotFound();
            }
            return View(questionanswer);
        }

        //
        // POST: /AdminQuestionAnswer/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            QuestionAnswer questionanswer = db.QuestionAnswers.Find(id);
            db.QuestionAnswers.Remove(questionanswer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}