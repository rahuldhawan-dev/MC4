using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.Interface;
using WorkOrders.Library.Controls;
using WorkOrders.Library.Permissions;
using WorkOrders.Views.WorkOrders;
using SecurityServiceClass = WorkOrders.Library.Permissions.SecurityService;

namespace LINQTo271.Controls.WorkOrders
{
    public abstract class WorkOrderDetailControlBase : WorkOrdersMvpUserControl, IWorkOrderDetailControl
    {
        #region Constants

        public const DetailViewMode DEFAULT_VIEW_MODE = DetailViewMode.ReadOnly;
        public struct ViewStateKeys
        {
            public const string CURRENT_MVP_MODE = "CurrentMvpMode",
                                WORK_ORDER_ID = "WorkOrderID";
        }

        #endregion

        #region Private Members

        protected DetailViewMode? _classDefaultViewMode;
        protected ISecurityService _securityService;
        protected IWorkOrderDetailView _parentView;

        #endregion

        #region Properties

        protected ISecurityService SecurityService
        {
            get
            {
                if (_securityService == null)
                    _securityService = SecurityServiceClass.Instance;
                return _securityService;
            }
        }
        
        public virtual int WorkOrderID
        {
            get { return (int)IViewState.GetValue(ViewStateKeys.WORK_ORDER_ID); }
            set
            {
                IViewState.SetValue(ViewStateKeys.WORK_ORDER_ID, value);
                SetDataSource(value);
            }
        }

        public virtual DetailViewMode? InitialMode { get; set; }

        public virtual DetailViewMode CurrentMvpMode
        {
            get
            {
                var mode =
                    (DetailViewMode?)
                    IViewState.GetValue(ViewStateKeys.CURRENT_MVP_MODE);
                return mode ?? InitialMode ?? _classDefaultViewMode ?? DEFAULT_VIEW_MODE;
            }
            protected set { IViewState.SetValue(ViewStateKeys.CURRENT_MVP_MODE, value); }
        }

        public DataKey DataKey
        {
            get { return null; }
        }

        public object DataItem
        {
            get { return null; }
        }

        public IWorkOrderDetailView ParentView
        {
            get
            {
                if (_parentView == null)
                {
                    _parentView = GetParentView();
                }
                return _parentView;
            }
        }

        #endregion

        #region Protected Abstract Methods

        protected abstract void SetDataSource(int workOrderID);

        #endregion

        #region Protected Methods

        protected IWorkOrderDetailView GetParentView()
        {
            var curObject = Parent;

            while (curObject as IWorkOrderDetailView == null && curObject.Parent != null)
            {
                curObject = curObject.Parent;
            }

            return curObject as IWorkOrderDetailView;
        }

        #endregion

        #region Exposed Methods

        public virtual void UpdateItem(bool causesValidation)
        {
            throw new NotImplementedException();
        }

        public virtual void InsertItem(bool causesValidation)
        {
            throw new NotImplementedException();
        }

        public virtual void DeleteItem()
        {
            throw new NotImplementedException();
        }

        public virtual void ChangeMvpMode(DetailViewMode newMode)
        {
            CurrentMvpMode = newMode;
        }

        #endregion
    }

    public interface IWorkOrderDetailControl : IDetailControl
    {
        #region Properties

        int WorkOrderID { get; set; }
        IWorkOrderDetailView ParentView { get; }

        #endregion
    }
}
