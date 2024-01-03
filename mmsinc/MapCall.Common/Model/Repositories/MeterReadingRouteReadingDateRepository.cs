using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class MeterReadingRouteReadingDateRepository : RepositoryBase<MeterReadingRouteReadingDate>,
        IMeterReadingRouteReadingDateRepository
    {
        #region Constructors

        public MeterReadingRouteReadingDateRepository(ISession session, IContainer container) :
            base(session, container) { }

        #endregion

        #region Exposed Methods

        public void Evict(MeterReadingRouteReadingDate obj)
        {
            Session.Evict(obj);
        }

        #endregion
    }

    public interface IMeterReadingRouteReadingDateRepository : IRepository<MeterReadingRouteReadingDate>
    {
        #region Abstract Methods

        void Evict(MeterReadingRouteReadingDate obj);

        #endregion
    }
}
