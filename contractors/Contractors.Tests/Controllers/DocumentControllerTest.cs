using System;
using System.Web.Mvc;
using Contractors.Controllers;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Metadata;
using MMSINC.Testing;
using MMSINC.Utilities;
using MMSINC.Utilities.Documents;
using StructureMap;
using DocumentRepository = Contractors.Data.Models.Repositories.DocumentRepository;

namespace Contractors.Tests.Controllers
{
    [TestClass]
    public class DocumentControllerTest : ContractorControllerTestBase<DocumentController, Document, DocumentRepository>
    {
        #region Fields

        private InMemoryDocumentService _docServ;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(
            ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDocumentService>()
             .Use(_docServ = new InMemoryDocumentService());
            e.For<IDocumentTypeRepository>().Use<DocumentTypeRepository>();
            e.For<IDataTypeRepository>().Use<DataTypeRepository>();
            e.For<IDateTimeProvider>().Use<DateTimeProvider>();
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

            _target.New(_viewModelFactory.Build<NewDocument>());

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
            var model = new NewDocument(_container);
            model.Map(expected);
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
            var model = new NewDocument(_container) {
                TableName = "foo table", LinkedId = 666, DocumentType = documentType.Id
            };
            model.Map(expected);
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
                a.RequiresLoggedInUserOnly("~/Document/New");
                a.RequiresLoggedInUserOnly("~/Document/Create");
                a.RequiresLoggedInUserOnly("~/Document/Download");
            });
        }

        [TestMethod]
        public void TestDownloadExplicitlyRequiresSecureForms()
        {
            RequiresSecureForm("~/Document/Download");
        }
    }
}