using Microsoft.WindowsAzure.Storage.Table;

namespace Domain.Models.TableEntities
{
    public class SubjectEntity : TableEntity
    {
        /*
         * PK                       RK                                      UserID      URL
         * finance                  definition
         * math                     definition
         * finance_questions_open   "1AAAAAAA-BBBB-CCCC-DDDD-3EEEEEEEEEEE"              http://payforanswer/url1
         * finance_questions_open   "2AAAAAAA-BBBB-CCCC-DDDD-2EEEEEEEEEEE"              http://payforanswer/url2
         * finance_questions_paid   "3AAAAAAA-BBBB-CCCC-DDDD-2EEEEEEEEEEE"              http://payforanswer/url3
         * finance_questions_paid   "4AAAAAAA-BBBB-CCCC-DDDD-2EEEEEEEEEEE"              http://payforanswer/url4
         * finance_users            jvelazqu22@hotmail.com                  1
         * finance_users            jvelazqu24@hotmail.com                  2
         * question_1_subjects      finance
         * question_1_subjects      math
         * question_2_subjects      accounting
         * question_2_subjects      calculus
         * user_1_subjects          finance
         * user_1_subjects          math
         * user_2_subjects          accounting
         * user_2_subjects          calculus
         */

        public int UserID { get; set; }
        public string Url { get; set; }
    }
}
