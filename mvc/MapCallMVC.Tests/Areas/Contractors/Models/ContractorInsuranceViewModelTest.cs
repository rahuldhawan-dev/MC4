using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Contractors.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Utilities;
using Moq;
using System;

namespace MapCallMVC.Tests.Areas.Contractors.Models
{
    public abstract class ContractorInsuranceViewModelTest<TViewModel> : ViewModelTestBase<ContractorInsurance, TViewModel>
        where TViewModel : ContractorInsuranceViewModel
    {
        #region Fields

        private Mock<IAuthenticationService<User>> _authServ;
        public Mock<IDateTimeProvider> _dateTimeProvider;
        public User _user;

        #endregion

        #region Private Methods

        [TestInitialize]
        public void ContractorInsuranceViewModelTestInitialize()
        {
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(DateTime.Now);
            _authServ = new Mock<IAuthenticationService<User>>();
            _user = GetFactory<AdminUserFactory>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _container.Inject(_authServ.Object);
            _container.Inject(_dateTimeProvider.Object);
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Contractor);
            _vmTester.CanMapBothWays(x => x.InsuranceProvider);
            _vmTester.CanMapBothWays(x => x.PolicyNumber);
            _vmTester.CanMapBothWays(x => x.MeetsCurrentContractualLimits);
            _vmTester.CanMapBothWays(x => x.EffectiveDate);
            _vmTester.CanMapBothWays(x => x.TerminationDate);
            _vmTester.CanMapBothWays(x => x.ContractorInsuranceMinimumRequirement);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.Contractor);
            ValidationAssert.PropertyIsRequired(x => x.ContractorInsuranceMinimumRequirement);
            ValidationAssert.PropertyIsRequired(x => x.EffectiveDate);
            ValidationAssert.PropertyIsRequired(x => x.TerminationDate);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(x => x.Contractor, GetEntityFactory<Contractor>().Create());
            ValidationAssert.EntityMustExist(x => x.ContractorInsuranceMinimumRequirement, GetEntityFactory<ContractorInsuranceMinimumRequirement>().Create());
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(x => x.InsuranceProvider, ContractorInsurance.StringLengths.INSURANCE_PROVIDER);
            ValidationAssert.PropertyHasMaxStringLength(x => x.PolicyNumber, ContractorInsurance.StringLengths.POLICY_NUMBER);
        }

        #endregion
    }

    [TestClass]
    public class CreateContractorInsuranceTest : ContractorInsuranceViewModelTest<CreateContractorInsurance> { }
}
