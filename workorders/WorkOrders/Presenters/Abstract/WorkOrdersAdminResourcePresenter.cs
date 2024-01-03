using System;
using MMSINC.Data.Linq;
using MMSINC.Interface;

namespace WorkOrders.Presenters.Abstract
{
    public abstract class WorkOrdersAdminResourcePresenter<TEntity> : WorkOrdersResourcePresenter<TEntity>
        where TEntity : class, new()
    {
        #region Constructors

        protected WorkOrdersAdminResourcePresenter(IResourceView view, IRepository<TEntity> repository) : base(view, repository)
        {
        }

        #endregion

        #region Private Methods

        protected override void CheckUserSecurity()
        {
            if (!SecurityService.IsAdmin)
                throw new UnauthorizedAccessException("The current user is not set as an administrator, but adminstrative access is required.");
        }

        #endregion
    }
}
