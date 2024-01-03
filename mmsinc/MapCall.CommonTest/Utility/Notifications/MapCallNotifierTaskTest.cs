using System.Configuration;
using log4net;
using MapCall.Common.Utility.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Utility.Notifications
{
    [TestClass]
    public class MapCallNotifierTaskTest
    {
        [TestMethod]
        public void TestBaseUrlReturnsBaseUrlFromAppConfig()
        {
            var expected = "https://this.is.a.url";
            var target = new TestMapCallNotifierTask();
            ConfigurationManager.AppSettings[MapCallNotifierTask.BASE_URL_KEY] = expected;

            Assert.AreEqual(expected, target.BaseUrl);
        }

        private class TestMapCallNotifierTask : MapCallNotifierTask
        {
            public TestMapCallNotifierTask() : base(null, null, null) { }

            public TestMapCallNotifierTask(INotifier notifier, INotificationService notificationService, ILog log) :
                base(notifier, notificationService, log) { }

            public override void Run()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
