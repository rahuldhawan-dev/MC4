using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCallScheduler.Tests
{
    public class MapCallSchedulerInMemoryDatabaseTest<TEntity> : InMemoryDatabaseTest<TEntity>
    {
        protected override ITestDataFactoryService CreateFactoryService()
        {
            return new TestDataFactoryService(_container, typeof(OperatingCenterFactory).Assembly);
        }
    }

    public class MapCallSchedulerInMemoryDatabaseTest<TEntity, TRepository> : InMemoryDatabaseTest<TEntity, TRepository>
        where TRepository : class, IRepository<TEntity>
        where TEntity : class
    {
        protected override ITestDataFactoryService CreateFactoryService()
        {
            return new TestDataFactoryService(_container, typeof(OperatingCenterFactory).Assembly);
        }
    }
}
