using System;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Utilities.Documents;
using MMSINC.Exceptions;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using StructureMap;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class DocumentLinkControllerTest : MapCallMvcControllerTestBase<DocumentLinkController, DocumentLink>
    {
        #region Setup/Teardown

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDataTypeRepository>().Use<DataTypeRepository>();
            e.For<IDocumentTypeRepository>().Use<DocumentTypeRepository>();
            e.For<IDocumentRepository>().Use<DocumentRepository>();
            e.For<IDocumentService>().Singleton().Use<InMemoryDocumentService>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _container.Inject(Session);
            // needs to exist for some of the automatic tests.
            Request.Request.Setup(x => x.UrlReferrer).Returns(() => new Uri("http://www.internet.com"));
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeUpdateViewModel = (vm) => {
                var model = (EditDocumentLink)vm;
                model.UrlReferrer = "http://www.internet.com";
                model.TableName = "SomeTable";
            };
            options.CreateRedirectsToReferrerOnSuccess = true;
            options.CreateRedirectSuccessUrlFragment = "#DocumentsTab";
            options.DestroyRedirectsToReferrerOnSuccess = true;
            options.DestroyRedirectSuccessUrlFragment = "#DocumentsTab";
            options.DestroyRedirectsToReferrerOnError = true;
            options.DestroyRedirectErrorUrlFragment = "#DocumentsTab";
        }

        #endregion

        #region New(NewDocumentLink)

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            // override needed due to validation on TableName
            var model = new NewDocumentLink(_container) {TableName = "foo table"};

            var result = _target.New(model) as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(model, result.Model);
        }

        [TestMethod]
        public void TestNewAddsDropDownDataForDocumentTypesByTableName()
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

            _target.New(new NewDocumentLink(_container) {
                TableName = "foo table"
            });

            _target.AssertHasDropDownData(expected, dt => dt.Id, dt => dt.Name);
        }

        [TestMethod]
        public void TestNewThrowsExceptionIfTableNameIsNotSet()
        {
            new[] {"", " ", null}.Each(
                str => MyAssert.Throws<ModelValidationException>(
                    () => _target.New(new NewDocumentLink(_container) {
                        TableName = str
                    })));
        }

        #endregion

        #region Create(NewDocumentLink)

        [TestMethod]
        public void TestCreateThrowsModelValidationExceptionIfModelIsNotValid()
        {
            var doc = GetFactory<DocumentFactory>().Create();
            var docType = GetFactory<DocumentTypeFactory>().Create();
            _target.ModelState.AddModelError("foo", new Exception());

            MyAssert.Throws<ModelValidationException>(
                () => _target.Create(new NewDocumentLink(_container) {
                    DocumentId = doc.Id,
                    DocumentType = docType.Id,
                    LinkedId = 666,
                    TableName = "foo table"
                }));
        }

        [TestMethod]
        public override void TestCreateReturnsNewViewWithModelIfModelStateErrorsExist()
        {
            Assert.Inconclusive("Doesn't return validation results to client.");
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public void TestEditExplicitlyRequiresSecureForm()
        {
            RequiresSecureForm("~/DocumentLink/Edit");
        }

        [TestMethod]
        public override void TestUpdateReturnsEditViewWithModelWhenThereAreModelStateErrors()
        {
            Assert.Inconclusive("Test that I redirect to the UrlReferrer when there are errors");
        }

        [TestMethod]
        public override void TestUpdateRedirectsToShowActionAfterSuccessfulSave()
        {
            // override because it redirects to the UrlReferrer prop instead.
            var eq = GetEntityFactory<DocumentLink>().Create();
            var doctype = GetEntityFactory<DocumentType>().Create();
            var referrer = "http://somereferrer.com/foo/bar";
            var result = (RedirectResult)_target.Update(_viewModelFactory.BuildWithOverrides<EditDocumentLink, DocumentLink>(eq, new
            {
                DocumentType = doctype.Id,
                UrlReferrer = referrer
            }));

            Assert.AreEqual(referrer, result.Url);
        }

        #endregion

        #region SecureIndexForSingleRecord

        [TestMethod]
        public void TestSecureIndexForSingleRecordForciblyEnablesPagingAndSetsItTo25Records()
        {
            var model = new SecureSearchDocumentForSingleEntity();
            model.EnablePaging = false;
            model.PageSize = 1924521;

            _target.SecureIndexForSingleRecord(model);

            Assert.IsTrue(model.EnablePaging);
            Assert.AreEqual(25, model.PageSize);
        }

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/DocumentLink/New/");
                a.RequiresLoggedInUserOnly("~/DocumentLink/Create/");
                a.RequiresLoggedInUserOnly("~/DocumentLink/Destroy/");
                a.RequiresLoggedInUserOnly("~/DocumentLink/Edit/");
                a.RequiresLoggedInUserOnly("~/DocumentLink/Update/");
                a.RequiresLoggedInUserOnly("~/DocumentLink/SecureIndexForSingleRecord/");
            });
        }

        #endregion
    }
}
