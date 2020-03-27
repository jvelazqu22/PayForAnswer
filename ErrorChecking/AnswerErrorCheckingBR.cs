using Domain.Constants;
using Domain.Exceptions;
using Domain.Models.Entities;
using Domain.Models.ViewModel;
using System;

namespace ErrorChecking
{
    public class AnswerErrorCheckingBR : ErrorCheckingBR
    {
        public Answer CheckForErrorsAndProceedIfThereAreNoExceptions(int userId, NewAnswerViewModel newAnswerViewModel)
        {
            if (userId == 0)
                throw new NewAnswerException(string.Format("Missing userId trying to answer for question id: {0}", newAnswerViewModel.QuestionId));

            if (newAnswerViewModel == null)
                throw new NewAnswerException(string.Format("NewAnswerView Model is null. Answer user id: {0}", userId));

            if (newAnswerViewModel.QuestionId == Guid.Empty)
                throw new NewAnswerException(string.Format("Missing question id. Answer user id: {0}", userId));

            if (IsDescriptionEmpty(newAnswerViewModel.NewPostedAnswer))
                throw new EmptyDescriptionException();
            //throw new EmptyDescriptionException(string.Format("Missing answer description for question id {0} by user {1}",
            //    questionModel.Id, userId));

            Answer answerModel = new Answer();
            answerModel.QuestionId = newAnswerViewModel.QuestionId;
            answerModel.StatusId = StatusValues.AnswerSubmitted;
            answerModel.UserId = userId;
            answerModel.CreatedOn = DateTime.UtcNow;

            return answerModel;
        }
    }
}
