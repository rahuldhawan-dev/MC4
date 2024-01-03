using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Production.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MapCallMVC.Areas.Production.Models.ViewModels;
using MMSINC;
using MMSINC.Results;

namespace MapCallMVC.Tests.Areas.Production.Controller
{
    [TestClass]
    public class
        TaskGroupControllerTest : MapCallMvcControllerTestBase<TaskGroupController, TaskGroup, IRepository<TaskGroup>>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                const RoleModules role = RoleModules.ProductionDataAdministration;
                const string path = "~/Production/TaskGroup/";
                a.RequiresRole(path + "Search", role);
                a.RequiresRole(path + "Show", role);
                a.RequiresRole(path + "Index", role);
                a.RequiresRole(path + "Create", role, RoleActions.Add);
                a.RequiresRole(path + "New", role, RoleActions.Add);
                a.RequiresRole(path + "Edit", role, RoleActions.Edit);
                a.RequiresRole(path + "Update", role, RoleActions.Edit);
                a.RequiresRole(path + "Destroy", role, RoleActions.Delete);
                a.RequiresRole(path + "ByTaskGroupCategoryIdByEquipmentTypeId", role);
                a.RequiresRole(path + "ByTaskGroupCategoryIdByEquipmentTypeIds", role);
                a.RequiresRole(path + "ByTaskGroupCategoryIdOrAll", role);
                a.RequiresRole(path + "GetTaskDetailAndPlanTypeInformation", role);
                a.RequiresLoggedInUserOnly(path + "ByMaintenancePlanTaskTypeIds");
                a.RequiresLoggedInUserOnly(path + "ByTaskTypesForTaskGroupNames");
            });
        }

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var taskGrpCat1 = GetEntityFactory<TaskGroupCategory>().Create(new TaskGroupCategory {
                Description = "Cat 1", Type = "Test 1", Abbreviation = "T1", IsActive = true
            });
            var taskGrpCat2 = GetEntityFactory<TaskGroupCategory>().Create(new TaskGroupCategory {
                Description = "Cat 2", Type = "Test 2", Abbreviation = "T2", IsActive = false
            });

            var equipmentGroup1 = GetEntityFactory<EquipmentGroup>().Create(new EquipmentGroup {
                Code = "E",
                Description = "Electrical"
            });

            var equipmentType1 = GetEntityFactory<EquipmentType>().Create(new EquipmentType {
                Abbreviation = "ABBR1", Description = "Equipment type 1", EquipmentGroup = equipmentGroup1
            });
            var equipmentType2 = GetEntityFactory<EquipmentType>().Create(new EquipmentType {
                Abbreviation = "AB2", Description = "Equipment type 2", EquipmentGroup = equipmentGroup1
            });
            IList<EquipmentType> equipmentTypes = new List<EquipmentType>();
            equipmentTypes.Add(equipmentType1);
            equipmentTypes.Add(equipmentType2);

            var entity0 = GetEntityFactory<TaskGroup>().Create(new {
                TaskGroupName = "Task Group 0", EquipmentTypes = equipmentTypes, TaskGroupCategory = taskGrpCat1
            });
            var entity1 = GetEntityFactory<TaskGroup>().Create(new {
                TaskGroupName = "Task Group 1", EquipmentTypes = equipmentTypes, TaskGroupCategory = taskGrpCat2
            });

            var search = new SearchTaskGroup();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = (ExcelResult)_target.Index(search);

            using (var helper = new ExcelResultTester(_container, result, true)) 
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.TaskGroupName, "TaskGroupName");
                helper.AreEqual(entity1.TaskGroupName, "TaskGroupName", 1);
                helper.AreEqual(entity0.EquipmentTypes.ToString(), "EquipmentTypes");
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<TaskGroup>().Create();
            var expected = "Name field";

            _target.Update(_viewModelFactory.BuildWithOverrides<TaskGroupViewModel, TaskGroup>(eq, new {
                TaskGroupName = expected
            }));

            Assert.AreEqual(expected, Session.Get<TaskGroup>(eq.Id).TaskGroupName);
        }

        #endregion

        [TestMethod]
        public void TestByMaintenancePlanTaskTypeIdsReturnsCascadingActionResult()
        {
            var taskGroup1 = GetFactory<TaskGroupFactory>().Create(new {Id = 1, TaskGroupName = "Task Grp 1"});
            var taskGroup2 = GetFactory<TaskGroupFactory>().Create(new { Id = 2, TaskGroupName = "Task Grp 2"});
            var taskGroup3 = GetFactory<TaskGroupFactory>().Create(new { Id = 3, TaskGroupName = "Task Grp 3"});
            var taskGroup4 = GetFactory<TaskGroupFactory>().Create(new { Id = 4, TaskGroupName = "Task Grp 4"});
            var taskGroup5 = GetFactory<TaskGroupFactory>().Create(new { Id = 5, TaskGroupName = "Task Grp 5"});

            var maintenancePlanTaskType1 = GetFactory<MaintenancePlanTaskTypeFactory>().Create(new { Id = 1, Description = "MP1"});
            var maintenancePlanTaskType2 = GetFactory<MaintenancePlanTaskTypeFactory>().Create(new { Id = 2, Description = "MP2" });
            var maintenancePlanTaskType3 = GetFactory<MaintenancePlanTaskTypeFactory>().Create(new { Id = 3, Description = "MP3" });

            taskGroup1.MaintenancePlanTaskType = maintenancePlanTaskType1;
            taskGroup2.MaintenancePlanTaskType = maintenancePlanTaskType1;
            taskGroup3.MaintenancePlanTaskType = maintenancePlanTaskType2;
            taskGroup4.MaintenancePlanTaskType = maintenancePlanTaskType2;
            taskGroup5.MaintenancePlanTaskType = maintenancePlanTaskType2;

            Session.Flush();
            
            var results = _target.ByMaintenancePlanTaskTypeIds(new[] { maintenancePlanTaskType1.Id}) as CascadingActionResult;
            var data = results?.GetSelectListItems().ToArray();
            // first item is -- Select --
            Assert.AreEqual(3, (data ?? Array.Empty<SelectListItem>()).Count());
            
            results = _target.ByMaintenancePlanTaskTypeIds(new[] { maintenancePlanTaskType2.Id }) as CascadingActionResult;
            data = results?.GetSelectListItems().ToArray();
            Assert.AreEqual(4, (data ?? Array.Empty<SelectListItem>()).Count());

            results = _target.ByMaintenancePlanTaskTypeIds(new[] { maintenancePlanTaskType1.Id, maintenancePlanTaskType2.Id }) as CascadingActionResult;
            data = results?.GetSelectListItems().ToArray();
            Assert.AreEqual(6, (data ?? Array.Empty<SelectListItem>()).Count());

            results = _target.ByMaintenancePlanTaskTypeIds(new[] { maintenancePlanTaskType3.Id }) as CascadingActionResult;
            data = results?.GetSelectListItems().ToArray();
            Assert.AreEqual(0, (data ?? Array.Empty<SelectListItem>()).Count());
        }

        [TestMethod]
        public void TestByTaskGroupCategoryIdOrAllReturnsCascadingActionResult()
        {
            var taskGroupCategories = GetFactory<TaskGroupCategoryFactory>().CreateList(2);
            var taskGroupCategory = taskGroupCategories[0];
            var taskGroupCategory2 = taskGroupCategories[1];

            var taskGroup = GetFactory<TaskGroupFactory>().Create();
            var taskGroup2 = GetFactory<TaskGroupFactory>().Create(new { Id = 2, TaskGroupName = "Task Grp 2" });
            var taskGroup3 = GetFactory<TaskGroupFactory>().Create(new { Id = 3, TaskGroupName = "Task Grp 3" });

            taskGroupCategory.TaskGroups.Add(taskGroup);
            taskGroupCategory.TaskGroups.Add(taskGroup2);
            taskGroupCategory2.TaskGroups.Add(taskGroup3);

            taskGroup.TaskGroupCategory = taskGroupCategory;
            taskGroup2.TaskGroupCategory = taskGroupCategory;
            taskGroup3.TaskGroupCategory = taskGroupCategory2;

            Session.Flush();

            var results = _target.ByTaskGroupCategoryIdOrAll(taskGroupCategory.Id) as CascadingActionResult;
            var data = results?.GetSelectListItems().ToArray();

            Assert.AreEqual(3, (data ?? Array.Empty<SelectListItem>()).Count()); // first item is -- Select --
            Assert.AreEqual(taskGroup.ToString(), data?[1].Text);
            Assert.AreEqual(taskGroup.Id.ToString(), data?[1].Value);
            Assert.AreEqual(taskGroup2.ToString(), data?[2].Text);
            Assert.AreEqual(taskGroup2.Id.ToString(), data?[2].Value);

            results = _target.ByTaskGroupCategoryIdOrAll(taskGroupCategory2.Id) as CascadingActionResult;
            data = results?.GetSelectListItems().ToArray();

            Assert.AreEqual(2, (data ?? Array.Empty<SelectListItem>()).Count()); // first item is -- Select --
            Assert.AreEqual(taskGroup3.ToString(), data?[1].Text);
            Assert.AreEqual(taskGroup3.Id.ToString(), data?[1].Value);

            results = _target.ByTaskGroupCategoryIdOrAll(null) as CascadingActionResult;
            data = results?.GetSelectListItems().ToArray();

            Assert.AreEqual(4, (data ?? Array.Empty<SelectListItem>()).Count()); // first item is -- Select --
        }

        [TestMethod]
        public void TestByTaskGroupCategoryIdByEquipmentTypeIdReturnsCascadingActionResult()
        {
            var equipmentTypes = GetFactory<EquipmentTypeFactory>().CreateAll();
            var equipmentType = equipmentTypes[0];
            var equipmentType2 = equipmentTypes[1];
            var equipmentType3 = equipmentTypes[2];
            Session.Save(equipmentType);
            Session.Save(equipmentType2);
            Session.Save(equipmentType3);
            var taskGroupCategory = GetFactory<TaskGroupCategoryFactory>().Create();
            var taskGroupCategory2 = GetFactory<TaskGroupCategoryFactory>().Create(2);

            var taskGroup = GetFactory<TaskGroupFactory>().Create();
            var taskGroup2 = GetFactory<TaskGroupFactory>().Create(new { Id = 2, TaskGroupName = "Task Grp 2" });
            var taskGroup3 = GetFactory<TaskGroupFactory>().Create(new { Id = 3, TaskGroupName = "Task Grp 3" });
            Session.Save(taskGroup);
            Session.Save(taskGroup2);
            Session.Save(taskGroup3);

            taskGroup.EquipmentTypes.Add(equipmentType);
            taskGroup.EquipmentTypes.Add(equipmentType2);
            taskGroup2.EquipmentTypes.Add(equipmentType);
            taskGroup2.EquipmentTypes.Add(equipmentType2);
            taskGroup2.EquipmentTypes.Add(equipmentType3);
            taskGroup3.EquipmentTypes.Add(equipmentType2);
            Session.Save(taskGroup);
            Session.Save(taskGroup2);
            Session.Save(taskGroup3);

            taskGroupCategory.TaskGroups.Add(taskGroup);
            taskGroupCategory.TaskGroups.Add(taskGroup2);
            taskGroupCategory2.TaskGroups.Add(taskGroup3);
            Session.Save(taskGroupCategory);
            Session.Save(taskGroupCategory2);

            taskGroup.TaskGroupCategory = taskGroupCategory;
            taskGroup2.TaskGroupCategory = taskGroupCategory;
            taskGroup3.TaskGroupCategory = taskGroupCategory2;

            Session.Save(taskGroup);
            Session.Save(taskGroup2);
            Session.Save(taskGroup3);
            Session.Flush();

            var results =
                _target.ByTaskGroupCategoryIdByEquipmentTypeId(taskGroupCategory.Id, equipmentType.Id) as
                    CascadingActionResult;
            var data = results?.GetSelectListItems().ToArray();

            Assert.AreEqual(3, (data ?? Array.Empty<SelectListItem>()).Count()); // first item is -- Select --
            Assert.AreEqual(taskGroup.TaskGroupName, data?[1].Text);
            Assert.AreEqual(taskGroup.Id.ToString(), data?[1].Value);
            Assert.AreEqual(taskGroup2.TaskGroupName, data?[2].Text);
            Assert.AreEqual(taskGroup2.Id.ToString(), data?[2].Value);

            results =
                _target.ByTaskGroupCategoryIdByEquipmentTypeId(taskGroupCategory.Id, equipmentType2.Id) as
                    CascadingActionResult;
            data = results?.GetSelectListItems().ToArray();

            Assert.AreEqual(3, (data ?? Array.Empty<SelectListItem>()).Count()); // first item is -- Select --
            Assert.AreEqual(taskGroup.TaskGroupName, data?[1].Text);
            Assert.AreEqual(taskGroup.Id.ToString(), data?[1].Value);
            Assert.AreEqual(taskGroup2.TaskGroupName, data?[2].Text);
            Assert.AreEqual(taskGroup2.Id.ToString(), data?[2].Value);

            results =
                _target.ByTaskGroupCategoryIdByEquipmentTypeId(taskGroupCategory2.Id, equipmentType2.Id) as
                    CascadingActionResult;
            data = results?.GetSelectListItems().ToArray();

            Assert.AreEqual(2, (data ?? Array.Empty<SelectListItem>()).Count()); // first item is -- Select --
            Assert.AreEqual(taskGroup3.TaskGroupName, data?[1].Text);
            Assert.AreEqual(taskGroup3.Id.ToString(), data?[1].Value);
        }

        [TestMethod]
        public void TestByTaskGroupCategoryIdByEquipmentTypeIdsReturnsCascadingActionResult()
        {
            var equipmentTypes = GetFactory<EquipmentTypeFactory>().CreateAll();
            var equipmentType = equipmentTypes[0];
            var equipmentType2 = equipmentTypes[1];
            var equipmentType3 = equipmentTypes[2];
            Session.Save(equipmentType);
            Session.Save(equipmentType2);
            Session.Save(equipmentType3);
            var taskGroupCategory = GetFactory<TaskGroupCategoryFactory>().Create();
            var taskGroupCategory2 = GetFactory<TaskGroupCategoryFactory>().Create(2);

            var taskGroup = GetFactory<TaskGroupFactory>().Create();
            var taskGroup2 = GetFactory<TaskGroupFactory>().Create(new { Id = 2, TaskGroupName = "Task Grp 2" });
            var taskGroup3 = GetFactory<TaskGroupFactory>().Create(new { Id = 3, TaskGroupName = "Task Grp 3" });
            Session.Save(taskGroup);
            Session.Save(taskGroup2);
            Session.Save(taskGroup3);

            taskGroup.EquipmentTypes.Add(equipmentType);
            taskGroup.EquipmentTypes.Add(equipmentType2);
            taskGroup2.EquipmentTypes.Add(equipmentType);
            taskGroup2.EquipmentTypes.Add(equipmentType2);
            taskGroup2.EquipmentTypes.Add(equipmentType3);
            taskGroup3.EquipmentTypes.Add(equipmentType2);
            Session.Save(taskGroup);
            Session.Save(taskGroup2);
            Session.Save(taskGroup3);

            taskGroupCategory.TaskGroups.Add(taskGroup);
            taskGroupCategory.TaskGroups.Add(taskGroup2);
            taskGroupCategory2.TaskGroups.Add(taskGroup3);
            Session.Save(taskGroupCategory);
            Session.Save(taskGroupCategory2);

            taskGroup.TaskGroupCategory = taskGroupCategory;
            taskGroup2.TaskGroupCategory = taskGroupCategory;
            taskGroup3.TaskGroupCategory = taskGroupCategory2;

            Session.Save(taskGroup);
            Session.Save(taskGroup2);
            Session.Save(taskGroup3);
            Session.Flush();

            var results =
                _target.ByTaskGroupCategoryIdByEquipmentTypeIds(taskGroupCategory.Id,
                    new[] { equipmentType.Id, equipmentType2.Id }) as CascadingActionResult;
            var data = results?.GetSelectListItems().ToArray();

            Assert.AreEqual(3, (data ?? Array.Empty<SelectListItem>()).Count()); // first item is -- Select --
            Assert.AreEqual(taskGroup.TaskGroupName, data?[1].Text);
            Assert.AreEqual(taskGroup.Id.ToString(), data?[1].Value);
            Assert.AreEqual(taskGroup2.TaskGroupName, data?[2].Text);
            Assert.AreEqual(taskGroup2.Id.ToString(), data?[2].Value);

            results = _target.ByTaskGroupCategoryIdByEquipmentTypeIds(taskGroupCategory.Id,
                new[] { equipmentType.Id, equipmentType3.Id }) as CascadingActionResult;
            data = results?.GetSelectListItems().ToArray();

            Assert.AreEqual(3, (data ?? Array.Empty<SelectListItem>()).Count()); // first item is -- Select --
            Assert.AreEqual(taskGroup.TaskGroupName, data?[1].Text);
            Assert.AreEqual(taskGroup.Id.ToString(), data?[1].Value);
            Assert.AreEqual(taskGroup2.TaskGroupName, data?[2].Text);
            Assert.AreEqual(taskGroup2.Id.ToString(), data?[2].Value);

            results = _target.ByTaskGroupCategoryIdByEquipmentTypeIds(taskGroupCategory2.Id,
                new[] { equipmentType2.Id, equipmentType3.Id }) as CascadingActionResult;
            data = results?.GetSelectListItems().ToArray();

            Assert.AreEqual(2, (data ?? Array.Empty<SelectListItem>()).Count()); // first item is -- Select --
            Assert.AreEqual(taskGroup3.TaskGroupName, data?[1].Text);
            Assert.AreEqual(taskGroup3.Id.ToString(), data?[1].Value);

            results = _target.ByTaskGroupCategoryIdByEquipmentTypeIds(taskGroupCategory2.Id,
                new[] { equipmentType.Id, equipmentType2.Id, equipmentType3.Id }) as CascadingActionResult;
            data = results?.GetSelectListItems().ToArray();

            Assert.AreEqual(2, (data ?? Array.Empty<SelectListItem>()).Count()); // first item is -- Select --
            Assert.AreEqual(taskGroup3.TaskGroupName, data?[1].Text);
            Assert.AreEqual(taskGroup3.Id.ToString(), data?[1].Value);
        }
    }
}