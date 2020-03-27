using Domain.Constants;
using Domain.Models.Entities;
using Domain.Models.Entities.Identity;
using Domain.Models.Helper;
using Repository.Blob;
using Repository.SQL;
using System;
using System.Collections.Generic;

namespace DataLoad
{
    public class CommentData
    {
        public List<Comment> GetTestCommentsToBeAdded()
        {
            var comments = new List<Comment>
                {
                    new Comment 
                    {  
                        Description = "I did investigate a bit further, and things are funny. With O3, both versions (sort/non sort) are the same speed (4.5) but with O2, both are different (3.1/15.7) so I looked at the O2 version. There is a branch. So gcc seems to optimize for random data here. To further test if it is branch prediction, I tested the O2 code not with sort, but in the creation phase I set/removed the top bit of the byte for one half, but not the other. Things are the same result here, so it really has nothing to do with the data being sorted, but with the if condition being true/false for one half. –  PlasmaHH Jun 27 '12 at 14:16", 
                        UserId = 1, CreatedOn = DateTime.UtcNow, User = new ApplicationUser() { UserName = "jvelazquez1"} 
                    }, //1
                    new Comment 
                    {  
                        Description = "I did investigate a bit further, and things are funny. With O3, both versions (sort/non sort) are the same speed (4.5) but with O2, both are different (3.1/15.7) so I looked at the O2 version. There is a branch. So gcc seems to optimize for random data here. To further test if it is branch prediction, I tested the O2 code not with sort, but in the creation phase I set/removed the top bit of the byte for one half, but not the other. Things are the same result here, so it really has nothing to do with the data being sorted, but with the if condition being true/false for one half. –  PlasmaHH Jun 27 '12 at 14:16", 
                        UserId = 2, CreatedOn = DateTime.UtcNow, User = new ApplicationUser() { UserName = "jvelazquez2"}  
                    }, //2
                    new Comment 
                    {  
                        Description = "I did investigate a bit further, and things are funny. With O3, both versions (sort/non sort) are the same speed (4.5) but with O2, both are different (3.1/15.7) so I looked at the O2 version. There is a branch. So gcc seems to optimize for random data here. To further test if it is branch prediction, I tested the O2 code not with sort, but in the creation phase I set/removed the top bit of the byte for one half, but not the other. Things are the same result here, so it really has nothing to do with the data being sorted, but with the if condition being true/false for one half. –  PlasmaHH Jun 27 '12 at 14:16", 
                        UserId = 3, CreatedOn = DateTime.UtcNow, User = new ApplicationUser() { UserName = "jvelazquez3"}  
                    }, //3
                    new Comment 
                    {  
                        Description = "What architecture did you run on? Did you compile with good optimization settings? I just tried your code, with and without the sort (the C++ variant) and did not find any runtime difference. Having a look at the assembler output (gcc.godbolt.org is handy for that) I could also see that there is no branch done on the if, but a cmovge is being used. When using -O2 I see a difference in speed only, but not with -O3... – ", 
                        UserId = 1, CreatedOn = DateTime.UtcNow, User = new ApplicationUser() { UserName = "jvelazquez1"}  
                    }, //4
                    new Comment 
                    {  
                        Description = "What architecture did you run on? Did you compile with good optimization settings? I just tried your code, with and without the sort (the C++ variant) and did not find any runtime difference. Having a look at the assembler output (gcc.godbolt.org is handy for that) I could also see that there is no branch done on the if, but a cmovge is being used. When using -O2 I see a difference in speed only, but not with -O3... – ", 
                        UserId = 2, CreatedOn = DateTime.UtcNow, User = new ApplicationUser() { UserName = "jvelazquez2"}  
                    }, //5
                    new Comment 
                    {  
                        Description = "What architecture did you run on? Did you compile with good optimization settings? I just tried your code, with and without the sort (the C++ variant) and did not find any runtime difference. Having a look at the assembler output (gcc.godbolt.org is handy for that) I could also see that there is no branch done on the if, but a cmovge is being used. When using -O2 I see a difference in speed only, but not with -O3... – ", 
                        UserId = 3, CreatedOn = DateTime.UtcNow, User = new ApplicationUser() { UserName = "jvelazquez3"} 
                    }, //6
                };
            return comments;
        }

        public void AddCommentsToQuestions(BlobRepository blobRepository, List<Question> questions, List<Comment> comments, PfaDb context)
        {
            string htmlComments = Html.COMMENTS_DEFAULT_VALUE;
            var commentsBlobPath = string.Empty;
            var commentsUrl = string.Empty;

            for (int i = 0; i < questions.Count; i++)
            {
                if (i == 4)
                {
                    htmlComments = string.Empty;
                    comments.ForEach(c => htmlComments += string.Format(Html.COMMENTS, c.User.UserName, c.CreatedOn, c.Description));
                }
                commentsBlobPath = string.Format(StorageValues.QUESTION_COMMENT_PATH_PLACE_HOLDER, questions[i].Id,
                                                    StorageValues.COMMENTS_FILE_NAME);

                commentsUrl = string.Format(StorageValues.QUESTION_COMMENT_URL_PLACE_HOLDER, StorageValues.STORAGE_URL_PRIMARY,
                                                StorageValues.COMMENTS_CONTAINER, questions[i].Id, StorageValues.COMMENTS_FILE_NAME);

                blobRepository.AddUpdateHtmlFileContent(commentsBlobPath, htmlComments, StorageValues.COMMENTS_CONTAINER);
                questions[i].CommentsUrl = commentsUrl;
            }

            context.SaveChanges();
        }

        public void AddCommentsToAnswers(BlobRepository blobRepository, List<Answer> answers, List<Comment> comments, PfaDb context)
        {
            for (int i = 0; i < answers.Count; i++)
            {
                var htmlComments = Html.COMMENTS_DEFAULT_VALUE;
                if (i == 13)
                {
                    htmlComments = string.Empty;
                    comments.ForEach(c => htmlComments += string.Format(Html.COMMENTS, c.User.UserName, c.CreatedOn, c.Description));
                }
                var commentsBlobPath = string.Format(StorageValues.ANSWER_COMMENT_PATH_PLACE_HOLDER, answers[i].QuestionId, answers[i].Id,
                                                    StorageValues.COMMENTS_FILE_NAME);
                var commentsUrl = string.Format(StorageValues.ANSWER_COMMENT_URL_PLACE_HOLDER, StorageValues.STORAGE_URL_PRIMARY,
                                                StorageValues.COMMENTS_CONTAINER, answers[i].QuestionId, answers[i].Id, StorageValues.COMMENTS_FILE_NAME);
                blobRepository.AddUpdateHtmlFileContent(commentsBlobPath, htmlComments, StorageValues.COMMENTS_CONTAINER);
                answers[i].CommentsUrl = commentsUrl;
            }

            context.SaveChanges();
        }
    }
}
