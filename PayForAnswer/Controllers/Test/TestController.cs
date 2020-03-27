using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Repository.SQL;
using log4net;
using Domain.Constants;
using Domain.Exceptions;
using Domain.Models.Helper;
using System.IO;
using Repository.Blob;

namespace PayForAnswer.Controllers.Test
{
    public class TestController : BootstrapBaseController
    {
        PfaDb _pfaDb = new PfaDb();
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ActionResult Index()
        {
            var model = new TestModel() { CommentsUrl = "http://payforanswer.blob.core.windows.net/comments/1/comment.html" };

            return View(model);
        }

        public ActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(UploadFile fileToUpload)
        {
            foreach(var file in fileToUpload.Files)
                if (file.ContentLength > 0)
                    new BlobRepository().UploadAStreamToABlob(file.InputStream, Path.GetFileName(file.FileName), StorageValues.ATTACHMENT_CONTAINER);

            return View();
        }

        public ActionResult UploadAction()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadAction(UploadFile model, List<HttpPostedFileBase> fileUpload)
        {
            // Your Code - / Save Model Details to DB

            // Handling Attachments - 
            foreach (HttpPostedFileBase item in fileUpload)
            {
                if (item != null && Array.Exists(model.FilesToBeUploaded.Split(','), s => s.Equals(item.FileName)))
                {
                    //Save or do your action -  Each Attachment ( HttpPostedFileBase item ) 
                }
            }
            return View();
        }

    }
}
