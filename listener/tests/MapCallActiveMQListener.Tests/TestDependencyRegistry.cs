using MapCall.SAP.Model;
using MapCallActiveMQListener.Ioc;
using MapCallActiveMQListener.Library;
using MMSINC.Data;

namespace MapCallActiveMQListener.Tests
{
    public class TestDependencyRegistry : DependencyRegistryBase
    {
        #region Constructors

        public TestDependencyRegistry()
        {
            For<IUnitOfWorkFactory>().Use<TestUnitOfWorkFactory>();
        }

        #endregion
    }
}