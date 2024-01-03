using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface IRecurringFrequencyUnitRepository : IRepository<RecurringFrequencyUnit>
    {
        RecurringFrequencyUnit GetYear();
    }

    public class RecurringFrequencyUnitRepository : RepositoryBase<RecurringFrequencyUnit>,
        IRecurringFrequencyUnitRepository
    {
        #region Constructor

        public RecurringFrequencyUnitRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the Year unit.
        /// </summary>
        /// <returns></returns>
        public RecurringFrequencyUnit GetYear()
        {
            return Linq.Single(x => x.Description == "Year");
        }

        #endregion
    }
}
