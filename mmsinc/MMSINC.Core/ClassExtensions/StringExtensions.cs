using Humanizer;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

// ReSharper disable once CheckNamespace
namespace MMSINC.ClassExtensions.StringExtensions
{
    public static class StringExtensions
    {
        #region Constants

        public const string EMAIL_REGEX =
            @"^[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$";

        public static readonly Regex VALID_EMAIL_REGEX = new Regex(EMAIL_REGEX, RegexOptions.Compiled);
        public static readonly Regex STRIP_QUOTES_REGEX = new Regex("^\"(.*)\"$", RegexOptions.Compiled);
        public static readonly Regex DAYS_AGO_REGEX = new Regex(@"^(\d+) days ago", RegexOptions.Compiled);
        public static readonly Regex FROM_NOW_REGEX = new Regex(@"^(\d+) days from now", RegexOptions.Compiled);

        public const string TITLE_CASE_REGEX =
            "(?<!^)" + // don't match on the first character - never want to place a space here
            "(" +
            "  [A-Z][a-z] |" + // put a space before "Aaaa"
            "  (?<=[a-z])[A-Z] |" + // put a space into "aAAA" before the first capital
            "  (?<![A-Z])[A-Z]$" + // if the last letter is capital, prefix it with a space too
            ")";

        #endregion

        /// <summary>
        /// Overloads to string.Contains method that accepts a StringComparison type. This is
        /// to match StartsWith/EndsWith overloads. 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="stringToFind"></param>
        /// <param name="comparisonType"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static bool Contains(this string value, string stringToFind, StringComparison comparisonType)
        {
            return (value.IndexOf(stringToFind, comparisonType) > -1);
        }

        [DebuggerHidden]
        public static string ThrowIfNullOrWhiteSpace(this string value, string message)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(message);
            }

            return value;
        }

        /// <summary>
        /// Removes whitespace and junk from a value and returns its lowercased version.
        /// </summary>
        public static string SanitizeAndDowncase(this string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return value.Trim().ToLower();
        }

        public static bool IsDateTime(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            if (new[] { "today", "yesterday", "tomorrow", "a fortnight", "a fortnight ago" }.Contains(value))
            {
                return true;
            }

            return DAYS_AGO_REGEX.IsMatch(value) || FROM_NOW_REGEX.IsMatch(value);
        }

        public static DateTime ToDateTime(this string value)
        {
            value.ThrowIfNullOrWhiteSpace("value");

            var match = DAYS_AGO_REGEX.Match(value);
            if (match.Groups.Count > 1)
                return DateTime.Now.AddDays(-int.Parse(match.Groups[1].Value));

            if (FROM_NOW_REGEX.IsMatch(value))
                return DateTime.Now.AddDays(int.Parse(FROM_NOW_REGEX.Match(value).Groups[1].Value));

            switch (value)
            {
                case "today":
                    return DateTime.Now;
                case "yesterday":
                    return DateTime.Now.AddDays(-1);
                case "tomorrow":
                    return DateTime.Now.AddDays(1);
                case "a fortnight":
                    return DateTime.Now.AddDays(14);
                case "a fortnight ago":
                    return DateTime.Now.AddDays(-14);
                default:
                    return DateTime.Parse(value);
            }
        }

        /// <summary>
        /// Splits a string into chunks of the given size. If the string is shorter
        /// than the given size, then the string itself is returned. If the last chunk
        /// is shorter than the size, then the shortened chunk is returned. 
        /// </summary>
        public static IEnumerable<String> SplitEvery(this string value, int size)
        {
            var curIndex = 0;
            var stringLength = value.Length;
            while (curIndex < stringLength)
            {
                if (curIndex + size > stringLength)
                {
                    yield return value.Substring(curIndex);
                }
                else
                {
                    yield return value.Substring(curIndex, size);
                }

                curIndex += size;
            }
        }

        /// <summary>
        /// Splits a string wherever any whitespace character is found.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IEnumerable<string> SplitOnWhiteSpace(this string value)
        {
            return value.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
        }

        public static string Salt(this string value, Guid salt)
        {
            // I'd like to point out that this could be done a lot better. -Ross 9/14/2017
            using (var crypto = new SHA512CryptoServiceProvider())
            {
                var pBytes =
                    Encoding.UTF8.GetBytes(value).Concat(salt.ToByteArray());
                var hashed = crypto.ComputeHash(pBytes.ToArray());
                var sb = new StringBuilder();
                foreach (var b in hashed)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// Returns true if the value is a properly formatted email address.
        /// </summary>
        public static bool IsValidEmail(this string value)
        {
            value = value.SanitizeAndDowncase();
            return value != null && VALID_EMAIL_REGEX.IsMatch(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="expectQuotes">
        /// If true, an ArgumentException will be thrown if the given string is not wrapped in quotes.
        /// </param>
        /// <returns></returns>
        public static string StripQuotes(this string value, bool expectQuotes = false)
        {
            if (STRIP_QUOTES_REGEX.IsMatch(value))
            {
                return STRIP_QUOTES_REGEX.Match(value).Groups[1].ToString();
            }

            if (expectQuotes)
            {
                throw new ArgumentException(
                    String.Format(
                        "The value '{0}' was not wrapped in quotation marks as expected.", value));
            }

            return value;
        }

        public static string Singularize(this string value, bool knownToBePlural = true)
        {
            return InflectorExtensions.Singularize(value, knownToBePlural);
        }

        public static string Pluralize(this string value, bool knownToBeSingular = true)
        {
            return InflectorExtensions.Pluralize(value, knownToBeSingular);
        }

        public static string ToPascalCase(this string value)
        {
            return value.Dehumanize().Replace(" ", "");
        }

        /// <summary>
        /// Lowercases the first letter. Does not expect spaces.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToCamelCase(this string value)
        {
            return value.Titleize().Camelize().Replace(" ", "");
        }

        private class SpaceTransformer : IStringTransformer
        {
            public string Transform(string value)
            {
                return String.Join(" ",
                    Regex
                       .Replace(value, "(^[a-z]+|[A-Z][a-z]*)", "$1 ")
                       .Split(' ').Where(s => s != String.Empty).ToArray());
            }
        }

        public static string ToLowerSpaceCase(this string value)
        {
            return value.Transform(new SpaceTransformer(), To.LowerCase);
        }

        /// <summary>
        /// Use this for strings like "I WANT This TitlE cased",
        /// becomes I Want This Title Cased
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToCultureTitleCase(this string value)
        {
            var artsAndPreps = new List<string> {
                "a", "an", "and", "any", "at", "for", "from", "in", "into",
                "of", "on", "or", "some", "the", "to"
            };
            var result =
                Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
            var words = result.Split(' ');
            for (var i = 1; i < words.Length - 1; i++)
                if (artsAndPreps.Contains(words[i].ToLower()))
                    words[i] = words[i].ToLower();

            return String.Join(" ", words);
        }

        /// <summary>
        /// Use this for string like ThisIsAThingINeedTitleCasedASAP,
        /// becomes This is a Thing I Need Title Cased ASAP
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToTitleCase(this string value)
        {
            var artsAndPreps = new List<string> {
                "a", "an", "and", "any", "at", "for", "from", "in", "into",
                "of", "on", "or", "some", "the", "to"
            };

            var result = Regex
               .Replace(value, TITLE_CASE_REGEX, " $1", RegexOptions.IgnorePatternWhitespace);

            var words = result.Split(' ');
            // skip the first and last words.
            for (var i = 1; i < words.Length - 1; i++)
                if (artsAndPreps.Contains(words[i].ToLower()))
                    words[i] = words[i].ToLower();

            return String.Join(" ", words);
        }

        public static string ToBase64String(this string value)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
        }

        /// <summary>
        /// Remove the http query string (starting with <c>?</c> and everything after to the right) from
        /// <paramref name="url"/>.
        /// </summary>
        public static string RemoveQueryString(this string url)
        {
            return Regex.Replace(url, "\\?.+$", string.Empty);
        }

        /// <summary>
        /// Remove the http fragment string (starting with <c>#</c> and everything after to the right) from
        /// <paramref name="url"/>.
        /// </summary>
        public static string RemoveFragmentString(this string url)
        {
            return Regex.Replace(url, "#.+$", string.Empty);
        }

        public static string ToSqlite(this string sql)
        {
            sql = Regex.Replace(sql, "isnull", "ifnull", RegexOptions.IgnoreCase);
            sql = Regex.Replace(sql, @"year\(getdate\(\)\)", "strftime('%Y', 'now')", RegexOptions.IgnoreCase);
            sql = Regex.Replace(sql, @"year\(([A-Za-z0-9]+)\.([A-Za-z0-9]+)\)",
                m => String.Format("strftime('%Y', {0}.{1})", m.Groups[1], m.Groups[2]), RegexOptions.IgnoreCase);
            sql = Regex.Replace(sql, @"month\(getdate\(\)\)", "strftime('%m', 'now')", RegexOptions.IgnoreCase);
            sql = Regex.Replace(sql, @"dateadd\(([A-Za-z0-9]+)", m => String.Format("dateadd('{0}'", m.Groups[1]),
                RegexOptions.IgnoreCase);
            sql = Regex.Replace(sql, @"datediff\(mm, ('?[A-Za-z0-9\._ =()]+'?), ('?[A-Za-z0-9\._ =()]+'?)\)",
                m =>
                    $"((strftime('%Y', {m.Groups[1]}) - strftime('%Y', {m.Groups[2]})) * 12) + (strftime('%m', {m.Groups[1]}) - strftime('%m', {m.Groups[2]}))",
                RegexOptions.IgnoreCase);
            sql = Regex.Replace(sql, @"datediff\(WW, ('?[A-Za-z0-9\._ =()]+'?), ('?[A-Za-z0-9\._ =()]+'?)\)",
                m => $"round((ceiling(julianday({m.Groups[1]})) - ceiling(julianday({m.Groups[2]}))) / 7)",
                RegexOptions.IgnoreCase);
            sql = Regex.Replace(sql, @"datediff\(D, ('?[A-Za-z0-9\._ =()]+'?), ('?[A-Za-z0-9\._ =()]+'?)\)",
                m => $"(julianday({m.Groups[1]}) - julianday({m.Groups[2]}))",
                RegexOptions.IgnoreCase);

            sql = Regex.Replace(sql, @"datediff\(hour, ('?[A-Za-z0-9\.]+'?), ('?[A-Za-z0-9\.]+'?)\)",
                m => String.Format("strftime('%H', {0}) - strftime('%H', {1})", m.Groups[1], m.Groups[2]));

            // For minutes we need to use %s because it gives total number of seconds.
            // %M only gives the minutes of 00-59 which is useless. -Ross 10/26/2016
            sql = Regex.Replace(sql, @"datediff\(minute, ('?[A-Za-z0-9\.]+'?), ('?[A-Za-z0-9\.]+'?)\)",
                m => String.Format("(strftime('%s', {0}) - strftime('%s', {1})) / 60", m.Groups[1], m.Groups[2]));
            sql = Regex.Replace(sql, @"cast\(getdate\(\) as date\)", "getdate()", RegexOptions.IgnoreCase);
            sql = Regex.Replace(sql, @"getdate\(\)", "date('now')", RegexOptions.IgnoreCase);
            return sql;
        }

        public static bool IsMatch(this string str, string pattern)
        {
            return new Regex(pattern).IsMatch(str);
        }

        public static string ReplaceRegex(this string str, string pattern, string replacement)
        {
            return new Regex(pattern).Replace(str, replacement);
        }

        public static string CoalesceAndTrim(this string str, string otherValue = "")
        {
            return (str ?? otherValue).Trim();
        }
    }
}
