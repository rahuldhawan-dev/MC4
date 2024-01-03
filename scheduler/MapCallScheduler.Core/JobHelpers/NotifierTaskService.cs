using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using MapCall.Common.Utility.Notifications;
using MapCall.Common.Utility.Scheduling;
using StructureMap;

namespace MapCallScheduler.JobHelpers
{
    public class NotifierTaskService : INotifierTaskService
    {
        #region Constants

        public const string BASE_TASK_NAMESPACE = "MapCall.Common.Configuration.NotificationTasks";

        #endregion

        #region Private Members

        private readonly ILog _log;
        private readonly IContainer _container;

        #endregion

        #region Properties

        protected ILog Log
        {
            get { return _log; }
        }

        protected IContainer Container
        {
            get { return _container; }
        }

        #endregion

        #region Constructors

        public NotifierTaskService(ILog log, IContainer container)
        {
            _log = log;
            _container = container;
        }

        #endregion

        #region Private Methods

        private ITask InstantiateTask(Type type)
        {
            ITask task;

            try
            {
                Log.InfoFormat("Instantiating {0} task...", type.Name);
                task = (ITask)Container.GetInstance(type);
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Exception encountered instantiating {0} task: {1}", type.Name, e);
                throw;
            }

            if (task != null)
            {
                return task;
            }

            var error = String.Format("Could not instantiate {0} task...", type.Name);
            Log.Error(error);
            throw new NullReferenceException(error);
        }

        private IEnumerable<Type> GetAllTypes()
        {
            var types = 
                typeof(MapCallNotifierTask).Assembly.GetTypes()
                    .Where(
                        t =>
                            t.Namespace != null &&
                            t.Namespace.StartsWith(BASE_TASK_NAMESPACE) &&
                            !t.IsAbstract &&
                            t.IsSubclassOf(typeof(MapCallNotifierTask)));
            return types;
        }

        #endregion

        #region Exposed Methods

        public IEnumerable<ITask> GetAllTasks()
        {
            foreach (var task in GetAllTypes())
            {
                yield return InstantiateTask(task);
            }
        }

        #endregion
    }

    public interface INotifierTaskService
    {
        #region Abstract Methods

        IEnumerable<ITask> GetAllTasks();

        #endregion
    }
}