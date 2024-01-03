using System;
using System.Linq.Expressions;
using LINQTo271.Views.Abstract;
using MMSINC.Common;
using MMSINC.Interface;
using WorkOrders.Model;
using WorkOrders.Views.WorkOrders;

namespace LINQTo271.Views.WorkOrders.Input
{
    /// <summary>
    /// RPC view useful for allowing display/edit of initial Work Order information.  This is
    /// linked to from the list view, so that users can view historical orders while creating
    /// new ones (as a popup).
    /// </summary>
    public partial class WorkOrderInputResourceRPCView : WorkOrdersResourceRPCView<WorkOrder>, IWorkOrderResourceRPCView
    {
        #region Private Members

        protected IDetailView<WorkOrder> wodvWorkOrder;

        #endregion

        #region Properties

        public override IDetailView<WorkOrder> DetailView
        {
            get { return wodvWorkOrder; }
        }

        public override IListView<WorkOrder> ListView
        {
            get { return null; }
        }

        public override ISearchView<WorkOrder> SearchView
        {
            get { return null; }
        }

        public WorkOrderPhase Phase
        {
            get { return WorkOrderPhase.Input; }
        }

        #endregion

        #region Exposed Methods

        public override Expression<Func<WorkOrder, bool>> GenerateExpression()
        {
            return new ExpressionBuilder<WorkOrder>(
                wo => wo.WorkOrderID == Int32.Parse(Argument));
        }

        #endregion
    }
}
