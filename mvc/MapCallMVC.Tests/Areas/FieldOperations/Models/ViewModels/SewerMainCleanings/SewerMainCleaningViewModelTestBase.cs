using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.SewerMainCleanings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels.SewerMainCleanings
{
    public abstract class SewerMainCleaningViewModelTestBase<TViewModel>
        : ViewModelTestBase<SewerMainCleaning, TViewModel>
        where TViewModel : SewerMainCleaningViewModel
    {
        #region Private Members

        protected Mock<IDateTimeProvider> _dateTimeProvider;
        protected Mock<IAuthenticationService<User>> _authenticationService;
        protected User _user;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
            _authenticationService = e.For<IAuthenticationService<User>>().Mock();
            e.For<ISewerMainCleaningRepository>().Use<SewerMainCleaningRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<ITownRepository>().Use<TownRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<AdminUserFactory>().Create();
            _authenticationService.Setup(x => x.CurrentUser).Returns(_user);
        }

        #endregion

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert
               .PropertyIsRequired(x => x.Date)
               .PropertyIsRequired(x => x.FootageOfMainInspected)
               .PropertyIsRequired(x => x.InspectedDate)
               .PropertyIsRequired(x => x.InspectionType)
               .PropertyIsRequired(x => x.Opening1)
               .PropertyIsRequired(x => x.OperatingCenter)
               .PropertyIsRequired(x => x.Overflow);

            ValidationAssert
               .PropertyIsRequiredWhen(
                    x => x.Opening2,
                    GetEntityFactory<SewerOpening>().Create().Id,
                    x => x.OpeningTwoIsATerminus,
                    false,
                    true);
        }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester
               .CanMapBothWays(x => x.BlockageFound)
               .CanMapBothWays(x => x.CleaningSchedule)
               .CanMapBothWays(x => x.CrossStreet)
               .CanMapBothWays(x => x.CrossStreet2)
               .CanMapBothWays(x => x.Date)
               .CanMapBothWays(x => x.FootageOfMainInspected)
               .CanMapBothWays(x => x.GallonsOfWaterUsed)
               .CanMapBothWays(x => x.HydrantUsed)
               .CanMapBothWays(x => x.InspectedDate)
               .CanMapBothWays(
                    x => x.InspectionType,
                    GetFactory<AcousticSewerMainInspectionTypeFactory>().Create())
               .CanMapBothWays(
                    x => x.InspectionGrade,
                    GetFactory<ExcellentSewerMainInspectionGradeFactory>().Create())
               .CanMapBothWays(x => x.Opening1)
               .CanMapBothWays(x => x.Opening1Condition)
               .CanMapBothWays(x => x.Opening1FrameAndCover)
               .CanMapBothWays(x => x.Opening2IsATerminus)
               .CanMapBothWays(x => x.Opening2)
               .CanMapBothWays(x => x.Opening2Condition)
               .CanMapBothWays(x => x.Opening2FrameAndCover)
               .CanMapBothWays(x => x.OperatingCenter)
               .CanMapBothWays(x => x.Overflow)
               .CanMapBothWays(x => x.SewerOverflow)
               .CanMapBothWays(x => x.Street)
               .CanMapBothWays(x => x.TableNotes)
               .CanMapBothWays(x => x.Town);

            // can only be set when BlockageFound is true
            _viewModel.BlockageFound = _entity.BlockageFound = true;
            _vmTester
               .CanMapBothWays(x => x.CauseOfBlockage);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            // noop
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert
               .EntityMustExist(
                    x => x.InspectionType,
                    GetFactory<AcousticSewerMainInspectionTypeFactory>().Create())
               .EntityMustExist(
                    x => x.InspectionGrade,
                    GetFactory<ExcellentSewerMainInspectionGradeFactory>().Create())
               .EntityMustExist<CauseOfBlockage>(x => x.CauseOfBlockage)
               .EntityMustExist<CleaningSchedule>()
               .EntityMustExist<Hydrant>(x => x.HydrantUsed)
               .EntityMustExist<OpeningCondition>(x => x.Opening1Condition)
               .EntityMustExist<OpeningCondition>(x => x.Opening2Condition)
               .EntityMustExist<OpeningFrameAndCover>(x => x.Opening1FrameAndCover)
               .EntityMustExist<OpeningFrameAndCover>(x => x.Opening2FrameAndCover)
               .EntityMustExist<OperatingCenter>()
               .EntityMustExist<SewerOpening>(x => x.Opening1)
               .EntityMustExist<SewerOpening>(x => x.Opening2)
               .EntityMustExist<SewerOverflow>()
               .EntityMustExist<Street>()
               .EntityMustExist<Street>(x => x.CrossStreet)
               .EntityMustExist<Street>(x => x.CrossStreet2)
               .EntityMustExist<Town>();
        }

        [TestMethod]
        public void TestMapToEntitySetsSendToSAPFalseWhenOperatingCenterSAPEnabledFalse()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = false });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown {
                Town = town,
                OperatingCenter = opc1,
                Abbreviation = "ZZ"
            });
            var opening = GetEntityFactory<SewerOpening>().Create(new { Town = town, OperatingCenter = opc1 });
            _viewModel.Opening1 = opening.Id;

            _vmTester.MapToEntity();

            Assert.IsFalse(_viewModel.SendToSAP);
        }

        [TestMethod]
        public void TestMapToEntitySetsSendToSAPTrueWhenOperatingCenterSAPEnabledTrue()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown {
                Town = town,
                OperatingCenter = opc1,
                Abbreviation = "ZZ"
            });
            var opening = GetEntityFactory<SewerOpening>().Create(new { Town = town, OperatingCenter = opc1 });
            _viewModel.Opening2 = opening.Id;

            _vmTester.MapToEntity();

            Assert.IsTrue(_viewModel.SendToSAP);
        }

        [TestMethod]
        public void TestMapToEntitySetsSendToSAPFalseWhenOperatingCenterSAPEnabledTrueAndContractedOps()
        {
            var opc1 =
                GetFactory<UniqueOperatingCenterFactory>()
                   .Create(new { SAPEnabled = true, IsContractedOperations = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown {
                Town = town,
                OperatingCenter = opc1,
                Abbreviation = "ZZ"
            });
            var opening = GetEntityFactory<SewerOpening>().Create(new { Town = town, OperatingCenter = opc1 });
            _viewModel.Opening1 = opening.Id;

            _vmTester.MapToEntity();

            Assert.IsFalse(_viewModel.SendToSAP);
        }

        [TestMethod]
        public void TestMapToEntityClearsMainInspectionGradeWhenInspectionTypeIsMainCleaningOrSmokeTest()
        {
            GetFactory<SewerMainInspectionTypeFactoryBase>().CreateAll();
            GetFactory<SewerMainInspectionGradeFactoryBase>().CreateAll();

            foreach (var type in new[] {
                         SewerMainInspectionType.Indices.MAIN_CLEANING_PM,
                         SewerMainInspectionType.Indices.SMOKE_TEST
                     })
            {
                _viewModel.InspectionGrade = SewerMainInspectionGrade.Indices.EXCELLENT;
                _viewModel.InspectionType = type;

                var result = _viewModel.MapToEntity(new SewerMainCleaning());
                
                Assert.IsNull(result.InspectionGrade);
            }
        }

        [TestMethod]
        public void TestMapToEntityDoesNotClearMainInspectionGradeWhenInspectionTypeIsAcocusticOrCCTV()
        {
            GetFactory<SewerMainInspectionTypeFactoryBase>().CreateAll();
            GetFactory<SewerMainInspectionGradeFactoryBase>().CreateAll();

            foreach (var type in new[] {
                         SewerMainInspectionType.Indices.ACOUSTIC,
                         SewerMainInspectionType.Indices.CCTV
                     })
            {
                _viewModel.InspectionGrade = SewerMainInspectionGrade.Indices.EXCELLENT;
                _viewModel.InspectionType = type;

                var result = _viewModel.MapToEntity(new SewerMainCleaning());
                
                Assert.AreEqual(SewerMainInspectionGrade.Indices.EXCELLENT, result.InspectionGrade.Id);
            }
        }

        [TestMethod]
        public void TestMapToEntityClearsCauseOfBlockageWhenBlockageFoundIsFalse()
        {
            var cause = GetEntityFactory<CauseOfBlockage>().Create();

            _viewModel.CauseOfBlockage = cause.Id;
            _viewModel.BlockageFound = false;

            var result = _viewModel.MapToEntity(new SewerMainCleaning());
            
            Assert.IsNull(result.CauseOfBlockage);
        }

        [TestMethod]
        public void TestMapToEntityDoesNotClearCauseOfBlockageWhenBlockageFoundIsTrue()
        {
            var cause = GetEntityFactory<CauseOfBlockage>().Create();

            _viewModel.CauseOfBlockage = cause.Id;
            _viewModel.BlockageFound = true;

            var result = _viewModel.MapToEntity(new SewerMainCleaning());
            
            Assert.AreEqual(cause, result.CauseOfBlockage);
        }
        
        [TestMethod]
        public void TestMapToEntitySetsNeedsToSyncToTrue()
        {
            _vmTester.MapToEntity();

            Assert.IsTrue(_entity.NeedsToSync);
        }
    }
}
