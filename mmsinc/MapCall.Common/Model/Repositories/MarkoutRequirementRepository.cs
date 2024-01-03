using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface IMarkoutRequirementRepository : IRepository<MarkoutRequirement>
    {
        MarkoutRequirement GetEmergencyMarkoutRequirement();
    }

    public class MarkoutRequirementRepository : RepositoryBase<MarkoutRequirement>, IMarkoutRequirementRepository
    {
        #region Constructor

        public MarkoutRequirementRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Public Methods

        public MarkoutRequirement GetEmergencyMarkoutRequirement()
        {
            return Linq.Single(x => x.Description == "Emergency");
        }

        #endregion
    }
}
