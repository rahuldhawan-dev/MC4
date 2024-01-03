using MapCall.Common.Model.Entities.Users;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;

namespace MapCallImporter.Library.Testing
{
    public class MapCallImporterInMemoryDatabaseTestInterceptorWithChangeTracking
        : InMemoryDatabaseTestInterceptorWithChangeTracking<User>
    {
        public MapCallImporterInMemoryDatabaseTestInterceptorWithChangeTracking(
            InMemoryDatabaseTestInterceptor testInterceptor,
            ChangeTrackingInterceptor<User> changeTrackingInterceptor)
            : base(testInterceptor,
                changeTrackingInterceptor) { }
    }
}
