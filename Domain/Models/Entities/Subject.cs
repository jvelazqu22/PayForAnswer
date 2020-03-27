using Domain.Constants;
using Domain.Models.Entities.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Entities
{
    public class Subject
    {
        public long Id { get; set; }
        [Index]
        [StringLength(Size.MaxCharactersPerSubjectAllowed)]
        public string SubjectName { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}
