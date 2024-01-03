using System;
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
    public class
        TaskGroupCategoryControllerTest : MapCallMvcControllerTestBase<TaskGroupCategoryController, TaskGroupCategory>
    {
        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.ProductionDataAdministration;
            const string path = "~/Production/TaskGroupCategory/";
            Authorization.Assert(auth => {
                auth.RequiresRole(path + "Show", role);
                auth.RequiresRole(path + "Search", role);
                auth.RequiresRole(path + "Index", role);

                auth.RequiresLoggedInUserOnly(path + "ByEquipmentTypeId");
                auth.RequiresLoggedInUserOnly(path + "ByEquipmentTypeIds");

                auth.RequiresRole(path + "New", role, RoleActions.Add);
                auth.RequiresRole(path + "Create", role, RoleActions.Add);
                auth.RequiresRole(path + "Edit", role, RoleActions.Edit);
                auth.RequiresRole(path + "Update", role, RoleActions.Edit);
            });
        }

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<TaskGroupCategory>().Create();
            var expected = "new description  field value";

            _target.Update(_viewModelFactory.BuildWithOverrides<TaskGroupCategoryViewModel, TaskGroupCategory>(eq, new {
                Description = expected
            }));

            Assert.AreEqual(expected, Session.Get<TaskGroupCategory>(eq.Id).Description);
        }

        #endregion

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<TaskGroupCategory>().Create(new {
                Description = "Test 1", Type = "Test 1", Abbreviation = "T1", IsActive = true
            });
            var entity1 = GetEntityFactory<TaskGroupCategory>().Create(new {
                Description = "Test 2", Type = "Test 2", Abbreviation = "T2", IsActive = false
            });
            var search = new SearchTaskGroupCategory();
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

        [TestMethod]
        public void TestByEquipmentTypeIdReturnsCascadingActionResult()
        {
            var equipmentTypes = GetFactory<EquipmentTypeFactory>().CreateAll();
            var equipmentType = equipmentTypes[0];
            var equipmentType2 = equipmentTypes[1];
            var equipmentType3 = equipmentTypes[2];
            Session.Save(equipmentType);
            Session.Save(equipmentType2);
            Session.Save(equipmentType3);

            var taskGroupCategory = GetFactory<TaskGroupCategoryFactory>().Create();
            var taskGroupCategory2 = GetFactory<TaskGroupCategoryFactory>().Create(new {
                Description = "Task Grp Category 2",
                Type = "Test 2",
                Abbreviation = "T2",
                IsActive = true
            });

            var taskGroup = GetFactory<TaskGroupFactory>().Create(new
                { Id = 1, TaskGroupName = "Task Grp 1", TaskGroupCategory = taskGroupCategory });
            var taskGroup2 = GetFactory<TaskGroupFactory>().Create(new
                { Id = 2, TaskGroupName = "Task Grp 2", TaskGroupCategory = taskGroupCategory });
            var taskGroup3 = GetFactory<TaskGroupFactory>().Create(new
                { Id = 3, TaskGroupName = "Task Grp 3", TaskGroupCategory = taskGroupCategory2 });
            Session.Save(taskGroup);
            Session.Save(taskGroup2);
            Session.Save(taskGroup3);

            taskGroupCategory.TaskGroups.Add(taskGroup);
            taskGroupCategory.TaskGroups.Add(taskGroup2);
            taskGroupCategory2.TaskGroups.Add(taskGroup3);
            Session.Save(taskGroupCategory);
            Session.Save(taskGroupCategory2);

            taskGroup.EquipmentTypes.Add(equipmentType);
            taskGroup.EquipmentTypes.Add(equipmentType2);
            taskGroup2.EquipmentTypes.Add(equipmentType);
            taskGroup2.EquipmentTypes.Add(equipmentType2);
            taskGroup2.EquipmentTypes.Add(equipmentType3);
            taskGroup3.EquipmentTypes.Add(equipmentType2);
            Session.Save(taskGroup);
            Session.Save(taskGroup2);
            Session.Save(taskGroup3);

            Session.Flush();

            var results = _target.ByEquipmentTypeId(equipmentType.Id) as CascadingActionResult;
            var data = results?.GetSelectListItems().ToArray();

            Assert.AreEqual(2,
                (data ?? Array.Empty<SelectListItem>())
               .Count()); // 2 categories are returned, the first item is -- Select --
            Assert.AreEqual(taskGroupCategory.Type, data?[1].Text);
            Assert.AreEqual(taskGroupCategory.Id.ToString(), data?[1].Value);
        }

        [TestMethod]
        public void TestByEquipmentTypeIdsReturnsCascadingActionResult()
        {
            var equipmentTypes = GetFactory<EquipmentTypeFactory>().CreateAll();
            var equipmentType = equipmentTypes[0];
            var equipmentType2 = equipmentTypes[1];
            var equipmentType3 = equipmentTypes[2];
            Session.Save(equipmentType);
            Session.Save(equipmentType2);
            Session.Save(equipmentType3);
            var taskGroupCategory = GetFactory<TaskGroupCategoryFactory>().Create();
            var taskGroupCategory2 = GetFactory<TaskGroupCategoryFactory>().Create(new {
                Description = "Task Grp Category 2",
                Type = "Test 2",
                Abbreviation = "T2",
                IsActive = true
            });

            var taskGroupCategory3 = GetFactory<TaskGroupCategoryFactory>().Create(new {
                Description = "Task Grp Category 3",
                Type = "Test 3",
                Abbreviation = "T3",
                IsActive = true
            });

            var taskGroup = GetFactory<TaskGroupFactory>().Create(new
                { Id = 1, TaskGroupName = "Task Grp 1", TaskGroupCategory = taskGroupCategory });
            var taskGroup2 = GetFactory<TaskGroupFactory>().Create(new
                { Id = 2, TaskGroupName = "Task Grp 2", TaskGroupCategory = taskGroupCategory2 });
            var taskGroup3 = GetFactory<TaskGroupFactory>().Create(new
                { Id = 3, TaskGroupName = "Task Grp 3", TaskGroupCategory = taskGroupCategory3 });
            Session.Save(taskGroup);
            Session.Save(taskGroup2);
            Session.Save(taskGroup3);

            taskGroup.EquipmentTypes.Add(equipmentType);
            taskGroup.EquipmentTypes.Add(equipmentType2);
            taskGroup2.EquipmentTypes.Add(equipmentType);
            taskGroup2.EquipmentTypes.Add(equipmentType2);
            taskGroup2.EquipmentTypes.Add(equipmentType3);
            taskGroup3.EquipmentTypes.Add(equipmentType2);
            taskGroupCategory.TaskGroups.Add(taskGroup);
            taskGroupCategory2.TaskGroups.Add(taskGroup2);
            taskGroupCategory3.TaskGroups.Add(taskGroup3);
            Session.Save(taskGroupCategory);
            Session.Save(taskGroupCategory2);
            Session.Save(taskGroupCategory3);
            Session.Flush();

            var results =
                _target.ByEquipmentTypeIds(new[] { equipmentType.Id, equipmentType3.Id }) as CascadingActionResult;
            var data = results?.GetSelectListItems().ToArray();

            Assert.AreEqual(3,
                (data ?? Array.Empty<SelectListItem>())
               .Count()); // 2 categories are returned, the first item is -- Select --
            Assert.AreEqual(taskGroupCategory.Type, data?[1].Text);
            Assert.AreEqual(taskGroupCategory.Id.ToString(), data?[1].Value);
            Assert.AreEqual(taskGroupCategory2.Type, data?[2].Text);
            Assert.AreEqual(taskGroupCategory2.Id.ToString(), data?[2].Value);
        }
    }

    #endregion
}