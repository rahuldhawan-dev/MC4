using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MMSINC.Authentication
{
    public interface IAuthenticationLogRepository<TAuthenticationLog, TUser> : IRepository<TAuthenticationLog>
        where TAuthenticationLog : class, IAuthenticationLog<TUser>
        where TUser : IAdministratedUser
    {
        /// <summary>
        /// Returns a matching AuthenticationLog for a cookie if the LoggedOutAt
        /// value is null. 
        /// </summary>
        TAuthenticationLog FindActiveLogByCookie(string cookie);

        /// <summary>
        /// Use this to save new instances of AuthenticationLogs.
        /// </summary>
        /// <param name="log"></param>
        /// <param name="cookie"></param>
        void Save(TAuthenticationLog log, string cookie);
    }

    public class AuthenticationLogRepositoryBase<TAuthenticationLog, TUser> : RepositoryBase<TAuthenticationLog>,
        IAuthenticationLogRepository<TAuthenticationLog, TUser>
        where TAuthenticationLog : class, IAuthenticationLog<TUser>
        where TUser : IAdministratedUser
    {
        #region Constructor

        public AuthenticationLogRepositoryBase(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Private Methods

        /// <summary>
        /// Hashes a cookie into a predictable guid for database storage. 
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        private static Guid HashCookie(string cookie)
        {
            if (string.IsNullOrWhiteSpace(cookie))
            {
                throw new InvalidOperationException("The cookie being hashed can not be null or empty or whitespace.");
            }

            var bytes = Encoding.UTF8.GetBytes(cookie);
            using (var hasher = new MD5CryptoServiceProvider())
            {
                return new Guid(hasher.ComputeHash(bytes));
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a matching AuthenticationLog for a cookie if the LoggedOutAt
        /// value is null. 
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public TAuthenticationLog FindActiveLogByCookie(string cookie)
        {
            var hashed = HashCookie(cookie);
            // This does not check for expiration, that logic is handled in AuthenticationServiceBase.
            return Linq.SingleOrDefault(x => x.AuthCookieHash == hashed && x.LoggedOutAt == null);
        }

        /// <summary>
        /// Saves a NEW AuthenticationLog instance.
        /// </summary>
        /// <param name="log"></param>
        /// <param name="cookie"></param>
        public void Save(TAuthenticationLog log, string cookie)
        {
            log.AuthCookieHash = HashCookie(cookie);
            Save(log);
        }

        public override TAuthenticationLog Save(TAuthenticationLog entity)
        {
            if (entity.AuthCookieHash == Guid.Empty)
            {
                throw new InvalidOperationException(
                    "AuthCookieHash was not set. Use the Save overload that accepts a cookie string to ensure this gets set correctly.");
            }

            return base.Save(entity);
        }

        public sealed override void Delete(TAuthenticationLog entity)
        {
            // MapCall will be using this table to keep an authentication history. Deleting should not occur.
            throw new NotSupportedException("Deleting authentication logs is not allowed.");
        }

        #endregion
    }
}
