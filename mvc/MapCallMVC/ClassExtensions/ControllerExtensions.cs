using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using ControllerBase = MMSINC.Controllers.ControllerBase;

namespace MapCallMVC.ClassExtensions
{
    public static class ControllerExtensions
    {
         #region SendNotification
 
         public static void SendNotification(this ControllerBase that, int operatingCenterId, RoleModules module, string purpose, object data)
         {
             that.Container.GetInstance<INotificationService>().Notify(new NotifierArgs {
                 OperatingCenterId = operatingCenterId,
                 Module = module,
                 Purpose = purpose,
                 Data = data
             });
         }
 
        #endregion

        #region GetStateViewPath

        public static string GetStateViewPath(this Controller that, IThingWithState thing, string viewName)
        {
            string statePath;

            if (thing.State != null && that.ViewExists(statePath = Path.Combine(viewName, thing.State.Abbreviation)))
            {
                return statePath;
            }

            return viewName;
        }

        #endregion

        #region GetOperatingCenters

        public static IQueryable<OperatingCenter> OperatingCentersSortFn(IQueryable<OperatingCenter> os)
        {
            return os.OrderBy(o => o.State.Abbreviation).ThenBy(o => o.OperatingCenterCode);
        }

        public static Func<IRepository<OperatingCenter>, IQueryable<OperatingCenter>> GetAllOperatingCenters
            <TRepository, TEntity>(this ControllerBaseWithPersistence<TRepository, TEntity, User> that, Expression<Func<OperatingCenter, bool>> filterP = null)
            where TRepository : class, IRepository<TEntity>
            where TEntity : class
        {
            if (filterP == null)
            {
                return r => OperatingCentersSortFn(r.GetAll());
            }

            return r => OperatingCentersSortFn(r.Where(filterP));
        }

        public static Func<IRepository<OperatingCenter>, IQueryable<OperatingCenter>> GetUserOperatingCentersFn
            <TEntity>(this ControllerBaseWithPersistence<TEntity, User> that, RoleModules module,
                RoleActions action = RoleActions.Read)
            where TEntity : class
        {
            return GetUserOperatingCentersFn(
                (ControllerBaseWithPersistence<IRepository<TEntity>, TEntity, User>)that, module, action);
        }

        public static Func<IRepository<OperatingCenter>, IQueryable<OperatingCenter>> GetUserOperatingCentersFn
            <TRepository, TEntity>(this ControllerBaseWithPersistence<TRepository, TEntity, User> that,
                RoleModules module, RoleActions action = RoleActions.Read, Expression<Func<OperatingCenter, bool>> extraFilterP = null)
            where TRepository : class, IRepository<TEntity>
            where TEntity : class
        {
            extraFilterP = extraFilterP ?? (_ => true);
            var curUserIsAdmin = that.AuthenticationService.CurrentUserIsAdmin;
            var currentUser = that.AuthenticationService.CurrentUser;

            return (r) => {
                IQueryable<OperatingCenter> opCenters;
                if (curUserIsAdmin)
                {
                    opCenters = r.GetAll();
                }
                else
                {
                    var matches = currentUser.GetQueryableMatchingRoles(that.Container.GetInstance<IRepository<AggregateRole>>(),
                        module, action);
                    opCenters = matches.HasWildCardMatch
                        ? r.GetAll()
                        : r.Where(oc => matches.OperatingCenters.Contains(oc.Id));
                }

                // This method's not being called with an IOperatingCenterRepository instance, so GetAllSorted
                // doesn't work.
                return OperatingCentersSortFn(opCenters.Where(extraFilterP));
            };
        }

        public static Func<IEmployeeRepository, IQueryable<Employee>> GetUserOperatingCenterAccessibleEmployeesFn<TRepository, TEntity>(this ControllerBaseWithPersistence<TRepository, TEntity, User> that, RoleModules module, RoleActions action = RoleActions.Read)
            where TRepository : class, IRepository<TEntity>
            where TEntity : class
        {
            var currentUser = that.AuthenticationService.CurrentUser;

            return (r) => {
                IQueryable<Employee> employees;
                if (currentUser.IsAdmin)
                {
                    employees = r.GetAll();
                }
                else
                {
                    var matches = currentUser.GetCachedMatchingRoles(module, action);
                    employees = matches.HasWildCardMatch ? r.GetAll() : r.GetEmployeesByOperatingCenters(matches.OperatingCenters);
                }

                return employees;
            };
        }

        #endregion

        #region GetUserOperatingCentersEmployeesFn

        public static Func<IEmployeeRepository, IQueryable<Employee>> GetUserOperatingCentersEmployeesFn<TEntity>(this ControllerBaseWithPersistence<TEntity, User> that, RoleModules module, RoleActions action = RoleActions.Read)
            where TEntity : class
        {
            return
                GetUserOperatingCentersEmployeesFn(
                    (ControllerBaseWithPersistence<IRepository<TEntity>, TEntity, User>)that, module, action);
        }

        public static Func<IEmployeeRepository, IQueryable<Employee>> GetUserOperatingCentersEmployeesFn<TRepository, TEntity>(this ControllerBaseWithPersistence<TRepository, TEntity, User> that, RoleModules module, RoleActions action = RoleActions.Read)
            where TRepository : class, IRepository<TEntity>
            where TEntity : class
        {
            var currentUser = that.AuthenticationService.CurrentUser;

            return (r) => {
                if (currentUser.IsAdmin)
                {
                    return r.GetAllSorted();
                }

                var matches = currentUser.GetCachedMatchingRoles(module, action);

                return matches.HasWildCardMatch ? r.GetAllSorted() : r.GetEmployeesByOperatingCenters(matches.OperatingCenters);
            };
        }

        #endregion

        #region AddOperatingCenterDropDownData

        /// <summary>
        /// This method is a shortcut to avoid having to write
        /// this.AddDropDownData(this.GetAllOperatingCenters());
        /// </summary>
        /// <typeparam name="TRepository"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="that"></param>
        public static void AddOperatingCenterDropDownData<TRepository, TEntity>(this ControllerBaseWithPersistence<TRepository, TEntity, User> that)
            where TRepository : class, IRepository<TEntity>
            where TEntity : class
        {
            that.AddDynamicDropDownData<OperatingCenter, OperatingCenterDisplayItem>("OperatingCenter", GetAllOperatingCenters(that));
        }

        public static void AddOperatingCenterDropDownData<TRepository, TEntity>(this ControllerBaseWithPersistence<TRepository, TEntity, User> that, string name)
            where TRepository : class, IRepository<TEntity>
            where TEntity : class
        {
            that.AddDynamicDropDownData<OperatingCenter, OperatingCenterDisplayItem>(name, GetAllOperatingCenters(that));
        }

        /// <summary>
        /// This method is a short to avoid having to write a few extra words
        /// this.AddDropDownData(this.GetUserOperatingCentersFn(ROLE, RoleActions.EDIT));
        /// </summary>
        /// <typeparam name="TRepository"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="that"></param>
        /// <param name="module"></param>
        /// <param name="action"></param>
        public static void AddOperatingCenterDropDownDataForRoleAndAction<TRepository, TEntity>(this ControllerBaseWithPersistence<TRepository, TEntity, User> that, RoleModules module, RoleActions action = RoleActions.Read, string key = "OperatingCenter", Expression<Func<OperatingCenter, bool>> extraFilterP = null)
            where TRepository : class, IRepository<TEntity>
            where TEntity : class
        {
            extraFilterP = extraFilterP ?? (_ => true);

            that.AddDynamicDropDownData<IRepository<OperatingCenter>, OperatingCenter, OperatingCenterDisplayItem>(key,
                GetUserOperatingCentersFn(that, module, action, extraFilterP));
        }

        public static void AddOperatingCenterDropDownData<TRepository, TEntity>(this ControllerBaseWithPersistence<TRepository, TEntity, User> that, Expression<Func<OperatingCenter, bool>> filterP)
            where TRepository : class, IRepository<TEntity>
            where TEntity : class
        {
            that.AddDynamicDropDownData<OperatingCenter, OperatingCenterDisplayItem>(key: "OperatingCenter", dataGetter: GetAllOperatingCenters(that, filterP));
        }

        #endregion
    }
}
