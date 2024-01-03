using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Reports.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class EmployeeTrainingControllerTest : MapCallMvcControllerTestBase<EmployeeTrainingController, TrainingRecordAttendedEmployee>
    {
        private DataType _dataType;

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _dataType = GetEntityFactory<DataType>()
                .Create(new {
                    Name = TrainingRecord.DataTypeNames.EMPLOYEES_ATTENDED,
                    TableName = TrainingRecordMap.TABLE_NAME
                });
        }

        #endregion

        #region Authorization

        [TestMethod]		
        public override void TestControllerAuthorization()
        {
            var role = EmployeeTrainingController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/Reports/EmployeeTraining/Search/", role);
                a.RequiresRole("~/Reports/EmployeeTraining/Index/", role);
			});
		}				

        #endregion

        #region Index

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var positionGroupCommonName = GetFactory<PositionGroupCommonNameFactory>().Create();
            var positionGroup = GetFactory<PositionGroupFactory>().Create(new { Description = "position group", CommonName = positionGroupCommonName });
            var employee = GetFactory<ActiveEmployeeFactory>().Create(new { FirstName = "Mark", LastName = "Thomas", MiddleName = "G", OperatingCenter = operatingCenter, PositionGroup = positionGroup });
            var moduleCategory = GetEntityFactory<TrainingModuleCategory>().Create(new { Description = "Safety" });
            var trainingRequirement = GetEntityFactory<TrainingRequirement>().Create(new { IsOSHARequirement = true, IsActive = true });
            trainingRequirement.PositionGroupCommonNames.Add(positionGroupCommonName);
            trainingRequirement.TrainingFrequencyUnit = "Y";
            trainingRequirement.TrainingFrequency = 2;
            Session.Save(trainingRequirement);
            var trainingModule = GetEntityFactory<TrainingModule>().Create(new
            {
                Title = "Bloodborne Pathogens - General (OSHA: 1910.103) - Annual course",
                CourseApprovalNumber = "05-061401-31",
                TCHCertified = true,
                TrainingModuleCategory = moduleCategory,
                IsActive = true,
                TCHCreditValue = 1f,
                TrainingRequirement = trainingRequirement,
                TotalHours = 8f
            });
            var trainingRecordOlder = GetEntityFactory<TrainingRecord>().Create(new
            {
                TrainingModule = trainingModule,
                HeldOn = DateTime.Now.AddYears(-1),
                ScheduledDate = DateTime.Now.AddYears(-1)
            });
            var trainingRecord = GetEntityFactory<TrainingRecord>().Create(new
            {
                TrainingModule = trainingModule,
                HeldOn = DateTime.Now,
                ScheduledDate = DateTime.Now,
            });
            var trainingRecordFuture = GetEntityFactory<TrainingRecord>().Create(new
            {
                TrainingModule = trainingModule,
                ScheduledDate = DateTime.Now.AddYears(1)
            });
            GetFactory<TrainingRecordAttendedEmployeeFactory>()
                .Create(new { Employee = employee, LinkedId = trainingRecordOlder.Id });
            GetFactory<TrainingRecordAttendedEmployeeFactory>()
                .Create(new { Employee = employee, LinkedId = trainingRecord.Id });
            GetFactory<TrainingRecordScheduledEmployeeFactory>()
                .Create(new { Employee = employee, LinkedId = trainingRecordFuture.Id });
            Session.Flush();
            var search = new SearchEmployeeTraining();
            _target.ControllerContext = new ControllerContext();

            var bareResult = _target.Index(search);
            var result = bareResult as ViewResult;
            var resultModel = ((SearchEmployeeTraining)result.Model).Results.ToList();

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(1, resultModel.Count);
        }

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var positionGroupCommonName = GetFactory<PositionGroupCommonNameFactory>().Create();
            var positionGroup = GetFactory<PositionGroupFactory>().Create(new { Description = "position group", CommonName = positionGroupCommonName });
            var employee = GetFactory<ActiveEmployeeFactory>().Create(new { FirstName = "Mark", LastName = "Thomas", MiddleName = "G", OperatingCenter = operatingCenter, PositionGroup = positionGroup });
            var moduleCategory = GetEntityFactory<TrainingModuleCategory>().Create(new { Description = "Safety" });
            var trainingRequirement = GetEntityFactory<TrainingRequirement>().Create(new { IsOSHARequirement = true, IsActive = true });
            trainingRequirement.PositionGroupCommonNames.Add(positionGroupCommonName);
            trainingRequirement.TrainingFrequencyUnit = "Y";
            trainingRequirement.TrainingFrequency = 2;
            Session.Save(trainingRequirement);
            var trainingModule = GetEntityFactory<TrainingModule>().Create(new
            {
                Title = "Bloodborne Pathogens - General (OSHA: 1910.103) - Annual course",
                CourseApprovalNumber = "05-061401-31",
                TCHCertified = true,
                TrainingModuleCategory = moduleCategory,
                IsActive = true,
                TCHCreditValue = 1f,
                TrainingRequirement = trainingRequirement,
                TotalHours = 8f
            });
            var trainingRecordOlder = GetEntityFactory<TrainingRecord>().Create(new
            {
                TrainingModule = trainingModule,
                HeldOn = DateTime.Now.AddYears(-1),
                ScheduledDate = DateTime.Now.AddYears(-1)
            });
            var trainingRecord = GetEntityFactory<TrainingRecord>().Create(new
            {
                TrainingModule = trainingModule,
                HeldOn = DateTime.Now,
                ScheduledDate = DateTime.Now,
            });
            var trainingRecordFuture = GetEntityFactory<TrainingRecord>().Create(new
            {
                TrainingModule = trainingModule,
                ScheduledDate = DateTime.Now.AddYears(1)
            });
            GetFactory<TrainingRecordAttendedEmployeeFactory>()
                .Create(new { Employee = employee, LinkedId = trainingRecordOlder.Id });
            GetFactory<TrainingRecordAttendedEmployeeFactory>()
                .Create(new { Employee = employee, LinkedId = trainingRecord.Id });
            GetFactory<TrainingRecordScheduledEmployeeFactory>()
                .Create(new { Employee = employee, LinkedId = trainingRecordFuture.Id });
            Session.Flush();
            var search = new SearchEmployeeTraining();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(employee.LastName, "LastName");
            }
        }

        [TestMethod]
        public void TestIndexFragReturnsNotFoundIfNoRecords()
        {
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var positionGroupCommonName = GetFactory<PositionGroupCommonNameFactory>().Create();
            var positionGroup = GetFactory<PositionGroupFactory>().Create(new { Description = "position group", CommonName = positionGroupCommonName });
            var employee = GetFactory<ActiveEmployeeFactory>().Create(new { FirstName = "Mark", LastName = "Thomas", MiddleName = "G", OperatingCenter = operatingCenter, PositionGroup = positionGroup });
            var moduleCategory = GetEntityFactory<TrainingModuleCategory>().Create(new { Description = "Safety" });
            
            var search = new SearchEmployeeTraining();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.FRAGMENT;

            var result = _target.Index(search) as PartialViewResult;
            Assert.AreEqual("_NoResults", result.ViewName);
        }

        #endregion
	}
}
