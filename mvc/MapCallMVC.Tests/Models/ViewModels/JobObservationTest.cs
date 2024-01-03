using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class JobObservationTest : MapCallMvcInMemoryDatabaseTestBase<JobObservation>
    {
        #region Fields

        private JobObservationViewModel _viewModel;
        private JobObservation _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private ViewModelTester<JobObservationViewModel, JobObservation> _vmTester;
        private DateTime _now;
        private Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _entity = GetEntityFactory<JobObservation>().Create();
            _viewModel = _viewModelFactory.Build<JobObservationViewModel, JobObservation>(_entity);
            _vmTester = new ViewModelTester<JobObservationViewModel, JobObservation>(_viewModel, _entity);

            _user = new User();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _now = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(_now);
        }
        
        #endregion

        #region Mapping
        
        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.ObservationDate);
            _vmTester.CanMapBothWays(x => x.Address);
            _vmTester.CanMapBothWays(x => x.TaskObserved);
            _vmTester.CanMapBothWays(x => x.WhyWasTaskSafeOrAtRisk);
            _vmTester.CanMapBothWays(x => x.Deficiencies);
            _vmTester.CanMapBothWays(x => x.RecommendSolutions);
            _vmTester.CanMapBothWays(x => x.EqTruckForkliftsHoistsLadders);
            _vmTester.CanMapBothWays(x => x.EqFrontEndLoaderOrBackhoe);
            _vmTester.CanMapBothWays(x => x.EqOther);
            _vmTester.CanMapBothWays(x => x.CsPreEntryChecklistOrEntryPermit);
            _vmTester.CanMapBothWays(x => x.CsAtmosphereContinuouslyMonitored);
            _vmTester.CanMapBothWays(x => x.CsRetrievalEquipmentTripodHarnessWinch);
            _vmTester.CanMapBothWays(x => x.CsVentilationEquipment);
            _vmTester.CanMapBothWays(x => x.PpeHardHat);
            _vmTester.CanMapBothWays(x => x.PpeReflectiveVest);
            _vmTester.CanMapBothWays(x => x.PpeEyeProtection);
            _vmTester.CanMapBothWays(x => x.PpeEarProtection);
            _vmTester.CanMapBothWays(x => x.PpeFootProtection);
            _vmTester.CanMapBothWays(x => x.PpeGloves);
            _vmTester.CanMapBothWays(x => x.TcBarricadesConesBarrels);
            _vmTester.CanMapBothWays(x => x.TcAdvancedWarningSigns);
            _vmTester.CanMapBothWays(x => x.TcLightsArrowBoard);
            _vmTester.CanMapBothWays(x => x.TcPoliceFlagman);
            _vmTester.CanMapBothWays(x => x.TcWorkZoneInCompliance);
            _vmTester.CanMapBothWays(x => x.PsWalkwaysClear);
            _vmTester.CanMapBothWays(x => x.PsMaterialStockpile);
            _vmTester.CanMapBothWays(x => x.ExMarkoutRequestedForWorkSite);
            _vmTester.CanMapBothWays(x => x.ExWorkSiteSafetyCheckListUtilized);
            _vmTester.CanMapBothWays(x => x.ExUtilitiesSupportedProtected);
            _vmTester.CanMapBothWays(x => x.ExAtmosphereTestingPerformed);
            _vmTester.CanMapBothWays(x => x.ExSpoilPile2FeetFromEdgeOfExcavation);
            _vmTester.CanMapBothWays(x => x.ExLadderUsedIfGreaterThan4FeetDeep);
            _vmTester.CanMapBothWays(x => x.ExShoringNecessaryOver5FeetDeep);
            _vmTester.CanMapBothWays(x => x.ExProtectiveSystemInUseOver5Feet);
            _vmTester.CanMapBothWays(x => x.ExWaterControlSystemInUse);
            _vmTester.CanMapBothWays(x => x.ErChecklistUtilized);
            _vmTester.CanMapBothWays(x => x.ErErgonomicFactorsProhibitingGoodBodyMechanics);
            _vmTester.CanMapBothWays(x => x.ErToolsEquipmentUsedCorrectly);
        }

        [TestMethod]
        public void TestOperatingCenterCanMapBothWays()
        {
            var opc = GetEntityFactory<OperatingCenter>().Create();
            _entity.OperatingCenter = opc;
            
            _vmTester.MapToViewModel();

            Assert.AreEqual(opc.Id, _viewModel.OperatingCenter);

            _entity.OperatingCenter = null;
            _vmTester.MapToEntity();
            
            Assert.AreSame(opc, _entity.OperatingCenter);
        }

        [TestMethod]
        public void TestJobCategoryCanMapBothWays()
        {
            var jc = GetEntityFactory<JobCategory>().Create(new { Description = "foo" });
            _entity.Department = jc;

            _vmTester.MapToViewModel();
            
            Assert.AreEqual(jc.Id, _viewModel.Department);

            _entity.Department = null;
            _vmTester.MapToEntity();

            Assert.AreSame(jc, _entity.Department);
        }

        [TestMethod]
        public void TestCoordinateCanMapBothWays()
        {
            var coord = GetEntityFactory<Coordinate>().Create();
            _entity.Coordinate = coord;

            _vmTester.MapToViewModel();

            Assert.AreEqual(coord.Id, _viewModel.Coordinate);

            _entity.Coordinate = null;
            _vmTester.MapToEntity();

            Assert.AreSame(coord, _entity.Coordinate);
        }

        [TestMethod]
        public void TestOverallSafetyRatingCanMapBothWays()
        {
            var osr = GetEntityFactory<OverallSafetyRating>().Create(new {Description = "Foo"});
            _entity.OverallSafetyRating = osr;

            _vmTester.MapToViewModel();

            Assert.AreEqual(osr.Id, _viewModel.OverallSafetyRating);

            _entity.OverallSafetyRating = null;
            _vmTester.MapToEntity();

            Assert.AreSame(osr, _entity.OverallSafetyRating);
        }

        [TestMethod]
        public void TestOverallQualityRatingCanMapBothWays()
        {
            var oqr = GetEntityFactory<OverallQualityRating>().Create(new {Description = "Foo"});
            _entity.OverallQualityRating = oqr;

            _vmTester.MapToViewModel();

            Assert.AreEqual(oqr.Id, _viewModel.OverallQualityRating);

            _entity.OverallQualityRating = null;
            _vmTester.MapToEntity();

            Assert.AreSame(oqr, _entity.OverallQualityRating);
        }

        [TestMethod]
        public void TestSafetyLeadsDropdownsCanMapNullable()
        {
            var jc = GetEntityFactory<JobCategory>().Create(new { Description = "foo" });
            _entity.Department = jc;
            _entity.EqFrontEndLoaderOrBackhoe = false;
            _entity.EqTruckForkliftsHoistsLadders = true;

            _vmTester.MapToViewModel();

            Assert.AreEqual(jc.Id, _viewModel.Department);

            _entity.Department = null;
            _vmTester.MapToEntity();

            Assert.AreSame(jc, _entity.Department);

            Assert.IsFalse(_entity.EqFrontEndLoaderOrBackhoe);
            Assert.IsTrue(_entity.EqTruckForkliftsHoistsLadders);
            Assert.IsNull(_entity.EqOther);
            Assert.IsNull(_entity.CsPreEntryChecklistOrEntryPermit);
            Assert.IsNull(_entity.CsAtmosphereContinuouslyMonitored);
            Assert.IsNull(_entity.CsRetrievalEquipmentTripodHarnessWinch);
            Assert.IsNull(_entity.CsVentilationEquipment);
            Assert.IsNull(_entity.PpeHardHat);
            Assert.IsNull(_entity.PpeReflectiveVest);
            Assert.IsNull(_entity.PpeEyeProtection);
            Assert.IsNull(_entity.PpeEarProtection);
            Assert.IsNull(_entity.PpeFootProtection);
            Assert.IsNull(_entity.PpeGloves);
            Assert.IsNull(_entity.TcBarricadesConesBarrels);
            Assert.IsNull(_entity.TcAdvancedWarningSigns);
            Assert.IsNull(_entity.TcLightsArrowBoard);
            Assert.IsNull(_entity.TcPoliceFlagman);
            Assert.IsNull(_entity.TcWorkZoneInCompliance);
            Assert.IsNull(_entity.PsWalkwaysClear);
            Assert.IsNull(_entity.PsMaterialStockpile);
            Assert.IsNull(_entity.ExMarkoutRequestedForWorkSite);
            Assert.IsNull(_entity.ExWorkSiteSafetyCheckListUtilized);
            Assert.IsNull(_entity.ExUtilitiesSupportedProtected);
        }

        #endregion

        #region Validation

        [TestMethod]
        public void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Address, JobObservation.StringLengths.ADDRESS);
        }

        #endregion

        [TestMethod]
        public void TestSetDefaultsSetsDefaults()
        {
            _viewModel = new CreateJobObservation(_container);
            _viewModel.SetDefaults();

            Assert.AreEqual(_now, _viewModel.ObservationDate);
        }
    }
}
