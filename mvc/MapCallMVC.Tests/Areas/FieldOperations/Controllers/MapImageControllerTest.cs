using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing;
using MMSINC.Utilities.Pdf;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class MapImageControllerTest : MapCallMvcControllerTestBase<MapImageController, MapImage, MapImageRepository>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IImageToPdfConverter>().Use(new Mock<IImageToPdfConverter>().Object);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _container.Inject<IMapImageRepository>(Repository);
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = RoleModules.FieldServicesImages;
            Authorization.Assert(a =>
            {
                a.RequiresRole("~/FieldOperations/MapImage/Index/", module, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/MapImage/Search/", module, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/MapImage/Show/", module, RoleActions.Read);
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
        public void TestShowSetsDirectionsInViewDataWhenEntityExists()
        {
            var town = GetFactory<TownFactory>().Create();
            var entity = GetFactory<MapImageFactory>().Create(new { North = "NORTH", South = "SOUTH", East = "EAST", West = "WEST", Town = town });
            var north = GetFactory<MapImageFactory>().Create(new { MapPage = "NORTH", Town = town });
            var south = GetFactory<MapImageFactory>().Create(new { MapPage = "SOUTH", Town = town });
            var east = GetFactory<MapImageFactory>().Create(new { MapPage = "EAST", Town = town });
            var west = GetFactory<MapImageFactory>().Create(new { MapPage = "WEST", Town = town });

            _target.Show(entity.Id);

            Assert.AreSame(north, _target.ViewData["North"]);
            Assert.AreSame(south, _target.ViewData["South"]);
            Assert.AreSame(east, _target.ViewData["East"]);
            Assert.AreSame(west, _target.ViewData["West"]);
        }

        [TestMethod]
        public void TestShowDoesNotSetDirectionsInViewDataIfEntityDoesNotExist()
        {
            _target.Show(0);
            Assert.IsFalse(_target.ViewData.ContainsKey("North"));
            Assert.IsFalse(_target.ViewData.ContainsKey("South"));
            Assert.IsFalse(_target.ViewData.ContainsKey("East"));
            Assert.IsFalse(_target.ViewData.ContainsKey("West"));
        }
        
        [TestMethod]
        public void TestShowRespondsToPdf()
        {
            var mockRepo = new Mock<IMapImageRepository>();
            var entity = GetFactory<MapImageFactory>().Create();
            mockRepo.Setup(x => x.Find(entity.Id)).Returns(entity);
            var expectedData = new byte[] {1, 2, 3};
            mockRepo.Setup(x => x.GetImageDataAsPdf(entity)).Returns(expectedData);
            _container.Inject(mockRepo.Object);
            
            Request = Application.CreateRequestHandler("~/FieldOperations/MapImage/Show" + entity.Id + ".pdf");
            _target = Request.CreateAndInitializeController<MapImageController>();
            var result = _target.Show(entity.Id);
            Assert.IsInstanceOfType(result, typeof(AssetImagePdfResult));
            Assert.AreSame(expectedData, ((AssetImagePdfResult)result).PrerenderedPdf);
            Assert.AreSame(entity, ((AssetImagePdfResult)result).Entity);
        }

        [TestMethod]
        public void TestShowPdfReturns404WhenTheImageFileDoesNotExist()
        {
            var fileNotFound = new AssetImageException(AssetImageExceptionType.FileNotFound, "blah");
            var mockRepo = new Mock<IMapImageRepository>();
            var entity = GetFactory<MapImageFactory>().Create();
            mockRepo.Setup(x => x.Find(entity.Id)).Returns(entity);
            mockRepo.Setup(x => x.GetImageDataAsPdf(entity)).Throws(fileNotFound);
            _container.Inject(mockRepo.Object);

            Request = Application.CreateRequestHandler("~/FieldOperations/MapImage/Show" + entity.Id + ".pdf");
            _target = Request.CreateAndInitializeController<MapImageController>();
            var result = _target.Show(entity.Id);
            MvcAssert.IsNotFound(result, "The requested image does not exist on the system. Please contact your administrator as the image may not have been uploaded correctly.");
        }

        [TestMethod]
        public void TestShowPdfReturns404WhenTheImageIsCorrupt()
        {
            var fileNotFound = new AssetImageException(AssetImageExceptionType.InvalidImageData, "blah");
            var mockRepo = new Mock<IMapImageRepository>();
            var entity = GetFactory<MapImageFactory>().Create();
            mockRepo.Setup(x => x.Find(entity.Id)).Returns(entity);
            mockRepo.Setup(x => x.GetImageDataAsPdf(entity)).Throws(fileNotFound);
            _container.Inject(mockRepo.Object);

            Request = Application.CreateRequestHandler("~/FieldOperations/MapImage/Show" + entity.Id + ".pdf");
            _target = Request.CreateAndInitializeController<MapImageController>();
            var result = _target.Show(entity.Id);
            MvcAssert.IsNotFound(result, "The requested image is corrupt or otherwise unable to be displayed. Please contact your administrator as this image needs to be uploaded again.");
        }

        #endregion

        #endregion
    }
}
