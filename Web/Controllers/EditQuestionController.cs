using BusinessRules;
using Domain.App_GlobalResources;
using Domain.Constants;
using Domain.Exceptions;
using Domain.Models;
using Domain.Models.Entities;
using Domain.Models.Helper;
using Domain.Models.ViewModel;
using ErrorChecking;
using log4net;
using Microsoft.AspNet.Identity;
using Repository.Blob;
using Repository.Interfaces;
using Repository.SQL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace Web.Controllers
{
    [Authorize]
    public class EditQuestionController : BaseController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ActionResult Edit(Guid questionID)
        {
            UpdateQuestionViewModel questionDetailsModel =
                new QuestionBR().GetUpdateQuestionDetailsModel(questionID, new QuestionRepository(), new BlobRepository(), DateTime.UtcNow, User.Identity.GetUserId<int>());

            if (User.Identity.GetUserId<int>() != questionDetailsModel.UserId && !User.IsInRole(Role.Admin))
            {
                Danger("You don't have access to edit this question", true);
                return RedirectToAction("Details", "Main", new { id = questionID });
            }

            //string description = questionDetailsModel.Description;
            //if (description.Contains(questionDetailsModel.Title + Html.TOP_QUESTION_AD_NEW_WINDOW_END_MARKER))
            //    questionDetailsModel.Description = description.Substring(description.IndexOf(Html.TOP_QUESTION_AD_NEW_WINDOW_END_MARKER) + Html.TOP_QUESTION_AD_NEW_WINDOW_END_MARKER.Length);

            return View(questionDetailsModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UpdateQuestionViewModel questionViewModel)
        {
            if (User.Identity.GetUserId<int>() != questionViewModel.UserId && !User.IsInRole(Role.Admin))
            {
                Danger("You don't have access to edit this question", true);
                return RedirectToAction("Details", "Main", new { id = questionViewModel.QuestionID });
            }

            var questionRepository = new QuestionRepository();
            Question question = questionRepository.GetQuestionByID(questionViewModel.QuestionID);
            questionViewModel.Attachments = question.Attachments.ToList();
            Error error = new UpdateQuestionErrorCheckingBR().CanTheQuestionBeUpdated(questionViewModel);
            if (error.ErrorFound)
            {
                Danger(error.Message, true);
                return View(questionViewModel);
            }

            //questionViewModel.Description = new Ads().AppendTopAdAndDescription(questionViewModel.Description, question.Id.ToString(), question.Amount, question.Title);

            question = new QuestionBR().UpdateQuestion(questionViewModel, question, questionRepository, new BlobRepository());

            Success("Update successfully saved", true);
            return RedirectToAction("Edit", new { questionID = question.Id });
        }

        public ActionResult DeleteAttachment(Guid Qid, int Aid)
        {
            var questionRepository = new QuestionRepository();
            Question question = questionRepository.GetQuestionByID(Qid);
            if (question.User.Id == User.Identity.GetUserId<int>())
            {
                Attachment attachment = question.Attachments.Where(a => a.ID == Aid).FirstOrDefault();
                if (attachment != null)
                {
                    new BlobBR().DeleteQuestionAttachment(Qid, attachment, new BlobRepository());
                    questionRepository.DeleteAttachment(attachment);
                }
            }
            return RedirectToAction("Edit", new { QuestionID = Qid });
        }
    }
}