using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;
using NHibernate;
using StructureMap;
using AdminUserFactory = MapCall.Common.Testing.Data.AdminUserFactory;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class IncidentRepositoryTest : MapCallMvcSecuredRepositoryTestBase<Incident,
        IncidentRepositoryTest.TestIncidentRepository, User>
    {
        #region Fields

        private Application _application;
        private Module _module;
        private RoleAction _readAction, _userAdminAction;

        private OperatingCenter _nj7, _nj4;

        private Employee _supervisorNJ7SupervisorSupervisor,
                         _supervisorNJ7Supervisor,
                         _supervisorNJ7,
                         _supervisorNJ4,
                         _employeeNJ7SupervisorNJ7,
                         _employeeNJ7SupervisorNJ4,
                         _employeeNJ4SupervisorNJ7,
                         _employeeNJ4SupervisorNJ4;

        private Incident _incidentNJ7EmployeeNJ7SupervisorNJ7,
                         _incidentNJ7EmployeeNJ7SupervisorNJ4,
                         _incidentNJ7EmployeeNJ4SupervisorNJ4,
                         _incidentNJ4EmployeeNJ4SupervisorNJ4,
                         _incidentNJ4EmployeeNJ4SupervisorNJ7,
                         _incidentNJ4EmployeeNJ7SupervisorNJ7;

        private Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<IDateTimeProvider>().Use((_dateTimeProvider = new Mock<IDateTimeProvider>()).Object);
        }

        [TestInitialize]
        public void TestInitialize()
        {

            _application = GetEntityFactory<Application>().Create(new {Id = RoleApplications.Operations});
            _module = GetEntityFactory<Module>().Create(new {Id = RoleModules.OperationsIncidents});
            _readAction = GetEntityFactory<RoleAction>().Create(new {Id = RoleActions.Read});
            _userAdminAction = GetEntityFactory<RoleAction>().Create(new {Id = RoleActions.UserAdministrator});

            _nj7 = GetEntityFactory<OperatingCenter>()
               .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            _nj4 = GetEntityFactory<OperatingCenter>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});

            _supervisorNJ7SupervisorSupervisor = GetEntityFactory<Employee>().Create(new {OperatingCenter = _nj7});
            _supervisorNJ7Supervisor = GetEntityFactory<Employee>()
               .Create(new {OperatingCenter = _nj7, ReportsTo = _supervisorNJ7SupervisorSupervisor});
            _supervisorNJ7 = GetEntityFactory<Employee>()
               .Create(new {OperatingCenter = _nj7, ReportsTo = _supervisorNJ7Supervisor});
            _supervisorNJ4 = GetEntityFactory<Employee>().Create(new {OperatingCenter = _nj4});
            _employeeNJ7SupervisorNJ7 = GetEntityFactory<Employee>()
               .Create(new {OperatingCenter = _nj7, ReportsTo = _supervisorNJ7});
            _employeeNJ7SupervisorNJ4 = GetEntityFactory<Employee>()
               .Create(new {OperatingCenter = _nj7, ReportsTo = _supervisorNJ4});
            _employeeNJ4SupervisorNJ7 = GetEntityFactory<Employee>()
               .Create(new {OperatingCenter = _nj4, ReportsTo = _supervisorNJ7});
            _employeeNJ4SupervisorNJ4 = GetEntityFactory<Employee>()
               .Create(new {OperatingCenter = _nj4, ReportsTo = _supervisorNJ4});

            _incidentNJ7EmployeeNJ7SupervisorNJ7 = GetEntityFactory<Incident>()
               .Create(new {OperatingCenter = _nj7, Employee = _employeeNJ7SupervisorNJ7});
            _incidentNJ7EmployeeNJ7SupervisorNJ4 = GetEntityFactory<Incident>()
               .Create(new {OperatingCenter = _nj7, Employee = _employeeNJ7SupervisorNJ4});
            _incidentNJ7EmployeeNJ4SupervisorNJ4 = GetEntityFactory<Incident>()
               .Create(new {OperatingCenter = _nj7, Employee = _employeeNJ4SupervisorNJ4});
            _incidentNJ4EmployeeNJ4SupervisorNJ4 = GetEntityFactory<Incident>()
               .Create(new {OperatingCenter = _nj4, Employee = _employeeNJ4SupervisorNJ4});
            _incidentNJ4EmployeeNJ4SupervisorNJ7 = GetEntityFactory<Incident>()
               .Create(new {OperatingCenter = _nj4, Employee = _employeeNJ4SupervisorNJ7});
            _incidentNJ4EmployeeNJ7SupervisorNJ7 = GetEntityFactory<Incident>()
               .Create(new {OperatingCenter = _nj4, Employee = _employeeNJ7SupervisorNJ7});
        }

        #endregion

        #region Linq/Criteria

        [TestMethod]
        public void TestSuperAdminsCanSeeEverything()
        {
            var user = GetFactory<AdminUserFactory>().Create();

            var repository = _container.With(new MockAuthenticationService(user).Object)
                                       .GetInstance<TestIncidentRepository>();

            // LINQ
            var result = repository.GetAll().ToList();

            MyAssert.Contains(result, _incidentNJ7EmployeeNJ7SupervisorNJ7);
            MyAssert.Contains(result, _incidentNJ7EmployeeNJ7SupervisorNJ4);
            MyAssert.Contains(result, _incidentNJ7EmployeeNJ4SupervisorNJ4);
            MyAssert.Contains(result, _incidentNJ4EmployeeNJ4SupervisorNJ4);
            MyAssert.Contains(result, _incidentNJ4EmployeeNJ4SupervisorNJ7);
            MyAssert.Contains(result, _incidentNJ4EmployeeNJ7SupervisorNJ7);

            // Criteria
            result = repository.GimmeCriteria().List<Incident>().ToList();

            MyAssert.Contains(result, _incidentNJ7EmployeeNJ7SupervisorNJ7);
            MyAssert.Contains(result, _incidentNJ7EmployeeNJ7SupervisorNJ4);
            MyAssert.Contains(result, _incidentNJ7EmployeeNJ4SupervisorNJ4);
            MyAssert.Contains(result, _incidentNJ4EmployeeNJ4SupervisorNJ4);
            MyAssert.Contains(result, _incidentNJ4EmployeeNJ4SupervisorNJ7);
            MyAssert.Contains(result, _incidentNJ4EmployeeNJ7SupervisorNJ7);
        }

        [TestMethod]
        public void TestUserAdminCanSeeEverythingInTheirOperatingCenterAsWellAsDirectReports()
        {
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false, Employee = _supervisorNJ7});
            var role = GetFactory<RoleFactory>()
               .Create(new {
                    Application = _application,
                    Module = _module,
                    Action = _userAdminAction,
                    User = user,
                    OperatingCenter = _nj7
                });

            Session.Save(user);

            var repository = _container.With(new MockAuthenticationService(user).Object)
                                       .GetInstance<TestIncidentRepository>();

            // LINQ
            var result = repository.GetAll().ToList();

            MyAssert.Contains(result, _incidentNJ7EmployeeNJ7SupervisorNJ7);
            MyAssert.Contains(result, _incidentNJ7EmployeeNJ7SupervisorNJ4);
            MyAssert.Contains(result, _incidentNJ7EmployeeNJ4SupervisorNJ4);
            MyAssert.DoesNotContain(result, _incidentNJ4EmployeeNJ4SupervisorNJ4);
            MyAssert.Contains(result, _incidentNJ4EmployeeNJ4SupervisorNJ7);
            MyAssert.Contains(result, _incidentNJ4EmployeeNJ7SupervisorNJ7);

            // Criteria
            result = repository.GimmeCriteria().List<Incident>().ToList();

            MyAssert.Contains(result, _incidentNJ7EmployeeNJ7SupervisorNJ7);
            MyAssert.Contains(result, _incidentNJ7EmployeeNJ7SupervisorNJ4);
            MyAssert.Contains(result, _incidentNJ7EmployeeNJ4SupervisorNJ4);
            MyAssert.DoesNotContain(result, _incidentNJ4EmployeeNJ4SupervisorNJ4);
            MyAssert.Contains(result, _incidentNJ4EmployeeNJ4SupervisorNJ7);
            MyAssert.Contains(result, _incidentNJ4EmployeeNJ7SupervisorNJ7);
        }

        [TestMethod]
        public void TestSupervisorsSeeOnlyTheirDirectReports()
        {
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false, Employee = _supervisorNJ7});

            var repository = _container.With(new MockAuthenticationService(user).Object)
                                       .GetInstance<TestIncidentRepository>();

            // LINQ
            var result = repository.GetAll().ToList();

            MyAssert.Contains(result, _incidentNJ7EmployeeNJ7SupervisorNJ7);
            MyAssert.DoesNotContain(result, _incidentNJ7EmployeeNJ7SupervisorNJ4);
            MyAssert.DoesNotContain(result, _incidentNJ7EmployeeNJ4SupervisorNJ4);
            MyAssert.DoesNotContain(result, _incidentNJ4EmployeeNJ4SupervisorNJ4);
            MyAssert.Contains(result, _incidentNJ4EmployeeNJ4SupervisorNJ7);
            MyAssert.Contains(result, _incidentNJ4EmployeeNJ7SupervisorNJ7);

            // Criteria
            result = repository.GimmeCriteria().List<Incident>().ToList();

            MyAssert.Contains(result, _incidentNJ7EmployeeNJ7SupervisorNJ7);
            MyAssert.DoesNotContain(result, _incidentNJ7EmployeeNJ7SupervisorNJ4);
            MyAssert.DoesNotContain(result, _incidentNJ7EmployeeNJ4SupervisorNJ4);
            MyAssert.DoesNotContain(result, _incidentNJ4EmployeeNJ4SupervisorNJ4);
            MyAssert.Contains(result, _incidentNJ4EmployeeNJ4SupervisorNJ7);
            MyAssert.Contains(result, _incidentNJ4EmployeeNJ7SupervisorNJ7);
        }

        [TestMethod]
        public void TestSupervisorsManagersSeeTheirReportsReports()
        {
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false, Employee = _supervisorNJ7Supervisor});

            var repository = _container.With(new MockAuthenticationService(user).Object)
                                       .GetInstance<TestIncidentRepository>();

            // LINQ
            var result = repository.GetAll().ToList();

            MyAssert.Contains(result, _incidentNJ7EmployeeNJ7SupervisorNJ7);
            MyAssert.DoesNotContain(result, _incidentNJ7EmployeeNJ7SupervisorNJ4);
            MyAssert.DoesNotContain(result, _incidentNJ7EmployeeNJ4SupervisorNJ4);
            MyAssert.DoesNotContain(result, _incidentNJ4EmployeeNJ4SupervisorNJ4);
            MyAssert.Contains(result, _incidentNJ4EmployeeNJ4SupervisorNJ7);
            MyAssert.Contains(result, _incidentNJ4EmployeeNJ7SupervisorNJ7);

            // Criteria
            result = repository.GimmeCriteria().List<Incident>().ToList();

            MyAssert.Contains(result, _incidentNJ7EmployeeNJ7SupervisorNJ7);
            MyAssert.DoesNotContain(result, _incidentNJ7EmployeeNJ7SupervisorNJ4);
            MyAssert.DoesNotContain(result, _incidentNJ7EmployeeNJ4SupervisorNJ4);
            MyAssert.DoesNotContain(result, _incidentNJ4EmployeeNJ4SupervisorNJ4);
            MyAssert.Contains(result, _incidentNJ4EmployeeNJ4SupervisorNJ7);
            MyAssert.Contains(result, _incidentNJ4EmployeeNJ7SupervisorNJ7);
        }

        [TestMethod]
        public void TestSupervisorsManagersSupervisorsSeeTheirReportsReportsReports()
        {
            var user = GetFactory<UserFactory>()
               .Create(new {IsAdmin = false, Employee = _supervisorNJ7SupervisorSupervisor});

            var repository = _container.With(new MockAuthenticationService(user).Object)
                                       .GetInstance<TestIncidentRepository>();

            // LINQ
            var result = repository.GetAll().ToList();

            MyAssert.Contains(result, _incidentNJ7EmployeeNJ7SupervisorNJ7);
            MyAssert.DoesNotContain(result, _incidentNJ7EmployeeNJ7SupervisorNJ4);
            MyAssert.DoesNotContain(result, _incidentNJ7EmployeeNJ4SupervisorNJ4);
            MyAssert.DoesNotContain(result, _incidentNJ4EmployeeNJ4SupervisorNJ4);
            MyAssert.Contains(result, _incidentNJ4EmployeeNJ4SupervisorNJ7);
            MyAssert.Contains(result, _incidentNJ4EmployeeNJ7SupervisorNJ7);

            // Criteria
            result = repository.GimmeCriteria().List<Incident>().ToList();

            MyAssert.Contains(result, _incidentNJ7EmployeeNJ7SupervisorNJ7);
            MyAssert.DoesNotContain(result, _incidentNJ7EmployeeNJ7SupervisorNJ4);
            MyAssert.DoesNotContain(result, _incidentNJ7EmployeeNJ4SupervisorNJ4);
            MyAssert.DoesNotContain(result, _incidentNJ4EmployeeNJ4SupervisorNJ4);
            MyAssert.Contains(result, _incidentNJ4EmployeeNJ4SupervisorNJ7);
            MyAssert.Contains(result, _incidentNJ4EmployeeNJ7SupervisorNJ7);
        }

        [TestMethod]
        public void TestReturnsNoResultsIfCurrentUserEmployeeIsNullAndUserIsNotAdmin()
        {
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false});

            var repository = _container.With(new MockAuthenticationService(user).Object)
                                       .GetInstance<TestIncidentRepository>();

            // LINQ
            var result = repository.GetAll().ToList();

            MyAssert.IsEmpty(result);

            result = repository.GimmeCriteria().List<Incident>().ToList();

            MyAssert.IsEmpty(result);
        }

        #endregion

        #region TestActionItemIsDeletedWithIncident

        [TestMethod]
        public void TestActionItemIsDeletedWithIncident()
        {
            this.TestActionItemIsDeletedWithThing("Incidents");
        }

        #endregion

        #region SearchOSHA

        [TestMethod]
        public void TestSearchOSHALessThan()
        {
            var feb1 = new DateTime(2017, 2, 1);
            var feb28 = new DateTime(2017, 2, 28);

            var incident = GetFactory<IncidentFactory>().Create(new {
                IncidentDate = new DateTime(2017, 2, 2)
            });

            var avail = new IncidentEmployeeAvailability {
                Incident = incident,
                EmployeeAvailabilityType = GetFactory<LostTimeIncidentEmployeeAvailabilityTypeFactory>().Create(),
                StartDate = feb1,
                EndDate = feb28
            };
            incident.IsOSHARecordable = true;
            incident.EmployeeAvailabilities.Add(avail);

            Session.Save(incident);
            Session.Flush();

            var searchModel = new TestOSHASearchModel();
            // Test that it includes incidents where IncidentDate is in the date range
            searchModel.IncidentDate = new DateRange {
                Operator = RangeOperator.LessThan,
                Start = null, //new DateTime(2017, 1, 7),
                End = new DateTime(2017, 2, 7)
            };

            var result = Repository.SearchOSHA(searchModel).Single();

            Assert.AreSame(incident, result);

            // Test that it includes incidents where IncidentDate is out of range but has EmployeeAvailability that is in range

            searchModel.IncidentDate.End = feb28;
            result = Repository.SearchOSHA(searchModel).Single();
            Assert.AreSame(incident, result);

            searchModel.IncidentDate.End = feb1.AddDays(2);
            result = Repository.SearchOSHA(searchModel).Single();
            Assert.AreSame(incident, result);

            incident.EmployeeAvailabilities.First().EndDate = null;
            result = Repository.SearchOSHA(searchModel).Single();
            Assert.AreSame(incident, result);

            // Test that no incidents are returned when everything is out of range.
            searchModel.IncidentDate.Start = null;
            searchModel.IncidentDate.End = new DateTime(2018, 1, 25);
            searchModel.OperatingCenter = new[] {0};
            result = Repository.SearchOSHA(searchModel).SingleOrDefault();
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestSearchOSHAGreaterEqual()
        {
            var feb1 = new DateTime(2017, 2, 1);
            var feb28 = new DateTime(2017, 2, 28);

            var incident = GetFactory<IncidentFactory>().Create(new {
                IncidentDate = new DateTime(2017, 1, 2)
            });

            var avail = new IncidentEmployeeAvailability {
                Incident = incident,
                EmployeeAvailabilityType = GetFactory<LostTimeIncidentEmployeeAvailabilityTypeFactory>().Create(),
                StartDate = feb1,
                EndDate = feb28
            };
            incident.IsOSHARecordable = true;
            incident.EmployeeAvailabilities.Add(avail);

            Session.Save(incident);
            Session.Flush();

            var searchModel = new TestOSHASearchModel();
            // Test that it includes incidents where IncidentDate is in the date range
            searchModel.IncidentDate = new DateRange {
                Operator = RangeOperator.GreaterThanOrEqualTo,
                Start = null, //new DateTime(2017, 1, 7),
                End = new DateTime(2016, 1, 7)
            };

            var result = Repository.SearchOSHA(searchModel).Single();

            Assert.AreSame(incident, result);

            // Test that it includes incidents where IncidentDate is out of range but has EmployeeAvailability that is in range

            searchModel.IncidentDate.End = feb28;
            result = Repository.SearchOSHA(searchModel).Single();
            Assert.AreSame(incident, result);

            searchModel.IncidentDate.End = feb28.AddDays(1);
            result = Repository.SearchOSHA(searchModel).Single();
            Assert.AreSame(incident, result);

            // Test that no incidents are returned when everything is out of range.
            searchModel.IncidentDate.Start = null;
            searchModel.IncidentDate.End = new DateTime(2020, 7, 25);
            searchModel.OperatingCenter = new[] {0};
            result = Repository.SearchOSHA(searchModel).SingleOrDefault();
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestSearchOSHA()
        {
            var feb1 = new DateTime(2017, 2, 1);
            var feb28 = new DateTime(2017, 2, 28);

            var badIncident = GetFactory<IncidentFactory>().Create(new {
                IncidentDate =
                    new DateTime(2014, 5,
                        1) // Need this to be a different date since IncidentDate defaults to DateTime.Today
            });
            var incident = GetFactory<IncidentFactory>().Create(new {
                IncidentDate = new DateTime(2017, 1, 2)
            });
            var incidentButNotOSHA = GetFactory<IncidentFactory>().Create(new {
                IncidentDate = new DateTime(2017, 1, 2)
            });
            var avail = new IncidentEmployeeAvailability {
                Incident = incident,
                EmployeeAvailabilityType = GetFactory<LostTimeIncidentEmployeeAvailabilityTypeFactory>().Create(),
                StartDate = feb1,
                EndDate = feb28
            };
            badIncident.IsOSHARecordable = true;
            incident.IsOSHARecordable = true;
            incidentButNotOSHA.IsOSHARecordable = false;

            incident.EmployeeAvailabilities.Add(avail);
            incidentButNotOSHA.EmployeeAvailabilities.Add(avail);

            Session.Save(incident);
            Session.Save(incidentButNotOSHA);
            Session.Flush();

            var searchModel = new TestOSHASearchModel();
            // Test that it includes incidents where IncidentDate is in the date range
            searchModel.IncidentDate = new DateRange {
                Start = new DateTime(2017, 1, 1),
                End = new DateTime(2017, 1, 7)
            };

            var result = Repository.SearchOSHA(searchModel).Single();
            Assert.AreSame(incident, result);

            // Test that it includes incidents where IncidentDate is out of range but has EmployeeAvailability that is in range

            searchModel.IncidentDate.Start = feb1;
            searchModel.IncidentDate.End = feb28;
            result = Repository.SearchOSHA(searchModel).Single();
            Assert.AreSame(incident, result);

            searchModel.IncidentDate.Start = feb1.AddDays(-1);
            searchModel.IncidentDate.End = feb28;
            result = Repository.SearchOSHA(searchModel).Single();
            Assert.AreSame(incident, result);

            searchModel.IncidentDate.Start = feb1;
            searchModel.IncidentDate.End = feb28.AddDays(1);
            result = Repository.SearchOSHA(searchModel).Single();
            Assert.AreSame(incident, result);

            searchModel.IncidentDate.Start = feb1.AddDays(1);
            searchModel.IncidentDate.End = feb28.AddDays(-1);
            result = Repository.SearchOSHA(searchModel).Single();
            Assert.AreSame(incident, result);

            // Test that no incidents are returned when everything is out of range.

            searchModel.IncidentDate.Start = new DateTime(2016, 12, 1);
            searchModel.IncidentDate.End = new DateTime(2016, 12, 12);
            result = Repository.SearchOSHA(searchModel).SingleOrDefault();
            Assert.IsNull(result);

            // Test OperatingCenter filtering too.
            searchModel.IncidentDate.Start = new DateTime(2017, 1, 1);
            searchModel.IncidentDate.End = new DateTime(2017, 1, 7);
            searchModel.OperatingCenter = new[] {incident.OperatingCenter.Id};
            result = Repository.SearchOSHA(searchModel).Single();
            Assert.AreSame(incident, result);

            searchModel.OperatingCenter = new[] {0};
            result = Repository.SearchOSHA(searchModel).SingleOrDefault();
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestSearchOSHAWhenEmployeeAvailabilityEndDateIsNotSet()
        {
            var feb1 = new DateTime(2017, 2, 1);
            var feb28 = new DateTime(2017, 2, 28);
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(new DateTime(2017, 2, 7));

            var badIncident = GetFactory<IncidentFactory>().Create(new {
                IncidentDate =
                    new DateTime(2014, 5,
                        1) // Need this to be a different date since IncidentDate defaults to DateTime.Today
            });
            var incident = GetFactory<IncidentFactory>().Create(new {NumberOfRestrictiveDutyDays = 400});
            incident.IncidentDate = new DateTime(2017, 1, 2);
            incident.IsOSHARecordable = true;
            badIncident.IsOSHARecordable = true;

            var avail = new IncidentEmployeeAvailability();
            avail.Incident = incident;
            avail.EmployeeAvailabilityType = GetFactory<LostTimeIncidentEmployeeAvailabilityTypeFactory>().Create();
            avail.StartDate = feb1;
            incident.EmployeeAvailabilities.Add(avail);
            Session.Save(incident);
            Session.Flush();

            var searchModel = new TestOSHASearchModel();
            searchModel.IncidentDate = new DateRange();

            // Test that it includes incidents where IncidentDate is in the date range
            searchModel.IncidentDate.Start = new DateTime(2017, 1, 1);
            searchModel.IncidentDate.End = new DateTime(2017, 1, 7);
            var result = Repository.SearchOSHA(searchModel).Single();
            Assert.AreSame(incident, result);

            // Test that it includes incidents where IncidentDate is out of range but has EmployeeAvailability that is in range

            searchModel.IncidentDate.Start = feb1;
            searchModel.IncidentDate.End = feb28;
            result = Repository.SearchOSHA(searchModel).Single();
            Assert.AreSame(incident, result);

            searchModel.IncidentDate.Start = feb1.AddDays(-1);
            searchModel.IncidentDate.End = feb28;
            result = Repository.SearchOSHA(searchModel).Single();
            Assert.AreSame(incident, result);

            searchModel.IncidentDate.Start = feb1;
            searchModel.IncidentDate.End = feb28.AddDays(1);
            result = Repository.SearchOSHA(searchModel).Single();
            Assert.AreSame(incident, result);

            searchModel.IncidentDate.Start = feb1.AddDays(1);
            searchModel.IncidentDate.End = feb28.AddDays(-1);
            result = Repository.SearchOSHA(searchModel).Single();
            Assert.AreSame(incident, result);

            // Test that no incidents are returned when everything is out of range.

            searchModel.IncidentDate.Start = new DateTime(2016, 12, 1);
            searchModel.IncidentDate.End = new DateTime(2016, 12, 12);
            result = Repository.SearchOSHA(searchModel).SingleOrDefault();
            Assert.IsNull(result);

            // Test OperatingCenter filtering too.
            searchModel.IncidentDate.Start = new DateTime(2017, 1, 1);
            searchModel.IncidentDate.End = new DateTime(2017, 1, 7);
            searchModel.OperatingCenter = new[] {incident.OperatingCenter.Id};
            result = Repository.SearchOSHA(searchModel).Single();
            Assert.AreSame(incident, result);

            searchModel.OperatingCenter = new[] {0};
            result = Repository.SearchOSHA(searchModel).SingleOrDefault();
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestSearchOSHAWhenNoRestOrLostTime()
        {
            var feb1 = new DateTime(2017, 2, 1);
            var feb28 = new DateTime(2017, 2, 28);

            var incident = GetFactory<IncidentFactory>().Create(new {
                IncidentDate = new DateTime(2017, 2, 2)
            });
            var incidentButNotOSHA = GetFactory<IncidentFactory>().Create(new {
                IncidentDate = new DateTime(2017, 2, 2)
            });

            incident.IsOSHARecordable = true;
            incidentButNotOSHA.IsOSHARecordable = false;

            Session.Save(incident);
            Session.Save(incidentButNotOSHA);
            Session.Flush();
            var searchModel = new TestOSHASearchModel();

            //false when operator is less than and incident date is greater than begin date
            searchModel.IncidentDate = new DateRange {
                Start = null,
                End = feb1
            };
            searchModel.IncidentDate.Operator = RangeOperator.LessThan;

            var result = Repository.SearchOSHA(searchModel).SingleOrDefault();
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestSearchOSHAReturnsOnlyRecordsWhereIsOSHARecordableIsTrue()
        {
            var incident = GetFactory<IncidentFactory>().Create(new {
                IncidentDate = new DateTime(2017, 1, 2)
            });

            var searchModel = new TestOSHASearchModel();
            searchModel.IncidentDate = new DateRange {
                Start = new DateTime(2017, 1, 1),
                End = new DateTime(2017, 1, 7)
            };
            incident.IsOSHARecordable = true;
            Session.Save(incident);
            Session.Flush();

            var result = Repository.SearchOSHA(searchModel).SingleOrDefault();
            Assert.AreSame(incident, result);

            incident.IsOSHARecordable = false;
            Session.Save(incident);
            Session.Flush();

            result = Repository.SearchOSHA(searchModel).SingleOrDefault();
            Assert.IsNull(result);
        }

        #endregion

        #region FindByOperatingCenterAndNumber

        [TestMethod]
        public void TestFindByOperatingCenterDoesExactlyThat()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var sm1 = GetEntityFactory<Incident>().Create();
            var sm2 = GetEntityFactory<Incident>().Create();
            var sm3 = GetEntityFactory<Incident>().Create(new { OperatingCenter = opc });
            Session.Save(sm1);
            Session.Flush();

            var result = Repository.GetByOperatingCenter(sm1.OperatingCenter.Id).ToArray();

            Assert.IsTrue(result.Contains(sm1));
            Assert.IsTrue(result.Contains(sm2));
            Assert.IsFalse(result.Contains(sm3));
        }

        #endregion

        #region FindByEmployeeId

        [TestMethod]
        public void TestFindByEmployeeDoesExactlyThat()
        {
            var emp = GetFactory<ActiveEmployeeFactory>().Create();
            var sm1 = GetEntityFactory<Incident>().Create(new { Employee = emp });
            var sm2 = GetEntityFactory<Incident>().Create(new { Employee = emp });
            var sm3 = GetEntityFactory<Incident>().Create();

            var result = Repository.GetByEmployeeId(emp.Id).ToArray();
            var first = result[0];
            var last = result[1];

            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(first.Id, sm2.Id);
            Assert.AreEqual(last.Id, sm1.Id);
            Assert.IsFalse(result.Contains(sm3));
        }

        #endregion

        #region Test class

        public class TestIncidentRepository : IncidentRepository
        {
            public ICriteria GimmeCriteria()
            {
                return Criteria;
            }

            public TestIncidentRepository(IRepository<AggregateRole> roleRepo, ISession session, IContainer container,
                IAuthenticationService<User> authenticationService, IDateTimeProvider dateTimeProvider) : base(roleRepo,
                session, container, authenticationService, dateTimeProvider) { }
        }

        public class TestOSHASearchModel : SearchSet<Incident>, ISearchIncidentOSHARecordableSummary
        {
            public int[] OperatingCenter { get; set; }

            public DateRange IncidentDate { get; set; }
        }

        #endregion
    }
}
