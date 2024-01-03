using System;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Presenter;
using WorkOrders.Model;
using WorkOrders.Presenters.Abstract;
using WorkOrders.Views.Restorations;

namespace WorkOrders.Presenters.Restorations
{
    public class RestorationResourceRPCPresenter : WorkOrdersResourceRPCPresenter<Restoration>
    {
        #region Private Members

        private IRestorationResourceRPCView _restorationResourceRPCView;
        private IRestorationDetailView _restorationDetailView;

        #endregion

        #region Properties

        protected IRestorationResourceRPCView RestorationResourceRPCView
        {
            get
            {
                if (_restorationResourceRPCView == null)
                    _restorationResourceRPCView = View as IRestorationResourceRPCView;
                return _restorationResourceRPCView;
            }
        }

        protected IRestorationDetailView RestorationDetailView
        {
            get
            {
                if (_restorationDetailView == null)
                    _restorationDetailView = DetailView as IRestorationDetailView;
                return _restorationDetailView;
            }
        }

        #endregion

        #region Constructors

        public RestorationResourceRPCPresenter(IResourceRPCView<Restoration> view, IRepository<Restoration> repository) : base(view, repository)
        {
        }

        #endregion

        #region Event Handlers

        protected override void DetailView_EditClicked(object sender, EventArgs e)
        {
            ChangeViewCommand(RPCCommands.Update);
        }

        public override void OnViewLoaded()
        {
            base.OnViewLoaded();

            if (RestorationResourceRPCView != null)
            {
                RestorationDetailView.WorkOrderID =
                    RestorationResourceRPCView.WorkOrderID;
            }
        }

        #endregion
    }
}
