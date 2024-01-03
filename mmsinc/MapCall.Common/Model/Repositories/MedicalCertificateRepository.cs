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
    public class MedicalCertificateRepository : MapCallEmployeeSecuredRepositoryBase<MedicalCertificate>,
        IMedicalCertificateRepository
    {
        #region Properties

        public override RoleModules Role => RoleModules.HumanResourcesEmployeeLimited;

        #endregion

        public MedicalCertificateRepository(ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo) : base(session, container,
            authenticationService, roleRepo) { }
    }

    public interface IMedicalCertificateRepository : IRepository<MedicalCertificate> { }
}
