using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Results;
using MMSINC.Testing;
using StructureMap;
using System.Linq;
using System.Web.Mvc;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class FunctionalLocationControllerTest : MapCallMvcControllerTestBase<FunctionalLocationController, FunctionalLocation>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<ITownRepository>().Use<TownRepository>();
            e.For<IFunctionalLocationRepository>().Use<FunctionalLocationRepository>();
        }

        #endregion

        #region Roles

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                var module = RoleModules.FieldServicesDataLookups;
                a.RequiresRole("~/FieldOperations/FunctionalLocation/Search", module, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/FunctionalLocation/Show", module, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/FunctionalLocation/Index", module, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/FunctionalLocation/Edit", module, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/FunctionalLocation/Update", module, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/FunctionalLocation/New", module, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/FunctionalLocation/Create", module, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/FunctionalLocation/Destroy", module, RoleActions.Delete);
                a.RequiresLoggedInUserOnly("~/FieldOperations/FunctionalLocation/ByTownForFacilityAssetType");
                a.RequiresLoggedInUserOnly("~/FieldOperations/FunctionalLocation/ByTownForSewerOpenings");
                a.RequiresLoggedInUserOnly("~/FieldOperations/FunctionalLocation/ActiveByTownForSewerOpenings");
                a.RequiresLoggedInUserOnly("~/FieldOperations/FunctionalLocation/ByTownForMainAsset");
                a.RequiresLoggedInUserOnly("~/FieldOperations/FunctionalLocation/ByTownForSewerMainAsset");
                a.RequiresLoggedInUserOnly("~/FieldOperations/FunctionalLocation/ByFacilityId");
                a.RequiresLoggedInUserOnly("~/FieldOperations/FunctionalLocation/ByTownId");
                a.RequiresLoggedInUserOnly("~/FieldOperations/FunctionalLocation/ActiveByTownId");
                a.RequiresLoggedInUserOnly("~/FieldOperations/FunctionalLocation/ActiveByTownIdForHydrantAssetType");
                a.RequiresLoggedInUserOnly("~/FieldOperations/FunctionalLocation/ActiveByTownIdForSewerOpeningAssetType");
                a.RequiresLoggedInUserOnly("~/FieldOperations/FunctionalLocation/ActiveByTownIdForValveAssetType");
            });
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetFactory<FunctionalLocationFactory>().Create(new {Description = "description 0"});
            var entity1 = GetFactory<FunctionalLocationFactory>().Create(new {Description = "description 1"});
            var search = new SearchFunctionalLocation();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.Description, "Description");
                helper.AreEqual(entity1.Description, "Description", 1);
            }
        }

        #endregion

        #region BySomethingId

        [TestMethod]
        public void TestByTownForFacilityAssetType()
        {
            var town = GetEntityFactory<Town>().Create();
            var invalid = GetEntityFactory<Town>().Create();
            var assetTypeValid = GetFactory<FacilityAssetTypeFactory>().Create();
            var assetTypeInvalid = GetFactory<ValveAssetTypeFactory>().Create();

            var functionalLocation = GetFactory<EquipmentFunctionalLocationFactory>().Create(new { Town = town, AssetType = assetTypeValid });
            var functionalLocationInvalid1 = GetFactory<ValveFunctionalLocationFactory>().Create(new { Town = town, AssetType = assetTypeInvalid });
            var functionalLocationInvalid2 = GetFactory<ValveFunctionalLocationFactory>().Create(new { Town = invalid, AssetType = assetTypeValid });
            var functionalLocationInvalid3 = GetFactory<ValveFunctionalLocationFactory>().Create(new { Town = invalid, AssetType = assetTypeInvalid });

            var result = (CascadingActionResult)_target.ByTownForFacilityAssetType(town.Id);
            var actual = result.GetSelectListItems();

            Assert.AreEqual(1, actual.Count() - 1);
        }

        [TestMethod]
        public void TestByTownId()
        {
            var town = GetEntityFactory<Town>().Create();
            var invalid = GetEntityFactory<Town>().Create();
            
            var functionalLocation = GetFactory<EquipmentFunctionalLocationFactory>().Create(new { Town = town });
            var functionalLocation1 = GetFactory<ValveFunctionalLocationFactory>().Create(new { Town = town });
            var functionalLocation2 = GetFactory<ValveFunctionalLocationFactory>().Create(new { Town = town });
            var functionalLocationInvalid = GetFactory<ValveFunctionalLocationFactory>().Create(new { Town = invalid });

            var result = (CascadingActionResult)_target.ByTownId(town.Id);
            var actual = result.GetSelectListItems();

            Assert.AreEqual(3, actual.Count() - 1);
            Assert.AreEqual(functionalLocation2.Id.ToString(), actual.Last().Value);
        }

        [TestMethod]
        public void TestByTownIdForSewerOpenings()
        {
            var town = GetEntityFactory<Town>().Create();
            var assetType = GetFactory<SewerOpeningAssetTypeFactory>().Create();
            var invalid = GetEntityFactory<Town>().Create();

            var functionalLocation = GetFactory<EquipmentFunctionalLocationFactory>().Create(new { Town = town, AssetType = assetType });
            var functionalLocation1 = GetFactory<ValveFunctionalLocationFactory>().Create(new { Town = town, AssetType = assetType });
            var functionalLocation2 = GetFactory<ValveFunctionalLocationFactory>().Create(new { Town = town, AssetType = assetType });
            var functionalLocationInvalid = GetFactory<ValveFunctionalLocationFactory>().Create(new { Town = invalid, AssetType = assetType });

            var result = (CascadingActionResult)_target.ActiveByTownIdForSewerOpeningAssetType(town.Id);
            var actual = result.GetSelectListItems();

            Assert.AreEqual(3, actual.Count() - 1);
            Assert.AreEqual(functionalLocation2.Id.ToString(), actual.Last().Value);
        }

        [TestMethod]
        public void TestByTownIdForHydrants()
        {
            var town = GetEntityFactory<Town>().Create();
            var assetType = GetFactory<HydrantAssetTypeFactory>().Create();
            var invalid = GetEntityFactory<Town>().Create();

            var functionalLocation = GetFactory<EquipmentFunctionalLocationFactory>().Create(new { Town = town, AssetType = assetType });
            var functionalLocation1 = GetFactory<ValveFunctionalLocationFactory>().Create(new { Town = town, AssetType = assetType });
            var functionalLocation2 = GetFactory<ValveFunctionalLocationFactory>().Create(new { Town = town, AssetType = assetType });
            var functionalLocationInvalid = GetFactory<ValveFunctionalLocationFactory>().Create(new { Town = invalid, AssetType = assetType });

            var result = (CascadingActionResult)_target.ActiveByTownIdForHydrantAssetType(town.Id);
            var actual = result.GetSelectListItems();

            Assert.AreEqual(3, actual.Count() - 1);
            Assert.AreEqual(functionalLocation2.Id.ToString(), actual.Last().Value);
        }

        [TestMethod]
        public void TestByTownIdForValvesDoesNotReturnForHydrants()
        {
            var town = GetEntityFactory<Town>().Create();
            var assetType = GetFactory<ValveAssetTypeFactory>().Create();
            var hydrantAssetType = GetFactory<HydrantAssetTypeFactory>().Create();
            var valveControls = GetFactory<SomeValveControlFactory>().Create();
            var invalid = GetEntityFactory<Town>().Create();

            var functionalLocation = GetFactory<EquipmentFunctionalLocationFactory>().Create(new { Town = town, AssetType = assetType });
            var functionalLocation1 = GetFactory<ValveFunctionalLocationFactory>().Create(new { Town = town, AssetType = assetType });
            var functionalLocation2 = GetFactory<ValveFunctionalLocationFactory>().Create(new { Town = town, AssetType = assetType });
            var functionalLocation11 = GetFactory<ValveFunctionalLocationFactory>().Create(new { Town = town, AssetType = hydrantAssetType });
            var functionalLocation12 = GetFactory<ValveFunctionalLocationFactory>().Create(new { Town = town, AssetType = hydrantAssetType});
            var functionalLocationInvalid = GetFactory<ValveFunctionalLocationFactory>().Create(new { Town = invalid, AssetType = assetType });

            var result = (CascadingActionResult)_target.ActiveByTownIdForValveAssetType(town.Id,valveControls.Id);
            var actual = result.GetSelectListItems();

            Assert.AreEqual(3, actual.Count() - 1);
            Assert.AreEqual(functionalLocation2.Id.ToString(), actual.Last().Value);
        }

        [TestMethod]
        public void TestByTownIdForValvesAlsoReturnsHydrants()
        {
            var town = GetEntityFactory<Town>().Create();
            var assetType = GetFactory<ValveAssetTypeFactory>().Create();
            var hydrantAssetType = GetFactory<HydrantAssetTypeFactory>().Create();
            var valveControls = GetFactory<BlowOffWithFlushingValveControlFactory>().Create();
            var invalid = GetEntityFactory<Town>().Create();

            var functionalLocation = GetFactory<EquipmentFunctionalLocationFactory>().Create(new { Town = town, AssetType = assetType });
            var functionalLocation1 = GetFactory<ValveFunctionalLocationFactory>().Create(new { Town = town, AssetType = assetType });
            var functionalLocation2 = GetFactory<ValveFunctionalLocationFactory>().Create(new { Town = town, AssetType = assetType });
            var functionalLocation11 = GetFactory<ValveFunctionalLocationFactory>().Create(new { Town = town, AssetType = hydrantAssetType });
            var functionalLocation12 = GetFactory<ValveFunctionalLocationFactory>().Create(new { Town = town, AssetType = hydrantAssetType });
            var functionalLocationInvalid = GetFactory<ValveFunctionalLocationFactory>().Create(new { Town = invalid, AssetType = assetType });

            var result = (CascadingActionResult)_target.ActiveByTownIdForValveAssetType(town.Id, valveControls.Id);
            var actual = result.GetSelectListItems();

            Assert.AreEqual(5, actual.Count() - 1);
            Assert.AreEqual(functionalLocation12.Id.ToString(), actual.Last().Value);
        }

        [TestMethod]
        public void TestByTownIdForValvesDoesNotReturnResults()
        {
            var result = (CascadingActionResult)_target.ActiveByTownIdForValveAssetType(null, null);
            var actual = result.GetSelectListItems();

            Assert.AreEqual(0, actual.Count());
        }

        [TestMethod]
        public void TestByFacilityId()
        {
            var assetType = GetFactory<EquipmentAssetTypeFactory>().Create();
            var town = GetEntityFactory<Town>().Create();
            var facility = GetEntityFactory<Facility>().Create(new { Town = town });
            var functionalLocation = GetFactory<EquipmentFunctionalLocationFactory>().Create(new { AssetType = assetType, Town = town });

            var result = (CascadingActionResult)_target.ByFacilityId(facility.Id);
            var actual = result.GetSelectListItems();

            Assert.AreEqual(1, actual.Count() - 1);
        }

        [TestMethod]
        public void TestByTownForSewerOpenings()
        {
            var town = GetEntityFactory<Town>().Create();
            var invalid = GetEntityFactory<Town>().Create();

            var functionalLocation1 = GetFactory<SewerMainFunctionalLocationFactory>().Create(new { Town = town });
            var functionalLocation2 = GetFactory<SewerOpeningFunctionalLocationFactory>().Create(new { Town = town });
            var functionalLocationInvalid1 = GetFactory<ValveFunctionalLocationFactory>().Create(new { Town = invalid });
            var functionalLocationInvalid2 = GetFactory<SewerMainFunctionalLocationFactory>().Create(new { Town = invalid });

            var result = (CascadingActionResult)_target.ByTownForSewerOpenings(town.Id);
            var actual = result.GetSelectListItems();

            Assert.AreEqual(2, actual.Count() - 1);
        }
        
        #endregion

        #region Edit/Update

        [TestMethod]
        public void TestEditSetsOperatingCenterToTownOperatingCenter()
        {
            var op = GetFactory<OperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = op, Town = town});
            var eq = GetFactory<FunctionalLocationFactory>().Create(new { Town = town });

            var result = _target.Edit(eq.Id) as ViewResult;
            var model = (EditFunctionalLocation)result.Model;

            Assert.AreEqual(op.Id, model.OperatingCenter);
        }

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetFactory<FunctionalLocationFactory>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditFunctionalLocation, FunctionalLocation>(eq, new {
                Description = expected
            }));

            Assert.AreEqual(expected, Session.Get<FunctionalLocation>(eq.Id).Description);
        }

        #endregion
    }
}
