using System.Net;
using System.Linq;
using System.Web.Mvc;
using MMSINC.ClassExtensions.ObjectExtensions;

namespace MMSINC.ErrorHandlers
{
    public class AjaxErrorHandlerAttribute : FilterAttribute, IExceptionFilter, IResultFilter
    {
        #region Consts

        public const string DEFAULT_GENERIC_ERROR_MESSAGE = "Internal server error.";

        private const string CONTENT_TYPE_APPLICATION_JSON = "application/json";

        #endregion

        #region Fields

        private string _genericErrorMessage;
        private HttpStatusCode _statusCode = HttpStatusCode.InternalServerError;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the generic message used when ShowExplicitErrorMessages is false.
        /// </summary>
        public string GenericErrorMessage
        {
            get
            {
                return (!string.IsNullOrWhiteSpace(_genericErrorMessage)
                    ? _genericErrorMessage
                    : DEFAULT_GENERIC_ERROR_MESSAGE);
            }
            set { _genericErrorMessage = value; }
        }

        /// <summary>
        /// Set this to true if the exception message should be sent to the client instead of some generic message.
        /// </summary>
        public bool ShowExplicitErrorMessages { get; set; }

        public HttpStatusCode StatusCode
        {
            get { return _statusCode; }
            set { _statusCode = value; }
        }

        #endregion

        #region Private Methods

        private bool ShouldRespondWithJson(ControllerContext context)
        {
            var req = context.HttpContext.Request;
            // AcceptTypes will end up being null instead of an empty array if, for some reason,
            // no acceptable types are sent with a request.
            return (req.IsAjaxRequest() ||
                    (req.AcceptTypes != null && req.AcceptTypes.Contains(CONTENT_TYPE_APPLICATION_JSON)));
        }

        #endregion

        #region Exposed Methods

        public void OnException(ExceptionContext filterContext)
        {
            // Verify if AJAX request
            if (ShouldRespondWithJson(filterContext))
            {
                filterContext.Result = new JsonHttpStatusCodeResult(_statusCode, (ShowExplicitErrorMessages
                    ? filterContext.Exception.Message
                    : GenericErrorMessage));

                // If ShowExplicit is true, then the yellow screen of death can be returned
                // and displayed. Ain't that neato! Not really. Anyway, we need to set
                // ExceptionHandled = true if we only want the client to recieve a short message
                // and not the whole shpeil.
                filterContext.ExceptionHandled = (!ShowExplicitErrorMessages);
            }
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.ThrowIfNull("filterContext");
            if (!ShouldRespondWithJson(filterContext))
            {
                return;
            }

            var result = filterContext.Result as HttpStatusCodeResult;
            // We definitely handle this if we've got a status code result with a 404
            if (result != null)
            {
                // Here we can use the StatusDescription as-is since it's always going to be a short string.
                // We don't need to worry about sending back detailed error messages here like we do 
                // in the OnException handler.
                filterContext.Result = new JsonHttpStatusCodeResult(_statusCode, result.StatusDescription);
                filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
            }
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
            // noop
        }

        #endregion
    }
}
