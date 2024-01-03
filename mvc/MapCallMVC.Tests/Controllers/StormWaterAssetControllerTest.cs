using System.Linq;
using MapCall.Common.Configuration;
using MMSINC.Data.NHibernate;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class StormWaterAssetControllerTest : MapCallMvcControllerTestBase<StormWaterAssetController, StormWaterAsset> 
    {
        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _container.Inject<IRepository<StormWaterAsset>>(Repository);
            _container.Inject(Session);
        }

        #endregion

        #region Tests
     
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.FieldServicesAssets;
                a.RequiresRole("~/StormWaterAsset/Index/", module, RoleActions.Read);
                a.RequiresRole("~/StormWaterAsset/Search/", module, RoleActions.Read);
                a.RequiresRole("~/StormWaterAsset/Show/", module, RoleActions.Read);
            });
        }

        #region Index

        [TestMethod]
        public void TestIndexRespondsToMapWithExpectedModels()
        {
            InitializeControllerAndRequest("~/StormWaterAsset/Index.map");
            var good = GetFactory<StormWaterAssetFactory>().Create(new { OperatingCenter = typeof(UniqueOperatingCenterFactory) });
            var bad = GetFactory<StormWaterAssetFactory>().Create(new { OperatingCenter = typeof(UniqueOperatingCenterFactory) });
            var model = new SearchStormWaterAsset {
                OperatingCenter = good.OperatingCenter.Id
            };
            var result = (MapResult)_target.Index(model);

            var resultModel = result.CoordinateSets.Single().Coordinates.ToArray();
            Assert.AreEqual(1, resultModel.Count());
            Assert.IsTrue(resultModel.Contains(good));
            Assert.IsFalse(resultModel.Contains(bad));
        }

        [TestMethod]
        public void TestIndexRespondsToMapWithoutModelsIfModelStateIsNotValid()
        {
            InitializeControllerAndRequest("~/StormWaterAsset/Index.map");
            var good = GetFactory<StormWaterAssetFactory>().Create(new { OperatingCenter = typeof(UniqueOperatingCenterFactory) });

            var model = new SearchStormWaterAsset();
            var validResult = (MapResult)_target.Index(model);

            Assert.AreEqual(1, validResult.CoordinateSets.Single().Coordinates.Count());
            Assert.IsTrue(validResult.CoordinateSets.Single().Coordinates.Contains(good));

            _target.ModelState.AddModelError("error", "error");
            var badResult = (MapResult)_target.Index(model);

            Assert.IsFalse(badResult.CoordinateSets.Any());
        }

        #endregion

        #endregion
    }
}
