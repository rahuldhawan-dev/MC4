using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class PlanningPlantRepository : RepositoryBase<PlanningPlant>, IPlanningPlantRepository
    {
        #region Constructors

        public PlanningPlantRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Exposed Methods

        public PlanningPlant GetByCode(string code)
        {
            return Linq.SingleOrDefault(p => p.Code == code);
        }

        #endregion
    }

    public interface IPlanningPlantRepository : IRepository<PlanningPlant>
    {
        #region Abstract Methods

        PlanningPlant GetByCode(string code);

        #endregion
    }
}
