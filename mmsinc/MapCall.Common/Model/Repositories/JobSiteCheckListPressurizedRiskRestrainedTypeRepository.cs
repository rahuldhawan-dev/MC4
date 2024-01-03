using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface
        IJobSiteCheckListPressurizedRiskRestrainedTypeRepository : IRepository<
            JobSiteCheckListPressurizedRiskRestrainedType>
    {
        JobSiteCheckListPressurizedRiskRestrainedType GetNo();
        JobSiteCheckListPressurizedRiskRestrainedType GetYes();
    }

    public class JobSiteCheckListPressurizedRiskRestrainedTypeRepository :
        RepositoryBase<JobSiteCheckListPressurizedRiskRestrainedType>,
        IJobSiteCheckListPressurizedRiskRestrainedTypeRepository
    {
        #region Constructors

        public JobSiteCheckListPressurizedRiskRestrainedTypeRepository(ISession session, IContainer container) : base(
            session, container) { }

        #endregion

        #region Public Methods

        public JobSiteCheckListPressurizedRiskRestrainedType GetNo()
        {
            return Linq.Single(x => x.Id == JobSiteCheckListPressurizedRiskRestrainedType.Indices.NO);
        }

        public JobSiteCheckListPressurizedRiskRestrainedType GetYes()
        {
            return Linq.Single(x => x.Id == JobSiteCheckListPressurizedRiskRestrainedType.Indices.YES);
        }

        #endregion
    }
}
