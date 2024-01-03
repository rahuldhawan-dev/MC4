using System;
using System.IO;
using System.Linq;
using System.Text;
using log4net;
using MapCallScheduler.Library.Common;
using MapCallScheduler.Library.Configuration;
using MapCallScheduler.Library.Filesystem;
using MapCallScheduler.Library.JobHelpers.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.ClassExtensions;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.Library.JobHelpers.FileImports
{
    public abstract class FileDownloadServiceTestBase<TConfiguration, TTarget>
        where TConfiguration : class, IFileServiceConfiguration
        where TTarget : FileDownloadServiceBase<TConfiguration>
    {
        #region Private Members

        protected Mock<TConfiguration> _config;
        protected Mock<IFileConfigSection> _fileConfig;
        protected Mock<IFileClientFactory> _ftpClientFactory;
        protected Mock<IFileClient> _client;
        protected IContainer _container;
        protected string _workingDirectory = "foo/bar";
        protected TTarget _target;

        #endregion

        #region Exposed Methods

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container(InitializeContainer);
            _fileConfig = new Mock<IFileConfigSection>();
            _fileConfig.SetupGet(f => f.WorkingDirectory).Returns(_workingDirectory);
            _config.SetupGet(c => c.FileConfig).Returns(_fileConfig.Object);
            _client = new Mock<IFileClient>();
            _ftpClientFactory.Setup(x => x.Build()).Returns(_client.Object);

            _target = _container.GetInstance<TTarget>();
        }

        private void InitializeContainer(ConfigurationExpression e)
        {
            e.For<ILog>().Mock();
            _config = e.For<TConfiguration>().Mock();
            _ftpClientFactory = e.For<IFileClientFactory>().Mock();
        }

        [TestMethod]
        public void TestGetAllFilesEnumeratesMaterialFilesOnRemotePathsAndThenDisposesTheCLient()
        {
            foreach (var f in _target.GetAllFiles()) { }

            _client.Verify(c => c.GetListing(_workingDirectory, _target.FilePattern));
        }

        [TestMethod]
        public void TestGetAllFilesGetsFileContents()
        {
            var filename = "foo";
            var fileInfo = new FileInfo(filename);
            var contents = "this is what is in the file";
            var bytes = Encoding.UTF8.GetBytes(contents);
            _client.Setup(c => c.GetListing(_workingDirectory, _target.FilePattern)).Returns(new[] {
                fileInfo
            });
            _client.Setup(c => c.DownloadFile(fileInfo.FullName)).Returns(new FileData(fileInfo.FullName, contents, bytes));

            var files = _target.GetAllFiles().ToArray();

            Assert.AreEqual(fileInfo.FullName, files[0].Filename);
            Assert.AreEqual(contents, files[0].Content);
            Assert.AreEqual(bytes, files[0].Bytes);
        }

        [TestMethod]
        public void TestDeleteFileDeletesFile()
        {
            _fileConfig.SetupGet(c => c.MakeChanges).Returns(true);

            _target.DeleteFile("foo");

            _client.Verify(c => c.DeleteFile("foo"));
        }

        [TestMethod]
        public void TestDeleteFilePropagatesEncounteredException()
        {
            var ex = new Exception();
            _fileConfig.SetupGet(c => c.MakeChanges).Returns(true);
            _client.Setup(x => x.DeleteFile("foo")).Throws(ex);

            try
            {
                _target.DeleteFile("foo");
                Assert.Fail("Exception was not thrown as expected.");
            }
            catch (Exception e)
            {
                Assert.AreSame(ex, e.InnerException);
            }
        }

        [TestMethod]
        public void TestDeleteFileDoesNotDeleteFileIfNotConfiguredToMakeChanges()
        {
            _target.DeleteFile("foo");

            _client.Verify(c => c.DeleteFile("foo"), Times.Never);
        }

        #endregion
    }
}
