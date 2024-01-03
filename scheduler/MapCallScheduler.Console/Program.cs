using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using CommandLine;
using MapCallScheduler.Jobs;
using Quartz;
using Quartz.Spi;
using StructureMap;

namespace MapCallScheduler
{
    public class Program
    {
        #region Constants

        public static readonly Type MAPCALL_JOB_BASE = typeof(MapCallJobBase);

        #endregion

        #region Properties

        public static IEnumerable<Type> JobTypes
        {
            get { return MAPCALL_JOB_BASE.Assembly.GetTypes().Where(x => x.IsSubclassOf(MAPCALL_JOB_BASE)); }
        }

        #endregion

        #region Private Methods

        private static Options ParseArguments(string[] args)
        {
            return Parser.Default.ParseArguments<Options>(args)
                         .MapResult((opts) => {
                              if (!string.IsNullOrWhiteSpace(opts.JobName))
                              {
                                  opts.JobType = LoadJobType(opts.JobName);
                              }
                              return opts;
                          }, errors => throw new ArgumentException("Command line arguments invalid."));
        }

        private static Type LoadJobType(string jobName)
        {
            var expected = jobName.EndsWith("Job") ? jobName : jobName + "Job";

            foreach (var jobType in JobTypes)
            {
                if (jobType.Name == expected)
                {
                    return jobType;
                }
            }

            var jobNames = string.Join(Environment.NewLine, ListJobTypeNames());

            throw new ArgumentException(
                $"Invalid job name '{jobName}'. Options are:{Environment.NewLine}{jobNames}");
        }

        private static IEnumerable<string> ListJobTypeNames()
        {
            return JobTypes
                   .OrderBy(x => x.Name)
                   .Select(x => new Regex("(.+)Job$").Replace(x.Name, "$1"));
        }

        static void Main(string[] args)
        {
            var container = new Container(new DependencyRegistry());
            Options options;

            try
            {
                options = ParseArguments(args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            Run(options, container);
        }

        private static void Run(Options options, IContainer container)
        {
            if (options.ListJobs)
            {
                ListJobs();
            }
            else if (options.NoService)
            {
                RunNow(options, container);
            }
            else
            {
                RunService(options, container);
            }
        }

        private static void ListJobs()
        {
            Console.WriteLine("The following jobs are available:");
            foreach (var jobType in ListJobTypeNames())
            {
                Console.WriteLine($"    {jobType}");
            }
        }

        private static void RunNow(Options options, IContainer container)
        {
            var jobTypes = options.JobType == null ? JobTypes : new[] {options.JobType};
            var jobFactory = container.GetInstance<IJobFactory>();
            
            Console.WriteLine((options.JobName != null
                ? String.Format("Starting job '{0}'", options.JobName)
                : "Starting all jobs") + " standalone." + Environment.NewLine);
            Thread.Sleep(500);

            foreach (var jobType in jobTypes)
            {
                var job = (MapCallJobBase)jobFactory.NewJob(new StubTriggerFiredBundle(jobType), null);

                job.Execute(null);
            }

            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }

        private static void RunService(Options options, IContainer container)
        {
            var service = container.GetInstance<IMapCallSchedulerService>();

            Console.WriteLine((options.JobName != null
                ? String.Format("Starting job '{0}'", options.JobName)
                : "Starting all jobs") + " within service.");
            Console.WriteLine("Type \"exit\" at any time to quit." + Environment.NewLine);
            Thread.Sleep(500);

            service.Start(options.JobType);

            string input = null;
            while (input != "exit")
            {
                input = Console.ReadLine();
            }

            service.Stop();
        }

        #endregion

        private class StubTriggerFiredBundle : TriggerFiredBundle
        {
            private readonly Type _jobType;

            public override IJobDetail JobDetail => new StubJobDetail(_jobType);

            public StubTriggerFiredBundle(Type jobType) : base(null, null, null, false, DateTimeOffset.Now, null, null, null)
            {
                _jobType = jobType;
            }
        }

        private class StubJobDetail : IJobDetail
        {
            private readonly Type _jobType;

            IJobDetail IJobDetail.Clone()
            {
                throw new NotImplementedException();
            }

            public JobBuilder GetJobBuilder()
            {
                throw new NotImplementedException();
            }

            public StubJobDetail(Type jobType)
            {
                _jobType = jobType;
            }

            public JobKey Key => throw new NotImplementedException();
            public string Description => throw new NotImplementedException();
            public Type JobType => _jobType;
            public JobDataMap JobDataMap => throw new NotImplementedException();
            public bool Durable => throw new NotImplementedException();
            public bool PersistJobDataAfterExecution => throw new NotImplementedException();
            public bool ConcurrentExecutionDisallowed => throw new NotImplementedException();
            public bool RequestsRecovery => throw new NotImplementedException();
        }
    }
}
