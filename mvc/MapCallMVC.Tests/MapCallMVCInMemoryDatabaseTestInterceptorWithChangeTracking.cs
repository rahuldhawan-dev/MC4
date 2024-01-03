using MapCall.Common.Model.Entities.Users;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;

namespace MapCallMVC.Tests
{
    /// <inheritdoc />
    public class MapCallMVCInMemoryDatabaseTestInterceptorWithChangeTracking
        : InMemoryDatabaseTestInterceptorWithChangeTracking<User>
    {
        public MapCallMVCInMemoryDatabaseTestInterceptorWithChangeTracking(
            InMemoryDatabaseTestInterceptor testInterceptor,
            ChangeTrackingInterceptor<User> changeTrackingInterceptor)
            : base(testInterceptor, changeTrackingInterceptor) { }
    }
}
