using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MMSINC
{
    public class ResourceRegistryDictionary : Dictionary<string, IHtmlString>
    {
        #region Constructor

        public ResourceRegistryDictionary()
            : base(StringComparer.InvariantCultureIgnoreCase) { }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns all of the resource values as one combined MvcHtmlString
        /// </summary>
        /// <returns></returns>
        public IHtmlString ToMvcHtmlString()
        {
            if (Count == 0)
            {
                // Let's not waste memory by making new strings.
                return MvcHtmlString.Empty;
            }

            if (Count == 1)
            {
                // Let's also not waste memory by making a new
                // string if we only have one.
                return this.First().Value;
            }

            var sb = new StringBuilder();
            foreach (var v in Values)
            {
                // Should append the non-html encoded string.
                sb.Append(v.ToString());
            }

            return new MvcHtmlString(sb.ToString());
        }

        public void SafeAdd(string url, IHtmlString tag)
        {
            if (!ContainsKey(url))
            {
                Add(url, tag);
            }
            else if (tag.ToString() != this[url].ToString())
            {
                throw new ArgumentException(
                    "An item with the same key has already been added, and the new value does not match the existing value for that key.");
            }
        }

        #endregion
    }
}
