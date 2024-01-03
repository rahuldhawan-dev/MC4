using System.Web.Mvc;
using Contractors.Data.Models.Repositories;
using MapCall.Common.Model.Entities;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using StructureMap;

namespace Contractors.Data.DesignPatterns.Mvc
{
    // TODO: This entire file is in the wrong project. It can just exist in the Contractors project.
    // TODO: Do all of these controller classes need to exist? Can they be replaced with just using the base controller classes?

    public abstract class ControllerBase<TRepository, TEntity> : MMSINC.Controllers.ControllerBaseWithAuthentication<TRepository, TEntity, ContractorUser>
        where TRepository : class, IRepository<TEntity>
        where TEntity : class
    {
        #region Constants

        public const string NO_SUCH_WORK_ORDER = "No such work order.";
        public const string NO_SUCH_WORK_ORDER_FORMAT = "WorkOrderID '{0}' does not exist.";

        #endregion

        #region Private Methods

        protected HttpNotFoundResult NoSuchWorkOrder()
        {
            return HttpNotFound(NO_SUCH_WORK_ORDER);
        }

        protected WorkOrder GetWorkOrder(int workOrderId, ICriteria baseCriteria = null)
        {
            return baseCriteria == null
                ? _container.GetInstance<IWorkOrderRepository>().Find(workOrderId)
                : baseCriteria.Add(Restrictions.IdEq(workOrderId)).UniqueResult
                    <WorkOrder>();
        }

        #endregion

        #region Constructors

        protected ControllerBase(ControllerBaseWithAuthenticationArguments<TRepository, TEntity, ContractorUser> args) : base(args) { }

        #endregion
    }

    public abstract class ControllerBaseWithValidation<TRepository, TEntity> : MMSINC.Controllers.ControllerBaseWithPersistence<TRepository, TEntity, ContractorUser>
        where TRepository : class, IRepository<TEntity>
        where TEntity : class, new()
    {
        // TODO: fix this duplication
        #region Constants

        public const string NO_SUCH_WORK_ORDER = "No such work order.";
        public const string NO_SUCH_WORK_ORDER_FORMAT = "WorkOrderID '{0}' does not exist.";

        #endregion

        #region Private Methods

        protected HttpNotFoundResult NoSuchWorkOrder()
        {
            return HttpNotFound(NO_SUCH_WORK_ORDER);
        }

        protected WorkOrder GetWorkOrder(int workOrderId, ICriteria baseCriteria = null)
        {
            return baseCriteria == null
                ? _container.GetInstance<IWorkOrderRepository>().Find(workOrderId)
                : baseCriteria.Add(Restrictions.IdEq(workOrderId)).UniqueResult
                    <WorkOrder>();
        }

        #endregion        

        #region Constructors

        protected ControllerBaseWithValidation(ControllerBaseWithPersistenceArguments<TRepository, TEntity, ContractorUser> args) : base(args) { }

        #endregion
    }

    public abstract class ControllerBase<TEntity> : ControllerBase<IRepository<TEntity>, TEntity>
        where TEntity : class
    {
        #region Constructors

        protected ControllerBase(ControllerBaseWithAuthenticationArguments<IRepository<TEntity>, TEntity, ContractorUser> args) : base(args) { }

        #endregion
    }

    public abstract class ControllerBaseWithValidation<TEntity> : ControllerBaseWithValidation<IRepository<TEntity>, TEntity>
        where TEntity : class,  new()
    {
        #region Constructors

        protected ControllerBaseWithValidation(ControllerBaseWithPersistenceArguments<IRepository<TEntity>, TEntity, ContractorUser> args) : base(args) { }

        #endregion
    }
}
