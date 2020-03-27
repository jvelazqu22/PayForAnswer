using Domain.Constants;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Repository.Interfaces;
using System;
using System.Configuration;

namespace Repository.Queue
{
    public class QueueRepository : IQueueRepository
    {
        private CloudStorageAccount storageAccount;
        private CloudQueueClient queueClient;
        private CloudQueue queue;

        public QueueRepository()
        {
            storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            // If this is running in an Azure Web Site (not a Cloud Service) use the Web.config file:
            //    var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);

            queueClient = storageAccount.CreateCloudQueueClient();
            queue = queueClient.GetQueueReference(StorageValues.NEW_QUESTION_NOTIFICATIONS_QUEUE);
        }

        public void AddMessage(string msg)
        {
            CloudQueueMessage message = new CloudQueueMessage(msg);
            queue.AddMessage(message, TimeSpan.FromMinutes(60));
        }

        public string GetMessage()
        {
            CloudQueueMessage message = queue.GetMessage();
            if (message != null)
                return message.AsString;
            return string.Empty;
        }

        public void DeleteMessage(CloudQueueMessage message)
        {
            queue.DeleteMessage(message);
        }
    }
}
