using Domain.Constants;
using Domain.Models.Entities;
using Domain.Models.ViewModel;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace BusinessRules
{
    public class FeedbackBR
    {
        public void AddFeedback(SiteFeedbackViewModel model, IBlobRepository blobRepository, IFeedbackRepository generalRepository)
        {
            FeedbackUrlsAndPaths urlAndPaths = new FeedbackUrlsAndPaths();
            Guid guid = Guid.NewGuid();
            string feedbackBlobPath = urlAndPaths.GetFeedbackPath(guid);
            blobRepository.AddUpdateHtmlFileContent(feedbackBlobPath, model.Message, StorageValues.FEEDBACK_CONTAINER);
            var url = urlAndPaths.GetFeedbackPrimaryUrl(guid);
            var feedback = new Feedback() { Title = model.Title, FeedbackUrl = url, CreatedOn = DateTime.UtcNow };
            generalRepository.InsertFeedback(feedback);
        }
    }
}
