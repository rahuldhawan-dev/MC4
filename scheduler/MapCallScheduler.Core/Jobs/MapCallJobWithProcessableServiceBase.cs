using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using log4net;
using MapCallScheduler.Library.Common;
using MapCallScheduler.Library.Email;
using MMSINC.ClassExtensions.StringExtensions;
using Quartz;

namespace MapCallScheduler.Jobs
{
    public abstract class MapCallJobWithProcessableServiceBase<TService> : MapCallJobBase
        where TService : IProcessableService
    {
        #region Private Members

        protected readonly TService _service;

        #endregion

        #region Constructors

        public MapCallJobWithProcessableServiceBase(ILog log, TService service, IDeveloperEmailer emailer) : base(log, emailer)
        {
            _service = service;
        }

        #endregion

        #region Private Methods

        protected virtual void OnError(Exception e)
        {
            var subject = "Error in " + GetServiceName();
            Log.Error(subject, e);
            Emailer.SendErrorMessage("MapCallScheduler: " + subject, e);
        }

        protected virtual void DoProcess()
        {
            _service.Process();
        }

        protected override void ExecuteJob(IJobExecutionContext context)
        {
            if (Debugger.IsAttached)
            {
                DoProcess();
                return;
            }

            try
            {
                DoProcess();
                _log.Info($"Called DoProcess - MapCallJobWithProcessableServiceBase.ExecuteJob {typeof(TService)?.GetType().Name}");
            }
            catch (Exception e)
            {
                _log.Info($"Exception Occurred in MapCallJobWithProcessableServiceBase.ExecuteJob {typeof(TService)?.GetType().Name}");
                OnError(e);
            }
        }

        #endregion

        #region Exposed Methods

        public virtual string GetServiceName()
        {
            return new Regex(@"^I(.+)Proxy$").Replace(_service.GetType().Name, "$1").ToLowerSpaceCase();
        }

        #endregion
    }
}