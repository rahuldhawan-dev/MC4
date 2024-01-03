using System.Linq;
using System.Web.Mvc;
using MMSINC.Data.NHibernate;
using MMSINC.Exceptions;
using MMSINC.Testing.MSTest.TestExtensions;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class CoordinateControllerTest : MapCallMvcControllerTestBase<CoordinateController, Coordinate>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/Coordinate/Show/");
                a.RequiresLoggedInUserOnly("~/Coordinate/New/");
                a.RequiresLoggedInUserOnly("~/Coordinate/Create/");
                a.RequiresLoggedInUserOnly("~/Coordinate/Close/");
                a.RequiresLoggedInUserOnly("~/Coordinate/GetCoordinateIdForAsset/");
            });
        }

        #region Create

        [TestMethod]
        public override void TestCreateRedirectsToTheRecordsShowPageAfterSuccessfullySaving()
        {
            // noop, returns a json result. Tested below.
        }

        [TestMethod]
        public override void TestCreateSavesNewRecordWhenModelStateIsValid()
        {
            // override because create returns a json result with coordinate id, lat, and lon when successful
            var valueFor = "foo";
            var icon = GetFactory<MapIconFactory>().Create();
            var model = _viewModelFactory.BuildWithOverrides<CreateCoordinate, Coordinate>(GetFactory<CoordinateFactory>().Build(new {Icon = icon}), new {
                ValueFor = valueFor,
                IconId = icon.Id
            });

            JsonResult result = null;
            MyAssert.CausesIncrease(() => result = _target.Create(model) as JsonResult,
                () => Repository.GetAll().Count());
            dynamic data = result.Data;
            Assert.AreEqual(model.Id, data.coordinateId);
            Assert.AreEqual(model.Latitude, data.lat);
            Assert.AreEqual(model.Longitude, data.lng);
        }

        [TestMethod]
        public void TestCreateDoesNotCreateNewCoordinateForExistingValues()
        {
            var valueFor = "foo";
            var model = _viewModelFactory.BuildWithOverrides<CreateCoordinate, Coordinate>(GetFactory<CoordinateFactory>().Create(), new {
                ValueFor = valueFor
            });

            JsonResult result = null;
            MyAssert.DoesNotCauseIncrease(() => result = _target.Create(model) as JsonResult,
                () => Repository.GetAll().Count());
            dynamic data = result.Data;
            Assert.AreEqual(model.Id, data.coordinateId);
            Assert.AreEqual(model.Latitude, data.lat);
            Assert.AreEqual(model.Longitude, data.lng);
        }

        [TestMethod]
        public override void TestCreateReturnsNewViewWithModelIfModelStateErrorsExist()
        {
            // override because this throws an exception for modelstate errors for some reason.
            _target.ModelState.AddModelError("foo", "UH OH!!!");

            MyAssert.Throws<ModelValidationException>(() => _target.Create(new CreateCoordinate(_container)));
        }

        #endregion

        #region New

        [TestMethod]
        public void TestNewFromExistingCoordinatesReturnsView()
        {
            var original = GetFactory<CoordinateFactory>().Create();
            var model = _viewModelFactory.BuildWithOverrides<CreateCoordinate>(new { Id = original.Id });

            var result = (ViewResult)_target.New(model);
            var resultModel = (CreateCoordinate)result.Model;

            Assert.AreSame(model, result.Model);
            Assert.AreEqual(original.Latitude, resultModel.Latitude);
            Assert.AreEqual(original.Longitude, resultModel.Longitude);
        }

        #endregion

        #region Show

        [TestMethod]
        public void TestShowSetsDefaultIconOnCoordinateIfCoordinateDoesNotHaveAnIcon()
        {
            var coordinate = GetFactory<CoordinateFactory>().Create();
            coordinate.Icon = null;
            Session.Save(coordinate);

            var setRepo = new Mock<IRepository<IconSet>>();
            var iconRepo = new Mock<IRepository<MapIcon>>();
            var expectedIcon = new MapIcon {FileName = "pin_black"};

            iconRepo.Setup(x => x.GetAll()).Returns(new[] {expectedIcon}.AsQueryable());
            _container.Inject(setRepo.Object);
            _container.Inject(iconRepo.Object);

            var result = _target.Show(coordinate.Id);
            Assert.AreSame(expectedIcon, coordinate.Icon);
        }

        #endregion
    }
}
