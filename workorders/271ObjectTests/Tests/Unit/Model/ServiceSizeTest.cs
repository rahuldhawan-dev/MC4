using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for ServiceSizeTest
    /// </summary>
    [TestClass]
    public class ServiceSizeTest
    {
        [TestMethod]
        public void TestToStringMethodReturnsSizeServ()
        {
            var sizeServ = "this is size serv";

            var target = new ServiceSize {
                ServiceSizeDescription = sizeServ
            };

            Assert.AreEqual(sizeServ, target.ToString());
        }
    }
}
