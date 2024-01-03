using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Operations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Data;
using MMSINC.Testing;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Operations.Models.ViewModels
{
    public abstract class FamilyMedicalLeaveActCaseViewModelTestBase<TViewModel> : MapCallMvcInMemoryDatabaseTestBase<FamilyMedicalLeaveActCase>
        where TViewModel : ViewModel<FamilyMedicalLeaveActCase>
    {
        #region Fields

        protected ViewModelTester<TViewModel, FamilyMedicalLeaveActCase> _vmTester;
        protected TViewModel _viewModel;
        protected FamilyMedicalLeaveActCase _entity;
        protected Mock<IAuthenticationService<User>> _authServ;
        protected User _user;

        #endregion

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IAuthenticationService<User>>().Use((_authServ = new Mock<IAuthenticationService<User>>()).Object);
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<IEmployeeRepository>().Use<EmployeeRepository>();
            e.For<IDateTimeProvider>().Use(new Mock<IDateTimeProvider>().Object);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create(new { IsAdmin = true });
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _viewModel = _container.GetInstance<TViewModel>();
            _entity = new FamilyMedicalLeaveActCase();
            _vmTester = new ViewModelTester<TViewModel, FamilyMedicalLeaveActCase>(_viewModel, _entity);
        }
    }

    [TestClass]
    public class CreateFamilyMedicalLeaveActCaseTest : FamilyMedicalLeaveActCaseViewModelTestBase<CreateFamilyMedicalLeaveActCase>
    {
        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.StartDate);
            _vmTester.CanMapBothWays(x => x.EndDate);
            _vmTester.CanMapBothWays(x => x.FrequencyDays);
            _vmTester.CanMapBothWays(x => x.CertificationExtended);
            _vmTester.CanMapBothWays(x => x.SendPackage);
            _vmTester.CanMapBothWays(x => x.PackageDateSent);
            _vmTester.CanMapBothWays(x => x.PackageDateReceived);
            _vmTester.CanMapBothWays(x => x.PackageDateDue);
            _vmTester.CanMapBothWays(x => x.ChronicCondition);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
             ValidationAssert.PropertyIsRequired(_viewModel, x => x.Employee);
        }

        [TestMethod]
        public void TestEmployeeCanMapBothWays()
        {
            var emp = GetEntityFactory<Employee>().Create(new {Description = "Foo"});
            _entity.Employee = emp;

            _vmTester.MapToViewModel();

            Assert.AreEqual(emp.Id, _viewModel.Employee);

            _entity.Employee = null;
            _vmTester.MapToEntity();

            Assert.AreSame(emp, _entity.Employee);
        }

        [TestMethod]
        public void TestCompanyAbsenceCertificationCanMapBothWays()
        {
            var ent = GetEntityFactory<CompanyAbsenceCertification>().Create(new {Description = "Foo"});
            _entity.CompanyAbsenceCertification = ent;

            _vmTester.MapToViewModel();

            Assert.AreEqual(ent.Id, _viewModel.CompanyAbsenceCertification);

            _entity.CompanyAbsenceCertification = null;
            _vmTester.MapToEntity();

            Assert.AreSame(ent, _entity.CompanyAbsenceCertification);
        }

        #endregion
    }

    [TestClass]
    public class EditFamilyMedicalLeaveActCaseTest : FamilyMedicalLeaveActCaseViewModelTestBase<EditFamilyMedicalLeaveActCase>
    {
        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.StartDate);
            _vmTester.CanMapBothWays(x => x.EndDate);
            _vmTester.CanMapBothWays(x => x.FrequencyDays);
            _vmTester.CanMapBothWays(x => x.CertificationExtended);
            _vmTester.CanMapBothWays(x => x.SendPackage);
            _vmTester.CanMapBothWays(x => x.PackageDateSent);
            _vmTester.CanMapBothWays(x => x.PackageDateReceived);
            _vmTester.CanMapBothWays(x => x.PackageDateDue);
            _vmTester.CanMapBothWays(x => x.ChronicCondition);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Employee);
        }

        [TestMethod]
        public void TestEmployeeCanMapBothWays()
        {
            var emp = GetEntityFactory<Employee>().Create(new { Description = "Foo" });
            _entity.Employee = emp;

            _vmTester.MapToViewModel();

            Assert.AreEqual(emp.Id, _viewModel.Employee);

            _entity.Employee = null;
            _vmTester.MapToEntity();

            Assert.AreSame(emp, _entity.Employee);
        }

        [TestMethod]
        public void TestCompanyAbsenceCertificationCanMapBothWays()
        {
            var ent = GetEntityFactory<CompanyAbsenceCertification>().Create(new { Description = "Foo" });
            _entity.CompanyAbsenceCertification = ent;

            _vmTester.MapToViewModel();

            Assert.AreEqual(ent.Id, _viewModel.CompanyAbsenceCertification);

            _entity.CompanyAbsenceCertification = null;
            _vmTester.MapToEntity();

            Assert.AreSame(ent, _entity.CompanyAbsenceCertification);
        }

        #endregion
    }
}
