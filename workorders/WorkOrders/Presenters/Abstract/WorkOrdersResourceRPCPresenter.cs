using System;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Presenter;
using MMSINC.Utilities.Permissions;
using SecurityServiceClass = WorkOrders.Library.Permissions.SecurityService;

namespace WorkOrders.Presenters.Abstract
{
    public abstract class WorkOrdersResourceRPCPresenter<TEntity> : ResourceRPCPresenter<TEntity>
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

        protected WorkOrdersResourceRPCPresenter(IResourceRPCView<TEntity> view, IRepository<TEntity> repository) : base(view, repository)
        {
        }

        #endregion

        #region Private Methods

        protected override void CheckUserSecurity()
        {
            if (!SecurityService.UserHasAccess)
                throw new UnauthorizedAccessException("The Current User has not been granted access to the Work Management system.");
        }

        #endregion

        protected override void ProcessCommandAndArgument()
        {
            if (RPCView.Command == null)
            {
                // we weren't given a command, lets get out of here.
                RPCView.Redirect("WorkOrderGeneralResourceView.aspx");
            }
            else
            {
                base.ProcessCommandAndArgument();
            }
        }
    }
}
