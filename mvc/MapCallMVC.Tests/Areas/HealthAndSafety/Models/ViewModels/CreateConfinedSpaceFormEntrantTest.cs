using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.HealthAndSafety.Models.ViewModels
{
    [TestClass]
    public class CreateConfinedSpaceFomEntranceTest : ViewModelTestBase<ConfinedSpaceFormEntrant, CreateConfinedSpaceFormEntrant>
    {
        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.EntrantType);
        }

        [TestMethod]
        public void TestMapToEntityOnlySetsEmployeeWhenIsEmployeeIsTrue()
        {
            var employee = GetEntityFactory<Employee>().Create();
            _viewModel.Employee = employee.Id;
            _viewModel.ContractingCompany = "Some company";
            _viewModel.ContractorName = "Some name";
            _viewModel.IsEmployee = true;

            _vmTester.MapToEntity();
            Assert.AreSame(employee, _entity.Employee);
            Assert.IsNull(_entity.ContractingCompany);
            Assert.IsNull(_entity.ContractorName);
        }
        
        [TestMethod]
        public void TestMapToEntityOnlySetsContractorNameAndContractingCompanyWhenIsEmployeeIsFalse()
        {
            var employee = GetEntityFactory<Employee>().Create();
            _viewModel.Employee = employee.Id;
            _viewModel.ContractingCompany = "Some company";
            _viewModel.ContractorName = "Some name";
            _viewModel.IsEmployee = false;

            _vmTester.MapToEntity();
            Assert.IsNull(_entity.Employee);
            Assert.AreEqual("Some company", _entity.ContractingCompany);
            Assert.AreEqual("Some name", _entity.ContractorName);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.EntrantType);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.ContractingCompany, "Some company", x => x.IsEmployee, false, true);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.ContractorName, "Some contractor name", x => x.IsEmployee, false, true);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.Employee, 1, x => x.IsEmployee, true, false);
        }
        
        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            _viewModel.IsEmployee = true; // This validation won't matter otherwise
            ValidationAssert.EntityMustExist(_viewModel, x => x.Employee, GetEntityFactory<Employee>().Create());
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.ContractingCompany, ConfinedSpaceFormEntrant.StringLengths.CONTRACTING_COMPANY);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.ContractorName, ConfinedSpaceFormEntrant.StringLengths.CONTRACTOR_NAME);
        }
    }
}
