using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MMSINC.Authentication
{
    public interface IAuthenticationLog<TUser> : IEntity
        where TUser : IAdministratedUser
    {
        int Id { get; }
        TUser User { get; set; }
        string IpAddress { get; set; }
        DateTime LoggedInAt { get; set; }
        DateTime? LoggedOutAt { get; set; }
        Guid AuthCookieHash { get; set; }
        DateTime ExpiresAt { get; set; }
    }

    [Serializable]
    public abstract class AuthenticationLogBase<TUser> : IAuthenticationLog<TUser> where TUser : IAdministratedUser
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual TUser User { get; set; }

        [StringLength(50)]
        public virtual string IpAddress { get; set; }

        public virtual DateTime LoggedInAt { get; set; }
        public virtual DateTime? LoggedOutAt { get; set; }

        /// <summary>
        /// Gets/sets the guid hash for the actual authentication cookie.
        /// </summary>
        /// <remarks>
        /// This is an MD5 hash so we're not storing users usable authentication
        /// cookie in the database. Also the authentication cookie itself is of 
        /// varying size. This gives us a predictable size in the db and should also
        /// be much faster querying.
        /// 
        /// Setter is internal/protected to try to enforce that only the repository
        /// should deal with setting this value.
        ///  </remarks>
        public virtual Guid AuthCookieHash { get; set; }

        public virtual DateTime ExpiresAt { get; set; }

        #endregion
    }
}
