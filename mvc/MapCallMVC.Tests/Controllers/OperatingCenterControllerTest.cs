using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MMSINC;
using MMSINC.Data;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class OperatingCenterControllerTest : MapCallMvcControllerTestBase<OperatingCenterController, OperatingCenter, OperatingCenterRepository>
    {
        #region Init/Cleanup

        protected override User CreateUser()
        {
            // creating a user here from the factory will create an extra operating center.
            return null;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _container.Inject<IOperatingCenterRepository>(Repository);
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeUpdateViewModel = (vm) => {
                var model = (EditOperatingCenter)vm;
                model.HydrantPaintingFrequency = 1;
                model.HydrantPaintingFrequencyUnit = GetEntityFactory<RecurringFrequencyUnit>().Create().Id;
            };
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.FieldServicesDataLookups;
                a.RequiresRole("~/OperatingCenter/Show/", module);
                a.RequiresRole("~/OperatingCenter/Index/", module);

                a.RequiresSiteAdminUser("~/OperatingCenter/Edit/");
                a.RequiresSiteAdminUser("~/OperatingCenter/Update/");
                a.RequiresSiteAdminUser("~/OperatingCenter/AddAssetType/");
                a.RequiresSiteAdminUser("~/OperatingCenter/RemoveAssetType/");
                a.RequiresSiteAdminUser("~/OperatingCenter/AddWaterSystem/");
                a.RequiresSiteAdminUser("~/OperatingCenter/RemoveWaterSystem/");

                a.RequiresLoggedInUserOnly("~/OperatingCenter/ByStateId/");
                a.RequiresLoggedInUserOnly("~/OperatingCenter/ByStateIdOrAll/");
                a.RequiresLoggedInUserOnly("~/OperatingCenter/ByStateIds/");
                a.RequiresLoggedInUserOnly("~/OperatingCenter/GetStateIdByOperatingCenterId/");
                a.RequiresLoggedInUserOnly("~/OperatingCenter/WorkOrdersEnabledByStateId/");
                a.RequiresLoggedInUserOnly("~/OperatingCenter/ByStateIdForFieldServicesAssets/");
                a.RequiresLoggedInUserOnly("~/OperatingCenter/ActiveByStateIdForFieldServicesAssets/");
                a.RequiresLoggedInUserOnly("~/OperatingCenter/ByStateIdForFieldServicesWorkManagement");
                a.RequiresLoggedInUserOnly("~/OperatingCenter/ActiveByStateIdForEngineeringRiskRegisterAssets");
                a.RequiresLoggedInUserOnly("~/OperatingCenter/ByStateIdForEngineeringRiskRegisterAssets");
                a.RequiresLoggedInUserOnly("~/OperatingCenter/ByStateIdForProductionWorkManagement");
                a.RequiresLoggedInUserOnly("~/OperatingCenter/ByStateIdForProductionEquipment");
                a.RequiresLoggedInUserOnly("~/OperatingCenter/ActiveByStateIdOrAll/");
                a.RequiresLoggedInUserOnly("~/OperatingCenter/ByStateIdForEnvironmentalGeneral/");
                a.RequiresLoggedInUserOnly("~/OperatingCenter/ByStateIdForEnvironmentalChemicalData/");
                a.RequiresLoggedInUserOnly("~/OperatingCenter/ByStateIdForWaterQualityGeneral/");
                a.RequiresLoggedInUserOnly("~/OperatingCenter/ByStateIdForHealthAndSafety/");
                a.RequiresLoggedInUserOnly("~/OperatingCenter/ActiveByStateIdForHealthAndSafety/");
                a.RequiresLoggedInUserOnly("~/OperatingCenter/ByStateIdForHumanResourcesEmployeeLimited/");
                a.RequiresLoggedInUserOnly("~/OperatingCenter/GetInfoMasterInfo/");
                a.RequiresLoggedInUserOnly("~/OperatingCenter/ActiveByTownId/");
                a.RequiresLoggedInUserOnly("~/OperatingCenter/ByTownId/");
                a.RequiresLoggedInUserOnly("~/OperatingCenter/GetStateByOperatingCenterId/");
                a.RequiresLoggedInUserOnly("~/OperatingCenter/IsContractedOperations/");
                a.RequiresLoggedInUserOnly("~/OperatingCenter/IsSAPEnabled/");
                a.RequiresLoggedInUserOnly("~/OperatingCenter/IsSAPWorkOrdersEnabled/");
                a.RequiresLoggedInUserOnly("~/OperatingCenter/GetByPublicWaterSupplyForWaterQuality/");
                a.RequiresLoggedInUserOnly("~/OperatingCenter/GetByPublicWaterSuppliesForWaterQuality/");
                a.RequiresLoggedInUserOnly("~/OperatingCenter/ByStateIdAndContracted/"); 
                a.RequiresLoggedInUserOnly("~/OperatingCenter/ByStateIdOrIsContractedOperationsForHumanResourcesCovid/"); 
                a.RequiresLoggedInUserOnly("~/OperatingCenter/ByStateIdOrIsContractedOperationsForHealthAndSafetyNearMiss/");
                a.RequiresLoggedInUserOnly("~/OperatingCenter/ByStateIdForHealthAndSafetyLockoutForm/");
                a.RequiresLoggedInUserOnly("~/OperatingCenter/ActiveByStateIdsOrAll/");
                a.RequiresLoggedInUserOnly("~/OperatingCenter/ByStateIdForProductionFacilities");
            });
        }

        #region Index

        [TestMethod]
        public void TestIndexRespondsWithExcelFileOfAllOperatingCentersWhenRouteIsForExcelFiles()
        {
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ResponseFormatter.KnownExtensions.EXCEL_2003;
            var expectedNJ4 = GetFactory<OperatingCenterFactory>().Create(new { OperatingCenterCode = "NJ4", OperatingCenterName = "Some Operating Center Name" });
            var expectedQQ2 = GetFactory<OperatingCenterFactory>().Create(new { OperatingCenterCode = "QQ2" });

            var result = (ExcelResult)_target.Index();
            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(expectedNJ4.Id, "Id");
                helper.AreEqual(expectedNJ4.OperatingCenterCode, "OperatingCenterCode");
                helper.AreEqual(expectedNJ4.OperatingCenterName, "OperatingCenterName");

                helper.AreEqual(expectedQQ2.Id, "Id", 1);
                helper.AreEqual(expectedQQ2.OperatingCenterCode, "OperatingCenterCode", 1);
                helper.AreEqual(expectedQQ2.OperatingCenterName, "OperatingCenterName", 1);
            }
        }

        #endregion

        #region Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var opc = GetFactory<OperatingCenterFactory>().Create();
            var viewModel = _viewModelFactory.Build<EditOperatingCenter, OperatingCenter>(opc);
            viewModel.OperatingCenterName = "WHOA";

            _target.Update(viewModel);

            Session.Evict(opc);

            var opcAgain = Session.Query<OperatingCenter>().Single(x => x.Id == opc.Id);

            Assert.AreEqual("WHOA", opcAgain.OperatingCenterName);
        }

        [TestMethod]
        public void TestUpdateResetsOperatingCenterCodeOnViewModelIfModelStateIsInvalid()
        {
            var opc = GetFactory<OperatingCenterFactory>().Create(new { OperatingCenterCode = "YUP" });
            var viewModel = _viewModelFactory.Build<EditOperatingCenter, OperatingCenter>(opc);

            // OpCenterCode will be null on postback, so simulate that.
            viewModel.OperatingCenterCode = null;
            _target.ModelState.AddModelError("nope", "nope.avi");
            _target.Update(viewModel);

            Assert.AreEqual("YUP", viewModel.OperatingCenterCode);
        }

        [TestMethod]
        public void TestUpdateDoesNotResetOperatingCenterCodeIfTheOperatingCenterDoesNotExist()
        {
            var opc = GetFactory<OperatingCenterFactory>().Create(new { OperatingCenterCode = "YUP" });
            var viewModel = _viewModelFactory.Build<EditOperatingCenter, OperatingCenter>(opc);
            viewModel.Id = opc.Id + 1;

            // OpCenterCode will be null on postback, so simulate that.
            viewModel.OperatingCenterCode = null;
            _target.Update(viewModel);

            Assert.IsNull(viewModel.OperatingCenterCode);
        }

        #endregion

        #region GetInfoMasterInfo

        [TestMethod]
        public void TestGetInfoMasterInfoReturnsInfoMasterInfo()
        {
            var opc = GetFactory<OperatingCenterFactory>().Create(new { InfoMasterMapId = "foo", InfoMasterMapLayerName = "bar"});

            var result = _target.GetInfoMasterInfo(opc.Id) as JsonResult;

            Assert.IsNotNull(result);
            var resultData = (dynamic)result.Data;
            
            Assert.AreEqual(opc.InfoMasterMapId, resultData.InfoMasterMapId);
            Assert.AreEqual(opc.InfoMasterMapLayerName, resultData.InfoMasterMapLayerName);
        }

        #endregion

        #region ByStateIdOrAll

        [TestMethod]
        public void TestByStateIdOrAllReturnsOperatingCentersByStateWhenStateIdIsSupplied()
        {
            var state1 = GetFactory<StateFactory>().Create(new{ Abbreviation = "ZZ"});
            var state2 = GetFactory<StateFactory>().Create(new{ Abbreviation = "QQ"});
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = state1 });
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = state2 });

            var result = (CascadingActionResult)_target.ByStateIdOrAll(state1.Id);
            var data = (IEnumerable<OperatingCenterDisplayItem>)result.Data;
            Assert.AreEqual(1, data.Count());
            Assert.AreEqual(opc1.Id, data.Single().Id);
        }

        [TestMethod]
        public void TestByStateIdOrAllReturnsAllOperatingCentersWhenStateIdIsNotSupplied()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create();

            var result = (CascadingActionResult)_target.ByStateIdOrAll(null);
            var data = (IEnumerable<OperatingCenterDisplayItem>)result.Data;
            Assert.AreEqual(2, data.Count());
            Assert.IsTrue(data.Any(x => x.Id == opc1.Id));
            Assert.IsTrue(data.Any(x => x.Id == opc2.Id));
        }

        [TestMethod]
        public void TestActiveByStateIdOrAllOrAllReturnsOnlyActiveOperatingCentersWhenStateIdIsNotNull()
        {
            var state1 = GetEntityFactory<State>().Create(new { Abbreviation = "NJ" });
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new{IsActive = true, State = state1});
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create(new{IsActive = false, State = state1});
            
            var result = (CascadingActionResult)_target.ActiveByStateIdOrAll(state1.Id);
            var data = (IEnumerable<OperatingCenterDisplayItem>)result.Data;

            Assert.AreEqual(1, data.Count());
            Assert.IsTrue(data.Any(x => x.Id == opc1.Id));
            Assert.IsFalse(data.Any(x => x.Id == opc2.Id));
        }

        [TestMethod]
        public void TestActiveByStateIdOrAllReturnsOnlyActiveOperatingCentersWhenStateIdIsNull()
        {
            var state1 = GetEntityFactory<State>().Create(new { Abbreviation = "NJ" });
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = true, State = state1 });
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = false, State = state1 });
            
            var result = (CascadingActionResult)_target.ActiveByStateIdOrAll(null);
            var data = (IEnumerable<OperatingCenterDisplayItem>)result.Data;

            Assert.AreEqual(1, data.Count());
            Assert.IsTrue(data.Any(x => x.Id == opc1.Id));
            Assert.IsFalse(data.Any(x => x.Id == opc2.Id));
        }

        [TestMethod]
        public void TestByStateIdForHealthAndSafetyReturnsOnlyActiveOperatingCentersWhenStateIdIsNull()
        {
            var state1 = GetEntityFactory<State>().Create(new { Abbreviation = "NJ" });
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = state1 });
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = state1 });
            var user = GetEntityFactory<User>().Create(new { DefaultOperatingCenter = opc1 });
            GetFactory<RoleFactory>().Create(new {
                Application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.Operations }),
                Module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.OperationsHealthAndSafety }),
                Action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read }),
                OperatingCenter = opc1,
                User = user
            });
            Session.Save(user);
            _authenticationService.Setup(x => x.CurrentUser).Returns(user);

            var result = (CascadingActionResult)_target.ByStateIdForHealthAndSafety(null);
            var data = (IEnumerable<OperatingCenterDisplayItem>)result.Data;

            Assert.AreEqual(1, data.Count());
        }

        [TestMethod]
        public void TestByStateIdForProductionEquipmentReturnsOnlyByStateIdForRole()
        {
            var state1 = GetEntityFactory<State>().Create(new { Abbreviation = "NJ" });
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = state1 });
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = state1 });
            var user = GetEntityFactory<User>().Create(new { DefaultOperatingCenter = opc1 });
            GetFactory<RoleFactory>().Create(new {
                Application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.Operations }),
                Module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.ProductionEquipment }),
                Action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read }),
                OperatingCenter = opc1,
                User = user
            });
            Session.Save(user);
            _authenticationService.Setup(x => x.CurrentUser).Returns(user);

            var result = (CascadingActionResult)_target.ByStateIdForProductionEquipment(null);
            var data = (IEnumerable<OperatingCenterDisplayItem>)result.Data;

            Assert.AreEqual(1, data.Count());
        }

        [TestMethod]
        public void TestByStateIdForWaterQualityGeneralReturnsOnlyByStateIdForRole()
        {
            var newJerseyState = GetEntityFactory<State>().Create(new { Abbreviation = "NJ" });
            var operatingCenter1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = newJerseyState });
            var operatingCenter2 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = newJerseyState });
            var user = GetEntityFactory<User>().Create(new { DefaultOperatingCenter = operatingCenter1 });
            GetFactory<RoleFactory>().Create(new {
                Application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.Operations }),
                Module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.WaterQualityGeneral }),
                Action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read }),
                OperatingCenter = operatingCenter1,
                User = user
            });
            Session.Save(user);
            _authenticationService.Setup(x => x.CurrentUser).Returns(user);

            var result = (CascadingActionResult)_target.ByStateIdForWaterQualityGeneral(null);
            var operatingCenterDisplayItems = ((IEnumerable<OperatingCenterDisplayItem>)result.Data).ToList();

            Assert.AreEqual(1, operatingCenterDisplayItems.Count);
            Assert.AreEqual(operatingCenter1.Id, operatingCenterDisplayItems.First().Id);
        }

        [TestMethod]
        public void TestByStateIdForHumanResourcesCovidReturnsOnlyByStateIdForRole()
        {
            var state1 = GetEntityFactory<State>().Create(new { Abbreviation = "NJ" });
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = state1 });
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = state1 });
            var user = GetEntityFactory<User>().Create(new { DefaultOperatingCenter = opc1 });
            GetFactory<RoleFactory>().Create(new {
                Application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.HumanResources }),
                Module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.HumanResourcesCovid }),
                Action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read }),
                OperatingCenter = opc1,
                User = user
            });
            Session.Save(user);
            _authenticationService.Setup(x => x.CurrentUser).Returns(user);

            var result = (CascadingActionResult)_target.ByStateIdOrIsContractedOperationsForHumanResourcesCovid(null, null);
            var data = (IEnumerable<OperatingCenterDisplayItem>)result.Data;

            Assert.AreEqual(1, data.Count());
        }

        [TestMethod]
        public void TestByStateIdForHealthAndSafetyNearMissReturnsOnlyByStateIdForRole()
        {
            var state1 = GetEntityFactory<State>().Create(new { Abbreviation = "NJ" });
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = state1 });
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = state1 });
            var user = GetEntityFactory<User>().Create(new { DefaultOperatingCenter = opc1 });
            GetFactory<RoleFactory>().Create(new {
                Application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.Operations }),
                Module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.OperationsHealthAndSafety }),
                Action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read }),
                OperatingCenter = opc1,
                User = user
            });
            Session.Save(user);
            _authenticationService.Setup(x => x.CurrentUser).Returns(user);

            var result = (CascadingActionResult)_target.ByStateIdOrIsContractedOperationsForHealthAndSafetyNearMiss(null, null);
            var data = (IEnumerable<OperatingCenterDisplayItem>)result.Data;

            Assert.AreEqual(1, data.Count());
        }

        [TestMethod]
        public void TestByStateIdForHealthAndSafetyLockoutFormReturnsOnlyByStateIdForRole()
        {
            var state1 = GetFactory<StateFactory>().Create(new { Abbreviation = "ZZ" });
            var state2 = GetFactory<StateFactory>().Create(new { Abbreviation = "QQ" });
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = state1 });
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = state2 });
            var user = GetEntityFactory<User>().Create(new { DefaultOperatingCenter = opc1 });
            var createRole = new Action<OperatingCenter, User>((opCntr, usr) => {
                GetFactory<RoleFactory>().Create(new {
                    Application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.Operations }),
                    Module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.OperationsHealthAndSafety }),
                    Action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read }),
                    OperatingCenter = opCntr,
                    User = usr
                });
            });
            createRole(opc1, user);
            createRole(opc2, user);
            Session.Save(user);
            _authenticationService.Setup(x => x.CurrentUser).Returns(user);

            var result = (CascadingActionResult)_target.ByStateIdForHealthAndSafetyLockoutForm(state2.Id);
            var data = (IEnumerable<OperatingCenterDisplayItem>)result.Data;

            Assert.AreEqual(1, data.Count());
            Assert.IsTrue(data.Any(x => x.Id == opc2.Id));
        }

        [TestMethod]
        public void TestByStateIdForHealthAndSafetyLockoutFormWhenStateIdWithDifferentRole()
        {
            var state1 = GetFactory<StateFactory>().Create(new { Abbreviation = "ZZ" });
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = state1 });
            var user = GetEntityFactory<User>().Create(new { DefaultOperatingCenter = opc1 });
            var createRole = new Action<OperatingCenter, User>((opCntr, usr) => {
                GetFactory<RoleFactory>().Create(new {
                    Application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.Operations }),
                    Module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.OperationsDistributionOnly }),
                    Action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read }),
                    OperatingCenter = opCntr,
                    User = usr
                });
            });
            createRole(opc1, user);
            Session.Save(user);
            _authenticationService.Setup(x => x.CurrentUser).Returns(user);

            var result = (CascadingActionResult)_target.ByStateIdForHealthAndSafetyLockoutForm(state1.Id);
            var data = (IEnumerable<OperatingCenterDisplayItem>)result.Data;

            Assert.AreEqual(0, data.Count());
        }

        [TestMethod]
        public void TestByStateIdForHealthAndSafetyLockoutFormWhenStateIdIsNull()
        {
            var state1 = GetFactory<StateFactory>().Create(new { Abbreviation = "ZZ" });
            var state2 = GetFactory<StateFactory>().Create(new { Abbreviation = "QQ" });
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = state1 });
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = state2 });
            var opc3 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = state2 });
            var user = GetEntityFactory<User>().Create(new { DefaultOperatingCenter = opc1 });
            var createRole = new Action<OperatingCenter, User>((opCntr, usr) => {
                GetFactory<RoleFactory>().Create(new {
                    Application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.Operations }),
                    Module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.OperationsHealthAndSafety }),
                    Action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read }),
                    OperatingCenter = opCntr,
                    User = usr
                });
            });
            createRole(opc1, user);
            createRole(opc2, user);
            createRole(opc3, user);
            Session.Save(user);
            _authenticationService.Setup(x => x.CurrentUser).Returns(user);

            var result = (CascadingActionResult)_target.ByStateIdForHealthAndSafetyLockoutForm(null);
            var data = (IEnumerable<OperatingCenterDisplayItem>)result.Data;

            Assert.AreEqual(3, data.Count());
            Assert.IsTrue(data.Any(x => x.Id == opc1.Id));
            Assert.IsTrue(data.Any(x => x.Id == opc2.Id));
            Assert.IsTrue(data.Any(x => x.Id == opc3.Id));
        }

        [TestMethod]
        public void TestByStateIdForHealthAndSafetyLockoutFormWhenStateIdIsNullWithDifferentRole()
        {
            var state1 = GetFactory<StateFactory>().Create(new { Abbreviation = "ZZ" });
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = state1 });
            var user = GetEntityFactory<User>().Create(new { DefaultOperatingCenter = opc1 });
            var createRole = new Action<OperatingCenter, User>((opCntr, usr) => {
                GetFactory<RoleFactory>().Create(new {
                    Application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.FieldServices }),
                    Module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.FieldServicesReports }),
                    Action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read }),
                    OperatingCenter = opCntr,
                    User = usr
                });
            });
            createRole(opc1, user);
            Session.Save(user);
            _authenticationService.Setup(x => x.CurrentUser).Returns(user);

            var result = (CascadingActionResult)_target.ByStateIdForHealthAndSafetyLockoutForm(null);
            var data = (IEnumerable<OperatingCenterDisplayItem>)result.Data;

            Assert.AreEqual(0, data.Count());
        }

        [TestMethod]
        public void TestByStateIdForHumanResourcesCovidReturnsOnlyByStateIdAndIsContractedOperationsForRole()
        {
            var state1 = GetEntityFactory<State>().Create(new { Abbreviation = "NJ" });
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = state1, IsContractedOperations = true });
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = state1, IsContractedOperations = false });
            var opc3 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = state1, IsContractedOperations = true });
            var user = GetEntityFactory<User>().Create(new { DefaultOperatingCenter = opc1 });
            var createRole = new Action<OperatingCenter, User>((opCntr, usr) => {
                GetFactory<RoleFactory>().Create(new {
                    Application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.HumanResources}),
                    Module = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.HumanResourcesCovid}),
                    Action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read}),
                    OperatingCenter = opCntr,
                    User = usr
                });
            });
            createRole(opc1, user);
            createRole(opc2, user);
            createRole(opc3, user);
            Session.Save(user);
            _authenticationService.Setup(x => x.CurrentUser).Returns(user);

            var result = (CascadingActionResult)_target.ByStateIdOrIsContractedOperationsForHumanResourcesCovid(null, true);
            var data = (IEnumerable<OperatingCenterDisplayItem>)result.Data;

            Assert.AreEqual(2, data.Count());
            Assert.IsTrue(data.Any(x => x.Id == opc1.Id));
            Assert.IsFalse(data.Any(x => x.Id == opc2.Id));
            Assert.IsTrue(data.Any(x => x.Id == opc3.Id));
        }

        [TestMethod]
        public void TestByStateIdForHealthAndSafetyNearMissReturnsOnlyByStateIdAndIsContractedOperationsForRole()
        {
            var state1 = GetEntityFactory<State>().Create(new { Abbreviation = "NJ" });
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = state1, IsContractedOperations = true });
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = state1, IsContractedOperations = false });
            var opc3 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = state1, IsContractedOperations = true });
            var user = GetEntityFactory<User>().Create(new { DefaultOperatingCenter = opc1 });
            var createRole = new Action<OperatingCenter, User>((opCntr, usr) => {
                GetFactory<RoleFactory>().Create(new {
                    Application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.Operations }),
                    Module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.OperationsHealthAndSafety }),
                    Action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read }),
                    OperatingCenter = opCntr,
                    User = usr
                });
            });
            createRole(opc1, user);
            createRole(opc2, user);
            createRole(opc3, user);
            Session.Save(user);
            _authenticationService.Setup(x => x.CurrentUser).Returns(user);

            var result = (CascadingActionResult)_target.ByStateIdOrIsContractedOperationsForHealthAndSafetyNearMiss(null, true);
            var data = (IEnumerable<OperatingCenterDisplayItem>)result.Data;

            Assert.AreEqual(2, data.Count());
            Assert.IsTrue(data.Any(x => x.Id == opc1.Id));
            Assert.IsFalse(data.Any(x => x.Id == opc2.Id));
            Assert.IsTrue(data.Any(x => x.Id == opc3.Id));
        }

        [TestMethod]
        public void TestActiveByStateIdForFieldServicesAssetsReturnsOnlyActiveByStateIdForRole()
        {
            var newJerseyState = GetEntityFactory<State>().Create(new { Abbreviation = "NJ" });
            var operatingCenter1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = newJerseyState, IsActive = true });
            var operatingCenter2 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = newJerseyState, IsActive = true });
            var operatingCenter3 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = newJerseyState, IsActive = false });
            var user = GetEntityFactory<User>().Create(new { DefaultOperatingCenter = operatingCenter1 });
            GetFactory<RoleFactory>().Create(new {
                Application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.FieldServices }),
                Module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.FieldServicesAssets }),
                Action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read }),
                OperatingCenter = operatingCenter1,
                User = user
            });
            GetFactory<RoleFactory>().Create(new {
                Application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.FieldServices }),
                Module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.FieldServicesAssets }),
                Action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read }),
                OperatingCenter = operatingCenter3,
                User = user
            });
            Session.Save(user);
            _authenticationService.Setup(x => x.CurrentUser).Returns(user);

            var result = (CascadingActionResult)_target.ActiveByStateIdForFieldServicesAssets(null);
            var operatingCenterDisplayItems = ((IEnumerable<OperatingCenterDisplayItem>)result.Data).ToList();

            Assert.AreEqual(1, operatingCenterDisplayItems.Count);
            Assert.AreEqual(operatingCenter1.Id, operatingCenterDisplayItems.First().Id);
        }

        [TestMethod]
        public void TestActiveByStateIdsOrAllOrAllReturnsOnlyActiveOperatingCentersWhenStateIdsAreNotNull()
        {
            var state1 = GetEntityFactory<State>().Create(new { Abbreviation = "NJ" });
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = true, State = state1 });
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = false, State = state1 });

            var result = (CascadingActionResult)_target.ActiveByStateIdsOrAll(new []{ state1.Id });
            var data = (IEnumerable<OperatingCenterDisplayItem>)result.Data;

            Assert.AreEqual(1, data.Count());
            Assert.IsTrue(data.Any(x => x.Id == opc1.Id));
            Assert.IsFalse(data.Any(x => x.Id == opc2.Id));
        }

        [TestMethod]
        public void TestActiveByStateIdsOrAllReturnsOnlyActiveOperatingCentersWhenStateIdsAreNull()
        {
            var state1 = GetEntityFactory<State>().Create(new { Abbreviation = "NJ" });
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = true, State = state1 });
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = false, State = state1 });

            var result = (CascadingActionResult)_target.ActiveByStateIdsOrAll(null);
            var data = (IEnumerable<OperatingCenterDisplayItem>)result.Data;

            Assert.AreEqual(1, data.Count());
            Assert.IsTrue(data.Any(x => x.Id == opc1.Id));
            Assert.IsFalse(data.Any(x => x.Id == opc2.Id));
        }

        [TestMethod]
        public void TestActiveByStateIdForEngineeringRiskRegisterAssetsReturnsOnlyActiveByStateIdForRole()
        {
            var newJerseyState = GetEntityFactory<State>().Create(new { Abbreviation = "NJ" });
            var operatingCenter1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = newJerseyState, IsActive = true });
            var operatingCenter2 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = newJerseyState, IsActive = false });
            var user = GetEntityFactory<User>().Create(new { DefaultOperatingCenter = operatingCenter1 });
            GetFactory<RoleFactory>().Create(new {
                Application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.FieldServices }),
                Module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.EngineeringRiskRegister }),
                Action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read }),
                OperatingCenter = operatingCenter1,
                User = user
            });
            GetFactory<RoleFactory>().Create(new {
                Application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.FieldServices }),
                Module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.EngineeringRiskRegister }),
                Action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read }),
                OperatingCenter = operatingCenter2,
                User = user
            });
            Session.Save(user);
            _authenticationService.Setup(x => x.CurrentUser).Returns(user);

            var result = (CascadingActionResult)_target.ActiveByStateIdForEngineeringRiskRegisterAssets(null);
            var operatingCenterDisplayItems = ((IEnumerable<OperatingCenterDisplayItem>)result.Data).ToList();

            Assert.AreEqual(1, operatingCenterDisplayItems.Count);
            Assert.AreEqual(operatingCenter1.Id, operatingCenterDisplayItems.First().Id);
        }

        [TestMethod]
        public void TestActiveByStateIdForHealthAndSafetyReturnsOnlyActiveByStateIdForRole()
        {
            var newJerseyState = GetEntityFactory<State>().Create(new { Abbreviation = "NJ" });
            var operatingCenter1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = newJerseyState, IsActive = true });
            var operatingCenter2 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = newJerseyState, IsActive = false });
            var user = GetEntityFactory<User>().Create(new { DefaultOperatingCenter = operatingCenter1 });
            GetFactory<RoleFactory>().Create(new {
                Application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.Operations }),
                Module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.OperationsHealthAndSafety }),
                Action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read }),
                OperatingCenter = operatingCenter1,
                User = user
            });
            GetFactory<RoleFactory>().Create(new {
                Application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.Operations }),
                Module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.OperationsHealthAndSafety }),
                Action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read }),
                OperatingCenter = operatingCenter2,
                User = user
            });
            Session.Save(user);
            _authenticationService.Setup(x => x.CurrentUser).Returns(user);

            var result = (CascadingActionResult)_target.ActiveByStateIdForHealthAndSafety(null);
            var operatingCenterDisplayItems = ((IEnumerable<OperatingCenterDisplayItem>)result.Data).ToList();

            Assert.AreEqual(1, operatingCenterDisplayItems.Count);
            Assert.AreEqual(operatingCenter1.Id, operatingCenterDisplayItems.First().Id);
        }

        [TestMethod]
        public void TestByStateIdForProductionFacilitiesReturnsOnlyByStateIdForRole()
        {
            var state1 = GetEntityFactory<State>().Create(new { Abbreviation = "NJ" });
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = state1 });
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = state1 });
            var user = GetEntityFactory<User>().Create(new { DefaultOperatingCenter = opc1 });
            GetFactory<RoleFactory>().Create(new {
                Application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.Production }),
                Module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.ProductionFacilities }),
                Action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read }),
                OperatingCenter = opc1,
                User = user
            });
            Session.Save(user);
            _authenticationService.Setup(x => x.CurrentUser).Returns(user);

            var result = (CascadingActionResult)_target.ByStateIdForProductionFacilities(null);
            var data = (IEnumerable<OperatingCenterDisplayItem>)result.Data;

            Assert.AreEqual(1, data.Count());
        }

        [TestMethod]
        public void TestByStateIdForEnvironmentalChemicalDataReturnsOperatingCentersOnlyByStateIdForRole()
        {
            var state1 = GetEntityFactory<State>().Create(new { Abbreviation = "NJ" });
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = state1 });
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = state1 });
            var user = GetEntityFactory<User>().Create(new { DefaultOperatingCenter = opc1 });
            GetFactory<RoleFactory>().Create(new {
                Application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.Environmental }),
                Module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.EnvironmentalChemicalData }),
                Action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read }),
                OperatingCenter = opc1,
                User = user
            });
            Session.Save(user);
            _authenticationService.Setup(x => x.CurrentUser).Returns(user);

            var result = (CascadingActionResult)_target.ByStateIdForEnvironmentalChemicalData(null);
            var data = (IEnumerable<OperatingCenterDisplayItem>)result.Data;

            Assert.AreEqual(1, data.Count());
        }

        #endregion

        #region ActiveByTownId

        [TestMethod]
        public void TestActiveByTownIdReturnsOnlyActiveOperatingCentersForTown()
        {
            var town1 = GetEntityFactory<Town>().Create();
            var activeOpc = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = true });
            var inactiveOpc = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = false });
            GetEntityFactory<OperatingCenterTown>().Create(new {
                OperatingCenter = activeOpc,
                Town = town1
            });
            GetEntityFactory<OperatingCenterTown>().Create(new {
                OperatingCenter = inactiveOpc,
                Town = town1
            });

            var result = (CascadingActionResult)_target.ActiveByTownId(town1.Id);
            var data = (IEnumerable<OperatingCenterDisplayItem>)result.Data;

            Assert.AreEqual(1, data.Count());
            Assert.IsTrue(data.Any(x => x.Id == activeOpc.Id));
            Assert.IsFalse(data.Any(x => x.Id == inactiveOpc.Id));
        }        

        #endregion

        #endregion
    }
}
