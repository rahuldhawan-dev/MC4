using System;
using System.Linq.Expressions;
using LINQTo271.Views.Abstract;
using MMSINC.Common;
using MMSINC.Interface;
using WorkOrders.Model;

namespace LINQTo271.Views.WorkOrders.PermitProcessing
{
    public partial class PermitProcessingResourceViewRPC : WorkOrderResourceRPCView
    {
        #region Control Declarations

        protected IDetailView<WorkOrder> ppdvWorkOrder;

        #endregion
        
        #region Properties

        public override IListView<WorkOrder> ListView
        {
            get { return null; }
        }

        public override IDetailView<WorkOrder> DetailView
        {
            get { return ppdvWorkOrder; }
        }

        public override ISearchView<WorkOrder> SearchView
        {
            get { return null; }
        }
        
        public override WorkOrderPhase Phase
        {
            get { return WorkOrderPhase.Planning; }
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