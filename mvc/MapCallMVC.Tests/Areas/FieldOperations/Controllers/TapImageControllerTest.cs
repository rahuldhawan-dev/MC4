using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Utilities.Pdf;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class TapImageControllerTest : MapCallMvcControllerTestBase<TapImageController, TapImage, TapImageRepository>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<ITapImageRepository>().Use(_ => Repository);
            e.For<IStateRepository>().Use<StateRepository>();
            e.For<ITownRepository>().Use<TownRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<IImageToPdfConverter>().Use(new Mock<IImageToPdfConverter>().Object);
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = RoleModules.FieldServicesImages;
            Authorization.Assert(a =>
            {
                a.RequiresRole("~/FieldOperations/TapImage/Index/", module, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/TapImage/Search/", module, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/TapImage/Show/", module);
                a.RequiresRole("~/FieldOperations/TapImage/New/", module, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/TapImage/Create/", module, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/TapImage/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/TapImage/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/TapImage/Destroy/", module, RoleActions.Delete);
            });
        }

        #region Show

        [TestMethod]
        public void TestShowRespondsToPdf()
        {
            var mockRepo = new Mock<ITapImageRepository>();
            var entity = GetFactory<TapImageFactory>().Create();
            mockRepo.Setup(x => x.Find(entity.Id)).Returns(entity);
            var expectedData = new byte[] { 1, 2, 3 };
            mockRepo.Setup(x => x.GetImageDataAsPdf(entity)).Returns(expectedData);
            _container.Inject(mockRepo.Object);

            Request = Application.CreateRequestHandler("~/FieldOperations/TapImage/Show" + entity.Id + ".pdf");
            _target = Request.CreateAndInitializeController<TapImageController>();
            var result = _target.Show(entity.Id);
            Assert.IsInstanceOfType(result, typeof(AssetImagePdfResult));
            Assert.AreSame(expectedData, ((AssetImagePdfResult)result).PrerenderedPdf);
            Assert.AreSame(entity, ((AssetImagePdfResult)result).Entity);
        }

        [TestMethod]
        public void TestShowPdfReturns404WhenTheImageFileDoesNotExist()
        {
            var fileNotFound = new AssetImageException(AssetImageExceptionType.FileNotFound, "blah");
            var mockRepo = new Mock<ITapImageRepository>();
            var entity = GetFactory<TapImageFactory>().Create();
            mockRepo.Setup(x => x.Find(entity.Id)).Returns(entity);
            mockRepo.Setup(x => x.GetImageDataAsPdf(entity)).Throws(fileNotFound);
            _container.Inject(mockRepo.Object);

            Request = Application.CreateRequestHandler("~/FieldOperations/TapImage/Show" + entity.Id + ".pdf");
            _target = Request.CreateAndInitializeController<TapImageController>();
            var result = _target.Show(entity.Id);
            MvcAssert.IsNotFound(result, "The requested image does not exist on the system. Please contact your administrator as the image may not have been uploaded correctly.");
        }

        [TestMethod]
        public void TestShowPdfReturns404WhenTheImageIsCorrupt()
        {
            var fileNotFound = new AssetImageException(AssetImageExceptionType.InvalidImageData, "blah");
            var mockRepo = new Mock<ITapImageRepository>();
            var entity = GetFactory<TapImageFactory>().Create();
            mockRepo.Setup(x => x.Find(entity.Id)).Returns(entity);
            mockRepo.Setup(x => x.GetImageDataAsPdf(entity)).Throws(fileNotFound);
            _container.Inject(mockRepo.Object);

            Request = Application.CreateRequestHandler("~/FieldOperations/TapImage/Show" + entity.Id + ".pdf");
            _target = Request.CreateAndInitializeController<TapImageController>();
            var result = _target.Show(entity.Id);
            MvcAssert.IsNotFound(result, "The requested image is corrupt or otherwise unable to be displayed. Please contact your administrator as this image needs to be uploaded again.");
        }

        [TestMethod]
        public void TestShowShowsWarningIfLinkedServiceIsLinkedToASampleSite()
        {
            var premise = GetEntityFactory<Premise>().Create(new {
                PremiseNumber = "xyz",
            });

            var sampleSites = GetEntityFactory<SampleSite>().CreateList(2, new {
                Premise = premise
            });

            var service = GetEntityFactory<Service>().Create(new {
                Premise = premise,
                Installation = "9001",
                ServiceNumber = (long?)123
            });

            Session.Flush();
            Session.Clear();

            var tapImage = GetFactory<TapImageFactory>().Create(new {
                Service = service
            });

            Session.Flush();
            Session.Clear();
            
            var result = _target.Show(tapImage.Id) as ViewResult;

            Assert.AreEqual(TapImageController.SAMPLE_SITE_WARNING, ((List<string>)result.TempData[MMSINC.Controllers.ControllerBase.ERROR_MESSAGE_KEY]).Single());
        }

        [TestMethod]
        public void TestShowDoesNotShowWarningIfNotLinkedServiceIsLinkedToASampleSite()
        {
            var tapImage = GetEntityFactory<TapImage>().Create();

            var result = _target.Show(tapImage.Id) as ViewResult;

            Assert.IsNull(result.TempData[MMSINC.Controllers.ControllerBase.ERROR_MESSAGE_KEY]);
        }

        [TestMethod]
        public void TestShowDoesNotShowWarningIfLinkedServiceIsNotLinkedToASampleSite()
        {
            var service = GetEntityFactory<Service>().Create(new { ServiceNumber = (long?)123, PremiseNumber = "123123123" });
            var tapImage = GetEntityFactory<TapImage>().Create(new {Service = service});

            var result = _target.Show(tapImage.Id) as ViewResult;

            Assert.IsNull(result.TempData[MMSINC.Controllers.ControllerBase.ERROR_MESSAGE_KEY]);
        }

        #endregion

        #region Destroy

        [TestMethod]
        public override void TestDestroyRedirectsToSearchPageWhenRecordIsSuccessfullyDestroyed()
        {
            // This test needs to be overridden, but the test that ensures a record is deleted does not.
            // That's because the other test creates two entities. This test only creates one.
            // Because of that, the BaseAssetImageRepository.Delete method will try to delete
            // the actual image file because it doesn't see any other record that uses the same image file.
            Assert.Inconclusive("Test me. I need a mock repo");
        }

        #endregion

        #region Create

        [TestMethod]
        public override void TestCreateReturnsNewViewWithModelIfModelStateErrorsExist()
        {
            // This needs to be here for Create tests to get around the
            // file saving stuff in the actual repository.
            var mockRepo = new Mock<ITapImageRepository>();
            _container.Inject(mockRepo.Object);
            _target = Request.CreateAndInitializeController<TapImageController>();
            _target.ModelState.AddModelError("Error", "error");
            var vi = GetFactory<TapImageFactory>().Create();

            var model = _viewModelFactory.Build<CreateTapImage, TapImage>(vi);
            model.Id = 0;
            model.FileUpload = new AjaxFileUpload {
                FileName = "Some file.tif",
                BinaryData = new byte[] { }
            };

            var result = _target.Create(model);
            MvcAssert.IsViewWithNameAndModel(result, "New", model);
        }

        [TestMethod]
        public override void TestCreateSavesNewRecordWhenModelStateIsValid()
        {
            // noop-override because of the need for a mocked repo. This test
            // is handled in the below successfully saved test, though.
        }

        [TestMethod]
        public override void TestCreateRedirectsToTheRecordsShowPageAfterSuccessfullySaving()
        {
            // This needs to be here for Create tests to get around the
            // file saving stuff in the actual repository.
            var mockRepo = new Mock<ITapImageRepository>();
            _container.Inject(mockRepo.Object);
            _target = Request.CreateAndInitializeController<TapImageController>();

            var vi = GetFactory<TapImageFactory>().Create();

            var model = _viewModelFactory.Build<CreateTapImage, TapImage>( vi);
            model.Id = 0;
            model.FileUpload = new AjaxFileUpload
            {
                FileName = "Some file.tif",
                BinaryData = new byte[] { }
            };

            var result = _target.Create(model);
            // Don't need area in the route. Null area is replaced with current area when the url is generated.
            MvcAssert.RedirectsToRoute(result, "TapImage", "Show", new { id = model.Id });
        }

        [TestMethod]
        public void TestCreateReturnsNewViewWithPassedInModelIfItFails()
        {
            _target.ModelState.AddModelError("Nope", "nope");
            var model = _viewModelFactory.Build<CreateTapImage>();
            var result = _target.Create(model);
            MvcAssert.IsViewWithModel(result, model);
        }

        #endregion

        #region New

        [TestMethod]
        public void TestNewSetsOperatingCenterDropDownData()
        {
            var expected = GetFactory<OperatingCenterFactory>().Create();
            _target.New(null);
            var vd = (IEnumerable<SelectListItem>)_target.ViewData["OperatingCenter"];
            Assert.AreEqual(1, vd.Count());
            Assert.AreEqual(expected.ToString(), vd.Single().Text);
            Assert.AreEqual(expected.Id.ToString(), vd.Single().Value);
        }

        [TestMethod]
        public void TestNewReturnsNewView()
        {
            MvcAssert.IsViewNamed(_target.New(null), "New");
        }

        [TestMethod]
        public void TestNewWithValidIdSetsDefaultValues()
        {
            _currentUser.IsAdmin = true;
            var service = GetEntityFactory<Service>().Create();
            
            var result = (ViewResult)_target.New(service.Id);
            var model = (CreateTapImage)result.Model;

            Assert.AreEqual(service.Id, model.Service);
            // this will fail if null values aren't checked in SetDefaults.
            // rest of the properties are fully tested in CreateTapImageTest
        }

        #endregion

        #region Edit

        [TestMethod]
        public void TestEditReturnsEditView()
        {
            var entity = GetFactory<TapImageFactory>().Create();
            var result = _target.Edit(entity.Id);
            MvcAssert.IsViewNamed(result, "Edit");
        }

        [TestMethod]
        public void TestEditSetsOperatingCenterDropDownData()
        {
            var ti = GetFactory<TapImageFactory>().Create();
            var expected = GetFactory<OperatingCenterFactory>().Create();
            _target.Edit(ti.Id);
            var vd = (IEnumerable<SelectListItem>)_target.ViewData["OperatingCenter"];
            Assert.AreEqual(1, vd.Count());
            Assert.AreEqual(expected.ToString(), vd.Single().Text);
            Assert.AreEqual(expected.Id.ToString(), vd.Single().Value);
        }

        #endregion

        #region Update

        [TestMethod]
        public void TestUpdateReturnsNotFoundIfModelDoesNotExist()
        {
            var model = _viewModelFactory.Build<EditTapImage>();
            var result = _target.Update(model);
            MvcAssert.IsNotFound(result);
        }

        [TestMethod]
        public void TestUpdateRedirectsToShowViewAfterSuccessfullySaving()
        {
            var entity = GetFactory<TapImageFactory>().Create();
            var model = _viewModelFactory.Build<EditTapImage, TapImage>( entity);

            var result = _target.Update(model);
            MvcAssert.RedirectsToRoute(result, "TapImage", "Show", new { id = model.Id });
        }
        #endregion

        #region Index

        [TestMethod]
        public void TestIndexReturnsFragmentWithResults()
        {
            var service = GetEntityFactory<Service>().Create();
            var tapImage1 = GetEntityFactory<TapImage>().Create(new { PremiseNumber = "123456789", Service = service });
            var tapImage2 = GetEntityFactory<TapImage>().Create(new { PremiseNumber = "123456789" });
            var tapImage3 = GetEntityFactory<TapImage>().Create(new { PremiseNumber = "123456789" });
            var search1 = new SearchTapImage() { PremiseNumber = "123456789", HasAsset = false };
            var search2 = new SearchTapImage() { PremiseNumber = "987654321", HasAsset = false };
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ResponseFormatter.KnownExtensions.FRAGMENT;

            var result1 = _target.Index(search1);
            
            Assert.AreEqual(2, search1.Results.Count());
            Assert.IsFalse(search1.Results.Contains(tapImage1));
            Assert.IsTrue(search1.Results.Contains(tapImage2));
            Assert.IsTrue(search1.Results.Contains(tapImage3));
            MvcAssert.IsPartialView(result1);
            MvcAssert.IsViewNamed(result1, "_Index");

            var result2 = _target.Index(search2);

            Assert.AreEqual(0, search2.Results.Count());
            Assert.IsFalse(search2.Results.Contains(tapImage1));
            Assert.IsFalse(search2.Results.Contains(tapImage2));
            Assert.IsFalse(search2.Results.Contains(tapImage3));
            MvcAssert.IsPartialView(result2);
            MvcAssert.IsViewNamed(result2, "_NoResults");
        }

        #endregion

        #endregion
    }
}
