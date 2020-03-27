using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Domain.Models.TableEntities.SubjectTable;
using Domain.Constants;
using Domain.Models.TableEntities;
using Utilities;
using Repository.Interfaces;

namespace Repository.Tables
{
    public class SubjectEntityTableRepository : ISubjectEntityTableRepository
    {
        private CloudStorageAccount storageAccount;
        private CloudTableClient tableClient;
        private CloudTable subjectEntityTable;

        public SubjectEntityTableRepository()
        {
            storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            // If this is running in an Azure Web Site (not a Cloud Service) use the Web.config file:
            //    var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);
            tableClient = storageAccount.CreateCloudTableClient();

            subjectEntityTable = tableClient.GetTableReference(SubjectEntityValues.SUBJECT_ENTITIES_TABLE_NAME);
            //subjectTable.CreateIfNotExists();
        }

        public List<String> GetTopSubjectMatches(string term)
        {
            term = term.ToLower();
            List<string> list = new List<string>();
            string termToExclude = new GeneralHelper().GetTermWithNextLastLetter(term);
            IEnumerable<SubjectEntity> query;
            if (term.ToLower() != termToExclude.ToLower())
            {
                query = (from subject in subjectEntityTable.CreateQuery<SubjectEntity>()
                         where subject.PartitionKey.CompareTo(term) >= 0 && subject.PartitionKey.CompareTo(termToExclude) < 0
                         && subject.RowKey == SubjectEntityValues.SUBJECT_DEFINITION_ROW_KEY
                         select subject).Take(General.MaxNumberOfSmartSearchResults);
            }
            else
            {
                query = (from subject in subjectEntityTable.CreateQuery<SubjectEntity>()
                         where subject.PartitionKey.CompareTo(term) >= 0 && subject.RowKey == SubjectEntityValues.SUBJECT_DEFINITION_ROW_KEY
                         select subject).Take(General.MaxNumberOfSmartSearchResults);
            }

            foreach (SubjectEntity subject in query)
                list.Add(subject.PartitionKey);

            return list;
        }

        public List<SubjectEntity> GetEntitySubjects(string partionKey)
        {
            List<SubjectEntity> list = new List<SubjectEntity>();
            if (string.IsNullOrEmpty(partionKey)) return list;

            return (from subjectEntity in subjectEntityTable.CreateQuery<SubjectEntity>()
                    where subjectEntity.PartitionKey == partionKey
                    select subjectEntity).ToList();
        }


        public SubjectEntity GetSubject(string searchTerm)
        {
            // Retrieve the entity with partition key of searchTerm and row key of SubjectValues.SUBJECT_DEFINITION_ROW_KEY
            TableOperation retrieveSubject = TableOperation.Retrieve<SubjectEntity>(searchTerm.ToLower(), SubjectEntityValues.SUBJECT_DEFINITION_ROW_KEY);

            // Retrieve entity 
            SubjectEntity specificEntity = (SubjectEntity)subjectEntityTable.Execute(retrieveSubject).Result;
            return specificEntity;
        }

        public void InsertOrReplaceSubjectEntity(SubjectEntity subjectEntity)
        {
            TableOperation insertSubject = TableOperation.InsertOrReplace(subjectEntity);
            subjectEntityTable.Execute(insertSubject);
        }

        public void InsertOrReplaceSubjectEntityList(List<SubjectEntity> subjectEntityList)
        {
            subjectEntityList.ForEach(se => subjectEntityTable.Execute(TableOperation.InsertOrReplace(se)));
        }

        public void InsertOrReplaceSubjectEntityListInBatch(List<SubjectEntity> subjectEntityList)
        {
            TableBatchOperation batchOperation = new TableBatchOperation();
            subjectEntityList.ForEach(s => batchOperation.InsertOrReplace(s));
            subjectEntityTable.ExecuteBatch(batchOperation);
        }

        public void InsertOrMergeSubjectEntityList(List<SubjectEntity> subjectEntityList)
        {
            subjectEntityList.ForEach(se => subjectEntityTable.Execute(TableOperation.InsertOrMerge(se)));
        }
    }
}
