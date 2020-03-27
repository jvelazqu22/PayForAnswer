using Domain.Constants;
using Domain.Models;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.ViewModel;

namespace BusinessRules.Errors
{
    public class AnswerErrorCheckingBR : ErrorCheckingBR
    {
        public Answer CheckForErrorsAndProceedIfThereAreNoExceptions(int userId, NewAnswerViewModel newAnswerViewModel)
        {
            if (userId == 0)
                throw new NewAnswerException(string.Format("Missing userId trying to answer for question id: {0}", newAnswerViewModel.QuestionId));

            if (newAnswerViewModel == null)
                throw new NewAnswerException(string.Format("NewAnswerView Model is null. Answer user id: {0}", userId));

            if (newAnswerViewModel.QuestionId == 0)
                throw new NewAnswerException(string.Format("Missing question id. Answer user id: {0}", userId));

            if (IsDescriptionEmpty(newAnswerViewModel.NewPostedAnswer))
                throw new EmptyDescriptionException();
            //throw new EmptyDescriptionException(string.Format("Missing answer description for question id {0} by user {1}",
            //    questionModel.Id, userId));

            Answer answerModel = new Answer();
            answerModel.Description = newAnswerViewModel.NewPostedAnswer;
            answerModel.QuestionId = newAnswerViewModel.QuestionId;
            answerModel.StatusId = StatusValues.AnswerSubmitted;
            answerModel.UserId = userId;
            answerModel.CreatedOn = DateTime.UtcNow;

            return answerModel;
        }
    }
}
