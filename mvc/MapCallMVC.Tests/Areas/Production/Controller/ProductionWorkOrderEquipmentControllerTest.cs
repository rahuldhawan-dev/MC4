using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Production.Controllers;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Production.Controller
{
    [TestClass]
    public class ProductionWorkOrderEquipmentControllerTest : MapCallMvcControllerTestBase<ProductionWorkOrderEquipmentController, ProductionWorkOrderEquipment, ProductionWorkOrdersEquipmentRepository>
    {
        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.UpdateRedirectsToRouteOnSuccessArgs = (vm) => new { action = "Show", controller = "ProductionWorkOrder", vm.Id };
            options.InitializeUpdateViewModel = (vm) => {
                var model = (ProductionWorkOrderEquipmentViewModel)vm;
                model.AsLeftCondition = GetEntityFactory<AsLeftCondition>().Create().Id;
                model.AsFoundCondition = GetEntityFactory<AsFoundCondition>().Create().Id;
                model.AsFoundConditionReason = GetEntityFactory<AssetConditionReason>().Create().Id;
                model.AsLeftConditionReason = GetEntityFactory<AssetConditionReason>().Create().Id;
            };
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.ProductionWorkManagement;
            Authorization.Assert(a => {
                a.RequiresRole("~/Production/ProductionWorkOrderEquipment/Update", role, RoleActions.Edit);
            });
        }

        #region Update

        [TestMethod]
        public void TestUpdateProductionWorkOrderEquipment()
        {
            var pwo = GetEntityFactory<ProductionWorkOrderEquipment>().Create();
            var asLeftConditions = GetEntityFactory<AsLeftCondition>().CreateList(6);
            var asFoundConditions = GetEntityFactory<AsFoundCondition>().CreateList(5);
            var asFoundConditionExpected = asFoundConditions.Single(x => x.Id == AsFoundCondition.Indices.ACCEPTABLE_GOOD);
            var asLeftConditionExpected = asLeftConditions.Single(x => x.Id == AsLeftCondition.Indices.ACCEPTABLE_GOOD);

            var actionResult = _target.Update(_viewModelFactory.BuildWithOverrides<ProductionWorkOrderEquipmentViewModel, ProductionWorkOrderEquipment>(pwo, new {
                AsLeftCondition = asLeftConditionExpected.Id,
                AsFoundCondition = asFoundConditionExpected.Id
            }));

            var pwoe = Session.Get<ProductionWorkOrderEquipment>(pwo.Id);
            var result = actionResult as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Show", result.RouteValues["action"]);
            Assert.AreEqual(asLeftConditionExpected.Id, pwoe.AsLeftCondition.Id);
            Assert.AreEqual(asFoundConditionExpected.Id, pwoe.AsFoundCondition.Id);
        }

        [TestMethod]
        public void TestUpdateRedirectsToShowWithFragWhenAsLeftConditionIsNull()
        {
            var pwo = GetEntityFactory<ProductionWorkOrderEquipment>().Create();

            Assert.IsNull(pwo.AsLeftCondition);

            var actionResult = _target.Update(_viewModelFactory.BuildWithOverrides<ProductionWorkOrderEquipmentViewModel, ProductionWorkOrderEquipment>(pwo, new {
                ProductionWorkOrderId = pwo.Id
            }));

            var pwoe = Session.Get<ProductionWorkOrderEquipment>(pwo.Id);

            var result = actionResult as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(null, pwoe.AsLeftCondition);
            Assert.AreEqual("Show", result.RouteValues["action"]);
        }

        [TestMethod]
        public void TestUpdateProductionWorkOrderEquipmentForNeedsEmergencyRepairAndNeedsRepair()
        {
            var pwo = GetEntityFactory<ProductionWorkOrderEquipment>().Create();
            var asLeftConditions = GetEntityFactory<AsLeftCondition>().CreateList(6);
            var asFoundConditions = GetEntityFactory<AsFoundCondition>().CreateList(5);
            var asFoundConditionExpected = asFoundConditions.Single(x => x.Id == AsFoundCondition.Indices.ACCEPTABLE_GOOD);

            foreach (var asLeftConditionExpected in asLeftConditions)
            {
                var actionResult = _target.Update(_viewModelFactory.BuildWithOverrides<ProductionWorkOrderEquipmentViewModel, ProductionWorkOrderEquipment>(pwo, new {
                    AsLeftCondition = asLeftConditionExpected.Id,
                    AsFoundCondition = asFoundConditionExpected.Id
                }));

                var result = actionResult as RedirectToRouteResult;
                Assert.IsNotNull(result);

                Assert.AreEqual(
                    AsLeftCondition.AUTO_CREATE_PRODUCTION_WORK_ORDER_STATUSES.Contains(asLeftConditionExpected.Id)
                        ? "CreateProductionWorkOrderForEquipment"
                        : "Show", result.RouteValues["action"]);

                var pwoe = Session.Get<ProductionWorkOrderEquipment>(pwo.Id);
                Assert.AreEqual(asLeftConditionExpected.Id, pwoe.AsLeftCondition.Id);
                Assert.AreEqual(asFoundConditionExpected.Id, pwoe.AsFoundCondition.Id);
            }
        }

        [TestMethod]
        public void TestMapToEntitySetsPriorityWhenAsLeftConditionNeedsEmergencyRepairOrNeedsRepair()
        {
            var pwo = GetEntityFactory<ProductionWorkOrderEquipment>().Create();
            var asLeftConditions = GetEntityFactory<AsLeftCondition>().CreateList(6);
            var asFoundConditions = GetEntityFactory<AsFoundCondition>().CreateList(5);
            var asFoundConditionExpected = asFoundConditions.Single(x => x.Id == AsFoundCondition.Indices.ACCEPTABLE_GOOD);

            foreach (var asLeftConditionExpected in asLeftConditions)
            {
                var model = _viewModelFactory.BuildWithOverrides<ProductionWorkOrderEquipmentViewModel, ProductionWorkOrderEquipment>(pwo, new {
                    AsLeftCondition = asLeftConditionExpected.Id,
                    AsFoundCondition = asFoundConditionExpected.Id
                });

                var entity = new ProductionWorkOrderEquipment();
                model.MapToEntity(entity);

                switch (asLeftConditionExpected.Id)
                {
                    case AsLeftCondition.Indices.NEEDS_REPAIR:
                        Assert.IsTrue(entity.Priority.Id == (int)ProductionWorkOrderPriority.Indices.HIGH);
                        break;
                    case AsLeftCondition.Indices.NEEDS_EMERGENCY_REPAIR:
                        Assert.IsTrue(entity.Priority.Id == (int)ProductionWorkOrderPriority.Indices.EMERGENCY);
                        break;
                }
            }
        }

        #endregion

        #endregion
    }
}