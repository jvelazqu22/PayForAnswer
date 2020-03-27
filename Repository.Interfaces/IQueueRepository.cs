using Microsoft.WindowsAzure.Storage.Queue;

namespace Repository.Interfaces
{
    public interface IQueueRepository
    {
        void AddMessage(string msg);

        string GetMessage();

        void DeleteMessage(CloudQueueMessage message);
    }
}
