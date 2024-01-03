using System.Text.RegularExpressions;

// ReSharper disable once CheckNamespace
namespace MMSINC.ClassExtensions.RegexExtensions
{
    public static class RegexExtensions
    {
        #region Exposed Methods

        /// <summary>
        /// Tries to match to the given string.  If a match is found itse set to the out
        /// parameter and true is returned, else the match remains mull and false is
        /// returned.
        /// </summary>
        public static bool TryMatch(this Regex regex, string str, out Match match)
        {
            if (regex.IsMatch(str))
            {
                match = regex.Match(str);
                return true;
            }

            match = null;
            return false;
        }

        /// <summary>
        /// Tries to get the value of the indexed matched group, else returns null.
        /// </summary>
        public static string TryGetGroup(this Regex regex, string str, int group)
        {
            Match match;
            return string.IsNullOrEmpty(str) || !regex.TryMatch(str, out match) ? null : match.Groups[@group].Value;
        }

        #endregion
    }
}
