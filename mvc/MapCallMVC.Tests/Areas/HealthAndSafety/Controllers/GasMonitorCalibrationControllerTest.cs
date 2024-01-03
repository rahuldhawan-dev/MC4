using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.HealthAndSafety.Controllers;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.HealthAndSafety.Controllers
{
    [TestClass]
    public class GasMonitorCalibrationControllerTest : MapCallMvcControllerTestBase<GasMonitorCalibrationController, GasMonitorCalibration>
    {
        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.OperationsLockoutForms;

            Authorization.Assert(a =>
            {
                a.RequiresRole("~/HealthAndSafety/GasMonitorCalibration/Search/", role);
                a.RequiresRole("~/HealthAndSafety/GasMonitorCalibration/Show/", role);
                a.RequiresRole("~/HealthAndSafety/GasMonitorCalibration/Index/", role);
                a.RequiresRole("~/HealthAndSafety/GasMonitorCalibration/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/HealthAndSafety/GasMonitorCalibration/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/HealthAndSafety/GasMonitorCalibration/Destroy/", role, RoleActions.Delete);
            });
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetFactory<GasMonitorCalibrationFactory>().Create();
            var search = new SearchGasMonitorCalibration();
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
            var entity = GetEntityFactory<GasMonitorCalibration>().Create();

            _target.Update(_viewModelFactory.BuildWithOverrides<EditGasMonitorCalibration, GasMonitorCalibration>(entity, new
            {
                CalibrationFailedNotes = "Blippity blop"
            }));

            Assert.AreEqual("Blippity blop", Session.Get<GasMonitorCalibration>(entity.Id).CalibrationFailedNotes);
        }

        #endregion
    }
}
