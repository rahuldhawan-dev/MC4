using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

// ReSharper disable CheckNamespace
namespace MMSINC.ClassExtensions.IOrderedDictionaryExtensions
    // ReSharper restore CheckNamespace
{
    public static class IOrderedDictionaryExtensions
    {
        #region Extension Methods

        public static void CleanValues(this IOrderedDictionary dict)
        {
            var keys = dict.Keys.ToArray(o => o.ToString());

            foreach (var key in keys)
            {
                if (dict[key] == null)
                    continue;
                var value = dict[key].ToString();

                dict[key] = CleanValue(value);
            }
        }

        #endregion

        #region Exposed Methods

        // This is needed to fix values that are databound to CascadingDropDowns.
        // Their SelectedValues are always in the format of <SelectedValue>:::<SelectedText>:::
        public static string CleanValue(string value)
        {
            const string pattern = ":::";

            if (value == null || value.IndexOf(pattern, StringComparison.Ordinal) == -1)
            {
                return value;
            }
            else if (value == pattern)
            {
                return null;
            }
            else
            {
                return Regex.Split(value, pattern)[0];
            }
        }

        #endregion
    }
}
