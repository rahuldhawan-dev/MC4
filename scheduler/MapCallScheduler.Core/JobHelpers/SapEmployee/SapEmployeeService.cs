using MapCallScheduler.Library.JobHelpers.Sap;

namespace MapCallScheduler.JobHelpers.SapEmployee
{
    public class SapEmployeeService : SapFileProcessingServiceBase<ISapEmployeeFileService, ISapEmployeeUpdaterService>, ISapEmployeeService
    {
        #region Constructors

        public SapEmployeeService(ISapEmployeeFileService fileService, ISapEmployeeUpdaterService updaterService) : base(fileService, updaterService) { }

        #endregion
    }
}
