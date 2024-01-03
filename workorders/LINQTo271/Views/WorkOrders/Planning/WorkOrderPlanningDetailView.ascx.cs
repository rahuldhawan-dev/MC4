using System;
using System.Web.UI.WebControls;
using LINQTo271.Controls.WorkOrders;
using LINQTo271.Views.Abstract;
using MMSINC.Controls;
using MMSINC.Interface;
using WorkOrders.Model;

namespace LINQTo271.Views.WorkOrders.Planning
{
    /// <summary>
    /// Planning Detail View 
    /// 
    /// </summary>
    //TODO: Fix the ternary operator that is rendering the notes.
    public partial class WorkOrderPlanningDetailView : WorkOrderDetailView
    {
        #region Constants

        //public struct WorkOrderParameterNames
        //{
        //    public const string NOTES = "Notes";
        //}

        #endregion

        #region Control Declarations

        protected IObjectContainerDataSource odsWorkOrder;
        protected IDetailControl fvWorkOrder;
        protected IWorkOrderInputFormView _wofvInitialInformation;

        protected IButton _btnEditInitialInfo,
                          _btnCancelInitialInfo;

        #endregion

        #region Properties

        public override Button CancelButton
        {
            get { return null; }
        }

        public override Button EditButton
        {
            get { return null; }
        }

        public override IDetailControl DetailControl
        {
            get { return fvWorkOrder; }
        }

        public override IObjectContainerDataSource DataSource
        {
            get { return odsWorkOrder; }
        }

        public override WorkOrderPhase Phase
        {
            get { return WorkOrderPhase.Planning; }
        }

        public IButton btnEditInitialInfo
        {
            get
            {
                if (_btnEditInitialInfo == null)
                    _btnEditInitialInfo =
                        fvWorkOrder.FindIControl<IButton>("btnEditInitialInfo");
                return _btnEditInitialInfo;
            }
        }

        public IButton btnCancelInitialInfo
        {
            get
            {
                if (_btnCancelInitialInfo == null)
                    _btnCancelInitialInfo =
                        fvWorkOrder.FindIControl<IButton>("btnCancelInitialInfo");
                return _btnCancelInitialInfo;
            }
        }

        public IWorkOrderInputFormView wofvInitialInformation
        {
            get
            {
                if (_wofvInitialInformation == null)
                    _wofvInitialInformation =
                        fvWorkOrder.FindIControl<IWorkOrderInputFormView>("wofvInitialInformation");
                return _wofvInitialInformation;
            }
        }

        #endregion

        #region Event Handlers

        protected void btnEditInitialInfo_Click(object sender, EventArgs e)
        {
            btnEditInitialInfo.Visible = false;
            btnCancelInitialInfo.Visible = true;
            wofvInitialInformation.ChangeMvpMode(DetailViewMode.Edit);
            wofvInitialInformation.WorkOrderID =
                (int)wofvInitialInformation.InnerDetailControl.DataKey.Value;
        }

        protected void btnCancelInitialInfo_Click(object sender, EventArgs e)
        {
            btnEditInitialInfo.Visible = true;
            btnCancelInitialInfo.Visible = false;
            wofvInitialInformation.ChangeMvpMode(DetailViewMode.ReadOnly);
            wofvInitialInformation.WorkOrderID =
                (int)wofvInitialInformation.InnerDetailControl.DataKey.Value;
        }

        #endregion

        #region Exposed Methods

        public override void SetViewControlsVisible(bool visible)
        {
            // noop (for now)
        }

        public override void SetViewMode(DetailViewMode newMode)
        {
            // only ever want to 'edit' from here:
            DetailControl.ChangeMvpMode(DetailViewMode.Edit);
        }

        #endregion
    }
}
