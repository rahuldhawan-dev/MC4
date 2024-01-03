using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Data;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class ViolationCertificateRepository : MapCallEmployeeSecuredRepositoryBase<ViolationCertificate>,
        IViolationCertificateRepository
    {
        #region Properties

        public override RoleModules Role => RoleModules.HumanResourcesEmployeeLimited;

        #endregion

        #region Constructors

        public ViolationCertificateRepository(IRepository<AggregateRole> roleRepo, ISession session, IContainer container,
            IAuthenticationService<User> authenticationService) : base(session, container, authenticationService,
            roleRepo) { }

        #endregion
    }

    public interface IViolationCertificateRepository : IRepository<ViolationCertificate> { }
}
