using System;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.AssetUploads;
using MapCallImporter.Common;
using MapCallScheduler.JobHelpers.AssetUploadProcessor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Common;
using MMSINC.Data.V2;
using MMSINC.Data.V2.NHibernate;
using MMSINC.Interface;
using MMSINC.Testing.NHibernate.V2;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.Library.JobHelpers.AssetUploadProcessor
{
    [TestClass]
    public class AssetUploadProcessorServiceTest : MapCallSchedulerInMemoryDatabaseTest<AssetUpload>
    {
        private Mock<IAssetUploadFileService> _fileService;
        private Mock<IAssetUploadFileHandler> _fileHandler;
        private Mock<ISmtpClient> _smtpClient;
        private Mock<ISmtpClientFactory> _smtpClientFactory;
        private Mock<ILog> _log;
        private AssetUploadProcessorService _target;

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IAssetUploadFileService>().Use((_fileService = new Mock<IAssetUploadFileService>()).Object);
            e.For<IAssetUploadFileHandler>().Use((_fileHandler = new Mock<IAssetUploadFileHandler>()).Object);
            e.For<ISmtpClientFactory>().Use((_smtpClientFactory = new Mock<ISmtpClientFactory>()).Object);
            _smtpClient = new Mock<ISmtpClient>();
            _smtpClientFactory.Setup(x => x.Build()).Returns(_smtpClient.Object);
            e.For<ILog>().Use((_log = new Mock<ILog>()).Object);
            e.For<IUnitOfWorkFactory>().Use<TestUnitOfWorkFactory>();
            e.For(typeof(IRepository<>)).Use(typeof(Repository<>));
        }

        [TestInitialize]
        public void TestInitialize()
        {
            GetFactory<AssetUploadStatusFactory>().CreateAll();
            _target = _container.GetInstance<AssetUploadProcessorService>();
        }

        #endregion

        [TestMethod]
        public void TestProcessProcessesAnyPendingUploads()
        {
            var createdBy = GetEntityFactory<User>().Create(new {Email = "foo@bar.baz"});
            var pending = GetFactory<AssetUploadFactory>()
               .CreateList(2, new {
                    Status = typeof(PendingAssetUploadStatusFactory),
                    CreatedBy = createdBy
                });
            var success = GetFactory<AssetUploadFactory>()
               .CreateList(2, new {Status = typeof(SuccessAssetUploadStatusFactory)});
            var error = GetFactory<AssetUploadFactory>()
               .CreateList(2, new {Status = typeof(ErrorAssetUploadStatusFactory)});
            
            foreach (var upload in pending)
            {
                _fileService.Setup(x => x.GetFilePath(upload.FileGuid)).Returns("foo bar");
                _fileHandler.Setup(x => x.Handle("foo bar")).Returns(
                    new TimedExcelFileMappingResult(ExcelFileProcessingResult.FileValid, typeof(Object),
                        0));
            }

            _target.Process();

            foreach (var upload in pending)
            {
                Assert.AreEqual(AssetUploadStatus.Indices.SUCCESS, Session.Load<AssetUpload>(upload.Id).Status.Id);
            }
        }

        [TestMethod]
        public void TestProcessSetsStatusToErrorAndSetsErrorTextWhenErrorEncountered()
        {
            var pending = GetFactory<AssetUploadFactory>().Create(new {
                Status = typeof(PendingAssetUploadStatusFactory),
                CreatedBy = GetEntityFactory<User>().Create(new {Email = "foo@bar.baz"})
            });

            var issue = "Bad data is bad.";
            _fileService.Setup(x => x.GetFilePath(pending.FileGuid)).Returns("foo bar");
            _fileHandler.Setup(x => x.Handle("foo bar")).Returns(
                new TimedExcelFileMappingResult(ExcelFileProcessingResult.InvalidFileContents, typeof(Object),
                    0, new[] {issue}));

            _target.Process();
            var reloaded = Session.Load<AssetUpload>(pending.Id);

            Assert.AreEqual(AssetUploadStatus.Indices.ERROR, reloaded.Status.Id);
            Assert.IsTrue(reloaded.ErrorText.Contains(issue));
        }

        [TestMethod]
        public void TestProcessSendsEmailWhenErrorEncountered()
        {
            var pending = GetFactory<AssetUploadFactory>().Create(new {
                Status = typeof(PendingAssetUploadStatusFactory),
                CreatedBy = GetEntityFactory<User>().Create(new {Email = "foo@bar.baz"})
            });

            var issue = "Bad data is bad.";
            _fileService.Setup(x => x.GetFilePath(pending.FileGuid)).Returns("foo bar");
            _fileHandler.Setup(x => x.Handle("foo bar")).Returns(
                new TimedExcelFileMappingResult(ExcelFileProcessingResult.InvalidFileContents, typeof(Object),
                    0, new[] {issue}));

            _target.Process();

            _smtpClient.Verify(x => x.Send(It.Is<IMailMessage>(m =>
                m.Subject == $"MapCallImporter: Error Importing File {pending.FileName}" &&
                m.Body.Contains(issue) &&
                m.To[0].Address == pending.CreatedBy.Email)));
        }
    }
}
