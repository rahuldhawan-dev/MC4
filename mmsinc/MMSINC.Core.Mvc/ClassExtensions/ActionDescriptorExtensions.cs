using System.Linq;
using System.Web.Mvc;

namespace MMSINC.ClassExtensions
{
    public static class ActionDescriptorExtensions
    {
        /// <summary>
        /// Returns the http verb required to access an action.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static HttpVerbs GetHttpVerb(this ActionDescriptor action)
        {
            // Okay so this is kinda garbage because none of the HttpGet/Post attributes inherit from a nice-to-use parent attribute.
            var verbAttr = action.GetCustomAttributes(true).OfType<ActionMethodSelectorAttribute>().ToArray();

            // NOTE: There's an AcceptVerbsAttribute which allows any old string. We don't use it, so it's not looked for here.
            if (verbAttr.OfType<HttpPostAttribute>().Any())
            {
                return HttpVerbs.Post;
            }
            else if (verbAttr.OfType<HttpDeleteAttribute>().Any())
            {
                return HttpVerbs.Delete;
            }
            else if (verbAttr.OfType<HttpPutAttribute>().Any())
            {
                return HttpVerbs.Put;
            }

            // NOTE: Not putting in support for HEAD/OPTIONS/PATCH since we never use them.
            // If we can't find anything useful, Get is probably what we need.
            return HttpVerbs.Get;
        }
    }
}
