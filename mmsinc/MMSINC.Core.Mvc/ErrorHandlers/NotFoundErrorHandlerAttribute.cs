using System.Linq;
using System.Web;
using System.Web.Mvc;
using MMSINC.ClassExtensions.ObjectExtensions;

namespace MMSINC.ErrorHandlers
{
    /// <summary>
    /// MVC filter for handling 404 Not Found errors
    /// </summary>
    public class NotFoundErrorHandlerAttribute : FilterAttribute, IExceptionFilter, IResultFilter
    {
        #region Consts

        private const string CONTENT_TYPE_APPLICATION_JSON = "application/json",
                             DEFAULT_DEFAULT_MESSAGE = "The requested resource could not be found.";

        #endregion

        #region Properties

        /// <summary>
        /// Gets/sets the View that should be used for handling 404 errors.
        /// </summary>
        public string View { get; set; }

        public string DefaultErrorMessage { get; set; }

        /// <summary>
        /// Gets/sets whether this filter should be ignored for ajax requests. Default is false
        /// </summary>
        public bool HandleAjaxRequests { get; set; }

        #endregion

        #region Constructor

        public NotFoundErrorHandlerAttribute()
        {
            DefaultErrorMessage = DEFAULT_DEFAULT_MESSAGE;
        }

        #endregion

        #region Private Methods

        private void SetResponseThings(ControllerContext context, HttpException httpEx)
        {
            var resp = context.HttpContext.Response;
            resp.Clear();
            resp.StatusCode = httpEx.GetHttpCode();
            resp.TrySkipIisCustomErrors = true;
        }

        private bool ShouldRespondWithJson(ControllerContext context)
        {
            var req = context.HttpContext.Request;
            // AcceptTypes will end up being null instead of an empty array if, for some reason,
            // no acceptable types are sent with a request.
            return (req.IsAjaxRequest() ||
                    (req.AcceptTypes != null && req.AcceptTypes.Contains(CONTENT_TYPE_APPLICATION_JSON)));
        }

        #endregion

        #region Public Methods

        public virtual ViewResult CreateErrorResult(ControllerContext context, HttpException httpEx)
        {
            var controllerName = (string)context.RouteData.Values["controller"];
            var actionName = (string)context.RouteData.Values["action"];
            var model = new HandleErrorInfo(httpEx, controllerName, actionName);

            var result = new ViewResult {
                ViewName = View,
                ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                TempData = context.Controller.TempData
            };

            return result;
        }

        #region IExceptionFilter

        public void OnException(ExceptionContext filterContext)
        {
            filterContext.ThrowIfNull("filterContext");
            var httpEx = filterContext.Exception as HttpException;

            if (httpEx == null || httpEx.GetHttpCode() != 404)
            {
                // It's some other exception so we aren't handling it.
                return;
            }

            filterContext.Result = CreateErrorResult(filterContext, httpEx);
            filterContext.ExceptionHandled = true;
            SetResponseThings(filterContext, httpEx);
        }

        #endregion

        #region IResultFilter

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.ThrowIfNull("filterContext");
            if (!HandleAjaxRequests && ShouldRespondWithJson(filterContext))
            {
                return;
            }

            var result = filterContext.Result as HttpStatusCodeResult;
            // We definitely handle this if we've got a status code result with a 404
            if (result != null && result.StatusCode == 404)
            {
                // NOTE: If result.StatusDescription wasn't set to a custom message, it 
                //       gets set to 'An exception of type blah blah was thrown' and displayed
                //       to the user. 

                var statDesc = result.StatusDescription;
                if (string.IsNullOrEmpty(statDesc))
                {
                    statDesc = DefaultErrorMessage;
                }

                var httpEx = new HttpException(result.StatusCode, statDesc);
                filterContext.Result = CreateErrorResult(filterContext, httpEx);
                SetResponseThings(filterContext, httpEx);
                filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
            }
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
            // noop
        }

        #endregion

        #endregion
    }
}
