using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Domain.Models.Helper
{
    public class UploadFile
    {
        //public HttpPostedFileBase File { get; set; }
        public IEnumerable<HttpPostedFileBase> Files { get; set; }
        public string FilesToBeUploaded { get; set; }
    }
}
