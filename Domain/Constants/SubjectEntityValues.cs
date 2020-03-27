using System.Collections.Generic;

namespace Domain.Constants
{

    public static class SubjectEntityValues
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

        public const string SUBJECT_ENTITIES_TABLE_NAME = "SubjectEntities";
        public const string SUBJECT_DEFINITION_ROW_KEY = "definition";
        public const string SUBJECT_OPEN_QUESTIONS_PKEY_SUFFIX = "_questions_open";
        public const string SUBJECT_PAID_QUESTIONS_PKEY_SUFFIX = "_questions_paid";
        public const string SUBJECT_USERS_PKEY_SUFFIX = "_users";
        public const string QUESTION_SUBJECTS_PKEY = "question_{0}_subjects";
        public const string USER_SUBJECTS_PKEY = "user_{0}_subjects";

        public static string[] DISALLOWED_KEY_FIELDS_ARRAY = { "/", "?", "#", @"\" };
        public static Dictionary<string, string> DISALLOWED_KEY_FIELDS_DICTIONARY = new Dictionary<string, string>()
        {
            {"/", "PAYFORANSWER_FORWARD_SLASH"},
            {"?", "PAYFORANSWER_QUESTION_MARK"},
            {"#", "PAYFORANSWER_HASH"},
            {@"\", "PAYFORANSWER_BACK_SLASH"}
        };
    }
}
