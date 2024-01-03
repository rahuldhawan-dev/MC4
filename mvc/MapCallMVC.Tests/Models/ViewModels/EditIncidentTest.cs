using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class EditIncidentTest : MapCallMvcInMemoryDatabaseTestBase<Incident>
    {
        private ViewModelTester<EditIncident, Incident> _vmTester;
        private EditIncident _viewModel;
        private Incident _entity;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use((_dateTimeProvider = new Mock<IDateTimeProvider>()).Object);
            e.For<IEmployeeRepository>().Use<EmployeeRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<ITownRepository>().Use<TownRepository>();
            e.For<IVehicleRepository>().Use<VehicleRepository>();
            e.For<ISensorRepository>().Use<SensorRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create();
            _authServ = new Mock<IAuthenticationService<User>>();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _container.Inject(_authServ.Object);
            _entity = GetFactory<IncidentFactory>().Create();
            _viewModel = _viewModelFactory.Build<EditIncident, Incident>( _entity);
            _vmTester = new ViewModelTester<EditIncident, Incident>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        #region Mapping

        [TestMethod]
        public void TestSupervisorMapsToEntity()
        {
            var emp = GetEntityFactory<Employee>().Create();
            _entity.Supervisor = emp;
            _viewModel.Supervisor = null;

            _vmTester.MapToViewModel();
            Assert.AreEqual(emp.Id, _viewModel.Supervisor);

            _entity.Supervisor = null;
            _vmTester.MapToEntity();
            Assert.AreSame(emp, _entity.Supervisor);
        }
        
        [TestMethod]
        public void TestWorkersCompensationClaimStatusMapsToEntity()
        {
            var status = GetEntityFactory<WorkersCompensationClaimStatus>().Create();
            _entity.WorkersCompensationClaimStatus = status;
            _viewModel.WorkersCompensationClaimStatus = null;

            _vmTester.MapToViewModel();
            Assert.AreEqual(status.Id, _viewModel.WorkersCompensationClaimStatus);

            _entity.WorkersCompensationClaimStatus = null;
            _vmTester.MapToEntity();
            Assert.AreSame(status, _entity.WorkersCompensationClaimStatus);
        }


        [TestMethod]
        public void TestPersonnelAreaMapsToEntityButDoesntUpdate()
        {
            var personnel = GetEntityFactory<PersonnelArea>().Create();
            var personnelJr = GetEntityFactory<PersonnelArea>().Create(new
            {
                Description = "Words that describe"
            });
            var emp = GetFactory<EmployeeFactory>().Create(new
            {
                PersonnelArea = personnel
            });

            _entity.Employee = emp;
            _entity.PersonnelArea = emp.PersonnelArea;
            _vmTester.MapToViewModel();
            _entity.Employee.PersonnelArea = personnelJr;
            _vmTester.MapToEntity();
            Assert.AreSame(personnelJr, _entity.Employee.PersonnelArea);
            Assert.AreSame(personnel, _entity.PersonnelArea);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IncidentStatus);
        }

        [TestMethod]
        public void TestIncidentStatusCanMapBothWays()
        {
            var incidentStatus = GetEntityFactory<IncidentStatus>().Create(new {Description = "Foo"});
            _entity.IncidentStatus = incidentStatus;

            _vmTester.MapToViewModel();

            Assert.AreEqual(incidentStatus.Id, _viewModel.IncidentStatus);

            _entity.IncidentStatus = null;
            _vmTester.MapToEntity();

            Assert.AreSame(incidentStatus, _entity.IncidentStatus);
        }
        
        [TestMethod]
        public void TestTargetCompletionDateIs10DaysLater()
        {
            var expected = _entity.IncidentDate;
            var expectedTarget = expected.AddDays(10);
            var target = new EditIncident(_container);
           
            //Assert Target date is null and set during Mapping
            Assert.AreEqual(null, _entity.IncidentCommitteeReportTargetCompletionDate);

            target.SetDefaults();
            target.Map(_entity);
            target.MapToEntity(_entity);

            //Assert
            Assert.AreEqual(expected, _entity.IncidentReportedDate);
            Assert.AreEqual(expectedTarget, _entity.IncidentCommitteeReportTargetCompletionDate);
        }

        [TestMethod]
        public void TestMapToEntityDoesNotMapWhenUserDoesNotHaveReadAccess()
        {
            _user.Roles.Add(new Role() {
                Action = new RoleAction() { Id = (int)RoleActions.Read },
                Module = new Module() { Id = (int)RoleModules.OperationsIncidentsDrugTesting },
                OperatingCenter = _user.DefaultOperatingCenter
            });
            _viewModel.DrugAndAlcoholTestingDecision = IncidentDrugAndAlcoholTestingDecision.Indices.OTHER;
            _viewModel.DrugAndAlcoholTestingNotes = "testing drug section notes";
            _viewModel.DrugAndAlcoholTestingResult = 2;

            _vmTester.MapToEntity();

            Assert.AreNotEqual(IncidentDrugAndAlcoholTestingDecision.Indices.OTHER, _entity.DrugAndAlcoholTestingDecision.Id);
            Assert.AreNotEqual("testing drug section notes", _entity.DrugAndAlcoholTestingNotes);
            Assert.AreNotEqual(2, _entity.DrugAndAlcoholTestingResult.Id);
        }

        #endregion

        #region Validation

        [TestMethod]
        public void TestValidationFailsIfUserIsNotAdminAndEmployeeChanges()
        {
            // Check user is not admin and that this is valid since the property hasn't changed.
            _authServ.Setup(x => x.CurrentUserIsAdmin).Returns(false);
            _viewModel.EmployeeType = EmployeeType.Indices.EMPLOYEE;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.Employee);

            // Then check that this fails if the property does change.
            _viewModel.Employee = GetEntityFactory<Employee>().Create().Id;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.Employee, EditIncident.VALIDATION_NON_ADMIN_EMPLOYEE);

            // Then check that this passes if it changes and the user is an admin.
            _authServ.Setup(x => x.CurrentUserIsAdmin).Returns(true);
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.Employee);
        }

        [TestMethod]
        public void TestValidationFailsIfUserIsNotAdminAndFacilityChanges()
        {
            // Check user is not admin and that this is valid since the property hasn't changed.
            _authServ.Setup(x => x.CurrentUserIsAdmin).Returns(false);
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.Facility);

            // Then check that this fails if the property does change.
            _viewModel.Facility = GetFactory<FacilityFactory>().Create().Id;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.Facility, EditIncident.VALIDATION_NON_ADMIN_FACILITY);

            // Then check that this passes if it changes and the user is an admin.
            _authServ.Setup(x => x.CurrentUserIsAdmin).Returns(true);
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.Facility);
        }

        [TestMethod]
        public void TestValidationFailsIfUserIsNotAdminAndOperatingCenterChanges()
        {
            // Check user is not admin and that this is valid since the property hasn't changed.
            _authServ.Setup(x => x.CurrentUserIsAdmin).Returns(false);
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.OperatingCenter);

            // Then check that this fails if the property does change.
            _viewModel.OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create().Id;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.OperatingCenter, EditIncident.VALIDATION_NON_ADMIN_OPERATING_CENTER);

            // Then check that this passes if it changes and the user is an admin.
            _authServ.Setup(x => x.CurrentUserIsAdmin).Returns(true);
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.OperatingCenter);
        }

        [TestMethod]
        public void TestValidationFailsIfUserIsNotAdminAndSupervisorChanges()
        {
            // Check user is not admin and that this is valid since the property hasn't changed.
            _authServ.Setup(x => x.CurrentUserIsAdmin).Returns(false);
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.Supervisor);

            // Then check that this fails if the property does change.
            _viewModel.Supervisor = GetEntityFactory<Employee>().Create().Id;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.Supervisor, EditIncident.VALIDATION_NON_ADMIN_SUPERVISOR);

            // Then check that this passes if it changes and the user is an admin.
            _authServ.Setup(x => x.CurrentUserIsAdmin).Returns(true);
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.Supervisor);
        }


        [TestMethod]
        public void TestValidationFailsIfUserIsNotAdminAndSupervisorChangesAndCurrentSupervisorIsNull()
        {
            // Check user is not admin and that this is valid since the property hasn't changed.
            _authServ.Setup(x => x.CurrentUserIsAdmin).Returns(false);
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.Supervisor);

            // Then check that this fails if the property does change.
            _entity.Supervisor = null;
            _viewModel.Supervisor = GetEntityFactory<Employee>().Create().Id;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.Supervisor, EditIncident.VALIDATION_NON_ADMIN_SUPERVISOR);

            // Then check that this passes if it changes and the user is an admin.
            _authServ.Setup(x => x.CurrentUserIsAdmin).Returns(true);
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.Supervisor);
        }

        #endregion

        #region SetDefaults

        [TestMethod]
        public void TestSetDefaultsSetsDoesNotChangeFiveWhysCompletedValue()
        {
            _viewModel.FiveWhysCompleted = null;
            _viewModel.SetDefaults();
            Assert.IsNull(_viewModel.FiveWhysCompleted, "Value should not have been changed");
        }

        #endregion

        #endregion
    }
}
