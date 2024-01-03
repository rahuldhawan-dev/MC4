using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using StructureMap;

namespace MapCallMVC.Configuration
{
    // This filter's purpose is only to audit that a user has viewed
    // the pdf of an image that exists. It should not audit images
    // that do not exist, and it should not audit just viewing the show
    // page for a thing.
    public class AuditImageFilter : IActionFilter
    {
        #region Private Members

        protected readonly IContainer _container;

        #endregion

        #region Constructors

        public AuditImageFilter(IContainer container)
        {
            _container = container;
        }

        #endregion

        #region Private Methods

        protected virtual void DoLogging(ActionExecutedContext filterContext, AssetImagePdfResult pdf)
        {
            var dtProvider = _container.GetInstance<IDateTimeProvider>();
            var curUser = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
           
            var uv = new UserViewed
            {
                User = curUser,
                ViewedAt = dtProvider.GetCurrentDate()
            };

            var model = pdf.Entity;

            var town = (Town)model.AsDynamic().Town;

            // ReSharper disable CanBeReplacedWithTryCastAndCheckForNull
            if (model is TapImage)
            {
                uv.TapImage = (TapImage)model;
            }
            else if (model is ValveImage)
            {
                uv.ValveImage = (ValveImage)model;
            }
            else if (model is AsBuiltImage)
            {
                uv.AsBuiltImage = (AsBuiltImage)model;
            }
            // ReSharper restore CanBeReplacedWithTryCastAndCheckForNull

            var repo = _container.GetInstance<IRepository<UserViewed>>();
            repo.Save(uv);
        }

        #endregion

        #region Exposed Methods

        public void OnActionExecuting(ActionExecutingContext filterContext) { }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!filterContext.ActionDescriptor.GetCustomAttributes(typeof(AuditImageAttribute), true).Any())
            {
                return;
            }

            // This attribute should only be placed on the ActionResult method for the
            // action that requires it.

            // 0. Check if there's a value for "logview", if it is anything other than "0"
            //    then do the logging. This includes null. You can log for null.

            if (filterContext.RequestContext.HttpContext.Request.QueryString["logview"] == "0")
            {
                return;
            }

            // Not entirely sure the context of where/when this would be set, but putting it
            // in here to future-proof. A canceled action won't have the result executed, so 
            // the user wouldn't actually view the pdf in this case.
            if (filterContext.Canceled)
            {
                return;
            }

            // 1. Ensure that the result is a PdfResult. No auditing if it's returning
            //    a 404 or an error or anything else.
            if (!(filterContext.Result is AssetImagePdfResult pdf))
            {
                return;
            }

            DoLogging(filterContext, pdf);
        }

        #endregion
    }
}