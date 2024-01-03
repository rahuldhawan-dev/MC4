using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface IEmergencyResponsePlanRepository : IRepository<EmergencyResponsePlan> { }

    public class EmergencyResponsePlanRepository : RepositoryBase<EmergencyResponsePlan>,
        IEmergencyResponsePlanRepository
    {
        #region Constructor

        public EmergencyResponsePlanRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion
    }
}
