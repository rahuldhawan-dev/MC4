using System;
using System.IO;
using log4net;
using MapCallScheduler.JobHelpers.SystemDeliveryEntry.DumpTasks;
using MapCallScheduler.Library.Configuration;
using MapCallScheduler.Library.Filesystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.SystemDeliveryEntry
{
    [TestClass]
    public class SystemDeliveryEntryFileUploaderTest
    {
        #region Private Members

        private IContainer _container;
        private SystemDeliveryEntryFileUploader _target;
        private Mock<ISystemDeliveryEntryFileDumpServiceConfiguration> _serviceConfig;
        private Mock<IFileConfigSection> _config;
        private Mock<IFileClient> _fileClient;
        private TestDateTimeProvider _dateTimeProvider;
        private DateTime _now;

        #endregion

        #region Private Methods

        private void InitializeContainer(ConfigurationExpression e)
        {
            _serviceConfig = e.For<ISystemDeliveryEntryFileDumpServiceConfiguration>().Mock();
            _config = e.For<IFileConfigSection>().Mock();
            _serviceConfig.Setup(x => x.FileConfig).Returns(_config.Object);
            _fileClient = e.For<IFileClient>().Mock();
            e.For<IDateTimeProvider>().Use(_dateTimeProvider = new TestDateTimeProvider(_now = DateTime.Now));
            e.For<ILog>().Mock();
        }

        #endregion

        #region Exposed Methods

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container(InitializeContainer);
            _target = _container.GetInstance<SystemDeliveryEntryFileUploader>();
        }

        #endregion

        [TestMethod]
        public void TestUploadSystemDeliveryEntriesUploadsSystemDeliveryEntriesFile()
        {
            var contents = "contents";
            var workingDirectory = "workingDirectory";
            var path = Path.Combine(workingDirectory,
                $"MapCall_SystemDeliveryEntries_{_now.ToString(CommonStringFormats.SQL_DATE_WITHOUT_PARAMETER)}.txt");

            _config.Setup(x => x.WorkingDirectory).Returns(workingDirectory);

            _target.UploadSystemDeliveryEntries(contents);

            _fileClient.Verify(x => x.WriteFile(path, contents));
        }
    }
}
