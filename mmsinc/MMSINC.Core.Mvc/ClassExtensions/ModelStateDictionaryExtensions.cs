using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace MMSINC.ClassExtensions
{
    public static class ModelStateExtensions
    {
        /// <summary>
        /// Converts a ModelStateDictionary into a RouteValueDictionary using its keys and AttemptedValue.
        /// </summary>
        /// <param name="modelStateDict"></param>
        /// <returns></returns>
        public static RouteValueDictionary ToRouteValueDictionary(this ModelStateDictionary modelStateDict)
        {
            var rvd = new RouteValueDictionary();

            foreach (var ms in modelStateDict)
            {
                // The documentation for AttemptedValue is dumb. AttemptedValue is the raw string
                // value as sent back from the client, either in the query string or postdata or 
                // whatever. RawValue is the string converted by the ValueProvider to whatever 
                // strong type is needed for the model. 
                if (!String.IsNullOrWhiteSpace(ms.Key) && ms.Value.Errors.Count == 0)
                    rvd.Add(ms.Key, ms.Value.Value.AttemptedValue);
            }

            return rvd;
        }

        /// <summary>
        /// Returns a dictionary of key value pairs for model state errors. NOTE that if a property has multiple errors 
        /// then those errors will be flattened into a single string.
        /// </summary>
        /// <param name="modelStateDict"></param>
        /// <returns></returns>
        public static IDictionary<string, string> ToDictionaryOfErrors(this ModelStateDictionary modelStateDict)
        {
            var errorDictionary = new Dictionary<string, string>();
            foreach (var ms in modelStateDict)
            {
                if (ms.Value.Errors.Any())
                {
                    errorDictionary.Add(ms.Key, string.Join(" ", ms.Value.Errors.Select(x => x.ErrorMessage)));
                }
            }

            return errorDictionary;
        }
    }
}
