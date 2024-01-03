using System.Web;
using System.Web.Mvc;
using MMSINC.Controllers;

namespace MapCall.Common.Controllers
{
    public class ErrorController : MMSINC.Controllers.ControllerBase
    {
        #region Constants

        public const string DEFAULT_NOT_FOUND_MESSAGE = "Not found";

        #endregion

        #region Exposed Methods

        [AllowAnonymous]
        public ActionResult Forbidden()
        {
            return View();
        }

        [AllowAnonymous]
        public ViewResult NotFound()
        {
            if (ViewData.Model == null)
            {
                var c = (string)RouteData.Values["controller"];
                var a = (string)RouteData.Values["action"];
                ViewData.Model =
                    new HandleErrorInfo(
                        new HttpException(404, DEFAULT_NOT_FOUND_MESSAGE), c, a);
            }

            return View();
        }

        #endregion

        public ErrorController(ControllerBaseArguments args) : base(args) { }
    }
}
