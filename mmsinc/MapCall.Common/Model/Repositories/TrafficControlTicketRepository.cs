using MapCall.Common.Data;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class TrafficControlTicketRepository : MapCallSecuredRepositoryBase<TrafficControlTicket>,
        ITrafficControlTicketRepository
    {
        public override RoleModules Role => RoleModules.FieldServicesWorkManagement;

        public TrafficControlTicketRepository(ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo) : base(session, container,
            authenticationService, roleRepo) { }
    }

    public interface ITrafficControlTicketRepository : IRepository<TrafficControlTicket> { }
}
