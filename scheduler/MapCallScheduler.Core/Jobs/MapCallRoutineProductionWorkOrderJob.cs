using log4net;
using MapCallScheduler.JobHelpers.MapCallRoutineProductionWorkOrder;
using MapCallScheduler.Library.Email;
using MapCallScheduler.Metadata;

namespace MapCallScheduler.Jobs
{
    [Daily, StartAt(6, 15)]
    public class MapCallRoutineProductionWorkOrderJob : MapCallJobWithProcessableServiceAndAwwErrorEmailBase<IMapCallRoutineProductionWorkOrderService>
    {
        #region Constants

        public const string ERROR_MESSAGE = "Error scheduling Routine Production Work Orders";

        #endregion

        #region Properties

        protected override string ExtraEmailSubject => ERROR_MESSAGE;

        #endregion

        #region Constructors

        public MapCallRoutineProductionWorkOrderJob(ILog log, IMapCallRoutineProductionWorkOrderService service, IDeveloperEmailer emailer) : base(log, service, emailer) { }

        #endregion
    }
}
