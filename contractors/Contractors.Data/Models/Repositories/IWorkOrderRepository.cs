using System.Collections.Generic;
using Contractors.Data.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using NHibernate;
using NHibernate.Criterion;

namespace Contractors.Data.Models.Repositories
{
    public interface IWorkOrderRepository : IWorkOrderRepositoryBase
    {
        #region Abstract Properties

        //ICriteria Criteria { get; }
        ICriteria GeneralOrders { get; }
        ICriteria PlanningOrders { get; }
        ICriteria SchedulingOrders { get; }
        ICriteria FinalizationOrders { get; }

        #endregion

        #region Abstract Methods

        ICriterion GetPlanningCriteria();
        ICriterion GetFinalizationCriteria();
        ICriterion GetSchedulingCriteria();
        ICriterion GetGeneralCriteria();

        IEnumerable<WorkOrder> GetByTownIdForServices(int townId);

        #endregion

        #region Methods

        WorkOrder FindSchedulingOrder(int id);

        IEnumerable<WorkOrder> SearchGeneralOrders(IWorkOrderSearch search);
        IEnumerable<WorkOrder> SearchPlanningOrders(IWorkOrderSearch search);
        IEnumerable<WorkOrder> SearchSchedulingOrders(IWorkOrderSearch search);
        IEnumerable<WorkOrder> SearchFinalizationOrders(IWorkOrderSearch search);

        #endregion
    }
}