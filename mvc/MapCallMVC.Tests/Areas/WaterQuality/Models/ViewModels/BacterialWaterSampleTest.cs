using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.Repositories.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.WaterQuality.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace MapCallMVC.Tests.Areas.WaterQuality.Models.ViewModels
{
    [TestClass]
    public class CreateBacterialWaterSampleTest : MapCallMvcInMemoryDatabaseTestBase<BacterialWaterSample>
    {
        #region Fields

        private ViewModelTester<CreateBacterialWaterSample, BacterialWaterSample> _vmTester;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private CreateBacterialWaterSample _viewModel;
        private BacterialWaterSample _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private Mock<IRoleService> _roleService;
        private Mock<IUserRepository> _userRepo;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _roleService = e.For<IRoleService>().Mock();
            _userRepo = e.For<IUserRepository>().Mock();
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
            e.For<ISampleSiteRepository>().Use<SampleSiteRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create(new { IsAdmin = true });
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _viewModel = new CreateBacterialWaterSample(_container);
            _entity = new BacterialWaterSample();
            _vmTester = new ViewModelTester<CreateBacterialWaterSample, BacterialWaterSample>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.SampleCollectionDTM);
            _vmTester.CanMapBothWays(x => x.ReceivedByLabDTM);
            _vmTester.CanMapBothWays(x => x.Location);
            _vmTester.CanMapBothWays(x => x.Cl2Free);
            _vmTester.CanMapBothWays(x => x.Cl2Total);
            _vmTester.CanMapBothWays(x => x.Nitrite);
            _vmTester.CanMapBothWays(x => x.Nitrate);
            _vmTester.CanMapBothWays(x => x.FinalHPC);
            _vmTester.CanMapBothWays(x => x.Monochloramine);
            _vmTester.CanMapBothWays(x => x.FreeAmmonia);
            _vmTester.CanMapBothWays(x => x.Ph);
            _vmTester.CanMapBothWays(x => x.Temperature);
            _vmTester.CanMapBothWays(x => x.Iron);
            _vmTester.CanMapBothWays(x => x.Manganese);
            _vmTester.CanMapBothWays(x => x.Turbidity);
            _vmTester.CanMapBothWays(x => x.OrthophosphateAsP);
            _vmTester.CanMapBothWays(x => x.Conductivity);
            _vmTester.CanMapBothWays(x => x.Alkalinity);
            _vmTester.CanMapBothWays(x => x.NonSheenColonyCount);
            _vmTester.CanMapBothWays(x => x.SheenColonyCount);
            _vmTester.CanMapBothWays(x => x.ColiformConfirm);
            _vmTester.CanMapBothWays(x => x.EColiConfirm);
            _vmTester.CanMapBothWays(x => x.SAPWorkOrderId);
            _vmTester.CanMapBothWays(x => x.ColiformSetupDTM);
            _vmTester.CanMapBothWays(x => x.ColiformReadDTM);
            //_vmTester.CanMapBothWays(x => x.ColiformReadAnalyst);
            //_vmTester.CanMapBothWays(x => x.ColiformSetupAnalyst);
            //_vmTester.CanMapBothWays(x => x.HPCReadAnalyst);
            //_vmTester.CanMapBothWays(x => x.HPCSetupAnalyst);
            _vmTester.CanMapBothWays(x => x.HPCSetupDTM);
            _vmTester.CanMapBothWays(x => x.HPCReadDTM);
            _vmTester.CanMapBothWays(x => x.Address);
            _vmTester.CanMapBothWays(x => x.IsSpreader);
            _vmTester.CanMapBothWays(x => x.IsInvalid);
        }

        [TestMethod]
        public void TestColiformSetupAnalystCanMapBothWays()
        {
            var analyst = GetEntityFactory<BacterialWaterSampleAnalyst>().Create();
            _entity.ColiformSetupAnalyst = analyst;
            _vmTester.MapToViewModel();
            Assert.AreEqual(analyst.Id, _viewModel.ColiformSetupAnalyst);

            _entity.ColiformSetupAnalyst = null;
            _vmTester.MapToEntity();
            Assert.AreSame(analyst, _entity.ColiformSetupAnalyst);
        }

        [TestMethod]
        public void TestColiformReadAnalystCanMapBothWays()
        {
            var analyst = GetEntityFactory<BacterialWaterSampleAnalyst>().Create();
            _entity.ColiformReadAnalyst = analyst;
            _vmTester.MapToViewModel();
            Assert.AreEqual(analyst.Id, _viewModel.ColiformReadAnalyst);

            _entity.ColiformReadAnalyst = null;
            _vmTester.MapToEntity();
            Assert.AreSame(analyst, _entity.ColiformReadAnalyst);
        }

        [TestMethod]
        public void TestHPCSetupAnalystCanMapBothWays()
        {
            var analyst = GetEntityFactory<BacterialWaterSampleAnalyst>().Create();
            _entity.HPCSetupAnalyst = analyst;
            _vmTester.MapToViewModel();
            Assert.AreEqual(analyst.Id, _viewModel.HPCSetupAnalyst);

            _entity.HPCSetupAnalyst = null;
            _vmTester.MapToEntity();
            Assert.AreSame(analyst, _entity.HPCSetupAnalyst);
        }

        [TestMethod]
        public void TestHPCReadAnalystCanMapBothWays()
        {
            var analyst = GetEntityFactory<BacterialWaterSampleAnalyst>().Create();
            _entity.HPCReadAnalyst = analyst;
            _vmTester.MapToViewModel();
            Assert.AreEqual(analyst.Id, _viewModel.HPCReadAnalyst);

            _entity.HPCReadAnalyst = null;
            _vmTester.MapToEntity();
            Assert.AreSame(analyst, _entity.HPCReadAnalyst);
        }

        [TestMethod]
        public void TestReasonForInvalidationCanMapBothWays()
        {
            var reason = GetEntityFactory<BacterialWaterSampleReasonForInvalidation>().Create();
            _entity.ReasonForInvalidation = reason;
            _vmTester.MapToViewModel();
            Assert.AreEqual(reason.Id, _viewModel.ReasonForInvalidation);

            _entity.ReasonForInvalidation = null;
            _vmTester.MapToEntity();
            Assert.AreSame(reason, _entity.ReasonForInvalidation);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.SampleCollectionDTM);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.BacterialSampleType);
        }

        [TestMethod]
        public void TestMapToEntitySetsDataEnteredToNow()
        {
            var expected = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expected);

            Assert.AreNotEqual(expected, _entity.DataEntered);

            _vmTester.MapToEntity();

            Assert.AreEqual(expected, _entity.DataEntered);
        }

        [TestMethod]
        public void TestMapToEntitySetsCollectedBy()
        {
            var userFromRepo = new User {UserName = "this thing"};
            _userRepo.Setup(x => x.Where(It.Is<Expression<Func<User, bool>>>(fn => fn.Compile().Invoke(userFromRepo)))).Returns(new [] {userFromRepo}.AsQueryable());

            // If viewmodel.CollectedBy is null, then it must be set to the current user
            _user.UserName = "some user";
            _entity.CollectedBy = null;
            _roleService.Setup(x => x.CanAccessRole(RoleModules.WaterQualityGeneral, RoleActions.UserAdministrator, null)).Returns(false);
            _vmTester.MapToEntity();
            Assert.AreSame(_user, _entity.CollectedBy);
//            Assert.AreEqual("some user", _entity.CollectedBy);

            // If user is WaterQualityGeneral user admin, then it must be set to what the viewmodel says unless it's null.
            _roleService.Setup(x => x.CanAccessRole(RoleModules.WaterQualityGeneral, RoleActions.UserAdministrator, null)).Returns(true);
            _viewModel.CollectedBy = "this thing";
            _vmTester.MapToEntity();
            Assert.AreSame(userFromRepo, _entity.CollectedBy);
           // Assert.AreEqual("this thing", _entity.CollectedBy);
            _viewModel.CollectedBy = null;
            _entity.CollectedBy = null;
            _vmTester.MapToEntity();
            Assert.AreSame(_user, _entity.CollectedBy);
            //Assert.AreEqual("some user", _entity.CollectedBy);

            // If user is not WaterQualityGeneral user admin, then it must be set to current user unless entity.CollectedBy is already set.
            _roleService.Setup(x => x.CanAccessRole(RoleModules.WaterQualityGeneral, RoleActions.UserAdministrator, null)).Returns(false);
            _entity.CollectedBy = null;
            _viewModel.CollectedBy = "this should be ignored";
            _vmTester.MapToEntity();
            Assert.AreSame(_user, _entity.CollectedBy);
            _user.UserName = "wrong";
            _vmTester.MapToEntity();
            Assert.AreSame(_user, _entity.CollectedBy);
        }

        [TestMethod]
        public void TestSampleSiteCanMapBothWays()
        {
            var sampleSite = GetEntityFactory<SampleSite>().Create();
            _entity.SampleSite = sampleSite;

            _vmTester.MapToViewModel();

            Assert.AreEqual(sampleSite.Id, _viewModel.SampleSite);

            _entity.SampleSite = null;
            _vmTester.MapToEntity();

            Assert.AreSame(sampleSite, _entity.SampleSite);
        }

        [TestMethod]
        public void TestNonSheenColonyCountOperatorCanMapBothWays()
        {
            var prop = GetEntityFactory<NonSheenColonyCountOperator>().Create(new {Description = "Foo"});
            _entity.NonSheenColonyCountOperator = prop;

            _vmTester.MapToViewModel();

            Assert.AreEqual(prop.Id, _viewModel.NonSheenColonyCountOperator);

            _entity.NonSheenColonyCountOperator = null;
            _vmTester.MapToEntity();

            Assert.AreSame(prop, _entity.NonSheenColonyCountOperator);
        }

        [TestMethod]
        public void TestSheenColonyCountOperatorCanMapBothWays()
        {
            var prop = GetEntityFactory<SheenColonyCountOperator>().Create(new {Description = "Foo"});
            _entity.SheenColonyCountOperator = prop;

            _vmTester.MapToViewModel();

            Assert.AreEqual(prop.Id, _viewModel.SheenColonyCountOperator);

            _entity.SheenColonyCountOperator = null;
            _vmTester.MapToEntity();

            Assert.AreSame(prop, _entity.SheenColonyCountOperator);
        }

        [TestMethod]
        public void TestBacterialSampleTypeCanMapBothWays()
        {
            var bacterialSampleType = GetFactory<RoutineBacterialSampleTypeFactory>().Create(new {Description = "Foo"});
            _entity.BacterialSampleType = bacterialSampleType;

            _vmTester.MapToViewModel();

            Assert.AreEqual(bacterialSampleType.Id, _viewModel.BacterialSampleType);

            _entity.BacterialSampleType = null;
            _vmTester.MapToEntity();

            Assert.AreSame(bacterialSampleType, _entity.BacterialSampleType);
        }

        [TestMethod]
        public void TestEstimatingProjectCanMapBothWays()
        {
            var estimatingProject = GetEntityFactory<EstimatingProject>().Create(new {Description = "Foo"});
            _entity.EstimatingProject = estimatingProject;

            _vmTester.MapToViewModel();

            Assert.AreEqual(estimatingProject.Id, _viewModel.EstimatingProject);

            _entity.EstimatingProject = null;
            _vmTester.MapToEntity();

            Assert.AreSame(estimatingProject, _entity.EstimatingProject);
        }

        [TestMethod]
        public void TestBacterialWaterSampleCanMapBothWays()
        {
            var origBacterialWaterSample = GetEntityFactory<BacterialWaterSample>().Create();
            _entity.OriginalBacterialWaterSample = origBacterialWaterSample;

            _vmTester.MapToViewModel();

            Assert.AreEqual(origBacterialWaterSample.Id, _viewModel.OriginalBacterialWaterSample);

            _entity.OriginalBacterialWaterSample = null;
            _vmTester.MapToEntity();

            Assert.AreSame(origBacterialWaterSample, _entity.OriginalBacterialWaterSample);
        }

        [TestMethod]
        public void TestTownCanMapBothWays()
        {
            var town = GetEntityFactory<Town>().Create(new {ShortName= "Foo"});
            _entity.SampleTown = town;

            _vmTester.MapToViewModel();

            Assert.AreEqual(town.Id, _viewModel.SampleTown);

            _entity.SampleTown = null;
            _vmTester.MapToEntity();

            Assert.AreSame(town, _entity.SampleTown);
        }

        [TestMethod]
        public void TestCoordinateCanMapBothWays()
        {
            var coordinate = GetEntityFactory<Coordinate>().Create();
            _entity.SampleCoordinate = coordinate;

            _vmTester.MapToViewModel();

            Assert.AreEqual(coordinate.Id, _viewModel.SampleCoordinate);

            _entity.SampleCoordinate = null;
            _vmTester.MapToEntity();

            Assert.AreSame(coordinate, _entity.SampleCoordinate);
        }

        #region Mapping IsReadyForLIMS

        [TestMethod]
        public void TestShouldDisplayReadyForLIMSFieldReturnsTrueIfIsReadyForLIMSHasNonNullValue()
        {
            _viewModel.IsReadyForLIMS = null;
            Assert.IsFalse(_viewModel.ShouldDisplayReadyForLIMSField);

            _viewModel.IsReadyForLIMS = false;
            Assert.IsTrue(_viewModel.ShouldDisplayReadyForLIMSField);

            _viewModel.IsReadyForLIMS = true;
            Assert.IsTrue(_viewModel.ShouldDisplayReadyForLIMSField);
        }

        [TestMethod]
        public void TestMapSetsIsReadyForLIMSForVariousSituations()
        {
            var notReady = GetFactory<NotReadyLIMSStatusFactory>().Create();
            var ready = GetFactory<ReadyToSendLIMSStatusFactory>().Create();
            var sentSuccessfully = GetFactory<SentSuccessfullyLIMSStatusFactory>().Create();
            var sendFailed = GetFactory<SendFailedLIMSStatusFactory>().Create();

            // If entity's status is null, IsReadyForLIMS should be false
            _entity.LIMSStatus = null;
            _vmTester.MapToViewModel();
            Assert.IsFalse(_viewModel.IsReadyForLIMS.Value);

            // If entity's status is Not Ready, IsReadyForLIMS should be false 
            _entity.LIMSStatus = notReady;
            _vmTester.MapToViewModel();
            Assert.IsFalse(_viewModel.IsReadyForLIMS.Value);

            // if entity's status is Ready to Send, IsReadyForLIMS should be true 
            _entity.LIMSStatus = ready;
            _vmTester.MapToViewModel();
            Assert.IsTrue(_viewModel.IsReadyForLIMS.Value);

            // If entity's status is anything else, IsReadyForLIMS should be NULL
            _entity.LIMSStatus = sentSuccessfully;
            _vmTester.MapToViewModel();
            Assert.IsNull(_viewModel.IsReadyForLIMS);
            _entity.LIMSStatus = sendFailed;
            _vmTester.MapToViewModel();
            Assert.IsNull(_viewModel.IsReadyForLIMS);
        }

        [TestMethod]
        public void TestMapToEntitySetsLIMSStatusInVariousSituations()
        {
            var notReady = GetFactory<NotReadyLIMSStatusFactory>().Create();
            var ready = GetFactory<ReadyToSendLIMSStatusFactory>().Create();
            var sentSuccessfully = GetFactory<SentSuccessfullyLIMSStatusFactory>().Create();
            var sendFailed = GetFactory<SendFailedLIMSStatusFactory>().Create();

            // If IsReadyForLIMS is null, do not update the status
            _viewModel.IsReadyForLIMS = null;
            _entity.LIMSStatus = sendFailed; // Setting this since it shouldn't be updated
            _vmTester.MapToEntity();
            Assert.AreSame(sendFailed, _entity.LIMSStatus);

            // If IsReadyForLIMS is false, status should be Not Ready
            _entity.LIMSStatus = null; // Needs to be nulled out due to the logic behind setting it to sendFailed above.
            _viewModel.IsReadyForLIMS = false;
            _vmTester.MapToEntity();
            Assert.AreSame(notReady, _entity.LIMSStatus);

            // If IsReadyForLIMS is true, status should be ready. 
            _viewModel.IsReadyForLIMS = true;
            _vmTester.MapToEntity();
            Assert.AreSame(ready, _entity.LIMSStatus);
        }

        [TestMethod]
        public void TestMapToEntityDoesNotSetLIMSStatusIfLIMSStatusIsSentSuccessfully()
        {
            var notReady = GetFactory<NotReadyLIMSStatusFactory>().Create();
            var ready = GetFactory<ReadyToSendLIMSStatusFactory>().Create();
            var sentSuccessfully = GetFactory<SentSuccessfullyLIMSStatusFactory>().Create();
            var sendFailed = GetFactory<SendFailedLIMSStatusFactory>().Create();

            // If IsReadyForLIMS is false, do not update the status
            _viewModel.IsReadyForLIMS = false;
            _entity.LIMSStatus = sentSuccessfully; // Setting this since it shouldn't be updated
            _vmTester.MapToEntity();
            Assert.AreSame(sentSuccessfully, _entity.LIMSStatus);

            // Same test but with sendFailed
            _viewModel.IsReadyForLIMS = false;
            _entity.LIMSStatus = sendFailed; // Setting this since it shouldn't be updated
            _vmTester.MapToEntity();
            Assert.AreSame(sendFailed, _entity.LIMSStatus);
        }


        #endregion

        [TestMethod]
        public void TestSetDefaultsSetsValuesForSampleSite()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var sampleSite = GetEntityFactory<SampleSite>().Create(new { OperatingCenter = operatingCenter });
            var sampleIdMatrix = GetEntityFactory<SampleIdMatrix>().Create(new { SampleSite = sampleSite });
            Session.Evict(sampleSite);
            sampleSite = Session.Load<SampleSite>(sampleSite.Id);

            _viewModel.SampleSite = sampleSite.Id;

            _viewModel.SetDefaults();

            Assert.AreEqual(operatingCenter.Id, _viewModel.OperatingCenter);
            Assert.AreEqual(sampleSite.Id, _viewModel.SampleSite);
        }


        [TestMethod]
        public void TestSetDefaultsSetsIsReadyForLIMSToFalse()
        {
            _viewModel.IsReadyForLIMS = null;
            _viewModel.SetDefaults();
            Assert.IsFalse(_viewModel.IsReadyForLIMS.Value);
        }

        #region Validation

        [TestMethod]
        public void TestValidateForCl2Total()
        {
            var pwsid = GetFactory<PublicWaterSupplyFactory>().Create();
            var sampleSite = GetFactory<SampleSiteFactory>().Create();

            Action<decimal?, int?> isValid = (chlorine, sampleSiteId) =>
            {
                _viewModel.Cl2Total = chlorine;
                _viewModel.SampleSite = sampleSiteId;
                ValidationAssert.ModelStateIsValid(_viewModel, x => x.Cl2Total);
            };
            
            Action<decimal?, int?> isInvalid = (chlorine, sampleSiteId) =>
            {
                _viewModel.Cl2Total = chlorine;
                _viewModel.SampleSite = sampleSiteId;
                ValidationAssert.ModelStateHasError(_viewModel, x => x.Cl2Total, "Cl2Total is required for the selected Sample Site.");
            };
            
            // Must always return true if SampleSite is null
            isValid(null, null);
            isValid(0m, null);

            // Must always return true if PWSID is null
            sampleSite.PublicWaterSupply = null;
            isValid(null, sampleSite.Id);
            isValid(0m, sampleSite.Id);

            // Must always return true if PWSID is not null and TotalChlorineReported is false
            pwsid.TotalChlorineReported = false;
            sampleSite.PublicWaterSupply = pwsid;
            isValid(null, sampleSite.Id);
            isValid(0m, sampleSite.Id);

            // Must return false if PWSID.TotalChlorineReported is true and the chlorine value is null
            pwsid.TotalChlorineReported = true;
            isInvalid(null, sampleSite.Id);
            isValid(0m, sampleSite.Id);
        }

        [TestMethod]
        public void TestValidateForCl2Free()
        {
            var pwsid = GetFactory<PublicWaterSupplyFactory>().Create();
            var sampleSite = GetFactory<SampleSiteFactory>().Create();

            Action<decimal?, int?> isValid = (chlorine, sampleSiteId) =>
            {
                _viewModel.Cl2Free = chlorine;
                _viewModel.SampleSite = sampleSiteId;
                ValidationAssert.ModelStateIsValid(_viewModel, x => x.Cl2Free);
            };

            Action<decimal?, int?> isInvalid = (chlorine, sampleSiteId) =>
            {
                _viewModel.Cl2Free = chlorine;
                _viewModel.SampleSite = sampleSiteId;
                ValidationAssert.ModelStateHasError(_viewModel, x => x.Cl2Free, "Cl2Free is required for the selected Sample Site.");
            };

            // Must always return true if SampleSite is null
            isValid(null, null);
            isValid(0m, null);

            // Must always return true if PWSID is null
            sampleSite.PublicWaterSupply = null;
            isValid(null, sampleSite.Id);
            isValid(0m, sampleSite.Id);

            // Must always return true if PWSID is not null and FreeChlorineReported is false
            pwsid.FreeChlorineReported = false;
            sampleSite.PublicWaterSupply = pwsid;
            isValid(null, sampleSite.Id);
            isValid(0m, sampleSite.Id);

            // Must return false if PWSID.FreeChlorineReported is true and the chlorine value is null
            pwsid.FreeChlorineReported = true;
            isInvalid(null, sampleSite.Id);
            isValid(0m, sampleSite.Id);
        }

        [TestMethod]
        public void TestReasonForInvalidationIsRequiredWhenIsInvalidIsTrue()
        {
            var reason = GetEntityFactory<BacterialWaterSampleReasonForInvalidation>().Create();

            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.ReasonForInvalidation, reason.Id, x => x.IsInvalid, true, false);
        }

        #endregion

        #endregion
    }

    [TestClass]
    public class EditBacterialWaterSampleTest : MapCallMvcInMemoryDatabaseTestBase<BacterialWaterSample>
    {
        #region Fields

        private ViewModelTester<EditBacterialWaterSample, BacterialWaterSample> _vmTester;
        private EditBacterialWaterSample _viewModel;
        private BacterialWaterSample _entity;
        private User _user;
        private Mock<IRoleService> _roleService;
        private Mock<IAuthenticationService<User>> _authServ;
        private Mock<IUserRepository> _userRepo;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _roleService = e.For<IRoleService>().Mock();
            _userRepo = e.For<IUserRepository>().Mock();
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            e.For<ISampleSiteRepository>().Use<SampleSiteRepository>();
            e.For<ISampleSiteRepository>().Use<SampleSiteRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new EditBacterialWaterSample(_container);
            _entity = new BacterialWaterSample();
            _vmTester = new ViewModelTester<EditBacterialWaterSample, BacterialWaterSample>(_viewModel, _entity);
            _user = GetFactory<UserFactory>().Create(new { IsAdmin = true });
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _container.Inject(_authServ.Object);
            _container.Inject(_roleService.Object);
            _container.Inject(_userRepo.Object);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.BacterialSampleType);
        }

        [TestMethod]
        public void TestMapToEntitySetsCollectedBy()
        {
            var userFromRepo = new User {UserName = "this thing"};
            _userRepo.Setup(x => x.Where(It.Is<Expression<Func<User, bool>>>(fn => fn.Compile().Invoke(userFromRepo)))).Returns(new [] {userFromRepo}.AsQueryable());

            // If viewmodel.CollectedBy is null, then it must be set to the current user
            _user.UserName = "some user";
            _entity.CollectedBy = null;
            _roleService.Setup(x => x.CanAccessRole(RoleModules.WaterQualityGeneral, RoleActions.UserAdministrator, null)).Returns(false);
            _vmTester.MapToEntity();
            Assert.AreSame(_user, _entity.CollectedBy);
            //            Assert.AreEqual("some user", _entity.CollectedBy);

            // If user is WaterQualityGeneral user admin, then it must be set to what the viewmodel says unless it's null.
            _roleService.Setup(x => x.CanAccessRole(RoleModules.WaterQualityGeneral, RoleActions.UserAdministrator, null)).Returns(true);
            _viewModel.CollectedBy = "this thing";
            _vmTester.MapToEntity();
            Assert.AreSame(userFromRepo, _entity.CollectedBy);
            // Assert.AreEqual("this thing", _entity.CollectedBy);
            _viewModel.CollectedBy = null;
            _entity.CollectedBy = null;
            _vmTester.MapToEntity();
            Assert.AreSame(_user, _entity.CollectedBy);
            //Assert.AreEqual("some user", _entity.CollectedBy);

            // If user is not WaterQualityGeneral user admin, then it must be set to current user unless entity.CollectedBy is already set.
            _roleService.Setup(x => x.CanAccessRole(RoleModules.WaterQualityGeneral, RoleActions.UserAdministrator, null)).Returns(false);
            _entity.CollectedBy = null;
            _viewModel.CollectedBy = "this should be ignored";
            _vmTester.MapToEntity();
            Assert.AreSame(_user, _entity.CollectedBy);
            _user.UserName = "wrong";
            _vmTester.MapToEntity();
            Assert.AreSame(_user, _entity.CollectedBy);
        }

        #region Mapping IsReadyForLIMS

        [TestMethod]
        public void TestShouldDisplayReadyForLIMSFieldReturnsTrueIfIsReadyForLIMSHasNonNullValue()
        {
            _viewModel.IsReadyForLIMS = null;
            Assert.IsFalse(_viewModel.ShouldDisplayReadyForLIMSField);

            _viewModel.IsReadyForLIMS = false;
            Assert.IsTrue(_viewModel.ShouldDisplayReadyForLIMSField);

            _viewModel.IsReadyForLIMS = true;
            Assert.IsTrue(_viewModel.ShouldDisplayReadyForLIMSField);
        }

        [TestMethod]
        public void TestMapSetsIsReadyForLIMSForVariousSituations()
        {
            var notReady = GetFactory<NotReadyLIMSStatusFactory>().Create();
            var ready = GetFactory<ReadyToSendLIMSStatusFactory>().Create();
            var sentSuccessfully = GetFactory<SentSuccessfullyLIMSStatusFactory>().Create();
            var sendFailed = GetFactory<SendFailedLIMSStatusFactory>().Create();

            // If entity's status is null, IsReadyForLIMS should be false
            _entity.LIMSStatus = null;
            _vmTester.MapToViewModel();
            Assert.IsFalse(_viewModel.IsReadyForLIMS.Value);

            // If entity's status is Not Ready, IsReadyForLIMS should be false 
            _entity.LIMSStatus = notReady;
            _vmTester.MapToViewModel();
            Assert.IsFalse(_viewModel.IsReadyForLIMS.Value);

            // if entity's status is Ready to Send, IsReadyForLIMS should be true 
            _entity.LIMSStatus = ready;
            _vmTester.MapToViewModel();
            Assert.IsTrue(_viewModel.IsReadyForLIMS.Value);

            // If entity's status is anything else, IsReadyForLIMS should be NULL
            _entity.LIMSStatus = sentSuccessfully;
            _vmTester.MapToViewModel();
            Assert.IsNull(_viewModel.IsReadyForLIMS);
            _entity.LIMSStatus = sendFailed;
            _vmTester.MapToViewModel();
            Assert.IsNull(_viewModel.IsReadyForLIMS);
        }

        [TestMethod]
        public void TestMapToEntitySetsLIMSStatusInVariousSituations()
        {
            var notReady = GetFactory<NotReadyLIMSStatusFactory>().Create();
            var ready = GetFactory<ReadyToSendLIMSStatusFactory>().Create();
            var sentSuccessfully = GetFactory<SentSuccessfullyLIMSStatusFactory>().Create();
            var sendFailed = GetFactory<SendFailedLIMSStatusFactory>().Create();

            // If IsReadyForLIMS is null, do not update the status
            _viewModel.IsReadyForLIMS = null;
            _entity.LIMSStatus = sendFailed; // Setting this since it shouldn't be updated
            _vmTester.MapToEntity();
            Assert.AreSame(sendFailed, _entity.LIMSStatus);

            // If IsReadyForLIMS is false, status should be Not Ready
            _entity.LIMSStatus = null; // Needs to be nulled out due to the logic behind setting it to sendFailed above.
            _viewModel.IsReadyForLIMS = false;
            _vmTester.MapToEntity();
            Assert.AreSame(notReady, _entity.LIMSStatus);

            // If IsReadyForLIMS is true, status should be ready. 
            _viewModel.IsReadyForLIMS = true;
            _vmTester.MapToEntity();
            Assert.AreSame(ready, _entity.LIMSStatus);
        }

        [TestMethod]
        public void TestMapToEntityDoesNotSetLIMSStatusIfLIMSStatusIsSentSuccessfully()
        {
            var notReady = GetFactory<NotReadyLIMSStatusFactory>().Create();
            var ready = GetFactory<ReadyToSendLIMSStatusFactory>().Create();
            var sentSuccessfully = GetFactory<SentSuccessfullyLIMSStatusFactory>().Create();
            var sendFailed = GetFactory<SendFailedLIMSStatusFactory>().Create();

            // If IsReadyForLIMS is false, do not update the status
            _viewModel.IsReadyForLIMS = false;
            _entity.LIMSStatus = sentSuccessfully; // Setting this since it shouldn't be updated
            _vmTester.MapToEntity();
            Assert.AreSame(sentSuccessfully, _entity.LIMSStatus);

            // Same test but with sendFailed
            _viewModel.IsReadyForLIMS = false;
            _entity.LIMSStatus = sendFailed; // Setting this since it shouldn't be updated
            _vmTester.MapToEntity();
            Assert.AreSame(sendFailed, _entity.LIMSStatus);
        }

        #endregion

        #region Validation

        [TestMethod]
        public void TestValidateForCl2Total()
        {
            var pwsid = GetFactory<PublicWaterSupplyFactory>().Create();
            var sampleSite = GetFactory<SampleSiteFactory>().Create();

            Action<decimal?, int?> isValid = (chlorine, sampleSiteId) =>
            {
                _viewModel.Cl2Total = chlorine;
                _viewModel.SampleSite = sampleSiteId;
                ValidationAssert.ModelStateIsValid(_viewModel, x => x.Cl2Total);
            };

            Action<decimal?, int?> isInvalid = (chlorine, sampleSiteId) =>
            {
                _viewModel.Cl2Total = chlorine;
                _viewModel.SampleSite = sampleSiteId;
                ValidationAssert.ModelStateHasError(_viewModel, x => x.Cl2Total, "Cl2Total is required for the selected Sample Site.");
            };

            // Must always return true if SampleSite is null
            isValid(null, null);
            isValid(0m, null);

            // Must always return true if PWSID is null
            sampleSite.PublicWaterSupply = null;
            isValid(null, sampleSite.Id);
            isValid(0m, sampleSite.Id);

            // Must always return true if PWSID is not null and TotalChlorineReported is false
            pwsid.TotalChlorineReported = false;
            sampleSite.PublicWaterSupply = pwsid;
            isValid(null, sampleSite.Id);
            isValid(0m, sampleSite.Id);

            // Must return false if PWSID.TotalChlorineReported is true and the chlorine value is null
            pwsid.TotalChlorineReported = true;
            isInvalid(null, sampleSite.Id);
            isValid(0m, sampleSite.Id);
        }

        [TestMethod]
        public void TestValidateForCl2Free()
        {
            var pwsid = GetFactory<PublicWaterSupplyFactory>().Create();
            var sampleSite = GetFactory<SampleSiteFactory>().Create();

            Action<decimal?, int?> isValid = (chlorine, sampleSiteId) =>
            {
                _viewModel.Cl2Free = chlorine;
                _viewModel.SampleSite = sampleSiteId;
                ValidationAssert.ModelStateIsValid(_viewModel, x => x.Cl2Free);
            };

            Action<decimal?, int?> isInvalid = (chlorine, sampleSiteId) =>
            {
                _viewModel.Cl2Free = chlorine;
                _viewModel.SampleSite = sampleSiteId;
                ValidationAssert.ModelStateHasError(_viewModel, x => x.Cl2Free, "Cl2Free is required for the selected Sample Site.");
            };

            // Must always return true if SampleSite is null
            isValid(null, null);
            isValid(0m, null);

            // Must always return true if PWSID is null
            sampleSite.PublicWaterSupply = null;
            isValid(null, sampleSite.Id);
            isValid(0m, sampleSite.Id);

            // Must always return true if PWSID is not null and FreeChlorineReported is false
            pwsid.FreeChlorineReported = false;
            sampleSite.PublicWaterSupply = pwsid;
            isValid(null, sampleSite.Id);
            isValid(0m, sampleSite.Id);

            // Must return false if PWSID.FreeChlorineReported is true and the chlorine value is null
            pwsid.FreeChlorineReported = true;
            isInvalid(null, sampleSite.Id);
            isValid(0m, sampleSite.Id);
        }

        #endregion

        #endregion
    }
}