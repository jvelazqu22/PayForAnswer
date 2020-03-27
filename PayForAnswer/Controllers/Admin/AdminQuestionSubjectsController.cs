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
    public class AdminQuestionSubjectsController : Controller
    {
        private PfaDb db = new PfaDb();

        //
        // GET: /AdminQuestionSubjects/

        public ActionResult Index()
        {
            var questionsubjects = db.QuestionSubjects.Include(q => q.Question).Include(q => q.Subject);
            return View(questionsubjects.ToList());
        }

        //
        // GET: /AdminQuestionSubjects/Details/5

        public ActionResult Details(long id = 0)
        {
            QuestionSubject questionsubject = db.QuestionSubjects.Find(id);
            if (questionsubject == null)
            {
                return HttpNotFound();
            }
            return View(questionsubject);
        }

        //
        // GET: /AdminQuestionSubjects/Create

        public ActionResult Create()
        {
            ViewBag.QuestionId = new SelectList(db.Questions, "Id", "Title");
            ViewBag.SubjectName = new SelectList(db.Subjects, "Id", "SubjectName");
            return View();
        }

        //
        // POST: /AdminQuestionSubjects/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(QuestionSubject questionsubject)
        {
            if (ModelState.IsValid)
            {
                db.QuestionSubjects.Add(questionsubject);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.QuestionId = new SelectList(db.Questions, "Id", "Title", questionsubject.QuestionId);
            ViewBag.SubjectName = new SelectList(db.Subjects, "Id", "SubjectName", questionsubject.SubjectName);
            return View(questionsubject);
        }

        //
        // GET: /AdminQuestionSubjects/Edit/5

        public ActionResult Edit(long id = 0)
        {
            QuestionSubject questionsubject = db.QuestionSubjects.Find(id);
            if (questionsubject == null)
            {
                return HttpNotFound();
            }
            ViewBag.QuestionId = new SelectList(db.Questions, "Id", "Title", questionsubject.QuestionId);
            ViewBag.SubjectName = new SelectList(db.Subjects, "Id", "SubjectName", questionsubject.SubjectName);
            return View(questionsubject);
        }

        //
        // POST: /AdminQuestionSubjects/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(QuestionSubject questionsubject)
        {
            if (ModelState.IsValid)
            {
                db.Entry(questionsubject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.QuestionId = new SelectList(db.Questions, "Id", "Title", questionsubject.QuestionId);
            ViewBag.SubjectName = new SelectList(db.Subjects, "Id", "SubjectName", questionsubject.SubjectName);
            return View(questionsubject);
        }

        //
        // GET: /AdminQuestionSubjects/Delete/5

        public ActionResult Delete(long id = 0)
        {
            QuestionSubject questionsubject = db.QuestionSubjects.Find(id);
            if (questionsubject == null)
            {
                return HttpNotFound();
            }
            return View(questionsubject);
        }

        //
        // POST: /AdminQuestionSubjects/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            QuestionSubject questionsubject = db.QuestionSubjects.Find(id);
            db.QuestionSubjects.Remove(questionsubject);
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