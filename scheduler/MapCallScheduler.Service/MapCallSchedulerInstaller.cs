using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace MapCallScheduler
{
    [RunInstaller(true)]
    public partial class MapCallSchedulerInstaller : Installer
    {
        #region Private Members

        private ServiceInstaller _serviceInstaller;
        private ServiceProcessInstaller _processInstaller;

        #endregion

        #region Constructors

        public MapCallSchedulerInstaller()
        {
            InitializeComponent();

            _processInstaller = new ServiceProcessInstaller();
            _serviceInstaller = new ServiceInstaller();

            _processInstaller.Account = ServiceAccount.LocalSystem;

            _serviceInstaller.StartType = ServiceStartMode.Automatic;

            _serviceInstaller.ServiceName = "MapCallScheduler";

            Installers.Add(_serviceInstaller);
            Installers.Add(_processInstaller);
        }

        #endregion
    }
}
