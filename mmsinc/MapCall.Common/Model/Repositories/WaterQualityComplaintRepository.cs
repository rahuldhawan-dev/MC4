using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Transform;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class WaterQualityComplaintRepository : RepositoryBase<WaterQualityComplaint>,
        IWaterQualityComplaintRepository
    {
        public WaterQualityComplaintRepository(ISession session, IContainer container) : base(session, container) { }

        public IEnumerable<WaterQualityComplaintByStateForYearReportItem> GetByStateForYearReport(
            ISearchWaterQualityComplaintByStateForYear search)
        {
            return Session.CreateSQLQuery("exec rptWQComplaintsByStateForYear ?, ?")
                          .SetInt32(0, search.Year)
                          .SetString(1, search.State)
                          .SetResultTransformer(
                               Transformers.AliasToBean<WaterQualityComplaintByStateForYearReportItem>())
                          .List<WaterQualityComplaintByStateForYearReportItem>();
        }
    }

    public interface IWaterQualityComplaintRepository : IRepository<WaterQualityComplaint>
    {
        IEnumerable<WaterQualityComplaintByStateForYearReportItem> GetByStateForYearReport(
            ISearchWaterQualityComplaintByStateForYear search);
    }
}
