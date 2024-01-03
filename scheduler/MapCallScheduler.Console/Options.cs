using System;
using CommandLine;
using CommandLine.Text;

namespace MapCallScheduler
{
    public class Options
    {
        #region Properties

        [Option('j', "job-name",
            HelpText = "Start a single job with the given name.",
            SetName = "run")]
        public string JobName { get; set; }

        public Type JobType { get; set; }

        [Option('n', "no-service",
            HelpText = "Run job(s) standalone right away, without the scheduler service.",
            SetName = "run")]
        public bool NoService { get; set; }
        
        [Option('l', "list-jobs",
            HelpText = "List available job names and exit.",
            SetName = "list")]
        public bool ListJobs { get; set; }

        #endregion
    }
}
