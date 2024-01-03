using System;
using System.IO;
using log4net;
using MapCallScheduler.JobHelpers.NonRevenueWater;
using MapCallScheduler.Library.Configuration;
using MapCallScheduler.Library.Filesystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.NonRevenueWaters
{
    [TestClass]
    public class NonRevenueWaterEntryFileUploaderTest
    {
        #region Constants

        private const string CONTENTS = "contents";
        private const string WORKING_DIRECTORY = "workingDirectory";

        #endregion

        #region Private Members

        private IContainer _container;
        private NonRevenueWaterEntryFileUploader _target;
        private Mock<INonRevenueWaterEntryFileDumpServiceConfiguration> _serviceConfig;
        private Mock<IFileConfigSection> _config;
        private Mock<IFileClient> _fileClient;
        private TestDateTimeProvider _dateTimeProvider;
        private DateTime _now;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container(InitializeContainer);
            _target = _container.GetInstance<NonRevenueWaterEntryFileUploader>();
        }

        #endregion

        #region Private Methods

        private void InitializeContainer(ConfigurationExpression e)
        {
            _serviceConfig = e.For<INonRevenueWaterEntryFileDumpServiceConfiguration>().Mock();
            _config = e.For<IFileConfigSection>().Mock();
            _serviceConfig.Setup(x => x.FileConfig).Returns(_config.Object);
            _fileClient = e.For<IFileClient>().Mock();
            e.For<IDateTimeProvider>().Use(_dateTimeProvider = new TestDateTimeProvider(_now = DateTime.Now));
            e.For<ILog>().Mock();
        }

        #endregion

        [TestMethod]
        public void Test_UploadNonRevenueWaterEntries_UploadsFile()
        {
            var path = Path.Combine(WORKING_DIRECTORY,
                $"MapCall_AccountedForLosses_{_now.ToString(CommonStringFormats.SQL_DATE_WITHOUT_PARAMETER)}.txt");

            _config.Setup(x => x.WorkingDirectory).Returns(WORKING_DIRECTORY);

            _target.UploadNonRevenueWaterEntries(CONTENTS);

            _fileClient.Verify(x => x.WriteFile(path, CONTENTS));
        }
    }
}
