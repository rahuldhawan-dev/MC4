using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Data;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface ISewerOverflowRepository : IRepository<SewerOverflow>
    {
        IEnumerable<SewerOverflow> FindByStreetId(int streetId);
    }

    public class SewerOverflowRepository : MapCallSecuredRepositoryBase<SewerOverflow>, ISewerOverflowRepository
    {
        public override RoleModules Role
        {
            get { return RoleModules.FieldServicesAssets; }
        }

        public IEnumerable<SewerOverflow> FindByStreetId(int streetId)
        {
            return Linq.Where(x => x.Street.Id == streetId).OrderBy(x => x.Id).ToList();
        }

        public SewerOverflowRepository(ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo) : base(session, container,
            authenticationService, roleRepo) { }
    }
}
