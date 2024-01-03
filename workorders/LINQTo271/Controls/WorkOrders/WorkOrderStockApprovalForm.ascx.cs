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
    public partial class WorkOrderStockApprovalForm : WorkOrderDetailControlBase, IWorkOrderStockApprovalForm
    {
        #region Constants

        public struct WorkOrderParameterNames
        {
            public const string MATERIALS_APPROVED_BY_ID = "MaterialsApprovedByID",
                                MATERIALS_APPROVED_ON = "MaterialsApprovedOn",
                                STREET_OPENING_PERMIT_REQUIRED = "StreetOpeningPermitRequired",
                                TRAFFIC_CONTROL_REQUIRED = "TrafficControlRequired",
                                DIGITAL_ASBUILT_REQUIRED = "DigitalAsBuiltRequired";
        }

        #endregion

        #region Control Declarations

        protected IDetailControl fvWorkOrder;
        protected IObjectContainerDataSource odsWorkOrder;

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
            e.NewValues[WorkOrderParameterNames.MATERIALS_APPROVED_ON] =
                DateTime.Now;
            e.NewValues[WorkOrderParameterNames.MATERIALS_APPROVED_BY_ID] =
                SecurityService.GetEmployeeID();

            var oldWorkOrder = Repository.Get(WorkOrderID);
            // checkbox values mysteriously disappearing from the database, this is keeping them from overwriting.
            // following the same format as the account form code behind
            e.NewValues[WorkOrderParameterNames.STREET_OPENING_PERMIT_REQUIRED] = oldWorkOrder.StreetOpeningPermitRequired;
            e.NewValues[WorkOrderParameterNames.TRAFFIC_CONTROL_REQUIRED] = oldWorkOrder.TrafficControlRequired;
            e.NewValues[WorkOrderParameterNames.DIGITAL_ASBUILT_REQUIRED] = oldWorkOrder.DigitalAsBuiltRequired;
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

    public interface IWorkOrderStockApprovalForm : IWorkOrderDetailControl
    {
        
    }
}