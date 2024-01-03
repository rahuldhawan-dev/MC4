using System;
using System.Linq.Expressions;
using LINQTo271.Views.Abstract;
using MMSINC.Common;
using MMSINC.Interface;
using WorkOrders.Model;

namespace LINQTo271.Views.WorkOrders.Finalization
{
    public partial class WorkOrderFinalizationResourceRPCView : WorkOrderResourceRPCView
    {
        #region Control Declarations

        protected IDetailView<WorkOrder> wodvWorkOrder;

        #endregion

        #region Properties

        public override WorkOrderPhase Phase
        {
            get { return WorkOrderPhase.Finalization; }
        }

        public override IListView<WorkOrder> ListView
        {
            get { return null; }
        }

        public override IDetailView<WorkOrder> DetailView
        {
            get { return wodvWorkOrder; }
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
    }
}
