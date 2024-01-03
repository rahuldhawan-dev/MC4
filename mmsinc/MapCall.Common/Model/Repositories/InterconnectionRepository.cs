using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface IInterconnectionRepository : IRepository<Interconnection>
    {
        #region Methods

        IEnumerable<Interconnection>
            GetInterconnectionsThatHaveContractsExpiringInXDays(int days);

        #endregion
    }

    public class InterconnectionRepository : RepositoryBase<Interconnection>, IInterconnectionRepository
    {
        #region Private Members

        private readonly IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Constructor

        public InterconnectionRepository(ISession session, IContainer container, IDateTimeProvider dateTimeProvider) :
            base(session, container)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        #endregion

        #region Public Methods

        // This method is used for a scheduler task.
        public IEnumerable<Interconnection> GetInterconnectionsThatHaveContractsExpiringInXDays(int days)
        {
            var expDate = _dateTimeProvider.GetCurrentDate().BeginningOfDay().AddDays(days);
            var dayAfterExpDate = expDate.GetNextDay();

            return Linq.Where(x =>
                x.ContractEndDate.HasValue &&
                x.ContractEndDate >= expDate &&
                x.ContractEndDate < dayAfterExpDate).ToList();
        }

        #endregion
    }
}
