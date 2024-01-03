using log4net;
using MapCallScheduler.Jobs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.NameValueCollectionExtensions;
using MMSINC.Interface;
using MMSINC.Utilities;
using Moq;
using StructureMap;
using System;
using System.Configuration;
using System.Linq;
using MMSINC.Common;

namespace MapCallScheduler.Tests.Jobs
{
    [TestClass]
    public class HeartbeatJobTest
    {
        #region Private Members

        private Mock<ILog> _log;
        private Mock<ISmtpClient> _smtpClient;
        private Mock<ISmtpClientFactory> _smtpClientFactory;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private IContainer _container;
        private HeartbeatJob _target;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _container.Inject((_log = new Mock<ILog>()).Object);
            _container.Inject((_smtpClientFactory = new Mock<ISmtpClientFactory>()).Object);
            _container.Inject((_dateTimeProvider = new Mock<IDateTimeProvider>()).Object);
            _smtpClient = new Mock<ISmtpClient>();
            _smtpClientFactory.Setup(x => x.Build()).Returns(_smtpClient.Object);
            _target = _container.GetInstance<HeartbeatJob>();
        }

        #endregion

        [TestMethod]
        public void TestExecuteSendsOutEmail()
        {
            var now = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(now);
            _smtpClient.Setup(
                x => x.Send(
                    It.Is<IMailMessage>(
                        m =>
                            m.From.Address == ConfigurationManager.AppSettings.EnsureValue("noreply_address") &&
                            m.To.First().Address == HeartbeatJob.TO_ADDRESS &&
                            m.Subject ==
                            String.Format(HeartbeatJob.SUBJECT_FORMAT, AppDomain.CurrentDomain.FriendlyName) &&
                            m.Body == String.Format(HeartbeatJob.BODY_FORMAT, now))));

            _target.Execute(null);

            _smtpClient.VerifyAll();
            _dateTimeProvider.VerifyAll();
        }
    }
}