using System;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Presenter;
using MMSINC.Utilities.Permissions;
using SecurityServiceClass = WorkOrders.Library.Permissions.SecurityService;

namespace WorkOrders.Presenters.Abstract
{
    public abstract class WorkOrdersResourcePresenter<TEntity> : ResourcePresenter<TEntity>
        where TEntity : class, new()
    {
        #region Properties

        protected override ISecurityService SecurityService
        {
            get
            {
                if (_securityService == null)
                    _securityService = SecurityServiceClass.Instance;
                return _securityService;
            }
        }

        #endregion

        #region Constructors

        public WorkOrdersResourcePresenter(IResourceView view, IRepository<TEntity> repository)
            : base(view, repository)
        {
        }

        #endregion

        #region Private Methods

        protected override void CheckUserSecurity()
        {
            // TODO: Is this necessary here?  Maybe we can do that inside the security service.
            if (SecurityService.CurrentUser == null)
                throw new UnauthorizedAccessException("SecurityService has not been initialized with the current user.");

            // If you're getting this error locally, you need to make sure the applicationhost.config is setup like 
            // the wiki: http://hsyplsvns001.amwater.net:81/mapcall/mapcall_workorders/wikis/home
            if (!SecurityService.UserHasAccess)
                throw new UnauthorizedAccessException("The Current User has not been granted access to the Work Management system.");
        }

        #endregion
    }
}
 