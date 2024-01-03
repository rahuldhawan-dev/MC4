
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using NHibernate;
using StructureMap;

namespace Contractors.Data.Models.Repositories
{
    public class AuthenticationLogRepository : AuthenticationLogRepositoryBase<ContractorsAuthenticationLog, ContractorUser>
    {
        public AuthenticationLogRepository(ISession session, IContainer container) : base(session, container) { }
    }
}
