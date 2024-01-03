using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Data;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Moq;
using NHibernate.Linq;
using StructureMap;
using System;
using System.Linq;
using MapCall.Common.Model.Repositories.Users;
using MMSINC.Data.NHibernate;
using MapCall.Common.Model.Entities.Users;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class EmployeeRepositoryTest : MapCallMvcInMemoryDatabaseTestBase<Employee, EmployeeRepository>
    {
        #region Fields

        private EmployeeStatus _activeStatus, _inactiveStatus;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private DateTime _now;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Singleton().Use((_dateTimeProvider = new Mock<IDateTimeProvider>()).Object);
            e.For(typeof(IRepository<>)).Use(typeof(RepositoryBase<>));
            e.For<IEmployeeStatusRepository>().Use<EmployeeStatusRepository>();
            e.For<ICommercialDriversLicenseProgramStatusRepository>()
             .Use<CommercialDriversLicenseProgramStatusRepository>();
            e.For<IUserRepository>().Use<UserRepository>();
            e.For<IUserTypeRepository>().Use<UserTypeRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _now = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(_now);

            _activeStatus = GetFactory<ActiveEmployeeStatusFactory>().Create();
            _inactiveStatus = GetFactory<InactiveEmployeeStatusFactory>().Create();

            // This just needs to exist.
            GetFactory<InternalUserTypeFactory>().Create();
            SetupSupportData();
        }

        private void SetupSupportData()
        {
            GetFactory<InProgramCommercialDriversLicenseProgramStatusFactory>().Create();
            GetFactory<PursingCommercialDriversLicenseProgramStatusFactory>().Create();
            GetFactory<NotInProgramCommercialDriversLicenseProgramStatusFactory>().Create();
        }

        #endregion

        #region Tests

        #region Cascades

        [TestMethod]
        public void TestGetByEmployeeIdReturnsEmployeeWithEmployeeId()
        {
            var employeeId = "12341234";
            var fac = GetEntityFactory<Employee>();
            var employee = fac.Create(new {EmployeeId = employeeId});

            var target = Repository.GetByEmployeeId(employeeId);

            Assert.AreEqual(employee, target);

            target = Repository.GetByEmployeeId("1234");

            Assert.IsNull(target);
        }

        [TestMethod]
        public void TestGetByOperatingCenterIdReturnsEmployeesWithOperatingCenterId()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var good = GetFactory<EmployeeFactory>().Create(new {OperatingCenter = opc});
            var invalid = GetFactory<EmployeeFactory>().Create();

            var result = Repository.GetByOperatingCenterId(new[] {opc.Id}).ToArray();

            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains(good));
            Assert.IsFalse(result.Contains(invalid));
        }

        [TestMethod]
        public void TestGetActiveEmployeesByOperatingCenterIdReturnsActiveEmployeesWithOperatingCenterId()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();

            var good =
                GetFactory<EmployeeFactory>()
                   .Create(new {OperatingCenter = opc, Status = typeof(ActiveEmployeeStatusFactory)});
            var invalid = GetFactory<EmployeeFactory>().Create();
            var inactiveEmployee =
                GetFactory<EmployeeFactory>()
                   .Create(new {OperatingCenter = opc, Status = typeof(InactiveEmployeeStatusFactory)});

            var result = Repository.GetActiveEmployeesByOperatingCenterId(opc.Id).ToArray();

            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains(good));
            Assert.IsFalse(result.Contains(invalid));
            Assert.IsFalse(result.Contains(inactiveEmployee));
        }

        [TestMethod]
        public void TestGetEmployeesByOperatingCentersReturnsEmployeesWithOperatingCenters()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();

            var activeEmployee =
                GetFactory<EmployeeFactory>()
                   .Create(new { OperatingCenter = opc, Status = typeof(ActiveEmployeeStatusFactory) });
            var inactiveEmployee =
                GetFactory<EmployeeFactory>()
                   .Create(new { OperatingCenter = opc, Status = typeof(InactiveEmployeeStatusFactory) });

            var result = Repository.GetEmployeesByOperatingCenters(opc.Id).ToArray();

            Assert.AreEqual(2, result.Count());
            CollectionAssert.Contains(result, activeEmployee);
            CollectionAssert.Contains(result, inactiveEmployee);
        }

        #endregion

        #region EmployeesWithCommercialDriversLicenseRenewalsDue

        [TestMethod]
        public void TestGetEmployeesWithCommercialDriversLicenseRenewalsDueReturnsCorrectValues()
        {
            var ipcdlps = GetFactory<InProgramCommercialDriversLicenseProgramStatusFactory>().Create();
            var inProgramEmployees = GetEntityFactory<Employee>().CreateList(5,
                new {CommercialDriversLicenseProgramStatus = ipcdlps, Status = _activeStatus});
            var driversLicenseClassA = GetEntityFactory<DriversLicenseClass>().Create(new {Description = "A"});

            var licenseDueRenewalToday = GetEntityFactory<DriversLicense>().Create(new {
                RenewalDate = DateTime.Today, IssuedDate = DateTime.Now.AddYears(-1), Employee = inProgramEmployees[0],
                DriversLicenseClass = driversLicenseClassA
            });

            var licenseDueRenewalTwoMonthsBeginningDay = GetEntityFactory<DriversLicense>().Create(new {
                RenewalDate = DateTime.Today.AddDays(60).BeginningOfDay(), IssuedDate = DateTime.Now.AddYears(-1),
                Employee = inProgramEmployees[1], DriversLicenseClass = driversLicenseClassA
            });
            var licenseDueRenewalTwoMonthsEndOfDay = GetEntityFactory<DriversLicense>().Create(new {
                RenewalDate = DateTime.Today.AddDays(60).EndOfDay(), IssuedDate = DateTime.Now.AddYears(-1),
                Employee = inProgramEmployees[3], DriversLicenseClass = driversLicenseClassA
            });
            var licenseDueRenewalTwoMonthsNow = GetEntityFactory<DriversLicense>().Create(new {
                RenewalDate = DateTime.Now.AddDays(60), IssuedDate = DateTime.Now.AddYears(-1),
                Employee = inProgramEmployees[2], DriversLicenseClass = driversLicenseClassA
            });

            var licenseNotDueRenewal = GetEntityFactory<DriversLicense>().Create(new {
                RenewalDate = DateTime.Today.AddDays(100), IssuedDate = DateTime.Now.AddYears(-1),
                Employee = inProgramEmployees[4], DriversLicenseClass = driversLicenseClassA
            });
            var licenseDueRenewalForEmployeeNotInProgram = GetEntityFactory<DriversLicense>().Create(new {
                RenewalDate = DateTime.Now.AddDays(60),
                IssuedDate = DateTime.Now.AddYears(-1),
                DriversLicenseClass = driversLicenseClassA
            });

            var inactiveEmployee = GetEntityFactory<Employee>().Create(new
                {CommercialDriversLicenseProgramStatus = ipcdlps, Status = _inactiveStatus});
            var inactiveLicenseDueRenewalTwoMonthsBeginningDay = GetEntityFactory<DriversLicense>().Create(new {
                RenewalDate = DateTime.Today.AddDays(60).BeginningOfDay(), IssuedDate = DateTime.Now.AddYears(-1),
                Employee = inactiveEmployee, DriversLicenseClass = driversLicenseClassA
            });
            var inactiveEmployee2 = GetEntityFactory<Employee>()
               .Create(new {CommercialDriversLicenseProgramStatus = ipcdlps});
            inactiveEmployee2.Status = null;
            Repository.Save(inactiveEmployee2);

            var inactiveLicenseDueRenewalTwoMonthsBeginningDay2 = GetEntityFactory<DriversLicense>().Create(new {
                RenewalDate = DateTime.Today.AddDays(60).BeginningOfDay(), IssuedDate = DateTime.Now.AddYears(-1),
                Employee = inactiveEmployee2, DriversLicenseClass = driversLicenseClassA
            });

            var results = Repository.GetEmployeesWithCommercialDriversLicenseRenewalsDueInTwoMonths(null);

            Assert.AreEqual(3, results.Count());
        }

        #endregion

        #region GetRecentTraining

        [TestMethod]
        public void TestGetRecentTrainingReturnsNJ7EmployeeThatAttendingTrainingRecordForTrainingModuleBloodborne()
        {
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var positionGroupCommonName = GetFactory<PositionGroupCommonNameFactory>().Create();
            var positionGroup = GetFactory<PositionGroupFactory>().Create(new
                {Description = "position group", CommonName = positionGroupCommonName});
            var employee = GetFactory<EmployeeFactory>().Create(new {
                FirstName = "Mark", LastName = "Thomas", MiddleName = "G", OperatingCenter = operatingCenter,
                PositionGroup = positionGroup, Status = _activeStatus
            });
            var moduleCategory = GetEntityFactory<TrainingModuleCategory>().Create(new {Description = "Safety"});
            var trainingRequirement = GetEntityFactory<TrainingRequirement>()
               .Create(new {IsOSHARequirement = true, IsActive = true});
            trainingRequirement.PositionGroupCommonNames.Add(positionGroupCommonName);
            trainingRequirement.TrainingFrequencyUnit = "Y";
            trainingRequirement.TrainingFrequency = 2;
            Session.Save(trainingRequirement);
            var trainingModule = GetEntityFactory<TrainingModule>().Create(new {
                Title = "Bloodborne Pathogens - General (OSHA: 1910.103) - Annual course",
                CourseApprovalNumber = "05-061401-31",
                TCHCertified = true,
                TrainingModuleCategory = moduleCategory,
                IsActive = true,
                TCHCreditValue = 1f,
                TrainingRequirement = trainingRequirement,
                TotalHours = 8f
            });
            var trainingRecordOlder = GetEntityFactory<TrainingRecord>().Create(new {
                TrainingModule = trainingModule,
                HeldOn = DateTime.Now.AddYears(-1),
                ScheduledDate = DateTime.Now.AddYears(-1)
            });
            var trainingRecord = GetEntityFactory<TrainingRecord>().Create(new {
                TrainingModule = trainingModule,
                HeldOn = DateTime.Now,
                ScheduledDate = DateTime.Now,
            });
            var trainingRecordFuture = GetEntityFactory<TrainingRecord>().Create(new {
                TrainingModule = trainingModule,
                ScheduledDate = DateTime.Now.AddYears(1)
            });
            GetFactory<TrainingRecordAttendedEmployeeFactory>()
               .Create(new {Employee = employee, LinkedId = trainingRecordOlder.Id});
            GetFactory<TrainingRecordAttendedEmployeeFactory>()
               .Create(new {Employee = employee, LinkedId = trainingRecord.Id});
            GetFactory<TrainingRecordScheduledEmployeeFactory>()
               .Create(new {Employee = employee, LinkedId = trainingRecordFuture.Id});
            Session.Flush();

            var results = Repository.GetRecentTraining(new TestSearchEmployeeTraining());

            Assert.AreEqual(1, results.Count());
            var result = results.First();
            Assert.AreEqual(operatingCenter.OperatingCenterCode, result.OperatingCenter);
            Assert.AreEqual(positionGroupCommonName.Description, result.CommonName);
            Assert.AreEqual(employee.FirstName, result.FirstName);
            Assert.AreEqual(employee.LastName, result.LastName);
            Assert.AreEqual(trainingRequirement.IsOSHARequirement, result.OSHARequirement);
            Assert.AreEqual(positionGroup.PositionDescription, result.Position);
            Assert.AreEqual(positionGroup.Group, result.PositionGroup);
            MyAssert.AreClose(trainingRecordFuture.ScheduledDate.Value, result.NextScheduledDate.Value);
            MyAssert.AreClose(trainingRecord.HeldOn.Value, result.RecentTrainingDate.Value);
            Assert.AreEqual(trainingModule.TotalHours, result.TotalHours);
            Assert.AreEqual(trainingRequirement.Description, result.TrainingRequirement);
            Session.Clear();
            Session.Flush();
            trainingRecord = Session.Get<TrainingRecord>(trainingRecord.Id);
            Assert.AreEqual(trainingRecord.NextDueDate.Value, result.NextDueByDate);
        }

        // Aumack Example - Bug 2572
        [TestMethod]
        public void TestAumackReturnsTheCorrectRowsFromBug2572()
        {
            var dataTypeEmployeesAttended = GetEntityFactory<DataType>().Create(new
                {TableName = TrainingRecordMap.TABLE_NAME, Name = TrainingRecord.DataTypeNames.EMPLOYEES_ATTENDED});
            var dataTypeEmployeesScheduled = GetEntityFactory<DataType>().Create(new
                {TableName = TrainingRecordMap.TABLE_NAME, Name = TrainingRecord.DataTypeNames.EMPLOYEES_SCHEDULED});
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create(new {OperatingCenterCode = "NJ7"});
            var positionGroupCommonName = GetFactory<PositionGroupCommonNameFactory>()
               .Create(new {Description = "Utility Mechanic/Inspector Non-Supv"});
            var positionGroup = GetFactory<PositionGroupFactory>().Create(new
                {Group = "UTYMCH4", Description = "Utility Mechanic IV", CommonName = positionGroupCommonName});
            var aumack = GetFactory<EmployeeFactory>().Create(new {
                FirstName = "Robert", LastName = "Aumack Jr.", OperatingCenter = operatingCenter,
                PositionGroup = positionGroup, Status = _activeStatus
            });
            var walters = GetFactory<EmployeeFactory>().Create(new {
                FirstName = "Marty", LastName = "Walters", OperatingCenter = operatingCenter,
                PositionGroup = positionGroup, Status = _inactiveStatus
            });
            var moduleCategory = GetEntityFactory<TrainingModuleCategory>().Create(new {Description = "Safety"});
            var trainingRequirement = GetEntityFactory<TrainingRequirement>().Create(new {
                Description = "Power Industrial Trucks/Fork Lifts",
                IsOSHARequirement = true,
                IsActive = true
            });
            var originalTrainingModule = GetEntityFactory<TrainingModule>().Create(new {
                TrainingRequirement = trainingRequirement,
                TrainingModuleCategory = moduleCategory,
                Title = "ForkLift Training"
            });
            var replacementTrainingModule = GetEntityFactory<TrainingModule>().Create(new {
                TrainingRequirement = trainingRequirement,
                TrainingModuleCategory = moduleCategory,
                Title = "Powered Industrial Trucks (forklift operator training) - OSHA 1910.178"
            });
            trainingRequirement.PositionGroupCommonNames.Add(positionGroupCommonName);
            trainingRequirement.TrainingFrequencyUnit = "D";
            trainingRequirement.TrainingFrequency = 1095;
            trainingRequirement.ActiveInitialAndRecurringTrainingModule = replacementTrainingModule;
            Session.Save(trainingRequirement);
            // create a training record and have aumack and walters attend.
            var trainingRecord = GetEntityFactory<TrainingRecord>()
               .Create(new {TrainingModule = originalTrainingModule, HeldOn = DateTime.Now});
            GetEntityFactory<EmployeeLink>()
               .Create(new {Employee = aumack, LinkedId = trainingRecord.Id, DataType = dataTypeEmployeesAttended});
            GetEntityFactory<EmployeeLink>()
               .Create(new {Employee = walters, LinkedId = trainingRecord.Id, DataType = dataTypeEmployeesAttended});

            // lets throw a second requirement/module into the mix, to make sure it isn't assuming the values from the other one.
            var asbTrainingRequirement = GetEntityFactory<TrainingRequirement>().Create(new
                {Description = "Asbestos Awareness", IsOSHARequirement = true, IsActive = true});
            var asbTrainingModule = GetEntityFactory<TrainingModule>().Create(new {
                TrainingRequirement = asbTrainingRequirement, TrainingModuleCategory = moduleCategory,
                Title = "Asbestos Awareness"
            });
            asbTrainingRequirement.PositionGroupCommonNames.Add(positionGroupCommonName);
            asbTrainingRequirement.TrainingFrequency = 365;
            asbTrainingRequirement.TrainingFrequencyUnit = "D";
            asbTrainingRequirement.ActiveInitialTrainingModule = asbTrainingModule;
            Session.Save(asbTrainingRequirement);

            Session.Flush();
            Session.Clear();

            var results = Repository.GetRecentTraining(new TestSearchEmployeeTraining());

            Assert.AreEqual(2, results.Count());

            var first = results.First();
            var last = results.Last();
            Assert.AreEqual(aumack.LastName, first.LastName, "The one record is Aumack not Walters, Active Only");
            Assert.AreEqual(aumack.LastName, last.LastName, "The one record is Aumack not Walters, Active Only");

            Assert.AreNotEqual(first.NextDueByDate, last.NextDueByDate);

            MyAssert.AreClose(trainingRecord.HeldOn.Value,
                last.RecentTrainingDate
                    .Value); // "Aumack has a date in for the training he attended that was replace with a newer module");
            // Ensure the next due by date is set for the report
            MyAssert.AreClose(trainingRecord.HeldOn.Value.AddDays(trainingRequirement.TrainingFrequency.Value),
                last.NextDueByDate.Value);

            // Schedule a new training record, setup aumack to attend, ensure that's the next scheduled date returned
            var trainingRecordNew = GetEntityFactory<TrainingRecord>().Create(new
                {TrainingModule = replacementTrainingModule, ScheduledDate = DateTime.Now.AddYears(1)});
            GetEntityFactory<EmployeeLink>()
               .Create(new {Employee = aumack, LinkedId = trainingRecordNew.Id, DataType = dataTypeEmployeesScheduled});

            results = Repository.GetRecentTraining(new TestSearchEmployeeTraining());

            Assert.AreEqual(2, results.Count());
            last = results.Last();
            MyAssert.AreClose(DateTime.Now.AddYears(1), last.NextScheduledDate.Value);
        }

        #endregion

        #region GetTrainingByDate

        [TestMethod]
        public void TestGetTrainingByDateDoesFilterByDateAttendedWhenDateAttendedIsSearchedFor()
        {
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var employee = GetFactory<EmployeeFactory>().Create(new {
                FirstName = "Mark", LastName = "Thomas", MiddleName = "G", OperatingCenter = operatingCenter,
                Status = _activeStatus
            });

            var trainingRecordOlder = GetEntityFactory<TrainingRecord>().Create(new {
                HeldOn = DateTime.Now.AddYears(-1),
                ScheduledDate = DateTime.Now.AddYears(-1)
            });
            var trainingRecord = GetEntityFactory<TrainingRecord>().Create(new {
                HeldOn = DateTime.Now,
                ScheduledDate = DateTime.Now,
            });
            GetFactory<TrainingRecordAttendedEmployeeFactory>()
               .Create(new {Employee = employee, LinkedId = trainingRecordOlder.Id});
            GetFactory<TrainingRecordAttendedEmployeeFactory>()
               .Create(new {Employee = employee, LinkedId = trainingRecord.Id});
            Session.Flush();

            var results = Repository.GetTrainingByDate(new TestSearchEmployeeTrainingByDate());

            // There should be two results because there are two training records and no date has been searched for.
            Assert.AreEqual(2, results.Count());

            var search = new TestSearchEmployeeTrainingByDate();
            search.DateAttended = new DateRange {
                Start = DateTime.Now.AddDays(-1),
                End = DateTime.Now.AddDays(1),
                Operator = RangeOperator.Between
            };

            results = Repository.GetTrainingByDate(search);
            Assert.AreEqual(1, results.Count());
        }

        [TestMethod]
        public void TestGetTrainingByDateReturnsAllTrainingNotJustRequired()
        {
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var positionGroupCommonName = GetFactory<PositionGroupCommonNameFactory>().Create();
            var positionGroup = GetFactory<PositionGroupFactory>().Create(new
                {Description = "position group", CommonName = positionGroupCommonName});
            var employee = GetFactory<EmployeeFactory>().Create(new {
                FirstName = "Mark", LastName = "Thomas", MiddleName = "G", OperatingCenter = operatingCenter,
                PositionGroup = positionGroup, Status = _activeStatus
            });
            var moduleCategory = GetEntityFactory<TrainingModuleCategory>().Create(new {Description = "Safety"});
            //var trainingRequirement = GetEntityFactory<TrainingRequirement>().Create(new { IsOSHARequirement = true, IsActive = true });
            //trainingRequirement.PositionGroupCommonNames.Add(positionGroupCommonName);
            //trainingRequirement.TrainingFrequencyUnit = "Y";
            //trainingRequirement.TrainingFrequency = 2;
            //  Session.Save(trainingRequirement);
            var trainingModule = GetEntityFactory<TrainingModule>().Create(new {
                Title = "Bloodborne Pathogens - General (OSHA: 1910.103) - Annual course",
                CourseApprovalNumber = "05-061401-31",
                TCHCertified = true,
                TrainingModuleCategory = moduleCategory,
                IsActive = true,
                TCHCreditValue = 1f,
                //      TrainingRequirement = trainingRequirement,
                TotalHours = 8f
            });
            var trainingRecordOlder = GetEntityFactory<TrainingRecord>().Create(new {
                TrainingModule = trainingModule,
                HeldOn = DateTime.Now.AddYears(-1),
                ScheduledDate = DateTime.Now.AddYears(-1)
            });
            var trainingRecord = GetEntityFactory<TrainingRecord>().Create(new {
                TrainingModule = trainingModule,
                HeldOn = DateTime.Now,
                ScheduledDate = DateTime.Now,
            });
            var trainingRecordFuture = GetEntityFactory<TrainingRecord>().Create(new {
                TrainingModule = trainingModule,
                ScheduledDate = DateTime.Now.AddYears(1)
            });
            GetFactory<TrainingRecordAttendedEmployeeFactory>()
               .Create(new {Employee = employee, LinkedId = trainingRecordOlder.Id});
            GetFactory<TrainingRecordAttendedEmployeeFactory>()
               .Create(new {Employee = employee, LinkedId = trainingRecord.Id});
            GetFactory<TrainingRecordScheduledEmployeeFactory>()
               .Create(new {Employee = employee, LinkedId = trainingRecordFuture.Id});
            Session.Flush();

            var results = Repository.GetTrainingByDate(new TestSearchEmployeeTrainingByDate());

            Assert.AreEqual(2, results.Count(), "Both of the attended training records should be here.");
            var result = results.First();
            Assert.AreEqual(operatingCenter.OperatingCenterCode, result.OperatingCenter);
            Assert.AreEqual(positionGroupCommonName.Description, result.CommonName);
            Assert.AreEqual(employee.FirstName, result.FirstName);
            Assert.AreEqual(employee.LastName, result.LastName);
            //  Assert.AreEqual(trainingRequirement.IsOSHARequirement, result.OSHARequirement);
            Assert.AreEqual(positionGroup.PositionDescription, result.Position);
            Assert.AreEqual(positionGroup.Group, result.PositionGroup);
            MyAssert.AreClose(trainingRecordFuture.ScheduledDate.Value, result.NextScheduledDate.Value);
            MyAssert.AreClose(trainingRecord.HeldOn.Value, result.RecentTrainingDate.Value);
            Assert.AreEqual(trainingModule.TotalHours, result.TotalHours);
            //  Assert.AreEqual(trainingRequirement.Description, result.TrainingRequirement);
            Session.Clear();
            Session.Flush();
            //            trainingRecord = Session.Get<TrainingRecord>(trainingRecord.Id);
            //            Assert.AreEqual(trainingRecord.NextDueDate.Value, result.NextDueByDate);
        }

        #endregion

        #region GetEmployeeTrainingRecordExport

        [TestMethod]
        public void TestGetEmployeeTrainingRecordExportReturnsTheItemsThatItHasBeenInstructedToReturn()
        {
            var dataType = GetEntityFactory<DataType>().Create(new
                {TableName = TrainingRecordMap.TABLE_NAME, Name = TrainingRecord.DataTypeNames.EMPLOYEES_ATTENDED});
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var employee = GetEntityFactory<Employee>().Create(new
                {FirstName = "Mark", LastName = "Thomas", MiddleName = "G", OperatingCenter = operatingCenter});
            var instructor = GetEntityFactory<Employee>().Create();
            var secondInstructor = GetEntityFactory<Employee>().Create();
            var classLocation = GetEntityFactory<ClassLocation>()
               .Create(new {Description = "Conference Room", OperatingCenter = operatingCenter});
            var learnItemType = GetEntityFactory<LEARNItemType>().Create(new {Abbreviation = "CIL"});
            var trainingModule1 = GetEntityFactory<TrainingModule>().Create(new
                {TotalHours = 16f, AmericanWaterCourseNumber = "1", LEARNItemType = learnItemType});
            var trainingModule2 = GetEntityFactory<TrainingModule>().Create(new
                {TotalHours = 8f, AmericanWaterCourseNumber = "2", LEARNItemType = learnItemType});
            var trainingModule3 = GetEntityFactory<TrainingModule>()
               .Create(new {TotalHours = 3f, LEARNItemType = learnItemType});

            #region the good

            var trainingRecord1 = GetEntityFactory<TrainingRecord>().Create(new {
                TrainingModule = trainingModule1,
                ClassLocation = classLocation,
                HeldOn = new DateTime(2003, 6, 27, 12, 30, 00),
                //Instructor = instructor,
                OutsideInstructor = "Dr. Brown",
                SecondInstructor = secondInstructor
            });
            var trainingSession1 = GetEntityFactory<TrainingSession>().Create(new {
                TrainingRecord = trainingRecord1,
                StartDateTime = new DateTime(2003, 5, 27, 12, 30, 00),
                EndDateTime = new DateTime(2003, 5, 27, 13, 30, 00)
            });
            GetEntityFactory<EmployeeLink>().Create(new
                {Employee = employee, LinkedId = trainingRecord1.Id, DataType = dataType});

            var trainingRecord2 = GetEntityFactory<TrainingRecord>().Create(new {
                TrainingModule = trainingModule2,
                ClassLocation = classLocation,
                HeldOn = new DateTime(2004, 6, 27, 12, 30, 00),
                Instructor = instructor,
                SecondInstructor = secondInstructor
            });
            var trainingSession2 = GetEntityFactory<TrainingSession>().Create(new {
                TrainingRecord = trainingRecord2,
                StartDateTime = new DateTime(2004, 6, 27, 12, 30, 00),
                EndDateTime = new DateTime(2004, 6, 27, 13, 30, 00)
            });
            var trainingSession3 = GetEntityFactory<TrainingSession>().Create(new {
                TrainingRecord = trainingRecord2,
                StartDateTime = new DateTime(2004, 6, 28, 12, 30, 00),
                EndDateTime = new DateTime(2004, 6, 28, 13, 30, 00)
            });
            GetEntityFactory<EmployeeLink>().Create(new
                {Employee = employee, LinkedId = trainingRecord2.Id, DataType = dataType});

            var trainingRecordAlreadyExported = GetEntityFactory<TrainingRecord>().Create(new {
                TrainingModule = trainingModule1,
                ClassLocation = classLocation,
                HeldOn = new DateTime(2001, 6, 27, 12, 30, 00),
                Instructor = instructor,
                SecondInstructor = secondInstructor,
                AttendeesExportedDate = new DateTime(2002, 6, 27, 12, 30, 00)
            });
            var trainingSessionAlreadyExported = GetEntityFactory<TrainingSession>().Create(new {
                TrainingRecord = trainingRecordAlreadyExported,
                StartDateTime = new DateTime(2001, 7, 28, 12, 30, 00),
                EndDateTime = new DateTime(2001, 7, 28, 13, 30, 00)
            });
            GetEntityFactory<EmployeeLink>().Create(new
                {Employee = employee, LinkedId = trainingRecordAlreadyExported.Id, DataType = dataType});

            #endregion

            #region the bad

            var badTrainingRecordNoAmericanWaterCourseNumber = GetEntityFactory<TrainingRecord>().Create(new {
                TrainingModule = trainingModule3, ClassLocation = classLocation,
                HeldOn = new DateTime(2004, 6, 27, 12, 30, 00), Instructor = instructor,
                SecondInstructor = secondInstructor
            });
            GetEntityFactory<EmployeeLink>().Create(new
                {Employee = employee, LinkedId = badTrainingRecordNoAmericanWaterCourseNumber.Id, DataType = dataType});

            #endregion

            Session.Flush();
            Session.Clear();
            var results = Repository.GetEmployeeTrainingRecordExport(new TestSearchEmployeeTrainingRecordExport())
                                    .ToList();

            Assert.AreEqual(3, results.Count());
            var first = results[0];
            var middle = results[1];
            var last = results[2];
            Assert.AreEqual(last.EmployeeId, first.EmployeeId);
            Assert.AreEqual(trainingRecord1.OutsideInstructor, first.InstructorName);
            Assert.AreEqual(instructor.FullName, last.InstructorName);
            Assert.AreEqual(EmployeeTrainingRecordExportItem.ITEM_TYPE, first.ItemType);
            Assert.AreEqual(EmployeeTrainingRecordExportItem.COMPLETION_STATUS, first.CompletionStatus);
            Assert.AreEqual(String.Format(CommonStringFormats.DATE, trainingSession1.EndDateTime),
                first.CompletionDate);
            Assert.AreEqual(String.Format(CommonStringFormats.DATE, trainingSessionAlreadyExported.EndDateTime),
                last.CompletionDate);
            Assert.AreEqual(String.Format(CommonStringFormats.DATE, trainingSession3.EndDateTime),
                middle.CompletionDate);
        }

        #endregion

        #region GetEmployeesWithMedicalCertificatesDue

        private MedicalCertificate CreateMedicalCertificate(DateTime expiration)
        {
            return
                GetEntityFactory<MedicalCertificate>()
                   .Create(new {ExpirationDate = expiration, Employee = typeof(ActiveEmployeeFactory)});
        }

        [TestMethod]
        public void TestGetEmployeesWithMedicalCertificatesDueInTwoWeeksDoesThatThing()
        {
            var dueInOneWeek = CreateMedicalCertificate(_now.AddWeeks(1));
            var dueInThreeWeeks = CreateMedicalCertificate(_now.AddWeeks(3));
            var dueInTwoWeeks = CreateMedicalCertificate(_now.AddWeeks(2));

            var results = Repository.GetEmployeesWithMedicalCertificatesDueInTwoWeeks();

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(dueInTwoWeeks.Employee, results.First());
        }

        [TestMethod]
        public void TestGetEmployeesWithMedicalCertificatesDueInOneMonthDoesThatThing()
        {
            var dueInThreeWeeks = CreateMedicalCertificate(_now.AddDays(21));
            var dueInFiveWeeks = CreateMedicalCertificate(_now.AddDays(35));
            var dueInOneMonth = CreateMedicalCertificate(_now.AddDays(28));

            var results = Repository.GetEmployeesWithMedicalCertificatesDueInOneMonth();

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(dueInOneMonth.Employee, results.First());
        }

        [TestMethod]
        public void TestGetEmployeesWithMedicalCertificatesOverdueReturnsEmployeesWithCertificatesDueYesterday()
        {
            var dueTwoDaysAgo = CreateMedicalCertificate(_now.AddDays(-2));
            var dueToday = CreateMedicalCertificate(_now);
            var dueYesterday = CreateMedicalCertificate(_now.AddDays(-1));

            var results = Repository.GetEmployeesWithMedicalCertificatesOverdue();

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(dueYesterday.Employee, results.First());
        }

        #endregion

        #region GetActiveEmployeesByUserRole

        [TestMethod]
        public void TestGetActiveEmployeesByUserRoleReturnsEmployeesWithUserRoleRegardlessOfWhichRoleActionTheyHave()
        {
            var expectedRoleModule = RoleModules.OperationsLockoutForms;
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var employee = GetFactory<ActiveEmployeeFactory>().Create();
            var user = GetEntityFactory<User>().Create(new {Employee = employee});

            var addAction = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Add});
            var deleteAction = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Delete});

            // Test employee with user without any matching roles does not end up in list 
            var result = Repository.GetActiveEmployeesByUserRole(opc.Id, expectedRoleModule);
            Assert.IsFalse(result.Any());

            var role = GetFactory<RoleFactory>().Create(new {
                Application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.Operations}),
                Module = GetFactory<ModuleFactory>().Create(new {Id = expectedRoleModule}),
                Action = addAction,
                User = user
            });

            void doTest(RoleAction action, OperatingCenter opcForRole, bool mustBeInResult)
            {
                role.OperatingCenter = opcForRole;
                role.Action = action;
                Session.Save(role);
                Session.Flush();

                var result1 = Repository.GetActiveEmployeesByUserRole(opc.Id, expectedRoleModule);
                var testableResult = result1.Any();
                if (mustBeInResult)
                {
                    Assert.IsTrue(testableResult,
                        $"No result was found for action '{action.Description}' and operating center '{opcForRole}'.");
                }
                else
                {
                    Assert.IsFalse(testableResult,
                        $"An unexpected result was found for action '{action.Description}' and operating center '{opcForRole}'.");
                }
            }

            // user has wildcard match
            doTest(addAction, null, true);
            doTest(deleteAction, null, true);

            // user has opc match too
            doTest(addAction, opc, true);
            doTest(deleteAction, opc, true);

            // shouldn't match due to operating center mismatch
            doTest(addAction, GetEntityFactory<OperatingCenter>().Create(), false);

            // Shouldn't match due to inactive employee status
            employee.Status = GetFactory<InactiveEmployeeStatusFactory>().Create();
            Session.Save(employee);
            doTest(addAction, null, false);
        }

        #endregion

        #region GetEmployeesByUserRole

        [TestMethod]
        public void TestGetEmployeesByUserRoleReturnsEmployeesWithUserRoleRegardlessOfWhichRoleActionTheyHave()
        {
            var expectedRoleModule = RoleModules.OperationsLockoutForms;
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var employee = GetFactory<ActiveEmployeeFactory>().Create();
            var user = GetEntityFactory<User>().Create(new {Employee = employee});

            var addAction = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Add});
            var deleteAction = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Delete});

            // Test employee with user without any matching roles does not end up in list 
            var result = Repository.GetEmployeesByUserRole(opc.Id, expectedRoleModule);
            Assert.IsFalse(result.Any());

            var role = GetFactory<RoleFactory>().Create(new {
                Application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.Operations}),
                Module = GetFactory<ModuleFactory>().Create(new {Id = expectedRoleModule}),
                Action = addAction,
                User = user
            });

            void doTest(RoleAction action, OperatingCenter opcForRole, bool mustBeInResult)
            {
                role.OperatingCenter = opcForRole;
                role.Action = action;
                Session.Save(role);
                Session.Flush();

                var result1 = Repository.GetEmployeesByUserRole(opc.Id, expectedRoleModule);
                var testableResult = result1.Any();
                if (mustBeInResult)
                {
                    Assert.IsTrue(testableResult,
                        $"No result was found for action '{action.Description}' and operating center '{opcForRole}'.");
                }
                else
                {
                    Assert.IsFalse(testableResult,
                        $"An unexpected result was found for action '{action.Description}' and operating center '{opcForRole}'.");
                }
            }

            // user has wildcard match
            doTest(addAction, null, true);
            doTest(deleteAction, null, true);

            // user has opc match too
            doTest(addAction, opc, true);
            doTest(deleteAction, opc, true);

            // shouldn't match due to operating center mismatch
            doTest(addAction, GetEntityFactory<OperatingCenter>().Create(), false);
        }

        #endregion

        #region GetActiveEmployeesByOperatingCentersForRole

        [TestMethod]
        public void TestGetActiveEmployeesByOperatingCentersForRoleRegardlessOfWhichRoleActionTheyHave()
        {
            var expectedRoleModule = RoleModules.OperationsLockoutForms;
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var employee = GetFactory<ActiveEmployeeFactory>().Create(new {OperatingCenter = opc});
            var user = GetEntityFactory<User>().Create(new {Employee = employee});
            var userJr = GetFactory<AdminUserFactory>().Create();
            var addAction = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Add});
            var deleteAction = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Delete});

            // Test employee with user without any matching roles does not end up in list 
            var result = Repository.GetActiveEmployeesByOperatingCentersForRole(user, expectedRoleModule);
            Assert.IsFalse(result.Any());

            var role = GetFactory<RoleFactory>().Create(new {
                Application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.Operations}),
                Module = GetFactory<ModuleFactory>().Create(new {Id = expectedRoleModule}),
                Action = addAction,
                User = user
            });

            void doTest(RoleAction action, OperatingCenter opcForRole, bool mustBeInResult)
            {
                role.OperatingCenter = opcForRole;
                role.Action = action;
                Session.Save(role);
                Session.Flush();

                var result1 = Repository.GetActiveEmployeesByOperatingCentersForRole(user, expectedRoleModule);
                var testableResult = result1.Any();
                if (mustBeInResult)
                {
                    Assert.IsTrue(testableResult,
                        $"No result was found for action '{action.Description}' and operating center '{opcForRole}'.");
                }
                else
                {
                    Assert.IsFalse(testableResult,
                        $"An unexpected result was found for action '{action.Description}' and operating center '{opcForRole}'.");
                }
            }

            // user has wildcard match
            doTest(addAction, null, true);
            doTest(deleteAction, null, true);

            // user has opc match too
            doTest(addAction, opc, true);
            doTest(deleteAction, opc, true);

            // shouldn't match due to operating center mismatch
            doTest(addAction, GetEntityFactory<OperatingCenter>().Create(), false);
        }

        [TestMethod]
        public void TestGetActiveEmployeesByOperatingCentersForRoleShowsAllOpcForAdmin()
        {
            var expectedRoleModule = RoleModules.OperationsLockoutForms;
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var employee = GetFactory<ActiveEmployeeFactory>().Create(new {OperatingCenter = opc});
            var employee2 = GetFactory<ActiveEmployeeFactory>().Create(new {OperatingCenter = opc2});
            var user = GetEntityFactory<User>().Create(new {Employee = employee});
            var userJr = GetFactory<AdminUserFactory>().Create(new {Employee = employee2});

            var role = GetFactory<RoleFactory>().Create(new {
                Application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.Operations}),
                Module = GetFactory<ModuleFactory>().Create(new {Id = expectedRoleModule}),
                Action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Add}),
                OperatingCenter = opc,
                User = user
            });
            var result1 = Repository.GetActiveEmployeesByOperatingCentersForRole(userJr, expectedRoleModule);
            var testableResult = result1.Any();

            Assert.IsTrue(testableResult,
                $"No result was found for action '{role.Action.Description}' and operating center '{opc}'.");
        }

        #endregion

        #endregion

        #region Test Classes

        private class TestSearchEmployeeTraining : SearchSet<EmployeeTrainingReportItem>, ISearchEmployeeTraining
        {
            public int? OperatingCenter { get; set; }
            public bool? OSHARequirement { get; set; }
            public int? TrainingRequirement { get; set; }
            public int? PositionGroupCommonName { get; set; }
            public int? PositionGroup { get; set; }
            public string LastName { get; set; }
            public DateRange RecentTrainingDate { get; set; }
            public DateRange NextScheduledDate { get; set; }
            public DateRange NextDueByDate { get; set; }
        }

        private class TestSearchEmployeeTrainingRecordExport : SearchSet<EmployeeTrainingRecordExportItem> { }

        private class TestSearchEmployeeTrainingByDate : SearchSet<EmployeeTrainingByDateReportItem>,
            ISearchEmployeeTrainingByDate
        {
            public int? OperatingCenter { get; set; }
            public bool? OSHARequirement { get; set; }
            public int? TrainingRequirement { get; set; }
            public int? PositionGroupCommonName { get; set; }
            public int? PositionGroup { get; set; }
            public string LastName { get; set; }
            public DateRange RecentTrainingDate { get; set; }
            public DateRange NextScheduledDate { get; set; }
            public DateRange NextDueByDate { get; set; }
            public DateRange DateAttended { get; set; }
        }

        #endregion
    }
}
