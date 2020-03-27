using Domain.Constants;
using Domain.Models.Entities;
using Repository.Blob;
using Repository.SQL;
using System.Collections.Generic;

namespace DataLoad
{
    public class DescriptionData
    {
        public void AddDescriptionToQuestions(BlobRepository blobRepository, List<Question> questions, PfaDb context)
        {
            string description = string.Empty;
            string descriptionBlobPath = string.Empty;
            string descriptionUrl = string.Empty;

            for (int i = 0; i < questions.Count; i++)
            {
                if (i == 0) description = "What is the square root of -1?";
                else if (i == 1) description = "Is there a way to create my Menu Items for the actionbar in my MainActivity where my ViewPager is created so that way im only creating my webview items (back, forward and refresh buttons) once. Then inside my onOptions.... Then specify the menu items to be able to handle my webview.  All my fragments in my ViewPager are webviews, i just want 3 constant Actionbar items to control the webview's that is being shown. Instead of recreating them each time my viewpager loads another fragment. Any Idea's???";
                else if (i == 2) description = "Why does the predicted mass of the quantum vacuum have little effect on the expansion of the universe?";
                else if (i == 3) description = "Can quantum mechanics and general relativity be realized as a fully consistent theory (perhaps as a quantum field theory)?[7] Is spacetime fundamentally continuous or discrete? Would a consistent theory involve a force mediated by a hypothetical graviton, or be a product of a discrete structure of spacetime itself (as in loop quantum gravity)? Are there deviations from the predictions of general relativity at very small or very large scales or in other extreme circumstances that flow from a quantum gravity theory?";
                else if (i == 4) description = "The P versus NP problem is a major unsolved problem in computer science. Informally, it asks whether every problem whose solution can be quickly verified by a computer can also be quickly solved by a computer. It was introduced in 1971 by Stephen Cook in his seminal paper 'The complexity of theorem proving procedures'[2] and is considered by many to be the most important open problem in the field.[3] It is one of the seven Millennium Prize Problems selected by the Clay Mathematics Institute to carry a US$ 1,000,000 prize for the first correct solution. The informal term quickly used above means the existence of an algorithm for the task that runs in polynomial time. The general class of questions for which some algorithm can provide an answer in polynomial time is called 'class' or just 'P'. For some questions, there is no known way to find an answer quickly, but if one is provided with information showing what the answer is, it may be possible to verify the answer quickly. The class of questions for which an answer can be verified in polynomial time is called NP. Consider the subset sum problem, an example of a problem that is easy to verify, but whose answer may be difficult to compute. Given a set of integers, does some nonempty subset of them sum to 0? For instance, does a subset of the set {−2, −3, 15, 14, 7, −10} add up to 0? The answer 'yes, because {−2, −3, −10, 15} add up to zero' can be quickly verified with three additions. However, there is no known algorithm to find such a subset in polynomial time (there is one, however, in exponential time, which consists of 2n-1 tries), and indeed such an algorithm can only exist if P = NP; hence this problem is in NP (quickly checkable) but not necessarily in P (quickly solvable). An answer to the P = NP question would determine whether problems that can be verified in polynomial time, like the subset-sum problem, can also be solved in polynomial time. If it turned out that P ≠ NP, it would mean that there are problems in NP (such as NP-complete problems) that are harder to compute than to verify: they could not be solved in polynomial time, but the answer could be verified in polynomial time. Aside from being an important problem in computational theory, a proof either way would have profound implications for mathematics, cryptography, algorithm research, artificial intelligence, game theory, multimedia processing and many other fields.";
                else if (i == 5) description = "I have a computer with a blue tint on it and my monitor seems to changed language to dutch. I can see on my monitor options that the blue hue is turned up while red and green remain the same but i my monitor wont let me change the hue. also when my cord is not plugged in the entire screen is white. what do i do My monitor is aGateway FPD1730.";
                else if (i == 6) description = "Need to replace the Blade 400. It can be somewhat intimidating to a new pilot, especially when you look in the manual at the parts listing and see all of those parts. Willing to pay for step by step instructions to replace it";
                else if (i == 7) description = description = "Every time I flush the toilet, it overflow for a long period of time. I think something may be stuck in the pipes. I am willing to pay for step by step directions to identify the problem and fix it.";

                descriptionBlobPath = string.Format(StorageValues.QUESTION_DESCRIPTION_PATH_PLACE_HOLDER, questions[i].Id.ToString(),
                                                            StorageValues.DESCRIPTION_FILE_NAME);
                descriptionUrl = string.Format(StorageValues.QUESTION_DESCRIPTION_URL_PLACE_HOLDER, StorageValues.STORAGE_URL_PRIMARY,
                                                StorageValues.DESCRIPTION_CONTAINER, questions[i].Id, StorageValues.DESCRIPTION_FILE_NAME);
                blobRepository.AddUpdateHtmlFileContent(descriptionBlobPath, description, StorageValues.DESCRIPTION_CONTAINER);
                questions[i].DescriptionUrl = descriptionUrl;
            }

            context.SaveChanges();
        }

        public void AddDescriptionToAnswers(BlobRepository blobRepository, List<Answer> answers, PfaDb context)
        {
            foreach (var answer in answers)
            {
                var description = "Some answer";
                var descriptionBlobPath = string.Format(StorageValues.ANSWER_DESCRIPTION_PATH_PLACE_HOLDER, answer.QuestionId, answer.Id,
                                                    StorageValues.DESCRIPTION_FILE_NAME);
                var descriptionUrl = string.Format(StorageValues.ANSWER_DESCRIPTION_URL_PLACE_HOLDER, StorageValues.STORAGE_URL_PRIMARY,
                                                StorageValues.DESCRIPTION_CONTAINER, answer.QuestionId, answer.Id, StorageValues.DESCRIPTION_FILE_NAME);
                blobRepository.AddUpdateHtmlFileContent(descriptionBlobPath, description, StorageValues.DESCRIPTION_CONTAINER);
                answer.DescriptionUrl = descriptionUrl;
            }
            context.SaveChanges();
        }
    }
}
