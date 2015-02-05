using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;

namespace SunLine.Community.Common
{
    public static class StringExtensions
    {
        private const string UserNameRegex = @"(?:(?<=\s)|^)@(\w*[A-Za-z_]+\w*)";
        private const string HashtagRegex = @"(?:(?<=\s)|^)#(\w*[A-Za-z_]+\w*)";
        private const string UrlRegex = @"(http|ftp|https):\/\/([\w\-_]+(?:(?:\.[\w\-_]+)+))([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?";

        public static int NumberOfWords(this String s)
        {
            int c = 0;
            for (int i = 1; i < s.Length; i++)
            {
                if (Char.IsWhiteSpace(s[i - 1]))
                {
                    if (Char.IsLetterOrDigit(s[i]) ||
                        Char.IsPunctuation(s[i]))
                    {
                        c++;
                    }
                }
            }
            if (s.Length > 2)
            {
                c++;
            }
            return c;
        }

        public static int ReadingTime(this String s)
        {
            int words = s.NumberOfWords();
            const int wordsPerMinute = 180;
            return words / wordsPerMinute;
        }

        public static string GetMindWithLinks(this String s)
        {
            string text = Regex.Replace(s, HashtagRegex, "<a href='#'>$&</a>");
            text = Regex.Replace(text, UserNameRegex, "<a href='/Users/Show/$&'>$&</a>");
            text = Regex.Replace(text, UrlRegex, "<a href='$&' target='_blank'>$&</a>");
            return text;
        }

        public static MatchCollection GetMatchedUrls(this String s)
        {
            return Regex.Matches(s, UrlRegex);
        }

        public static IList<string> GetUrls(this String s)
        {
            var matches = GetMatchedUrls(s);
            var urls = new List<string>();
            if (matches.Count == 0)
            {
                return urls;
            }

            foreach (Match match in matches)
            {
                urls.Add(match.Value);
            }

            return urls;
        }

        public static MatchCollection GetMatchedUserNames(this String s)
        {
            return Regex.Matches(s, UserNameRegex);
        }

        public static IList<string> GetUserNames(this String s)
        {
            var matches = GetMatchedUserNames(s);
            var userNames = new List<string>();
            if (matches.Count == 0)
            {
                return userNames;
            }

            foreach (Match match in matches)
            {
                userNames.Add(match.Value.Replace("@", ""));
            }

            return userNames;
        }

        public static MatchCollection GetMatchedHashtags(this String s)
        {
            return Regex.Matches(s, HashtagRegex);
        }

        public static IList<String> GetHashtags(this String s)
        {
            var matches = GetMatchedHashtags(s);

            var hashtags = new List<string>();
            if (matches.Count == 0)
            {
                return hashtags;
            }

            foreach (Match match in matches)
            {
                hashtags.Add(match.Value.Replace("#", "").ToLower());
            }

            return hashtags;
        }

        public static string GetUserNameToReply(this String s)
        {
            var matches = Regex.Matches(s, UserNameRegex);

            if (matches.Count == 0)
            {
                return null;
            }

            if (matches[0].Index != 0)
            {
                return null;
            }

            return matches[0].Value.Replace("@", "");
        }

        public static string CalculateMd5Hash(this String s)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(s);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            var sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
