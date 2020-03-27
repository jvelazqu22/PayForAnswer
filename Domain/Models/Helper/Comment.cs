using Domain.Models.Entities;
using Domain.Models.Entities.Identity;
using System;

namespace Domain.Models.Helper
{
    public class Comment
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
