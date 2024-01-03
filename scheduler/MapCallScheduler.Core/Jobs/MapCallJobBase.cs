using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using log4net;
using MapCallScheduler.Library.Email;
using MapCallScheduler.Library.Quartz;
using NHibernate;
using Quartz;
using StructureMap;
using StructureMap.Pipeline;

namespace MapCallScheduler.Jobs
{
    [DisallowConcurrentExecution]
    public abstract class MapCallJobBase : IJob
    {
        #region Private Members

        [ThreadStatic]
        private static IObjectCache objectCache;

        protected readonly ILog _log;
        protected readonly IDeveloperEmailer _emailer;

        #endregion

        #region Properties

        protected ILog Log => _log;
        protected IDeveloperEmailer Emailer => _emailer;

        public static IObjectCache Cache => objectCache;

        #endregion

        #region Constructors

        public MapCallJobBase(ILog log, IDeveloperEmailer emailer)
        {
            _log = log;
            _emailer = emailer;
        }

        #endregion

        #region Private Methods

        protected static void RegisterANewCache()
        {
            objectCache = new LifecycleObjectCache();
        }

        #endregion

        #region Abstract Methods

        protected abstract void ExecuteJob(IJobExecutionContext context);

        #endregion

        #region Exposed Methods

        public Task Execute(IJobExecutionContext context)
        {
            RegisterANewCache();

            var beforeConnections =
                IPGlobalProperties.GetIPGlobalProperties().GetTcpIPv4Statistics().CurrentConnections;
            Log.Debug($"{GetType().Name} Connections Before Run: {beforeConnections}");
            Log.Info($"Executing Job {GetType().Name}");

            ExecuteJob(context);

            var afterConnections =
                IPGlobalProperties.GetIPGlobalProperties().GetTcpIPv4Statistics().CurrentConnections;
            Log.Debug($"{GetType().Name} Connections After Run: {afterConnections}, Difference: {afterConnections - beforeConnections}");

            return Task.CompletedTask;
        }

        #endregion
    }
}