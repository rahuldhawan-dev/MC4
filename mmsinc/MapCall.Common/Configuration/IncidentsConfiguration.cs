using MMSINC.Utilities.ActiveMQ;

namespace MapCall.Common.Configuration
{
    public class IncidentsConfiguration : ActiveMQConfiguration
    {
        #region Constants

        public const string GROUP_NAME = "incidents";

        #endregion

        #region Properties

        public override string GroupName => GROUP_NAME;

        #endregion
    }
}
