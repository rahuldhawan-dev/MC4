using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using LINQTo271.Views.Abstract;
using MapCall.Common.Model.Entities;
using MapCall.Common.Utility.Notifications;
using MMSINC.Controls;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using Microsoft.Practices.Web.UI.WebControls;
using MMSINC.Utilities;
using WorkOrders.Model;
using WorkOrders.Presenters.WorkOrders;
using MostRecentlyInstalledService = WorkOrders.Model.MostRecentlyInstalledService;
using WorkDescription = WorkOrders.Model.WorkDescription;
using WorkOrder = WorkOrders.Model.WorkOrder;

namespace LINQTo271.Controls.WorkOrders
{
    public partial class WorkOrderAdditionalFinalizationInfoForm : WorkOrderDetailControlBase, IWorkOrderAdditionalFinalizationInfoForm
    {
        #region Constants

        public struct WorkOrderParameterNames
        {
            public const string NOTES = "Notes";
        }

        public struct ControlIDs
        {
            public const string CURRENT_NOTES_LABEL = "lblCurrentNotes",
                                APPEND_NOTES_BOX = "txtAppendNotes",
                                WORK_DESCRIPTION_SELECT = "ddlFinalWorkDescription";
        }

        #endregion

        #region Control Declarations

        protected IDetailControl fvWorkOrder;
        protected IObjectContainerDataSource odsWorkOrder;
        protected IRepository<WorkOrder> _repository;

        protected IRepository<WorkOrderDescriptionChange>
            _descriptionChangeRepository;

        private IRepository<MostRecentlyInstalledService> _mostRecentlyInstalledServiceRepository;

        #endregion

        #region Properties

        public string CurrentNotes
        {
            get
            {
                return
                    fvWorkOrder.FindIControl<ILabel>(
                        ControlIDs.CURRENT_NOTES_LABEL).Text;
            }
        }

        public string AppendNotes
        {
            get
            {
                return fvWorkOrder.FindIControl<ITextBox>(ControlIDs.APPEND_NOTES_BOX).Text;
            }
        }

        public int FinalWorkDescriptionID =>
            fvWorkOrder
               .FindIControl<IDropDownList>(ControlIDs.WORK_DESCRIPTION_SELECT)
               .GetSelectedValue()
               .Value;

        public object DataSource
        {
            set { odsWorkOrder.DataSource = value; }
        }

        // TODO: This shouldn't need to be here
        protected IRepository<WorkOrder> Repository =>
            _repository ??
            (_repository = DependencyResolver.Current.GetService<IRepository<WorkOrder>>());

        protected IRepository<WorkOrderDescriptionChange> DescriptionChangeRepository =>
            _descriptionChangeRepository ?? (_descriptionChangeRepository =
                DependencyResolver.Current.GetService<IRepository<WorkOrderDescriptionChange>>());

        protected IRepository<MostRecentlyInstalledService> MostRecentlyInstalledServiceRepository =>
            _mostRecentlyInstalledServiceRepository ?? (_mostRecentlyInstalledServiceRepository =
                DependencyResolver.Current.GetService<IRepository<MostRecentlyInstalledService>>());

        #endregion

        #region Private Methods

        protected override void SetDataSource(int workOrderID)
        {
            DataSource = Repository.Get(workOrderID);
        }

        private void AppendWorkDescriptionChange(string fromWorkDescriptionID)
        {
            DescriptionChangeRepository.InsertNewEntity(
                new WorkOrderDescriptionChange {
                    WorkOrderID = WorkOrderID,
                    ToWorkDescriptionID = FinalWorkDescriptionID,
                    FromWorkDescriptionID = Int32.Parse(fromWorkDescriptionID),
                    ResponsibleEmployeeID = SecurityService.Employee.EmployeeID,
                    DateOfChange = DateTime.Now
                });
        }

        private void SendMainBreakNotification()
        {
            var notifier = DependencyResolver.Current.GetService<INotificationService>();
            var workOrder = Repository.Get(WorkOrderID);
            notifier.Notify(
                workOrder.OperatingCenterID.Value,
                RoleModules.FieldServicesWorkManagement,
                WorkOrderDetailPresenter.MAIN_BREAK_NOTIFICATION,
                workOrder,
                "Work Order Changed to Main Break Repair/Replace");
        }

        private void SetValueIfUnset(IOrderedDictionary dict, string key, object valueToSet)
        {
            if (string.IsNullOrWhiteSpace(dict[key]?.ToString()) && valueToSet != null)
            {
                dict[key] = valueToSet.ToString();
            }
        }

        private void MaybeMapRecentServiceSizesAndMaterials(FormViewUpdateEventArgs e)
        {
            var renewals = WorkDescription.SERVICE_LINE_RENEWALS; 
            // we only want to do this if the work description is being changed to a service line renewal,
            // and not if it already was
            if (renewals.Contains(Convert.ToInt32(e.OldValues["WorkDescriptionID"])) ||
                !renewals.Contains(Convert.ToInt32(e.NewValues["WorkDescriptionID"])))
            {
                return;
            }
            
            var workOrder = Repository.Get(WorkOrderID);
            var recentService =
                MostRecentlyInstalledServiceRepository
                   .GetFilteredSortedData(
                        s => s.Premise.OperatingCenterID == workOrder.OperatingCenterID &&
                             s.Premise.Installation == workOrder.Installation.ToString(),
                        null)
                   .SingleOrDefault();

            if (recentService == null)
            {
                return;
            }

            // no sense in setting if the value is "UNKNOWN"
            if (recentService.ServiceMaterial?.Description != "UNKNOWN")
            {
                SetValueIfUnset(
                    e.NewValues,
                    "CompanyServiceLineMaterialID",
                    recentService.ServiceMaterialID);
            }

            if (recentService.CustomerSideMaterial?.Description != "UNKNOWN")
            {
                SetValueIfUnset(
                    e.NewValues,
                    "CustomerServiceLineMaterialID",
                    recentService.CustomerSideMaterialID);
            }

            SetValueIfUnset(e.NewValues, "CompanyServiceLineSizeID", recentService.ServiceSizeID);
            SetValueIfUnset(e.NewValues, "CustomerServiceLineSizeID", recentService.CustomerSideSizeID);
        }

        #endregion

        #region Events

        public event EventHandler<ObjectContainerDataSourceStatusEventArgs>
            Updating;

        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            fvWorkOrder.ChangeMvpMode(CurrentMvpMode);
        }

        protected void lbUpdate_Click(object sender, EventArgs e)
        {
            fvWorkOrder.UpdateItem(true);
        }

        protected void fvWorkOrder_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            if (!string.IsNullOrEmpty(AppendNotes))
            {
                e.NewValues[WorkOrderParameterNames.NOTES] =
                    (string.IsNullOrWhiteSpace(CurrentNotes)
                        ? string.Empty
                        : Environment.NewLine)
                    + $"{SecurityService.CurrentUser.Name} "
                    + DateTime.Now.ToString(
                        CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE_FOR_WEBFORMS)
                    + $" {AppendNotes}";
            }

            if (e.OldValues["WorkDescriptionID"].ToString() !=
                FinalWorkDescriptionID.ToString())
            {
                AppendWorkDescriptionChange(
                    e.OldValues["WorkDescriptionID"].ToString());
                MaybeMapRecentServiceSizesAndMaterials(e);
            }

            if (e.OldValues[nameof(WorkOrder.InitialServiceLineFlushTime)] !=
                e.NewValues[nameof(WorkOrder.InitialServiceLineFlushTime)] &&
                e.NewValues[nameof(WorkOrder.InitialServiceLineFlushTime)] != null)
            {
                e.NewValues[nameof(WorkOrder.InitialFlushTimeEnteredById)] =
                    SecurityService.GetEmployeeID();
                e.NewValues[nameof(WorkOrder.InitialFlushTimeEnteredAt)] =
                    DependencyResolver.Current.GetService<IDateTimeProvider>().GetCurrentDate();
            }
        }

        protected void ods_Updated(object sender, ObjectContainerDataSourceStatusEventArgs e)
        {
            Updating?.Invoke(sender, e);
        }
        
        /// <summary>
        /// TODO: Fix this, test this, give this some love.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlFinalWorkDescription_OnDataBinding(object sender, EventArgs e)
        {
            var ddl = (IDropDownList)sender;
            ddl.DataBinding -= ddlFinalWorkDescription_OnDataBinding;
            try
            {
                ddl.DataBind();
            }
            catch (ArgumentOutOfRangeException)
            {
                ddl.Items.Clear();
                var wo = (WorkOrder)ddl.DataItem;
                ddl.Items.Add(new ListItem(
                    wo.WorkDescription.Description,
                    wo.WorkDescription.WorkDescriptionID.ToString()));
            }
        }

        protected void fvWorkOrder_OnDataBound(object sender, EventArgs e)
        {
            var wo = (WorkOrder)((IDetailControl)sender).DataItem;
            var expr = WorkOrderSearchView.NotRetiredRemovedOrCancelled.Compile();
            ((IDetailControl)sender).FindControl<HyperLink>("hlFinalization").Visible = expr(wo);
            if (IPage.IRequest.RelativeUrl.Contains("WorkOrderFinalizationResourceRPCPage"))
            {
                ((IDetailControl)sender).FindControl<HyperLink>("hlFinalization").Visible = false; 
            }
        }

        #endregion

        #region Exposed Methods

        public void UpdateDetailControl()
        {
            fvWorkOrder.UpdateItem(true);
        }

        #endregion
    }

    public interface IWorkOrderAdditionalFinalizationInfoForm : IControl
    {
        #region Methods

        void UpdateDetailControl();

        #endregion
    }
}