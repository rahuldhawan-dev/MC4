using System;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Production.Controllers;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Production.Controller
{
    [TestClass]
    public class AssetReliabilityControllerTest : MapCallMvcControllerTestBase<AssetReliabilityController, AssetReliability>
    {
        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<UserFactory>().Create(new {Employee = GetEntityFactory<Employee>().Create()});
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Add(new TestDateTimeProvider(_now = DateTime.Now));
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                const RoleModules role = AssetReliabilityController.ROLE;
                const string path = "~/AssetReliability/";
                a.RequiresRole(path + "Search", role);
                a.RequiresRole(path + "Show", role);
                a.RequiresRole(path + "Index", role);
                a.RequiresRole(path + "Create", role, RoleActions.Add);
                a.RequiresRole(path + "New", role, RoleActions.Add);
                a.RequiresRole(path + "Copy", role, RoleActions.Add);
                a.RequiresRole(path + "Edit", role, RoleActions.Edit);
                a.RequiresRole(path + "Update", role, RoleActions.Edit);
                a.RequiresRole(path + "Destroy", role, RoleActions.Delete);
            });
        }

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            //noop its tested below
        }

        [TestMethod]
        public void TestNewReturnsNewViewModelWhenProductionWorkOrderOrEquipmentIsPassed()
        {
            var expected = 123;
            var result = (ViewResult)_target.New(productionWorkOrderId: expected);
            var resultModel = (CreateAssetReliability)result.Model;

            Assert.AreEqual(expected, resultModel.ProductionWorkOrder);
            Assert.IsNull(resultModel.Equipment, "Only the PWO value should have been set");
            MvcAssert.IsViewNamed(result, "New");

            result = (ViewResult)_target.New(equipmentId: expected);
            resultModel = (CreateAssetReliability)result.Model;

            Assert.AreEqual(expected, resultModel.Equipment);
            Assert.IsNull(resultModel.ProductionWorkOrder, "Only the Equipment value should have been set");
            MvcAssert.IsViewNamed(result, "New");
        }

        [TestMethod]
        public void TestCopyReturnsNewViewModelWithValuesPreFilled()
        {
            var pwo = GetEntityFactory<ProductionWorkOrder>().Create();
            var equipment = GetEntityFactory<Equipment>().Create();
            var entity = GetEntityFactory<AssetReliability>().Create(new{ ProductionWorkOrder = pwo, Equipment = equipment});

            var result = (ViewResult)_target.Copy(entity.Id);
            var resultModel = (CreateAssetReliability)result.Model;

            Assert.AreEqual(pwo.Id, resultModel.ProductionWorkOrder);
            Assert.AreEqual(equipment.Id, resultModel.Equipment);
            MvcAssert.IsViewNamed(result, "New");
        }

        [TestMethod]
        public void TestDeleteDeletesTheEntityId()
        {
            var pwo = GetEntityFactory<ProductionWorkOrder>().Create();
            var equipment = GetEntityFactory<Equipment>().Create();
            var entity = GetEntityFactory<AssetReliability>().Create(new { ProductionWorkOrder = pwo, Equipment = equipment });

            var result = Repository.Save(entity);

            Assert.AreEqual(entity.Id, result.Id);

            _target.Destroy(entity.Id);
            Assert.IsNull(Repository.Find(entity.Id));
        }
    }
}
