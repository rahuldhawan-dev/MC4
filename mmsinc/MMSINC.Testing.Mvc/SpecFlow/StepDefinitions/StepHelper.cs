using System.Collections.Generic;

namespace MMSINC.Testing.SpecFlow.StepDefinitions
{
    /// <summary>
    /// Really common methods used in a ton of step definitions.
    /// </summary>
    public static class StepHelper
    {
        #region Fields

        private static readonly Dictionary<string, bool> _truthyStrings = new Dictionary<string, bool> {
            {"can", true},
            {"can not", false},
            {"can't", false},

            {"do", true},
            {"do not", false},
            {"don't", false},

            {"does", true},
            {"does not", false},
            {"doesn't", false},

            {"is", true},
            {"isn't", false},
            {"is not", false},

            {"should", true},
            {"should not", false},
            {"shouldn't", false},
            {"shant", false},
        };

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns true if the word equals "can" or "should" or something else that is truthy.
        /// </summary>
        public static bool IsTruthy(string word)
        {
            if (_truthyStrings.ContainsKey(word))
            {
                return _truthyStrings[word];
            }

            throw new KeyNotFoundException(
                $"The word '{word}' isn't registered in the StepHelper._truthyStrings dictionary.");
        }

        #endregion
    }
}
