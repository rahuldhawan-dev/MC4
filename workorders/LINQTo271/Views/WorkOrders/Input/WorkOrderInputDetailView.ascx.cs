using System;
using LINQTo271.Common;
using LINQTo271.Controls.WorkOrders;
using LINQTo271.Views.Abstract;
using MMSINC.Common;
using MMSINC.Controls;
using MMSINC.Interface;
using Microsoft.Practices.Web.UI.WebControls;
using WorkOrders.Model;
using WorkOrders.Views.WorkOrders;

namespace LINQTo271.Views.WorkOrders.Input
{
    /// <summary>
    /// Detail View for the main initial input of Work Order information.  This is
    /// a special detail view in that the List View is a child, and triggers based
    /// off of asset selection to list historical orders with the same asset.
    /// </summary>
    public partial class WorkOrderInputDetailView : WorkOrderDetailView, IWorkOrderDetailView
    {
        #region Control Declarations

        protected IListView<WorkOrder> wolvWorkOrderHistory;
        protected IWorkOrderInputFormView fvWorkOrder;
        protected IWorkOrderDetailControl woDocumentForm;
        protected IHiddenField _hidLatitude, _hidLongitude;
        protected IDropDownList _ddlOperatingCenter;
        protected ILatLonPicker _latLonPicker;
        protected IPanel pnlDocumentTab;
        protected IPlaceHolder phDocumentTab;

        #endregion

        #region Constants

        public struct EntityKeys
        {
        	//TODO: Fix, hacked 20090309 ARR
            public const string CREATOR_ID = "CreatorID",
                                SECONDARY_PHONE_NUMBER = "SecondaryPhoneNumber",
                                LATITUDE = "Latitude",
                                LONGITUDE = "Longitude",
                                OPERATING_CENTER_ID = "OperatingCenterID";
        }
        public const string ASSET_CONTROL_ID = "llpAsset";
        public const string OPERATING_CENTER_DROPDOWN = "ddlOperatingCenter";

        #endregion

        #region Properties
        
        public override IDetailControl DetailControl
        {
            get{ return fvWorkOrder; }
        }

        public override IObjectContainerDataSource DataSource
        {
            get { return fvWorkOrder.InnerDataSource; }
        }

        public override WorkOrderPhase Phase
        {
            get { return WorkOrderPhase.Input; }
        }

        protected IHiddenField hidLatitude
        {
            get
            {
                if(_hidLatitude == null)
                {
                    _hidLatitude = latLonPicker.hidLatitude;
                }
                return _hidLatitude;
            }
        }

        protected IHiddenField hidLongitude
        {
            get
            {
                if (_hidLongitude == null)
                {
                    _hidLongitude = latLonPicker.hidLongitude;
                }
                return _hidLongitude;
            }
        }

        protected IDropDownList ddlOperatingCenter
        {
            get
            {
                if(_ddlOperatingCenter == null)
                {
                    _ddlOperatingCenter = fvWorkOrder.FindIControl<IDropDownList>(OPERATING_CENTER_DROPDOWN);
                }
                return _ddlOperatingCenter;
            }
        }

        protected ILatLonPicker latLonPicker
        {
            get
            {
                if(_latLonPicker == null)
                {
                    _latLonPicker =
                        fvWorkOrder.FindIControl<ILatLonPicker>(ASSET_CONTROL_ID);
                }
                return _latLonPicker;
            }
        }

        public override object CurrentDataKey
        {
            get { return fvWorkOrder.InnerDetailControl.DataKey.Value; }
        }

        #endregion

        #region Event Handlers

        #if !DEBUG

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            fvWorkOrder.InnerDetailControl.Visible = false;
        }

        #endif

        protected void ods_Inserting(object sender, ObjectContainerDataSourceStatusEventArgs e)
        {
            OnInserting(new EntityEventArgs<WorkOrder>((WorkOrder)e.Instance));
        }

        protected void ods_Updating(object sender, ObjectContainerDataSourceStatusEventArgs e)
        {
            OnUpdating(new EntityEventArgs<WorkOrder>((WorkOrder)e.Instance));
        }

        public override void SetViewMode(DetailViewMode newMode)
        {
            base.SetViewMode(newMode);

            if (newMode == DetailViewMode.Edit)
            {
                //TODO: Code review.  RPC Hack ahead. There is probably a better way to do this, but I haven't found it.
                //If we are in an RPC page this sets the workorderid from the querystring.
                if (IRequest.IQueryString.GetValue("arg") != null)
                    fvWorkOrder.WorkOrderID = int.Parse(IRequest.IQueryString.GetValue("arg"));

                woDocumentForm.WorkOrderID = fvWorkOrder.WorkOrderID;
            }

            //Docs should only show if we are editing.
            pnlDocumentTab.Visible = phDocumentTab.Visible = CurrentMode == DetailViewMode.Edit;
        }

        public override void SetViewControlsVisible(bool visible)
        {
            base.SetViewControlsVisible(visible);

            //RPC Page
            btnEdit.Visible = CurrentMode == DetailViewMode.ReadOnly;
        }

        #endregion
    }
}
