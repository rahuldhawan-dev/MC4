using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Production.Controllers;
using MapCallMVC.Areas.Production.Models.ViewModels.WellTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Production.Controller
{
    [TestClass]
    public class WellTestControllerTest : MapCallMvcControllerTestBase<WellTestController, WellTest>
    {
        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => {
                var facility = GetEntityFactory<Facility>().Create(new {
                    CompanySubsidiary = GetEntityFactory<CompanySubsidiary>().Create(),
                    PublicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create(),
                });

                var operatingCenter = GetEntityFactory<OperatingCenter>().Create(new {
                    State = GetEntityFactory<State>().Create()
                });

                var equipment = GetEntityFactory<Equipment>().Create(new {
                    OperatingCenter = operatingCenter,
                    Facility = facility,
                });

                return GetEntityFactory<WellTest>().Create(new {
                    Employee = GetEntityFactory<Employee>().Create(),
                    ProductionWorkOrder = GetEntityFactory<ProductionWorkOrder>().Create(),
                    Facility = facility,
                    OperatingCenter = operatingCenter,
                    Equipment = equipment,
                    DateOfTest = Lambdas.GetNow,
                    PumpingRate = 1000,
                    MeasurementPoint = 12.20M,
                    StaticWaterLevel = 213.23M,
                    PumpingWaterLevel = 391.23M
                });
            };
        }

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                const string urlPathPart = "~/Production/WellTest";

                a.RequiresRole($"{urlPathPart}/Search/", RoleModules.ProductionWorkManagement);
                a.RequiresRole($"{urlPathPart}/Show/", RoleModules.ProductionWorkManagement);
                a.RequiresRole($"{urlPathPart}/Index/", RoleModules.ProductionWorkManagement);
                a.RequiresRole($"{urlPathPart}/New/", RoleModules.ProductionWorkManagement, RoleActions.Add);
                a.RequiresRole($"{urlPathPart}/Create/", RoleModules.ProductionWorkManagement, RoleActions.Add);
                a.RequiresRole($"{urlPathPart}/Edit/", RoleModules.ProductionWorkManagement, RoleActions.Edit);
                a.RequiresRole($"{urlPathPart}/Update/", RoleModules.ProductionWorkManagement, RoleActions.Edit);
            });
        }

        #endregion

        #region New

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            // other tests needed to cover this.
        }

        [TestMethod]
        public void TestNewReturnsViewWithCreateViewModelWhenParametersAreValid()
        {
            var productionWorkOrder = GetEntityFactory<ProductionWorkOrder>().Create();
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var equipment = GetEntityFactory<Equipment>().Create();

            var newViewModel = _viewModelFactory.BuildWithOverrides<NewWellTestViewModel>(new {
                ProductionWorkOrder = productionWorkOrder.Id,
                OperatingCenter = operatingCenter.Id,
                Equipment = equipment.Id
            });

            var actionResult = (ViewResult)_target.New(newViewModel);
            var viewModel = (CreateWellTestViewModel)actionResult.Model;

            Assert.AreEqual(productionWorkOrder.Id, viewModel.ProductionWorkOrder);
            Assert.AreEqual(operatingCenter.Id, viewModel.OperatingCenter);
            Assert.AreEqual(equipment.Id, viewModel.Equipment);

            MvcAssert.IsViewNamed(actionResult, "New");
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            const int pumpingRate = 243;

            var entity = GetEntityFactory<WellTest>().Create();

            _target.Update(_viewModelFactory.BuildWithOverrides<WellTestViewModel, WellTest>(entity, new {
                PumpingRate = pumpingRate
            }));

            var updatedEntity = Session.Get<WellTest>(entity.Id);

            Assert.AreEqual(pumpingRate, updatedEntity.PumpingRate);
        }

        #endregion

        #region Index

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            var expectedDate = DateTime.Today;

            var state = GetEntityFactory<State>().Create();
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create(new { State = state });
            var facility = GetEntityFactory<Facility>().Create(new { OperatingCenter = operatingCenter });
            var equipment = GetEntityFactory<Equipment>().Create(new { Facility = facility });
            var employee = GetEntityFactory<Employee>().Create();
            
            GetEntityFactory<WellTest>().Create(new {
                Equipment = equipment,
                Employee = employee,
                DateOfTest = expectedDate,
                PumpingRate = 1,
                MeasurementPoint = 43M,
                StaticWaterLevel = 412.34M,
                PumpingWaterLevel = 143.02M
            });

            GetEntityFactory<WellTest>().Create(new {
                Equipment = equipment,
                Employee = employee,
                DateOfTest = expectedDate,
                PumpingRate = 2,
                MeasurementPoint = 86M,
                StaticWaterLevel = 824.68M,
                PumpingWaterLevel = 286.04M
            });

            var search = new SearchWellTestsViewModel {
                State = state.Id,
                OperatingCenter = operatingCenter.Id,
                Facility = facility.Id,
                Equipment = equipment.Id
            };

            _target.Index(search);
            Assert.AreEqual(2, search.Count);
        }

        #endregion

        #region Search

        [TestMethod]
        public void TestIndexXlsExportsExcel()
        {
            DateTime DateTimeGenerator(int daysAgo) => DateTime.Now.AddDays(daysAgo * -1);

            const int expectedResultsCount = 4;

            var state = GetEntityFactory<State>().Create();
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create(new { State = state });
            var companySubsidiary = GetEntityFactory<CompanySubsidiary>().Create();
            var facility = GetEntityFactory<Facility>().Create(new {
                OperatingCenter = operatingCenter, 
                CompanySubsidiary = companySubsidiary
            });
            var equipment = GetEntityFactory<Equipment>().Create(new { Facility = facility });
            var employee = GetEntityFactory<Employee>().Create();

            var wellTestFactory = GetEntityFactory<WellTest>();

            var wellTests = new List<WellTest>(expectedResultsCount);
            for (var a = 0; a < expectedResultsCount; a++)
            {
                wellTests.Add(wellTestFactory.Create(new {
                    Equipment = equipment,
                    Employee = employee,
                    DateOfTest = DateTimeGenerator(1),
                    PumpingRate = 1,
                    MeasurementPoint = 43M,
                    StaticWaterLevel = 412.34M,
                    PumpingWaterLevel = 143.02M
                }));
            }

            var searchViewModel = new SearchWellTestsViewModel {
                State = state.Id,
                OperatingCenter = operatingCenter.Id,
                Facility = facility.Id,
                Equipment = equipment.Id
            };

            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ResponseFormatter.KnownExtensions.EXCEL_2003;

            var searchViewResult = _target.Index(searchViewModel) as ExcelResult;

            var helper = new ExcelResultTester(_container, searchViewResult, true);
            for (var rowIndex = 0; rowIndex < wellTests.Count(); rowIndex++)
            {
                var wellTest = wellTests[rowIndex];

                helper.AreEqual(wellTest.Id, "Id", rowIndex);
                helper.AreEqual(wellTest.PumpingRate, WellTest.DisplayNames.Form.PUMPING_RATE, rowIndex);
                helper.AreEqual(wellTest.DateOfTest, "DateOfTest", rowIndex);
                helper.AreEqual(wellTest.MeasurementPoint, WellTest.DisplayNames.Form.MEASUREMENT_POINT, rowIndex);
                helper.AreEqual(wellTest.StaticWaterLevel, WellTest.DisplayNames.Form.STATIC_WATER_LEVEL, rowIndex);
                helper.AreEqual(wellTest.PumpingWaterLevel, WellTest.DisplayNames.Form.PUMPING_WATER_LEVEL, rowIndex);
            }
            Assert.AreEqual(expectedResultsCount, helper.GetRowCount(helper.GetSheetNames().First()));
        }

        #endregion
    }
}