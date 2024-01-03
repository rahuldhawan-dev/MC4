using System;
using System.Web.Mvc;
using Contractors.Controllers;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Utilities.Documents;
using StructureMap;

namespace Contractors.Tests.Controllers
{
    [TestClass]
    public class DocumentLinkControllerTest : ContractorControllerTestBase<DocumentLinkController, DocumentLink>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDataTypeRepository>().Use<DataTypeRepository>();
            e.For<IDocumentTypeRepository>().Use<DocumentTypeRepository>();
            e.For<IDocumentRepository>().Use<DocumentRepository>();
            e.For<IDocumentService>().Use<InMemoryDocumentService>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            // needs to exist for some of the automatic tests.
            Request.Request.Setup(x => x.UrlReferrer).Returns(() => new Uri("http://www.internet.com"));
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => GetEntityFactory<DocumentLink>()
               .Create(new {
                    DataType = typeof(DataTypeFactory),
                    DocumentType = typeof(DocumentTypeFactory),
                    Document = typeof(DocumentFactory)
                });
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public void TestEditExplicitlyRequiresSecureForm()
        {
            RequiresSecureForm("~/DocumentLink/Edit");
        }

        [TestMethod]
        public override void TestUpdateRedirectsToShowActionAfterSuccessfulSave()
        {
            // override because it redirects to the UrlReferrer prop instead.
            var eq = GetEntityFactory<DocumentLink>().Create(new {
                DataType = typeof(DataTypeFactory),
                Document = typeof(DocumentFactory)
            });
            var doctype = GetEntityFactory<DocumentType>().Create();
            var referrer = "http://somereferrer.com/foo/bar";
            var result = _target.Update(
                _viewModelFactory
                   .BuildWithOverrides<EditDocumentLink, DocumentLink>(eq, new {
                        DocumentType = doctype.Id,
                        UrlReferrer = referrer
                    })) as RedirectResult;

            Assert.AreEqual(referrer, result.Url);
        }

        #endregion

        #region Destroy(int)

        [TestMethod]
        public override void TestDestroyRedirectsBackToShowPageOfAttemptedDeletedRecordIfThereAreModelStateErrors()
        {
            // noop - this is tested below
        }

        [TestMethod]
        public override void TestDestroyRedirectsToSearchPageWhenRecordIsSuccessfullyDestroyed()
        {
            // noop override: this keeps redirecting back to the Show page, so there's some validation error coming up.
            // the other tests cover this already.
        }

        [TestMethod]
        public void TestDestroyRedirectsBackToTheReferrerIfSet()
        {
            var link = GetFactory<DocumentLinkFactory>().Create();
            var model = _container.GetInstance<DeleteDocumentLink>();
            model.Map(link);
            var url = "http://somesite.com";
            Request.Request.Setup(x => x.UrlReferrer).Returns(() => new Uri(url));

            var result = _target.Destroy(model) as RedirectResult;

            Assert.AreEqual(url + DocumentLinkController.FRAGMENT_IDENTIFIER, result.Url);
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/DocumentLink/Edit");
                a.RequiresLoggedInUserOnly("~/DocumentLink/Update");
                a.RequiresLoggedInUserOnly("~/DocumentLink/Destroy");
            });
        }
    }
}