using System.Collections.Generic;
using MMSINC.Data.NHibernate;
using MapCall.Common.Model.Entities.Customers;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories.Customers
{
    public class H2OSurveyRepository : RepositoryBase<H2OSurvey>, IH2OSurveyRepository
    {
        public H2OSurveyRepository(ISession session, IContainer container) : base(session, container) { }

        public IEnumerable<H2OSurvey> GetAllSurveys()
        {
            return this.Criteria.List<H2OSurvey>();
        }
    }

    public interface IH2OSurveyRepository
    {
        IEnumerable<H2OSurvey> GetAllSurveys();
    }
}
