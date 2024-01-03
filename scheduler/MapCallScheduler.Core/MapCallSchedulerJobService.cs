using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MapCallScheduler.Jobs;
using MapCallScheduler.Metadata;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.ClassExtensions.MemberInfoExtensions;
using MMSINC.Utilities;
using Quartz;

namespace MapCallScheduler
{
    /// <summary>
    /// Lists available jobs, creates jobs and job triggers.
    /// </summary>
    public class MapCallSchedulerJobService : IMapCallSchedulerJobService
    {
        #region Constants

        public const string BASE_JOB_NAMESPACE = "MapCallScheduler.Jobs";

        #endregion

        #region Private Members

        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMapCallSchedulerDateService _dateService;

        #endregion

        #region Constructors

        public MapCallSchedulerJobService(IDateTimeProvider dateTimeProvider, IMapCallSchedulerDateService dateService)
        {
            _dateTimeProvider = dateTimeProvider;
            _dateService = dateService;
        }

        #endregion

        #region Private Methods

        private DateTime DetermineStart(MemberInfo jobType)
        {
            if (jobType.HasAttribute<ImmediateAttribute>())
            {
                return _dateTimeProvider.GetCurrentDate().AddSeconds(1);
            }

            if (jobType.HasAttribute<StartAtAttribute>())
            {
                var startAt = jobType.GetCustomAttribute<StartAtAttribute>();
                return _dateTimeProvider.GetCurrentDate().GetNext(startAt.Hour, startAt.Minute);
            }

            return _dateService.GetStartDateTime();
        }

        private static Action<SimpleScheduleBuilder> DetermineSchedule(MemberInfo jobType)
        {
            if (!jobType.HasAttribute<IntervalAttribute>())
            {
                return x => x.WithIntervalInHours(24).RepeatForever();
            }

            var interval = jobType.GetCustomAttribute<IntervalAttribute>();

            return x => interval.SetInterval(x).RepeatForever();
        }

        #endregion

        #region Exposed Methods

        public IEnumerable<Type> GetAllJobs()
        {
            var types = 
                typeof(MapCallNotifierJob).Assembly.GetTypes()
                    .Where(
                        t =>
                            t.Namespace != null &&
                            t.Namespace.StartsWith(BASE_JOB_NAMESPACE) &&
                            !t.IsAbstract &&
                            t.IsSubclassOf(typeof(MapCallJobBase)));
            return types;
        }

        public IJobDetail Build(Type jobType, string name, string group)
        {
            return JobBuilder.Create(jobType)
                .WithIdentity(name, group)
                .Build();
        }

        public ITrigger BuildTrigger(Type jobType, string name, string group)
        {
            return TriggerBuilder.Create()
                .WithIdentity(name, group)
                .StartAt(DetermineStart(jobType))
                .WithSimpleSchedule(DetermineSchedule(jobType))
                .Build();
        }

        public bool HasImmediateAttribute(Type jobType)
        {
            return jobType.HasAttribute<ImmediateAttribute>();
        }

        #endregion
    }

    public interface IMapCallSchedulerJobService
    {
        #region Abstract Methods

        IEnumerable<Type> GetAllJobs();
        IJobDetail Build(Type jobType, string name, string group);
        ITrigger BuildTrigger(Type jobType, string name, string group);
        bool HasImmediateAttribute(Type jobType);

        #endregion
    }
}