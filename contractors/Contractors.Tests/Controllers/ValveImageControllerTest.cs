using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Contractors.Configuration;
using Contractors.Controllers;
using Contractors.Data.Models.Repositories;
using MapCall.Common.Model.Entities;
using MMSINC.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using Moq;
using StructureMap;

namespace Contractors.Tests.Controllers
{
    [TestClass]
    public class ValveImageControllerTest : ContractorControllerTestBase<ValveImageController, ValveImage, ValveImageRepository>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<ITownRepository>().Use<TownRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                a.RequiresLoggedInUserOnly("~/ValveImage/Index/");
                a.RequiresLoggedInUserOnly("~/ValveImage/Search/");
                a.RequiresLoggedInUserOnly("~/ValveImage/Show/");
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
      
        #endregion
    }
}
