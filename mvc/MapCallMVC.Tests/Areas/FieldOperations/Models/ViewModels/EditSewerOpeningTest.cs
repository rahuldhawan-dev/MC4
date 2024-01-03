using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.Repositories.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Utilities;
using Moq;
using StructureMap;
using System;
using System.Linq;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels
{
    [TestClass]
    public class EditSewerOpeningTest : MapCallMvcInMemoryDatabaseTestBase<SewerOpening>
    {
        #region Fields

        private ViewModelTester<EditSewerOpening, SewerOpening> _vmTester;
        private EditSewerOpening _viewModel;
        private SewerOpening _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private AssetStatus _activeStatus;
        private AssetStatus _retiredStatus;
        private AssetStatus _cancelledStatus;
        private AssetStatus _removedStatus;
        private Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IUserRepository>().Use<UserRepository>();
            e.For<IAssetStatusRepository>().Use<AssetStatusRepository>();
            e.For<IRepository<AssetStatus>>().Use(ctx => ctx.GetInstance<IAssetStatusRepository>());
            e.For<ISewerOpeningRepository>().Use<SewerOpeningRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<ITownRepository>().Use<TownRepository>();
            e.For<IStreetRepository>().Use<StreetRepository>();
            e.For<IFunctionalLocationRepository>().Use<FunctionalLocationRepository>();
            e.For<IIconSetRepository>().Use<IconSetRepository>();
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

            _activeStatus = GetFactory<ActiveAssetStatusFactory>().Create();
            _retiredStatus = GetFactory<RetiredAssetStatusFactory>().Create();
            _cancelledStatus = GetFactory<CancelledAssetStatusFactory>().Create();
            _removedStatus = GetFactory<RemovedAssetStatusFactory>().Create();

            _entity = GetEntityFactory<SewerOpening>().Create(new {
                Status = _activeStatus
            });
            _viewModel = _viewModelFactory.Build<EditSewerOpening, SewerOpening>( _entity);
            _vmTester = new ViewModelTester<EditSewerOpening, SewerOpening>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.OpeningNumber);
            _vmTester.CanMapBothWays(x => x.TaskNumber);
            _vmTester.CanMapBothWays(x => x.DateInstalled);
            _vmTester.CanMapBothWays(x => x.MapPage);
            _vmTester.CanMapBothWays(x => x.OldNumber);
            _vmTester.CanMapBothWays(x => x.DistanceFromCrossStreet);
            _vmTester.CanMapBothWays(x => x.IsEpoxyCoated);
            _vmTester.CanMapBothWays(x => x.Route);
            _vmTester.CanMapBothWays(x => x.Stop);
            _vmTester.CanMapBothWays(x => x.OpeningSuffix);
            _vmTester.CanMapBothWays(x => x.IsDoghouseOpening);
            _vmTester.CanMapBothWays(x => x.GeoEFunctionalLocation);
        }

        #region Can Map Both Ways

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
            var assetStatus = GetFactory<ActiveAssetStatusFactory>().Create();
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
            var assetType = GetFactory<SewerMainAssetTypeFactory>().Create();
            var fl = GetEntityFactory<FunctionalLocation>().Create(new { AssetType = assetType});
            _entity.FunctionalLocation = fl;

            _vmTester.MapToViewModel();

            Assert.AreEqual(fl.Id, _viewModel.FunctionalLocation);

            _entity.FunctionalLocation = null;
            _vmTester.MapToEntity();

            Assert.AreSame(fl, _entity.FunctionalLocation);
        }
        #endregion

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Coordinate);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Status);
        }

        [TestMethod]
        public void TestDateRetiredIsRequiredWhenAssetStatusIsRetired()
        {
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

        [TestMethod]
        public void TestValidationFailsIfOpeningNumberIsNotUniqueToOperatingCenter()
        {
            _viewModel.Status = AssetStatus.Indices.ACTIVE;
            _user.IsAdmin = true;
            var sm1 = GetFactory<SewerOpeningFactory>().Create(new {
                OpeningNumber = "MZZ-1", OpeningSuffix = 1, OperatingCenter = _entity.OperatingCenter,
                Status = new AssetStatus {Id = _viewModel.Status.Value}
            });
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.OpeningNumber);

            _viewModel.OpeningNumber = sm1.OpeningNumber;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.OpeningNumber, $"The generated opening number '{sm1.OpeningNumber}' is not unique to the operating center '{_entity.OperatingCenter}'");
        }

        [TestMethod]
        public void TestValidationFailsIfMultipleRecordsInDatabaseHaveTheSameOpeningNumberForTheEditedHydrant()
        {
            _user.IsAdmin = true;
            var sms = GetFactory<SewerOpeningFactory>().CreateList(2, new { OpeningNumber = "MZZ-1", OpeningSuffix = 1, OperatingCenter = _entity.OperatingCenter });

            _viewModel.OpeningNumber = sms[0].OpeningNumber;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.OpeningNumber, $"The generated opening number '{sms.First().OpeningNumber}' is not unique to the operating center '{_entity.OperatingCenter}'");
        }

        [TestMethod]
        public void TestFunctionalLocationValidation()
        {
            _user.IsAdmin = true;
            // This is required only when the selected operating center's IsContractedOperations == false and SAPENabled
            var opcWithContractedOps = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsContractedOperations = true, SAPEnabled = true });
            var opcWithoutContractedOps = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsContractedOperations = false, SAPEnabled = true });

            _viewModel.FunctionalLocation = null;
            _entity.OperatingCenter = opcWithContractedOps;
            //_viewModel.OperatingCenter = opcWithContractedOps.Id;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.FunctionalLocation);

            _entity.OperatingCenter = opcWithoutContractedOps;
            //_viewModel.OperatingCenter = opcWithoutContractedOps.Id;
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.FunctionalLocation, "The Functional Location field is required.", true);

            var functionalLocation = GetFactory<FunctionalLocationFactory>().Create();
            _viewModel.FunctionalLocation = functionalLocation.Id;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.FunctionalLocation);
        }

        [TestMethod]
        public void TestCriticalNotesIsRequiredIfCriticalIsTrue()
        {

            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.CriticalNotes, "Stuff", x => x.Critical, true, false);
        }

        [TestMethod]
        public void TestCriticalNotesMustBeNullIfCriticalIsFalse()
        {
            _viewModel.Critical = false;
            _viewModel.CriticalNotes = "blah";
            ValidationAssert.ModelStateHasError(_viewModel, x => x.CriticalNotes, "Critical checkbox must be checked when setting critical notes.");

            _viewModel.CriticalNotes = null;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.CriticalNotes);
        }

        [TestMethod]
        public void TestMapToEntitySetsSendToSAPFalseWhenOperatingCenterSAPEnabledFalse()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = false });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc1, Abbreviation = "ZZ" });
            _entity.Town = town;
            _entity.OperatingCenter = opc1;
            _viewModel.Status = _activeStatus.Id;

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
            _viewModel.Status = _activeStatus.Id;

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
            _viewModel.Status = _activeStatus.Id;

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
                _viewModel.Status = _activeStatus.Id;

                _vmTester.MapToEntity();

                Assert.IsTrue(_viewModel.SendToSAP);
            }
        }

        [TestMethod]
        public void TestMapToEntitySetsNotificationToTrueWhenStatusChangedFromActive()
        {
            var entity = GetEntityFactory<SewerOpening>().Create(new { Status = _activeStatus });
            var model = _viewModelFactory.BuildWithOverrides<EditSewerOpening, SewerOpening>(entity, new { Status = _cancelledStatus.Id });
            _vmTester = new ViewModelTester<EditSewerOpening, SewerOpening>(model, entity);

            _vmTester.MapToEntity();

            Assert.IsTrue(model.SendNotificationOnSave);
        }

        [TestMethod]
        public void TestMapToEntitySetsNotificationToTrueWhenStatusChangedToActive()
        {
            var entity = GetEntityFactory<SewerOpening>().Create(new {Status = _cancelledStatus});
            var model = _viewModelFactory.BuildWithOverrides<EditSewerOpening, SewerOpening>(entity, new { Status = _activeStatus.Id});
            _vmTester = new ViewModelTester<EditSewerOpening, SewerOpening>(model, entity);

            _vmTester.MapToEntity();

            Assert.IsTrue(model.SendNotificationOnSave);
        }

        [TestMethod]
        public void TestMapToEntityTestSetNotificationToFalseWhenStatusDoesNotChange()
        {
            var entity = GetEntityFactory<SewerOpening>().Create(new { Status = _activeStatus });
            var model = _viewModelFactory.BuildWithOverrides<EditSewerOpening, SewerOpening>(entity, new { Status = _activeStatus.Id });
            _vmTester = new ViewModelTester<EditSewerOpening, SewerOpening>(model, entity);

            _vmTester.MapToEntity();

            Assert.IsFalse(model.SendNotificationOnSave);
        }

        [TestMethod]
        public void TestMapToEntitySetNotificationToTrueWhenStatusIsChangedToStatusNotLimitedByAdmin()
        {
            var adminStatus = GetFactory<ActiveAssetStatusFactory>().Create(new { IsUserAdminOnly = true});
            var nonAdminStatus = GetFactory<PendingAssetStatusFactory>().Create(new { IsUserAdminOnly = false });

            var entity = GetEntityFactory<SewerOpening>().Create(new {Status = adminStatus});
            var model = _viewModelFactory.BuildWithOverrides<EditSewerOpening, SewerOpening>(entity,new { Status = nonAdminStatus.Id});
            _vmTester = new ViewModelTester<EditSewerOpening, SewerOpening>(model, entity);

            _vmTester.MapToEntity();

            Assert.IsTrue(model.SendNotificationOnSave);


        }

        [TestMethod]
        public void TestMapToEntityCancelsAnyAttachedWorkOrders()
        {
            var now = DateTime.Now;
            var reason = GetEntityFactory<WorkOrderCancellationReason>()
               .Create(new {Status = "ARET", Description = "Asset Retired"});
            _entity.WorkOrders = GetFactory<WorkOrderFactory>().CreateList(2);
            _viewModel.DateRetired = now;

            _vmTester.MapToEntity();

            foreach (var workOrder in _entity.WorkOrders)
            {
                Assert.AreEqual(now, workOrder.CancelledAt);
                Assert.AreEqual(reason, workOrder.WorkOrderCancellationReason);
            }
        }


        [TestMethod]
        public void TestMapToEntityDoesNotUseOperatingCenterInspectionFrequencyWhenUserDoesNotSetValuesForInspectionFrequency()
        {
            var monthFreq = GetFactory<MonthlyRecurringFrequencyUnitFactory>().Create();

            // If the InspectionFrequency property has a value, then we're copying
            // those to the entity. 
            _viewModel.InspectionFrequency = 123;
            _viewModel.InspectionFrequencyUnit = monthFreq.Id;

            _vmTester.MapToEntity();
            Assert.AreEqual(123, _entity.InspectionFrequency);
            Assert.AreSame(monthFreq, _entity.InspectionFrequencyUnit);

            // If the InspectionFrequency property does not have a value, then we're
            // copying the values from the operating center instead.
            _viewModel.InspectionFrequency = null;
            _viewModel.InspectionFrequencyUnit = null;
            _vmTester.MapToEntity();
            Assert.IsNull(_entity.InspectionFrequency);
            Assert.IsNull(_entity.InspectionFrequencyUnit);
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
    }
}
