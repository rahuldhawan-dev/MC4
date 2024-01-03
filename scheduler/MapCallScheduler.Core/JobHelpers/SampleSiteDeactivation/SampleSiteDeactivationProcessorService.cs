using log4net;
using MapCall.Common.Model.Entities;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallScheduler.JobHelpers.SampleSiteDeactivation
{
    public class SampleSiteDeactivationProcessorService : ISampleSiteDeactivationProcessorService
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IRepository<SampleSite> _sampleSiteRepository;
        private readonly IRepository<MostRecentlyInstalledService> _recentlyInstalledServiceRepository;
        private readonly ILog _log;

        public SampleSiteDeactivationProcessorService(
            IDateTimeProvider dateTimeProvider,
            IRepository<SampleSite> sampleSiteRepository,
            IRepository<MostRecentlyInstalledService> recentlyInstalledServiceRepository,
            ILog log)
        {
            _dateTimeProvider = dateTimeProvider;
            _sampleSiteRepository = sampleSiteRepository;
            _recentlyInstalledServiceRepository = recentlyInstalledServiceRepository;
            _log = log;
        }

        public void Process()
        {
            _log.Info("Job SampleSiteDeactivationProcessorService started");
            var startDateTime = _dateTimeProvider.GetCurrentDate().GetPreviousDay().BeginningOfDay();
            var services = _recentlyInstalledServiceRepository.Where(r => r.UpdatedAt > startDateTime);
            foreach (var service in services)
            {
                if (service.Premise?.SampleSite == null || //sample site is null, do nth
                    service.Premise.SampleSite.Status?.Id == SampleSiteStatus.Indices.INACTIVE || // sample site status is already inactive, do nth
                    !service.Premise.SampleSite.LeadCopperSite.HasValue || // LeadCopperSite value is not set, do nth
                    !service.Premise.SampleSite.LeadCopperSite.Value) // Isn't a LeadCopperSite, do nth
                {
                    continue;
                }

                service.Premise.SampleSite.Status = new SampleSiteStatus {
                    Id = SampleSiteStatus.Indices.INACTIVE
                };
                service.Premise.SampleSite.SampleSiteInactivationReason = new SampleSiteInactivationReason {
                    Id = SampleSiteInactivationReason.Indices.NEW_SERVICE_DETAILS
                };
                _sampleSiteRepository.Save(service.Premise.SampleSite);
                _log.Info($"Sample Site Id: {service.Premise.SampleSite.Id} updated via Service Record Id: {service.Id}");
            }
            _log.Info("Job SampleSiteDeactivationProcessorService ended");
        }
    }
}
