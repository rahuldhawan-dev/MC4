using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Testing;
using Moq;
using StructureMap;
using System;
using System.Linq;
using IJobSiteCheckListRepository = MapCall.Common.Model.Repositories.IJobSiteCheckListRepository;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class BaseJobSiteCheckListViewModelTest : MapCallMvcInMemoryDatabaseTestBase<JobSiteCheckList>
    {
        #region Fields

        private ViewModelTester<JobSiteViewModel, JobSiteCheckList> _vmTester;
        private JobSiteViewModel _viewModel;
        private JobSiteCheckList _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private Mock<IJobSiteCheckListRepository> _mockRepo;
        //private JobSiteCheckListPressurizedRiskRestrainedType _yesPressure, _noPressure;


        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IAuthenticationService<User>>().Use((_authServ = new Mock<IAuthenticationService<User>>()).Object);
            e.For<IAuthenticationService>().Use(ctx => ctx.GetInstance<IAuthenticationService<User>>());
            e.For<IJobSiteCheckListRepository>().Use((_mockRepo = new Mock<IJobSiteCheckListRepository>()).Object);
            e.For<IEmployeeRepository>().Use<EmployeeRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<IWorkOrderRepository>().Use<WorkOrderRepository>();
            e.For<IJobSiteCheckListPressurizedRiskRestrainedTypeRepository>()
             .Use<JobSiteCheckListPressurizedRiskRestrainedTypeRepository>();
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        [TestInitialize]
        public void InitializeTest()
        {
            _entity = GetFactory<JobSiteCheckListFactory>().Create();
            _viewModel = _viewModelFactory.Build<JobSiteViewModel, JobSiteCheckList>(_entity);
            _vmTester = new ViewModelTester<JobSiteViewModel, JobSiteCheckList>(_viewModel, _entity);

            _user = new User();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            //_noPressure = GetFactory<NoJobSiteCheckListPressurizedRiskRestrainedTypeFactory>().Create();
            //_yesPressure = GetFactory<YesJobSiteCheckListPressurizedRiskRestrainedTypeFactory>().Create();
        }

        #endregion

        #region Tests

        #region Validation

        [TestMethod]
        public void TestCoordinateEntityMustExist()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.Coordinate, GetFactory<CoordinateFactory>().Create());
        }

        [TestMethod]
        public void TestMapCallWorkOrderEntityMustExist()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.MapCallWorkOrder, GetFactory<WorkOrderFactory>().Create());
        }

        [TestMethod]
        public void TestMarkoutNumberIsRequiredIfIsMarkoutValidForSiteIsTrue()
        {
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.MarkoutNumber, "abc", x => x.IsMarkoutValidForSite, true, false);
        }

        [TestMethod]
        public void TestAtmosphericOxygenLevelIsRequiredIfHasAtmosphereBeenTestedIsTrue()
        {
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.AtmosphericOxygenLevel, 24.42m, x => x.HasAtmosphereBeenTested, true, false, "Required");
        }

        [TestMethod]
        public void TestAtmosphericCarbonMonoxideLevelIsRequiredIfHasAtmosphereBeenTestedIsTrue()
        {
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.AtmosphericCarbonMonoxideLevel, 1.12m, x => x.HasAtmosphereBeenTested, true, false, "Required");
        }

        [TestMethod]
        public void TestAtmosphericLowerExplosiveLimitIsRequiredIfHasAtmosphereBeenTestedIsTrue()
        {
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.AtmosphericLowerExplosiveLimit, 24.42m, x => x.HasAtmosphereBeenTested, true, false, "Required");
        }

        [TestMethod]
        public void TestHasAtmosphereBeenTestedIsRequiredIfHasExcavationOverFourFeetDeepIsTrue()
        {
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.HasAtmosphereBeenTested, true, x => x.HasExcavationOverFourFeetDeep, true, false, "Required");
        }

        [TestMethod]
        public void TestIsLadderOnSlopeIsRequiredIfHasExcavationOverFourFeetDeepIsTrue()
        {

            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.IsLadderOnSlope, true, x => x.HasExcavationOverFourFeetDeep, true, false, "Required");
        }

        [TestMethod]
        public void TestIsALadderInPlaceIsRequiredIfHasExcavationOverFourFeetDeepIsTrue()
        {
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.IsALadderInPlace, true, x => x.HasExcavationOverFourFeetDeep, true, false, "Required");
        }

        [TestMethod]
        public void TestIsSlopeAngleNotLessThanOneHalfHorizontalToOneVerticalIsRequiredIfHasExcavationOverFiveFeetDeepIsTrue()
        {

            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.IsSlopeAngleNotLessThanOneHalfHorizontalToOneVertical, true, x => x.HasExcavationFiveFeetOrDeeper, true, false, "Required");
        }

        [TestMethod]
        public void TestIsShoringSystemUsedIsRequiredIfHasExcavationOverFiveFeetDeepIsTrue()
        {

            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.IsShoringSystemUsed, true, x => x.HasExcavationFiveFeetOrDeeper, true, false, "Required");
        }

        [TestMethod]
        public void TestLadderExtendsAboveGradeIsRequiredIfHasExcavationOverFourFeetDeepIsTrue()
        {

            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.LadderExtendsAboveGrade, true, x => x.HasExcavationOverFourFeetDeep, true, false, "Required");
        }

        [TestMethod]
        public void TestShoringSystemSidesExtendAboveBaseOfSlopeIsRequiredIfHasExcavationOverFiveFeetDeepIsTrue()
        {

            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.ShoringSystemSidesExtendAboveBaseOfSlope, true, x => x.HasExcavationFiveFeetOrDeeper, true, false, "Required");
        }

        [TestMethod]
        public void TestShoringSystemInstalledTwoFeetFromBottomOfTrenchIsRequiredIfHasExcavationOverFiveFeetDeepIsTrue()
        {
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.ShoringSystemInstalledTwoFeetFromBottomOfTrench, true, x => x.HasExcavationFiveFeetOrDeeper, true, false, "Required");
        }

        [TestMethod]
        public void TestValidationFailsIfAnExistingExcavationDoesNotBelongToThePostedCheckList()
        {
            var excavation = GetFactory<JobSiteExcavationFactory>().Create();
            Assert.AreNotEqual(_entity.Id, excavation.JobSiteCheckList.Id, "Sanity check");

            var excavationVm = _viewModelFactory.Build<CreateJobSiteExcavation, JobSiteExcavation>(excavation);
            _viewModel.Excavations.Add(excavationVm);

            ValidationAssert.ModelStateHasError(_viewModel, "Excavations",
                "An excavation belonging to another job site checklist can not be added to a different checklist.");
        }

        [TestMethod]
        public void TestValidationFailsIfNoExcavationsExistAndHasExcavationIsTrue()
        {
            _viewModel.HasExcavation = true;
            _viewModel.Excavations.Add(new CreateJobSiteExcavation(_container));
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.Excavations);

            _viewModel.Excavations.Clear();
            ValidationAssert.ModelStateHasError(_viewModel, "Excavations",
                "At least one excavation must be added to a job site checklist.");

            _viewModel.Excavations = null;
            ValidationAssert.ModelStateHasError(_viewModel, "Excavations",
                "At least one excavation must be added to a job site checklist.");
        }

        [TestMethod]
        public void TestValidationPassesForExcavationsIfHasExcavationIsFalseAndExcavationsIsEmpty()
        {
            _viewModel.HasExcavation = false;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.Excavations);

            _viewModel.Excavations.Clear();
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.Excavations);

            _viewModel.Excavations = null;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.Excavations);
        }

        [TestMethod]
        public void TestCompliesWithStandardsIsRequiredIfHasTrafficControlIsTrue()
        {
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.CompliesWithStandards, true, x => x.HasTrafficControl, true, false, "Required");
        }

        [TestMethod]
        public void TestIsEmergencyMarkoutRequestIsRequiredIfIsMarkoutValidForSiteIsTrue()
        {

            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.IsEmergencyMarkoutRequest, true, x => x.IsMarkoutValidForSite, true, false, "Required");
        }

        [TestMethod]
        public void TestHasTrafficControlIsInvalidIfTrueAndNoControlTypeHasBeenSelected()
        {
            const string errMessage = "At least one form of traffic control must be selected.";

            _viewModel.HasTrafficControl = false;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.HasTrafficControl);

            _viewModel.HasTrafficControl = true;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.HasTrafficControl, errMessage);

            // And now we have to check that it passes for all the values!

            _viewModel.HasBarricadesForTrafficControl = true;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.HasTrafficControl);
            _viewModel.HasBarricadesForTrafficControl = false;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.HasTrafficControl, errMessage);

            _viewModel.HasConesForTrafficControl = true;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.HasTrafficControl);
            _viewModel.HasConesForTrafficControl = false;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.HasTrafficControl, errMessage);

            _viewModel.HasFlagPersonForTrafficControl = true;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.HasTrafficControl);
            _viewModel.HasFlagPersonForTrafficControl = false;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.HasTrafficControl, errMessage);

            _viewModel.HasPoliceForTrafficControl = true;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.HasTrafficControl);
            _viewModel.HasPoliceForTrafficControl = false;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.HasTrafficControl, errMessage);

            _viewModel.HasSignsForTrafficControl = true;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.HasTrafficControl);
            _viewModel.HasSignsForTrafficControl = false;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.HasTrafficControl, errMessage);
        }

        [TestMethod]
        public void TestSupervisorSignOffDateIsRequiredWhenSupervisorSignOffEmployeeIsNotNull()
        {
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.SupervisorSignOffDate, DateTime.Now, x => x.SupervisorSignOffEmployee, 1, null);
        }

        [TestMethod]
        public void TestSupervisorSignOffEmployeeIsRequiredWhenSupervisorSignOffDateIsNotNull()
        {
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.SupervisorSignOffEmployee, 1, x => x.SupervisorSignOffDate, DateTime.Now, null);
        }

        [TestMethod]
        public void TestHasExcavationOverFourFeetDeepMustEqualTrueIfHasExcavationsFiveFeetOrDeeperIsAlsoTrue()
        {
            _viewModel.HasExcavationFiveFeetOrDeeper = true;
            _viewModel.HasExcavationOverFourFeetDeep = false;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.HasExcavationOverFourFeetDeep, "This must be checked when there are excavations five feet or deeper.");

            _viewModel.HasExcavationOverFourFeetDeep = true;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.HasExcavationOverFourFeetDeep);
        }

        [TestMethod]
        public void TestAtLeastOneProtectionTypeIsRequiredIfHasExcavationsFiveFeetOrDeeperIsTrue()
        {
            _viewModel.HasExcavationFiveFeetOrDeeper = true;
            _viewModel.ProtectionTypes.Clear();

            ValidationAssert.ModelStateHasError(_viewModel, "ProtectionTypes",
                "At least one protection type must be selected.");

            _viewModel.ProtectionTypes.Add(1);

            ValidationAssert.ModelStateIsValid(_viewModel, "ProtectionTypes");
        }

        [TestMethod]
        public void TestPressurizedRiskRestrainedTypeIsRequiredWhenIsPressurizedRisksRestrainedFieldRequiredIsTrue()
        {

            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.PressurizedRiskRestrainedType, JobSiteCheckListPressurizedRiskRestrainedType.Indices.YES, x => x.IsPressurizedRisksRestrainedFieldRequired, true, false);
        }

        [TestMethod]
        public void TestNoRestraintReasonIsRequiredWhenPressurizedRiskRestrainedTypeIsNo()
        {
            var orr = GetEntityFactory<JobSiteCheckListNoRestraintReasonType>().Create();
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.NoRestraintReason, orr.Id, x => x.PressurizedRiskRestrainedType, JobSiteCheckListPressurizedRiskRestrainedType.Indices.NO, JobSiteCheckListPressurizedRiskRestrainedType.Indices.YES);
        }

        [TestMethod]
        public void TestRestraintMethodIsRequiredWhenPressurizedRiskRestrainedTypeIsYes()
        {
            var rm = GetEntityFactory<JobSiteCheckListRestraintMethodType>().Create();
            // I don't understand why it's using this error message instead of the required one.
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.RestraintMethod, rm.Id, x => x.PressurizedRiskRestrainedType, JobSiteCheckListPressurizedRiskRestrainedType.Indices.YES, JobSiteCheckListPressurizedRiskRestrainedType.Indices.NO);

        }

        #endregion

        #endregion

        #region Test classes

        private class JobSiteViewModel : BaseJobSiteCheckListViewModel
        {
            public JobSiteViewModel(IContainer container) : base(container) { }
        }

        #endregion
    }
}
