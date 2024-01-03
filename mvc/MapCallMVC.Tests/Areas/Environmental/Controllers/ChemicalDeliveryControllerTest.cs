using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Environmental.Controllers;
using MapCallMVC.Areas.Environmental.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Environmental.Controllers
{
    [TestClass]
    public class ChemicalDeliveryControllerTest : MapCallMvcControllerTestBase<ChemicalDeliveryController, ChemicalDelivery>
    {
        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = ChemicalDeliveryController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/Environmental/ChemicalDelivery/Search/", role);
                a.RequiresRole("~/Environmental/ChemicalDelivery/Show/", role);
                a.RequiresRole("~/Environmental/ChemicalDelivery/Index/", role);
                a.RequiresRole("~/Environmental/ChemicalDelivery/New/", role, RoleActions.Add);
                a.RequiresRole("~/Environmental/ChemicalDelivery/Create/", role, RoleActions.Add);
                a.RequiresRole("~/Environmental/ChemicalDelivery/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/Environmental/ChemicalDelivery/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/Environmental/ChemicalDelivery/Destroy/", role, RoleActions.Delete);
			});
		}				

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<ChemicalDelivery>().Create(new {ConfirmationInformation = "description 0"});
            var entity1 = GetEntityFactory<ChemicalDelivery>().Create(new {ConfirmationInformation = "description 1"});
            var search = new SearchChemicalDelivery();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.ConfirmationInformation, "ConfirmationInformation");
                helper.AreEqual(entity1.ConfirmationInformation, "ConfirmationInformation", 1);
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<ChemicalDelivery>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditChemicalDelivery, ChemicalDelivery>(eq, new {
                ConfirmationInformation = expected
            }));

            Assert.AreEqual(expected, Session.Get<ChemicalDelivery>(eq.Id).ConfirmationInformation);
        }
     
        #endregion
	}
}
