using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.HealthAndSafety.Controllers;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.HealthAndSafety.Controllers
{
    [TestClass]
    public class GasMonitorControllerTest : MapCallMvcControllerTestBase<GasMonitorController, GasMonitor, GasMonitorRepository>
    {
        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateGasMonitor)vm;
                model.OperatingCenter = GetEntityFactory<OperatingCenter>().Create().Id;
            };
            options.InitializeUpdateViewModel = (vm) => {
                var model = (EditGasMonitor)vm;
                model.OperatingCenter = GetEntityFactory<OperatingCenter>().Create().Id;
            };
        }

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.OperationsLockoutForms;

            Authorization.Assert(a => {
                a.RequiresRole("~/HealthAndSafety/GasMonitor/Search/", role);
                a.RequiresRole("~/HealthAndSafety/GasMonitor/Show/", role);
                a.RequiresRole("~/HealthAndSafety/GasMonitor/Index/", role);
                a.RequiresRole("~/HealthAndSafety/GasMonitor/New/", role, RoleActions.Add);
                a.RequiresRole("~/HealthAndSafety/GasMonitor/Create/", role, RoleActions.Add);
                a.RequiresRole("~/HealthAndSafety/GasMonitor/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/HealthAndSafety/GasMonitor/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/HealthAndSafety/GasMonitor/Destroy/", role, RoleActions.Delete);
                a.RequiresRole("~/HealthAndSafety/GasMonitor/AddGasMonitorCalibration/", role, RoleActions.Edit);
                a.RequiresRole("~/HealthAndSafety/GasMonitor/RemoveGasMonitorCalibration/", role, RoleActions.Edit);
            });
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var oc = GetEntityFactory<OperatingCenter>().Create(new {OperatingCenterCode = "Test"});
            var eq = GetEntityFactory<Equipment>().Create(new {OperatingCenter = oc});
            var entity0 = GetFactory<GasMonitorFactory>().Create(new {Equipment = eq});
            var search = new SearchGasMonitor();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = (ExcelResult)_target.Index(search);

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var entity = GetEntityFactory<GasMonitor>().Create();

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditGasMonitor, GasMonitor>(entity, new {
                CalibrationFrequencyDays = 424
            }));

            Assert.AreEqual(424, Session.Get<GasMonitor>(entity.Id).CalibrationFrequencyDays);
        }

        #endregion

        #region AddGasMonitorCalibration

        [TestMethod]
        public void TestAddGasMonitorCalibrationRedirectsToGasMonitorShowPageOnSuccess()
        {
            var gasMonitor = GetEntityFactory<GasMonitor>().Create();
            var addVm = _viewModelFactory.Build<AddGasMonitorCalibration>();
            addVm.Id = gasMonitor.Id;
            addVm.ViewModel = _viewModelFactory.Build<GasMonitorCalibrationViewModel>();
            addVm.ViewModel.CalibrationDate = DateTime.Now;
            addVm.ViewModel.CalibrationPassed = true;

            var result = _target.AddGasMonitorCalibration(addVm);

            MvcAssert.RedirectsToRoute(result, new {action = "Show", controller = "GasMonitor", id = gasMonitor.Id});
        }

        #endregion

    }
}
