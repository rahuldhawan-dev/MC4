using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;
using System.Linq;
using NHibernate.SqlCommand;

namespace MapCall.Common.Model.Repositories
{
    public interface ISampleSiteRepository : IRepository<SampleSite>
    {
        DetachedEntityCollection<SampleSite> GetByPublicWaterSupply(int id);
    }

    public class SampleSiteRepository : RepositoryBase<SampleSite>, ISampleSiteRepository
    {
        #region Constructor

        public SampleSiteRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Properties

        public override ICriteria Criteria =>
            base.Criteria
                .CreateAlias("Premise", "p", JoinType.LeftOuterJoin)
                .CreateAlias("p.MostRecentService", "mrs", JoinType.LeftOuterJoin)
                .CreateAlias("mrs.Service", "s", JoinType.LeftOuterJoin)
                .CreateAlias("s.ServiceMaterial", "sm", JoinType.LeftOuterJoin)
                .CreateAlias("s.CustomerSideMaterial", "csm", JoinType.LeftOuterJoin);

        #endregion

        #region Public Methods

        public DetachedEntityCollection<SampleSite> GetByPublicWaterSupply(int id)
        {
            var query = Linq.Where(x => x.PublicWaterSupply.Id == id)
                            .Select(ss => new SampleSite {
                                 Id = ss.Id,
                                 TownText = ss.TownText,
                                 Town = ss.Town,
                                 Facility = ss.Facility,
                                 LocationNameDescription = ss.LocationNameDescription,
                                 CommonSiteName = ss.CommonSiteName,
                                 OperatingCenter = ss.OperatingCenter,
                                 PublicWaterSupply = ss.PublicWaterSupply,
                                 County = ss.County,
                                 StreetNumber = ss.StreetNumber,
                                 BactiSite = ss.BactiSite,
                                 Status = ss.Status
                             });

            return new DetachedEntityCollection<SampleSite>(query);
        }

        #endregion
    }

    public static class ISampleSiteRepositoryExtensions
    {
        public static IQueryable<SampleSite> GetByPremiseNumber(this IRepository<SampleSite> that, string premiseNumber)
        {
            return that.Where(x => x.Premise != null && x.Premise.PremiseNumber == premiseNumber);
        }
    }
}
