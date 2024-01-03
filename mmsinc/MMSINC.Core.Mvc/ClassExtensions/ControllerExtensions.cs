using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MMSINC.Common;
using MMSINC.Results;
using MMSINC.Utilities.Excel;

namespace MMSINC.ClassExtensions
{
    public static class ControllerExtensions
    {
        #region Conststants

        public struct TempDataKeys
        {
            public const string REDIRECT_URL = "REDIRECT_URL";
        }

        public struct Urls
        {
            public const string FORBIDDEN = "~/Error/Forbidden";
        }

        #endregion

        #region Exstension Methods

        #region Calendar

        public static CalendarResult<T> Calendar<T>(this Controller controller, IEnumerable<T> model,
            Func<T, CalendarItem> conversion, bool allowGet = false)
        {
            return new CalendarResult<T>(model, conversion) {
                JsonRequestBehavior = allowGet ? JsonRequestBehavior.AllowGet : JsonRequestBehavior.DenyGet
            };
        }

        #endregion

        #region Excel

        /// <summary>
        /// Returns an ExcelResult with the given model used for data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="model">The data to be exported.</param>
        /// <param name="args">Optional parameters for how the worksheet should be created.</param>
        /// <returns></returns>
        public static ExcelResult Excel<T>(this Controller controller, IEnumerable<T> model,
            ExcelExportSheetArgs args = null)
        {
            return new ExcelResult().AddSheet(model, args);
        }

        #endregion

        #region HttpStatusCode

        public static HttpStatusCodeResult HttpStatusCode(this ControllerBase controller, HttpStatusCode statusCode)
        {
            return new HttpStatusCodeResult((int)statusCode);
        }

        #endregion

        #region SetRedirectUrl

        public static ControllerBase SetRedirectUrl(this ControllerBase controller, string redirectUrl)
        {
            controller.TempData[TempDataKeys.REDIRECT_URL] = redirectUrl;

            return controller;
        }

        #endregion

        #region Forbidden

        public static RedirectResult Forbidden(this ControllerBase controller)
        {
            return new RedirectResult(Urls.FORBIDDEN);
        }

        #endregion

        #region GetReflectedControllerDescriptor

        /// <summary>
        /// Returns a new ReflectedControllerDescriptor for this controller. Can be cached because it's not reliant
        /// on the controller/request instance.
        /// </summary>
        public static ReflectedControllerDescriptor GetReflectedControllerDescriptor(this IController controller)
        {
            return new ReflectedControllerDescriptor(controller.GetType());
        }

        #endregion

        #region RespondTo

        [DebuggerStepThrough]
        public static ActionResult RespondTo(this Controller controller, Action<ResponseFormatter> formatter)
        {
            var rf = new ResponseFormatter(formatter);
            return rf.GetActionResult(controller.ControllerContext);
        }

        #endregion

        #region ViewExists

        public static bool ViewExists(this Controller controller, string name)
        {
            return ViewEngines.Engines.FindView(controller.ControllerContext, name, null).View != null;
        }

        #endregion

        #endregion
    }
}
