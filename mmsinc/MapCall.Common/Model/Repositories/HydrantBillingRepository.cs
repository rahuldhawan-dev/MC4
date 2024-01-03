using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface IHydrantBillingRepository : IRepository<HydrantBilling>
    {
        HydrantBilling GetPublicHydrantBilling();
    }

    public class HydrantBillingRepository : RepositoryBase<HydrantBilling>, IHydrantBillingRepository
    {
        #region Constructor

        public HydrantBillingRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Public Methods

        public HydrantBilling GetPublicHydrantBilling()
        {
            return Find(HydrantBilling.Indices.PUBLIC);
        }

        #endregion
    }
}
