using System;
using System.Configuration;

namespace MapCallScheduler
{
    public class MapCallSchedulerConfiguration : IMapCallSchedulerConfiguration
    {
        #region Constants

        public struct ConfigKeys
        {
            public const string 
                IS_PRODUCTION = "IsProduction",
                IS_STAGING = "IsStaging",
                START_TIME = "StartTime",
                ALL_EMAILS_GO_TO = "AllEmailsGoTo",
                JOB_NAME = "JobName",
                MAXIMUM_CONCURRENT_REQUESTS = "MaximumConcurrentRequests", 
                MAXIMUM_ROWS = "MaximumRows",
                API_KEY = "AW-API-KEY",
                API_URL = "AW-API-URL";
        }

        #endregion

        #region Properties

        public bool IsProduction => Convert.ToBoolean(ConfigurationManager.AppSettings[ConfigKeys.IS_PRODUCTION]); 
        public bool IsStaging => Convert.ToBoolean(ConfigurationManager.AppSettings[ConfigKeys.IS_STAGING]);

        public string StartTime => ConfigurationManager.AppSettings[ConfigKeys.START_TIME];

        public string AllEmailsGoTo => ConfigurationManager.AppSettings[ConfigKeys.ALL_EMAILS_GO_TO];

        public string JobName => ConfigurationManager.AppSettings[ConfigKeys.JOB_NAME];

        public int MaximumConcurrentRequests => Convert.ToInt32(ConfigurationManager.AppSettings[ConfigKeys.MAXIMUM_CONCURRENT_REQUESTS]);
        
        public int MaximumRows => Convert.ToInt32(ConfigurationManager.AppSettings[ConfigKeys.MAXIMUM_ROWS]);

        public string APIKey => ConfigurationManager.AppSettings[ConfigKeys.API_KEY];
        public string APIURL => ConfigurationManager.AppSettings[ConfigKeys.API_URL];
        
        #endregion
    }

    public interface IMapCallSchedulerConfiguration
    {
        #region Abstract Properties

        bool IsProduction {get; }
        bool IsStaging { get; }
        string StartTime { get; }
        string AllEmailsGoTo { get; }
        string JobName { get; }
        int MaximumConcurrentRequests { get; }
        int MaximumRows { get; }
        string APIKey { get; }
        string APIURL { get; }
        
        #endregion
    }
}
