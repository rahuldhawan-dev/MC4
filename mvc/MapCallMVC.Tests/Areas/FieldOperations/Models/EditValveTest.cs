using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Utilities;
using Moq;
using StructureMap;
using System;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models
{
    [TestClass]
    public class EditValveTest : MapCallMvcInMemoryDatabaseTestBase<Valve>
    {
        #region Fields

        private ViewModelTester<EditValve, Valve> _vmTester;
        private EditValve _viewModel;
        private Valve _entity;
        private AssetStatus _activeStatus, _retiredStatus, _otherStatus, _cancelledStatus, _removedStatus;
        private User _admin;
        private Mock<IAuthenticationService<User>> _authServ;
        private Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IAuthenticationService<User>>().Use((_authServ = new Mock<IAuthenticationService<User>>()).Object);
            e.For<IDateTimeProvider>().Use((_dateTimeProvider = new Mock<IDateTimeProvider>()).Object);
            e.For<ITownRepository>().Use<TownRepository>();
            e.For<IStreetRepository>().Use<StreetRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<IAssetStatusRepository>().Use<AssetStatusRepository>();
            e.For<IValveRepository>().Use<ValveRepository>();
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _admin = GetFactory<AdminUserFactory>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_admin);

            _entity = GetEntityFactory<Valve>().Create();
            _viewModel = _viewModelFactory.Build<EditValve, Valve>( _entity);
            _vmTester = new ViewModelTester<EditValve, Valve>(_viewModel, _entity);

            _activeStatus = GetFactory<ActiveAssetStatusFactory>().Create();
            _retiredStatus = GetFactory<RetiredAssetStatusFactory>().Create();
            _otherStatus = GetFactory<PendingAssetStatusFactory>().Create();
            _removedStatus = GetFactory<RemovedAssetStatusFactory>().Create();
            _cancelledStatus = GetFactory<CancelledAssetStatusFactory>().Create();
            Session.Flush();
        }
        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.DateInstalled);
            _vmTester.CanMapBothWays(x => x.Critical);
            _vmTester.CanMapBothWays(x => x.CriticalNotes);
        }

        #endregion

        #region Validation

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.DateInstalled, DateTime.Now,
                x => x.Status, 1);

            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ValveNumber);
        }

        [TestMethod]
        public void TestStringLengths()
        {
            _viewModel.ValveNumber = "VAB-100XXXXXXXX";
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.ValveNumber, Valve.StringLengths.VALVE_NUMBER, true);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.WorkOrderNumber, Valve.StringLengths.WORK_ORDER_NUMBER, false, "The field WorkOrderNumber must be a string with a maximum length of 25.");
        }

        [TestMethod]
        public void TestFunctionalLocationValidation()
        {
            // This is required only when the selected operating center's IsContractedOperations == false. and SAPEnabled
            var opcWithContractedOps = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsContractedOperations = true, SAPEnabled = false });
            var opcWithoutContractedOps = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsContractedOperations = false, SAPEnabled = true });
            var valveType = GetFactory<ValveTypeFactory>().Create();
            var normalPosition = GetFactory<ValveNormalPositionFactory>().Create();

            _viewModel.ValveType = valveType.Id;
            _viewModel.NormalPosition = normalPosition.Id;
            _viewModel.Turns = Convert.ToDecimal(0.25);
            _viewModel.DateInstalled = DateTime.Now;
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
        public void TestCanChangeToAnUnusedValveNumber()
        {
            var valve = GetEntityFactory<Valve>().Create(new {Status = _activeStatus , DateInstalled = DateTime.Now});
            var target = _viewModelFactory.BuildWithOverrides<EditValve, Valve>(valve, new {ValveNumber = "VAB-108", ValveSuffix = 108});
            target.ValveType = GetFactory<ValveTypeFactory>().Create().Id;
            target.NormalPosition = GetFactory<ValveNormalPositionFactory>().Create().Id;
            target.Turns = Convert.ToDecimal(0.25);

            _vmTester = new ViewModelTester<EditValve, Valve>(target, valve);

            _vmTester.MapToEntity();

            ValidationAssert.ModelStateIsValid(target);
        }

        [TestMethod]
        public void TestValveNumberHasNotChangedAndDoesNotThrowValidationError()
        {
            GetFactory<PendingAssetStatusFactory>().Create();
            var valve = GetEntityFactory<Valve>().Create(new { Status = _activeStatus, DateInstalled = DateTime.Now, ValveNumber = "HAB-101", ValveSuffix = 101  });
            var target = _viewModelFactory.BuildWithOverrides<EditValve, Valve>(valve, new {Status = AssetStatus.Indices.PENDING});
            _vmTester = new ViewModelTester<EditValve, Valve>(target, valve);

            _vmTester.MapToEntity();

            ValidationAssert.ModelStateIsValid(target);
        }

        [TestMethod]
        public void TestValveNumberCannotBeChangedToAnotherActiveValveNumberInTheOperatingCenter()
        {
            GetFactory<PendingAssetStatusFactory>().Create();
            var valve1 = GetEntityFactory<Valve>().Create(new { Status = _activeStatus });
            var valve2 = GetEntityFactory<Valve>().Create(new { Status = _activeStatus });
            var target = _viewModelFactory.BuildWithOverrides<EditValve, Valve>(valve1, new { ValveNumber = valve2.ValveNumber, ValveSuffix = valve2.ValveSuffix});
            _vmTester = new ViewModelTester<EditValve, Valve>(target, valve1);

            _vmTester.MapToEntity();

            ValidationAssert.ModelStateHasError(target, x => x.ValveNumber, EditValve.ERROR_VALVE_NUMBER_ALREADY_USED);
        }

        [TestMethod]
        public void TestValveNumberCanBeChangedToAValveNumberForWhenANonActiveValveHasSaidValveNumber()
        {
            var valve1 = GetEntityFactory<Valve>().Create(new { Status = _activeStatus,DateInstalled = DateTime.Now, ValveNumber = "VAB-101", ValveSuffix = 101 });
            var valve2 = GetEntityFactory<Valve>().Create(new { Status = _retiredStatus, ValveNumber = "VAB-102", ValveSuffix = 102 });
            var target = _viewModelFactory.BuildWithOverrides<EditValve, Valve>(valve1, new { ValveNumber = valve2.ValveNumber, ValveSuffix = valve2.ValveSuffix });
            target.ValveType = GetFactory<ValveTypeFactory>().Create().Id;
            target.NormalPosition = GetFactory<ValveNormalPositionFactory>().Create().Id;
            target.Turns = Convert.ToDecimal(0.25);
            _vmTester = new ViewModelTester<EditValve, Valve>(target, valve1);

            _vmTester.MapToEntity();

            ValidationAssert.ModelStateIsValid(target);

        }

        #endregion

        [TestMethod]
        public void TestMapToEntitySetsSendNotificationsOnSaveToTrueIfChangedFromActive()
        {
            var valve = GetEntityFactory<Valve>().Create(new { Status = _activeStatus});
            var model = _viewModelFactory.BuildWithOverrides<EditValve, Valve>(valve, new {Status = _otherStatus.Id });
            _vmTester = new ViewModelTester<EditValve, Valve>(model, valve);
            
            _vmTester.MapToEntity();

            Assert.IsTrue(model.SendNotificationsOnSave);
        }

        [TestMethod]
        public void TestMapToEntitySetsSendNotificationsOnSaveToTrueIfChangedToActive()
        {
            var valve = GetEntityFactory<Valve>().Create(new { Status = _otherStatus });
            var model = _viewModelFactory.BuildWithOverrides<EditValve, Valve>(valve, new { Status = _activeStatus.Id });
            _vmTester = new ViewModelTester<EditValve, Valve>(model, valve);

            _vmTester.MapToEntity();

            Assert.IsTrue(model.SendNotificationsOnSave);
        }

        [TestMethod]
        public void TestMapToEntitySetsSendNotificationsOnSaveToFalseIfNotChangedActive()
        {
            var valve = GetEntityFactory<Valve>().Create(new { Status = _activeStatus });
            var model = _viewModelFactory.BuildWithOverrides<EditValve, Valve>(valve, new { Status = _activeStatus.Id });
            _vmTester = new ViewModelTester<EditValve, Valve>(model, valve);

            _vmTester.MapToEntity();

            Assert.IsFalse(model.SendNotificationsOnSave);
        }

        [TestMethod]
        public void TestMapToEntitySetsSendNotificationWhenStatusIsChangedToAStatusThatIsNotLimitedToUserAdmin()
        {
            var adminStatus = GetFactory<ActiveAssetStatusFactory>().Create(new { IsUserAdminOnly = true });
            var nonAdminStatus = GetFactory<PendingAssetStatusFactory>().Create(new { IsUserAdminOnly = false });
            var valve = GetEntityFactory<Valve>().Create(new { Status = adminStatus });
            var model = _viewModelFactory.BuildWithOverrides<EditValve, Valve>(valve, new { Status = nonAdminStatus.Id });
            _vmTester = new ViewModelTester<EditValve, Valve>(model, valve);

            _vmTester.MapToEntity();

            Assert.IsTrue(model.SendNotificationsOnSave);
        }

        [TestMethod]
        public void TestValidationFailsIfHydrantSuffixIsNotWithinHydrantNumber()
        {
            GetFactory<PendingAssetStatusFactory>().Create();
            var val = GetFactory<ValveFactory>().Create(new { ValveNumber = "VAB-15A", ValveSuffix = 15 });
            var target = _viewModelFactory.BuildWithOverrides<EditValve, Valve>(val, new { ValveSuffix = 16 });

            ValidationAssert.ModelStateHasError(target, x => x.ValveNumber, Valve.VALVE_NUMBER_PATTERN_ERROR);

            target.ValveNumber = "VAB-155A";
            target.ValveSuffix = 15;
            ValidationAssert.ModelStateHasError(target, x => x.ValveNumber, Valve.VALVE_NUMBER_PATTERN_ERROR);

            target.ValveNumber = "VAB-16A15";
            ValidationAssert.ModelStateHasError(target, x => x.ValveNumber, Valve.VALVE_NUMBER_PATTERN_ERROR);

            target.ValveNumber = "VAB-15A";
            target.ValveSuffix = 15;
            target.Status = AssetStatus.Indices.PENDING;
            ValidationAssert.ModelStateIsValid(target);

            target.ValveNumber = "VAB-15";
            ValidationAssert.ModelStateIsValid(target);
        }

        [TestMethod]
        public void TestMapToEntitySetsSendToSAPFalseWhenOperatingCenterSAPEnabledFalse()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = false });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc1, Abbreviation = "ZZ" });
            _entity.Town = town;
            _entity.OperatingCenter = opc1;

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
        public void TestMapToEntitySetsValveInspectionFrequencyAndUnitWhenNullForOperatingCentersThatHaveUsesValveInspectionFrequencyTrue()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create(new { UsesValveInspectionFrequency = true, SmallValveInspectionFrequency = 5 });
            _entity.OperatingCenter = opc;
            _viewModel.OperatingCenter = opc.Id;

            _vmTester.MapToEntity();

            Assert.AreEqual(5, _entity.InspectionFrequency);
            Assert.AreEqual(4, _entity.InspectionFrequencyUnit.Id);
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
    }
}