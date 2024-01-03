using System;
using System.Web.Mvc;
using System.Web.UI.WebControls;
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
    public partial class WorkOrderTrafficControlForm : WorkOrderDetailControlBase, IWorkOrderTrafficControlForm
    {
        #region Constants

        public struct WorkOrderParameterNames
        {
            public const string NOTES = "Notes";
        }

        public struct ControlIDs
        {
            public const string CURRENT_NOTES_LABEL = "lblCurrentNotes",
                                APPEND_NOTES_BOX = "txtAppendNotes";
        }

        #endregion

        #region Control Declarations

        protected IDetailControl fvWorkOrder;
        protected IObjectContainerDataSource odsWorkOrder;

        protected ITextBox _txtAppendNotes;
        protected ILabel _lblCurrentNotes;

        #endregion

        #region Private Members

        protected IRepository<WorkOrder> _repository;

        #endregion

        #region Properties

        public object DataSource
        {
            set { odsWorkOrder.DataSource = value; }
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

        #endregion

        #region Private Methods

        protected override void SetDataSource(int workOrderID)
        {
            DataSource = Repository.Get(workOrderID);
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

        protected void fvWorkOrder_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            if (!string.IsNullOrEmpty(AppendNotes))
            {
                e.NewValues[WorkOrderParameterNames.NOTES] =
                    (string.IsNullOrWhiteSpace(CurrentNotes)
                        ? string.Empty
                        : CurrentNotes + Environment.NewLine) +
                    $"{SecurityService.CurrentUser.Name} {DateTime.Now.ToString(CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE_FOR_WEBFORMS)}: {AppendNotes}";
            }
        }

        protected void ods_Updated(object sender, ObjectContainerDataSourceStatusEventArgs e)
        {
            if (Updating != null)
                Updating(sender, e);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            fvWorkOrder.UpdateItem(true);
        }

        #endregion
    }

    public interface IWorkOrderTrafficControlForm : IWorkOrderDetailControl
    {
        
    }
}