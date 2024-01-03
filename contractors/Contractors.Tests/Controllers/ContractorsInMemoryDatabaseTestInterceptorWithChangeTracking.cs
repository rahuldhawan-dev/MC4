using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;

namespace Contractors.Tests.Controllers
{
    /// <inheritdoc />
    public class ContractorsInMemoryDatabaseTestInterceptorWithChangeTracking
        : InMemoryDatabaseTestInterceptorWithChangeTracking<ContractorUser>
    {
        public ContractorsInMemoryDatabaseTestInterceptorWithChangeTracking(
            InMemoryDatabaseTestInterceptor testInterceptor,
            ChangeTrackingInterceptor<ContractorUser> changeTrackingInterceptor)
            : base(testInterceptor, changeTrackingInterceptor) { }
    }
}
