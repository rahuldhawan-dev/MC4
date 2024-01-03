using System.Net;
using System.Web.Mvc;

namespace MMSINC
{
    /// <summary>
    /// Class that gets around the Visual Studio Development Server(Cassini) eating the
    /// StatusDescription value. This may also be needed if this is used in a site that's
    /// not running in integrated mode on IIS.
    /// http://stackoverflow.com/questions/2708187/vs2010-development-web-server-does-not-use-integrated-mode-http-handlers-modules
    /// </summary>
    public class JsonHttpStatusCodeResult : HttpStatusCodeResult
    {
        #region Constructors

        public JsonHttpStatusCodeResult(HttpStatusCode statusCode) : this((int)statusCode) { }

        public JsonHttpStatusCodeResult(HttpStatusCode statusCode, string statusDescription) : this((int)statusCode,
            statusDescription) { }

        public JsonHttpStatusCodeResult(int statusCode)
            : base(statusCode) { }

        public JsonHttpStatusCodeResult(int statusCode, string statusDescription)
            : base(statusCode, statusDescription) { }

        #endregion

        #region Methods

        public override void ExecuteResult(ControllerContext context)
        {
            base.ExecuteResult(context);
            var resp = context.HttpContext.Response;
            resp.ContentType = "text/plain";
            resp.Write(StatusDescription);
        }

        #endregion
    }
}
