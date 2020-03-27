using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities
{
    public class Log
    {
        public int Id { set; get; }
        public DateTime Date { get; set; }
        public string Thread { set; get; }
        public string Level { get; set; }
        public string Logger { set; get; }
        public string Message { get; set; }
        public string Exception { set; get; }
    }
}
