using System.Web.Mvc;

namespace MapCallApi.Configuration
{
    public class TrySkipIisCustomErrorsFilter : IActionFilter
    {
        #region Exposed Methods

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //noop
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // instruct IIS to not return custom errors, instead return application error messages
            filterContext.RequestContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }

        #endregion
    }
}
