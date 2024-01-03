using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using Moq;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class CreateJobSiteExcavationTest : MapCallMvcInMemoryDatabaseTestBase<JobSiteExcavation>
    {
        #region Fields

        private ViewModelTester<CreateJobSiteExcavation, JobSiteExcavation> _vmTester;
        private CreateJobSiteExcavation _viewModel;
        private JobSiteExcavation _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _entity = GetFactory<JobSiteExcavationFactory>().Create();
            _viewModel = _viewModelFactory.Build<CreateJobSiteExcavation, JobSiteExcavation>( _entity);
            _vmTester = new ViewModelTester<CreateJobSiteExcavation, JobSiteExcavation>(_viewModel, _entity);

            _authServ = new Mock<IAuthenticationService<User>>();
            _user = new User();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _container.Inject(_authServ.Object);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestLocationTypeDescriptionReturnsExactlyThat()
        {
            var locType1 = GetFactory<JobSiteExcavationLocationTypeFactory>().Create(new { Description = "Hello" });
            var locType2 = GetFactory<JobSiteExcavationLocationTypeFactory>().Create(new { Description = "Is it me you're looking for?" });

            _viewModel.LocationType = locType1.Id;

            Assert.AreEqual("Hello", _viewModel.LocationTypeDescription);

            _viewModel.LocationType = locType2.Id;

            Assert.AreEqual("Is it me you're looking for?", _viewModel.LocationTypeDescription);
        }

        [TestMethod]
        public void TestSoilTypeDescriptionReturnsExactlyThat()
        {
            var soil1 = GetFactory<JobSiteExcavationSoilTypeFactory>().Create(new { Description = "Hello" });
            var soil2 = GetFactory<JobSiteExcavationSoilTypeFactory>().Create(new { Description = "Is it me you're looking for?" });

            _viewModel.SoilType = soil1.Id;

            Assert.AreEqual("Hello", _viewModel.SoilTypeDescription);

            _viewModel.SoilType = soil2.Id;

            Assert.AreEqual("Is it me you're looking for?", _viewModel.SoilTypeDescription);
        }

        #region Mapping

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.DepthInInches);
            _vmTester.CanMapBothWays(x => x.ExcavationDate);
            _vmTester.CanMapBothWays(x => x.LengthInFeet);
            _vmTester.CanMapBothWays(x => x.WidthInFeet);
        }

        [TestMethod]
        public void TestLocationTypeCanMapBothWays()
        {
            var locType = GetFactory<JobSiteExcavationLocationTypeFactory>().Create();
            _entity.LocationType = locType;
            _vmTester.MapToViewModel();
            Assert.AreEqual(locType.Id, _viewModel.LocationType);

            _entity.LocationType = null;
            _vmTester.MapToEntity();
            Assert.AreSame(locType, _entity.LocationType);
        }

        [TestMethod]
        public void TestSoilTypeCanMapBothWays()
        {
            var soil = GetFactory<JobSiteExcavationSoilTypeFactory>().Create();
            _entity.SoilType = soil;
            _vmTester.MapToViewModel();
            Assert.AreEqual(soil.Id, _viewModel.SoilType);

            _entity.SoilType = null;
            _vmTester.MapToEntity();
            Assert.AreSame(soil, _entity.SoilType);
        }

        [TestMethod]
        public void TestMapToEntitySetsCreatedByToCurrentUserForNewExcavations()
        {
            _user.UserName = "Wow!";
            
            _vmTester.MapToEntity();

            Assert.AreEqual("Wow!", _entity.CreatedBy);
        }

        #endregion

        #region Validation

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.DepthInInches);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ExcavationDate);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.LengthInFeet);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.LocationType);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.SoilType);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.WidthInFeet);
        }

        [TestMethod]
        public void TestMinValueRequiredFields()
        {
            ValidationAssert.PropertyHasMinValueRequirement(_viewModel, x => x.DepthInInches, 1m, error: "Depth must be in inches.");
            ValidationAssert.PropertyHasMinValueRequirement(_viewModel, x => x.LengthInFeet, 0m);
            ValidationAssert.PropertyHasMinValueRequirement(_viewModel, x => x.WidthInFeet, 0m);
        }

        [TestMethod]
        public void TestEntityMustExistRequirements()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.LocationType, GetFactory<JobSiteExcavationLocationTypeFactory>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.SoilType, GetFactory<JobSiteExcavationSoilTypeFactory>().Create());
        }

        #endregion

        #endregion
    }
}
