using System.ComponentModel.DataAnnotations;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using Domain.Constants;

namespace Domain.Models.TableEntities
{
    public class Subject : TableEntity
    {
        /*
         * PK       RK          UserID  QuestionID
         * Finance  UID_ACTIVE  1       
         * Finance  UID_ACTIVE  2
         * Finance  QID_ACTIVE          "1AAAAAAA-BBBB-CCCC-DDDD-3EEEEEEEEEEE"
         * Finance  QID_ACTIVE          "3AAAAAAA-BBBB-CCCC-DDDD-2EEEEEEEEEEE"
         * Finance  DEFINITION
         */

        public Subject()
        {
            this.PartitionKey = "Finance".ToLower();
            this.RowKey = SubjectValues.ACTIVE_USER_ROW_KEY;
        }

        public int UserID { get; set; }
        
        public Guid QuestionID { get; set;}
    }
}
