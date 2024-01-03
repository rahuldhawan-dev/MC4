using System;
using System.Linq.Expressions;
using LINQTo271.Views.Abstract;
using MMSINC.Common;
using MMSINC.Interface;
using WorkOrders.Model;
using WorkOrders.Views.WorkOrders;

namespace LINQTo271.Views.WorkOrders.ReadOnly
{
    public partial class WorkOrderReadOnlyResourceRPCView : WorkOrdersResourceRPCView<WorkOrder>, IWorkOrderResourceRPCView
    {
        #region Control Declarations

        protected IDetailView<WorkOrder> woroWorkOrder;

        #endregion

        #region Properties

        public override IDetailView<WorkOrder> DetailView
        {
            get { return woroWorkOrder; }
        }

        public override IListView<WorkOrder> ListView
        {
            get { return null; }
        }

        public override ISearchView<WorkOrder> SearchView
        {
            get { return null; }
        }

        #endregion

        #region Exposed Methods

        public override Expression<Func<WorkOrder, bool>> GenerateExpression()
        {
            return new ExpressionBuilder<WorkOrder>(
                wo => wo.WorkOrderID == Int32.Parse(Argument));
        }

        #endregion

        #region Implementation of IWorkOrderView

        public WorkOrderPhase Phase
        {
            get { return WorkOrderPhase.Input; }
        }

        #endregion
    }
}
