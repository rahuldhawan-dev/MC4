using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using MapCallIntranet.Controllers;
using MapCallIntranet.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.Documents;
using Moq;
using StructureMap;

namespace MapCallIntranet.Tests
{
    [TestClass]
    public class NearMissControllerTest : MapCallIntranetControllerTestBase<NearMissController, NearMiss>
    {
        #region Private Members

        private Mock<HttpContextBase> _httpContext;
        private Mock<HttpRequestBase> _httpRequest;
        private InMemoryDocumentService _docServ;
        private Mock<IDocumentDataRepository> _documentDataRepo;
        private Mock<IRepository<DocumentLink>> _docLinkRepo;

        #endregion

        #region Test Initialization

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDataTypeRepository>().Use<DataTypeRepository>();
            e.For<IDocumentTypeRepository>().Use<DocumentTypeRepository>();
            e.For<IDocumentRepository>().Use<DocumentRepository>();
            e.For<IDocumentService>().Use(_docServ = new InMemoryDocumentService());
            _documentDataRepo = e.For<IDocumentDataRepository>().Mock();
            _docLinkRepo = e.For<IRepository<DocumentLink>>().Mock(); 
        }

        [TestInitialize]
        public void TestInitialize()
        {
            SetupRequestContext();
        }
        
        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateNearMiss)vm;
                model.State = GetEntityFactory<State>().Create().Id;
                model.OperatingCenter = GetEntityFactory<OperatingCenter>().Create().Id;
                model.Type = GetEntityFactory<NearMissType>().Create().Id;
                model.Category = GetEntityFactory<NearMissCategory>().Create().Id;
                model.SystemType = GetEntityFactory<SystemType>().Create().Id;
                model.CompletedCorrectiveActions = false;
                model.NotCompanyFacility = false;
            };
        }
        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization(){}

        #endregion

        #region New/Create
        [TestMethod]
        public void TestCreateNearMissRequiresHttpPost()
        {
            MyAssert.MethodHasAttribute<HttpPostAttribute>(_target, "Create", typeof(CreateNearMiss));
        }

        [TestMethod]
        public void TestCreatesWhenModelStateIsValid()
        {
            var nearMiss = GetFactory<NearMissFactory>().Create();
            var opc = GetEntityFactory<OperatingCenter>().Create();
            var model = new CreateNearMiss(_container) {
                Id = nearMiss.Id,
                OccurredAt = nearMiss.OccurredAt,
                Severity = nearMiss.Severity,
                Description = nearMiss.Description
            };

            var result = _target.Create(model);
            MvcAssert.RedirectsToRoute(result,
                new {
                    action = "Show",
                    controller = "NearMiss",
                    id = model.Id
                });
        }
        
        [TestMethod]
        [Ignore]
        public override void TestShowReturnsNotFoundIfRecordCanNotBeFound() { }

        [TestMethod]
        [Ignore]
        public override void TestShowReturnsShowViewWhenRecordIsFound() { }

        [TestMethod]
        public void TestNearMissWithDocumentOnCreateUploadsDocument()
        {
            _container.Inject(_docServ);
            var nearMiss = GetFactory<NearMissFactory>().Create();
            var dataType = GetEntityFactory<DataType>().Create(new { TableName = "NearMisses", Name = "NearMisses" });
            var documentType = GetEntityFactory<DocumentType>().Create(new { DataType = dataType, Name = "Near Miss Document" });
            dataType.DocumentTypes.Add(documentType);
            var expectedData = new byte[] { 1, 2, 3, 4 };
            var model = new CreateNearMiss(_container) {
                Id = nearMiss.Id,
                OccurredAt = nearMiss.OccurredAt,
                Severity = nearMiss.Severity,
                Description = nearMiss.Description,
                FileUpload = new AjaxFileUpload {
                    FileName = "Some file.tif",
                    BinaryData = expectedData
                }
            };

            var result = _target.Create(model);

            Assert.AreEqual(model.Id, ((RedirectToRouteResult)result).RouteValues["id"]);
            MvcAssert.RedirectsToRoute(result,
                new {
                    action = "Show",
                    controller = "NearMiss",
                    id = model.Id
                });
        }

        [TestMethod]
        public void TestCreatesWhenModelStateIsValidAndFileAlreadyExists()
        {
            _container.Inject(_docServ);
            var nearMiss = GetFactory<NearMissFactory>().Create();
            var expectedData = new byte[] { 1, 2, 3, 4 };
            var dataType = GetEntityFactory<DataType>().Create(new { TableName = "NearMisses", Name = "NearMisses" });
            var documentType = GetEntityFactory<DocumentType>().Create(new { DataType = dataType, Name = "Near Miss Document" });
            var documentData = GetEntityFactory<DocumentData>().Create(new { BinaryData = expectedData});
            _documentDataRepo.Setup(x => x.FindByBinaryData(expectedData)).Returns(documentData);
            _docLinkRepo.Setup(x => x.Save(It.IsAny<DocumentLink>()));
            dataType.DocumentTypes.Add(documentType);
            
            var model = new CreateNearMiss(_container)
            {
                Id = nearMiss.Id,
                OccurredAt = nearMiss.OccurredAt,
                Severity = nearMiss.Severity,
                Description = nearMiss.Description,
                FileUpload = new AjaxFileUpload
                {
                    FileName = "Some file.tif",
                    BinaryData = expectedData
                }
            };

            var result = _target.Create(model);

            Assert.AreEqual(model.Id, ((RedirectToRouteResult)result).RouteValues["id"]);
            MvcAssert.RedirectsToRoute(result,
                new
                {
                    action = "Show",
                    controller = "NearMiss",
                    id = model.Id
                });
        }

        #endregion

        private void SetupRequestContext()
        {
            _httpContext = new Mock<HttpContextBase>();
            _httpRequest = new Mock<HttpRequestBase>();
            _httpContext.Setup(x => x.Request).Returns(_httpRequest.Object);
            _target.ControllerContext = new ControllerContext(
                _httpContext.Object, new RouteData(), _target);
        }
    }
}