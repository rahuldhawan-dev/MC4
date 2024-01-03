using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderScheduleOfValues;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class WorkOrderScheduleOfValueControllerTest : MapCallMvcControllerTestBase<WorkOrderScheduleOfValueController, WorkOrderScheduleOfValue>
    {
        #region Setup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => {
                var workOrder = GetFactory<WorkOrderFactory>().Create();
                var scheduleOfValue = GetFactory<ScheduleOfValueFactory>().Create();
                return GetEntityFactory<WorkOrderScheduleOfValue>().Build(new {
                    WorkOrder = workOrder,
                    ScheduleOfValue = scheduleOfValue,
                    IsOvertime = false,
                    Total = 22M,
                    LaborUnitCost = 14M
                });
            };

            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateWorkOrderScheduleOfValue)vm;
                model.ScheduleOfValueCategory = GetEntityFactory<ScheduleOfValueCategory>().Create().Id;
            };
            options.InitializeUpdateViewModel = (vm) => {
                var model = (EditWorkOrderScheduleOfValue)vm;
                model.ScheduleOfValueCategory = GetEntityFactory<ScheduleOfValueCategory>().Create().Id;
            };

            options.CreateReturnsPartialShowViewOnSuccess = true;
            options.NewReturnsPartialView = true;
            options.ExpectedNewViewName = "_New";
            options.EditReturnsPartialView = true;
            options.ExpectedEditViewName = "_Edit";
            options.ShowReturnsPartialView = true;
            options.UpdateReturnsPartialShowViewOnSuccess = true;
            options.ExpectedShowViewName = "_Show";
            options.DestroyReturnsHttpStatusCodeNoContentOnSuccess = true;
        }

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/WorkOrderScheduleOfValue/New");
                a.RequiresLoggedInUserOnly("~/WorkOrderScheduleOfValue/Create");
                a.RequiresLoggedInUserOnly("~/WorkOrderScheduleOfValue/Edit");
                a.RequiresLoggedInUserOnly("~/WorkOrderScheduleOfValue/Update");
                a.RequiresLoggedInUserOnly("~/WorkOrderScheduleOfValue/Show");
                a.RequiresLoggedInUserOnly("~/WorkOrderScheduleOfValue/Destroy");
            });
        }

        #endregion

        #region Create/New

        [TestMethod]
        public void TestCreateCreatesWorkOrderScheduleOfValue()
        {
            var workOrder = GetFactory<WorkOrderFactory>().Create();
            var scheduleOfValueCategory = GetFactory<ScheduleOfValueCategoryFactory>().Create();
            var scheduleOfValue = GetFactory<ScheduleOfValueFactory>().Create(new {
                ScheduleOfValueCategory = scheduleOfValueCategory
            });

            var model = new CreateWorkOrderScheduleOfValue(_container) {
                WorkOrder = workOrder.Id,
                ScheduleOfValueCategory = scheduleOfValueCategory.Id,
                ScheduleOfValue = scheduleOfValue.Id,
                Total = 8
            };

            var result = (PartialViewResult)_target.Create(model);
            var resultModel = (WorkOrderScheduleOfValue)result.Model;

            Assert.AreEqual(8, resultModel.Total);
            Assert.AreEqual(scheduleOfValue.Id, resultModel.ScheduleOfValue?.Id);
            Assert.AreEqual(scheduleOfValueCategory.Id, resultModel.ScheduleOfValue?.ScheduleOfValueCategory?.Id);
        }

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            var workOrder = GetFactory<WorkOrderFactory>().Create();

            var result = (PartialViewResult)_target.New(workOrder.Id);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(CreateWorkOrderScheduleOfValue));
            Assert.AreEqual(((CreateWorkOrderScheduleOfValue)result.Model).WorkOrder, workOrder.Id);
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var expected = "Testing Description";
            var workOrder = GetFactory<WorkOrderFactory>().Create();
            var scheduleOfValue = GetFactory<ScheduleOfValueFactory>().Create();
            var workOrderScheduleOfValue = GetEntityFactory<WorkOrderScheduleOfValue>().Create(new {
                WorkOrder = workOrder,
                ScheduleOfValue = scheduleOfValue,
                IsOvertime = false,
                Total = 22M,
                LaborUnitCost = 14M
            });

            var model = _viewModelFactory.BuildWithOverrides<EditWorkOrderScheduleOfValue, WorkOrderScheduleOfValue>(workOrderScheduleOfValue, new {
                OtherDescription = expected
            });
            
            var result = (PartialViewResult)_target.Update(model);
            var resultMaterialUsed = (WorkOrderScheduleOfValue)result.Model;

            Assert.AreEqual("_Show", result.ViewName);
            Assert.AreEqual(expected, resultMaterialUsed.OtherDescription);
        }

        #endregion
    }
}
