using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MMSINC.Data.NHibernate;
using MMSINC.Exceptions;
using MMSINC.Testing.MSTest.TestExtensions;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallIntranet.Controllers;
using MapCallIntranet.Helpers;
using MapCallIntranet.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using Moq;

namespace MapCallIntranet.Tests
{
    [TestClass]
    public class CoordinateControllerTest : MapCallIntranetControllerTestBase<CoordinateController, Coordinate>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresUserWithIntranetProfile(("~/Coordinate/Show/"));
                a.RequiresUserWithIntranetProfile("~/Coordinate/New/");
                a.RequiresUserWithIntranetProfile("~/Coordinate/Close/");
                a.RequiresUserWithIntranetProfile("~/Coordinate/Create/");
                a.RequiresUserWithIntranetProfile("~/Coordinate/GetCoordinateIdForAsset/");
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
            var model = _viewModelFactory.BuildWithOverrides<CreateCoordinate, Coordinate>(GetFactory<CoordinateFactory>().Build(new { Icon = icon }), new {
                ValueFor = valueFor,
                IconId = icon.Id
            });

            JsonResult result = null;
            MyAssert.CausesIncrease(() => result = _target.Create(model) as JsonResult,
                () => Repository.GetAll().Count());
            dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(result.Data.ToString().Replace("=",":"));
            Assert.AreEqual(model.Id, data.coordinateId.Value);
            Assert.AreEqual(model.Latitude, (decimal)data.lat.Value);
            Assert.AreEqual(model.Longitude, (decimal)data.lng.Value);
        }

        [TestMethod]
        public override void TestCreateReturnsNewViewWithModelIfModelStateErrorsExist()
        {
            // override because this throws an exception for modelstate errors for some reason.
            _target.ModelState.AddModelError("foo", "UH OH!!!");

            MyAssert.Throws<ModelValidationException>(() => _target.Create(new CreateCoordinate(_container)));
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
            var expectedIcon = new MapIcon { FileName = "pin_black" };

            iconRepo.Setup(x => x.GetAll()).Returns(new[] { expectedIcon }.AsQueryable());
            _container.Inject(setRepo.Object);
            _container.Inject(iconRepo.Object);

            var result = _target.Show(coordinate.Id);
            Assert.AreSame(expectedIcon, coordinate.Icon);
        }

        #endregion
    }
}
