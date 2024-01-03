using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Reports.Controllers;
using MapCallMVC.Areas.Reports.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class MainBreakRepairsForGISControllerTest : MapCallMvcControllerTestBase<MainBreakRepairsForGISController,
        WorkOrder>
    {
        #region Setup/Teardown

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => GetEntityFactory<WorkOrder>().Create(new { WorkDescription = typeof(WaterMainBreakRepairWorkDescriptionFactory) });
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var role = RoleModules.FieldServicesWorkManagement;
                a.RequiresRole("~/Reports/MainBreakRepairsForGIS/Index", role);
                a.RequiresRole("~/Reports/MainBreakRepairsForGIS/Search", role);
            });
        }

        [TestMethod]
        public void TestIndexReturnsIndexViewWithArrayOfWorkOrders()
        {
            var wo1 = GetEntityFactory<WorkOrder>()
                .Create(new {WorkDescription = typeof(WaterMainBreakRepairWorkDescriptionFactory)});
            var wo2 = GetEntityFactory<WorkOrder>()
                .Create(new {WorkDescription = typeof(WaterMainBreakRepairWorkDescriptionFactory)});
            var search = new SearchMainBreakRepairsForGIS();

            var result = _target.Index(search) as ViewResult;
            var resultModel = ((SearchMainBreakRepairsForGIS)result.Model).Results.ToList();

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(2, resultModel.Count);
            Assert.AreSame(wo1, resultModel[0]);
            Assert.AreSame(wo2, resultModel[1]);
        }

        [TestMethod]
        public void TestIndexJSONExportsJSON()
        {
            var now = DateTime.Now;
            var wo0 = GetEntityFactory<WorkOrder>()
                .Create(new {
                    WorkDescription = typeof(WaterMainBreakRepairWorkDescriptionFactory),
                    DateCompleted = now.AddDays(-1)
                });
            var wo1 = GetEntityFactory<WorkOrder>()
                .Create(new {
                    WorkDescription = typeof(WaterMainBreakRepairWorkDescriptionFactory),
                    DateCompleted = now.AddDays(-1)
                });
            var search = new SearchMainBreakRepairsForGIS {
                DateCompleted = new DateRange {
                    Start = now.AddDays(-2),
                    End = now,
                    Operator = RangeOperator.Between
                }
            };
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.JSON;

            var result = _target.Index(search) as JsonResult;
            var helper = new JsonResultTester(result.Data);

            helper.AreEqual(wo0.Id.ToString(), "WorkOrderNumber");
            helper.AreEqual(wo1.Id.ToString(), "WorkOrderNumber", 1);
        }

        [TestMethod]
        public void TestIndexJSONThrowsExceptionIfDateSearchRangeIsNotSent()
        {
            var wo0 = GetEntityFactory<WorkOrder>()
                .Create(new {WorkDescription = typeof(WaterMainBreakRepairWorkDescriptionFactory)});
            var wo1 = GetEntityFactory<WorkOrder>()
                .Create(new {WorkDescription = typeof(WaterMainBreakRepairWorkDescriptionFactory)});
            var search = new SearchMainBreakRepairsForGIS();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.JSON;

            MyAssert.Throws<InvalidOperationException>(() =>
                _target.Index(search));
        }

        [TestMethod]
        public void TestIndexJSONThrowsExceptionIfDateSearchRangeIsLongerThanOneMonth()
        {
            var now = DateTime.Now;
            var wo0 = GetEntityFactory<WorkOrder>()
                .Create(new {WorkDescription = typeof(WaterMainBreakRepairWorkDescriptionFactory)});
            var wo1 = GetEntityFactory<WorkOrder>()
                .Create(new {WorkDescription = typeof(WaterMainBreakRepairWorkDescriptionFactory)});
            var search = new SearchMainBreakRepairsForGIS {
                DateCompleted = new DateRange {
                    Start = now.AddDays(-31),
                    End = now,
                    Operator = RangeOperator.Between
                }
            };
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.JSON;

            MyAssert.Throws<InvalidOperationException>(() =>
                _target.Index(search));
        }

        [TestMethod]
        public void TestIndexJSONThrowsExceptionIfDateSearchRangeOperatorIsNotBetween()
        {
            foreach (var op in new[] {
                RangeOperator.Equal, RangeOperator.GreaterThan, RangeOperator.GreaterThanOrEqualTo,
                RangeOperator.LessThan, RangeOperator.LessThanOrEqualTo
            })
            {
                var now = DateTime.Now;
                var wo0 = GetEntityFactory<WorkOrder>()
                    .Create(new {WorkDescription = typeof(WaterMainBreakRepairWorkDescriptionFactory)});
                var wo1 = GetEntityFactory<WorkOrder>()
                    .Create(new {WorkDescription = typeof(WaterMainBreakRepairWorkDescriptionFactory)});
                var search = new SearchMainBreakRepairsForGIS {
                    DateCompleted = new DateRange {
                        Start = now.AddMonths(-1).AddDays(-1),
                        End = now,
                        Operator = op
                    }
                };
                _target.ControllerContext = new ControllerContext();
                _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                    ResponseFormatter.KnownExtensions.JSON;

                MyAssert.Throws<InvalidOperationException>(() =>
                    _target.Index(search));
            }
        }
    }
}