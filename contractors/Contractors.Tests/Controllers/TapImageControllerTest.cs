using System.Collections.Generic;
using System.Linq;
using Contractors.Configuration;
using Contractors.Controllers;
using Contractors.Data.Models.Repositories;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MMSINC.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Metadata;
using Moq;
using StructureMap;
using System;
using System.Web.Mvc;
using MMSINC.Testing.Utilities;

namespace Contractors.Tests.Controllers
{
    [TestClass]
    public class TapImageControllerTest : ContractorControllerTestBase<TapImageController, TapImage, TapImageRepository>
    {
        #region Fields

        private Town _town;
        private OperatingCenter _opc;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(
            ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<ITownRepository>().Use<TownRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _town = GetFactory<TownFactory>().Create();
            _opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            _opc.Towns.Add(_town);
            _town.OperatingCenters.Add(_opc);
            _town.OperatingCentersTowns.Add(new OperatingCenterTown {
                OperatingCenter = _opc,
                Town = _town
            });
            Session.Save(_town);
            _currentUser.Contractor.OperatingCenters.Add(_opc);
            _automatedTestOperatingCenter = _opc;
            Session.Flush();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateTapImage)vm;
                model.FileUpload = new AjaxFileUpload {
                    FileName = "Some file.tif",
                    BinaryData = TestFiles.GetSinglePageTiffFile()
                };
                model.OperatingCenter = _automatedTestOperatingCenter.Id;
                model.ServiceType = "some service type";
                model.Town = _town.Id;
            };
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                a.RequiresLoggedInUserOnly("~/TapImage/Index/");
                a.RequiresLoggedInUserOnly("~/TapImage/Search/");
                a.RequiresLoggedInUserOnly("~/TapImage/Show/");
                a.RequiresLoggedInUserOnly("~/TapImage/New/");
                a.RequiresLoggedInUserOnly("~/TapImage/Create/");
            });
        }

        #region Search

        [TestMethod]
        public void TestSearchSetsStateDropDownData()
        {
            var expected = GetFactory<StateFactory>().Create();
            _target.Search();
            var vd = (IEnumerable<SelectListItem>)_target.ViewData["State"];
            Assert.AreEqual(1, vd.Count());
            Assert.AreEqual(expected.ToString(), vd.Single().Text);
            Assert.AreEqual(expected.Id.ToString(), vd.Single().Value);
        }

        #endregion

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

        #endregion

        #region Create

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

            var model = _viewModelFactory.Build<CreateTapImage, TapImage>(vi);
            model.Id = 0;
            model.Service = 42;
            model.FileUpload = new AjaxFileUpload
            {
                FileName = "Some file.tif",
                BinaryData = new byte[] { }
            };

            var result = _target.Create(model);
            // Don't need area in the route. Null area is replaced with current area when the url is generated.
            MvcAssert.RedirectsToRoute(result, "Service", "Show", new { id = model.Service });
        }

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

        #endregion

        #endregion
    }
}
