using LINQTo271.Views.RestorationTypeCosts;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _271ObjectTests.Tests.Unit.Views.RestorationTypeCosts
{
    /// <summary>
    /// Summary description for RestorationTypeCostResourceViewPageTest
    /// </summary>
    [TestClass]
    public class RestorationTypeCostResourceViewPageTest
    {
        [TestMethod]
        public void TestConstructorDoesNotThrowException()
        {
            RestorationTypeCostResourceViewPage target;

            MyAssert.DoesNotThrow(
                () => target = new RestorationTypeCostResourceViewPage());
        }
    }
}
