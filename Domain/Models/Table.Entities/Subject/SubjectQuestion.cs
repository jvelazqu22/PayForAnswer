using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Domain.Models.TableEntities.SubjectTable
{
    public class SubjectQuestion : TableEntity
    {
        public string SubjectName
        {
            get { return this.PartitionKey; }
            set { this.PartitionKey = value; }
        }
        
        public Guid QuestionID
        {
            get { return Guid.Parse(this.RowKey); }
            set { this.RowKey = value.ToString(); }
        }
    }
}
