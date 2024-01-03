using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Production.Controllers;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Production.Controller
{
    [TestClass]
    public class ProductionWorkDescriptionControllerTest : MapCallMvcControllerTestBase<ProductionWorkDescriptionController, ProductionWorkDescription>
    {
        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.ProductionWorkManagement;
            Authorization.Assert(auth => {
                auth.RequiresRole("~/Production/ProductionWorkDescription/Show", role, RoleActions.Read);
                auth.RequiresRole("~/Production/ProductionWorkDescription/Search", role, RoleActions.Read);
                auth.RequiresRole("~/Production/ProductionWorkDescription/Index", role, RoleActions.Read);

                auth.RequiresRole("~/Production/ProductionWorkDescription/New", role, RoleActions.Add);
                auth.RequiresRole("~/Production/ProductionWorkDescription/Create", role, RoleActions.Add);
                auth.RequiresRole("~/Production/ProductionWorkDescription/Edit", role, RoleActions.Edit);
                auth.RequiresRole("~/Production/ProductionWorkDescription/Update", role, RoleActions.Edit);

                auth.RequiresLoggedInUserOnly("~/Production/ProductionWorkDescription/ByEquipmentTypeId");
                auth.RequiresLoggedInUserOnly("~/Production/ProductionWorkDescription/ByEquipmentTypeIdForCreate");
                auth.RequiresLoggedInUserOnly("~/Production/ProductionWorkDescription/ByEquipmentTypeIdsForCreate");
            });
        }

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<ProductionWorkDescription>().Create(new { });
            var entity1 = GetEntityFactory<ProductionWorkDescription>().Create(new { });
            var search = new SearchProductionWorkDescription();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
            }
        }

        #endregion

        #region ByEquipmentTypeId

        [TestMethod]
        public void TestByEquipmentTypeIdReturnsByEquipmentTypeId()
        {
            var eqClass1 = GetFactory<EquipmentTypeGeneratorFactory>().Create();
            var eqClass2 = GetFactory<EquipmentTypeEngineFactory>().Create();
            var eq1 = GetEntityFactory<ProductionWorkDescription>().Create(new { EquipmentType = eqClass1, Description = "Foo" });
            var eq2 = GetEntityFactory<ProductionWorkDescription>().Create(new { EquipmentType = eqClass2, Description = "Bar" });

            var result = (CascadingActionResult)_target.ByEquipmentTypeId(eqClass2.Id);
            var actual = result.GetSelectListItems().ToArray();

            Assert.AreEqual(1, actual.Count() - 1);
            Assert.AreEqual(eq2.Id.ToString(), actual[1].Value);
        }

        [TestMethod]
        public void TestByEquipmentTypeIdForCreateReturnsByEquipmentTypeIdForCreate()
        {
            var eqClass1 = GetFactory<EquipmentTypeGeneratorFactory>().Create();
            var eqClass2 = GetFactory<EquipmentTypeEngineFactory>().Create();
            var orderTypes = GetFactory<OrderTypeFactory>().CreateAll();
            var pwd1 = GetEntityFactory<ProductionWorkDescription>().Create(new { EquipmentType = eqClass1, Description = "Foo", OrderType = orderTypes[0] });
            var pwd2 = GetEntityFactory<ProductionWorkDescription>().Create(new { EquipmentType = eqClass1, Description = "Bar", OrderType = orderTypes[1] });
            var pwd3 = GetEntityFactory<ProductionWorkDescription>().Create(new { EquipmentType = eqClass1, Description = "Hah", OrderType = orderTypes[4] });
            var pwd4 = GetEntityFactory<ProductionWorkDescription>().Create(new { EquipmentType = eqClass1, Description = "Baz", OrderType = orderTypes[2] });
            var pwd5 = GetEntityFactory<ProductionWorkDescription>().Create(new { EquipmentType = eqClass1, Description = "Bah", OrderType = orderTypes[3] });
            var pwd6 = GetEntityFactory<ProductionWorkDescription>().Create(new { EquipmentType = eqClass2, Description = "Bart", OrderType = orderTypes[3] });
            
            var result = (CascadingActionResult)_target.ByEquipmentTypeIdForCreate(eqClass1.Id);
            var actual = result.GetSelectListItems().ToArray();

            Assert.AreEqual(3, actual.Count() - 1);
            Assert.AreEqual(pwd5.Id.ToString(), actual[1].Value);
            Assert.AreEqual(pwd4.Id.ToString(), actual[2].Value);
            Assert.AreEqual(pwd1.Id.ToString(), actual[3].Value);
        }

        [TestMethod]
        public void TestByEquipmentTypeIdsForCreateReturnsByEquipmentTypeIdForCreate()
        {
            var eqClass1 = GetFactory<EquipmentTypeGeneratorFactory>().Create();
            var eqClass2 = GetFactory<EquipmentTypeEngineFactory>().Create();
            var orderTypes = GetFactory<OrderTypeFactory>().CreateAll();
            var pwd1 = GetEntityFactory<ProductionWorkDescription>().Create(new { EquipmentType = eqClass1, Description = "Foo", OrderType = orderTypes[0] });
            var pwd2 = GetEntityFactory<ProductionWorkDescription>().Create(new { EquipmentType = eqClass1, Description = "Bar", OrderType = orderTypes[1] });
            var pwd3 = GetEntityFactory<ProductionWorkDescription>().Create(new { EquipmentType = eqClass1, Description = "Baz", OrderType = orderTypes[4] });
            var pwd4 = GetEntityFactory<ProductionWorkDescription>().Create(new { EquipmentType = eqClass1, Description = "Bah", OrderType = orderTypes[2] });
            var pwd5 = GetEntityFactory<ProductionWorkDescription>().Create(new { EquipmentType = eqClass1, Description = "Hah", OrderType = orderTypes[3] });
            var pwd6 = GetEntityFactory<ProductionWorkDescription>().Create(new { EquipmentType = eqClass2, Description = "Ray", OrderType = orderTypes[3] });
            
            var result = (CascadingActionResult)_target.ByEquipmentTypeIdsForCreate(new int[]{eqClass1.Id, eqClass2.Id});
            var actual = result.GetSelectListItems().ToArray();

            Assert.AreEqual(4, actual.Count() - 1);
            Assert.AreEqual(pwd4.Id.ToString(), actual[1].Value);
            Assert.AreEqual(pwd1.Id.ToString(), actual[2].Value);
            Assert.AreEqual(pwd5.Id.ToString(), actual[3].Value);
            Assert.AreEqual(pwd6.Id.ToString(), actual[4].Value);
        }

        #endregion

        #endregion
    }
}