using Domain.Models;
using Domain.Models.TableEntities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Interfaces
{
    public interface ISubjectEntityTableRepository
    {
        List<String> GetTopSubjectMatches(string term);

        List<SubjectEntity> GetEntitySubjects(string partionKey);

        SubjectEntity GetSubject(string searchTerm);

        void InsertOrReplaceSubjectEntity(SubjectEntity subjectEntity);

        void InsertOrReplaceSubjectEntityList(List<SubjectEntity> subjectEntityList);

        void InsertOrReplaceSubjectEntityListInBatch(List<SubjectEntity> subjectEntityList);
        void InsertOrMergeSubjectEntityList(List<SubjectEntity> subjectEntityList);
    }
}
