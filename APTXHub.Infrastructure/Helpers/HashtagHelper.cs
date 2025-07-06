using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APTXHub.Data.Helpers
{
    public static class HashtagHelper
    {
        public static List<string> GetHashtags(string postText)
        {
            if (string.IsNullOrWhiteSpace(postText))
                return new List<string>();

            var hashtagPattern = new Regex(@"#\w+");
            var matches = hashtagPattern.Matches(postText)
                .Select(match => match.Value.TrimEnd('.', ',', '!', '?').ToLower())
                .Distinct()
                .ToList();

            return matches;
        }

        public static class ContentFormatter
        {
            public static string FormatHashtags(string content)
            {
                if (string.IsNullOrWhiteSpace(content)) return string.Empty;

                var hashtagPattern = new Regex(@"#\w+");
                var result = hashtagPattern.Replace(content, match =>
                {
                    var tag = match.Value;
                    return $"<span class='font-bold text-blue-600'>{tag}</span>";
                });

                return result;
            }
        }


    }
}