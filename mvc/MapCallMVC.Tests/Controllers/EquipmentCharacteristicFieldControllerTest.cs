using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class EquipmentCharacteristicFieldControllerTest : MapCallMvcControllerTestBase<EquipmentCharacteristicFieldController, EquipmentCharacteristicField>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateRedirectsToRouteOnErrorArgs = (vm) => {
                var model = (CreateEquipmentCharacteristicField)vm;
                var entity = Repository.Find(model.Id);
                return new { action = "Show", controller = "EquipmentType", id = model.EquipmentType };
            };
            options.CreateRedirectsToRouteOnSuccessArgs = (vm) => {
                var model = (CreateEquipmentCharacteristicField)vm;
                var entity = Repository.Find(model.Id);
                return new { action = "Show", controller = "EquipmentType", id = model.EquipmentType };
            };
            options.DestroyRedirectsToRouteOnErrorArgs = (id) => {
                var equipmentTypeId = Repository.Find(id).EquipmentType.Id;
                return new { action = "Show", controller = "EquipmentType", id = equipmentTypeId };
            };
            options.DestroyRedirectsToRouteOnSuccessArgs = (id) => {
                var equipmentTypeId = Repository.Find(id).EquipmentType.Id;
                return new { action = "Show", controller = "EquipmentType", id = equipmentTypeId };
            };
            options.UpdateRedirectsToRouteOnSuccessArgs = (vm) => {
                var model = (EditEquipmentCharacteristicField)vm;
                var entity = Repository.Find(model.Id);
                return new { action = "Show", controller = "EquipmentType", id = entity.EquipmentType.Id };
            };
            options.UpdateRedirectsToRouteOnErrorArgs = vm 
                => new { action = "Edit", controller = "EquipmentCharacteristicField", id = ((EditEquipmentCharacteristicField)vm).Id };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresRole("~/EquipmentCharacteristicField/Create", EquipmentCharacteristicFieldController.ROLE, RoleActions.Edit);
                a.RequiresRole("~/EquipmentCharacteristicField/Destroy", EquipmentCharacteristicFieldController.ROLE, RoleActions.Edit);
                a.RequiresRole("~/EquipmentCharacteristicField/Edit", EquipmentCharacteristicFieldController.ROLE, RoleActions.Edit);
                a.RequiresRole("~/EquipmentCharacteristicField/Update", EquipmentCharacteristicFieldController.ROLE, RoleActions.Edit);
                a.RequiresSiteAdminUser("~/EquipmentCharacteristicField/RemoveDropDownValue");
            });
        }

        [TestMethod]
        public void TestCannotRemoveFieldThatHasBeenUsedToSetACharacteristicOnAPieceOfEquipment()
        {
            var equipmentType = GetFactory<EquipmentTypeGeneratorFactory>().Create();
            var equipmentPurpose = GetEntityFactory<EquipmentPurpose>().Create(new {
                EquipmentType = equipmentType
            });
            var entity = GetEntityFactory<EquipmentCharacteristicField>().Create(new {
                EquipmentType = equipmentType
            });
            var equipment = GetEntityFactory<Equipment>().Create(new {
                EquipmentPurpose = equipmentPurpose
            });

            GetEntityFactory<EquipmentCharacteristic>().Create(new {
                Field = entity,
                Equipment = equipment,
                Value = "asdf"
            });

            Session.Flush();
            Session.Clear();

            var result = _target.Destroy(entity.Id) as RedirectToRouteResult;

            Assert.IsFalse(_target.ModelState.IsValid);
            Assert.AreEqual("EquipmentType", result.RouteValues["controller"]);
            Assert.AreEqual("Show", result.RouteValues["action"]);
            Assert.AreEqual(equipmentType.Id, result.RouteValues["id"]);
        }

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<EquipmentCharacteristicField>().Create(new { IsActive = true });
            
            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditEquipmentCharacteristicField, EquipmentCharacteristicField>(eq, new {
                IsActive = false
            }));

            Assert.IsFalse(Session.Get<EquipmentCharacteristicField>(eq.Id).IsActive);
        }

        #endregion
    }
}
