using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Configuration;
using MapCall.Common.Controllers;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class AssetMapControllerTest : MapCallMvcControllerTestBase<AssetMapController, IconSet>
    {
        #region Fields

        private AssetMapView _indexModel;
        private IconSet _assetIconSet;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {

            _assetIconSet = GetFactory<IconSetFactory>().Create(new {
                Id = IconSets.Assets
            });
            _assetIconSet.Icons = new List<MapIcon>();

            _indexModel = new AssetMapView
            {
                ControllerName = "Controller",
                ActionName = "Index"
            };
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.FieldServicesAssets;
            Authorization.Assert(a =>
            {
                a.RequiresRole("~/AssetMap/Index/", role);
                a.RequiresRole("~/AssetMap/GetExtents/", role);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // noop override: This isn't a typical search-result Index action.
        }

        [TestMethod]
        public void TestIndexDoesNotSetMapConfigurationWhenModelStateIsInvalid()
        {
            _target.ModelState.AddModelError("bluh", "afluahe");
            _indexModel.Search.Add("SomeProperty", "SomeValue");
            _target.Index(_indexModel);
            Assert.IsFalse(_target.ViewData.ContainsKey(MapController.MAP_CONFIGURATION));
        }

        [TestMethod]
        public void TestIndexReturnsViewWithValidationErrorsWhenModelStateIsNotValid()
        {
            var expected = "error message user will see";
            _target.ModelState.AddModelError("error", expected);
            _target.Index(_indexModel);
            Assert.AreEqual(expected, _indexModel.MapConfiguration.validationErrors.Single());
        }

        [TestMethod]
        public void TestIndexReturnsViewResultWhenModelStateIsValid()
        {
            MvcAssert.IsViewNamed(_target.Index(_indexModel), "Index");
        }

        [TestMethod]
        public void TestIndexSetsDataUrlOnMapConfigurationWhenModelStateIsValid()
        {
            _indexModel.Search.Add("SomeProperty", "SomeValue");
            _target.Index(_indexModel);

            Assert.AreEqual("/Controller/Index.map?SomeProperty=SomeValue", _indexModel.MapConfiguration.dataUrl);
        }

        [TestMethod]
        public void TestIndexSetsViewDataForSerializedIcons()
        {
            var expectedIcon = GetEntityFactory<MapIcon>().Create();
            _assetIconSet.Icons.Add(expectedIcon);
            _target.Index(_indexModel);

            // There should only be one.
            var resultIcon = _indexModel.MapConfiguration.icons.Single();
            Assert.AreEqual(String.Format("/Content/images/{0}", expectedIcon.FileName), resultIcon.url);
            Assert.AreEqual(expectedIcon.Width, resultIcon.width);
            Assert.AreEqual(expectedIcon.Height, resultIcon.height);
            Assert.AreEqual(expectedIcon.Id, resultIcon.id);
        }

        [TestMethod]
        public void TestGetExtentsReturnsAllAssetTypesInExtents()
        {
            var coords = GetEntityFactory<Coordinate>().CreateList(3, new { Latitude = 1m, Longitude = 1m });
            var invalidCoords = GetEntityFactory<Coordinate>().CreateList(3, new { Latitude = 10m, Longitude = 10m });
            var hyd1 = GetEntityFactory<Hydrant>().Create(new { Coordinate = coords[0]});
            var hyd2 = GetEntityFactory<Hydrant>().Create(new { Coordinate = invalidCoords[0] });
            var val1 = GetEntityFactory<Valve>().Create(new { Coordinate = coords[1]});
            var val2 = GetEntityFactory<Valve>().Create(new { Coordinate = invalidCoords[1] });
            var bo1 = GetFactory<BlowOffValveFactory>().Create(new { Coordinate = coords[2] });
            var bo2 = GetFactory<BlowOffValveFactory>().Create(new { Coordinate = invalidCoords[2] });
            var search = new AssetCoordinateSearch{ LatitudeMin = 0.5m, LatitudeMax = 1.5m, LongitudeMin = 0.5m, LongitudeMax = 1.5m};

            var results =_target.GetExtents(search) as AssetMapResult;
            
            Assert.AreEqual(3, results.CoordinateSets.Count);
        }

        [TestMethod]
        public void TestIndexSetsDataUrlOnMapConfigurationWhenModelStateIsValidAndOperatingCenterExists ()
        {
            _indexModel.Search.Add("OperatingCenter", "10");
            _target.Index(_indexModel);

            Assert.AreEqual("/Controller/Index.map?OperatingCenter=10", _indexModel.MapConfiguration.dataUrl);
        }

        [TestMethod]
        public void TestIndexSetsDataUrlOnMapConfigurationWhenModelStateIsValidAndMultipleOperatingCentersExists()
        {
            _indexModel.Search.Add("OperatingCenter", "10,14");
            _target.Index(_indexModel);

            Assert.AreEqual("/Controller/Index.map?OperatingCenter=10%2C14", _indexModel.MapConfiguration.dataUrl);
        }

        [TestMethod]
        public override void TestIndexCanPerformSearchForAllSearchModelProperties()
        {
            // Noop: This is all handled by index
        }

        #endregion
    }
}
