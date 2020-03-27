using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Entities
{
    public class Payment
    {
        public long Id { get; set; }
        public int? StatusId { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal Total { get; set; }
        public virtual Status Status { get; set; }
    }
}
