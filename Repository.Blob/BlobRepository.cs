using Domain.Constants;
using Domain.Models.Entities;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Repository.Interfaces;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;

namespace Repository.Blob
{
    public class BlobRepository : IBlobRepository
    {
        private CloudStorageAccount storageAccount;
        private CloudBlobClient blobClient;
        private CloudBlobContainer blobContainer;

        public BlobRepository()
        {
            storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            // If this is running in an Azure Web Site (not a Cloud Service) use the Web.config file:
            //    var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);

            blobClient = storageAccount.CreateCloudBlobClient();
        }

        public void UploadAStreamToABlob(Stream stream, string blobName, string containerName)
        {
            blobContainer = blobClient.GetContainerReference(containerName);
            SetBlobClientDefaultRequestOptions();
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(blobName);
            blob.UploadFromStream(stream);
        }

        public string GetHtmlFileContent(string blobName, string container)
        {
            blobContainer = blobClient.GetContainerReference(container);
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(blobName);
            MemoryStream stream = new MemoryStream();
            blob.DownloadToStream(stream);
            stream.Position = 0;
            StreamReader readStream = new StreamReader(stream);
            return readStream.ReadToEnd();
        }

        public string AddUpdateHtmlFileContent(string blobName, string content, string containerName)
        {
            string absoluteUri = string.Empty;
            try
            {
                blobContainer = blobClient.GetContainerReference(containerName);
                SetBlobClientDefaultRequestOptions();
                CloudBlockBlob blob = blobContainer.GetBlockBlobReference(blobName);
                blob.Properties.ContentType = "text/html";
                byte[] byteArray = Encoding.ASCII.GetBytes(content);
                using (MemoryStream stream = new MemoryStream(byteArray))
                {
                    stream.Position = 0;
                    blob.UploadFromStream(stream);
                    absoluteUri = blob.Uri.AbsoluteUri;
                }
            }
            catch (StorageException) { };
            return absoluteUri;
        }

        public void UploadFileToBlob(string blobName, string fileNameWithPath, string containerName)
        {
            // @"C:\myfile"
            blobContainer = blobClient.GetContainerReference(containerName);
            SetBlobClientDefaultRequestOptions();
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(blobName);
            blob.UploadFromFile(fileNameWithPath, FileMode.Open);
        }

        public void DeleteAttachment(string blobName)
        {
            blobContainer = blobClient.GetContainerReference(StorageValues.ATTACHMENT_CONTAINER);
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(blobName);
            blob.DeleteIfExists();
        }

        private void SetBlobClientDefaultRequestOptions()
        {
            blobClient.DefaultRequestOptions.ParallelOperationThreadCount = Size.ParallelOperationThreadCount;
            blobClient.DefaultRequestOptions.SingleBlobUploadThresholdInBytes = (long)(StorageSize.BytesInAMegabyte * 64); // max value
        }
    }
}
