using System;
using System.Web.Mvc;
using MapCall.Common.Testing.Utilities;
using MMSINC.Utilities.Documents;
using MMSINC.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using MMSINC.Utilities;
using StructureMap;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class DocumentControllerTest : MapCallMvcControllerTestBase<DocumentController, Document, DocumentRepository>
    {
        #region Fields

        private InMemoryDocumentService DocServ => (InMemoryDocumentService)_container.GetInstance<IDocumentService>();

        #endregion

        #region Setup/Teardown

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDocumentService>().Singleton().Use<InMemoryDocumentService>().Singleton();
            e.For<IDateTimeProvider>().Use<DateTimeProvider>();
            e.For<IDocumentTypeRepository>().Use<DocumentTypeRepository>();
            e.For<IDataTypeRepository>().Use<DataTypeRepository>();
            e.For<IDocumentDataRepository>().Use<DocumentDataRepository>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                var model = (NewDocument)vm;
                model.FileUpload = new AjaxFileUpload {
                    BinaryData = new byte[] { 1, 2, 3, 4 },
                    FileName = "SomeFile.fil"
                };
                model.TableName = "SomeTable";
                model.LinkedId = 1;
                model.DocumentType = GetEntityFactory<DocumentType>().Create().Id;
            };
            options.CreateRedirectsToReferrerOnSuccess = true;
        }

        #endregion

        #region New(NewDocument)

        [TestMethod]
        public void TestNewSetsAListOfDocumentTypesAsDropDownData()
        {
            var expected = GetFactory<DocumentTypeFactory>().CreateList(3);

            _target.New();

            _target.AssertHasDropDownData(expected, dt => dt.Id, dt => dt.Name);
        }

        [TestMethod]
        public void TestNewSetsAListOfDocumentTypesByTableNameIfProvided()
        {
            var dataType = GetFactory<DataTypeFactory>().Create(new {TableName = "foo table"});
            var expected = GetFactory<DocumentTypeFactory>().CreateList(3, new {
                DataType = dataType
            });
            // not sure why this needs to happen, but it does:
            foreach (var docType in expected)
            {
                dataType.DocumentTypes.Add(docType);
            }
            Session.Save(dataType);
            // extras:
            GetFactory<DocumentTypeFactory>().CreateList(3);

            _target.New(new NewDocument(_container) {TableName = "foo table"});

            _target.AssertHasDropDownData(expected, dt => dt.Id, dt => dt.Name);
        }

        #endregion

        #region Create(NewDocument)

        [TestMethod]
        public override void TestCreateRedirectsToTheRecordsShowPageAfterSuccessfullySaving()
        {
            // noop: Does a few different things for redirects. Tested below.
        }

        [TestMethod]
        public void TestCreateRedirectsToTheHomepageIfNoReferrer()
        {
            var createdBy = GetFactory<MapCall.Common.Testing.Data.UserFactory>().Create();
            var expected = GetFactory<DocumentFactory>().Build(new {
                CreatedBy = createdBy, UpdatedBy = createdBy
            });
            var model = _viewModelFactory.Build<NewDocument, Document>(expected);
            model.FileUpload = new AjaxFileUpload {
               // BinaryData = expected.BinaryData,
               BinaryData = new byte[]{0}, 
               FileName = expected.FileName
            };

            var result = _target.Create(model) as RedirectToRouteResult;

            Assert.AreEqual("Home", result.RouteValues["controller"]);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void TestCreateRedirectsBackToTheReferrerWhenSetAndTableNameIsSet()
        {
            var documentType = GetFactory<DocumentTypeFactory>().Create();
            var expected = GetFactory<DocumentFactory>().BuildWithConcreteDependencies();
            var documentStatus = GetFactory<DocumentStatusFactory>().Create();
            var model = _viewModelFactory.BuildWithOverrides<NewDocument, Document>(expected, new {
                TableName = "foo table", LinkedId = 666, DocumentType = documentType.Id
            });
            model.FileUpload = new AjaxFileUpload {
                //BinaryData = expected.BinaryData,
                BinaryData = new byte[] {0},
                FileName = expected.FileName
            };

            var url = "http://somesite.com";
            Request.Request.Setup(x => x.UrlReferrer).Returns(() => new Uri(url));

            var result = _target.Create(model) as RedirectResult;

            Assert.AreEqual(url + DocumentController.FRAGMENT_IDENTIFIER, result.Url);
        }

        [TestMethod]
        public override void TestCreateReturnsNewViewWithModelIfModelStateErrorsExist()
        {
            Assert.Inconclusive("This throws a ModelStateException instead of returning validation errors to the client.");
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/Document/New/");
                a.RequiresLoggedInUserOnly("~/Document/FindDocumentsByName/");
                a.RequiresLoggedInUserOnly("~/Document/Download/");
                a.RequiresLoggedInUserOnly("~/Document/Create/");
            });
        }

        [TestMethod]
        public void TestFindDocumentsByNameDoesNotRequireSecureForm()
        {
            DoesNotRequireSecureForm("~/Document/FindDocumentsByName/");
        }

        [TestMethod]
        public void TestDownloadExplicitlyRequiresSecureForms()
        {
            RequiresSecureForm("~/Document/Download");
        }
    }
}
