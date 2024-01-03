using System;
using log4net;
using MapCallScheduler.Library.Common;
using MapCallScheduler.Library.Email;

namespace MapCallScheduler.Jobs
{
    public abstract class MapCallJobWithProcessableServiceAndExtraErrorEmailBase<TService> : MapCallJobWithProcessableServiceBase<TService> 
        where TService : IProcessableService
    {
        #region Abstract Properties

        protected abstract string ExtraEmailAddress { get; }
        protected abstract string ExtraEmailSubject { get; }

        #endregion

        #region Constructors

        public MapCallJobWithProcessableServiceAndExtraErrorEmailBase(ILog log, TService service, IDeveloperEmailer emailer) : base(log, service, emailer) {}

        #endregion

        #region Private Methods

        protected virtual string GetExceptionMessage(Exception e)
        {
            return $"An exception of type {e.GetType()} was encountered with the following message: {e.Message}";
        }

        protected override void OnError(Exception e)
        {
            base.OnError(e);

            _emailer.SendMessage(ExtraEmailAddress, ExtraEmailSubject, GetExceptionMessage(e), false);
        }

        #endregion
    }
}