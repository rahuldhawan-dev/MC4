using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MMSINC.ClassExtensions.ObjectExtensions;

namespace MMSINC.Results
{
    public class ResponseFormatter
    {
        #region Consts

        /// <summary>
        /// The parameter name needed in a mapped route in order for this to function.
        /// This is also needed for route value data when generating urls and what not
        /// so that it picks the correct route.
        /// </summary>
        public const string ROUTE_EXTENSION_PARAMETER_NAME = "ext";

        /// <summary>
        /// Known route extensions that match a specific response format
        /// </summary>
        public struct KnownExtensions
        {
            // Internal because no one should be setting route data 
            // directly with using { ext: NO_EXTENSION }. Just don't put 
            // ext at all.
            internal const string NO_EXTENSION = "";

            public const string CALENDAR = "cal",
                                EXCEL_2003 = "xls",
                                FRAGMENT = "frag",
                                JSON = "json",
                                MAP = "map",
                                PDF = "pdf";
        }

        #endregion

        #region Fields

        private readonly Dictionary<string, Func<ActionResult>> _responders =
            new Dictionary<string, Func<ActionResult>>(StringComparer.OrdinalIgnoreCase);

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new ResponseFormatter instance with the given initializer. 
        /// </summary>
        /// <param name="initializer"></param>
        public ResponseFormatter(Action<ResponseFormatter> initializer)
        {
            initializer.ThrowIfNull("initializer");
            initializer(this);
        }

        #endregion

        #region Private Methods

        private string GetExtension(ControllerContext context)
        {
            var ext = (string)context.RouteData.Values[ROUTE_EXTENSION_PARAMETER_NAME];
            if (!string.IsNullOrWhiteSpace(ext))
            {
                return ext;
            }

            return KnownExtensions.NO_EXTENSION;
        }

        /// <summary>
        /// Returns the ActionResult based on the route extension. If the route
        /// has an unregistered extension, an HttpNotFoundResult is returned. 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public ActionResult GetActionResult(ControllerContext context)
        {
            var ext = GetExtension(context);
            if (_responders.ContainsKey(ext))
            {
                return _responders[ext]();
            }

            return new HttpNotFoundResult("Not found.");
        }

        #endregion

        #region Public Methods

        #region Responders

        /// <summary>
        /// Adds a responder for a given route extension
        /// </summary>
        /// <param name="ext">The extension(without the dot)</param>
        /// <param name="responder"></param>
        public void RespondTo(string ext, Func<ActionResult> responder)
        {
            _responders.Add(ext, responder);
        }

        public void Calendar(Func<ActionResult> responder)
        {
            RespondTo(KnownExtensions.CALENDAR, responder);
        }

        /// <summary>
        /// Adds a responder for requests that request an excel file.
        /// </summary>
        public void Excel(Func<ActionResult> responder)
        {
            RespondTo(KnownExtensions.EXCEL_2003, responder);
        }

        /// <summary>
        /// Adds a responder for requests that request an html fragment.
        /// </summary>
        public void Fragment(Func<ActionResult> responder)
        {
            RespondTo(KnownExtensions.FRAGMENT, responder);
        }

        /// <summary>
        /// Adds a responder for requests that request json data.
        /// </summary>
        public void Json(Func<ActionResult> responder)
        {
            RespondTo(KnownExtensions.JSON, responder);
        }

        /// <summary>
        /// Adds a responder for requests that request json data.
        /// </summary>
        public void Pdf(Func<ActionResult> responder)
        {
            RespondTo(KnownExtensions.PDF, responder);
        }

        /// <summary>
        /// Adds a responder for a request that is requesting a regular full html page.
        /// </summary>
        public void View(Func<ActionResult> responder)
        {
            RespondTo(KnownExtensions.NO_EXTENSION, responder);
        }

        #endregion

        #endregion
    }
}
