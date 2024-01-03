using System;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.Controls;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using StructureMap;
using WorkOrders;
using WorkOrders.Model;

namespace LINQTo271.Controls.WorkOrders
{
    /// <summary>
    /// This form is for the user to add the materials they used to the work order.
    /// If the material is a stock material, they must select a stock location for it.
    /// If material is N/A then do not need to select a Stock Location.
    /// </summary>
    public partial class WorkOrderMaterialsUsedForm : WorkOrderDetailControlBase, IWorkOrderMaterialsUsedForm
    {
        #region Constants

        public struct ControlIDs
        {
            public const string MATERIAL_ID = "ddlPartNumber",
                                MATERIAL_ID_EDIT = "ddlPartNumberEdit",
                                DESCRIPTION = "txtNonStockDescription",
                                QUANTITY = "txtQuantity",
                                STOCK_LOCATION_ID = "ddlStockLocation",
                                STOCK_LOCATION_ID_EDIT = "ddlStockLocationEdit",
                                EDIT_LINK = "lbEdit",
                                DELETE_LINK = "lbDelete";
        }

        public struct MaterialsUsedParameterNames
        {
            public const string WORK_ORDER_ID = "workOrderID",
                                MATERIAL_ID = "materialID",
                                DESCRIPTION = "description",
                                QUANTITY = "quantity",
                                STOCK_LOCATION_ID = "stockLocationID";
        }

        public struct ParameterNames
        {
            public const string OPERATING_CENTER_ID = "OperatingCenterID";
        }

        #endregion

        #region Control Declarations

        protected IObjectDataSource odsMaterialsUsed,
                                    odsActiveStockLocations,
                                    odsAllStockLocations,
                                    odsOperatingCenterStockedMaterials;
        
        protected IGridView gvMaterialsUsed;

        #endregion

        #region Private Members

        protected string _materialID,
                         _materialIDEdit,
                         _description,
                         _quantity,
                         _stockLocationID,
                         _stockLocationIDEdit;
        protected IRepository<WorkOrder> _workOrderRepository;
        protected WorkOrder _workOrder;

        #endregion

        #region Properties

        #region Form Values

        public string MaterialID
        {
            get
            {
                if (_materialID == null)
                    _materialID =
                        gvMaterialsUsed.IFooterRow.FindIControl<IDropDownList>(
                            ControlIDs.MATERIAL_ID).SelectedValue;
                return _materialID;
            }
        }

        public string MaterialIDEdit
        {
            get
            {
                if (_materialIDEdit == null)
                    _materialIDEdit =
                        gvMaterialsUsed.IRows[gvMaterialsUsed.EditIndex].
                            FindIControl<IDropDownList>(
                            ControlIDs.MATERIAL_ID_EDIT).SelectedValue;
                return _materialIDEdit;
            }
        }

        public string Description
        {
            get
            {
                if (_description == null)
                    _description =
                        gvMaterialsUsed.IFooterRow.FindIControl<ITextBox>(
                            ControlIDs.DESCRIPTION).Text;
                return _description;
            }
        }

        public string Quantity
        {
            get
            {
                if (_quantity == null)
                    _quantity =
                        gvMaterialsUsed.IFooterRow.FindIControl<ITextBox>(
                            ControlIDs.QUANTITY).Text;
                return _quantity;
            }
        }

        public string StockLocationID
        {
            get
            {
                if (_stockLocationID == null)
                    _stockLocationID =
                        gvMaterialsUsed.IFooterRow.FindIControl<IDropDownList>(
                            ControlIDs.STOCK_LOCATION_ID).SelectedValue;
                return _stockLocationID;
            }
        }

        public string StockLocationIDEdit
        {
            get
            {
                if (_stockLocationIDEdit == null)
                    _stockLocationIDEdit =
                        gvMaterialsUsed.IRows[gvMaterialsUsed.EditIndex].
                            FindIControl<IDropDownList>(
                            ControlIDs.STOCK_LOCATION_ID_EDIT).SelectedValue;
                return _stockLocationIDEdit;
            }
        }

        #endregion

        public string TableCssClass
        {
            set { gvMaterialsUsed.CssClass = value; }
        }

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
            odsMaterialsUsed.SelectParameters["WorkOrderID"].DefaultValue =
                workOrderID.ToString();
        }

        private void ToggleEditAndInsertControls(bool visible)
        {
            if (gvMaterialsUsed.IFooterRow != null)
            {
                WireUpInsertButtonForAsyncPostBack();
                gvMaterialsUsed.IFooterRow.Visible = visible;
            }

            foreach (var row in gvMaterialsUsed.IRows)
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
            var gridview = gvMaterialsUsed as MvpGridView;
            if (gridview != null)
            {
                ScriptManager.RegisterClientScriptBlock(
                    gridview,
                    gridview.GetType(),
                    "materialsUsed" + UniqueID,
                    "WorkOrderMaterialsUsedForm.handleUpdatePanelCallback()",
                    true);
            }
            odsActiveStockLocations.SetDefaultSelectParameterValue(
                ParameterNames.OPERATING_CENTER_ID,
                WorkOrder.OperatingCenterID.ToString());
            odsAllStockLocations.SetDefaultSelectParameterValue(
                ParameterNames.OPERATING_CENTER_ID,
                WorkOrder.OperatingCenterID.ToString());
            odsOperatingCenterStockedMaterials.SetDefaultSelectParameterValue(
                ParameterNames.OPERATING_CENTER_ID,
                WorkOrder.OperatingCenterID.ToString());
            ToggleEditAndInsertControls(CurrentMvpMode !=
                                        DetailViewMode.ReadOnly && !WorkOrder.MaterialsApproved);

        }

        private void WireUpInsertButtonForAsyncPostBack()
        {
            if (Page != null)
            {
                var sm = ScriptManager.GetCurrent(Page);
                var btn =
                    gvMaterialsUsed.IFooterRow?.FindControl<MvpLinkButton>(
                        "lbInsert");
                if (btn != null)
                {
                    sm?.RegisterAsyncPostBackControl(btn);
                }
            }
        }

        #endregion

        #region Validation Events

        protected void ddlPartNumber_Validate(object source, ServerValidateEventArgs args)
        {
            //If PartNumber is filled in, stock should be too and vice versa.
            args.IsValid = (String.IsNullOrEmpty(MaterialID))
                               ? String.IsNullOrEmpty(StockLocationID)
                               : !String.IsNullOrEmpty(StockLocationID);
        }

        protected void ddlPartNumberEdit_Validate(object source, ServerValidateEventArgs args)
        {
            //If PartNumber is filled in, stock should be too and vice versa.
            args.IsValid = (String.IsNullOrEmpty(MaterialIDEdit))
                               ? String.IsNullOrEmpty(StockLocationIDEdit)
                               : !String.IsNullOrEmpty(StockLocationIDEdit);
        }

        #endregion

        #region Control Events

        protected void lbInsert_Click(object sender, EventArgs e)
        {
            if (IPage.IsValid)
            {
                odsMaterialsUsed.Insert();
            }
        }

        protected void lbCancel_Click(object sender, EventArgs e)
        {
        }

        protected void odsMaterialsUsed_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            e.InputParameters[MaterialsUsedParameterNames.WORK_ORDER_ID] =
                WorkOrderID;
            if (!String.IsNullOrEmpty(MaterialID))
                e.InputParameters[MaterialsUsedParameterNames.MATERIAL_ID] =
                    MaterialID;
            e.InputParameters[MaterialsUsedParameterNames.DESCRIPTION] =
                Description;
            e.InputParameters[MaterialsUsedParameterNames.QUANTITY] = Quantity;
            if (!String.IsNullOrEmpty(StockLocationID))
                e.InputParameters[MaterialsUsedParameterNames.STOCK_LOCATION_ID]
                    = StockLocationID;
        }

        #endregion

        #endregion
    }

    public interface IWorkOrderMaterialsUsedForm : IWorkOrderDetailControl
    {
        #region Properties

        string TableCssClass { set; }

        #endregion
    }
}
