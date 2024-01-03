using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using MapCallScheduler.Library.Common;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Utilities;
using MMSINC.Utilities.Documents;
using StructureMap;
using MMSINC.Testing.NHibernate;
using Moq;
using MapCallScheduler.JobHelpers.NSIPremiseFileLink;
using MMSINC.Data.NHibernate;
using MapCallScheduler.Library.Configuration;
using MapCallScheduler.Library.Filesystem;
using MMSINC.Testing.ClassExtensions;
using System.IO;
using System.Text;
using MapCallScheduler.Library.JobHelpers.Common;
using NHibernate;
using Humanizer;

namespace MapCallScheduler.Tests.JobHelpers.NSIPremiseFileLink
{
    [TestClass]
    public class NSIPremiseFileLinkServiceTest : InMemoryDatabaseTest<Premise, PremiseRepository>
    {
        #region Private Members

        private NSIPremiseFileLinkService _target;
        private Mock<INSIPremiseFileLinkServiceConfiguration> _config;
        private Mock<ILog> _log;
        private InMemoryDocumentService _docServ;
        private Mock<IRepository<DocumentLink>> _docLinkRepo;
        private Mock<IRepository<Premise>> _premiseRepo;
        private Mock<IDocumentRepository> _docRepo;
        private Mock<IFileConfigSection> _fileConfig;
        private Mock<IDataTypeRepository> _dtRepo;
        private Mock<IFileClientFactory> _fileClientFactory;
        private Mock<IDocumentDataRepository> _documentDataRepo;
        private Mock<IFileClient> _client;
        private readonly string _workingDirectory = "foo/bar";
        private readonly string contents = "this is what is in the file";
        private FileInfo fileInfo;
        private Byte[] bytes;
        private Premise _premise;

        #endregion

        #region Test Initialization

        [TestInitialize]
        public void TestInitialize()
        {
            _container.Inject((_log = new Mock<ILog>()).Object);
            _container.Inject((_config = new Mock<INSIPremiseFileLinkServiceConfiguration>()).Object);
            _container.Inject((_docLinkRepo = new Mock<IRepository<DocumentLink>>()).Object);
            _container.Inject((_docRepo = new Mock<IDocumentRepository>()).Object);
            _premiseRepo = new Mock<IRepository<Premise>>();

            _fileConfig.SetupGet(f => f.WorkingDirectory).Returns(_workingDirectory);
            _fileConfig.SetupGet(f => f.MakeChanges).Returns(true);
            _config.SetupGet(c => c.FileConfig).Returns(_fileConfig.Object);
            _fileClientFactory.Setup(x => x.Build()).Returns(_client.Object);

            _premise = GetEntityFactory<Premise>().Create(new {
                PremiseNumber = "123",
                Installation = "123",
                ConnectionObject = "123",
                DeviceLocation = "123",
                Id = 1
            });

            //OperatingCenterID.PremiseNumber.InstallationNumber.DeviceLocation.connection.PDF
            var filename = "opCenterId." + _premise.PremiseNumber + "." + _premise.Installation + "." + _premise.DeviceLocation + "." + _premise.ConnectionObject + ".pdf"; 
            
            fileInfo = new FileInfo(filename);
            bytes = Encoding.UTF8.GetBytes(contents);

            
            _dtRepo.Setup(c => c.GetByTableName("Premises")).Returns(new [] {
                new DataType {
                    Id = 1, 
                    DocumentTypes = new List<DocumentType> {
                        new DocumentType {
                            Name = "Premise"
                        }
                    }
                }
            });
            
            _target = new NSIPremiseFileLinkService(_container,
              new NSIPremiseFileDownloadService(_config.Object,_fileClientFactory.Object,
                  _log.Object),
              _log.Object,
              _premiseRepo.Object);
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<ISession>().Mock();
            e.For<IDocumentService>().Use(_docServ = new InMemoryDocumentService());
            e.For<IDocumentTypeRepository>().Use<DocumentTypeRepository>();
            e.For<IDocumentRepository>().Use<DocumentRepository>();

            _documentDataRepo = e.For<IDocumentDataRepository>().Mock();
            _fileConfig = e.For<IFileConfigSection>().Mock();
            _client = e.For<IFileClient>().Mock();
            _fileClientFactory = e.For<IFileClientFactory>().Mock();
            _dtRepo = e.For<IDataTypeRepository>().Mock();
        }

        #endregion

        [TestMethod]
        public void TestDocumentGetsLinkedToPremise()
        {
            _premiseRepo.Setup(x => x.Where(It.IsAny<System.Linq.Expressions.Expression<Func<Premise, bool>>>())).Returns(new[] { _premise }.AsQueryable());
            _client.Setup(c => c.GetListing(_workingDirectory, "*.*.*.*.*.pdf")).Returns(new[] {
                fileInfo
            });
            _client.Setup(c => c.DownloadFile(fileInfo.FullName)).Returns(new FileData(fileInfo.FullName, contents, bytes));

            _target.Process();

            _docLinkRepo.Verify(x => x.Save(It.IsAny<DocumentLink>()), Times.Once);
            _docRepo.Verify(x => x.Save(It.IsAny<Document>()), Times.Once);
            _log.Verify(x => x.Info($"Deleting file '{fileInfo.FullName}'..."), Times.Once);
        }

        [TestMethod]
        public void TestProcessLogsNoFilesFound()
        {
            _client.Setup(c => c.GetListing(_workingDirectory, "*.*.*.*.*.pdf")).Returns(new FileInfo[] { });
            _target.Process();

            _log.Verify(x => x.Info($"Found 0 NSI files..."), Times.Once);
        }

        [TestMethod]
        public void TestProcessDoesNotDeleteFileIfNotLinkedToAPremise()
        {
            var filename = "opCenterId.321.321.321.321.pdf";
            fileInfo = new FileInfo(filename);
            _client.Setup(c => c.DownloadFile(fileInfo.FullName)).Returns(new FileData(fileInfo.FullName, contents, bytes));
            _client.Setup(c => c.GetListing(_workingDirectory, "*.*.*.*.*.pdf")).Returns(new[] {
                fileInfo
            });

            _target.Process();
            _log.Verify(x=> x.Info($"File '{fileInfo.FullName}' could not be matched to a Premise record!"), Times.Once);
        }

        [TestMethod]
        public void TestDocumentGetsLinkedToPremiseWhenFileAlreadyExists()
        {
            _premiseRepo.Setup(x => x.Where(It.IsAny<System.Linq.Expressions.Expression<Func<Premise, bool>>>())).Returns(new[] { _premise }.AsQueryable());
            _client.Setup(c => c.GetListing(_workingDirectory, "*.*.*.*.*.pdf")).Returns(new[] {
                fileInfo
            });
            _client.Setup(c => c.DownloadFile(fileInfo.FullName)).Returns(new FileData(fileInfo.FullName, contents, bytes));
            _documentDataRepo.Setup(x => x.FindByBinaryData(bytes)).Returns(new DocumentData());

            _target.Process();

            _docLinkRepo.Verify(x => x.Save(It.IsAny<DocumentLink>()), Times.Once);
            _docRepo.Verify(x => x.Save(It.IsAny<Document>()), Times.Once);
            _log.Verify(x => x.Info($"Deleting file '{fileInfo.FullName}'..."), Times.Once);
        }
    }
}
