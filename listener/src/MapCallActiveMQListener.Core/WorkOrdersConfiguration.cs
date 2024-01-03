using MMSINC.Utilities.ActiveMQ;

namespace MapCallActiveMQListener
{
    public class WorkOrdersConfiguration : ActiveMQConfiguration
    {
        #region Constants

        public const string GROUP_NAME = "workOrders";

        #endregion

        #region Properties

        public override string GroupName => GROUP_NAME;

        #endregion
    }
}
