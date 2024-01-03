using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Data;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace Contractors.Tests.Models.ViewModels
{
    [TestClass]
    public class ContractorMeterCrewViewModelTest : MapCallMvcInMemoryDatabaseTestBase<ContractorMeterCrew>
    {
        #region Fields

        private ViewModelTester<CreateContractorMeterCrew, ContractorMeterCrew> _vmTester;
        private CreateContractorMeterCrew _viewModel;
        private ContractorMeterCrew _entity;
        private Mock<IAuthenticationService<ContractorUser>> _authServ;
        private ContractorUser _user;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private IViewModelFactory _viewModelFactory;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<ContractorUser>>().Mock();
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
        }

        [TestInitialize]
        public void TestInitialize() 
        {
            _viewModelFactory = _container.GetInstance<ViewModelFactory>();

            _user = GetFactory<ContractorUserFactory>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _entity = GetEntityFactory<ContractorMeterCrew>().Create();
            _viewModel = _viewModelFactory.Build<CreateContractorMeterCrew, ContractorMeterCrew>(_entity);
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
