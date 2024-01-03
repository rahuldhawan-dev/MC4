using MapCall.Common.Testing.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapCallKafkaConsumer.Core.UnitTests.Testing
{
    public class MapCallKafkaConsumerInMemoryDatabaseTest<TEntity> : InMemoryDatabaseTest<TEntity>
    {
        protected override ITestDataFactoryService CreateFactoryService()
        {
            return new TestDataFactoryService(_container, typeof(OperatingCenterFactory).Assembly);
        }
    }

    public class MapCallKafkaConsumerInMemoryDatabaseTest<TEntity, TRepository> : InMemoryDatabaseTest<TEntity, TRepository>
        where TRepository : class, IRepository<TEntity>
        where TEntity : class
    {
        protected override ITestDataFactoryService CreateFactoryService()
        {
            return new TestDataFactoryService(_container, typeof(OperatingCenterFactory).Assembly);
        }
    }
}
