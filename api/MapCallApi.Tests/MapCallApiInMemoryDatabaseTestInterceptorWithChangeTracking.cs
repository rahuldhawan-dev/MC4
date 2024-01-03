using MapCall.Common.Model.Entities.Users;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;

namespace MapCallApi.Tests
{
    public class MapCallApiInMemoryDatabaseTestInterceptorWithChangeTracking
        : InMemoryDatabaseTestInterceptorWithChangeTracking<User>
    {
        public MapCallApiInMemoryDatabaseTestInterceptorWithChangeTracking(
            InMemoryDatabaseTestInterceptor testInterceptor,
            ChangeTrackingInterceptor<User> changeTrackingInterceptor)
            : base(testInterceptor, changeTrackingInterceptor) { }
    }
}
