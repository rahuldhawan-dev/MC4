using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Reports.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Moq;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class EmployeeTrainingRecordExportControllerTest : MapCallMvcControllerTestBase<EmployeeTrainingRecordExportController, Employee>
    {
        #region Private Members

        private Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _dateTimeProvider = new Mock<IDateTimeProvider>();

            _container.Inject(_dateTimeProvider.Object);
        }
        
        #endregion
        
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = EmployeeTrainingRecordExportController.ROLE;
            Authorization.Assert(a => {
                a.RequiresRole("~/Reports/EmployeeTrainingRecordExport/Search/", role, RoleActions.UserAdministrator);
                a.RequiresRole("~/Reports/EmployeeTrainingRecordExport/Index/", role, RoleActions.UserAdministrator);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            var dataType = GetEntityFactory<DataType>().Create(new { TableName = TrainingRecordMap.TABLE_NAME, Name = TrainingRecord.DataTypeNames.EMPLOYEES_ATTENDED });
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var employee = GetEntityFactory<Employee>().Create(new { FirstName = "Mark", LastName = "Thomas", MiddleName = "G", OperatingCenter = operatingCenter });
            var instructor = GetEntityFactory<Employee>().Create();
            var secondInstructor = GetEntityFactory<Employee>().Create();
            var classLocation = GetEntityFactory<ClassLocation>().Create(new { Description = "Conference Room", OperatingCenter = operatingCenter });
            var learnItemType = GetEntityFactory<LEARNItemType>().Create(new { Abbreviation = "CIL" });
            var trainingModule1 = GetEntityFactory<TrainingModule>().Create(new { TotalHours = 16f, AmericanWaterCourseNumber = "1", LEARNItemType = learnItemType });
            var trainingModule2 = GetEntityFactory<TrainingModule>().Create(new { TotalHours = 8f, AmericanWaterCourseNumber = "2", LEARNItemType = learnItemType });
            var trainingModule3 = GetEntityFactory<TrainingModule>().Create(new { LEARNItemType = learnItemType });

            #region the good

            var trainingRecord1 = GetEntityFactory<TrainingRecord>().Create(new
            {
                TrainingModule = trainingModule1,
                ClassLocation = classLocation,
                HeldOn = new DateTime(2003, 6, 27, 12, 30, 00),
                //Instructor = instructor,
                OutsideInstructor = "Dr. Brown",
                SecondInstructor = secondInstructor
            });
            var trainingSession1 = GetEntityFactory<TrainingSession>().Create(new
            {
                TrainingRecord = trainingRecord1,
                StartDateTime = new DateTime(2003, 6, 27, 12, 30, 00),
                EndDateTime = new DateTime(2003, 6, 27, 13, 30, 00)
            });
            GetEntityFactory<EmployeeLink>().Create(new { Employee = employee, LinkedId = trainingRecord1.Id, DataType = dataType });

            var trainingRecord2 = GetEntityFactory<TrainingRecord>().Create(new
            {
                TrainingModule = trainingModule2,
                ClassLocation = classLocation,
                HeldOn = new DateTime(2004, 6, 27, 12, 30, 00),
                Instructor = instructor,
                SecondInstructor = secondInstructor
            });
            var trainingSession2 = GetEntityFactory<TrainingSession>().Create(new
            {
                TrainingRecord = trainingRecord2,
                StartDateTime = new DateTime(2004, 6, 27, 12, 30, 00),
                EndDateTime = new DateTime(2004, 6, 27, 13, 30, 00)
            });
            var trainingSession3 = GetEntityFactory<TrainingSession>().Create(new
            {
                TrainingRecord = trainingRecord2,
                StartDateTime = new DateTime(2004, 6, 28, 12, 30, 00),
                EndDateTime = new DateTime(2004, 6, 28, 13, 30, 00)
            });
            GetEntityFactory<EmployeeLink>().Create(new { Employee = employee, LinkedId = trainingRecord2.Id, DataType = dataType });

            #endregion

            #region the bad

            var badTrainingRecordNoAmericanWaterCourseNumber = GetEntityFactory<TrainingRecord>().Create(new { TrainingModule = trainingModule3, ClassLocation = classLocation, HeldOn = new DateTime(2004, 6, 27, 12, 30, 00), Instructor = instructor, SecondInstructor = secondInstructor });
            GetEntityFactory<EmployeeLink>().Create(new { Employee = employee, LinkedId = badTrainingRecordNoAmericanWaterCourseNumber.Id, DataType = dataType });

            var badTrainingRecordAlreadyExported = GetEntityFactory<TrainingRecord>().Create(new { TrainingModule = trainingModule1, ClassLocation = classLocation, HeldOn = new DateTime(2001, 6, 27, 12, 30, 00), Instructor = instructor, SecondInstructor = secondInstructor, AttendeesExportedDate = new DateTime(2002, 6, 27, 12, 30, 00) });
            GetEntityFactory<EmployeeLink>().Create(new { Employee = employee, LinkedId = badTrainingRecordAlreadyExported.Id, DataType = dataType });

            #endregion

            Session.Flush();
            Session.Clear();

            var search= new SearchEmployeeTrainingRecordExport();
            
            var result = _target.Index(search) as ViewResult;
            var resultModel = ((SearchEmployeeTrainingRecordExport)result.Model).Results.ToList();

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(2, resultModel.Count);
        }

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var now = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(now);
            var dataType = GetEntityFactory<DataType>().Create(new { TableName = TrainingRecordMap.TABLE_NAME, Name = TrainingRecord.DataTypeNames.EMPLOYEES_ATTENDED });
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var employee = GetEntityFactory<Employee>().Create(new { FirstName = "Mark", LastName = "Thomas", MiddleName = "G", OperatingCenter = operatingCenter });
            var instructor = GetEntityFactory<Employee>().Create();
            var secondInstructor = GetEntityFactory<Employee>().Create();
            var classLocation = GetEntityFactory<ClassLocation>().Create(new { Description = "Conference Room", OperatingCenter = operatingCenter });
            var learnItemType = GetEntityFactory<LEARNItemType>().Create(new { Abbreviation = "CIL" });
            var trainingModule1 = GetEntityFactory<TrainingModule>().Create(new { TotalHours = 16f, AmericanWaterCourseNumber = "1", TCHCreditValue = 4f, LEARNItemType = learnItemType });
            var trainingModule2 = GetEntityFactory<TrainingModule>().Create(new { TotalHours = 8f, AmericanWaterCourseNumber = "2", TCHCreditValue = 5f, LEARNItemType = learnItemType });
            var trainingModule3 = GetEntityFactory<TrainingModule>().Create(new { LEARNItemType = learnItemType });

            #region the good

            var trainingRecord1 = GetEntityFactory<TrainingRecord>().Create(new
            {
                TrainingModule = trainingModule1,
                ClassLocation = classLocation,
                HeldOn = new DateTime(2003, 6, 27, 12, 30, 00),
                //Instructor = instructor,
                OutsideInstructor = "Dr. Brown",
                SecondInstructor = secondInstructor
            });
            var trainingSession1 = GetEntityFactory<TrainingSession>().Create(new
            {
                TrainingRecord = trainingRecord1,
                StartDateTime = new DateTime(2003, 6, 27, 12, 30, 00),
                EndDateTime = new DateTime(2003, 6, 27, 13, 30, 00)
            });
            GetEntityFactory<EmployeeLink>().Create(new { Employee = employee, LinkedId = trainingRecord1.Id, DataType = dataType });

            var trainingRecord2 = GetEntityFactory<TrainingRecord>().Create(new
            {
                TrainingModule = trainingModule2,
                ClassLocation = classLocation,
                HeldOn = new DateTime(2004, 6, 27, 12, 30, 00),
                Instructor = instructor,
                SecondInstructor = secondInstructor
            });
            var trainingSession2 = GetEntityFactory<TrainingSession>().Create(new
            {
                TrainingRecord = trainingRecord2,
                StartDateTime = new DateTime(2004, 6, 27, 12, 30, 00),
                EndDateTime = new DateTime(2004, 6, 27, 13, 30, 00)
            });
            var trainingSession3 = GetEntityFactory<TrainingSession>().Create(new
            {
                TrainingRecord = trainingRecord2,
                StartDateTime = new DateTime(2004, 6, 28, 12, 30, 00),
                EndDateTime = new DateTime(2004, 6, 28, 13, 30, 00)
            });
            GetEntityFactory<EmployeeLink>().Create(new { Employee = employee, LinkedId = trainingRecord2.Id, DataType = dataType });

            #endregion

            #region the bad

            var badTrainingRecordNoAmericanWaterCourseNumber = GetEntityFactory<TrainingRecord>().Create(new { TrainingModule = trainingModule3, ClassLocation = classLocation, HeldOn = new DateTime(2004, 6, 27, 12, 30, 00), Instructor = instructor, SecondInstructor = secondInstructor });
            GetEntityFactory<EmployeeLink>().Create(new { Employee = employee, LinkedId = badTrainingRecordNoAmericanWaterCourseNumber.Id, DataType = dataType });

            var badTrainingRecordHeldOn = GetEntityFactory<TrainingRecord>().Create(new { TrainingModule = trainingModule1, ClassLocation = classLocation, HeldOn = new DateTime(2001, 6, 27, 12, 30, 00), Instructor = instructor, SecondInstructor = secondInstructor });
            GetEntityFactory<EmployeeLink>().Create(new { Employee = employee, LinkedId = badTrainingRecordHeldOn.Id, DataType = dataType });

            #endregion

            var search = new SearchEmployeeTrainingRecordExport();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(employee.EmployeeId, "User ID");
                helper.AreEqual(EmployeeTrainingRecordExportItem.ITEM_TYPE, "LEARN Item Type");
                helper.AreEqual(trainingModule1.AmericanWaterCourseNumber, "Item ID");
                helper.AreEqual(EmployeeTrainingRecordExportItem.COMPLETION_STATUS, "Completion Status");
                helper.AreEqual(String.Format(CommonStringFormats.DATE, trainingSession1.EndDateTime), "Completion Date");
                helper.AreEqual(String.Format(CommonStringFormats.TIME_12, trainingSession1.EndDateTime), "Completion Time");
                helper.AreEqual(trainingModule1.TotalHours, "Total Hours");
                helper.AreEqual(trainingModule1.TCHCreditValue, "Contact Hours");
                //helper.AreEqual(trainingRecord1.Instructor.FullName, "InstructorName");
            }

            trainingRecord1 = Session.Load<TrainingRecord>(trainingRecord1.Id);
            MyAssert.AreClose(now, trainingRecord1.AttendeesExportedDate.Value);
            badTrainingRecordHeldOn = Session.Load<TrainingRecord>(badTrainingRecordHeldOn.Id);
            Assert.IsFalse(badTrainingRecordHeldOn.AttendeesExportedDate.HasValue);
        }
    }
}