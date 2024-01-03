using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Environmental.Controllers;
using MapCallMVC.Areas.Environmental.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class AllocationPermitWithdrawalNodeControllerTest : MapCallMvcControllerTestBase<AllocationPermitWithdrawalNodeController, AllocationPermitWithdrawalNode>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<IFacilityRepository>().Use<FacilityRepository>();
        }

        #endregion

        #region Roles

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                var module = RoleModules.EnvironmentalGeneral;
                a.RequiresRole("~/Environmental/AllocationPermitWithdrawalNode/Show/", module, RoleActions.Read);
                a.RequiresRole("~/Environmental/AllocationPermitWithdrawalNode/Search/", module, RoleActions.Read);
                a.RequiresRole("~/Environmental/AllocationPermitWithdrawalNode/Index/", module, RoleActions.Read);
                a.RequiresRole("~/Environmental/AllocationPermitWithdrawalNode/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/Environmental/AllocationPermitWithdrawalNode/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/Environmental/AllocationPermitWithdrawalNode/AddAllocationPermit/", module, RoleActions.Edit);
                a.RequiresRole("~/Environmental/AllocationPermitWithdrawalNode/RemoveAllocationPermit/", module, RoleActions.Edit);
                a.RequiresRole("~/Environmental/AllocationPermitWithdrawalNode/New/", module, RoleActions.Add);
                a.RequiresRole("~/Environmental/AllocationPermitWithdrawalNode/Create/", module, RoleActions.Add);
                a.RequiresRole("~/Environmental/AllocationPermitWithdrawalNode/Destroy/", module, RoleActions.Delete);
                a.RequiresRole("~/Environmental/AllocationPermitWithdrawalNode/AddEquipment/", module, RoleActions.Edit);
                a.RequiresRole("~/Environmental/AllocationPermitWithdrawalNode/RemoveEquipment/", module, RoleActions.Edit);
            });
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = _container.GetInstance<TestDataFactory<AllocationPermitWithdrawalNode>>().Create(new { Description = "description 0" });
            var entity1 = _container.GetInstance<TestDataFactory<AllocationPermitWithdrawalNode>>().Create(new { Description = "description 1" });
            var search = new SearchAllocationPermitWithdrawalNode();
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
            var eq = _container.GetInstance<TestDataFactory<AllocationPermitWithdrawalNode>>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditAllocationPermitWithdrawalNode, AllocationPermitWithdrawalNode>(eq, new{
                Description = expected
            })) as RedirectToRouteResult;

            Assert.AreEqual("Show", result.RouteValues["action"]);
            Assert.AreEqual(expected, Session.Get<AllocationPermitWithdrawalNode>(eq.Id).Description);
        }

        #endregion

        #region Children

        #region Equipment

        [TestMethod]
        public void TestAddEquipmentAddsEquipmentToAllocationPermitWithdrawalNode()
        {
            var allocationPermitWithdrawalNode = GetEntityFactory<AllocationPermitWithdrawalNode>().Create();
            var equipment = GetEntityFactory<Equipment>().Create(new { Identifier = "NJSD-1", Description = "Foo" });

            MyAssert.CausesIncrease(
                () => _target.AddEquipment(new AddAllocationPermitWithdrawalNodeEquipment(_container) { Equipment = equipment.Id, Id = allocationPermitWithdrawalNode.Id }),
                () => Session.Get<AllocationPermitWithdrawalNode>(allocationPermitWithdrawalNode.Id).Equipment.Count());
        }

        [TestMethod]
        public void TestRemoveEquipmentRemovesEquipmentFromAllocationPermitWithdrawalNode()
        {
            var allocationPermitWithdrawalNode = GetEntityFactory<AllocationPermitWithdrawalNode>().Create();
            var equipment = GetEntityFactory<Equipment>().Create(new { Identifier = "NJSD-1", Description = "Foo" });
            allocationPermitWithdrawalNode.Equipment.Add(equipment);
            Session.Save(allocationPermitWithdrawalNode);

            MyAssert.CausesDecrease(
                () => _target.RemoveEquipment(new RemoveAllocationPermitWithdrawalNodeEquipment(_container) { Equipment = equipment.Id, Id = allocationPermitWithdrawalNode.Id }),
                () => Session.Get<AllocationPermitWithdrawalNode>(allocationPermitWithdrawalNode.Id).Equipment.Count());
        }

        #endregion

        #endregion
    }
}
