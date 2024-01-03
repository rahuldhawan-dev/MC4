using Quartz;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using log4net;
using MapCallScheduler.Metadata;
using MMSINC.ClassExtensions.MemberInfoExtensions;
using MMSINC.Utilities;

namespace MapCallScheduler
{
    /// <summary>
    /// Uses a MapCallSchedulerJobService to schedule jobs with an IScheduler.
    /// </summary>
    public class MapCallSchedulerService : IMapCallSchedulerService
    {
        #region Private Members

        private readonly IScheduler _scheduler;
        private readonly IList<TriggerKey> _triggerKeys;
        private readonly IMapCallSchedulerJobService _jobService;
        private readonly ILog _log;
        #endregion

        #region Properties

        internal IList<TriggerKey> TriggerKeys
        {
            get { return _triggerKeys; }
        }

        #endregion

        #region Constructors

        public MapCallSchedulerService(IScheduler scheduler, IMapCallSchedulerJobService jobService, ILog log)
        {
            _scheduler = scheduler;
            _jobService = jobService;
            _triggerKeys = new List<TriggerKey>();
            _log = log;
        }

        #endregion

        #region Private Methods

        private void ScheduleJob(Type jobType)
        {
            var name = jobType.Name;
            var group = name.Replace("Job", "Group");
            var triggerName = name.Replace("Job", "Trigger");
            var job = _jobService.Build(jobType, name, group);
            var trigger = _jobService.BuildTrigger(jobType, triggerName, group);
            if (job != null)
                _log.Info($"Job: {name} - Key: {job.Key} Scheduled, Description: {job.Description} JobTye: {job.JobType} Disallowed: {job.ConcurrentExecutionDisallowed} durable: {job.Durable} startat: {trigger.StartTimeUtc} {trigger.Priority}");
            _scheduler.ScheduleJob(job, trigger);

            TriggerKeys.Add(trigger.Key);

            //StartAt isn't always triggered by Quartz
            //This says to trigger all the jobs with immediate so they start
            if (job != null && job.JobType != null && _jobService.HasImmediateAttribute(job.JobType))
            {
                _scheduler.TriggerJob(job.Key);
            }
        }


        #endregion

        #region Exposed Methods

        public void Start(string jobNames)
        {
            var jobs = new List<Type>();
            foreach (var jobName in jobNames.Split(','))
            {
                var expected = jobName.EndsWith("Job") ? jobName : jobName + "Job";
                var job = _jobService.GetAllJobs().SingleOrDefault(j => j.Name == expected);
                if (job == null)
                {
                    throw new ArgumentException($"Invalid job name '{jobName}'.");
                }

                jobs.Add(job);
            }

            Start(jobs.ToArray());
        }

        public void Start(params Type[] onlyJobs)
        {
            var jobs = onlyJobs.Any() ? onlyJobs : _jobService.GetAllJobs();

            foreach (var job in jobs)
            {
                ScheduleJob(job);
            }
            _scheduler.Start();
        }

        public void Stop()
        {
            _scheduler.UnscheduleJobs(new ReadOnlyCollection<TriggerKey>(_triggerKeys));
            TriggerKeys.Clear();
            _scheduler.Shutdown();
        }

        #endregion
    }

    public interface IMapCallSchedulerService
    {
        void Start(string jobNames);
        void Start(params Type[] onlyJobs);
        void Stop();
    }
}
