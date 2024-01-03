using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.Repositories.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models
{
    [TestClass]
    public class HydrantInspectionViewModelTest : MapCallMvcInMemoryDatabaseTestBase<HydrantInspection>
    {
        [TestMethod]
        public void TestMapSetsDisplayHydrantAndState()
        {
            var nj = GetEntityFactory<State>().Create(new {Abbreviation = "NJ", Name = "New Jersey"});
            var ny = GetEntityFactory<State>().Create(new {Abbreviation = "NY", Name = "New York"});
            var town = GetEntityFactory<Town>().Create(new {State = ny});
            var hydrant = GetEntityFactory<Hydrant>().Create(new {Town = town});
            var entity = GetEntityFactory<HydrantInspection>().Create(new {Hydrant = hydrant});
            _vmTester = new ViewModelTester<HydrantInspectionViewModel, HydrantInspection>(_viewModel, entity);

            _vmTester.MapToViewModel();

            Assert.AreEqual(ny.Id, _viewModel.State);
        }

        #region Fields

        private ViewModelTester<HydrantInspectionViewModel, HydrantInspection> _vmTester;
        private HydrantInspectionViewModel _viewModel;
        private HydrantInspection _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private HydrantInspectionType _flush;
        private HydrantInspectionType _flushInspect;
        private HydrantInspectionType _water;
        private HydrantInspectionType _inspect;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            e.For<IDateTimeProvider>().Use<TestDateTimeProvider>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<IUserRepository>().Use<UserRepository>();
            e.For<IHydrantRepository>().Use<HydrantRepository>();
            e.For<IValveRepository>().Use<ValveRepository>();
            e.For<IHydrantInspectionRepository>().Use<HydrantInspectionRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create(new {IsAdmin = true});
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _entity = GetEntityFactory<HydrantInspection>().Create();
            _viewModel = _viewModelFactory.Build<HydrantInspectionViewModel, HydrantInspection>(_entity);
            _vmTester = new ViewModelTester<HydrantInspectionViewModel, HydrantInspection>(_viewModel, _entity);

            _flush = GetEntityFactory<HydrantInspectionType>().Create(new {Description = "Flush"});
            _inspect = GetEntityFactory<HydrantInspectionType>().Create(new {Description = "Inspect"});
            _flushInspect = GetEntityFactory<HydrantInspectionType>().Create(new {Description = "INSPECT/FLUSH"});
            _water = GetEntityFactory<HydrantInspectionType>().Create(new {Description = "Water Quality"});
        }

        #endregion

        #region Mapping

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.ResidualChlorine);
            _vmTester.CanMapBothWays(x => x.DateInspected);
            _vmTester.CanMapBothWays(x => x.FullFlow);
            _vmTester.CanMapBothWays(x => x.GPM);
            _vmTester.CanMapBothWays(x => x.MinutesFlowed);
            _vmTester.CanMapBothWays(x => x.StaticPressure);
            _vmTester.CanMapBothWays(x => x.Remarks);
            _vmTester.CanMapBothWays(x => x.TotalChlorine);
        }

        [TestMethod]
        public void TestFreeNoReadReasonCanMapBothWays()
        {
            var reason = GetFactory<KitNotAvailableNoReadReasonFactory>().Create();
            _entity.FreeNoReadReason = reason;

            _vmTester.MapToViewModel();

            Assert.AreEqual(reason.Id, _viewModel.FreeNoReadReason);

            _entity.FreeNoReadReason = null;

            _vmTester.MapToEntity();

            Assert.AreSame(reason, _entity.FreeNoReadReason);
        }

        [TestMethod]
        public void TestTotalNoReadReasonCanMapBothWays()
        {
            var reason = GetFactory<KitNotAvailableNoReadReasonFactory>().Create();
            _entity.TotalNoReadReason = reason;

            _vmTester.MapToViewModel();

            Assert.AreEqual(reason.Id, _viewModel.TotalNoReadReason);

            _entity.TotalNoReadReason = null;

            _vmTester.MapToEntity();

            Assert.AreSame(reason, _entity.TotalNoReadReason);
        }

        [TestMethod]
        public void TestHydrantTagStatusCanMapBothWays()
        {
            var hydrantTagStatus = GetEntityFactory<HydrantTagStatus>().Create(new {Description = "Foo"});
            _entity.HydrantTagStatus = hydrantTagStatus;

            _vmTester.MapToViewModel();

            Assert.AreEqual(hydrantTagStatus.Id, _viewModel.HydrantTagStatus);

            _entity.HydrantTagStatus = null;
            _vmTester.MapToEntity();

            Assert.AreSame(hydrantTagStatus, _entity.HydrantTagStatus);
        }

        [TestMethod]
        public void TestHydrantInspectionTypeCanMapBothWays()
        {
            var hydrantInspectionType = GetEntityFactory<HydrantInspectionType>().Create(new {Description = "Foo"});
            _entity.HydrantInspectionType = hydrantInspectionType;

            _vmTester.MapToViewModel();

            Assert.AreEqual(hydrantInspectionType.Id, _viewModel.HydrantInspectionType);

            _entity.HydrantInspectionType = null;
            _vmTester.MapToEntity();

            Assert.AreSame(hydrantInspectionType, _entity.HydrantInspectionType);
        }

        #endregion

        #region Validation

        [TestMethod]
        public void TestFreeNoReadReasonRequiredWhenNoValueForResidualChlorine()
        {
            _viewModel.ResidualChlorine = null;

            ValidationAssert.PropertyIsRequired(_viewModel, x => x.FreeNoReadReason);

            _viewModel.ResidualChlorine = 1;

            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.FreeNoReadReason);
        }

        [TestMethod]
        public void TestTotalNoReadReasonRequiredWhenNoValueForResidualChlorine()
        {
            _viewModel.TotalChlorine = null;

            ValidationAssert.PropertyIsRequired(_viewModel, x => x.TotalNoReadReason);

            _viewModel.TotalChlorine = 1;

            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.TotalNoReadReason);
        }

        [TestMethod]
        public void TestRequiredFieldsAreRequired()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.DateInspected);
        }

        [TestMethod]
        public void TestHydrantTagStatusIsRequiredForNJ()
        {
            var hts = GetEntityFactory<HydrantTagStatus>().Create();

            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.HydrantTagStatus, hts.Id, x => x.State,
                State.Indices.NJ, State.Indices.NY);
        }

        [TestMethod]
        public void TestGPMIsRequiredForCertainInspectionTypes()
        {
            var somethingElse = GetEntityFactory<HydrantInspectionType>().Create(new {
                Description = "Something else"
            });

            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.GPM, 1.1m, x => x.HydrantInspectionType,
                _flush.Id, somethingElse.Id, "GPM is required for the selected inspection type.");

            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.GPM, 1.1m, x => x.HydrantInspectionType,
                _flushInspect.Id, somethingElse.Id, "GPM is required for the selected inspection type.");

            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.GPM, 1.1m, x => x.HydrantInspectionType,
                _water.Id, somethingElse.Id, "GPM is required for the selected inspection type.");
        }

        [TestMethod]
        public void TestMinutesFlowIsRequiredForCertainInspectionTypes()
        {
            var somethingElse = GetEntityFactory<HydrantInspectionType>().Create(new {
                Description = "Something else"
            });

            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.MinutesFlowed, 1.5m,
                x => x.HydrantInspectionType, _flush.Id, somethingElse.Id,
                "Minutes flowed is required for the selected inspection type.");

            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.MinutesFlowed, 1.5m,
                x => x.HydrantInspectionType, _flushInspect.Id, somethingElse.Id,
                "Minutes flowed is required for the selected inspection type.");

            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.MinutesFlowed, 1.5m,
                x => x.HydrantInspectionType, _water.Id, somethingElse.Id,
                "Minutes flowed is required for the selected inspection type.");
        }

        [TestMethod]
        public void TestMinutesFlowedMustBeGreaterThanZero()
        {
            _viewModel.MinutesFlowed = 0;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.MinutesFlowed,
                "Minutes flowed must be greater than zero.");

            _viewModel.MinutesFlowed = 0.0001m;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.MinutesFlowed);
        }

        [TestMethod]
        public void TestGPMMustBeGreaterThanZero()
        {
            _viewModel.GPM = 0;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.GPM, "GPM must be greater than zero.");

            _viewModel.GPM = 0.0001m;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.GPM);
        }

        [TestMethod]
        public void TestStaticPressureMustBeGreaterThanZeroAndLessThanOrEqualToThreeHundred()
        {
            _viewModel.StaticPressure = 0;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.StaticPressure,
                "Static pressure must be greater than 0 and less than or equal to 300.");

            _viewModel.StaticPressure = 0.0001m;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.StaticPressure);

            _viewModel.StaticPressure = 300.0001m;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.StaticPressure,
                "Static pressure must be greater than 0 and less than or equal to 300.");

            _viewModel.StaticPressure = 300;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.StaticPressure);
        }

        [TestMethod]
        public void TestResidualChlorineMustBeGreaterThanOrEqualToZeroAndLessThanOrEqualToNinePointNineNine()
        {
            _viewModel.ResidualChlorine = -1;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.ResidualChlorine,
                "Residual chlorine must be between 0 and 9.99");

            _viewModel.ResidualChlorine = 0;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.ResidualChlorine);

            _viewModel.ResidualChlorine = 10.1m;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.ResidualChlorine,
                "Residual chlorine must be between 0 and 9.99");

            _viewModel.ResidualChlorine = 9.99m;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.ResidualChlorine);
        }

        [TestMethod]
        public void TestPreResidualChlorineMustBeGreaterThanOrEqualToZeroAndLessThanOrEqualToNinePointNineNine()
        {
            _viewModel.PreResidualChlorine = -1;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.PreResidualChlorine,
                "Residual chlorine must be between 0 and 9.99");

            _viewModel.PreResidualChlorine = 0;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.PreResidualChlorine);

            _viewModel.PreResidualChlorine = 10.1m;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.PreResidualChlorine,
                "Residual chlorine must be between 0 and 9.99");

            _viewModel.PreResidualChlorine = 9.99m;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.PreResidualChlorine);
        }

        [TestMethod]
        public void TestTotalChlorineMustBeGreaterThanOrEqualToZeroAndLessThanOrEqualToFour()
        {
            _viewModel.TotalChlorine = -1;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.TotalChlorine,
                "Total chlorine must be between 0 and 4.");

            _viewModel.TotalChlorine = 0;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.TotalChlorine);

            _viewModel.TotalChlorine = 4.1m;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.TotalChlorine,
                "Total chlorine must be between 0 and 4.");

            _viewModel.TotalChlorine = 4;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.TotalChlorine);
        }

        [TestMethod]
        public void TestPreTotalChlorineMustBeGreaterThanOrEqualToZeroAndLessThanOrEqualToFour()
        {
            _viewModel.PreTotalChlorine = -1;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.PreTotalChlorine,
                "Total chlorine must be between 0 and 4.");

            _viewModel.PreTotalChlorine = 0;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.PreTotalChlorine);

            _viewModel.PreTotalChlorine = 4.1m;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.PreTotalChlorine,
                "Total chlorine must be between 0 and 4.");

            _viewModel.PreTotalChlorine = 4;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.PreTotalChlorine);
        }

        #endregion
    }
}
