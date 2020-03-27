using System;

namespace Domain.Exceptions
{
    public class UpdateAnswerStatusAttack : Exception
    {
        public UpdateAnswerStatusAttack() { }
        public UpdateAnswerStatusAttack(string message) : base(message) { }
    }
}
