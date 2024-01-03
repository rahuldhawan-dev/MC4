using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Utilities;
using MMSINC.Utilities.Pdf;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class AsBuiltImageControllerTest : MapCallMvcControllerTestBase<AsBuiltImageController, AsBuiltImage, AsBuiltImageRepository>
    {
        #region Fields

        private Mock<INotificationService> _notifier;
        private Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use((_dateTimeProvider = new Mock<IDateTimeProvider>()).Object);
            e.For<INotificationService>().Use((_notifier = new Mock<INotificationService>()).Object);
            e.For<IAsBuiltImageRepository>().Use<AsBuiltImageRepository>();
            e.For<IStateRepository>().Use<StateRepository>();
            e.For<ITownRepository>().Use<TownRepository>();
            e.For<ITownSectionRepository>().Use<TownSectionRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<IImageToPdfConverter>().Use(new Mock<IImageToPdfConverter>().Object);
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeSearchTester = (tester) => {
                tester.IgnoredPropertyNames.Add("Coordinate"); // property only exists for setting other search mapped values.
            };
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateAsBuiltImage)vm;
                model.FileUpload = new AjaxFileUpload {
                    FileName = "Some file.tif",
                    BinaryData = new byte[] { }
                };
            };
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = RoleModules.FieldServicesImages;
            Authorization.Assert(a =>
            {
                a.RequiresRole("~/FieldOperations/AsBuiltImage/New/", module, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/AsBuiltImage/Create/", module, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/AsBuiltImage/Show/", module, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/AsBuiltImage/Index/", module, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/AsBuiltImage/Search/", module, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/AsBuiltImage/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/AsBuiltImage/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/AsBuiltImage/Destroy/", module, RoleActions.Delete);
            });
        }

        #region Search

        [TestMethod]
        public void TestSearchSetsOperatingCenterDropDownData()
        {
            var expected = GetFactory<OperatingCenterFactory>().Create();
            _target.Search();
            var vd = (IEnumerable<SelectListItem>)_target.ViewData["OperatingCenter"];
            Assert.AreEqual(1, vd.Count());
            Assert.AreEqual(expected.ToString(), vd.Single().Text);
            Assert.AreEqual(expected.Id.ToString(), vd.Single().Value);
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexReturnsIndexViewWhenOnlyASingleResult()
        {
            var asBuiltImage1 = GetFactory<AsBuiltImageFactory>().Create();
            var search = new SearchAsBuiltImage();
            _target.ControllerContext = new ControllerContext();

            var result = (ViewResult)_target.Index(search);
            var resultModel = ((SearchAsBuiltImage)result.Model).Results.ToList();

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(1, resultModel.Count);
            Assert.AreSame(asBuiltImage1, resultModel[0]);
        }

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ResponseFormatter.KnownExtensions.EXCEL_2003;
            var asBuiltImage1 = GetFactory<AsBuiltImageFactory>().Create(new { CoordinatesModifiedOn = Lambdas.GetNow(), CreatedAt = Lambdas.GetNow(), TaskNumber = "1234" });
            var asBuiltImage2 = GetFactory<AsBuiltImageFactory>().Create(new { CoordinatesModifiedOn = Lambdas.GetNow(), CreatedAt = Lambdas.GetYesterday() });
            var search = new SearchAsBuiltImage();

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(asBuiltImage1.Id, "Id");
                helper.AreEqual(asBuiltImage1.CoordinatesModifiedOn, "CoordinatesModifiedOn");
                helper.AreEqual(asBuiltImage1.TaskNumber, "WBSNumber");
                helper.AreEqual(asBuiltImage2.Id, "Id", 1);
                helper.AreEqual(asBuiltImage2.CoordinatesModifiedOn, "CoordinatesModifiedOn", 1);
            }
        }

        [TestMethod]
        public void TestIndexReturnsResult()
        {
            var asBuiltImage1 = GetFactory<AsBuiltImageFactory>().Create(new {
                ApartmentNumber = "TestingApartmentNumber1"
            });
            var asBuiltImage2 = GetFactory<AsBuiltImageFactory>().Create(new {
                ApartmentNumber = "TestingApartmentNumber2"
            });
            var search = new SearchAsBuiltImage {
                ApartmentNumber = new SearchString {
                    MatchType = SearchStringMatchType.Wildcard,
                    Value = "Test"
                }
            };

            var result = (ViewResult)_target.Index(search);
            var resultModel = ((SearchAsBuiltImage)result.Model).Results.ToList();

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(2, resultModel.Count);
            Assert.AreSame(asBuiltImage1, resultModel[0]);
            Assert.AreSame(asBuiltImage2, resultModel[1]);
        }

        #endregion

        #region Create

        [TestMethod]
        public override void TestCreateReturnsNewViewWithModelIfModelStateErrorsExist()
        {
            // Override needed because of the need for the mock repo and to
            // bypass the FileUpload validation that's a major pain.

            var mockRepo = new Mock<IAsBuiltImageRepository>();
            _container.Inject(mockRepo.Object);
            _target = Request.CreateAndInitializeController<AsBuiltImageController>();

            _target.ModelState.AddModelError("Error", "error");
            var asBuilt = GetFactory<AsBuiltImageFactory>().Create();

            var model = _viewModelFactory.Build<CreateAsBuiltImage, AsBuiltImage>(asBuilt);
            model.Id = 0;
            model.FileUpload = new AjaxFileUpload {
                FileName = "Some file.tif",
                BinaryData = new byte[] {}
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
            var mockRepo = new Mock<IAsBuiltImageRepository>();
            _container.Inject(mockRepo.Object);
            _target = Request.CreateAndInitializeController<AsBuiltImageController>();

            var asBuilt = GetFactory<AsBuiltImageFactory>().Create();

            var model = _viewModelFactory.Build<CreateAsBuiltImage, AsBuiltImage>(asBuilt);
            model.Id = 0;
            model.FileUpload = new AjaxFileUpload {
                FileName = "Some file.tif",
                BinaryData = new byte[] {}
            };
            
            mockRepo.Setup(x => x.Find(model.Id)).Returns(asBuilt);
            var result = _target.Create(model);
            // Don't need area in the route. Null area is replaced with current area when the url is generated.
            MvcAssert.RedirectsToRoute(result, "AsBuiltImage", "Show", new { id = model.Id });
        }

        [TestMethod]
        public void TestCreateSendsNotificationEmail()
        {
            // This needs to be here for Create tests to get around the
            // file saving stuff in the actual repository.
            var mockRepo = new Mock<IAsBuiltImageRepository>();
            _container.Inject(mockRepo.Object);
            _target = Request.CreateAndInitializeController<AsBuiltImageController>();

            var asBuilt = GetFactory<AsBuiltImageFactory>().Create();

            var model = _viewModelFactory.Build<CreateAsBuiltImage, AsBuiltImage>(asBuilt);
            model.Id = 0;
            model.FileUpload = new AjaxFileUpload
            {
                FileName = "Some file.tif",
                BinaryData = new byte[] { }
            };
            model.CoordinateChanged = true;

            mockRepo.Setup(x => x.Find(model.Id)).Returns(asBuilt);

            NotifierArgs resultArgs = null;

            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            _target.Create(model);
            var entity = asBuilt; // They're identical outside of the id.

            Assert.AreEqual(entity.OperatingCenter.Id, resultArgs.OperatingCenterId);
            Assert.AreEqual(RoleModules.FieldServicesImages, resultArgs.Module);
            Assert.AreEqual("AsBuiltImage Coordinate Changed", resultArgs.Purpose);
            Assert.AreSame(entity, resultArgs.Data);
            Assert.IsNull(resultArgs.Subject);
        }

        #endregion

        #region New

        [TestMethod]
        public void TestNewSetsOperatingCenterDropDownData()
        {
            var expected = GetFactory<OperatingCenterFactory>().Create();
            _target.New();
            var vd = (IEnumerable<SelectListItem>)_target.ViewData["OperatingCenter"];
            Assert.AreEqual(1, vd.Count());
            Assert.AreEqual(expected.ToString(), vd.Single().Text);
            Assert.AreEqual(expected.Id.ToString(), vd.Single().Value);
        }

        #endregion

        #region Edit

        [TestMethod]
        public void TestEditSetsOperatingCenterDropDownData()
        {
            var abi = GetFactory<AsBuiltImageFactory>().Create();
            var expected = GetFactory<OperatingCenterFactory>().Create();
            _target.Edit(abi.Id);
            var vd = (IEnumerable<SelectListItem>)_target.ViewData["OperatingCenter"];
            Assert.AreEqual(1, vd.Count());
            Assert.AreEqual(expected.ToString(), vd.Single().Text);
            Assert.AreEqual(expected.Id.ToString(), vd.Single().Value);
        }

        #endregion

        #region Show

        [TestMethod]
        public void TestShowRespondsToPdf()
        {
            var mockRepo = new Mock<IAsBuiltImageRepository>();
            var entity = GetFactory<AsBuiltImageFactory>().Create();
            mockRepo.Setup(x => x.Find(entity.Id)).Returns(entity);
            var expectedData = new byte[] { 1, 2, 3 };
            mockRepo.Setup(x => x.GetImageDataAsPdf(entity)).Returns(expectedData);
            _container.Inject(mockRepo.Object);

            Request = Application.CreateRequestHandler("~/FieldOperations/AsBuiltImage/Show" + entity.Id + ".pdf");
            _target = Request.CreateAndInitializeController<AsBuiltImageController>();
            var result = _target.Show(entity.Id);
            Assert.IsInstanceOfType(result, typeof(AssetImagePdfResult));
            Assert.AreSame(expectedData, ((AssetImagePdfResult)result).PrerenderedPdf);
            Assert.AreSame(entity, ((AssetImagePdfResult)result).Entity);
        }

        [TestMethod]
        public void TestShowPdfReturns404WhenTheImageFileDoesNotExist()
        {
            var fileNotFound = new AssetImageException(AssetImageExceptionType.FileNotFound, "blah");
            var mockRepo = new Mock<IAsBuiltImageRepository>();
            var entity = GetFactory<AsBuiltImageFactory>().Create();
            mockRepo.Setup(x => x.Find(entity.Id)).Returns(entity);
            mockRepo.Setup(x => x.GetImageDataAsPdf(entity)).Throws(fileNotFound);
            _container.Inject(mockRepo.Object);

            Request = Application.CreateRequestHandler("~/FieldOperations/AsBuiltImage/Show" + entity.Id + ".pdf");
            _target = Request.CreateAndInitializeController<AsBuiltImageController>();
            var result = _target.Show(entity.Id);
            MvcAssert.IsNotFound(result, "The requested image does not exist on the system. Please contact your administrator as the image may not have been uploaded correctly.");
        }

        [TestMethod]
        public void TestShowPdfReturns404WhenTheImageIsCorrupt()
        {
            var fileNotFound = new AssetImageException(AssetImageExceptionType.InvalidImageData, "blah");
            var mockRepo = new Mock<IAsBuiltImageRepository>();
            var entity = GetFactory<AsBuiltImageFactory>().Create();
            mockRepo.Setup(x => x.Find(entity.Id)).Returns(entity);
            mockRepo.Setup(x => x.GetImageDataAsPdf(entity)).Throws(fileNotFound);
            _container.Inject(mockRepo.Object);

            Request = Application.CreateRequestHandler("~/FieldOperations/AsBuiltImage/Show" + entity.Id + ".pdf");
            _target = Request.CreateAndInitializeController<AsBuiltImageController>();
            var result = _target.Show(entity.Id);
            MvcAssert.IsNotFound(result, "The requested image is corrupt or otherwise unable to be displayed. Please contact your administrator as this image needs to be uploaded again.");
        }

        [TestMethod]
        public void TestShowRespondsToFragment()
        {
            var entity = GetFactory<AsBuiltImageFactory>().Create();
            InitializeControllerAndRequest("~/FieldOperations/AsBuiltImage/Show" + entity.Id + ".frag");

            var result = _target.Show(entity.Id);
            MvcAssert.IsViewWithNameAndModel(result, "_ShowPopup", entity);
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
            Assert.Inconclusive("Test me. I need a mock repo.");
        }

        #endregion

        #endregion
    }
}
