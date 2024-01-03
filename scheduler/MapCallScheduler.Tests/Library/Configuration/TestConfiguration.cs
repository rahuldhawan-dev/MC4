using MapCallScheduler.Library.Configuration;

namespace MapCallScheduler.Tests.Library.Configuration
{
    public class TestConfiguration : IFtpServiceConfiguration, IIncomingEmailServiceConfiguration
    {
        #region Properties

        public string GroupName => "test";

        public IFtpConfigSection FtpConfig => this.GetFtpConfig();

        public IIncomingEmailConfigSection IncomingEmailConfig => this.GetIncomingEmailConfig();

        #endregion
    }
}
