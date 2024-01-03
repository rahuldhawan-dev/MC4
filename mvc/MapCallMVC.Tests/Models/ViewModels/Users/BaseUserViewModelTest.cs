using MapCall.Common.Testing;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Models.ViewModels.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Entities;

namespace MapCallMVC.Tests.Models.ViewModels.Users
{
    [TestClass]
    public class BaseUserViewModelTest<TViewModel> : ViewModelTestBase<User, TViewModel>
        where TViewModel : BaseUserViewModel
    {
        #region Tests

        #region Mapping
        
        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Address);
            _vmTester.CanMapBothWays(x => x.CellPhoneNumber);
            _vmTester.CanMapBothWays(x => x.City);
            _vmTester.CanMapBothWays(x => x.DefaultOperatingCenter, GetEntityFactory<OperatingCenter>().Create());
            _vmTester.CanMapBothWays(x => x.Email);
            _vmTester.CanMapBothWays(x => x.FaxNumber);
            _vmTester.CanMapBothWays(x => x.FullName);
            // Needs to be set to false, its initial value is being set to true by the entity factory.
            _viewModel.HasAccess = false; 
            _vmTester.CanMapBothWays(x => x.HasAccess);
            _vmTester.CanMapBothWays(x => x.IsUserAdmin);
            _vmTester.CanMapBothWays(x => x.PhoneNumber);
            _vmTester.CanMapBothWays(x => x.State);
            _vmTester.CanMapBothWays(x => x.UserType, GetEntityFactory<UserType>().Create());
            _vmTester.CanMapBothWays(x => x.ZipCode);
        }

        [TestMethod]
        public void TestMapToEntitySetsLastNameByParsingLastNameOutOfFullName()
        {
            _viewModel.FullName = "Abc Def Xyz";
            _vmTester.MapToEntity();
            Assert.AreEqual("Xyz", _entity.LastName);
        }

        [TestMethod]
        public void TestMapToEntitySetsLastNameToNullIfFullNameIsNullOrEmpty()
        {
            _viewModel.FullName = null;
            _entity.LastName = "Last name";
            _vmTester.MapToEntity();
            Assert.IsNull(_entity.LastName);
        }

        [TestMethod]
        public void TestMapToEntitySetstEmployeeFromEmployeeNumber()
        {
            var employee = GetEntityFactory<Employee>().Create(new { EmployeeId = "44444444" });
            _viewModel.EmployeeNumber = "44444444";
            _vmTester.MapToEntity();
            Assert.AreSame(employee, _entity.Employee);
        }

        [TestMethod]
        public void TestMapToEntitySetsEmployeeToNullIfEmployeeNumberIsNull()
        {
            var employee = GetEntityFactory<Employee>().Create(new { EmployeeId = "44444444" });
            _entity.Employee = employee;
            _viewModel.EmployeeNumber = null;
            _vmTester.MapToEntity();
            Assert.IsNull(_entity.Employee);
        }

        #endregion

        #region Validation

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.DefaultOperatingCenter);
            ValidationAssert.PropertyIsRequired(x => x.Email);
            ValidationAssert.PropertyIsRequired(x => x.FullName);
            ValidationAssert.PropertyIsRequired(x => x.HasAccess);
            ValidationAssert.PropertyIsRequired(x => x.UserType);
        }
        
        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(x => x.DefaultOperatingCenter, GetEntityFactory<OperatingCenter>().Create());
            ValidationAssert.EntityMustExist(x => x.UserType, GetEntityFactory<UserType>().Create());
        }
        
        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(x => x.Address, User.StringLengths.ADDRESS);
            ValidationAssert.PropertyHasMaxStringLength(x => x.CellPhoneNumber, User.StringLengths.ALL_PHONE_NUMBERS);
            ValidationAssert.PropertyHasMaxStringLength(x => x.City, User.StringLengths.CITY);

            // testing email max length is annoying
            
            _viewModel.Email = "some@email.com".PadLeft(User.StringLengths.EMAIL, 'x');
            ValidationAssert.PropertyHasMaxStringLength(x => x.Email, User.StringLengths.EMAIL, useCurrentPropertyValue: true);
            ValidationAssert.PropertyHasMaxStringLength(x => x.EmployeeNumber, User.StringLengths.MAX_EMPLOYEE_ID);
            ValidationAssert.PropertyHasMaxStringLength(x => x.FaxNumber, User.StringLengths.ALL_PHONE_NUMBERS);
            ValidationAssert.PropertyHasMaxStringLength(x => x.FullName, User.StringLengths.MAX_FULL_NAME);
            ValidationAssert.PropertyHasMaxStringLength(x => x.PhoneNumber, User.StringLengths.ALL_PHONE_NUMBERS);
            ValidationAssert.PropertyHasMaxStringLength(x => x.ZipCode, User.StringLengths.ZIP_CODE);
        }

        [TestMethod]
        public void TestValidationPassesWhenEmployeeNumberIsNull()
        {
            _viewModel.EmployeeNumber = null;
            ValidationAssert.ModelStateIsValid(x => x.EmployeeNumber);
        }

        [TestMethod]
        public void TestValidationFailsIfEmployeeNumberDoesNotMatchAnExistingEmployeeRecord()
        {
            var employee = GetEntityFactory<Employee>().Create();

            _viewModel.EmployeeNumber = "1580305103";
            ValidationAssert.ModelStateHasError(x => x.EmployeeNumber, "Employee number must match an existing Employee record.");

            _viewModel.EmployeeNumber = employee.EmployeeId;
            ValidationAssert.ModelStateIsValid(x => x.EmployeeNumber);
        }

        [TestMethod]
        public void TestValidationFailsIfEmployeeIsLinkedToAnExistingUser()
        {
            var user = GetEntityFactory<User>().Create();
            var employee = GetEntityFactory<Employee>().Create(new { User = user });
            
            _viewModel.EmployeeNumber = employee.EmployeeId;
            ValidationAssert.ModelStateHasError(x => x.EmployeeNumber, $"The user '{user.UserName}' is already linked to this employee.");

            user.Employee = null;
            employee.User = null;
            Session.Save(employee);
            Session.Save(user);
            ValidationAssert.ModelStateIsValid(x => x.EmployeeNumber);
        }

        #endregion

        #endregion
    }
}
