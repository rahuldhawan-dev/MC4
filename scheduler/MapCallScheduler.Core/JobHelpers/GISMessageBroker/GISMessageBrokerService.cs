using log4net;
using MapCallScheduler.JobHelpers.GISMessageBroker.Tasks;
using MapCallScheduler.Library.Common;
using MapCallScheduler.Library.JobHelpers.Common;

namespace MapCallScheduler.JobHelpers.GISMessageBroker
{
    public class GISMessageBrokerService : TaskProcessingServiceBase<IGISMessageBrokerTaskService, IGISMessageBrokerTask>, IGISMessageBrokerService
    {
        #region Properties

        public override string TaskDescriptor => "message broker";
        public override string Descriptor { get; }

        #endregion

        #region Constructors

        public GISMessageBrokerService(ILog log, IGISMessageBrokerTaskService taskService) : base(log, taskService) { }

        #endregion
    }

    public interface IGISMessageBrokerService : IProcessableService {}
}
