using System;
using System.Linq;
using System.Linq.Expressions;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.ClassExtensions.WorkOrderAssetViewModelExtensions
{
    public static class WorkOrderAssetViewModelExtensions
    {
        #region Exposed Methods

        // TODO: This is in the wrong project. This should be in MapCallMVC.

        public static void MaybeCancelWorkOrders<TThis, TEntity>(this TThis that, TEntity entity,
            IRepository<WorkOrderCancellationReason> cancellationReasonRepository,
            Expression<Func<TThis, DateTime?>> memberFn)
            where TThis : ViewModel<TEntity>
            where TEntity : class, IRetirableWorkOrderAsset
        {
            // There is zero reason to have an expression/invocation here. "that" is the view model, 
            // so the thing calling it already has access to the property that's being invoked. Just
            // pass the value in directly. 
            var value = memberFn.Compile().Invoke(that);
            if (value.HasValue && !entity.DateRetired.HasValue)
            {
                foreach (var wo in entity.WorkOrders)
                {
                    if (!wo.DateCompleted.HasValue && !wo.MaterialsUsed.Any())
                    {
                        wo.AssignedContractor = null;
                        wo.AssignedToContractorOn = null;
                        wo.CancelledAt = value;
                        wo.WorkOrderCancellationReason = cancellationReasonRepository
                                                        .Where(r => r.Description == "Asset Retired").Single();
                        wo.SAPErrorCode = "RETRY::ORDER CANCELLED";
                    }
                }
            }
        }

        #endregion
    }
}
