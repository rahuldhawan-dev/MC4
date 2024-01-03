using MapCall.Common.Data;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface ISewerOpeningConnectionRepository : IRepository<SewerOpeningConnection> { }

    public class SewerOpeningConnectionRepository : MapCallSecuredRepositoryBase<SewerOpeningConnection>,
        ISewerOpeningConnectionRepository
    {
        #region Properties

        public override RoleModules Role
        {
            get { return RoleModules.FieldServicesAssets; }
        }

        #endregion

        public SewerOpeningConnectionRepository(ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo) : base(session, container,
            authenticationService, roleRepo) { }
    }
}
