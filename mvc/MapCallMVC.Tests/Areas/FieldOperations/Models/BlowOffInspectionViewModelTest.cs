using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.Repositories.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models
{
    public abstract class BlowOffInspectionViewModelTest<TViewModel>
        : ViewModelTestBase<BlowOffInspection, TViewModel>
        where TViewModel : BlowOffInspectionViewModel
    {
        #region Fields

        protected Mock<IAuthenticationService<User>> _authServ;
        protected User _user;
        protected Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
            e.For<IUserRepository>().Use<UserRepository>();
            e.For<IValveRepository>().Use<ValveRepository>();
            e.For<IHydrantRepository>().Use<HydrantRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<IHydrantInspectionRepository>().Use<HydrantInspectionRepository>();
            e.For<IBlowOffInspectionRepository>().Use<BlowOffInspectionRepository>();
        }

        [TestInitialize]
        public void BlowOffInspectionViewModelTestInitialize()
        {
            _user = GetFactory<UserFactory>().Create(new {IsAdmin = true});
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            GetEntityFactory<HydrantInspectionType>().Create(new {Description = "FLUSH"});
            GetEntityFactory<HydrantInspectionType>().Create(new {Description = "INSPECT/FLUSH"});
            GetEntityFactory<HydrantInspectionType>().Create(new {Description = "WATER QUALITY"});
        }

        protected override BlowOffInspection CreateEntity()
        {
            return GetEntityFactory<BlowOffInspection>().Create(new {
                Valve = typeof(ValveFactory)
            });
        }

        #endregion

        #region Validation

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Valve);
            _viewModel.ResidualChlorine = null;
            _viewModel.TotalChlorine = null;
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.FreeNoReadReason);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.TotalNoReadReason);
            _viewModel.ResidualChlorine = 1;
            _viewModel.TotalChlorine = 1;
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.TotalNoReadReason);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.FreeNoReadReason);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.Valve, GetFactory<ValveFactory>().Create());
        }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Valve,
                GetEntityFactory<Valve>().Create(new {OperatingCenter = GetEntityFactory<OperatingCenter>().Create()}));
            _vmTester.CanMapBothWays(x => x.FreeNoReadReason,
                GetFactory<KitNotAvailableNoReadReasonFactory>().Create());
            _vmTester.CanMapBothWays(x => x.TotalNoReadReason,
                GetFactory<KitNotAvailableNoReadReasonFactory>().Create());
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            // None have string validation
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
