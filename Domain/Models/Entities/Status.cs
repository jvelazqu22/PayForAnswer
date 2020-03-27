using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Entities
{
    public class Status
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Display(Name = "Status name")]
        public string Name { get; set; }
        [Display(Name = "Status")]
        public string DisplayName { get; set; }
    }
}
