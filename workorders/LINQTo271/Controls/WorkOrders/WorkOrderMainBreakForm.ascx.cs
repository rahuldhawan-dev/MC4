using System;
using System.Web.UI.WebControls;
using MMSINC.ClassExtensions;
using MMSINC.Controls;
using MMSINC.Interface;
using WorkOrders.Model;

namespace LINQTo271.Controls.WorkOrders
{
    public partial class WorkOrderMainBreakForm : WorkOrderDetailControlBase, IWorkOrderMainBreakForm
    {
        #region Constants

        public struct MainBreakParameterNames
        {
            public const string WORK_ORDER_ID = "workOrderID",
                                MAIN_BREAK_MATERIAL_ID = "mainBreakMaterialID",
                                MAIN_CONDITION_ID = "mainConditionId",
                                MAIN_FAILURE_TYPE_ID = "mainFailureTypeID",
                                DEPTH = "depth",
                                MAIN_BREAK_SOIL_CONDITION_ID = "mainBreakSoilConditionID",
                                CUSTOMERS_AFFECTED = "customersAffected",
                                SHUTDOWN_TIME = "shutdownTime",
                                MAIN_BREAK_DISINFECTION_METHOD_ID = "mainBreakDisinfectionMethodID",
                                MAIN_BREAK_FLUSH_METHOD_ID = "mainBreakFlushMethodID",
                                CHLORINE_RESIDUAL = "chlorineResidual",
                                BOIL_ALERT_ISSUED = "boilAlertIssued",
                                SERVICE_SIZE_ID = "serviceSizeID", 
                                FOOTAGE_REPLACED = "footageReplaced",
                                REPLACED_WITH_ID = "replacedWithId";
        }

        public struct ControlIDs
        {
            public const string EDIT_LINK = "lbEdit",
                                DELETE_LINK = "lbDelete",
                                MATERIAL = "ddlMaterial",
                                MAIN_CONDITION = "ddlCondition",
                                FAILURE_TYPE = "ddlFailureType",
                                DEPTH = "txtDepth",
                                SOIL_CONDITION = "ddlSoilCondition",
                                CUSTOMERS_AFFECTED = "txtCustomersAffected",
                                SHUTDOWN_TIME = "txtShutdownTime",
                                DISINFECTION_METHOD = "ddlDisinfectionMethod",
                                FLUSH_METHOD = "ddlFlushMethod",
                                CHLORINE_RESIDUAL = "txtChlorineResidual",
                                BOIL_ALERT_ISSUED = "cbBoilAlertIssued",
                                SERVICE_SIZE = "ddlServiceSize",
                                FOOTAGE_REPLACED = "txtFootageReplaced", 
                                LBL_WORK_DESCRIPTION_ID = "lblWorkDescriptionID",
                                FOOTAGE_REPLACED_REQUIRED_VALIDATOR = "rfvFootageReplaced",
                                REPLACED_WITH_VALIDATOR = "rfvReplacedWithId",
                                REPLACED_WITH = "ddlReplacedWith";
        }
        
        #endregion

        #region Private Members

        private int _mainBreakMaterialID,
                    _mainConditionID,
                    _mainFailureTypeID,
                    _mainBreakSoilConditionID,
                    _customersAffected,
                    _mainBreakDisinfectionMethodID,
                    _mainBreakFlushMethodID,
                    _serviceSizeID, 
                    _workDescriptionID;

        private int? _footageReplaced, _replacedWithId;

        private decimal _depth,
                        _shutdownTime;
        private decimal? _chlorineResidual;

        private bool _boilAlertIssued;

        protected IObjectDataSource odsMainBreak;
        protected IGridView gvMainBreak;
        protected ICheckBox cbBoilAlertIssued;
        protected ILabel lblWorkDescriptionID;
        protected IRequiredFieldValidator rfvFootageReplaced;
       
        #endregion

        #region Properties

        public int MainBreakMaterialID
        {
            get
            {
                Console.Write("MainBreakMaterialID was hit.");
                if (_mainBreakMaterialID == 0)
                    _mainBreakMaterialID = int.Parse((gvMainBreak.IFooterRow.FindIControl<IDropDownList>(ControlIDs.MATERIAL)).SelectedValue);
                return _mainBreakMaterialID;
            }
        }

        public int MainConditionID
        {
            get
            {
                Console.Write("MainConditionID was hit.");
                if (_mainConditionID == 0)
                    _mainConditionID = int.Parse((gvMainBreak.IFooterRow.FindIControl<IDropDownList>(ControlIDs.MAIN_CONDITION)).SelectedValue);
                return _mainConditionID;
            }
        }

        public int MainFailureTypeID
        {
            get
            {
                Console.Write("MainFailureTypeID was hit.");
                if (_mainFailureTypeID == 0)
                    _mainFailureTypeID = int.Parse((gvMainBreak.IFooterRow.FindIControl<IDropDownList>(ControlIDs.FAILURE_TYPE)).SelectedValue);
                return _mainFailureTypeID;
            }
        }

        public decimal Depth
        {
            get
            {
                Console.Write("Depth was hit.");
                if (_depth == 0)
                    _depth = decimal.Parse((gvMainBreak.IFooterRow.FindIControl<ITextBox>(ControlIDs.DEPTH)).Text);
                return _depth;
            }
        }

        public int MainBreakSoilConditionID
        {
            get
            {
                if (_mainBreakSoilConditionID == 0)
                    _mainBreakSoilConditionID = int.Parse((gvMainBreak.IFooterRow.FindIControl<IDropDownList>(ControlIDs.SOIL_CONDITION)).SelectedValue);
                return _mainBreakSoilConditionID;
            }
        }

        public int CustomersAffected
        {
            get
            {
                if (_customersAffected == 0)
                    _customersAffected = int.Parse((gvMainBreak.IFooterRow.FindIControl<ITextBox>(ControlIDs.CUSTOMERS_AFFECTED)).Text);
                return _customersAffected;
            }
        }

        public decimal ShutdownTime
        {
            get
            {
                if (_shutdownTime == 0)
                    _shutdownTime = decimal.Parse((gvMainBreak.IFooterRow.FindIControl<ITextBox>(ControlIDs.SHUTDOWN_TIME)).Text);
                return _shutdownTime;
            }
        }

        public int MainBreakDisinfectionMethodID
        {
            get
            {
                if (_mainBreakDisinfectionMethodID == 0)
                    _mainBreakDisinfectionMethodID = int.Parse((gvMainBreak.IFooterRow.FindIControl<IDropDownList>(ControlIDs.DISINFECTION_METHOD)).SelectedValue);
                return _mainBreakDisinfectionMethodID;
            }
        }

        public int MainBreakFlushMethodID
        {
            get
            {
                if (_mainBreakFlushMethodID == 0)
                    _mainBreakFlushMethodID = int.Parse((gvMainBreak.IFooterRow.FindIControl<IDropDownList>(ControlIDs.FLUSH_METHOD)).SelectedValue);
                return _mainBreakFlushMethodID;
            }
        }

        public decimal? ChlorineResidual
        {
            get
            {
                if (_chlorineResidual == 0 || _chlorineResidual == null)
                {
                    if (!string.IsNullOrEmpty((gvMainBreak.IFooterRow.FindIControl<ITextBox>(ControlIDs.CHLORINE_RESIDUAL)).Text))
                    {
                        _chlorineResidual = decimal.Parse((gvMainBreak.IFooterRow.FindIControl<ITextBox>(ControlIDs.CHLORINE_RESIDUAL)).Text);
                    }
                }
                return _chlorineResidual;
            }
        }

        public bool BoilAlertIssued
        {
            get
            {
                _boilAlertIssued = gvMainBreak.IFooterRow.FindIControl<ICheckBox>(ControlIDs.BOIL_ALERT_ISSUED).Checked;
                return _boilAlertIssued;
            }
        }

        public int ServiceSizeID
        {
            get
            {
                if (_serviceSizeID == 0)
                    _serviceSizeID = int.Parse((gvMainBreak.IFooterRow.FindIControl<IDropDownList>(ControlIDs.SERVICE_SIZE)).SelectedValue);
                return _serviceSizeID;
            }
        }

        public int? FootageReplaced
        {
            get
            {
                
                if (!_footageReplaced.HasValue)
                    if (!String.IsNullOrWhiteSpace(gvMainBreak.IFooterRow.FindIControl<ITextBox>(ControlIDs.FOOTAGE_REPLACED).Text))
                    _footageReplaced = int.Parse((gvMainBreak.IFooterRow.FindIControl<ITextBox>(ControlIDs.FOOTAGE_REPLACED)).Text);
                return _footageReplaced;
            }
        }

        public int? ReplacedWithId
        {
            get
            {
                if (_replacedWithId == 0 || _replacedWithId == null && (gvMainBreak.IFooterRow.FindIControl<IDropDownList>(ControlIDs.REPLACED_WITH)).SelectedValue != null)
                    _replacedWithId = int.Parse((gvMainBreak.IFooterRow.FindIControl<IDropDownList>(ControlIDs.REPLACED_WITH)).SelectedValue);
                return _replacedWithId;
            }
        }

        public int WorkDescriptionID
        {
            get { return _workDescriptionID; }
            set { _workDescriptionID = value; }
        }

        #endregion

        #region Private Methods

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

        private void ToggleEditAndInsertControls(bool visible)
        {
            if (gvMainBreak.IFooterRow != null)
            {
                gvMainBreak.IFooterRow.Visible = visible;
                ToggleReplaceFields(gvMainBreak.IFooterRow);
            }

            foreach (var row in gvMainBreak.IRows)
            {
                ToggleEditControlsInRow(row, visible);
            }
        }

        private void ToggleReplaceFields(IGridViewRow row)
        {
            var rfv1 = row.FindControl<RequiredFieldValidator>(ControlIDs.FOOTAGE_REPLACED_REQUIRED_VALIDATOR);
            var rfv2 = row.FindControl<RequiredFieldValidator>(ControlIDs.REPLACED_WITH_VALIDATOR);
            var replacedWith = row.FindControl<MvpDropDownList>(ControlIDs.REPLACED_WITH);
            var footageReplaced = row.FindControl<MvpTextBox>(ControlIDs.FOOTAGE_REPLACED);

            if (lblWorkDescriptionID.Text != WorkDescription.WATER_MAIN_BREAK_REPLACE_ID.ToString())
            {
                return;
            }

            if (rfv1 != null)
                rfv1.Enabled = true;
            if (rfv2 != null)
                rfv2.Enabled = true;
            if (replacedWith != null)
                replacedWith.Enabled = true;
            if (footageReplaced != null)
                footageReplaced.Enabled = true;
        }

        protected override void SetDataSource(int workOrderID)
        {
            odsMainBreak.SelectParameters["WorkOrderID"].DefaultValue = workOrderID.ToString();
        }

        #endregion

        #region Event Handlers

        protected override void Page_Prerender(object sender, EventArgs e)
        {
            base.Page_Prerender(sender, e);

            //For some unknown reason hiddenValues don't work right with updatepanels, using a label instead
            if (lblWorkDescriptionID.Text == String.Empty)
                lblWorkDescriptionID.Text = WorkDescriptionID.ToString();

            ToggleEditAndInsertControls(CurrentMvpMode != DetailViewMode.ReadOnly);
        }
        
        protected void lbMainBreakInsert_Click(object sender, EventArgs e)
        {
            if (IPage.IsValid)
            {
                odsMainBreak.Insert();
            }
        }

        protected void lbCancel_Click(object sender, EventArgs e)
        {
        }

        protected void odsMainBreak_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            e.InputParameters[MainBreakParameterNames.WORK_ORDER_ID] = WorkOrderID;
            e.InputParameters[MainBreakParameterNames.BOIL_ALERT_ISSUED] = BoilAlertIssued;
            e.InputParameters[MainBreakParameterNames.CHLORINE_RESIDUAL] = ChlorineResidual;
            e.InputParameters[MainBreakParameterNames.CUSTOMERS_AFFECTED] = CustomersAffected;
            e.InputParameters[MainBreakParameterNames.DEPTH] = Depth;
            e.InputParameters[MainBreakParameterNames.MAIN_BREAK_DISINFECTION_METHOD_ID] = MainBreakDisinfectionMethodID;
            e.InputParameters[MainBreakParameterNames.MAIN_CONDITION_ID] = MainConditionID;
            e.InputParameters[MainBreakParameterNames.MAIN_BREAK_FLUSH_METHOD_ID] = MainBreakFlushMethodID;
            e.InputParameters[MainBreakParameterNames.MAIN_BREAK_MATERIAL_ID] = MainBreakMaterialID;
            e.InputParameters[MainBreakParameterNames.MAIN_BREAK_SOIL_CONDITION_ID] = MainBreakSoilConditionID;
            e.InputParameters[MainBreakParameterNames.MAIN_FAILURE_TYPE_ID] = MainFailureTypeID;
            e.InputParameters[MainBreakParameterNames.SHUTDOWN_TIME] = ShutdownTime;
            e.InputParameters[MainBreakParameterNames.SERVICE_SIZE_ID] = ServiceSizeID;
            e.InputParameters[MainBreakParameterNames.FOOTAGE_REPLACED] = FootageReplaced;
            e.InputParameters[MainBreakParameterNames.REPLACED_WITH_ID] = ReplacedWithId;
        }
        
        #endregion

        protected void gvMainBreak_OnRowCreated(object sender, GridViewRowEventArgs e)
        {
            var rfv1 = e.Row.FindControl<RequiredFieldValidator>(ControlIDs.FOOTAGE_REPLACED_REQUIRED_VALIDATOR);
            var rfv2 = e.Row.FindControl<RequiredFieldValidator>(ControlIDs.REPLACED_WITH_VALIDATOR);
            if (lblWorkDescriptionID.Text == WorkDescription.WATER_MAIN_BREAK_REPLACE_ID.ToString())
            {
                if (rfv1 != null)
                    rfv1.Enabled = true;
                if (rfv2 != null)
                    rfv2.Enabled = true;
            }
        }
    }

    public interface IWorkOrderMainBreakForm : IWorkOrderDetailControl
    {
    }
}