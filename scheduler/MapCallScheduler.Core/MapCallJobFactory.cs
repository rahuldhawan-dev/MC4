using System;
using log4net;
using MapCallScheduler.Metadata;
using MMSINC.ClassExtensions.MemberInfoExtensions;
using Quartz;
using Quartz.Spi;
using StructureMap;

namespace MapCallScheduler
{
    /// <summary>
    /// Used by the Quartz scheduler internally to instantiate jobs.  This
    /// class is how StructureMap gets wired into that process.
    /// </summary>
    public class MapCallJobFactory : IJobFactory
    {
        #region Private Members

        private readonly IContainer _container;
        private ILog _log;

        #endregion

        #region Properties

        protected IContainer Container
        {
            get { return _container; }
        }

        #endregion

        #region Constructors

        public MapCallJobFactory(IContainer container, ILog log)
        {
            _log = log;
            _container = container;
        }

        #endregion

        #region Exposed Methods

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            try
            {
                var container = Container.GetNestedContainer(
                    bundle.JobDetail.JobType.HasAttribute<NoConfigureSessionAttribute>()
                        ? DependencyRegistry.NO_REGISTER_ISESSION
                        : DependencyRegistry.REGISTER_ISESSION);

                return (IJob)container.GetInstance(bundle.JobDetail.JobType);
            }
            catch (Exception e)
            {
                _log.Error($"Problem instantiating job: {e}");
                throw new SchedulerException("Problem instantiating job", e);
            }
        }

        public void ReturnJob(IJob job) {}

        #endregion
    }
}