using System;
using System.IO;
using System.Text;
using FluentFTP;
using log4net;
using MapCallScheduler.JobHelpers.SpaceTimeInsight;
using MapCallScheduler.Library.Configuration;
using MapCallScheduler.Library.Ftp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.SpaceTimeInsight
{
    [TestClass]
    public class SpaceTimeInsightFileUploadServiceTest
    {
        #region Private Members

        private SpaceTimeInsightFileUploadService _target;
        private Mock<ISpaceTimeInsightFileDumpServiceConfiguration> _config;
        private Mock<IFtpClientFactory> _ftpFactory;
        private Mock<IFtpConfigSection> _ftpConfig;
        private Mock<IFtpClient> _ftpClient;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private readonly string _workingDirectory = "foo/bar";
        private DateTime _now = DateTime.Now;
        private IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();

            _container.Inject(new Mock<ILog>().Object);

            _ftpConfig = new Mock<IFtpConfigSection>();
            _ftpConfig.SetupGet(f => f.WorkingDirectory).Returns(_workingDirectory);
            _ftpClient = new Mock<IFtpClient>();
            _container.Inject((_ftpFactory = new Mock<IFtpClientFactory>()).Object);
            _ftpFactory.Setup(f => f.FromConfig(_ftpConfig.Object)).Returns(_ftpClient.Object);

            _container.Inject((_config = new Mock<ISpaceTimeInsightFileDumpServiceConfiguration>()).Object);
            _config.SetupGet(c => c.FtpConfig).Returns(_ftpConfig.Object);

            _container.Inject((_dateTimeProvider = new Mock<IDateTimeProvider>()).Object);
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(_now);

            _target = _container.GetInstance<SpaceTimeInsightFileUploadService>();
        }

        #endregion

        private void TestEncryptAndUpload(string type, Action<SpaceTimeInsightFileUploadService, string> doTest)
        {
            var decrypted = "foo";
            var fileName = $"{type}-{_now.SecondsSinceEpoch()}.json";
            var stream = new IndesposableMemoryStream();

            _ftpClient.Setup(x => x.OpenWrite($"{_workingDirectory}/{fileName}")).Returns(stream);

            doTest(_target, decrypted);

            MyAssert.AreEqual(Encoding.ASCII.GetBytes(decrypted), stream.ToArray());
            Assert.IsTrue(stream.Disposed);
            _ftpClient.Verify(c => c.Dispose());

            stream.ActuallyDispose();
        }

        [TestMethod]
        public void TestUploadHydrantInspectionsEncryptsFileAndUploadsResult()
        {
            TestEncryptAndUpload("hydrant-inspection", (t, d) => t.UploadHydrantInspections(d));
        }

        [TestMethod]
        public void TestUploadMainBreaksEncryptsFileAndUploadsResult()
        {
            TestEncryptAndUpload("mainbreak", (t, d) => t.UploadMainBreaks(d));
        }

        [TestMethod]
        public void TestUploadWorkOrdersEncryptsFileAndUploadsResult()
        {
            TestEncryptAndUpload("workorder", (t, d) => t.UploadWorkOrders(d));
        }

        [TestMethod]
        public void TestUploadTankLevelDataEncryptsFileAndUploadsResult()
        {
            TestEncryptAndUpload("tank-level", (t, d) => t.UploadTankLevelData(d));
        }

        [TestMethod]
        public void TestUploadInterconnectDataEncryptsFileAndUploadsResult()
        {
            TestEncryptAndUpload("interconnect", (t, d) => t.UploadInterconnectData(d));
        }
    }

    public class IndesposableMemoryStream : MemoryStream
    {
        #region Properties

        public bool Disposed { get; private set; }

        #endregion

        #region Private Methods

        protected override void Dispose(bool disposing)
        {
            Disposed = true;
        }

        #endregion

        #region Exposed Methods

        public void ActuallyDispose()
        {
            base.Dispose(true);
        }

        #endregion
    }
}