using System;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using LINQTo271.Common;
using MMSINC.Controls;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using Microsoft.Practices.Web.UI.WebControls;
using MMSINC.Utilities;
using StructureMap;
using WorkOrders;
using WorkOrders.Model;

namespace LINQTo271.Controls.WorkOrders
{
    public partial class WorkOrderInputFormView : WorkOrderDetailControlBase, IWorkOrderInputFormView
    {
        #region Constants

        public struct EntityKeys
        {
            //TODO: Fix, hacked 20090309 ARR
            public const string CREATOR_ID = "CreatorID",
                                SECONDARY_PHONE_NUMBER = "SecondaryPhoneNumber",
                                LATITUDE = "Latitude",
                                LONGITUDE = "Longitude",
                                OPERATING_CENTER_ID = "OperatingCenterID",
                                ASSET_TYPE_ID = "AssetTypeID",
                                VALVE_ID = "ValveID",
                                HYDRANT_ID = "HydrantID",
                                SEWER_OPENING_ID = "SewerOpeningID",
                                SERVICE_NUMBER = "ServiceNumber",
                                PREMISE_NUMBER = "PremiseNumber",
                                ALERT_ISSUED = "AlertIssued", 
                                ALERT_STARTED = "AlertStarted";
        }
        public const string ASSET_CONTROL_ID = "llpAsset";

        #endregion

        #region Control Declarations

        protected IButton btnSave, btnRemoveContractorAssignment;
        protected IDetailControl fvWorkOrder;
        protected IHiddenField _hidLatitude, _hidLongitude;
        protected IObjectContainerDataSource odsWorkOrder;
        protected IObjectDataSource odsTowns, odsEmployees;
        protected ILatLonPicker _assetPicker;
        protected IPlaceHolder phContractorAssigned, phEchoshoreLeakAlert;
        protected ILabel lblNotes, lblDateReceived;
        protected HyperLink hlCollector;
        protected MvpHyperLink hlEchoshoreLeakAlert;

        #endregion

        #region Private Members

        // TODO: This shouldn't need to be here
        protected IRepository<WorkOrder> _repository;

        #endregion

        #region Properties

        protected ILatLonPicker AssetPicker
        {
            get
            {
                if (_assetPicker == null)
                {
                    _assetPicker =
                        fvWorkOrder.FindIControl<ILatLonPicker>(ASSET_CONTROL_ID);
                }
                return _assetPicker;
            }
        }

        // TODO: This shouldn't need to be here
        protected IRepository<WorkOrder> Repository
        {
            get
            {
                if (_repository == null)
                    _repository =
                        DependencyResolver.Current.GetService<IRepository<WorkOrder>>();
                return _repository;
            }
        }

        public IDetailControl InnerDetailControl
        {
            get { return fvWorkOrder; }
        }

        public IObjectContainerDataSource InnerDataSource
        {
            get { return odsWorkOrder; }
        }

        public object DataSource
        {
            set { odsWorkOrder.DataSource = value; }
        }

        public string Latitude
        {
            get { return AssetPicker.Latitude.ToString(); }
        }

        public string Longitude
        {
            get { return AssetPicker.Longitude.ToString(); }
        }

        public override int WorkOrderID
        {
            get
            {
                //TODO: RPC Hack.  Checking the value of arg here denotes 'We're in a RPC view'
                //We return the base id here if we are in an rpc page because the InnerDetailControl.DataKey is
                //always null...
                if (CurrentMvpMode == DetailViewMode.Edit && IRequest.IQueryString.GetValue("arg") == null)
                    return (int)InnerDetailControl.DataKey.Value;
                
                return base.WorkOrderID;
            }
            set
            {
                base.WorkOrderID = value;
            }
        }

        public bool AllowNotesEdit
        {
            get { return CurrentMvpMode == DetailViewMode.Insert; }
        }

        #endregion

        #region Constructors

        public WorkOrderInputFormView()
        {
            _classDefaultViewMode = DetailViewMode.Insert;
        }

        #endregion

        #region Private Methods

        // TODO: This shouldn't need to be here
        protected override void SetDataSource(int workOrderID)
        {
            // this isn't really sound design, but it's testable and tested.
            DataSource = Repository.Get(workOrderID);
        }

        private void SetMode(DetailViewMode mode)
        {
            fvWorkOrder.ChangeMvpMode(mode);
            btnSave.Visible = (mode != DetailViewMode.ReadOnly);
        }

        #endregion

        #region Events

        public event EventHandler<ObjectContainerDataSourceStatusEventArgs>
            Updating , Deleting , Inserting;

        #endregion

        #region Event Handlers

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMode(CurrentMvpMode);
        }

        #endregion

        #region Data Events

        protected void fvWorkOrder_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            //TODO: Fix, hacked 20090309 ARR
            e.Values[EntityKeys.LATITUDE] = Latitude;
            e.Values[EntityKeys.LONGITUDE] = Longitude;

            // TODO: this sucks (less now).  (still?)fix it
            e.Values[EntityKeys.CREATOR_ID] = SecurityService.GetEmployeeID();

            if (e.Values[EntityKeys.ALERT_ISSUED] != null 
                && e.Values[EntityKeys.ALERT_ISSUED].ToString().ToUpper() == "TRUE")
            {
                e.Values[EntityKeys.ALERT_STARTED] = DependencyResolver.Current.GetService<IDateTimeProvider>().GetCurrentDate();
            }
        }

        protected void fvWorkOrder_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            //TODO: Fix, hacked 20090309 ARR
            e.NewValues[EntityKeys.LATITUDE] = Latitude;
            e.NewValues[EntityKeys.LONGITUDE] = Longitude;

            switch (Convert.ToInt32(e.NewValues[EntityKeys.ASSET_TYPE_ID]))
            {
                case AssetTypeRepository.Indices.VALVE:
                    e.NewValues[EntityKeys.HYDRANT_ID] = null;
                    e.NewValues[EntityKeys.SEWER_OPENING_ID] = null;
                    e.NewValues[EntityKeys.PREMISE_NUMBER] = null;
                    e.NewValues[EntityKeys.SERVICE_NUMBER] = null;
                    break;
                case AssetTypeRepository.Indices.HYDRANT:
                    e.NewValues[EntityKeys.VALVE_ID] = null;
                    e.NewValues[EntityKeys.SEWER_OPENING_ID] = null;
                    e.NewValues[EntityKeys.PREMISE_NUMBER] = null;
                    e.NewValues[EntityKeys.SERVICE_NUMBER] = null;
                    break;
                case AssetTypeRepository.Indices.SEWER_OPENING:
                    e.NewValues[EntityKeys.HYDRANT_ID] = null;
                    e.NewValues[EntityKeys.VALVE_ID] = null;
                    e.NewValues[EntityKeys.PREMISE_NUMBER] = null;
                    e.NewValues[EntityKeys.SERVICE_NUMBER] = null;
                    break;
                case AssetTypeRepository.Indices.SERVICE:
                    e.NewValues[EntityKeys.HYDRANT_ID] = null;
                    e.NewValues[EntityKeys.VALVE_ID] = null;
                    e.NewValues[EntityKeys.SEWER_OPENING_ID] = null;
                    break;
            }

            if (e.NewValues[EntityKeys.ALERT_ISSUED] != null)
            {
                if (e.NewValues[EntityKeys.ALERT_ISSUED].ToString().ToUpper() == "TRUE"
                    && e.OldValues[EntityKeys.ALERT_ISSUED].ToString().ToUpper() != "TRUE"
                    && String.IsNullOrEmpty(e.OldValues[EntityKeys.ALERT_STARTED].ToString()))
                {
                    e.NewValues[EntityKeys.ALERT_STARTED] =
                        DependencyResolver.Current.GetService<IDateTimeProvider>()
                            .GetCurrentDate();
                }
            }

            // TODO:  keep a running log of any changes to the work description (WorkDescriptionChanges)
        }

        protected void ods_Updated(object sender, ObjectContainerDataSourceStatusEventArgs e)
        {
            if (Updating != null)
                Updating(sender, e);
        }

        protected void ods_Inserted(object sender, ObjectContainerDataSourceStatusEventArgs e)
        {
            if (Inserting != null)
                Inserting(sender, e);
        }

        protected void ods_Deleted(object sender, ObjectContainerDataSourceStatusEventArgs e)
        {
            if (Deleting != null)
                Deleting(sender, e);
        }

        #endregion

        #region Control Events

        protected void btnSave_Click(object sender, EventArgs e)
        {
            switch (CurrentMvpMode)
            {
                case DetailViewMode.Edit:
                    fvWorkOrder.UpdateItem(true);
                    break;
                case DetailViewMode.Insert:
                    fvWorkOrder.InsertItem(true);
                    break;
            }
        }

        protected void btnRemoveContractorAssignment_Click(object sender, EventArgs e)
        {
            var wo = Repository.Get(WorkOrderID);
            wo.AssignedContractorID = null;
            Repository.UpdateCurrentEntityLiterally(wo);
            fvWorkOrder.FindControl<PlaceHolder>("phContractorAssigned").Visible = false;
        }

        protected void ddlOperatingCenter_DataBound(object sender, EventArgs e)
        {
            var ddl =
                fvWorkOrder.FindIControl<IDropDownList>("ddlOperatingCenter");
            if (SecurityService.UserOperatingCentersCount == 1)
                ddl.SelectedIndex = 1;
        }

        #endregion

        #endregion

        #region Exposed Methods

        public override void ChangeMvpMode(DetailViewMode newMode)
        {
            base.ChangeMvpMode(newMode);
            SetMode(newMode);
        }

        public override void InsertItem(bool causesValidation)
        {
            InnerDetailControl.InsertItem(causesValidation);
        }

        public override void UpdateItem(bool causesValidation)
        {
            InnerDetailControl.UpdateItem(causesValidation);
        }

        #endregion

    }

    public interface IWorkOrderInputFormView : IWorkOrderDetailControl
    {
        #region Properties

        object DataSource { set; }
        IDetailControl InnerDetailControl { get; }
        IObjectContainerDataSource InnerDataSource { get; }

        #endregion
    }
}
