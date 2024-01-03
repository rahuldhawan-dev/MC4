using System;
using System.Diagnostics;
using System.IO;
using System.Web.Mvc;
using System.Web.UI;
using MMSINC.Controllers;
using MMSINC.Metadata;

namespace MapCall.Common.Controllers
{
    // Session state MUST be Disabled. 

    // SessionStateBehavior.Required(and Default) significantly slows down page loads because
    // it requires a session lock on every request. This is only noticable after something
    // gets added to TempData for the first time, then every request after locks the session.

    // SessionStateBehavior.ReadOnly does not actually mean readonly. You can still write to 
    // the session and muck things up, but there's no session lock. This can cause TempData
    // to be entirely erased due to race conditions.
    [AllowAnonymous]
    [SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public class ResourcesController : MMSINC.Controllers.ControllerBase
    {
        #region Constants

        public const string RESOURCE_NAMESPACE_FORMAT =
                                "MapCall.Common.Content.{0}.{1}",
                            RESOURCE_NOT_FOUND_FORMAT = "The resource '{0}' could not be found.";

        #endregion

        protected override bool DisableAsyncSupport
        {
            get
            {
                // Test mode sets this to true on ControllerBase. However, that
                // slows down the tests significantly when there's multiple images
                // being loaded on a page. Since all access to this controller should
                // be anonymous and not require any database access whatsoever, we
                // can allow this to be async at all times. -Ross 1/27/2014
                return false;
            }
        }

        #region Private Methods

        protected ActionResult GetResource(ResourceType type, string file, bool isBinary)
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                return HttpNotFound();
            }

            try
            {
                if (isBinary)
                {
                    return new FileStreamResult(GetResourceStream(type, file), type.ToContentType());
                }

                using (var stream = GetResourceStream(type, file))
                using (var reader = new StreamReader(stream))
                using (var writer = new StringWriter())
                {
                    // Why are we copying the string into a writer first? 
                    writer.Write(reader.ReadToEnd());
                    return Content(writer.GetStringBuilder().ToString(), type.ToContentType());
                }
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception)
            {
                // TODO: test this case
                /* log */
            }

            return HttpNotFound();
        }

        protected Stream GetResourceStream(ResourceType type, string file)
        {
            file = file.Replace('/', '.');
            var fullName = String.Format(RESOURCE_NAMESPACE_FORMAT, type, file);
            var stream = typeof(ResourcesController)
                        .Assembly
                        .GetManifestResourceStream(fullName);

            if (null == stream)
            {
                throw new ArgumentException(String.Format(RESOURCE_NOT_FOUND_FORMAT, fullName), "file");
            }

            return stream;
        }

        #endregion

        #region Actions

        public ActionResult Css(string file, string v = null)
        {
            return GetResource(ResourceType.CSS, file, false);
        }

        public ActionResult Js(string file, string v = null)
        {
            return GetResource(ResourceType.JS, file, false);
        }

        [AlwaysCache]
        public ActionResult Png(string file, string v = null)
        {
            return GetResource(ResourceType.PNG, file, true);
        }

        [AlwaysCache]
        public ActionResult Gif(string file, string v = null)
        {
            return GetResource(ResourceType.GIF, file, true);
        }

        #endregion

        public ResourcesController(ControllerBaseArguments args) : base(args) { }
    }

    public enum ResourceType
    {
        CSS,
        JS,
        PNG,
        GIF
    }

    public static class ResourceTypeExtensions
    {
        #region Constants

        public struct ContentTypes
        {
            public const string CSS = "text/css",
                                JS = "text/javascript",
                                PNG = "image/png",
                                GIF = "image/gif";
        }

        #endregion

        #region Extension Methods

        public static string ToContentType(this ResourceType type)
        {
            switch (type)
            {
                case ResourceType.CSS:
                    return ContentTypes.CSS;
                case ResourceType.JS:
                    return ContentTypes.JS;
                case ResourceType.PNG:
                    return ContentTypes.PNG;
                case ResourceType.GIF:
                    return ContentTypes.GIF;
                default:
                    throw new InvalidOperationException();
            }
        }

        #endregion
    }
}
