using log4net;
using MapCallScheduler.Library.Common;
using MapCallScheduler.Library.Email;

namespace MapCallScheduler.Jobs
{
    public abstract class MapCallJobWithProcessableServiceAndAwwErrorEmailBase<TService> : MapCallJobWithProcessableServiceAndExtraErrorEmailBase<TService>
        where TService : IProcessableService
    {
        #region Constants

        public const string AMWATER_EMAIL = "mapcall@amwater.com";

        #endregion

        #region Properties

        protected override string ExtraEmailAddress => AMWATER_EMAIL;

        #endregion

        #region Constructors

        public MapCallJobWithProcessableServiceAndAwwErrorEmailBase(ILog log, TService service, IDeveloperEmailer emailer) : base(log, service, emailer)
        {
        }

        #endregion
    }
}