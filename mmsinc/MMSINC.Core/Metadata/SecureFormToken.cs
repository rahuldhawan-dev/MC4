using System;
using System.Collections.Generic;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MMSINC.Metadata
{
    public abstract class SecureFormTokenBase<TThis, TValues> : ISecureFormToken<TThis, TValues>
        where TThis : ISecureFormToken<TThis, TValues>
        where TValues : ISecureFormDynamicValue<TValues, TThis>
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual int UserId { get; set; }
        public virtual Guid Token { get; set; }
        public virtual string Area { get; set; }
        public virtual string Controller { get; set; }
        public virtual string Action { get; set; }
        public virtual IList<TValues> DynamicValues { get; set; }
        public virtual DateTime CreatedAt { get; set; }

        #endregion

        #region Abstract Properties

        public abstract int ExpirationMinutes { get; }

        #endregion

        #region Constructors

        public SecureFormTokenBase()
        {
            DynamicValues = new List<TValues>();
        }

        #endregion

        #region Exposed Methods

        public virtual bool IsExpired(IDateTimeProvider dateTimeProvider)
        {
            return (dateTimeProvider.GetCurrentDate() - CreatedAt).TotalMinutes >= ExpirationMinutes;
        }

        #endregion
    }

    public class SecureFormToken : SecureFormTokenBase<SecureFormToken, SecureFormDynamicValue>
    {
        #region Constants

        public const int EXPIRATION_MINUTES = 60;

        #endregion

        #region Properties

        public override int ExpirationMinutes => EXPIRATION_MINUTES;

        #endregion
    }

    public interface ISecureFormToken<TThis, TValues> : IEntity
        where TThis : ISecureFormToken<TThis, TValues>
        where TValues : ISecureFormDynamicValue<TValues, TThis>
    {
        #region Abstract Properties

        int UserId { get; set; }
        Guid Token { get; set; }
        string Area { get; set; }
        string Controller { get; set; }
        string Action { get; set; }
        IList<TValues> DynamicValues { get; set; }
        DateTime CreatedAt { get; set; }

        #endregion

        #region Abstract Methods

        bool IsExpired(IDateTimeProvider dateTimeProvider);

        #endregion
    }

    public interface ITokenRepository<TToken, TValue> : IRepository<TToken>
        where TToken : ISecureFormToken<TToken, TValue>
        where TValue : ISecureFormDynamicValue<TValue, TToken>
    {
        #region Abstract Methods

        TToken FindByToken(Guid tokenId);

        #endregion
    }
}
