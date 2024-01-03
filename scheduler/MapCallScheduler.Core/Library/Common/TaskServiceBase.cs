using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MMSINC.ClassExtensions.TypeExtensions;
using StructureMap;

namespace MapCallScheduler.Library.Common
{
    public abstract class TaskServiceBase<TTaskBase> : ITaskService<TTaskBase>
    {
        #region Private Members

        protected readonly IContainer _container;

        #endregion

        #region Properties

        protected virtual Assembly TaskAssembly => typeof(TTaskBase).Assembly;
        protected virtual string[] SkipTaskNames => new string[0];

        #endregion

        #region Constructors

        public TaskServiceBase(IContainer container)
        {
            _container = container;
        }

        #endregion

        #region Private Methods

        protected virtual IEnumerable<Type> GetAllTaskTypes()
        {
            var baseType = typeof(TTaskBase);
            return TaskAssembly
                  .GetTypes()
                  .Where(t =>
                       !t.IsAbstract &&
                       !SkipTaskNames.Contains(t.Name) &&
                       (baseType.IsInterface
                           ? t.Implements(baseType)
                           : t.IsSubclassOf(baseType)));
        }

        protected TTask InstantiateTask<TTask>(Type taskType)
        {
            return (TTask)_container.GetInstance(taskType);
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<TTaskBase> GetAllTasks()
        {
            var types = GetAllTaskTypes();
            return types.Select(InstantiateTask<TTaskBase>);
        }

        #endregion
    }

    public interface ITaskService<out TTaskBase>
    {
        #region Abstract Methods

        IEnumerable<TTaskBase> GetAllTasks();

        #endregion
    }
}
