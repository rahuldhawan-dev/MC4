using System;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using MMSINC.Controls;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using Microsoft.Practices.Web.UI.WebControls;
using StructureMap;
using WorkOrders;
using WorkOrders.Model;

namespace LINQTo271.Controls.WorkOrders
{
    public partial class WorkOrderAccountForm : WorkOrderDetailControlBase, IWorkOrderAccountForm
    {
        #region Constants
        
        public struct WorkOrderParameterNames
        {
            public const string APPROVED = "Approved",
                                APPROVED_ON = "ApprovedOn",
                                APPROVED_BY_ID = "ApprovedByID",
                                COMPLETED_BY_ID = "CompletedByID",
                                DATE_COMPLETED = "DateCompleted",
                                NOTES = "Notes",
                                BUSINESS_UNIT = "BusinessUnit",
                                REQUIRES_INVOICE = "RequiresInvoice",
                                STREET_OPENING_PERMIT_REQUIRED = "StreetOpeningPermitRequired",
                                TRAFFIC_CONTROL_REQUIRED = "TrafficControlRequired",
                                DIGITAL_ASBUILT_REQUIRED = "DigitalAsBuiltRequired";
        }

        public struct ControlIDs
        {
            public const string Notes = "txtRejectionNotes";
            public const string BusinessUnit = "hidBusinessUnit";
            public const string RequiresInvoice = "ddlRequiresInvoice";
        }

        #endregion

        #region Control Declarations

        protected IDetailControl fvWorkOrder;
        protected IObjectContainerDataSource odsWorkOrder;
        protected ITextBox txtRejectionNotes;

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

        public string Notes
        {
            get
            {
                return
                    fvWorkOrder.FindIControl<ITextBox>(
                        ControlIDs.Notes).Text;
            }
        }

        public string BusinessUnit
        {
            get
            {
                return fvWorkOrder.FindIControl<IHiddenField>(
                    ControlIDs.BusinessUnit).Value;
            }
        }
        
        public bool RequiresInvoice
        {
            get
            {
                return
                    (bool)fvWorkOrder.FindIControl<IDropDownList>(
                        ControlIDs.RequiresInvoice).GetBooleanValue();

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
            if (e.CommandArgument.ToString() == "Reject")
            {
                e.NewValues.Add(WorkOrderParameterNames.COMPLETED_BY_ID, null);
                e.NewValues.Add(WorkOrderParameterNames.DATE_COMPLETED, null);

                //Build notes
                var notes = string.Format("{0} {1} {2}: {3}", Environment.NewLine, DateTime.Now, SecurityService.CurrentUser.Name, Notes);
                e.NewValues.Add(WorkOrderParameterNames.NOTES, notes);
            }
            else //Approve
            {
                var oldWorkOrder = Repository.Get(WorkOrderID);
                
                //set the old completedbyid and datecompleted so they arent null
                e.NewValues.Add(WorkOrderParameterNames.COMPLETED_BY_ID, oldWorkOrder.CompletedByID);
                e.NewValues.Add(WorkOrderParameterNames.DATE_COMPLETED, oldWorkOrder.DateCompleted);

                //Set the approval date to now
                e.NewValues.Add(WorkOrderParameterNames.APPROVED_ON,
                    DateTime.Now);

                //Set the currently logged in user to ApprovedByID
                e.NewValues.Add(WorkOrderParameterNames.APPROVED_BY_ID,
                    SecurityService.GetEmployeeID());

                //Set the BusinessUnit
                e.NewValues[WorkOrderParameterNames.BUSINESS_UNIT] =
                    BusinessUnit;
               // e.NewValues.Add(WorkOrderParameterNames.BUSINESS_UNIT, BusinessUnit);

               // WE HAVE OTHER VALUES THAT ARE DISAPPEARING WHEN THIS UPDATE OCCURS.
               // WE HAVEN'T DETERMINED WHY AND IT WOULD BE SIMPLER TO MOVE THIS TO MVC
               // THIS EXISTS TO ENSURE THAT THEY PERSIST AS THEY WERE.
               e.NewValues[WorkOrderParameterNames.STREET_OPENING_PERMIT_REQUIRED] = oldWorkOrder.StreetOpeningPermitRequired;
               e.NewValues[WorkOrderParameterNames.TRAFFIC_CONTROL_REQUIRED] = oldWorkOrder.TrafficControlRequired;
               e.NewValues[WorkOrderParameterNames.DIGITAL_ASBUILT_REQUIRED] = oldWorkOrder.DigitalAsBuiltRequired;

                if (oldWorkOrder.OperatingCenter != null && oldWorkOrder.OperatingCenter.HasWorkOrderInvoicing)
                {
                    e.NewValues[WorkOrderParameterNames.REQUIRES_INVOICE] = RequiresInvoice;
                }
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

    public interface IWorkOrderAccountForm : IWorkOrderDetailControl
    {
    }
}
