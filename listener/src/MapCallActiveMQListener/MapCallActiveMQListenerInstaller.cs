using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace MapCallActiveMQListener
{
    [RunInstaller(true)]
    public partial class MapCallActiveMQListenerInstaller : System.Configuration.Install.Installer
    {
        #region Private Members

        private ServiceInstaller _serviceInstaller;
        private ServiceProcessInstaller _processInstaller;

        #endregion

        public MapCallActiveMQListenerInstaller()
        {
            InitializeComponent();

            _processInstaller = new ServiceProcessInstaller();
            _serviceInstaller = new ServiceInstaller();

            _processInstaller.Account = ServiceAccount.LocalSystem;

            _serviceInstaller.StartType = ServiceStartMode.Automatic;

            _serviceInstaller.ServiceName = "MapCallActiveMQListener";

            Installers.Add(_serviceInstaller);
            Installers.Add(_processInstaller);
        }
    }
}
