using MapCallScheduler.Library.JobHelpers.Sap;

namespace MapCallScheduler.JobHelpers.SapPremise
{
    public class SapPremiseService : SapFileProcessingServiceBase<ISapPremiseFileService, ISapPremiseUpdaterService>, ISapPremiseService
    {
        public SapPremiseService(ISapPremiseFileService fileService, ISapPremiseUpdaterService updaterService) : base(fileService, updaterService)
        {
        }
    }
}