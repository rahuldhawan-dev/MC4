using MapCallScheduler.JobHelpers.GISMessageBroker.Tasks;
using MapCallScheduler.Library.Common;
using StructureMap;

namespace MapCallScheduler.JobHelpers.GISMessageBroker
{
    public class GISMessageBrokerTaskService : TaskServiceBase<IGISMessageBrokerTask>, IGISMessageBrokerTaskService
    {
        #region Constructors

        public GISMessageBrokerTaskService(IContainer container) : base(container) { }

        #endregion
    }

    public interface IGISMessageBrokerTaskService : ITaskService<IGISMessageBrokerTask> {}
}
