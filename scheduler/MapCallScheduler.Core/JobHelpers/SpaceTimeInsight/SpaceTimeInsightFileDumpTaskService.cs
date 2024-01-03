using System;
using System.Collections.Generic;
using System.Linq;
using MapCallScheduler.JobHelpers.SpaceTimeInsight.Tasks;
using MapCallScheduler.Library.Common;
using MMSINC.ClassExtensions.ReflectionExtensions;
using StructureMap;

namespace MapCallScheduler.JobHelpers.SpaceTimeInsight
{
    public class SpaceTimeInsightFileDumpTaskService : ISpaceTimeInsightFileDumpTaskService
    {
        #region Private Members

        private readonly IContainer _container;
        private static IEnumerable<Type> _dailyTypes, _monthlyTypes;

        #endregion

        #region Constructors

        public SpaceTimeInsightFileDumpTaskService(IContainer container)
        {
            _container = container;
        }

        #endregion

        #region Private Methods

        private IEnumerable<Type> GetByBase(Type baseType)
        {
            return typeof(SpaceTimeInsightFileDumpTaskBase<,>).Assembly.GetTypes()
                .Where(t => !t.IsAbstract && t.IsSubclassOfRawGeneric(baseType));
        }

        protected virtual IEnumerable<Type> GetAllDailyTypes()
        {
            return _dailyTypes ?? (_dailyTypes = GetByBase(typeof(SpaceTimeInsightDailyFileDumpTaskBase<,>)));
        }

        protected virtual IEnumerable<Type> GetAllMonthlyTypes()
        {
            return _monthlyTypes ?? (_monthlyTypes = GetByBase(typeof(SpaceTimeInsightMonthlyFileDumpTaskBase<,>)));
        }

        private ISpaceTimeInsightFileDumpTask InstantiateTask(Type taskType)
        {
            return (ISpaceTimeInsightFileDumpTask)_container.GetInstance(taskType);
        }

        #endregion

        #region Exposed Methods

        public IEnumerable<ISpaceTimeInsightFileDumpTask> GetAllMonthlyTasks()
        {
            return GetAllMonthlyTypes().Select(InstantiateTask);
        }

        public IEnumerable<ISpaceTimeInsightFileDumpTask> GetAllDailyTasks()
        {
            return GetAllDailyTypes().Select(InstantiateTask);
        }

        public IEnumerable<ISpaceTimeInsightFileDumpTask> GetAllTasks()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public interface ISpaceTimeInsightFileDumpTaskService : ITaskService<ISpaceTimeInsightFileDumpTask>
    {
        #region Abstract Methods

        IEnumerable<ISpaceTimeInsightFileDumpTask> GetAllDailyTasks();
        IEnumerable<ISpaceTimeInsightFileDumpTask> GetAllMonthlyTasks();

        #endregion
    }
}