using Microsoft.VisualStudio.TestTools.UnitTesting;
using log4net.Config;

namespace Contractors.Tests
{
    [TestClass]
    public class TestConfiguration
    {
        [AssemblyInitialize]
        public static void ContractorTestsAssemblyInitialize(TestContext context)
        {
            XmlConfigurator.Configure();
        }
    }
}
