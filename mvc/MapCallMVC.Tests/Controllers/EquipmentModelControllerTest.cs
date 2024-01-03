using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
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
    public class EquipmentModelControllerTest : MapCallMvcControllerTestBase<EquipmentModelController, EquipmentModel>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IEquipmentModelRepository>().Use<EquipmentModelRepository>();
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.ProductionFacilities;
                a.RequiresRole("~/EquipmentModel/Search/", module, RoleActions.Read);
                a.RequiresRole("~/EquipmentModel/Show/", module, RoleActions.Read);
                a.RequiresRole("~/EquipmentModel/Index/", module, RoleActions.Read);
                a.RequiresRole("~/EquipmentModel/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/EquipmentModel/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/EquipmentModel/New/", module, RoleActions.Add);
                a.RequiresRole("~/EquipmentModel/Create/", module, RoleActions.Add);
                a.RequiresRole("~/EquipmentModel/ByManufacturerId/", module, RoleActions.Read);
            });
        }
 
        [TestMethod]
        public void TestByManufacturerIdReturnsCascadingActionResult()
        {
            var manufacturerValid = GetFactory<EquipmentManufacturerFactory>().Create();
            var manufacturerInvalid = GetFactory<EquipmentManufacturerFactory>().Create();
            var modelValid = GetFactory<EquipmentModelFactory>().Create(new { EquipmentManufacturer = manufacturerValid });
            var modelInvalid = GetFactory<EquipmentModelFactory>().Create(new { EquipmentManufacturer = manufacturerInvalid });

            var results = _target.ByManufacturerId(manufacturerValid.Id) as CascadingActionResult;
            var data = results.GetSelectListItems();

            Assert.AreEqual(2, data.Count()); // because --Select Here--
            Assert.AreEqual(modelValid.Description, data.Last().Text);
            Assert.AreEqual(modelValid.Id.ToString(), data.Last().Value);
        }

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetFactory<EquipmentModelFactory>().Create(new {Description = "description 0"});
            var entity1 = GetFactory<EquipmentModelFactory>().Create(new {Description = "description 1"});
            var search = new SearchEquipmentModel();
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

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetFactory<EquipmentModelFactory>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditEquipmentModel, EquipmentModel>(eq, new {
                Description = expected
            }));

            Assert.AreEqual(expected, Session.Get<EquipmentModel>(eq.Id).Description);
        }

        #endregion
    }
}
