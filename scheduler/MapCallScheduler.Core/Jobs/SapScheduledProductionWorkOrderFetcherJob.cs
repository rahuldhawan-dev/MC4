using log4net;
using MapCallScheduler.JobHelpers.SapProductionWorkOrder;
using MapCallScheduler.JobHelpers.ScadaData;
using MapCallScheduler.Library.Email;
using MapCallScheduler.Metadata;
using Quartz;

namespace MapCallScheduler.Jobs
{
    [Minutely(5), Immediate]
    public class SapScheduledProductionWorkOrderFetcherJob : MapCallJobWithProcessableServiceBase<ISapScheduledProductionWorkOrderService>
    {
        public SapScheduledProductionWorkOrderFetcherJob(ILog log, ISapScheduledProductionWorkOrderService service, IDeveloperEmailer emailer) : base(log, service, emailer) { }
    }

//#if DEBUG
//    [Immediate]
//    public class SapManualProductionWorkOrderFetcherJob : MapCallJobWithProcessableServiceBase<SapManualProductionWorkOrderService.ISapManualProductionWorkOrderService>
//    {
//        public SapManualProductionWorkOrderFetcherJob(ILog log, SapManualProductionWorkOrderService.ISapManualProductionWorkOrderService service, IDeveloperEmailer emailer) : base(log, service, emailer) { }
//    }
//#endif  
}
