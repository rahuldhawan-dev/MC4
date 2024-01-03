using MapCallScheduler.Library.JobHelpers.Sap;

namespace MapCallScheduler.JobHelpers.SapMaterial
{
    public class SapMaterialService : SapFileProcessingServiceBase<ISapMaterialFileService, ISapMaterialUpdaterService>, ISapMaterialService
    {
        #region Constructors

        public SapMaterialService(ISapMaterialFileService fileService, ISapMaterialUpdaterService updaterService) : base(fileService, updaterService) {}

        #endregion
    }
}
