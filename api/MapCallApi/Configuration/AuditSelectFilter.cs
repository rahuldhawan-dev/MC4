using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Utilities;
using NHibernate;
using StructureMap;

namespace MapCallApi.Configuration
{
    public class AuditSelectFilter : IActionFilter
    {
        #region Constants

        public static readonly string[] ACTIONS_TO_LOG = { "Show", "Index" };

        #endregion

        #region Fields

        protected readonly IContainer _container;

        #endregion

        #region Properties

        public User CurrentUser
        {
            get
            {
                try
                {
                    return _container.GetInstance<IAuthenticationService<User>>().CurrentUser;

                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        #endregion

        #region Constructors

        public AuditSelectFilter(IContainer container)
        {
            _container = container;
        }

        #endregion

        #region Exposed Methods

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //noop
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var actionName = filterContext.ActionDescriptor.ActionName;

            if (!ACTIONS_TO_LOG.Contains(actionName) || CurrentUser == null)
                return;

            var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var id = filterContext.RouteData.Values["Id"];

            var entry = new AuditLogEntry
            {
                EntityName = controllerName,
                AuditEntryType = actionName,
                User = CurrentUser,
                Timestamp = _container.GetInstance<IDateTimeProvider>().GetCurrentDate()
            };

            if (id != null && !String.IsNullOrWhiteSpace(id.ToString()))
            {
                int outId;
                if (Int32.TryParse(id.ToString(), out outId))
                {
                    entry.EntityId = outId;
                }
            }

            var session = _container.GetInstance<ISession>();
            session.Save(entry);
        }

        #endregion
    }
}