using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MMSINC.Utilities
{
    /// <summary>
    /// A little utility class for getting words out of things. 
    /// </summary>
    public static class Wordify
    {
        // System.Data.Entity.Design.PluralizationServices
        // This is pretty usable for doing pluralization/singularizing of words
        // Should use it at some point. 

        // TODO: Might wanna look into caching some words so we don't have to repeatedly
        // re-process them over and over again. 

        // TODO: I'm sure this would be better Regexed. But this is Ross, and I don't
        //       get those things. 

        #region Private Methods

        private static IEnumerable<string> RejoinAcronyms(IEnumerable<string> words)
        {
            var curWords = words.ToArray(); // You can't access IEnumerable with an indexer, lame. 
            var newWords = new List<string>();

            var wordCount = curWords.Count();
            var curAcronym = "";

            Action addCurrentAcronym = () => {
                if (!string.IsNullOrWhiteSpace(curAcronym))
                {
                    newWords.Add(curAcronym);
                    curAcronym = string.Empty;
                }
            };

            for (var i = 0; i < wordCount; i++)
            {
                var cur = curWords[i];
                if (cur.Length > 1)
                {
                    addCurrentAcronym();
                    newWords.Add(cur);
                }
                else
                {
                    curAcronym += cur;
                }
            }

            addCurrentAcronym();
            return newWords;
        }

        #endregion

        #region Exposed Methods

        public static IEnumerable<string> GetWordsFromCamelCase(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return new string[] { };
            }

            // Trim cause we don't want spaces at the ends. 
            value = value.Trim();
            var words = Regex.Replace(value, "((?<=[a-z])[A-Z]|[A-Z](?=[a-z]))", " $1").Split(" ".ToCharArray());
            if (new Regex("[^\\w]").IsMatch(value))
            {
                throw new NotSupportedException(
                    "Whitespace? In MY camelCase? It's more likely than you think! Value: '" + value + "'");
            }

            return RejoinAcronyms(words);
        }

        /// <summary>
        /// Returns AThingLikeThis as A Thing Like This.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SpaceOutWordsFromCamelCase(string value)
        {
            return string.Join(" ", GetWordsFromCamelCase(value));
        }

        #endregion
    }
}
