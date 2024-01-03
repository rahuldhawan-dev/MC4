using System.Collections.Generic;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.NHibernate;
using NHibernate;
using NHibernate.SqlCommand;
using NHibernate.Type;

namespace MMSINC.Testing
{
    /// <summary>
    /// Composite interceptor comprising functionality from both
    /// <see cref="InMemoryDatabaseTestInterceptor"/> and <see cref="ChangeTrackingInterceptor{TUser}"/>.
    /// </summary>
    public class InMemoryDatabaseTestInterceptorWithChangeTracking<TUser>
        : EmptyInterceptor, IInMemoryDatabaseTestInterceptor, IChangeTrackingInterceptor<TUser>
        where TUser : class, IAdministratedUser
    {
        private readonly InMemoryDatabaseTestInterceptor _testInterceptor;
        private readonly ChangeTrackingInterceptor<TUser> _changeTrackingInterceptor;

        public InMemoryDatabaseTestInterceptorWithChangeTracking(
            InMemoryDatabaseTestInterceptor testInterceptor,
            ChangeTrackingInterceptor<TUser> changeTrackingInterceptor)
        {
            _testInterceptor = testInterceptor;
            _changeTrackingInterceptor = changeTrackingInterceptor;
        }

        public override SqlString OnPrepareStatement(SqlString sql)
        {
            return _testInterceptor.OnPrepareStatement(sql);
        }

        public override bool OnSave(
            object entity,
            object id,
            object[] state,
            string[] propertyNames,
            IType[] types)
        {
            return _changeTrackingInterceptor.OnSave(entity, id, state, propertyNames, types);
        }

        public override bool OnFlushDirty(
            object entity,
            object id,
            object[] currentState,
            object[] previousState,
            string[] propertyNames,
            IType[] types)
        {
            return _changeTrackingInterceptor.OnFlushDirty(entity, id, currentState, previousState, propertyNames, types);
        }

        public void Init()
        {
            _testInterceptor.Init();
        }

        public void Reset()
        {
            _testInterceptor.Reset();
        }

        public List<SqlString> PreparedStatements => _testInterceptor.PreparedStatements;
    }
}
