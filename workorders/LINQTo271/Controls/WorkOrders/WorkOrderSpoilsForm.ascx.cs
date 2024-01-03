using System;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using MMSINC.Controls;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using StructureMap;
using WorkOrders;
using WorkOrders.Model;

namespace LINQTo271.Controls.WorkOrders
{
    public partial class WorkOrderSpoilsForm : WorkOrderDetailControlBase
    {
        #region Constants

        public struct ControlIDs
        {
            public const string QUANTITY = "txtQuantity",
                                SPOIL_STORAGE_LOCATION =
                                    "ddlSpoilStorageLocation",
                                EDIT_LINK = "lbEdit",
                                DELETE_LINK = "lbDelete";
        }

        public struct SpoilsParameterNames
        {
            public const string WORK_ORDER_ID = "WorkOrderID",
                                SPOIL_TYPE_ID = "SpoilTypeID",
                                QUANTITY = "Quantity",
                                SPOIL_STORAGE_LOCATION_ID =
                                    "SpoilStorageLocationID",
                                OPERATING_CENTER_ID = "OperatingCenterID";

        }

        #endregion

        #region Control Declarations

        protected IObjectDataSource odsSpoils,
                                    odsOperatingCenterSpoilStorageLocations;
        protected IGridView gvSpoils;

        #endregion

        #region Private Members

        protected string _quantity, _spoilStorageLocationID;
        protected IRepository<WorkOrder> _workOrderRepository;
        protected WorkOrder _workOrder;

        #endregion

        #region Properties

        #region Form Values

        public string Quantity
        {
            get
            {
                if (_quantity == null)
                    _quantity =
                        gvSpoils.IFooterRow.FindIControl<ITextBox>(
                            ControlIDs.QUANTITY).Text;
                return _quantity;
            }
        }

        public string SpoilStorageLocationID
        {
            get
            {
                if (_spoilStorageLocationID == null)
                    _spoilStorageLocationID =
                        gvSpoils.IFooterRow.FindIControl<IDropDownList>(
                            ControlIDs.SPOIL_STORAGE_LOCATION).SelectedValue;
                return _spoilStorageLocationID;
            }
        }

        #endregion

        public IRepository<WorkOrder> WorkOrderRepository
        {
            get
            {
                if (_workOrderRepository == null)
                    _workOrderRepository =
                        DependencyResolver.Current.GetService<IRepository<WorkOrder>>();
                return _workOrderRepository;
            }
        }

        public WorkOrder WorkOrder
        {
            get
            {
                if (_workOrder == null)
                    _workOrder = WorkOrderRepository.Get(WorkOrderID);
                return _workOrder;
            }
        }

        #endregion

        #region Private Methods

        // TODO: this shouldn't need to be here
        protected override void SetDataSource(int workOrderID)
        {
            odsSpoils.SelectParameters["WorkOrderID"].DefaultValue =
                workOrderID.ToString();
        }

        private void ToggleEditAndInsertControls(bool visible)
        {
            if (gvSpoils.IFooterRow != null)
            {
                gvSpoils.IFooterRow.Visible = visible;
            }

            foreach (var row in gvSpoils.IRows)
            {
                ToggleEditControlsInRow(row, visible);
            }
        }

        #endregion

        #region Private Static Methods

        private static void ToggleEditControlsInRow(IGridViewRow row, bool visible)
        {
            var lbEdit =
                row.FindIControl<ILinkButton>(ControlIDs.EDIT_LINK);
            var lbDelete =
                row.FindIControl<ILinkButton>(ControlIDs.DELETE_LINK);

            if (lbEdit != null && lbDelete != null)
            {
                lbEdit.Visible = lbDelete.Visible = visible;
            }
        }

        #endregion

        #region Event Handlers

        #region Page Events

        protected override void Page_Prerender(object sender, EventArgs e)
        {
            base.Page_Prerender(sender, e);

            odsOperatingCenterSpoilStorageLocations.SetDefaultSelectParameterValue(
                SpoilsParameterNames.OPERATING_CENTER_ID,
                WorkOrder.OperatingCenterID.ToString());
            ToggleEditAndInsertControls(CurrentMvpMode !=
                                        DetailViewMode.ReadOnly);
        }

        #endregion

        #region Control Events

        protected void lbInsert_Click(object sender, EventArgs e)
        {
            if (IPage.IsValid)
            {
                odsSpoils.Insert();
            }
        }

        protected void lbCancel_Click(object sender, EventArgs e)
        {
        }

        protected void odsSpoils_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            e.InputParameters[SpoilsParameterNames.WORK_ORDER_ID] = WorkOrderID;
            e.InputParameters[SpoilsParameterNames.QUANTITY] = Quantity;
            e.InputParameters[SpoilsParameterNames.SPOIL_STORAGE_LOCATION_ID] =
                SpoilStorageLocationID;
        }

        #endregion

        #endregion
    }
}