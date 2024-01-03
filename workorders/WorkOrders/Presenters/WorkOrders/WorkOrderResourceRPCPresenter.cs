using System;
using System.Collections;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Presenter;
using WorkOrders.Model;
using WorkOrders.Presenters.Abstract;
using WorkOrders.Views.WorkOrders;

namespace WorkOrders.Presenters.WorkOrders
{
    public class WorkOrderResourceRPCPresenter : WorkOrdersResourceRPCPresenter<WorkOrder>
    {
        #region Properties

        public IWorkOrderResourceRPCView ResourceView
        {
            get { return (IWorkOrderResourceRPCView)View; }
        }

        #endregion

        #region Constructors

        public WorkOrderResourceRPCPresenter(IResourceRPCView<WorkOrder> view, IRepository<WorkOrder> repository) : base(view, repository)
        {
        }

        #endregion

        #region Private Methods

        protected override void CheckUserSecurity()
        {
            if (ResourceView.Phase == WorkOrderPhase.General &&
                    (ResourceView.Command != null && ResourceView.Argument != null) &&
                    (new ArrayList {RPCCommands.Create, RPCCommands.Delete, RPCCommands.Update}.Contains(ResourceView.RPCCommand)) && 
                !SecurityService.IsAdmin)
                throw new UnauthorizedAccessException(
                    "The current user is not set as an administrator, but adminstrative access is required.");
        }

        #endregion

        #region Exposed Methods

        public override void SetRepositoryDataKeyFromListViewDataKey()
        {
            // noop shakalaka
        }

        #endregion

        #region Event Handlers

        protected override void DetailView_EditClicked(object sender, EventArgs e)
        {
            ChangeViewCommand(RPCCommands.Update);
        }

        #endregion
    }
}
