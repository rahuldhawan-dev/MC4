using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.Repositories.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.NHibernate;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Models
{
    [TestClass]
    public class EditMainCrossingInspectionTest : MapCallMvcInMemoryDatabaseTestBase<MainCrossingInspection>
    {
        #region Fields

        private ViewModelTester<EditMainCrossingInspection, MainCrossingInspection> _vmTester;
        private EditMainCrossingInspection _viewModel;
        private MainCrossingInspection _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            e.For<IMainCrossingInspectionAssessmentRatingRepository>().Use<MainCrossingInspectionAssessmentRatingRepository>();
            e.For<IMainCrossingRepository>().Use<MainCrossingRepository>();
        }

        [TestInitialize]
        public void InitializeTest()
        {
            _user = GetFactory<UserFactory>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _entity = GetFactory<MainCrossingInspectionFactory>().Create();
            _viewModel = _viewModelFactory.Build<EditMainCrossingInspection, MainCrossingInspection>( _entity);
            _vmTester = new ViewModelTester<EditMainCrossingInspection, MainCrossingInspection>(_viewModel, _entity);

        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.InspectedOn);
            _vmTester.CanMapBothWays(x => x.PipeIsInService);
            _vmTester.CanMapBothWays(x => x.PipeHasExcessiveCorrosion);
            _vmTester.CanMapBothWays(x => x.PipeHasDelaminatedSteel);
            _vmTester.CanMapBothWays(x => x.PipeIsDamaged);
            _vmTester.CanMapBothWays(x => x.PipeHasCracks);
            _vmTester.CanMapBothWays(x => x.PipeHasConcreteSpools);
            _vmTester.CanMapBothWays(x => x.PipeLacksInsulation);
            _vmTester.CanMapBothWays(x => x.JointsAreLeaking);
            _vmTester.CanMapBothWays(x => x.JointsFailedSeparated);
            _vmTester.CanMapBothWays(x => x.JointsRestraintDamaged);
            _vmTester.CanMapBothWays(x => x.JointsBondStrapsDamaged);
            _vmTester.CanMapBothWays(x => x.SupportsHaveDeficientSupport);
            _vmTester.CanMapBothWays(x => x.SupportsAreDamaged);
            _vmTester.CanMapBothWays(x => x.SupportsHaveCorrosion);
            _vmTester.CanMapBothWays(x => x.EnvironmentIsInHazardousLocation);
            _vmTester.CanMapBothWays(x => x.EnvironmentHasDebrisBuildUp);
            _vmTester.CanMapBothWays(x => x.EnvironmentIsSubmergedInWater);
            _vmTester.CanMapBothWays(x => x.EnvironmentIsExposedToVehicleImpact);
            _vmTester.CanMapBothWays(x => x.EnvironmentIsNotSecuredFromPublic);
            _vmTester.CanMapBothWays(x => x.EnvironmentIsSusceptibleToStormDamage);
            _vmTester.CanMapBothWays(x => x.AdjacentFacilityHasBankErosion);
            _vmTester.CanMapBothWays(x => x.AdjacentFacilityHasBridgeDamage);
            _vmTester.CanMapBothWays(x => x.AdjacentFacilityHasPavementFailure);
            _vmTester.CanMapBothWays(x => x.AdjacentFacilityOverheadPowerLinesAreDown);
            _vmTester.CanMapBothWays(x => x.AdjacentFacilityHasPropertyDamage);
            _vmTester.CanMapBothWays(x => x.Comments);
        }

        [TestMethod]
        public void TestAssessmentRatingCanMapBothWays()
        {
            var rating = GetFactory<MainCrossingInspectionAssessmentRatingFactory>().Create();
            _entity.AssessmentRating = rating;
            _viewModel.AssessmentRating = null;
            _vmTester.MapToViewModel();
            Assert.AreEqual(rating.Id, _viewModel.AssessmentRating);

            _entity.AssessmentRating = null;
            _vmTester.MapToEntity();
            Assert.AreSame(rating, _entity.AssessmentRating);
        }

        [TestMethod]
        public void TestAllFieldsAreRequired()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AdjacentFacilityHasBankErosion);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AdjacentFacilityHasBridgeDamage);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AdjacentFacilityHasPavementFailure);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AdjacentFacilityHasPropertyDamage);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AdjacentFacilityOverheadPowerLinesAreDown);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AssessmentRating);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.EnvironmentHasDebrisBuildUp);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.EnvironmentIsExposedToVehicleImpact);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.EnvironmentIsInHazardousLocation);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.EnvironmentIsNotSecuredFromPublic);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.EnvironmentIsSubmergedInWater);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.EnvironmentIsSusceptibleToStormDamage);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.InspectedBy);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.InspectedOn);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.JointsAreLeaking);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.JointsBondStrapsDamaged);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.JointsFailedSeparated);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.JointsRestraintDamaged);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PipeHasConcreteSpools);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PipeHasCracks);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PipeHasDelaminatedSteel);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PipeHasExcessiveCorrosion);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PipeIsDamaged);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PipeIsInService);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PipeLacksInsulation);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.SupportsAreDamaged);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.SupportsHaveCorrosion);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.SupportsHaveDeficientSupport);
        }

        [TestMethod]
        public void TestValidationFailsIfInspectedByChangesAndCurrentUserIsNotAdmin()
        {
            // Sanity check
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.InspectedBy);
            
            var someUser = GetFactory<UserFactory>().Create();
            _viewModel.InspectedBy = someUser.Id;

            ValidationAssert.ModelStateHasError(_viewModel, x => x.InspectedBy, "Inspected By value can not be changed by a user.");

            _authServ.Setup(x => x.CurrentUserIsAdmin).Returns(true);
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.InspectedBy);

        }
        
        #endregion

    }
}
