using MMSINC.Data.V2;
using MMSINC.Testing.NHibernate.V2;

namespace MapCallImporter.Library.TypeRegistration
{
    public class TestStructureMapRegistry : StructureMapRegistryBase
    {
        #region Constructors

        public TestStructureMapRegistry()
        {
            For<IUnitOfWorkFactory>().Use<TestUnitOfWorkFactory>();
        }

        #endregion
    }
}
