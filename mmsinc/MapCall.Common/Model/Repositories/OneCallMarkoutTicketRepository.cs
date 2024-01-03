using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class OneCallMarkoutTicketRepository : RepositoryBase<OneCallMarkoutTicket>, IOneCallMarkoutTicketRepository
    {
        #region Constructors

        public OneCallMarkoutTicketRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Exposed Methods

        public IEnumerable<OneCallMarkoutTicket> GetDistinctTowns(int? operatingCenter, string county)
        {
            return
                from tkt in
                    Linq.GroupBy(t => new {OperatingCenterId = t.OperatingCenter.Id, t.CountyText, t.TownText})
                        .Select(grp => grp.First())
                where
                    (operatingCenter == null || tkt.OperatingCenter.Id == operatingCenter) &&
                    (county == null || tkt.CountyText == county)
                select new OneCallMarkoutTicket {
                    TownText = tkt.TownText
                };
        }

        public IEnumerable<OneCallMarkoutTicket> GetDistinctCDCCodes()
        {
            return
                from tkt in Linq.GroupBy(t => t.CDCCode).Select(grp => grp.First())
                select new OneCallMarkoutTicket {
                    CDCCode = tkt.CDCCode
                };
        }

        public IEnumerable<OneCallMarkoutTicket> GetDistinctCounties(int? operatingCenter)
        {
            return
                from tkt in
                    Linq.GroupBy(t => new {OperatingCenterId = t.OperatingCenter.Id, t.CountyText})
                        .Select(grp => grp.First())
                where operatingCenter == null || tkt.OperatingCenter.Id == operatingCenter
                select new OneCallMarkoutTicket {
                    CountyText = tkt.CountyText
                };
        }

        #endregion
    }

    public interface IOneCallMarkoutTicketRepository : IRepository<OneCallMarkoutTicket>
    {
        #region Abstract Methods

        IEnumerable<OneCallMarkoutTicket> GetDistinctTowns(int? operatingCenter, string county);
        IEnumerable<OneCallMarkoutTicket> GetDistinctCDCCodes();
        IEnumerable<OneCallMarkoutTicket> GetDistinctCounties(int? operatingCenter);

        #endregion
    }
}
