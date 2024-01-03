using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.Repositories.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models
{
    [TestClass]
    public class ContractorMeterCrewViewModelTest : MapCallMvcInMemoryDatabaseTestBase<ContractorMeterCrew>
    {
        #region Fields

        private ViewModelTester<CreateContractorMeterCrew, ContractorMeterCrew> _vmTester;
        private CreateContractorMeterCrew _viewModel;
        private ContractorMeterCrew _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
            e.For<IUserRepository>().Use<UserRepository>();
            e.For<IContractorRepository>().Use<ContractorRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create(new
            {
                IsAdmin = true
            });
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _entity = GetEntityFactory<ContractorMeterCrew>().Create();
            _viewModel = _viewModelFactory.Build<CreateContractorMeterCrew, ContractorMeterCrew>( _entity);
            _vmTester = new ViewModelTester<CreateContractorMeterCrew, ContractorMeterCrew>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Description);
            _vmTester.CanMapBothWays(x => x.AMLargeMeters);
            _vmTester.CanMapBothWays(x => x.AMMeters);
            _vmTester.CanMapBothWays(x => x.PMLargeMeters);
            _vmTester.CanMapBothWays(x => x.PMMeters);
        }


        [TestMethod]
        public void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AMLargeMeters);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AMMeters);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Contractor);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Description);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PMLargeMeters);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PMMeters);
        }

        [TestMethod]
        public void TestPropertiesHaveStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Description, ContractorMeterCrew.StringLengths.DESCRIPTION);
        }


        #endregion
    }
}
