using System;
using System.IO;
using log4net;
using MapCallScheduler.JobHelpers.GIS;
using MapCallScheduler.Library.Configuration;
using MapCallScheduler.Library.Filesystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.GIS
{
    [TestClass]
    public class GISFileUploaderTest
    {
        #region Private Members

        private IContainer _container;
        private GISFileUploader _target;

        private Mock<IGISFileDumpServiceConfiguration> _serviceConfig;
        private Mock<IFileConfigSection> _config;
        private Mock<IFileClient> _fileClient;
        private TestDateTimeProvider _dateTimeProvider;
        private DateTime _now;

        #endregion

        #region Private Methods

        private void InitializeContainer(ConfigurationExpression e)
        {
            _serviceConfig = e.For<IGISFileDumpServiceConfiguration>().Mock();
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

            _target = _container.GetInstance<GISFileUploader>();
        }

        #endregion

        [TestMethod]
        public void TestUploadHydrantsUploadsHydrantsFile()
        {
            var contents = "contents";
            var workingDirectory = "workingDirectory";
            var path = Path.Combine(workingDirectory,
                $"MapCall_Hydrants_{_now.ToString(CommonStringFormats.SQL_DATE_WITHOUT_PARAMETER)}.json");

            _config.Setup(x => x.WorkingDirectory).Returns(workingDirectory);

            _target.UploadHydrants(contents);

            _fileClient.Verify(x => x.WriteFile(path, contents));
        }

        [TestMethod]
        public void TestUploadValvesUploadsValvesFile()
        {
            var contents = "contents";
            var workingDirectory = "workingDirectory";
            var path = Path.Combine(workingDirectory,
                $"MapCall_Valves_{_now.ToString(CommonStringFormats.SQL_DATE_WITHOUT_PARAMETER)}.json");

            _config.Setup(x => x.WorkingDirectory).Returns(workingDirectory);

            _target.UploadValves(contents);

            _fileClient.Verify(x => x.WriteFile(path, contents));
        }

        [TestMethod]
        public void TestUploadSewerOpeningsUploadsSewerOpeningsFile()
        {
            var contents = "contents";
            var workingDirectory = "workingDirectory";
            var path = Path.Combine(workingDirectory,
                $"MapCall_SewerOpenings_{_now.ToString(CommonStringFormats.SQL_DATE_WITHOUT_PARAMETER)}.json");

            _config.Setup(x => x.WorkingDirectory).Returns(workingDirectory);

            _target.UploadSewerOpenings(contents);

            _fileClient.Verify(x => x.WriteFile(path, contents));
        }

        [TestMethod]
        public void TestUploadServicesUploadsServicesFile()
        {
            var contents = "contents";
            var workingDirectory = "workingDirectory";
            var path = Path.Combine(workingDirectory,
                $"MapCall_Services_{_now.ToString(CommonStringFormats.SQL_DATE_WITHOUT_PARAMETER)}.json");

            _config.Setup(x => x.WorkingDirectory).Returns(workingDirectory);

            _target.UploadServices(contents);

            _fileClient.Verify(x => x.WriteFile(path, contents));
        }

        [TestMethod]
        public void TestUploadAsBuiltImagesUploadsImagesFile()
        {
            var contents = "contents";
            var workingDirectory = "workingDirectory";
            var path = Path.Combine(workingDirectory,
                $"MapCall_AsBuiltImages_{_now.ToString(CommonStringFormats.SQL_DATE_WITHOUT_PARAMETER)}.json");

            _config.Setup(x => x.WorkingDirectory).Returns(workingDirectory);

            _target.UploadAsBuiltImages(contents);

            _fileClient.Verify(x => x.WriteFile(path, contents));
        }
    }
}
