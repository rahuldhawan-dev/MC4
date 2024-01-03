using MapCallScheduler.Library.Configuration;

namespace MapCallScheduler.JobHelpers.SpaceTimeInsight
{
    public class SpaceTimeInsightFileDumpServiceConfiguration : ISpaceTimeInsightFileDumpServiceConfiguration
    {
        #region Constants

        public const string GROUP_NAME = "spaceTimeInsightService";

        #endregion

        #region Properties

        public string GroupName => GROUP_NAME;

        public IFtpConfigSection FtpConfig => this.GetFtpConfig();

        #endregion
    }

    public interface ISpaceTimeInsightFileDumpServiceConfiguration : IFtpServiceConfiguration {}
}