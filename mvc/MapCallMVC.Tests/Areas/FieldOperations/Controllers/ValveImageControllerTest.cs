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
    public class ValveImageControllerTest : MapCallMvcControllerTestBase<ValveImageController, ValveImage, ValveImageRepository>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IValveImageRepository>().Use(_ => Repository);
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
                a.RequiresRole("~/FieldOperations/ValveImage/Index/", module, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/ValveImage/Search/", module, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/ValveImage/Show/", module, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/ValveImage/New/", module, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/ValveImage/Create/", module, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/ValveImage/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/ValveImage/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/ValveImage/Destroy/", module, RoleActions.Delete);
            });
        }

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ResponseFormatter.KnownExtensions.EXCEL_2003;
            var valve = GetFactory<ValveFactory>().Create(); 
            var valveImage1 = GetFactory<ValveImageFactory>().Create(new { CreatedAt = Lambdas.GetNow()});
            var valveImage2 = GetFactory<ValveImageFactory>().Create(new { 
                FileName = "some file.tif", 
                CreatedAt = Lambdas.GetYesterday(),
                OperatingCenter = new OperatingCenter{Id = 1, OperatingCenterName = "opCenter", OperatingCenterCode="opCenterCode"},
                Town = new Town{Id = 1, ShortName = "town", State = new State{Abbreviation = "NJ"}, County = new County{Name = "county"}},
                TownSection = "townSection",
                FullStreetName = new Street{Name = "fullStreetName"},
                ValveNumber = "valveNumber",
                Valve = valve,
                FullCrossStreetName = new Street { Name = "fullCrossStreetName" },
                Location = "location",
                NormalPosition = new ValveNormalPosition{Description = "normalPosition"},
                NumberOfTurns = "numberOfTurns",
                DateCompleted = "dateCompleted",
                ValveSize = "1",
                OpenDirection = new ValveOpenDirection{Description = "openDirection"},
                IsDefaultImageForValve = false,
                OfficeReviewRequired = true
            });
            var search = new SearchValveImage();

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(valveImage1.Id, "Id");
                helper.AreEqual(valveImage1.FileName, "FileName");
                helper.AreEqual(valveImage1.FileName, "FileName", 1);
                helper.AreEqual(valveImage2.CreatedAt, "DateAdded", 1);
                helper.AreEqual(valveImage2.OperatingCenter, "OperatingCenter", 1);
                helper.AreEqual(valveImage2.Town.State, "State", 1);
                helper.AreEqual(valveImage2.Town.County, "County", 1);
                helper.AreEqual(valveImage2.Town, "Town", 1);
                helper.AreEqual(valveImage2.TownSection, "TownSection", 1);
                helper.AreEqual(valveImage2.FullStreetName, "FullStreetName", 1);
                helper.AreEqual(valveImage2.ValveNumber, "ValveNumber", 1);
                helper.AreEqual(valveImage2.Valve.Id, "ValveId", 1);
                helper.AreEqual(valveImage2.FullCrossStreetName, "FullCrossStreetName", 1);
                helper.AreEqual(valveImage2.Location, "Location", 1);
                helper.AreEqual(valveImage2.NormalPosition, "NormalPosition", 1);
                helper.AreEqual(valveImage2.NumberOfTurns, "NumberOfTurns", 1);
                helper.AreEqual(valveImage2.DateCompleted, "DateCompleted", 1);
                helper.AreEqual(valveImage2.ValveSize, "ValveSize", 1);
                helper.AreEqual(valveImage2.OpenDirection, "OpenDirection", 1);
                helper.AreEqual(valveImage2.IsDefaultImageForValve, "IsDefaultImageForValve", 1);
                helper.AreEqual(valveImage2.OfficeReviewRequired, "OfficeReviewRequired", 1);
            }
        }

        [TestMethod]
        public void TestIndexReturnsResult()
        {
            var valveImage1 = GetEntityFactory<ValveImage>().Create(new {
                ApartmentNumber = "TestingApartmentNumber1"
            });
            var valveImage2 = GetEntityFactory<ValveImage>().Create(new {
                ApartmentNumber = "TestingApartmentNumber2"
            });
            var search = new SearchValveImage {
                ApartmentNumber = new SearchString {
                    MatchType = SearchStringMatchType.Wildcard,
                    Value = "Test"
                }
            };

            var result = (ViewResult)_target.Index(search);
            var resultModel = ((SearchValveImage)result.Model).Results.ToList();

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(2, resultModel.Count);
            Assert.AreSame(valveImage1, resultModel[0]);
            Assert.AreSame(valveImage2, resultModel[1]);
        }

        #endregion

        #region Show

        [TestMethod]
        public void TestShowRespondsToPdf()
        {
            var mockRepo = new Mock<IValveImageRepository>();
            var entity = GetFactory<ValveImageFactory>().Create();
            mockRepo.Setup(x => x.Find(entity.Id)).Returns(entity);
            var expectedData = new byte[] { 1, 2, 3 };
            mockRepo.Setup(x => x.GetImageDataAsPdf(entity)).Returns(expectedData);
            _container.Inject(mockRepo.Object);

            Request = Application.CreateRequestHandler("~/FieldOperations/ValveImage/Show" + entity.Id + ".pdf");
            _target = Request.CreateAndInitializeController<ValveImageController>();
            var result = _target.Show(entity.Id);
            Assert.IsInstanceOfType(result, typeof(AssetImagePdfResult));
            Assert.AreSame(expectedData, ((AssetImagePdfResult)result).PrerenderedPdf);
            Assert.AreSame(entity, ((AssetImagePdfResult)result).Entity);
        }

        [TestMethod]
        public void TestShowPdfReturns404WhenTheImageFileDoesNotExist()
        {
            var fileNotFound = new AssetImageException(AssetImageExceptionType.FileNotFound, "blah");
            var mockRepo = new Mock<IValveImageRepository>();
            var entity = GetFactory<ValveImageFactory>().Create();
            mockRepo.Setup(x => x.Find(entity.Id)).Returns(entity);
            mockRepo.Setup(x => x.GetImageDataAsPdf(entity)).Throws(fileNotFound);
            _container.Inject(mockRepo.Object);

            Request = Application.CreateRequestHandler("~/FieldOperations/ValveImage/Show" + entity.Id + ".pdf");
            _target = Request.CreateAndInitializeController<ValveImageController>();
            var result = _target.Show(entity.Id);
            MvcAssert.IsNotFound(result, "The requested image does not exist on the system. Please contact your administrator as the image may not have been uploaded correctly.");
        }

        [TestMethod]
        public void TestShowPdfReturns404WhenTheImageIsCorrupt()
        {
            var fileNotFound = new AssetImageException(AssetImageExceptionType.InvalidImageData, "blah");
            var mockRepo = new Mock<IValveImageRepository>();
            var entity = GetFactory<ValveImageFactory>().Create();
            mockRepo.Setup(x => x.Find(entity.Id)).Returns(entity);
            mockRepo.Setup(x => x.GetImageDataAsPdf(entity)).Throws(fileNotFound);
            _container.Inject(mockRepo.Object);

            Request = Application.CreateRequestHandler("~/FieldOperations/ValveImage/Show" + entity.Id + ".pdf");
            _target = Request.CreateAndInitializeController<ValveImageController>();
            var result = _target.Show(entity.Id);
            MvcAssert.IsNotFound(result, "The requested image is corrupt or otherwise unable to be displayed. Please contact your administrator as this image needs to be uploaded again.");
        }

        #endregion

        #region Create

        [TestMethod]
        public override void TestCreateReturnsNewViewWithModelIfModelStateErrorsExist()
        {
            // This needs to be here for Create tests to get around the
            // file saving stuff in the actual repository.
            var mockRepo = new Mock<IValveImageRepository>();
            _container.Inject(mockRepo.Object);
            _target = Request.CreateAndInitializeController<ValveImageController>();
            _target.ModelState.AddModelError("Error", "error");
            var vi = GetFactory<ValveImageFactory>().Create();

            var model = _viewModelFactory.Build<CreateValveImage, ValveImage>(vi);
            model.Id = 0;
            model.FileUpload = new AjaxFileUpload {
                FileName = "Some file.tif",
                BinaryData = new byte[] { }
            };

            var result = _target.Create(model);
            MvcAssert.IsViewWithNameAndModel(result, "New", model);
        }

        [TestMethod]
        public override void TestCreateRedirectsToTheRecordsShowPageAfterSuccessfullySaving()
        {
            // This needs to be here for Create tests to get around the
            // file saving stuff in the actual repository.
            var mockRepo = new Mock<IValveImageRepository>();
            _container.Inject(mockRepo.Object);
            _target = Request.CreateAndInitializeController<ValveImageController>();

            var vi = GetFactory<ValveImageFactory>().Create();

            var model = _viewModelFactory.Build<CreateValveImage, ValveImage>( vi);
            model.Id = 0;
            model.FileUpload = new AjaxFileUpload
            {
                FileName = "Some file.tif",
                BinaryData = new byte[] { }
            };

            var result = _target.Create(model);
            // Don't need area in the route. Null area is replaced with current area when the url is generated.
            MvcAssert.RedirectsToRoute(result, "ValveImage", "Show", new { id = model.Id });
        }

        [TestMethod]
        public override void TestCreateSavesNewRecordWhenModelStateIsValid()
        {
            Assert.Inconclusive("Test me");
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

        [TestMethod]
        public void TestNewSetsNormalPositionDropDownData()
        {
            var expected = GetFactory<ValveNormalPositionFactory>().Create();
            _target.New();
            var vd = (IEnumerable<SelectListItem>)_target.ViewData["NormalPosition"];
            Assert.AreEqual(1, vd.Count());
            Assert.AreEqual(expected.ToString(), vd.Single().Text);
            Assert.AreEqual(expected.Id.ToString(), vd.Single().Value);
        }

        [TestMethod]
        public void TestNewSetsOpenDirectionDropDownData()
        {
            var expected = GetFactory<ValveOpenDirectionFactory>().Create();
            _target.New();
            var vd = (IEnumerable<SelectListItem>)_target.ViewData["OpenDirection"];
            Assert.AreEqual(1, vd.Count());
            Assert.AreEqual(expected.ToString(), vd.Single().Text);
            Assert.AreEqual(expected.Id.ToString(), vd.Single().Value);
        }

        [TestMethod]
        public void TestNewWithValidIdSetsDefaultValues()
        {
            _currentUser.IsAdmin = true;
            var valve = GetFactory<ValveFactory>().Create();
            var result = (ViewResult)_target.New(valve.Id);
            var model = (CreateValveImage)result.Model;
            
            Assert.AreEqual(valve.OperatingCenter.Id, model.OperatingCenter);
        }
        
        #endregion

        #region Edit

        [TestMethod]
        public void TestEditSetsOperatingCenterDropDownData()
        {
            var vi = GetFactory<ValveImageFactory>().Create();
            var expected = GetFactory<OperatingCenterFactory>().Create();
            _target.Edit(vi.Id);
            var vd = (IEnumerable<SelectListItem>)_target.ViewData["OperatingCenter"];
            Assert.AreEqual(1, vd.Count());
            Assert.AreEqual(expected.ToString(), vd.Single().Text);
            Assert.AreEqual(expected.Id.ToString(), vd.Single().Value);
        }
        
        [TestMethod]
        public void TestEditSetsNormalPositionDropDownData()
        {
            var vi = GetFactory<ValveImageFactory>().Create();
            var expected = GetFactory<ValveNormalPositionFactory>().Create();
            _target.Edit(vi.Id);
            var vd = (IEnumerable<SelectListItem>)_target.ViewData["NormalPosition"];
            Assert.AreEqual(1, vd.Count());
            Assert.AreEqual(expected.ToString(), vd.Single().Text);
            Assert.AreEqual(expected.Id.ToString(), vd.Single().Value);
        }

        [TestMethod]
        public void TestEditSetsOpenDirectionDropDownData()
        {
            var vi = GetFactory<ValveImageFactory>().Create();
            var expected = GetFactory<ValveOpenDirectionFactory>().Create();
            _target.Edit(vi.Id);
            var vd = (IEnumerable<SelectListItem>)_target.ViewData["OpenDirection"];
            Assert.AreEqual(1, vd.Count());
            Assert.AreEqual(expected.ToString(), vd.Single().Text);
            Assert.AreEqual(expected.Id.ToString(), vd.Single().Value);
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

        #endregion
    }
}
