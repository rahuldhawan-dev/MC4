using System.Net;
using NUnit.Framework;

namespace FunctionalTests 
{
    [TestFixture]
    public class SantityTest : BaseApiTestClass
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [Test]
        public void TestSiteIsRunning()
        {
            TestAsyncResponse("new.html", response => {
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            });
        }
    }
}
