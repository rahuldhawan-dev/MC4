using System.Web.Mvc;

namespace MMSINC.ClassExtensions
{
    public static class ViewDataDictionaryExtensions
    {
        /// <summary>
        /// Tries to get a value from ViewData if it exists, otherwise it returns the defaultValue.
        /// </summary>
        public static T GetValueOrDefault<T>(this ViewDataDictionary dict, string key, T defaultValue)
        {
            if (dict.ContainsKey(key))
            {
                return (T)dict[key];
            }

            return defaultValue;
        }
    }
}
