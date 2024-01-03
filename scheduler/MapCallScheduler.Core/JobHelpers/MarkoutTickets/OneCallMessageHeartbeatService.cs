using System;
using MapCall.Common.Model.Entities;
using MapCallScheduler.Library;
using MapCallScheduler.Library.Common;
using MapCallScheduler.Library.Email;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallScheduler.JobHelpers.MarkoutTickets
{
    public class OneCallMessageHeartbeatService : IOneCallMessageHeartbeatService
    {
        #region Constants

        public const int DEFAULT_RECENT_INTERVAL = -240;
        public const string NO_RECENT_MESSAGE_SUBJECT = "No tickets processed in the past 240 minutes",
            NO_RECENT_MESSAGE_MESSAGE_FORMAT = "As of {0} no new tickets have been entered.";

        #endregion

        #region Private Members

        private readonly IDeveloperEmailer _emailer;
        private readonly IMarkoutTicketServiceConfiguration _config;
        private readonly IRepository<OneCallMarkoutTicket> _repository;
        private readonly IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Constructors

        public OneCallMessageHeartbeatService(IDeveloperEmailer emailer, IMarkoutTicketServiceConfiguration config, IDateTimeProvider dateTimeProvider, IRepository<OneCallMarkoutTicket> repository)
        {
            _emailer = emailer;
            _config = config;
            _dateTimeProvider = dateTimeProvider;
            _repository = repository;
        }

        #endregion

        #region Exposed Methods

        public void Process()
        {
            var now = _dateTimeProvider.GetCurrentDate();
            var recent = now.AddMinutes(_config.IncomingEmailConfig.GapInterval ?? DEFAULT_RECENT_INTERVAL);

            if (!_repository.Where(t => t.DateReceived > recent).Any())
            {
                _emailer.SendMessage(NO_RECENT_MESSAGE_SUBJECT, String.Format(NO_RECENT_MESSAGE_MESSAGE_FORMAT, now), false);
            }
        }

        #endregion
    }

    public interface IOneCallMessageHeartbeatService : IProcessableService { }
}