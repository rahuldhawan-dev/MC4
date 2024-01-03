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
    public class CreateContractorMeterCrewTest : MapCallMvcInMemoryDatabaseTestBase<ContractorMeterCrew>
    {
        #region Fields

        private ViewModelTester<CreateContractorMeterCrew, ContractorMeterCrew> _vmTester;
        private CreateContractorMeterCrew _viewModel;
        private ContractorMeterCrew _entity;
        private Mock<IAuthenticationService<ContractorUser>> _authServ;
        private ContractorUser _currentUser;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private IViewModelFactory _viewModelFactory;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(
            ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<ContractorUser>>().Mock();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModelFactory = _container.GetInstance<ViewModelFactory>();

            _currentUser = GetFactory<ContractorUserFactory>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_currentUser);
           // _dateTimeProvider = new Mock<IDateTimeProvider>();

            _container.Inject(_authServ.Object);
           // _container.Inject(_dateTimeProvider.Object);

            _entity = GetEntityFactory<ContractorMeterCrew>().Create();
            _viewModel = _viewModelFactory.Build<CreateContractorMeterCrew, ContractorMeterCrew>(_entity);
            _vmTester = new ViewModelTester<CreateContractorMeterCrew, ContractorMeterCrew>(_viewModel, _entity);
        }

        #endregion

        #region Tests


        [TestMethod]
        public void TestMapToEntitySetsContractorToCurrentUserContractor()
        {
            _entity.Contractor = null;
            _vmTester.MapToEntity();
            Assert.AreSame(_currentUser.Contractor, _entity.Contractor);
        }


        #endregion
    }
}
