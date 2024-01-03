using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Contractors.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Utilities;
using Moq;

namespace MapCallMVC.Tests.Areas.Contractors.Models
{
    public abstract class ContractorAgreementViewModelTest<TViewModel> : ViewModelTestBase<ContractorAgreement, TViewModel>
        where TViewModel : ContractorAgreementViewModel
    {
        #region Fields

        private Mock<IAuthenticationService<User>> _authServ;
        public Mock<IDateTimeProvider> _dateTimeProvider;
        public User _user;

        #endregion

        #region Private Methods

        [TestInitialize]
        public void ContractorAgreementViewModelTestInitialize()
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
            _vmTester.CanMapBothWays(x => x.Title);
            _vmTester.CanMapBothWays(x => x.Description);
            _vmTester.CanMapBothWays(x => x.Contractor);
            _vmTester.CanMapBothWays(x => x.ContractorCompany);
            _vmTester.CanMapBothWays(x => x.ContractorWorkCategoryType);
            _vmTester.CanMapBothWays(x => x.ContractorAgreementStatusType);
            _vmTester.CanMapBothWays(x => x.ContractorInsurance);
            _vmTester.CanMapBothWays(x => x.AgreementOwner);
            _vmTester.CanMapBothWays(x => x.AgreementStartDate);
            _vmTester.CanMapBothWays(x => x.AgreementEndDate);
            _vmTester.CanMapBothWays(x => x.EstimatedContractValue);
            _vmTester.CanMapBothWays(x => x.NJAWContractNumber);
            _vmTester.CanMapBothWays(x => x.Legacy);
            _vmTester.CanMapBothWays(x => x.AmendmentNumber);
            _vmTester.CanMapBothWays(x => x.StrategicSourcingProjectTrackingNumber);
            _vmTester.CanMapBothWays(x => x.Accounting);
            _vmTester.CanMapBothWays(x => x.EstimatedAnnualSpend);
            _vmTester.CanMapBothWays(x => x.EstimatedLifetimePayments);
            _vmTester.CanMapBothWays(x => x.EstimatedLifetimeReceipts);
            _vmTester.CanMapBothWays(x => x.EstimatedTerminationDate);
            _vmTester.CanMapBothWays(x => x.ContractOwnerEmployeeId);
            _vmTester.CanMapBothWays(x => x.ContractOwnerLastName);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.Title);
            ValidationAssert.PropertyIsRequired(x => x.Description);
            ValidationAssert.PropertyIsRequired(x => x.Contractor);
            ValidationAssert.PropertyIsRequired(x => x.ContractorCompany);
            ValidationAssert.PropertyIsRequired(x => x.ContractorWorkCategoryType);
            ValidationAssert.PropertyIsRequired(x => x.ContractorAgreementStatusType);
            ValidationAssert.PropertyIsRequired(x => x.AgreementOwner);
            ValidationAssert.PropertyIsRequired(x => x.AgreementStartDate);
            ValidationAssert.PropertyIsRequired(x => x.AgreementEndDate);
            ValidationAssert.PropertyIsRequired(x => x.EstimatedContractValue);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(x => x.Contractor, GetEntityFactory<Contractor>().Create());
            ValidationAssert.EntityMustExist(x => x.ContractorCompany, GetEntityFactory<ContractorCompany>().Create());
            ValidationAssert.EntityMustExist(x => x.ContractorWorkCategoryType, GetEntityFactory<ContractorWorkCategoryType>().Create());
            ValidationAssert.EntityMustExist(x => x.ContractorAgreementStatusType, GetEntityFactory<ContractorAgreementStatusType>().Create());
            ValidationAssert.EntityMustExist(x => x.ContractorInsurance, GetEntityFactory<ContractorInsurance>().Create());
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(x => x.Title, ContractorAgreement.StringLengths.TITLE);
            ValidationAssert.PropertyHasMaxStringLength(x => x.AgreementOwner, ContractorAgreement.StringLengths.AGREEMENT_OWNER);
            ValidationAssert.PropertyHasMaxStringLength(x => x.NJAWContractNumber, ContractorAgreement.StringLengths.NJAW_CONTRACT_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(x => x.StrategicSourcingProjectTrackingNumber, ContractorAgreement.StringLengths.STRATEGIC_SOURCING_PROJECT_TRACKING_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(x => x.Accounting, ContractorAgreement.StringLengths.ACCOUNTING);
            ValidationAssert.PropertyHasMaxStringLength(x => x.ContractOwnerEmployeeId, ContractorAgreement.StringLengths.CONTRACTOR_OWNER_EMPLOYEE_ID);
            ValidationAssert.PropertyHasMaxStringLength(x => x.ContractOwnerLastName, ContractorAgreement.StringLengths.CONTRACTOR_OWNER_LAST_NAME);
        }

        #endregion
    }

    [TestClass]
    public class CreateContractorAgreementTest : ContractorAgreementViewModelTest<CreateContractorAgreement> { }
}
