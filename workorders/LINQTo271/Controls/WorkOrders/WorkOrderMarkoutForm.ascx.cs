using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Controls;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using StructureMap;
using WorkOrders;
using MarkoutRequirementRepository = WorkOrders.Model.MarkoutRequirementRepository;
using WorkOrder = WorkOrders.Model.WorkOrder;

namespace LINQTo271.Controls.WorkOrders
{
    public partial class WorkOrderMarkoutForm : WorkOrderDetailControlBase,
        IWorkOrderMarkoutForm
    {
        #region Constants

        public struct MarkoutsParameterNames
        {
            public const string WORK_ORDER_ID = "workOrderID",
                MARKOUT_NUMBER = "markoutNumber",
                MARKOUT_TYPE_ID = "markoutTypeID",
                DATE_OF_REQUEST = "dateOfRequest",
                READY_DATE = "readyDate",
                EXPIRATION_DATE = "expirationDate",
                NOTE = "note",
                CREATOR_ID = "creatorID";
        }

        public struct ControlIDs
        {
            public const string MARKOUTS_GRIDVIEW = "gvMarkouts",
                MARKOUT_ERROR_LABEL = "lblMarkoutError",
                MARKOUT_TYPE_NEEDED_LABEL = "lblTypeNeeded",
                MARKOUTS_DATASOURCE = "odsMarkouts",
                MARKOUT_NUMBER_TEXTBOX = "txtMarkoutNumber",
                MARKOUT_TYPE_DROPDOWNLIST = "ddlMarkoutType",
                DATE_OF_REQUEST_TEXTBOX = "ccDateOfRequest",
                DATE_EXPIRATION = "ccMarkoutExpirationDate",
                DATE_READY = "ccMarkoutReadyDate",
                UPDATE_PANEL = "upMarkouts",
                ROW_EDIT_LINKBUTTON = "lbEdit",
                ROW_DELETE_LINKBUTTON = "lbDelete",
                NOTE_TEXTBOX = "txtNote";
        }

        public const string MARKOUT_TYPE_LABEL_FORMAT_STRING =
            "Type Needed: {0} <br/> {1}";

        #endregion

        #region Private Members

        protected ITextBox _txtMarkoutNumber,
            _ccDateOfRequest,
            _ccMarkoutReadyDate,
            _ccMarkoutExpirationDate,
            _txtNote;

        protected IDropDownList _ddlMarkoutType;

        protected IUpdatePanel upMarkouts;
        protected IObjectDataSource odsMarkouts;
        protected IGridView gvMarkouts;

        protected ILabel _markoutError, _markoutTypeNeeded;

        protected IRepository<WorkOrder> _workOrderRepository;

        protected bool? _markoutsEditable;

        #endregion

        #region Properties

        public int MarkoutRequirementID { get; set; }

        public ILabel MarkoutError
        {
            get
            {
                if (_markoutError == null)
                {
                    _markoutError =
                        upMarkouts.FindIControl<ILabel>(ControlIDs
                            .MARKOUT_ERROR_LABEL);
                }
                return _markoutError;
            }
        }

        public ILabel MarkoutTypeNeeded
        {
            get
            {
                if (_markoutTypeNeeded == null)
                {
                    _markoutTypeNeeded =
                        upMarkouts.FindIControl<ILabel>(
                            ControlIDs.MARKOUT_TYPE_NEEDED_LABEL);
                }
                return _markoutTypeNeeded;
            }
        }

        public string MarkoutNumber
        {
            get
            {
                if (_txtMarkoutNumber == null)
                {
                    _txtMarkoutNumber =
                        gvMarkouts.IFooterRow.FindIControl<ITextBox>(ControlIDs
                            .MARKOUT_NUMBER_TEXTBOX);
                }
                return _txtMarkoutNumber.Text;
            }
        }

        public string MarkoutTypeID
        {
            get
            {
                if (_ddlMarkoutType == null)
                {
                    _ddlMarkoutType =
                        gvMarkouts.IFooterRow.FindIControl<IDropDownList>(
                            ControlIDs.MARKOUT_TYPE_DROPDOWNLIST);
                }
                return _ddlMarkoutType.SelectedValue;
            }
        }

        public string DateOfRequest
        {
            get
            {
                if (_ccDateOfRequest == null)
                {
                    _ccDateOfRequest =
                        gvMarkouts.IFooterRow.FindIControl<ITextBox>(
                            ControlIDs.DATE_OF_REQUEST_TEXTBOX);
                }
                return _ccDateOfRequest.Text;
            }
        }

        public string ReadyDate
        {
            get
            {
                if (_ccMarkoutReadyDate == null)
                {
                    _ccMarkoutReadyDate =
                        gvMarkouts.IFooterRow.FindIControl<ITextBox>(
                            ControlIDs.DATE_READY);
                }
                return _ccMarkoutReadyDate.Text;
            }
        }

        public string ExpirationDate
        {
            get
            {
                if (_ccMarkoutExpirationDate == null)
                {
                    _ccMarkoutExpirationDate =
                        gvMarkouts.IFooterRow.FindIControl<ITextBox>(
                            ControlIDs.DATE_EXPIRATION);
                }
                return _ccMarkoutExpirationDate.Text;
            }
        }

        public string Note
        {
            get
            {
                if (_txtNote == null)
                {
                    _txtNote = gvMarkouts.IFooterRow.FindIControl<ITextBox>(
                        ControlIDs.NOTE_TEXTBOX);
                }
                return _txtNote.Text;
            }
        }

        protected IRepository<WorkOrder> WorkOrderRepository
        {
            get
            {
                if (_workOrderRepository == null)
                    _workOrderRepository =
                        DependencyResolver.Current.GetService<IRepository<WorkOrder>>();
                return _workOrderRepository;
            }
        }

        public bool MarkoutsEditable
        {
            get
            {
                if (!_markoutsEditable.HasValue)
                {
                    var order = WorkOrderRepository.Get(WorkOrderID);
                    _markoutsEditable = order.OperatingCenter?.MarkoutsEditable ?? false;
                }
                return _markoutsEditable.Value;
            }
        }

        #endregion

        #region Private Methods

        protected override void SetDataSource(int workOrderID)
        {
            odsMarkouts.SelectParameters["WorkOrderID"].DefaultValue = workOrderID.ToString();
            var order = WorkOrderRepository.Get(workOrderID);
            if (order.MarkoutTypeNeeded != null)
            {
                MarkoutTypeNeeded.Text = String.Format(
                    MARKOUT_TYPE_LABEL_FORMAT_STRING,
                    order.MarkoutTypeNeeded.Description,
                    order.RequiredMarkoutNote);
            }
            //MarkoutsEditable = order.OperatingCenter?.MarkoutsEditable ?? false;
        }

        private static void ToggleEditControlsInRow(IGridViewRow row, bool visible)
        {
            var lbEdit = row.FindIControl<ILinkButton>(ControlIDs.ROW_EDIT_LINKBUTTON);
            var lbDelete = row.FindIControl<ILinkButton>(ControlIDs.ROW_DELETE_LINKBUTTON);

            if (lbEdit != null && lbDelete != null)
            {
                lbEdit.Visible = lbDelete.Visible = visible;
            }
        }

        private void ToggleEditAndInsertControls(bool visible)
        {
            if (gvMarkouts.IFooterRow != null)
            {
                gvMarkouts.IFooterRow.Visible = visible;
                ToggleFooterControls();
                //var order = WorkOrderRepository.Get(WorkOrderID);
                //MarkoutsEditable = order.OperatingCenter.MarkoutsEditable;
                //gvMarkouts.IFooterRow.FindControl(ControlIDs.DATE_READY).Visible = MarkoutsEditable;
                //gvMarkouts.IFooterRow.FindControl(ControlIDs.DATE_EXPIRATION).Visible = MarkoutsEditable;
            }

            foreach (var row in gvMarkouts.IRows)
            {
                ToggleEditControlsInRow(row, visible);
            }
        }

        private void LogThisChange(string auditEntryType, string dateOfRequest, string readyDate, string expirationDate, int? markoutId)
        {
            // markouts editable - this might not be set yet, lets set it
            var order = WorkOrderRepository.Get(WorkOrderID);
            //MarkoutsEditable = order.OperatingCenter.MarkoutsEditable;

            // if we're inserting, lets go get the last markout inserted because dealing with forms page lifecycle sucks
            if (auditEntryType == "INSERT")
            {
                var markout = order.Markouts.OrderByDescending(x => x.MarkoutID).First();
                dateOfRequest = markout.DateOfRequest.ToString();
                readyDate = markout.ReadyDate.ToString();
                expirationDate = markout.ExpirationDate.ToString();
                markoutId = markout.MarkoutID;
            }

            var logger = DependencyResolver.Current.GetService<IAuditLogEntryRepository>();
            logger.Save(new AuditLogEntry {
                AuditEntryType = auditEntryType,
                EntityId = markoutId.Value,
                EntityName = "Markout",
                FieldName = "DateRequested",
                NewValue = dateOfRequest,
                Timestamp = DateTime.Now,
                User = new User {
                    Id = SecurityService.GetEmployeeID()
                }
            });
            logger.Save(new AuditLogEntry {
                AuditEntryType = auditEntryType,
                EntityId = markoutId.Value,
                EntityName = "Markout",
                FieldName = (MarkoutsEditable) ? "ExpirationDate" : "ExpirationDateGenerated",
                NewValue = expirationDate,
                Timestamp = DateTime.Now,
                User = new User {
                    Id = SecurityService.GetEmployeeID()
                }
            });
            logger.Save(new AuditLogEntry {
                AuditEntryType = auditEntryType,
                EntityId = markoutId.Value,
                EntityName = "Markout",
                FieldName = (MarkoutsEditable) ? "ReadyDate" : "ReadyDateGenerated",
                NewValue = readyDate,
                Timestamp = DateTime.Now,
                User = new User {
                    Id = SecurityService.GetEmployeeID()
                }
            });
        }

        #endregion

        #region Event Handlers

        protected void lbInsert_Click(object sender, EventArgs e)
        {
            odsMarkouts.Insert();
        }

        protected void lbCancel_Click(object sender, EventArgs e)
        {
            //noop
        }

        protected void odsMarkouts_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            e.InputParameters[MarkoutsParameterNames.WORK_ORDER_ID] = WorkOrderID;
            e.InputParameters[MarkoutsParameterNames.MARKOUT_NUMBER] = MarkoutNumber;
            e.InputParameters[MarkoutsParameterNames.MARKOUT_TYPE_ID] = MarkoutTypeID;
            e.InputParameters[MarkoutsParameterNames.DATE_OF_REQUEST] = DateOfRequest;
            e.InputParameters[MarkoutsParameterNames.EXPIRATION_DATE] = ExpirationDate;
            e.InputParameters[MarkoutsParameterNames.READY_DATE] = ReadyDate;
            e.InputParameters[MarkoutsParameterNames.NOTE] = Note;
            e.InputParameters[MarkoutsParameterNames.CREATOR_ID] = SecurityService.GetEmployeeID();
        }

        protected override void Page_Prerender(object sender, EventArgs e)
        {
            base.Page_Prerender(sender, e);

            ToggleEditAndInsertControls(CurrentMvpMode !=
                                        DetailViewMode.ReadOnly &&
                                        MarkoutRequirementID !=
                                        MarkoutRequirementRepository.Indices
                                            .NONE);
        }

        protected void odsMarkouts_OnUpdating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            LogThisChange("UPDATE",
                e.InputParameters["DateOfRequest"].ToString(),
                e.InputParameters["ReadyDate"].ToString(),
                e.InputParameters["ExpirationDate"].ToString(),
                int.Parse(e.InputParameters["MarkoutID"].ToString()));
        }

        protected void odsMarkouts_OnDeleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var logger = DependencyResolver.Current.GetService<IAuditLogEntryRepository>();
            logger.Save(new AuditLogEntry {
                AuditEntryType = "DESTROY",
                EntityId = int.Parse(e.InputParameters[0].ToString()),
                EntityName = "Markout",
                FieldName = "",
                NewValue = e.InputParameters["MarkoutID"].ToString(),
                Timestamp = DateTime.Now,
                User = new User {
                    Id = SecurityService.GetEmployeeID()
                }
            });
        }

        protected void odsMarkouts_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
        {
            LogThisChange("INSERT", string.Empty, string.Empty, string.Empty, null);
        }

        #endregion
        
        protected void gvMarkouts_OnPreRender(object sender, EventArgs e)
        {
            ToggleEditAndInsertControls(CurrentMvpMode !=
                                        DetailViewMode.ReadOnly &&
                                        MarkoutRequirementID !=
                                        MarkoutRequirementRepository.Indices
                                            .NONE);
        }
        
        protected void gvMarkouts_OnRowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ToggleFooterControls();
                //var order = WorkOrderRepository.Get(WorkOrderID);
                //MarkoutsEditable = order.OperatingCenter.MarkoutsEditable;
                //e.Row.FindControl(ControlIDs.DATE_READY).Visible = MarkoutsEditable;
                //e.Row.FindControl(ControlIDs.DATE_EXPIRATION).Visible = MarkoutsEditable;
                //upMarkouts.Update();
            }
        }

        private void ToggleFooterControls()
        {
            if (gvMarkouts?.IFooterRow == null)
                return;
            var dateReady = gvMarkouts.IFooterRow.FindControl("ccMarkoutReadyDate");
            var dateExpiring = gvMarkouts.IFooterRow.FindControl("ccMarkoutExpirationDate");
            if (dateReady != null)
                dateReady.Visible = MarkoutsEditable;
            if (dateExpiring != null)
                dateExpiring.Visible = MarkoutsEditable;
        }

        protected void upMarkouts_OnDataBinding(object sender, EventArgs e)
        {
            ToggleFooterControls();
        }

        protected void gvMarkouts_OnDataBound(object sender, EventArgs e)
        {
            ToggleFooterControls();
        }

        protected void upMarkouts_OnLoad(object sender, EventArgs e)
        {
            
        }

        protected void upMarkouts_OnPreRender(object sender, EventArgs e)
        {
            
        }

        protected void upMarkouts_OnInit(object sender, EventArgs e)
        {
            
        }

        protected void gvMarkouts_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            ToggleFooterControls();
        }

        protected void gvMarkouts_OnRowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            ToggleFooterControls();
        }

        protected void ccMarkoutExpirationDate_OnPreRender(object sender, EventArgs e)
        {
            ((MvpTextBox)sender).Visible = MarkoutsEditable;
        }

        protected void ccMarkoutReadyDate_OnPreRender(object sender, EventArgs e)
        {
            ((MvpTextBox)sender).Visible = MarkoutsEditable;
        }
    }


    public interface IWorkOrderMarkoutForm : IWorkOrderDetailControl
    {
    }
}