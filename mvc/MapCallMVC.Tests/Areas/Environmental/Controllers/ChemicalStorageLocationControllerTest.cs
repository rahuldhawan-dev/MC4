using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Environmental.Controllers;
using MapCallMVC.Areas.Environmental.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Environmental.Controllers
{
    [TestClass]
    public class ChemicalStorageLocationControllerTest : MapCallMvcControllerTestBase<ChemicalStorageLocationController, ChemicalStorageLocation>
    {
        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = ChemicalStorageLocationController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/Environmental/ChemicalStorageLocation/Search/", role);
                a.RequiresRole("~/Environmental/ChemicalStorageLocation/Show/", role);
                a.RequiresRole("~/Environmental/ChemicalStorageLocation/Index/", role);
                a.RequiresRole("~/Environmental/ChemicalStorageLocation/New/", role, RoleActions.Add);
                a.RequiresRole("~/Environmental/ChemicalStorageLocation/Create/", role, RoleActions.Add);
                a.RequiresRole("~/Environmental/ChemicalStorageLocation/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/Environmental/ChemicalStorageLocation/Update/", role, RoleActions.Edit);
                a.RequiresLoggedInUserOnly("~/Environmental/ChemicalStorageLocation/ByOperatingCenterId");
            });
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<ChemicalStorageLocation>().Create(new { IsActive = true });
            var entity1 = GetEntityFactory<ChemicalStorageLocation>().Create(new { IsActive = false });
            var search = new SearchChemicalStorageLocation();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.IsActive, "IsActive");
                helper.AreEqual(entity1.IsActive, "IsActive", 1);
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<ChemicalStorageLocation>().Create();
            var expected = "description field";

            _target.Update(_viewModelFactory.BuildWithOverrides<EditChemicalStorageLocation, ChemicalStorageLocation>(eq, x => {
                x.StorageLocationDescription = expected;
            }));

            Assert.AreEqual(expected, Session.Get<ChemicalStorageLocation>(eq.Id).StorageLocationDescription);
        }

        #endregion

        #region ByOperatingCenterId

        [TestMethod]
        public void TestByOperatingCenterIdReturnsCascadingResultForMatchingOperatingCenters()
        {
            _currentUser.IsAdmin = true;
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var goodChemicalStorageLocation = GetFactory<ChemicalStorageLocationFactory>().Create(new { OperatingCenter = opc });
            var badChemicalStorageLocation = GetFactory<ChemicalStorageLocationFactory>().Create();

            var result = (CascadingActionResult)_target.ByOperatingCenterId(opc.Id);
            var actual = result.GetSelectListItems().ToArray();

            Assert.AreEqual(1, actual.Count() - 1);
            Assert.AreEqual(goodChemicalStorageLocation.Id.ToString(), actual[1].Value);
        }

        #endregion
    }
}
