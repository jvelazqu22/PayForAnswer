using Domain.Constants;
using Domain.Models.Entities;
using System;
using System.Web;

namespace Utilities
{
    public class FeedbackUrlsAndPaths
    {
        public string GetFeedbackPrimaryUrl(Guid guid)
        {
            return string.Format(StorageValues.FEEDBACK_URL_PLACE_HOLDER,
                                    StorageValues.STORAGE_URL_PRIMARY,
                                    StorageValues.FEEDBACK_CONTAINER,
                                    guid,
                                    StorageValues.FEEDBACK_FILE_NAME);
        }

        public string GetFeedbackPath(Guid guid)
        {
            return string.Format(StorageValues.FEEDBACK_PATH_PLACE_HOLDER, guid, StorageValues.FEEDBACK_FILE_NAME);
        }
    }
}
