using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;
using System;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models
{
    [TestClass]
    public class CreateValveTest : MapCallMvcInMemoryDatabaseTestBase<Valve>
    {
        #region Fields

        private ViewModelTester<CreateValve, Valve> _vmTester;
        private CreateValve _viewModel;
        private Valve _entity;
        private AssetStatus _activeStatus;
        private AssetStatus _retiredStatus;
        private AssetStatus _removedStatus;
        private AssetStatus _cancelledStatus;
        private User _user;
        private Mock<IAuthenticationService<User>> _authServ;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private OperatingCenter _opc;
        private Town _town;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
            e.For<ITownRepository>().Use<TownRepository>();
            e.For<ITownSectionRepository>().Use<TownSectionRepository>();
            e.For<IStreetRepository>().Use<StreetRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<IAssetStatusRepository>().Use<AssetStatusRepository>();
            e.For<IValveRepository>().Use<ValveRepository>();
            e.For<IAbbreviationTypeRepository>().Use<AbbreviationTypeRepository>();
            e.For<IAuthenticationRepository<User>>().Use<AuthenticationRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            _town = GetEntityFactory<Town>().Create();
            _town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = _town, OperatingCenter = _opc, Abbreviation = "ZZ" });
            _entity = GetEntityFactory<Valve>().Create();
            _viewModel = _viewModelFactory.BuildWithOverrides<CreateValve, Valve>( _entity, new {
                WorkOrderNumber = "work order number"
            });
            _vmTester = new ViewModelTester<CreateValve, Valve>(_viewModel, _entity);

            _entity.Town = _town;
            _entity.OperatingCenter = _opc;
            _viewModel.Town = _town.Id;
            _viewModel.OperatingCenter = _opc.Id;

            _activeStatus = GetFactory<ActiveAssetStatusFactory>().Create();
            _retiredStatus = GetFactory<RetiredAssetStatusFactory>().Create();
            _removedStatus = GetFactory<RemovedAssetStatusFactory>().Create();
            _cancelledStatus = GetFactory<CancelledAssetStatusFactory>().Create();

            GetFactory<TownSectionAbbreviationTypeFactory>().Create();
            GetFactory<FireDistrictAbbreviationTypeFactory>().Create();
        }

        #endregion

        [TestMethod]
        public void TestOperatingCenterCanMapBothWays()
        {
            Session.Evict(_entity);
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create(new {Description = "Bah"});
            var town = GetFactory<TownFactory>().Create(new { ShortName = "EDISON" });
            town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = operatingCenter, Abbreviation = "EDI" });

            _entity.OperatingCenter = operatingCenter;
            _entity.Town = town;

            _vmTester.MapToViewModel();

            Assert.AreEqual(operatingCenter.Id, _viewModel.OperatingCenter);
            _entity.OperatingCenter = null;
            _vmTester.MapToEntity();

            Assert.AreSame(operatingCenter, _entity.OperatingCenter);
        }

        [TestMethod]
        public void TestTownCanMapBothWays()
        {
            Session.Evict(_entity);
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { OperatingCenterCode = "EW1" });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc1, Abbreviation = "EDI" });

            _entity.OperatingCenter = opc1;
            _entity.Town = town;
            _viewModel.Town = null;

            _vmTester.MapToViewModel();

            Assert.AreEqual(town.Id, _viewModel.Town);

            _entity.Town = null;
            _vmTester.MapToEntity();
            Assert.AreSame(town, _entity.Town);
        }

        [TestMethod]
        public void TestStreetCanMapBothWays()
        {
            var street = GetEntityFactory<Street>().Create(new { Description = "Foo" });
            _entity.Street = street;

            _vmTester.MapToViewModel();

            Assert.AreEqual(street.Id, _viewModel.Street);

            _entity.Street = null;
            _vmTester.MapToEntity();

            Assert.AreSame(street, _entity.Street);
        }

        [TestMethod]
        public void TestCrossStreetCanMapBothWays()
        {
            var crossStreet = GetEntityFactory<Street>().Create(new { Description = "Foo" });
            _entity.CrossStreet = crossStreet;

            _vmTester.MapToViewModel();

            Assert.AreEqual(crossStreet.Id, _viewModel.CrossStreet);

            _entity.CrossStreet = null;
            _vmTester.MapToEntity();

            Assert.AreSame(crossStreet, _entity.CrossStreet);
        }

        [TestMethod]
        public void TestMapToEntitySetsValveNumberAndValveSuffixToNextGeneratedNumberFromRepository()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { OperatingCenterCode = "EW1"});
            var town = GetFactory<TownFactory>().Create(new { ShortName = "EDISON" });
            town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc1, Abbreviation = "EDI"});
            var townSection = GetFactory<TownSectionFactory>().Create();

            _viewModel.OperatingCenter = opc1.Id;
            _viewModel.Town = town.Id;
            _viewModel.TownSection = townSection.Id;

            _vmTester.MapToEntity();

            Assert.AreEqual("VEDI-1", _entity.ValveNumber);
        }

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.DateInstalled);
            _vmTester.CanMapBothWays(x => x.Critical);
            _vmTester.CanMapBothWays(x => x.CriticalNotes);
            _vmTester.CanMapBothWays(x => x.WorkOrderNumber);
            _vmTester.CanMapBothWays(x => x.ControlsCrossing);
        }
        
        [TestMethod]
        public void TestStringLengths()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.MapPage, Valve.StringLengths.MAP_PAGE);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.SketchNumber, Valve.StringLengths.SKETCH_NUMBER, error: "The field Sketch # must be a string with a maximum length of 15.");
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.StreetNumber, Valve.StringLengths.STREET_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.ValveLocation, Valve.StringLengths.VALVE_LOCATION);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.WorkOrderNumber, Valve.StringLengths.WORK_ORDER_NUMBER, error: "The field WorkOrderNumber must be a string with a maximum length of 25.");
        }

        [TestMethod]
        public void TestRequiredFields()
        {
           ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.DateInstalled, DateTime.Now,
                x => x.Status, 1);
           ValidationAssert.PropertyIsRequired(_viewModel, x => x.Status);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Street);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Town);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.WorkOrderNumber);
        }

        [TestMethod]
        public void TestCriticalNotesRequiredIfMarkedCritical()
        {
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x=> x.CriticalNotes, "TestNotes", x => x.Critical, true, false);
        }

        [TestMethod]
        public void TestCriticalNotesMustBeNullIfCriticalFalse()
        {
            _viewModel.Critical = false;
            _viewModel.CriticalNotes = "blah blah";
            ValidationAssert.ModelStateHasError(_viewModel, x => x.CriticalNotes, ValveViewModel.ErrorMessages.ERROR_CRITICAL_NOTES_MUST_BE_NULL);

            _viewModel.CriticalNotes = null;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.CriticalNotes);
        }
        
        [TestMethod]
        public void TestMapToEntitySetsInitiatorToCurrentUser()
        {
            _entity.Initiator = null;

            _vmTester.MapToEntity();

            Assert.AreEqual(_user, _entity.Initiator);
        }

        [TestMethod]
        public void TestOperatingCenterIsRequired()
        {
            _viewModel.OperatingCenter = null;
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);

            var opc = GetEntityFactory<OperatingCenter>().Create();
            _viewModel.OperatingCenter = opc.Id;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.OperatingCenter);
        }

        [TestMethod]
        public void TestTownIsRequired()
        {
            _viewModel.Town = null;
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Town);

            var town = GetEntityFactory<Town>().Create();
            _viewModel.Town = town.Id;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.Town);
        }

        [TestMethod]
        public void TestValveSuffixIsRequiredWhenIsFoundValve()
        {
            _viewModel.IsFoundValve = true;
            _viewModel.ValveSuffix = null;

            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ValveSuffix, CreateValve.ErrorMessages.SUFFIX_REQUIRED);
        }

        [TestMethod]
        public void TestValveSuffixForFoundValveMustNotExistAndMustBeLowerThanTheCurrentMaxValveSuffix()
        {
            _user.IsAdmin = true;
            var opc = GetEntityFactory<OperatingCenter>().Create();
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc, Abbreviation = "XX"});
            
            var valve1 = GetEntityFactory<Valve>().Create(new {OperatingCenter = opc, Town = town, ValveSuffix = 1, ValveNumber = "VXX-1"});
            var valve3 = GetEntityFactory<Valve>().Create(new { OperatingCenter = opc, Town = town, ValveSuffix = 3, ValveNumber = "VXX-3" });

            _viewModel.IsFoundValve = true;
            _viewModel.ValveSuffix = 4;
            _viewModel.Town = town.Id;
            _viewModel.OperatingCenter = opc.Id;

            ValidationAssert.ModelStateHasError(_viewModel, x => x.ValveSuffix, CreateValve.ErrorMessages.SUFFIX_TOO_LARGE);

            _viewModel.ValveSuffix = 3;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.ValveSuffix, CreateValve.ErrorMessages.SUFFIX_ALREADY_EXISTS);

            _viewModel.ValveSuffix = 2;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.ValveSuffix);
        }

        [TestMethod]
        public void TestValveSuffixMustBeGreaterThanOrEqualToOne()
        {
            _viewModel.ValveSuffix = 1;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.ValveSuffix);

            _viewModel.ValveSuffix = 0;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.ValveSuffix, CreateValve.ErrorMessages.SUFFIX_GREATER_THAN_ZERO);
        }

        [TestMethod]
        public void TestRequiresNotificationReturnsTrueIfRequiresNotification()
        {
            _viewModel.Status = _activeStatus.Id;
            Assert.IsTrue(_viewModel.RequiresNotification());

            _viewModel.Status = _retiredStatus.Id;
            Assert.IsTrue(_viewModel.RequiresNotification());

            _viewModel.Status = GetFactory<PendingAssetStatusFactory>().Create().Id;
            Assert.IsFalse(_viewModel.RequiresNotification());
        }

        [TestMethod]
        public void TestSAPEquipmentIdValidation()
        {
            // This is required only when the selected operating center's SAPEnabled IsContractedOperations == false.
            var opcSAPEnabledContractedOps = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsContractedOperations = true, SAPEnabled = true });
            var opcSapEnabled = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsContractedOperations = false, SAPEnabled = true });
            var valveType = GetFactory<ValveTypeFactory>().Create();
            var normalPosition = GetFactory<ValveNormalPositionFactory>().Create();

            _viewModel.ValveType = valveType.Id;
            _viewModel.NormalPosition = normalPosition.Id;
            _viewModel.Turns = Convert.ToDecimal(0.25);
            _viewModel.FunctionalLocation = null;
            _viewModel.DateInstalled = DateTime.Now;
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
        public void TestMapToEntitySetsValveInspectionFrequencyAndUnitWhenNullForOperatingCentersThatHaveUsesValveInspectionFrequencySetToTrue()
        {
            _opc.UsesValveInspectionFrequency = true;
            _opc.SmallValveInspectionFrequency = 42123;

          //  _viewModel.IsFoundValve = true;
            _viewModel.OperatingCenter = _opc.Id;
            _viewModel.InspectionFrequency = null;
            _viewModel.InspectionFrequencyUnit = null;
          //  _viewModel.Town = _opc.Towns.First().Id;

            _vmTester.MapToEntity();

            Assert.AreEqual(42123, _entity.InspectionFrequency);
            Assert.AreEqual(4, _entity.InspectionFrequencyUnit.Id);
        }

        [TestMethod]
        public void TestNumberDuplicationTruthTable()
        {
            _viewModel.IsFoundValve = true;

            this.TestNumberDuplicationTruthTable(_entity, _viewModel, "ValveSuffix",
                CreateValve.ErrorMessages.SUFFIX_ALREADY_EXISTS, x => new {
                    Status = x,
                    _entity.OperatingCenter,
                    _entity.ValveSuffix,
                    ValveNumber = $"VZZ-{_entity.ValveSuffix}"
                });
        }
    }
}