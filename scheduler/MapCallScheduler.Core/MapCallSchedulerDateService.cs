using System;
using System.Text.RegularExpressions;
using MMSINC.ClassExtensions.RegexExtensions;
using MMSINC.Utilities;
using Quartz.Impl.AdoJobStore;

namespace MapCallScheduler
{
    public class MapCallSchedulerDateService : IMapCallSchedulerDateService
    {
        #region Constants

        public const string CONFIG_START_TIME_REGEX = "^([0-1][0-9]|2[0-3])(?::([0-5][0-9]))?$";

        #endregion

        #region Private Members

        private readonly IMapCallSchedulerConfiguration _config;
        private readonly IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Constructors

        public MapCallSchedulerDateService(IMapCallSchedulerConfiguration config, IDateTimeProvider dateTimeProvider)
        {
            _config = config;
            _dateTimeProvider = dateTimeProvider;
        }

        #endregion

        #region Exposed Methods

        public DateTime GetStartDateTime()
        {
            var configValue = _config.StartTime;
            Match match;

            if (configValue == "IMMEDIATE")
            {
                return _dateTimeProvider.GetCurrentDate().AddSeconds(1);
            }
            if (new Regex(CONFIG_START_TIME_REGEX).TryMatch(configValue, out match))
            {
                return _dateTimeProvider.GetNext(int.Parse(match.Groups[1].Value),
                    match.Groups[2].Success ? int.Parse(match.Groups[2].Value) : 0);
            }

            throw new InvalidConfigurationException(
                String.Format("Error parsing app setting with key '{0}', value is '{1}'", MapCallSchedulerConfiguration.ConfigKeys.START_TIME,
                    configValue));
        }

        #endregion
    }

    public interface IMapCallSchedulerDateService
    {
        #region Abstract Methods

        DateTime GetStartDateTime();

        #endregion
    }
}
