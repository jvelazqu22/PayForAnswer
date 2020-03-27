using Domain.Models.Helper;
using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class RedirectFromPayPalViewModel
    {
        public string Title { get; set; }


        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal Total { get; set; }

        public DateTime CreatedOn { get; set; }

        public Error Error { get; set; }
    }
}
