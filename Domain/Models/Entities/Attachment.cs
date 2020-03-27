
namespace Domain.Models.Entities
{
    public class Attachment
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Container { get; set; }
        public string PrimaryUri { get; set; }
        public string SecondaryUri { get; set; }
        public string ContentType { get; set; }
        public long SizeInBytes { get; set; }
    }
}
