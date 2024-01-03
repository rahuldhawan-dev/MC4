using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Areas.Reports.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class WaterLossManagementControllerTest : MapCallMvcControllerTestBase<WaterLossManagementController, WorkOrder>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresRole("~/Reports/WaterLossManagement/Index", WaterLossManagementController.ROLE);
                a.RequiresRole("~/Reports/WaterLossManagement/Search", WaterLossManagementController.ROLE);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            Assert.Inconclusive("Test me");
        }

        [TestMethod]
        public void TestIndexDisplaysErrorIfBetweenAndStartIsNotStartOfMonth()
        {
            GetEntityFactory<WorkOrder>().Create(new { LostWater = 10, DateCompleted = new DateTime(2023, 8, 15) });

            var result = _target.Index(new SearchWaterLoss {
                Date = new RequiredDateRange { Operator = RangeOperator.Between, Start = new DateTime(2023, 8, 2), End = new DateTime(2023, 8, 31)}
            }) as ViewResult;

            Assert.AreEqual(WaterLossManagementController.START_ERROR,
                ((List<string>)result.TempData[MMSINC.Controllers.ControllerBase.ERROR_MESSAGE_KEY]).Single());
        }

        [TestMethod]
        public void TestIndexDisplaysErrorIfBetweenEndIsNotEndOfMonth()
        {
            GetEntityFactory<WorkOrder>().Create(new { LostWater = 10, DateCompleted = new DateTime(2023, 8, 15) });

            var result = _target.Index(new SearchWaterLoss {
                Date = new RequiredDateRange { Operator = RangeOperator.Between, Start = new DateTime(2023, 8, 1), End = new DateTime(2023, 8, 30) }
            }) as ViewResult;

            Assert.AreEqual(WaterLossManagementController.END_ERROR,
                ((List<string>)result.TempData[MMSINC.Controllers.ControllerBase.ERROR_MESSAGE_KEY]).Single());
        }

        [TestMethod]
        [DataRow(RangeOperator.GreaterThan)]
        [DataRow(RangeOperator.GreaterThanOrEqualTo)]
        public void TestIndexDisplaysErrorIfGreaterThanEndIsNotStartOfMonth(RangeOperator op)
        {
            GetEntityFactory<WorkOrder>().Create(new { LostWater = 10, DateCompleted = new DateTime(2023, 8, 15) });

            var result = _target.Index(new SearchWaterLoss {
                Date = new RequiredDateRange { Operator = op, End = new DateTime(2023, 8, 2) }
            }) as ViewResult;

            Assert.AreEqual(WaterLossManagementController.START_ERROR,
                ((List<string>)result.TempData[MMSINC.Controllers.ControllerBase.ERROR_MESSAGE_KEY]).Single());
        }

        [TestMethod]
        [DataRow(RangeOperator.LessThan)]
        [DataRow(RangeOperator.LessThanOrEqualTo)]
        public void TestIndexDisplayErrorsIfLessThanEndIsNotEndOfMonth(RangeOperator op)
        {
            GetEntityFactory<WorkOrder>().Create(new { LostWater = 10, DateCompleted = new DateTime(2023, 8, 15) });
            
            var result = _target.Index(new SearchWaterLoss {
                Date = new RequiredDateRange { Operator = op, End = new DateTime(2023, 8, 20) }
            }) as ViewResult;

            Assert.AreEqual(WaterLossManagementController.END_ERROR,
                ((List<string>)result.TempData[MMSINC.Controllers.ControllerBase.ERROR_MESSAGE_KEY]).Single());
        }
        
        [TestMethod]
        public void TestIndexDisplaysExactErrorMessageIfOperatorEqualTo()
        {
            GetEntityFactory<WorkOrder>().Create(new { LostWater = 10, DateCompleted = new DateTime(2023, 8, 15) });
            
            var result = _target.Index(new SearchWaterLoss {
                Date = new RequiredDateRange { Operator = RangeOperator.Equal, End = new DateTime(2023, 8, 15) }
            }) as ViewResult;

            Assert.AreEqual(WaterLossManagementController.EXACT_ERROR,
                ((List<string>)result.TempData[MMSINC.Controllers.ControllerBase.ERROR_MESSAGE_KEY]).Single());
        }
    }
}