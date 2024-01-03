using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using MMSINC.Authentication;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface IMapCallAuthenticationLogRepository : IAuthenticationLogRepository<AuthenticationLog, User>
    {
        IEnumerable<AuthenticationLog> SearchAuthenticationLogs(ISearchAuthenticationLog search);
    }

    public class MapCallAuthenticationLogRepository : AuthenticationLogRepositoryBase<AuthenticationLog, User>,
        IMapCallAuthenticationLogRepository
    {
        #region Constructor

        public MapCallAuthenticationLogRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Public Methods

        public IEnumerable<AuthenticationLog> SearchAuthenticationLogs(ISearchAuthenticationLog search)
        {
            var query = Session.QueryOver<AuthenticationLog>();
            User user = null;
            query.JoinAlias(x => x.User, () => user);
            OperatingCenter opc = null;
            query.JoinAlias(() => user.DefaultOperatingCenter, () => opc);

            return Search(search, query);
        }

        #endregion
    }
}
