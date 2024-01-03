using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Operations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Operations.Models.ViewModels
{
    public abstract class AbsenseNotificationViewModelTestBase<TViewModel> : MapCallMvcInMemoryDatabaseTestBase<AbsenceNotification>
        where TViewModel : ViewModel<AbsenceNotification>
    {
        #region Private Methods

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use((_dateTimeProvider = new Mock<IDateTimeProvider>()).Object);
            e.For<IAuthenticationService<User>>().Use((_authServ = new Mock<IAuthenticationService<User>>()).Object);
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<IEmployeeRepository>().Use<EmployeeRepository>();
            e.For<IFamilyMedicalLeaveActCaseRepository>().Use<FamilyMedicalLeaveActCaseRepository>();
        }

        #endregion

        #region Exposed Methods

        [TestInitialize]
        public virtual void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create(new {IsAdmin = true});
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _entity = new AbsenceNotification();
            _viewModel = _container.GetInstance<TViewModel>();
            _vmTester = new ViewModelTester<TViewModel, AbsenceNotification>(_viewModel, _entity);
        }

        #endregion

        #region Fields

        protected ViewModelTester<TViewModel, AbsenceNotification> _vmTester;
        protected TViewModel _viewModel;
        protected AbsenceNotification _entity;
        protected Mock<IAuthenticationService<User>> _authServ;
        protected User _user;
        protected Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion
    }

    [TestClass]
    public class CreateAbsenceNotificationTest : AbsenseNotificationViewModelTestBase<CreateAbsenceNotification>
    {
        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.EndDate);
            _vmTester.CanMapBothWays(x => x.HumanResourcesNotes);
            _vmTester.CanMapBothWays(x => x.HumanResourcesReviewed);
            _vmTester.CanMapBothWays(x => x.LastDayOfWork);
            _vmTester.CanMapBothWays(x => x.PackageDateDue);
            _vmTester.CanMapBothWays(x => x.PackageDateSent);
            _vmTester.CanMapBothWays(x => x.StartDate);
            _vmTester.CanMapBothWays(x => x.SupervisorNotes);
            _vmTester.CanMapBothWays(x => x.TotalHoursOfAbsence);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Employee);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.EmployeeFMLANotification);
        }

        [TestMethod]
        public void TestFamilyMedicalLeaveActCaseCanMapBothWays()
        {
            var employee = GetEntityFactory<Employee>().Create();
            var fmla = GetEntityFactory<FamilyMedicalLeaveActCase>().Create(new {Employee = employee});
            _entity.FamilyMedicalLeaveActCase = fmla;

            _vmTester.MapToViewModel();

            Assert.AreEqual(fmla.Id, _viewModel.FamilyMedicalLeaveActCase);

            _entity.FamilyMedicalLeaveActCase = null;
            _vmTester.MapToEntity();

            Assert.AreSame(fmla, _entity.FamilyMedicalLeaveActCase);
        }

        [TestMethod]
        public void TestEmployeeAbsenceClaimCanMapBothWays()
        {
            var eac = GetEntityFactory<EmployeeAbsenceClaim>().Create(new { Description = "Foo" });
            _entity.EmployeeAbsenceClaim = eac;

            _vmTester.MapToViewModel();

            Assert.AreEqual(eac.Id, _viewModel.EmployeeAbsenceClaim);

            _entity.EmployeeAbsenceClaim = null;
            _vmTester.MapToEntity();

            Assert.AreSame(eac, _entity.EmployeeAbsenceClaim);
        }

        [TestMethod]
        public void TestMapToEntitySetsSubmittedByToCurrentUser()
        {
            _entity.SubmittedBy = null;
            _vmTester.MapToEntity();
            
            Assert.AreSame(_user, _entity.SubmittedBy);
        }

        [TestMethod]
        public void TestEmployeeFMLANotificationCanMapBothWays()
        {
            var fmlnotification = GetEntityFactory<EmployeeFMLANotification>().Create(new {Description = "Foo"});
            _entity.EmployeeFMLANotification = fmlnotification;

            _vmTester.MapToViewModel();

            Assert.AreEqual(fmlnotification.Id, _viewModel.EmployeeFMLANotification);

            _entity.EmployeeFMLANotification = null;
            _vmTester.MapToEntity();

            Assert.AreSame(fmlnotification, _entity.EmployeeFMLANotification);
        }

        [TestMethod]
        public void TestProgressiveDisciplineCanMapBothWays()
        {
            var progressiveDiscipline = GetEntityFactory<ProgressiveDiscipline>().Create(new {Description = "Foo"});
            _entity.ProgressiveDiscipline = progressiveDiscipline;

            _vmTester.MapToViewModel();

            Assert.AreEqual(progressiveDiscipline.Id, _viewModel.ProgressiveDiscipline);

            _entity.ProgressiveDiscipline = null;
            _vmTester.MapToEntity();

            Assert.AreSame(progressiveDiscipline, _entity.ProgressiveDiscipline);
        }

        [TestMethod]
        public void TestAbsenceStatusCanMapBothWays()
        {
            var absenceStatus = GetEntityFactory<AbsenceStatus>().Create(new {Description = "Foo"});
            _entity.AbsenceStatus = absenceStatus;

            _vmTester.MapToViewModel();

            Assert.AreEqual(absenceStatus.Id, _viewModel.AbsenceStatus);

            _entity.AbsenceStatus = null;
            _vmTester.MapToEntity();

            Assert.AreSame(absenceStatus, _entity.AbsenceStatus);
        }

        #endregion
    }

    [TestClass]
    public class EditAbsenceNotificationTest : AbsenseNotificationViewModelTestBase<EditAbsenceNotification>
    {
        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.EndDate);
            _vmTester.CanMapBothWays(x => x.HumanResourcesNotes);
            _vmTester.CanMapBothWays(x => x.HumanResourcesReviewed);
            _vmTester.CanMapBothWays(x => x.LastDayOfWork);
            _vmTester.CanMapBothWays(x => x.PackageDateDue);
            _vmTester.CanMapBothWays(x => x.PackageDateSent);
            _vmTester.CanMapBothWays(x => x.StartDate);
            _vmTester.CanMapBothWays(x => x.SupervisorNotes);
            _vmTester.CanMapBothWays(x => x.TotalHoursOfAbsence);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Employee);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.EmployeeFMLANotification);
        }

        [TestMethod]
        public void TestFamilyMedicalLeaveActCaseCanMapBothWays()
        {
            var employee = GetEntityFactory<Employee>().Create();
            var fmla = GetEntityFactory<FamilyMedicalLeaveActCase>().Create(new { Employee = employee });
            _entity.FamilyMedicalLeaveActCase = fmla;

            _vmTester.MapToViewModel();

            Assert.AreEqual(fmla.Id, _viewModel.FamilyMedicalLeaveActCase);

            _entity.FamilyMedicalLeaveActCase = null;
            _vmTester.MapToEntity();

            Assert.AreSame(fmla, _entity.FamilyMedicalLeaveActCase);
        }

        [TestMethod]
        public void TestEmployeeAbsenceClaimCanMapBothWays()
        {
            var eac = GetEntityFactory<EmployeeAbsenceClaim>().Create(new {Description = "Foo"});
            _entity.EmployeeAbsenceClaim = eac;

            _vmTester.MapToViewModel();

            Assert.AreEqual(eac.Id, _viewModel.EmployeeAbsenceClaim);

            _entity.EmployeeAbsenceClaim = null;
            _vmTester.MapToEntity();

            Assert.AreSame(eac, _entity.EmployeeAbsenceClaim);
        }

        [TestMethod]
        public void TestEmployeeFMLANotificationCanMapBothWays()
        {
            var fmlnotification = GetEntityFactory<EmployeeFMLANotification>().Create(new { Description = "Foo" });
            _entity.EmployeeFMLANotification = fmlnotification;

            _vmTester.MapToViewModel();

            Assert.AreEqual(fmlnotification.Id, _viewModel.EmployeeFMLANotification);

            _entity.EmployeeFMLANotification = null;
            _vmTester.MapToEntity();

            Assert.AreSame(fmlnotification, _entity.EmployeeFMLANotification);
        }

        [TestMethod]
        public void TestProgressiveDisciplineCanMapBothWays()
        {
            var progressiveDiscipline = GetEntityFactory<ProgressiveDiscipline>().Create(new {Description = "Foo"});
            _entity.ProgressiveDiscipline = progressiveDiscipline;

            _vmTester.MapToViewModel();

            Assert.AreEqual(progressiveDiscipline.Id, _viewModel.ProgressiveDiscipline);

            _entity.ProgressiveDiscipline = null;
            _vmTester.MapToEntity();

            Assert.AreSame(progressiveDiscipline, _entity.ProgressiveDiscipline);
        }

        [TestMethod]
        public void TestAbsenceStatusCanMapBothWays()
        {
            var absenceStatus = GetEntityFactory<AbsenceStatus>().Create(new {Description = "Foo"});
            _entity.AbsenceStatus = absenceStatus;

            _vmTester.MapToViewModel();

            Assert.AreEqual(absenceStatus.Id, _viewModel.AbsenceStatus);

            _entity.AbsenceStatus = null;
            _vmTester.MapToEntity();

            Assert.AreSame(absenceStatus, _entity.AbsenceStatus);
        }

        [TestMethod]
        public void TestMapSetsOperatingCenterFromEmployeeOperatingCenter()
        {
            var absenceNotification = GetEntityFactory<AbsenceNotification>().Create();

            var target = new EditAbsenceNotification(_container);
            target.Map(absenceNotification);

            Assert.AreEqual(absenceNotification.Employee.OperatingCenter.Id, target.OperatingCenter);
        }

        #endregion
    }
}
