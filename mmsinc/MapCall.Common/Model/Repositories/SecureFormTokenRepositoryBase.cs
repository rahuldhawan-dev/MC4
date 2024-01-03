using System;
using System.Linq;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public abstract class SecureFormTokenRepositoryBase<TToken, TValues> : RepositoryBase<TToken>,
        ITokenRepository<TToken, TValues>
        where TToken : class, ISecureFormToken<TToken, TValues>
        where TValues : ISecureFormDynamicValue<TValues, TToken>
    {
        #region Fields

        protected readonly IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Constructors

        public SecureFormTokenRepositoryBase(ISession session, IContainer container, IDateTimeProvider dateTimeProvider)
            : base(session, container)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        #endregion

        #region Exposed Methods

        public TToken FindByToken(Guid tokenId)
        {
            return (from t in Linq where t.Token == tokenId select t).SingleOrDefault();
        }

        public override TToken Save(TToken entity)
        {
            entity.CreatedAt = entity.CreatedAt == DateTime.MinValue
                ? _dateTimeProvider.GetCurrentDate()
                : entity.CreatedAt;
            entity.Token = entity.Token == default(Guid) ? Guid.NewGuid() : entity.Token;

            return base.Save(entity);
        }

        #endregion
    }
}
