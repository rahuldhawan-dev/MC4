using LINQTo271.Views.Crews;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _271ObjectTests.Tests.Unit.Views.Crews
{
    /// <summary>
    /// Summary description for CrewResourceViewPageTest
    /// </summary>
    [TestClass]
    public class CrewResourceViewPageTest
    {
        [TestMethod]
        public void TestConstruction()
        {
            MyAssert.DoesNotThrow(() => new CrewResourceViewPage());
        }
    }
}
