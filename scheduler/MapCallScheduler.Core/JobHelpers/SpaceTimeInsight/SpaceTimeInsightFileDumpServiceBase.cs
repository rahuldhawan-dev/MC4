using log4net;
using MapCallScheduler.JobHelpers.SpaceTimeInsight.Tasks;
using MapCallScheduler.Library.JobHelpers.FileDumps;

namespace MapCallScheduler.JobHelpers.SpaceTimeInsight
{
    public abstract class SpaceTimeInsightFileDumpServiceBase : FileDumpServiceBase<ISpaceTimeInsightFileDumpTaskService, ISpaceTimeInsightFileDumpTask>, ISpaceTimeInsightFileDumpService
    {
        #region Properties

        public override string Descriptor => "space time insight";

        #endregion

        #region Constructors

        protected SpaceTimeInsightFileDumpServiceBase(ILog log, ISpaceTimeInsightFileDumpTaskService taskService) : base(log, taskService) { }

        #endregion
    }

    public interface ISpaceTimeInsightFileDumpService : IFileDumpService {}
}