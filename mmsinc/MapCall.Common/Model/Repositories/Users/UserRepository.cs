using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data.NHibernate;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels.Users;
using NHibernate;
using NHibernate.Criterion;
using StructureMap;

namespace MapCall.Common.Model.Repositories.Users
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        #region Constructor

        /// <summary>
        /// CONSTRUCTOR DOES NOT DANCE!
        /// http://www.youtube.com/watch?v=TfoQj9kVnLU
        /// </summary>
        /// <param name="session"></param>
        public UserRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        public override User Find(int id)
        {
            return base.Find(id);
        }

        public override User Load(int id)
        {
            return base.Load(id);
        }

        #region Methods

        public IEnumerable<User> GetUsersWithRole(int moduleId, int? operatingCenterId, int? actionId,
            bool? userHasAccess)
        {
            const int userAdmin = (int)RoleActions.UserAdministrator;

            var userRoles = GetAggregateRoles(moduleId, operatingCenterId, actionId, userHasAccess, userAdmin);

            return userRoles.Select(x => x.User).OrderBy(x => x.UserName).Distinct();
        }

        public IEnumerable<User> GetUsersFilterByWithAndWithOutOperatingCenter(int moduleId, int? operatingCenterId, int? actionId,
            bool? userHasAccess)
        {
            IEnumerable<User> usersWithOpc = Enumerable.Empty<User>();
            IEnumerable<User> usersWithoutOpc = Enumerable.Empty<User>();
            const int userAdmin = (int)RoleActions.UserAdministrator;

            if (operatingCenterId.HasValue)
            {
                var userRoles = GetAggregateRoles(moduleId, operatingCenterId, actionId, userHasAccess, userAdmin);
                usersWithOpc = userRoles.Where(x => x.OperatingCenter != null && x.OperatingCenter.Id == operatingCenterId.Value).Select(x => x.User).OrderBy(x => x.FullName).Distinct().AsEnumerable();
                usersWithoutOpc = userRoles.Where(x => x.OperatingCenter == null).Select(x => x.User).OrderBy(x => x.FullName).Distinct().AsEnumerable();
            }

            return usersWithOpc.Concat(usersWithoutOpc);
        }

        private IQueryable<AggregateRole> GetAggregateRoles(int moduleId, int? operatingCenterId, int? actionId, bool? userHasAccess,
            int userAdmin)
        {
            var userRoles = Linq.SelectMany(x => x.AggregateRoles).Where(x => x.Module.Id == moduleId);
            if (operatingCenterId.HasValue)
            {
                userRoles =
                    userRoles.Where(x => x.OperatingCenter == null || x.OperatingCenter.Id == operatingCenterId.Value);
            }

            // Any module is considered a match for action if the action we're looking for is RoleActions.Read.
            if (actionId.HasValue && (RoleActions)actionId != RoleActions.Read)
            {
                userRoles = userRoles.Where(x => x.Action.Id == userAdmin || x.Action.Id == actionId);
            }

            if (userHasAccess.HasValue)
            {
                userRoles = userRoles.Where(x => x.User.HasAccess == userHasAccess.Value);
            }

            return userRoles;
        }

        public IEnumerable<User> GetUsersByOperatingCenterId(int operatingCenterId)
        {
            return Linq.Where(x => x.DefaultOperatingCenter.Id == operatingCenterId);
        }
        
        public IEnumerable<User> SearchUsers(ISearchUser search)
        {
            var query = Session.QueryOver<User>();
            Employee emp = null;
            // Not all Users have Employees associated with them.
            query.JoinAlias(x => x.Employee, () => emp, NHibernate.SqlCommand.JoinType.LeftOuterJoin);

            if (!string.IsNullOrWhiteSpace(search.EmployeeId))
            {
                query = query.Where(() => emp.EmployeeId == search.EmployeeId);
            }

            return Search(search, query).ToList();
        }

        public IEnumerable<User> SearchUserTracking(ISearchUserTracking search)
        {
            User user = null;
            var query = Session.QueryOver<User>(() => user);
            OperatingCenter opc = null;
            query.JoinAlias(x => x.DefaultOperatingCenter, () => opc);

            if (search.User.HasValue)
            {
                query.Where(x => x.Id == search.User.Value);
            }

            var notLoggedIn = search.NotLoggedInAt;
            if (notLoggedIn != null && notLoggedIn.IsValid && (notLoggedIn.Start.HasValue && notLoggedIn.End.HasValue))
            {
                AuthenticationLog log = null;

                query.WithSubquery.WhereNotExists(QueryOver.Of<AuthenticationLog>(() => log)
                                                           .Where(l => l.User.Id == user.Id &&
                                                                       notLoggedIn.Start.Value.Date <= log.LoggedInAt &&
                                                                       log.LoggedInAt < notLoggedIn.End.Value)
                                                           .Select(x => x.Id));
            }

            return Search(search, query).ToList();
        }

        #endregion
    }

    public static class UserRepositoryExtensions
    {
        public static User GetUserByUserName(this IRepository<User> that, string userName)
        {
#if DEBUG
            try
            {
#endif
                var user = that.TryGetUserByUserName(userName);

                if (user == null)
                {
                    throw new InvalidOperationException(
                        string.Format("No such user: '{0}'", userName));
                }

                return user;
#if DEBUG
            }
            catch (Exception e)
            {
                throw new Exception(
                    string.Format(
                        "Connection string: {0}{1}Original message:{1}{2}",
                        ConfigurationManager.ConnectionStrings["MCProd"],
                        Environment.NewLine, e.Message));
            }
#endif
        }

        public static User TryGetUserByUserName(this IRepository<User> that, string userName)
        {
            // Some MapCall users really like to hit space before entering their username for some reason.
            userName = userName.Trim();

            // Usernames are unique constrained, so we can use SingleOrDefault here and
            // not worry about an error being thrown. 
            return that.Where(u => u.UserName == userName).SingleOrDefault();
        }

        public static IQueryable<User> FindByOperatingCenterIdAndPartialNameMatch(this IRepository<User> that,
            string partial, int operatingCenterId)
        {
            partial = partial.Trim();
            return that.Where(u =>
                            (u.DefaultOperatingCenter.Id == operatingCenterId ||
                             u.AggregateRoles.Any(r =>
                                 r.Module.Id ==
                                 (int)RoleModules
                                    .FieldServicesWorkManagement &&
                                 (r.OperatingCenter == null ||
                                  r.OperatingCenter.Id ==
                                  operatingCenterId))) &&
                            u.FullName.Contains(partial))
                       .OrderBy(u => u.FullName);
        }
    }

    public interface IUserRepository : IRepository<User>
    {
        #region Methods

        IEnumerable<User> GetUsersWithRole(int moduleId, int? operatingCenterId, int? actionId, bool? userHasAccess);
        IEnumerable<User> GetUsersFilterByWithAndWithOutOperatingCenter(int moduleId, int? operatingCenterId, int? actionId, bool? userHasAccess);
        IEnumerable<User> GetUsersByOperatingCenterId(int operatingCenterId);
        IEnumerable<User> SearchUsers(ISearchUser search);
        IEnumerable<User> SearchUserTracking(ISearchUserTracking search);

        #endregion
    }
}
