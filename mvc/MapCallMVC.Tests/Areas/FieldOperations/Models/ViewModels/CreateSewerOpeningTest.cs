using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;
using System;
using System.Linq;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels
{
    [TestClass]
    public class CreateSewerOpeningTest : MapCallMvcInMemoryDatabaseTestBase<SewerOpening>
    {
        #region Fields

        private ViewModelTester<CreateSewerOpening, SewerOpening> _vmTester;
        private CreateSewerOpening _viewModel;
        private SewerOpening _entity;
        private Mock<IDateTimeProvider> _dateTimeProvider;
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
            e.For<IAbbreviationTypeRepository>().Use<AbbreviationTypeRepository>();
            e.For<ISewerOpeningRepository>().Use<SewerOpeningRepository>();
            e.For<IAssetStatusRepository>().Use<AssetStatusRepository>();
            e.For<IRepository<AssetStatus>>().Use(ctx => ctx.GetInstance<IAssetStatusRepository>());
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<ITownRepository>().Use<TownRepository>();
            e.For<IStreetRepository>().Use<StreetRepository>();
            e.For<IFunctionalLocationRepository>().Use<FunctionalLocationRepository>();
            e.For<IFacilityRepository>().Use<FacilityRepository>();
            e.For<IAuthenticationService<User>>().Use((_authServ = new Mock<IAuthenticationService<User>>()).Object);
            e.For<IDateTimeProvider>().Use((_dateTimeProvider = new Mock<IDateTimeProvider>()).Object);
            e.For<ISensorRepository>().Use<SensorRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _viewModel = _viewModelFactory.BuildWithOverrides<CreateSewerOpening>(new {TaskNumber = "task number"});
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create(new { SAPEnabled = true, IsContractedOperations = false });
            _entity = GetFactory<SewerOpeningFactory>().Create(new { OperatingCenter = operatingCenter });
            _vmTester = new ViewModelTester<CreateSewerOpening, SewerOpening>(_viewModel, _entity);

            operatingCenter.Towns.Add(_entity.Town);
            var opcTown = new OperatingCenterTown {OperatingCenter = operatingCenter, Town = _entity.Town, Abbreviation = "MEH"};
            operatingCenter.OperatingCenterTowns.Add(opcTown);
            _entity.Town.OperatingCentersTowns.Add(opcTown);
            _activeStatus = GetFactory<ActiveAssetStatusFactory>().Create();
            _retiredStatus = GetFactory<RetiredAssetStatusFactory>().Create();
            _cancelledStatus = GetFactory<CancelledAssetStatusFactory>().Create();
            _removedStatus = GetFactory<RemovedAssetStatusFactory>().Create();

            _entity.Status = _activeStatus;
            _viewModel.Status = _activeStatus.Id;
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.TaskNumber);
            _vmTester.CanMapBothWays(x => x.DateInstalled);
            _vmTester.CanMapBothWays(x => x.MapPage);
            _vmTester.CanMapBothWays(x => x.OldNumber);
            _vmTester.CanMapBothWays(x => x.DistanceFromCrossStreet);
            _vmTester.CanMapBothWays(x => x.IsEpoxyCoated);
            _vmTester.CanMapBothWays(x => x.Route);
            _vmTester.CanMapBothWays(x => x.Stop);
            _vmTester.CanMapBothWays(x => x.IsDoghouseOpening);
            _vmTester.CanMapBothWays(x => x.GeoEFunctionalLocation);
            _vmTester.CanMapBothWays(x => x.TaskNumber);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Town);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Coordinate);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Street);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Status);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.TaskNumber);
        }

        [TestMethod]
        public void TestDateRetiredIsRequiredWhenAssetStatusIsRetired()
        {
            _viewModel.OperatingCenter = GetEntityFactory<OperatingCenter>().Create().Id;

            foreach (var assetStatusId in AssetStatus.RETIRED_STATUS_IDS)
            {
                ValidationAssert.PropertyIsRequiredWhen(
                    _viewModel, 
                    x => x.DateRetired, 
                    DateTime.Now, 
                    x => x.Status, 
                    assetStatusId, 
                    AssetStatus.Indices.ACTIVE, 
                    "DateRetired is required for retired / removed sewer openings.");
            }
        }

        #region Can Map Both Ways

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
        public void TestTownCanMapBothWays()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opc1, Town = town, Abbreviation = "SF" });
            _entity.Town = town;
            _entity.OperatingCenter = opc1;

            _vmTester.MapToViewModel();

            Assert.AreEqual(town.Id, _viewModel.Town);

            _entity.Town = null;
            _vmTester.MapToEntity();

            Assert.AreSame(town, _entity.Town);
        }

        [TestMethod]
        public void TestStreetCanMapBothWays()
        {
            var street = GetEntityFactory<Street>().Create();
            _entity.Street = street;

            _vmTester.MapToViewModel();

            Assert.AreEqual(street.Id, _viewModel.Street);

            _entity.Street = null;
            _vmTester.MapToEntity();

            Assert.AreSame(street, _entity.Street);
        }

        [TestMethod]
        public void TestIntersectingStreetCanMapBothWays()
        {
            var street = GetEntityFactory<Street>().Create(new { Description = "Foo" });
            _entity.IntersectingStreet = street;

            _vmTester.MapToViewModel();

            Assert.AreEqual(street.Id, _viewModel.IntersectingStreet);

            _entity.IntersectingStreet = null;
            _vmTester.MapToEntity();

            Assert.AreSame(street, _entity.IntersectingStreet);
        }

        [TestMethod]
        public void TestAssetStatusCanMapBothWays()
        {
            var assetStatus = GetFactory<CancelledAssetStatusFactory>().Create();
            _entity.Status = assetStatus;

            _vmTester.MapToViewModel();

            Assert.AreEqual(assetStatus.Id, _viewModel.Status);

            _entity.Status = null;
            _vmTester.MapToEntity();

            Assert.AreSame(assetStatus, _entity.Status);
        }

        [TestMethod]
        public void TestSewerOpeningMaterialCanMapBothWays()
        {
            var smm = GetEntityFactory<SewerOpeningMaterial>().Create(new { Description = "Foo" });
            _entity.SewerOpeningMaterial = smm;

            _vmTester.MapToViewModel();

            Assert.AreEqual(smm.Id, _viewModel.SewerOpeningMaterial);

            _entity.SewerOpeningMaterial = null;
            _vmTester.MapToEntity();

            Assert.AreSame(smm, _entity.SewerOpeningMaterial);
        }

        [TestMethod]
        public void TestTownSectionCanMapBothWays()
        {
            var townSection = GetEntityFactory<TownSection>().Create();
            _entity.TownSection = townSection;

            _vmTester.MapToViewModel();

            Assert.AreEqual(townSection.Id, _viewModel.TownSection);

            _entity.TownSection = null;
            _vmTester.MapToEntity();

            Assert.AreSame(townSection, _entity.TownSection);
        }

        [TestMethod]
        public void TestFunctionalLocationCanMapBothWays()
        {
            var fl = GetEntityFactory<FunctionalLocation>().Create();
            _entity.FunctionalLocation = fl;

            _vmTester.MapToViewModel();

            Assert.AreEqual(fl.Id, _viewModel.FunctionalLocation);

            _entity.FunctionalLocation = null;
            _vmTester.MapToEntity();

            Assert.AreSame(fl, _entity.FunctionalLocation);
        }

        [TestMethod]
        public void TestRetiredDateCanMapBothWaysWhenAssetStatusIsNotRetired()
        {
            var expectedDate = DateTime.Now;

            foreach (var assetStatusId in AssetStatus.ALL_STATUS_IDS.Except(AssetStatus.RETIRED_STATUS_IDS))
            {
                _entity.DateRetired = expectedDate;

                _vmTester.MapToViewModel();

                Assert.AreEqual(expectedDate, _viewModel.DateRetired);

                _viewModel.Status = assetStatusId;

                _vmTester.MapToEntity();

                Assert.IsNull(_entity.DateRetired, $"Assertion failed for status id: {assetStatusId}. Expected null, got: {_entity.DateRetired}");
            }
        }

        [TestMethod]
        public void TestRetiredDateCanMapBothWaysWhenAssetStatusIsRetired()
        {
            var expectedDate = DateTime.Now;

            foreach (var assetStatusId in AssetStatus.RETIRED_STATUS_IDS)
            {
                _entity.DateRetired = expectedDate;

                _vmTester.MapToViewModel();

                Assert.AreEqual(expectedDate, _viewModel.DateRetired);

                _viewModel.Status = assetStatusId;

                _vmTester.MapToEntity();

                Assert.AreEqual(expectedDate, _entity.DateRetired, $"Assertion failed for status id: {assetStatusId}. Expected {expectedDate}, got: {_entity.DateRetired}");
            }
        }

        #endregion

        [TestMethod]
        public void TestMapToEntitySetsOpeningNumberAndOpeningSuffixToNextGeneratedNumberFromRepository()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opc1, Town = town, Abbreviation = "SF" });
            _viewModel.OperatingCenter = opc1.Id;
            _viewModel.Town = town.Id;

            _vmTester.MapToEntity();

            Assert.AreEqual("MSF-1", _entity.OpeningNumber);
        }

        [TestMethod]
        public void TestExceptionIsThrownIfAnotherTownInTheOperatingCenterHasTheSameOpeningNumber()
        {
            _user.IsAdmin = true;
            var opc = GetFactory<OperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opc, Town = town, Abbreviation = "ZZ" });
            var otherTown = GetEntityFactory<Town>().Create();
            otherTown.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opc, Town = otherTown, Abbreviation = "ZZ" });

            var sm1 = GetEntityFactory<SewerOpening>().Create(new { OperatingCenter = opc, Town = town, OpeningNumber = "MZZ-1", OpeningSuffix = 1 });

            _viewModel.OperatingCenter = opc.Id;
            _viewModel.Town = otherTown.Id;

            MyAssert.Throws<InvalidOperationException>(() => _vmTester.MapToEntity());
        }

        [TestMethod]
        public void TestFunctionalLocationValidation()
        {
            // This is required only when the selected operating center's SAPEnabled IsContractedOperations == false.
            var opcSAPEnabledContractedOps = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsContractedOperations = true, SAPEnabled = true });
            var opcSapEnabled = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsContractedOperations = false, SAPEnabled = true });

            _viewModel.Town = GetFactory<TownFactory>().Create().Id;
            _viewModel.Street = GetFactory<StreetFactory>().Create().Id;
            _viewModel.Coordinate = GetFactory<CoordinateFactory>().Create().Id;
            _viewModel.SewerOpeningType = GetEntityFactory<SewerOpeningType>().Create().Id;
            _viewModel.OperatingCenter = opcSAPEnabledContractedOps.Id;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.FunctionalLocation);

            _viewModel.OperatingCenter = opcSapEnabled.Id;
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.FunctionalLocation, "The Functional Location field is required.", true);

            var functionalLocation = GetFactory<FunctionalLocationFactory>().Create();
            _viewModel.FunctionalLocation = functionalLocation.Id;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.FunctionalLocation);
        }

        [TestMethod]
        public void TestMapToEntitySetsSendToSAPFalseWhenOperatingCenterSAPEnabledFalse()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = false });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc1, Abbreviation = "ZZ" });
            _entity.Town = town;
            _entity.OperatingCenter = opc1;
            _viewModel.Town = town.Id;
            _viewModel.OperatingCenter = opc1.Id;

            _vmTester.MapToEntity();

            Assert.IsFalse(_viewModel.SendToSAP);
        }

        [TestMethod]
        public void TestMapToEntitySetsSendToSAPTrueWhenOperatingCenterSAPEnabledTrue()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc1, Abbreviation = "ZZ" });
            _entity.Town = town;
            _entity.OperatingCenter = opc1;
            _viewModel.Town = town.Id;
            _viewModel.OperatingCenter = opc1.Id;

            _vmTester.MapToEntity();

            Assert.IsTrue(_viewModel.SendToSAP);
        }

        [TestMethod]
        public void TestMapToEntitySetsSendToSAPFalseWhenOperatingCenterSAPEnabledTrueAndContractedOps()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true, IsContractedOperations = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc1, Abbreviation = "ZZ" });
            _entity.Town = town;
            _entity.OperatingCenter = opc1;
            _viewModel.Town = town.Id;
            _viewModel.OperatingCenter = opc1.Id;

            _vmTester.MapToEntity();

            Assert.IsFalse(_viewModel.SendToSAP);
        }


        [TestMethod]
        public void TestMapToEntitySetsSendToSAPToFalseIfTheExistingStatusIsCancelledAndTheStatusItNotBeingChanged()
        {
            // This same test should work for Cancelled, Retired, and Removed.
            var statuses = new[] { _cancelledStatus, _retiredStatus, _removedStatus };

            foreach (var status in statuses)
            {
                var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
                var town = GetEntityFactory<Town>().Create();
                town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc1, Abbreviation = "ZZ" });
                _entity.Town = town;
                _entity.OperatingCenter = opc1;
                _entity.Status = status;
                _viewModel.Town = town.Id;
                _viewModel.OperatingCenter = opc1.Id;
                _viewModel.Status = status.Id;

                _vmTester.MapToEntity();

                Assert.IsFalse(_viewModel.SendToSAP);
            }
        }

        [TestMethod]
        public void TestMapToEntitySetsSendToSAPToTrueIfTheExistingStatusWasCancelledAndTheStatusIsChanged()
        {
            // This same test should work for Cancelled, Retired, and Removed.
            var statuses = new[] { _cancelledStatus, _retiredStatus, _removedStatus };

            foreach (var status in statuses)
            {
                var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
                var town = GetEntityFactory<Town>().Create();
                town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc1, Abbreviation = "ZZ" });
                _entity.Town = town;
                _entity.OperatingCenter = opc1;
                _entity.Status = status;
                _viewModel.Town = town.Id;
                _viewModel.OperatingCenter = opc1.Id;
                _viewModel.Status = _activeStatus.Id;

                _vmTester.MapToEntity();

                Assert.IsTrue(_viewModel.SendToSAP);
            }
        }

        [TestMethod]
        public void TestRequiresNotificationReturnsTrueWhenAssetStatusIsActiveOrRetired()
        {
            _viewModel.Status = _activeStatus.Id;
            Assert.IsTrue(_viewModel.RequiresNotification());

            _viewModel.Status = _retiredStatus.Id;
            Assert.IsTrue(_viewModel.RequiresNotification());

            _viewModel.Status = _cancelledStatus.Id;
            Assert.IsFalse(_viewModel.RequiresNotification());

        }

        [TestMethod]
        public void TestMapToEntityOnlySetsInspectionFrequencyFromOperatingCenter()
        {
            var freq = GetFactory<YearlyRecurringFrequencyUnitFactory>().Create();
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create(new {
                SewerOpeningInspectionFrequency = 3,
                SewerOpeningInspectionFrequencyUnit = freq
            });

            // both InspectionFrequency values should be overwritten even if they're set.
            // They do not auto map.
            _viewModel.InspectionFrequency = 123;
            _viewModel.InspectionFrequencyUnit = 323;
            _viewModel.OperatingCenter = opc.Id;

            _vmTester.MapToEntity();
            Assert.AreEqual(3, _entity.InspectionFrequency);
            Assert.AreSame(freq, _entity.InspectionFrequencyUnit);
        }

        [TestMethod]
        public void TestCriticalNotesIsRequiredIfCriticalIsTrue()
        {
            var oc = GetEntityFactory<OperatingCenter>().Create();
            _viewModel.OperatingCenter = oc.Id;
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.CriticalNotes, "stuff", x => x.Critical, true, false);
        }

        [TestMethod]
        public void TestCriticalNotesMustBeNullIfCriticalIsFalse()
        {
            var opcWithContractedOps = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsContractedOperations = true });

            _viewModel.OperatingCenter = opcWithContractedOps.Id;
            _viewModel.Critical = false;
            _viewModel.CriticalNotes = "blah";
            ValidationAssert.ModelStateHasError(_viewModel, x => x.CriticalNotes, "Critical checkbox must be checked when setting critical notes.");

            _viewModel.CriticalNotes = null;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.CriticalNotes);
        }

        [TestMethod]
        public void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.CriticalNotes, SewerOpening.StringLengths.CRITICAL_NOTES);
        }

        #endregion
    }
}
