using AutoMapper;
using Domain.Models;
using Domain.Models.Entities;

namespace PayForAnswer.App_Start
{
    public static class InitializeObjects
    {
        public static void AutoMapObjects()
        {
            Mapper.CreateMap<Question, ValidateQuestionViewModel>();
        }
    }
}