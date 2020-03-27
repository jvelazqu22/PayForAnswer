using Domain.App_GlobalResources;
using Domain.Models.Entities;
using Domain.Models.ViewModel;
using Repository.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace PayForAnswer.Controllers
{
    //[Authorize]
    public class RegistrationController : BootstrapBaseController
    {
        
        PfaDb _pfaDb= new PfaDb();

        [HttpGet]
        public ActionResult Autocomplete(string term)
        {
            var model =
                _pfaDb.Subjects
                        .Where(s => s.SubjectName.StartsWith(term))
                        .Take(10)
                        .Select(s => new
                        {
                            s.SubjectName
                        });

            //List<string> results = new SubjectEntityTableRepository().GetTopSubjectMatches(term);
            List<string> results = new List<string>();
            foreach (var item in model.ToList())
                results.Add(item.SubjectName);

            return new JsonResult()
            {
                Data = results.ToArray(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        //
        //// GET: /Registration/
        [Authorize]
        public ActionResult Subjects()
        {
            var model = new List<SubjectListViewModel>();
            return View(model);
        }


        //
        // GET: /Registration/
        [Authorize]
        [HttpPost]
        public ActionResult Subjects(string searchTerm = null)
        {
            searchTerm = searchTerm.ToLower();
            UserProfile user = _pfaDb.UserProfiles.Find(WebSecurity.CurrentUserId);
            //var questionTableRepository = new SubjectEntityTableRepository();
            Subject subject = string.IsNullOrEmpty(searchTerm) ? null : _pfaDb.Subjects.SingleOrDefault(s => s.SubjectName.Equals(searchTerm));
            //SubjectEntity subject = string.IsNullOrEmpty(searchTerm) ? null : questionTableRepository.GetSubject(searchTerm);

            if (subject != null && !string.IsNullOrEmpty(searchTerm))
            {
                //subject = new SubjectEntity() { PartitionKey = SubjectEntityValues.USER_SUBJECTS_PKEY, RowKey = searchTerm  };
                //questionTableRepository.InsertOrReplaceSubjectEntity(subject);
                user.Subjects.Add(subject);
                _pfaDb.SaveChanges();
            }
            else if (!string.IsNullOrEmpty(searchTerm))
            {
                //List<SubjectEntity> subjectEntities = new List<SubjectEntity>()
                //{
                //    new SubjectEntity() { PartitionKey = searchTerm, RowKey = SubjectEntityValues.SUBJECT_DEFINITION_ROW_KEY  },
                //    new SubjectEntity() { PartitionKey = string.Format(SubjectEntityValues.USER_SUBJECTS_PKEY, user.UserId), RowKey = searchTerm  },
                //};

                //questionTableRepository.InsertOrReplaceSubjectEntityList(subjectEntities);
                subject = new Subject { SubjectName = searchTerm };
                _pfaDb.Subjects.Add(subject);
                _pfaDb.SaveChanges();

                user.Subjects.Add(subject);
                _pfaDb.SaveChanges();
            }


            //var model = questionTableRepository.GetEntitySubjects(string.Format(SubjectEntityValues.USER_SUBJECTS_PKEY, user.UserId))
            var model = user.Subjects
            .Select(s => new SubjectListViewModel
            {
                Id = s.Id,
                SubjectName = s.SubjectName
            });


            if (Request.IsAjaxRequest())
            {
                return PartialView("_Subjects", model);
            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ConfirmRegistration(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                if (WebSecurity.ConfirmAccount(id))
                {
                    Success(CommonResources.MsgInfoRegConfirmation);
                    return RedirectToAction("Subjects", "Registration");
                }
                //TODO: review how to handle errors here: http://www.mikepope.com/blog/AddComment.aspx?blogid=2267
                else
                {
                    Error(CommonResources.MsgErrorRegistration);
                }
            }
            else
            {
                Error(CommonResources.MsgErrorRegComfirmationCodeMissing);
                return RedirectToAction("Register", "Account");
            }

            ViewBag.Token = id;
            return View();
        }

        [HttpGet]
        public ActionResult RegistrationEmailSent(string emailAddress)
        {
            ViewBag.EmailAddress = emailAddress;
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (_pfaDb != null)
            {
                _pfaDb.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
