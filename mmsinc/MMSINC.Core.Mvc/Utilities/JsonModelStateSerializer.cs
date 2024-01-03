using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MMSINC.ClassExtensions.ObjectExtensions;

namespace MMSINC.Utilities
{
    /// <summary>
    /// Class for taking a ModelStateDictionary and getting a json serializable object.
    /// </summary>
    public class JsonModelStateSerializer
    {
        #region Properties

        /// <summary>
        /// Set to true if the ModelState's keys should be
        /// camelCased instead of PascalCase. Defaults to false.
        /// </summary>
        public bool CamelCaseKeys { get; set; }

        #endregion

        #region Exposed Methods

        internal string SerializeKey(string key)
        {
            if (!CamelCaseKeys)
            {
                return key;
            }

            var paths = key.Split('.');
            for (var i = 0; i < paths.Length; i++)
            {
                var cur = paths[i];
                paths[i] = cur[0].ToString().ToLowerInvariant() + cur.Substring(1);
            }

            return string.Join(".", paths);
        }

        /// <summary>
        /// Returns a dictionary serialized version of the given ModelState. This
        /// dictionary can then be passed to a regular json serializer. 
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, IEnumerable<string>> SerializeErrors(ModelStateDictionary modelState)
        {
            modelState.ThrowIfNull("modelState");
            // ModelState will have arrays for every single property, not just the ones with errors.
            // So we only want the errors cause otherwise we'd be sending back gigantic json objects.
            //
            // Also, we're only sending back what ErrorMessage is set to, if anything. We don't want
            // to send back the exception message since that could potentially leak secure information 
            // to the client.
            return modelState
                  .Where(kvp => kvp.Value.Errors.Any())
                  .ToDictionary(kvp => SerializeKey(kvp.Key), kvp => kvp.Value.Errors.Select(x => x.ErrorMessage));
        }

        #endregion
    }
}
