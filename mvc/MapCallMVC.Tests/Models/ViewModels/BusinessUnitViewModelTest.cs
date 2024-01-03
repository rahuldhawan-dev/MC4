using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    public abstract class BusinessUnitViewModelTest<TViewModel> : ViewModelTestBase<BusinessUnit, TViewModel> where TViewModel : BusinessUnitViewModel
    {
        #region Fields

        private Mock<IAuthenticationService<User>> _authServ;
        public User _user;
        public Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetEntityFactory<User>().Create();
            _user.Employee = GetEntityFactory<Employee>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
        }

        #endregion

        #region Exposed Methods

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Area, GetEntityFactory<BusinessUnitArea>().Create());
            _vmTester.CanMapBothWays(x => x.AuthorizedStaffingLevelBargainingUnit);
            _vmTester.CanMapBothWays(x => x.AuthorizedStaffingLevelManagement);
            _vmTester.CanMapBothWays(x => x.AuthorizedStaffingLevelNonBargainingUnit);
            _vmTester.CanMapBothWays(x => x.AuthorizedStaffingLevelTotal);
            _vmTester.CanMapBothWays(x => x.BU);
            _vmTester.CanMapBothWays(x => x.Department, GetEntityFactory<Department>().Create());
            _vmTester.CanMapBothWays(x => x.Description);
            _vmTester.CanMapBothWays(x => x.EmployeeResponsible, GetEntityFactory<Employee>().Create());
            _vmTester.CanMapBothWays(x => x.Is271Visible);
            _vmTester.CanMapBothWays(x => x.OperatingCenter, GetEntityFactory<OperatingCenter>().Create());
            _vmTester.CanMapBothWays(x => x.Order);
            _vmTester.CanMapBothWays(x => x.IsActive);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.BU);
            ValidationAssert.PropertyIsRequired(x => x.Department);
            ValidationAssert.PropertyIsRequired(x => x.Is271Visible);
            ValidationAssert.PropertyIsRequired(x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(x => x.Order);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(x => x.BU, BusinessUnit.StringLengths.BU);
            ValidationAssert.PropertyHasMaxStringLength(x => x.Description, BusinessUnit.StringLengths.DESCRIPTION);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(x => x.Area, GetEntityFactory<BusinessUnitArea>().Create());
            ValidationAssert.EntityMustExist(x => x.Department, GetEntityFactory<Department>().Create());
            ValidationAssert.EntityMustExist(x => x.EmployeeResponsible, GetEntityFactory<Employee>().Create());
            ValidationAssert.EntityMustExist(x => x.OperatingCenter, GetEntityFactory<OperatingCenter>().Create());
        }

        #endregion
    }
}
