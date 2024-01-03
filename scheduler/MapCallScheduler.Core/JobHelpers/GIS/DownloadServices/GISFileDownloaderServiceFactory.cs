using StructureMap;

namespace MapCallScheduler.JobHelpers.GIS.DownloadServices
{
    public class GISFileDownloadServiceServiceFactory : IGISFileDownloadServiceServiceFactory
    {
        #region Private Members

        private readonly IContainer _container;

        #endregion

        #region Constructors

        public GISFileDownloadServiceServiceFactory(IContainer container)
        {
            _container = container;
        }

        #endregion

        #region Exposed Methods

        public HydrantService BuildHydrantDownloadService()
        {
            return _container.GetInstance<HydrantService>();
        }

        public SewerOpeningService BuildSewerOpeningDownloadService()
        {
            return _container.GetInstance<SewerOpeningService>();
        }

        public ValveService BuildValveDownloadService()
        {
            return _container.GetInstance<ValveService>();
        }

        public ServiceService BuildServiceDownloadService()
        {
            return _container.GetInstance<ServiceService>();
        }

        #endregion
    }

    public interface IGISFileDownloadServiceServiceFactory
    {
        #region Abstract Methods

        HydrantService BuildHydrantDownloadService();
        SewerOpeningService BuildSewerOpeningDownloadService();
        ValveService BuildValveDownloadService();
        ServiceService BuildServiceDownloadService();

        #endregion
    }
}
