using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class YoutubeHelper
    {
        public const string PUBLISHED = "published";
        public const string VIEW_COUNT = "viewCount";
        public const string RELEVANCE = "relevance";

        public static string CreateGdataLink(string youtubeChannel, int resultStartingWith, int numberOfResults, string orderBy)
        {
            return string.Format(
                    "http://gdata.youtube.com/feeds/api/users/{0}/uploads?alt=rss&start-index={1}&max-results={2}&orderby={3}",
                    youtubeChannel, resultStartingWith, numberOfResults, orderBy);
        }

    }
}
