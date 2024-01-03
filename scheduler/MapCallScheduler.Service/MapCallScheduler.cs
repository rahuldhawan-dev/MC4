using System.ServiceProcess;

namespace MapCallScheduler
{
    public partial class MapCallScheduler : ServiceBase
    {
        #region Private Members

        private readonly IMapCallSchedulerService _service;
        private readonly IMapCallSchedulerConfiguration _config;

        #endregion

        #region Constructors

        public MapCallScheduler()
        {
            InitializeComponent();
        }

        public MapCallScheduler(IMapCallSchedulerService service, IMapCallSchedulerConfiguration config)
        {
            _service = service;
            _config = config;
        }

        #endregion

        #region Private Methods

        protected override void OnStart(string[] args)
        {
            if (!string.IsNullOrWhiteSpace(_config.JobName))
            {
                _service.Start(_config.JobName);
            }
            else
            {
                _service.Start();
            }
        }

        protected override void OnStop()
        {
            _service.Stop();
        }

        #endregion
    }
}
