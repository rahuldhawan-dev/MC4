using System.Linq;

using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using StructureMap;

namespace Contractors.Data.Models.Repositories
{
    public class ServiceInstallationRepository : SecuredRepositoryBase<ServiceInstallation, ContractorUser>, IServiceInstallationRepository
    {
        #region Properties

        #region Base Linq/Criteria

        public override IQueryable<ServiceInstallation> Linq
        {
            get { return base.Linq.Where(wo => (wo.WorkOrder.AssignedContractor == CurrentUser.Contractor)); }
        }

        public override ICriteria Criteria
        {
            get
            {
                return base.Criteria
                    .CreateAlias("WorkOrder", "o")
                    .CreateAlias("o.AssignedContractor", "c")
                    .Add(Restrictions.Eq("c.Id", CurrentUser.Contractor.Id));
            }
        }

        #endregion

        #endregion

        #region Constructors

        public ServiceInstallationRepository(ISession session, IAuthenticationService<ContractorUser> authenticationService, IContainer container) : base(session, authenticationService, container) { }

        #endregion
    }
}