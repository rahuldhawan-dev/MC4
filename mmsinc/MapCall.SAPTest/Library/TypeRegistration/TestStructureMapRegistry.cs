using MMSINC.Data;
using MMSINC.Testing.NHibernate;
using StructureMap;

namespace MapCall.SAPTest.Library.TypeRegistration
{
    public class TestStructureMapRegistry : Registry
    {
        #region Constructors

        public TestStructureMapRegistry()
        {
            For<IUnitOfWorkFactory>().Use<TestUnitOfWorkFactory>();
        }

        #endregion
    }
}
