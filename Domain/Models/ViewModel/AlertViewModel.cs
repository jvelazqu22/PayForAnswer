using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Domain.Models.ViewModel
{
    public class AlertViewModel
    {
        [AllowHtml]
        public string Message { get; set; }
        public string AlertStyle { get; set; }
    }
}
