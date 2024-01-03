using MapCallScheduler.Library.JobHelpers.Sap;

namespace MapCallScheduler.JobHelpers.SapChemical
{
    public class SapChemicalService : SapFileProcessingServiceBase<ISapChemicalFileService, ISapChemicalUpdaterService>, ISapChemicalService
    {
        #region Constructors

        public SapChemicalService(ISapChemicalFileService fileService, ISapChemicalUpdaterService updaterService) : base(fileService, updaterService) {}

        #endregion
    }
}
