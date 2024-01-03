using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.Repositories.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models
{
    [TestClass]
    public class CopySewerOpeningTest : MapCallMvcInMemoryDatabaseTestBase<SewerOpening>
    {
        #region Private Members

        private ViewModelTester<CopySewerOpening, SewerOpening> _vmTester;
        private CopySewerOpening _viewModel;
        private SewerOpening _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private AssetStatus _activeStatus;
        private AssetStatus _retiredStatus;
        private AssetStatus _cancelledStatus;
        private AssetStatus _removedStatus;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            e.For<IUserRepository>().Use<UserRepository>();
            e.For<ISewerOpeningRepository>().Use<SewerOpeningRepository>();
            e.For<IAssetStatusRepository>().Use<AssetStatusRepository>();
            e.For<IRepository<AssetStatus>>().Use(ctx => ctx.GetInstance<IAssetStatusRepository>());
            e.For<IDateTimeProvider>().Mock();
            e.For<IAbbreviationTypeRepository>().Use<AbbreviationTypeRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _authServ.Setup(x => x.CurrentUserIsAdmin).Returns(true);
            _user.IsAdmin = true;

            _entity = GetEntityFactory<SewerOpening>().Create();
            _viewModel = _viewModelFactory.Build<CopySewerOpening, SewerOpening>(_entity);
            _vmTester = new ViewModelTester<CopySewerOpening, SewerOpening>(_viewModel, _entity);

            _activeStatus = GetFactory<ActiveAssetStatusFactory>().Create();
            _retiredStatus = GetFactory<RetiredAssetStatusFactory>().Create();
            _cancelledStatus = GetFactory<CancelledAssetStatusFactory>().Create();
            _removedStatus = GetFactory<RemovedAssetStatusFactory>().Create();
            GetFactory<TownSectionAbbreviationTypeFactory>().Create();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestStatusIsNotCopiedFromOriginalRecordSoThatOtherRequiredFieldsCanBeSetProperly()
        {
            var statuses = new[] {_activeStatus, _retiredStatus, _cancelledStatus, _removedStatus};
            foreach (var status in statuses)
            {
                var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
                var town = GetEntityFactory<Town>().Create();
                town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc1, Abbreviation = "ZZ" });
                _entity.Town = town;
                _entity.OperatingCenter = opc1;
                _entity.Status = status;
                _viewModel.Town = town.Id;
                _viewModel.OperatingCenter = opc1.Id;

                var entity = _vmTester.MapToEntity();

                Assert.AreEqual(entity.Status.Id, AssetStatus.Indices.PENDING);
            }
        }

        [TestMethod]
        public void TestMapToEntityCopiesInspectionFrequencyIfItIsSetButOtherwiseUsesTheOperatingCentersInspectionFrequency()
        {
            var freq = GetFactory<YearlyRecurringFrequencyUnitFactory>().Create();
            var monthFreq = GetFactory<MonthlyRecurringFrequencyUnitFactory>().Create();
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create(new {
                SewerOpeningInspectionFrequency = 3,
                SewerOpeningInspectionFrequencyUnit = freq
            });

            _viewModel.Town = null; // This is to bypass some error throwing stuff related to opening number generation. Unimportant to this test.
            _viewModel.OperatingCenter = opc.Id;

            // If the InspectionFrequency property has a value, then we're copying
            // those to the entity. 
            _viewModel.InspectionFrequency = 123;
            _viewModel.InspectionFrequencyUnit = monthFreq.Id;

            _vmTester.MapToEntity();
            Assert.AreEqual(123, _entity.InspectionFrequency);
            Assert.AreSame(monthFreq, _entity.InspectionFrequencyUnit);
            
            _viewModel.InspectionFrequency = null;
            _vmTester.MapToEntity();
            Assert.IsNull(_entity.InspectionFrequency);
            Assert.AreSame(monthFreq, _entity.InspectionFrequencyUnit);
        }

        #endregion
    }
}