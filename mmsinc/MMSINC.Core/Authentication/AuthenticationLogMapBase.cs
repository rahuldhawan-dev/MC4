using FluentNHibernate.Mapping;

namespace MMSINC.Authentication
{
    /// <summary>
    /// Base map for all AuthenticationLogs.
    /// </summary>
    public class AuthenticationLogMapBase<TAuthenticationLog, TUser> : ClassMap<TAuthenticationLog>
        where TAuthenticationLog : IAuthenticationLog<TUser>
        where TUser : IAdministratedUser
    {
        #region Properties

        protected virtual string TableName
        {
            get { return "AuthenticationLogs"; }
        }

        #endregion

        protected AuthenticationLogMapBase()
        {
            // ReSharper disable once DoNotCallOverridableMethodsInConstructor
            Table(TableName);
            Id(x => x.Id);
            Map(x => x.AuthCookieHash)
               .Not.Nullable()
               .Unique();
            Map(x => x.IpAddress)
               .Length(50)
               .Not.Nullable();
            Map(x => x.LoggedInAt)
               .Not.Nullable();
            Map(x => x.LoggedOutAt)
               .Nullable();
            Map(x => x.ExpiresAt)
               .Not.Nullable();
            References(x => x.User)
               .Not.Nullable();
        }
    }
}
