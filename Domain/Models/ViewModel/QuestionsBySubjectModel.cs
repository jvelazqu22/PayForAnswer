using Domain.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Models.ViewModel
{
    public class QuestionsBySubjectModel
    {
        public IQueryable<Question> Questions { get; set; }
        public IEnumerable<Subject> Subjects { get; set; }
        public string CommaSpaceDelimitedSubjects { get; set; }
        public int PageNumber { get; set; }
    }
}
