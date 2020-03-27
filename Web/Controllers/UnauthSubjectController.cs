using BusinessRules;
using Domain.App_GlobalResources;
using Domain.Models.Helper;
using Domain.Models.ViewModel;
using ErrorChecking;
using Repository.SQL;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class UnauthSubjectController : BaseController
    {
        public ActionResult BySubject(int? page = 1)
        {
            List<string> tempSubjectList = Session["SubjectList"] != null ? (List<string>)Session["SubjectList"] : new List<string>();
            if(tempSubjectList.Count == 0)
            {
                tempSubjectList = new SubjectsErrorChecking().GetSanatizedSubjectNamesList(CommonResources.UnAuthDefaultSubjects);
                Session["SubjectList"] = tempSubjectList;
            }

            QuestionsBySubjectModel questionsBySubjectModel = new SubjectBR().GetUnRegUserSubjectsAndQuestionsRelatedToSubjects(tempSubjectList, new QuestionSubjectRepository());
            ViewBag.MethodName = MethodBase.GetCurrentMethod().Name;
            questionsBySubjectModel.PageNumber = (page ?? 1);
            return View(questionsBySubjectModel);
        }

        public ActionResult UpdateSubjects(string commaDelimitedSubjects = null)
        {
            List<string> tempSubjectList = Session["SubjectList"] != null ? (List<string>)Session["SubjectList"] : new List<string>();

            List<string> cleanList = new List<string>();
            Error error = new SubjectsErrorChecking().IsUnRegUserSubjectListValid(commaDelimitedSubjects, ref cleanList);
            if (error.ErrorFound)
            {
                Danger(error.Message, true);
                Session["SubjectList"] = tempSubjectList;
            }
            else if (cleanList.Count > 0)
            {
                Session["SubjectList"] = cleanList;
            }
            return Json(new { url = Url.Action("BySubject", "UnauthSubject") });
        }
    }
}
