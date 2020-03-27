using Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Repository.Interfaces
{
    public interface IBlobRepository
    {
        string GetHtmlFileContent(string blobName, string container);
        string AddUpdateHtmlFileContent(string questionID, string content, string containerName);
        void UploadAStreamToABlob(Stream stream, string blobName, string containerName);
        void DeleteAttachment(string blobName);
    }
}
