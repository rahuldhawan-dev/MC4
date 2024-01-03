using MapCallScheduler.Library;
using MapCallScheduler.Library.Configuration;

namespace MapCallScheduler.JobHelpers.MarkoutTickets
{
    public class MarkoutTicketServiceConfiguration : IMarkoutTicketServiceConfiguration
    {
        #region Constants

        public const string GROUP_NAME = "markoutTicketService";

        #endregion

        #region Properties

        public string GroupName => GROUP_NAME;

        public IIncomingEmailConfigSection IncomingEmailConfig => this.GetIncomingEmailConfig();

        #endregion
    }

    public interface IMarkoutTicketServiceConfiguration : IIncomingEmailServiceConfiguration {}
}