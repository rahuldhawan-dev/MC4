using _271ObjectTests.Tests.Library;
using MapCall.Common.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data.Linq;
using MMSINC.Exceptions;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.Linq;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Rhino.Mocks;
using System;
using System.Linq;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for WorkOrderTestTest
    /// </summary>
    [TestClass]
    public class WorkOrderTest : EventFiringTestClass
    {
        #region Private Members

        private TestWorkOrder _target;
        private IRepository<WorkOrder> _repository;
        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void WorkOrderTestInitialize()
        {
            _target = new TestWorkOrderBuilder();
            _repository = new MockWorkOrderRepository();
        }

        #endregion
        
        #region SAPEditable Fields

        [TestMethod]
        public void TestPlantMaintenanceActivityTypeOverrideIsEditableCases()
        {
            _target.OperatingCenter = new OperatingCenter { SAPEnabled = false };
            Assert.IsTrue(_target.PlantMaintenanceActivityTypeOverrideEditable);

            _target.OperatingCenter = new OperatingCenter { SAPEnabled = true, SAPWorkOrdersEnabled = false };
            Assert.IsTrue(_target.PlantMaintenanceActivityTypeOverrideEditable);

            _target.OperatingCenter = new OperatingCenter {SAPEnabled = true, SAPWorkOrdersEnabled = true,IsContractedOperations = true };
            Assert.IsTrue(_target.PlantMaintenanceActivityTypeOverrideEditable);

            _target.OperatingCenter = new OperatingCenter {SAPEnabled = true,SAPWorkOrdersEnabled = true,IsContractedOperations = false};
            Assert.IsTrue(_target.PlantMaintenanceActivityTypeOverrideEditable);
        }

        [TestMethod]
        public void TestPlantMaintenanceActivityTypeOverrideIsNotEditableWhenItHasOverride()
        {
            _target.OperatingCenter = new OperatingCenter { SAPEnabled = true, SAPWorkOrdersEnabled = true, IsContractedOperations = false };
            _target.PlantMaintenanceActivityTypeOverride = new PlantMaintenanceActivityType();

            Assert.IsFalse(_target.PlantMaintenanceActivityTypeOverrideEditable);
        }
        
        [TestMethod]
        public void TestPlantMaintenanceActivityTypeOverrideIsNotEditableWhenItIsApproved()
        {
            _target.OperatingCenter = new OperatingCenter { SAPEnabled = true, SAPWorkOrdersEnabled = true, IsContractedOperations = false };
            _target.ApprovedOn = DateTime.Now;

            Assert.IsFalse(_target.PlantMaintenanceActivityTypeOverrideEditable);
        }


        [TestMethod]
        public void TestAssetTypeIsEditableCases()
        {
            _target.OperatingCenter = new OperatingCenter { SAPEnabled = false };
            Assert.IsTrue(_target.AssetTypeEditable);

            _target.OperatingCenter = new OperatingCenter { SAPEnabled = true, SAPWorkOrdersEnabled = false };
            Assert.IsTrue(_target.AssetTypeEditable);

            _target.OperatingCenter = new OperatingCenter { SAPEnabled = true, SAPWorkOrdersEnabled = true, IsContractedOperations = true };
            Assert.IsTrue(_target.AssetTypeEditable);

            _target.OperatingCenter = new OperatingCenter { SAPEnabled = true, SAPWorkOrdersEnabled = true, IsContractedOperations = false };
            Assert.IsTrue(_target.AssetTypeEditable);
        }

        [TestMethod]
        public void TestAssetTypeIsNotEditableWhenItHasPMATOverride()
        {
            _target.OperatingCenter = new OperatingCenter { SAPEnabled = true, SAPWorkOrdersEnabled = true, IsContractedOperations = false };
            _target.PlantMaintenanceActivityTypeOverride = new PlantMaintenanceActivityType();

            Assert.IsFalse(_target.AssetTypeEditable);
        }

        [TestMethod]
        public void TestAssetTypeIsNotEditableWhenItIsApproved()
        {
            _target.OperatingCenter = new OperatingCenter { SAPEnabled = true, SAPWorkOrdersEnabled = true, IsContractedOperations = false };
            _target.ApprovedOn = DateTime.Now;

            Assert.IsFalse(_target.AssetTypeEditable);
        }


        [TestMethod]
        public void TestAssetIsEditableCases()
        {
            _target.OperatingCenter = new OperatingCenter { SAPEnabled = false };
            Assert.IsTrue(_target.AssetEditable);

            _target.OperatingCenter = new OperatingCenter { SAPEnabled = true, SAPWorkOrdersEnabled = false };
            Assert.IsTrue(_target.AssetEditable);

            _target.OperatingCenter = new OperatingCenter { SAPEnabled = true, SAPWorkOrdersEnabled = true, IsContractedOperations = true };
            Assert.IsTrue(_target.AssetEditable);

            _target.OperatingCenter = new OperatingCenter { SAPEnabled = true, SAPWorkOrdersEnabled = true, IsContractedOperations = false };
            Assert.IsTrue(_target.AssetEditable);
        }

        [TestMethod]
        public void TestAssetIsNotEditableWhenItHasOverride()
        {
            _target.OperatingCenter = new OperatingCenter { SAPEnabled = true, SAPWorkOrdersEnabled = true, IsContractedOperations = false };
            _target.PlantMaintenanceActivityTypeOverride = new PlantMaintenanceActivityType();

            Assert.IsFalse(_target.AssetEditable);
        }

        [TestMethod]
        public void TestAssetIsNotEditableWhenItIsApproved()
        {
            _target.OperatingCenter = new OperatingCenter { SAPEnabled = true, SAPWorkOrdersEnabled = true, IsContractedOperations = false };
            _target.ApprovedOn = DateTime.Now;

            Assert.IsFalse(_target.AssetEditable);
        }


        #endregion

        [TestMethod]
        public void TestCreateNewWorkOrder()
        {
            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSetPremiseNumberToAStringThatsTooShort()
        {
            var premiseNumber = "1234567";

            MyAssert.Throws(() => _target.PremiseNumber = premiseNumber,
                typeof(DomainLogicException));
        }

        [TestMethod]
        public void TestCannotSaveWithAFakePremiseNumberWithoutEnteringSomeNotes()
        {
            for (var i = 0; i <= 9; ++i)
            {
                var premiseNumber =
                    String.Format("{0}{0}{0}{0}{0}{0}{0}{0}{0}", i);

                _target = new TestWorkOrderBuilder()
                    .BuildCompleteOrder()
                    .WithPremiseNumber(premiseNumber)
                    .WithNotes(null);

                MyAssert.Throws<DomainLogicException>(
                    () => _repository.InsertNewEntity(_target));

                _target = new TestWorkOrderBuilder()
                    .BuildCompleteOrder()
                    .WithPremiseNumber(premiseNumber)
                    .WithNotes("");

                MyAssert.Throws<DomainLogicException>(
                    () => _repository.InsertNewEntity(_target));

                _target = new TestWorkOrderBuilder()
                    .BuildCompleteOrder()
                    .WithPremiseNumber(premiseNumber)
                    .WithNotes("these are enough notes");

                MyAssert.DoesNotThrow(
                    () => _repository.InsertNewEntity(_target));
            }
        }

        [TestMethod]
        public void TestMarkoutRequiredPropertyReturnsFalseWhenMarkoutRequirementSetToNone()
        {
            _target = new TestWorkOrderBuilder()
                .WithMarkoutRequirement(TestMarkoutRequirementBuilder.None);

            Assert.IsFalse(_target.MarkoutRequired);
        }

        [TestMethod]
        public void TestMarkoutRequirementPropertyReturnsTrueWhenMarkoutRequirementNotSetToNone()
        {
            _target = new TestWorkOrderBuilder()
                .WithMarkoutRequirement(TestMarkoutRequirementBuilder.Emergency);

            Assert.IsTrue(_target.MarkoutRequired);

            _target = new TestWorkOrderBuilder()
                .WithMarkoutRequirement(TestMarkoutRequirementBuilder.Routine);

            Assert.IsTrue(_target.MarkoutRequired);
        }

        [TestMethod]
        public void TestAssetPropertyReflectsValveIfAssetTypeIsValve()
        {
            _target = new TestWorkOrderBuilder()
                .WithAssetType(new TestAssetTypeBuilder<Valve>())
                .WithValve(new Valve());

            Assert.AreSame(_target.Valve, _target.Asset.Valve);
        }

        [TestMethod]
        public void TestAssetPropertyReflectsHydrantIfAssetTypeIsHydrant()
        {
            _target = new TestWorkOrderBuilder()
                .WithAssetType(new TestAssetTypeBuilder<Hydrant>())
                .WithHydrant(new Hydrant());

            Assert.AreSame(_target.Hydrant, _target.Asset.Hydrant);
        }

        [TestMethod]
        public void TestAssetPropertyReflectsSewerOpeningIfAssetTypeIsSewerOpening()
        {
            _target = new TestWorkOrderBuilder()
                .WithAssetType(new TestAssetTypeBuilder<SewerOpening>())
                .WithSewerOpening(new SewerOpening());

            Assert.AreSame(_target.SewerOpening, _target.Asset.SewerOpening);
        }

        [TestMethod]
        public void TestAssetPropertyReflectsStormCatchIfAssetTypeIsStormCatch()
        {
            _target = new TestWorkOrderBuilder()
                .WithAssetType(new TestAssetTypeBuilder<StormCatch>())
                .WithStormCatch(new StormCatch());

        }

        [TestMethod]
        public void TestAssetPropertyReflectsEquipmentIfAssetTypeIsEquipment()
        {
            _target = new TestWorkOrderBuilder()
                .WithAssetType(new TestAssetTypeBuilder<Equipment>())
                .WithEquipment(new Equipment());

            Assert.AreEqual(_target.Equipment, _target.Asset.Equipment);
        }

        [TestMethod]
        public void TestAssetPropertyReturnsNullIfNoAssetSet()
        {
            // This is only useful prior to saving.  the order should not be able to save
            // without an asset defined
            _target = new TestWorkOrderBuilder()
                .WithValve(null)
                .WithHydrant(null)
                .WithSewerOpening(null)
                .WithStormCatch(null)
                .WithEquipment(null);

            Assert.IsNull(_target.Asset);
        }

        [TestMethod]
        public void TestDaysSinceCompletionReturnsNumberOfDaysBetweenDateCompletedAndNow()
        {
            _target.DateCompleted = null;

            Assert.IsNull(_target.DaysSinceCompletion);

            _target.DateCompleted = DateTime.Now.AddDays(-5);

            Assert.AreEqual(5, _target.DaysSinceCompletion);
        }

        [TestMethod]
        public void TestLatitudePropertyReflectsAssetLatitudeValue()
        {
            var latitude = 1.1;
            var valve = new Valve { Coordinate = new Coordinate { Latitude = latitude } };
            _target = new TestWorkOrderBuilder()
                .WithAssetType(new TestAssetTypeBuilder<Valve>())
                .WithValve(valve);

            Assert.AreEqual(latitude, _target.Latitude);
        }

        [TestMethod]
        public void TestLongitudePropertyReflectsAssetLongitudeValue()
        {
            var longitude = 1.1;
            var hydrant = new Hydrant { Coordinate = new Coordinate { Longitude = longitude }};
            _target = new TestWorkOrderBuilder()
                .WithAssetType(new TestAssetTypeBuilder<Hydrant>())
                .WithHydrant(hydrant);

            Assert.AreEqual(longitude, _target.Longitude);
        }

        #region Phase

        [TestMethod]
        public void TestWorkOrderIsInPlanningPhaseWhenInputRequirementsAreMet()
        {
            _target = new TestWorkOrderBuilder().BuildForPlanning();

            Assert.AreEqual(WorkOrderPhase.Planning, _target.Phase);
        }

        [TestMethod]
        public void TestWorkOrderIsInSchedulingPhaseWhenPlanningRequirementsAreMet()
        {
            _target = new TestWorkOrderBuilder().BuildForScheduling();

            Assert.AreEqual(WorkOrderPhase.Scheduling, _target.Phase);
        }

        [TestMethod]
        public void TestWorkOrderIsInFinalizationPhaseWhenSchedulingRequirementsAreMet()
        {
            _target = new TestWorkOrderBuilder().BuildForFinalization();

            Assert.AreEqual(WorkOrderPhase.Finalization, _target.Phase);
        }

        [TestMethod]
        public void TestWorkOrderIsInInputIfNoSAPWorkOrderNumberAndSAPEnabled()
        {
            var operatingCenter = new OperatingCenter
            {
                SAPEnabled = true, SAPWorkOrdersEnabled = true
            };
            _target = new TestWorkOrderBuilder().BuildIncompleteOrder()
                .WithOperatingCenter(operatingCenter);

            Assert.AreEqual(WorkOrderPhase.Input, _target.Phase);
        }

        #endregion

        [TestMethod]
        public void TestStreetAddressReturnsStreetNumberAndNameSeparatedBySpace()
        {
            var streetNumber = "123";
            var streetName = "Maple";
            var suffix = "blvd";

            _target = new TestWorkOrderBuilder()
                     .WithStreet(new TestStreetBuilder().WithStreetName(streetName).WithSuffix(suffix))
                     .WithStreetNumber(streetNumber);
            
            var expected = streetNumber + " " + streetName+" "+ suffix;

            Assert.AreEqual(expected, _target.StreetAddress);
        }
        [TestMethod]
        public void TestStreetAddressReturnsStreetNumberAndNameSeparatedBySpaceNoSuffix()
        {
            var prefix = "";
            var streetNumber = "123";
            var streetName = "Maple";
            var suffix = "";

            _target = new TestWorkOrderBuilder()
                     .WithStreet(new TestStreetBuilder().WithStreetName(streetName).WithSuffix(suffix).WithPrefix(prefix))
                     .WithStreetNumber(streetNumber);

            var expected = streetNumber + " " + streetName ;

            Assert.AreEqual(expected, _target.StreetAddress);
        }
        [TestMethod]
        public void TestStreetAddressReturnsStreetNumberAndNameSeparatedBySpaceWithPrefix()
        {
            var streetNumber = "123";
            var streetName = "Maple";
            var suffix = "mnt pass";
            var prefix = "E";

            _target = new TestWorkOrderBuilder()
                     .WithStreet(new TestStreetBuilder().WithStreetName(streetName).WithSuffix(suffix).WithPrefix(prefix))
                     .WithStreetNumber(streetNumber);

            var expected = streetNumber + " " + prefix + " " + streetName + " " + suffix;

            Assert.AreEqual(expected, _target.StreetAddress);
        }

        [TestMethod]
        public void TestTownAddressPropertyReturnsTownStateAndZipCode()
        {
            var state = new State() { Abbreviation = "NJ" };
            var county = new County() {
                State = state
            };
            var zipCode = "07720";
            var townName = "Bradley Beach";
            _target = new TestWorkOrderBuilder()
                .WithTown(
                new TestTownBuilder().WithName(townName).WithState(state).WithCounty(county))
                .WithZipCode(zipCode);
            var expected = String.Format("{0}, {1} {2}", townName, state.Abbreviation,
                zipCode);

            Assert.AreEqual(expected, _target.TownAddress);
        }

        [TestMethod]
        public void TestCannotSaveWithoutRequestedByEmployeeSetWhenRequesterIsEmployee()
        {
            _target = new TestWorkOrderBuilder()
                .WithRequester(new WorkOrderRequester {
                    WorkOrderRequesterID =
                        WorkOrderRequesterRepository.Indices.EMPLOYEE
                })
                .WithRequestingEmployee(null);

            MyAssert.Throws<DomainLogicException>(() => _repository.InsertNewEntity(_target),
                "Attempting to save a WorkOrder whose requester is an employee when the RequestingEmployee has not been set should throw an exception.");

            _target.RequestingEmployee = new Employee();

            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutValueForLatitude()
        {
            var longitude = 1.1;
            _target = new TestWorkOrderBuilder()
                .WithValve(null);
            _target.Longitude = longitude;

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
            MyAssert.Throws<DomainLogicException>(
                () => _repository.UpdateCurrentEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutValueForLongitude()
        {
            var latitude = 1.1;
            _target = new TestWorkOrderBuilder()
                .WithValve(null);
            _target.Latitude = latitude;

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
            MyAssert.Throws<DomainLogicException>(
                () => _repository.UpdateCurrentEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutValueForRequestedBy()
        {
            _target = new TestWorkOrderBuilder()
                .WithRequester(null);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutValueForDrivenBy()
        {
            _target = new TestWorkOrderBuilder()
                .WithPurpose(null);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutValueForAssetType()
        {
            _target = new TestWorkOrderBuilder()
                .WithAssetType(null);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutValueForDescription()
        {
            _target = new TestWorkOrderBuilder()
                .WithWorkDescription((WorkDescription)null);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutValueForMarkoutRequirement()
        {
            _target = new TestWorkOrderBuilder()
                .WithMarkoutRequirement(null);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutValueForCreatedBy()
        {
            _target = new TestWorkOrderBuilder()
                .WithCreatedBy(null);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutValueForTown()
        {
            _target = new TestWorkOrderBuilder()
                .WithTown((Town)null);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutValueForPriority()
        {
            _target = new TestWorkOrderBuilder()
                .WithPriority(null);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestVerifiesAddressOnSave()
        {
            _target = new TestWorkOrderBuilder()
                .WithTown(AddressVerificationTest.ValidTown)
                .WithStreet(AddressVerificationTest.ValidStreet)
                .WithNearestCrossStreet(AddressVerificationTest.InvalidStreet);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutCustomerInformationWhenRequesterIsCustomer()
        {
            _target = new TestWorkOrderBuilder()
                .WithRequester(TestWorkOrderRequesterBuilder.Customer)
                .WithCustomerName(null)
                .WithStreetNumber(null);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));

            _target = new TestWorkOrderBuilder()
                .WithRequester(TestWorkOrderRequesterBuilder.Customer)
                .WithCustomerName("Some Guy")
                .WithStreetNumber("123");

            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWhenAssetTypeAndWorkDescriptionConflict()
        {
            // NOTE: the default from TestWorkOrderBuilder is an order that uses Valve
            _target = new TestWorkOrderBuilder()
                .WithAssetType(new TestAssetTypeBuilder<Hydrant>())
                .WithHydrant(new Hydrant());

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWhenAssetTypeAndAssetConflict()
        {
            // NOTE: the default from TestWorkOrderBuilder is an order that uses Valve
            _target = new TestWorkOrderBuilder()
                .WithValve(null)
                .WithHydrant(new Hydrant());

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithMarkoutToBeCalledDateAndNoRequiredMarkoutType()
        {
            _target.MarkoutToBeCalled = DateTime.Now;

            MyAssert.Throws<DomainLogicException>(
                () => _repository.UpdateCurrentEntity(_target));

            _target.MarkoutTypeNeeded = new MarkoutType {
                MarkoutTypeID = 1,
                Description = "foo"
            };

            MyAssert.DoesNotThrow(() => _repository.UpdateCurrentEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithRequiredMarkoutTypeAndNoMarkoutRequirementDate()
        {
            _target.MarkoutTypeNeeded = new MarkoutType {
                MarkoutTypeID = 1,
                Description = "foo"
            };

            MyAssert.Throws<DomainLogicException>(
                () => _repository.UpdateCurrentEntity(_target));

            _target.MarkoutToBeCalled = DateTime.Now;

            MyAssert.DoesNotThrow(() => _repository.UpdateCurrentEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWorkOrderWithMarkoutTypeNeededOfNotListedAndNoRequiredMarkoutNotes()
        {
            _target.MarkoutTypeNeeded = new MarkoutType {
                Description = "NOT LISTED"
            };
            _target.MarkoutToBeCalled = DateTime.Today;

            MyAssert.Throws<DomainLogicException>(() =>
                                                  _repository
                                                      .UpdateCurrentEntity(
                                                          _target));

            _target.RequiredMarkoutNote = "here's your note";

            MyAssert.DoesNotThrow(() => _repository.UpdateCurrentEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithAMainBreakWorkDescriptionAndNoMainBreakInformationInput()
        {
            var mainAssetType = new TestAssetTypeBuilder().WithTypeName(
                TestAssetTypeBuilder.Descriptions.MAIN);

            foreach (var workDescriptionID in WorkDescriptionRepository.MAIN_BREAKS)
            {
                _target = new TestWorkOrderBuilder()
                    .WithAssetType(mainAssetType)
                    .WithWorkDescription(new WorkDescription {
                        WorkDescriptionID = workDescriptionID,
                        AssetType = mainAssetType
                    });

                MyAssert.Throws<DomainLogicException>(
                    () => _repository.InsertNewEntity(_target),
                    "WorkOrder does not properly validate for Main Break information when necessary");
                MyAssert.Throws<DomainLogicException>(
                    () => _repository.UpdateCurrentEntity(_target),
                    "WorkOrder does not properly validate for Main Break information when necessary");

                // no repair time range value
                _target.CustomerImpactRange = new CustomerImpactRange();
                _target.SignificantTrafficImpact = true;
                _target.LostWater = 1;

                MyAssert.Throws<DomainLogicException>(
                    () => _repository.InsertNewEntity(_target),
                    "WorkOrder does not properly validate for RepairTimeRange");
                MyAssert.Throws<DomainLogicException>(
                    () => _repository.UpdateCurrentEntity(_target),
                    "WorkOrder does not properly validate for RepairTimeRange");

                // no customer impact range value
                _target.CustomerImpactRange = null;
                _target.RepairTimeRange = new RepairTimeRange();

                MyAssert.Throws<DomainLogicException>(
                    () => _repository.InsertNewEntity(_target),
                    "WorkOrder does not properly validate for CustomerImpactRange");
                MyAssert.Throws<DomainLogicException>(
                    () => _repository.UpdateCurrentEntity(_target),
                    "WorkOrder does not properly validate for CustomerImpactRange");

                // no significant traffic impact value
                _target.CustomerImpactRange = new CustomerImpactRange();
                _target.SignificantTrafficImpact = null;

                MyAssert.Throws<DomainLogicException>(
                    () => _repository.InsertNewEntity(_target),
                    "WorkOrder does not properly validate for SignificantTrafficImpact");
                MyAssert.Throws<DomainLogicException>(
                    () => _repository.UpdateCurrentEntity(_target),
                    "WorkOrder does not properly validate for SignificantTrafficImpact");

                // no lost water
                //_target.SetPropertyValueByName("Phase", WorkOrderPhase.Finalization);
                _target.SignificantTrafficImpact = true;
                //_target.LostWater = null;

                //MyAssert.Throws<DomainLogicException>(
                //    () => _repository.InsertNewEntity(_target));

                // should be valid
                //_target.LostWater = 1;

                MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
                MyAssert.DoesNotThrow(() => _repository.UpdateCurrentEntity(_target));
            }
        }

        [TestMethod]
        public void TestCannotSaveWhenOperatingCenterHasInvoicingWithoutEquipmentLaborValuesInFinalization  ()
        {
            var operatingCenter = new OperatingCenter {
                HasWorkOrderInvoicing = true
            };
            _target = new TestWorkOrderBuilder().BuildForFinalization()
                .WithDateCompleted(DateTime.Now)
                .WithOperatingCenter(operatingCenter);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.UpdateCurrentEntity(_target),
                "WorkOrder does not properly validate for when invoicing enabled and labor items are not included");
        }

        [TestMethod]
        public void TestCannotSaveWithAMainBreakWorkDescriptionAndNoMainBreakInformationFinalization()
        {
            var mainAssetType = new TestAssetTypeBuilder().WithTypeName(
                TestAssetTypeBuilder.Descriptions.MAIN);

            foreach (var workDescriptionID in WorkDescriptionRepository.MAIN_BREAKS)
            {
                _target = new TestWorkOrderBuilder().BuildForFinalization()
                    .WithDateCompleted(DateTime.Now)
                    .WithAssetType(mainAssetType)
                    .WithWorkDescription(new WorkDescription {
                        WorkDescriptionID = workDescriptionID,
                        AssetType = mainAssetType
                    });

                MyAssert.Throws<DomainLogicException>(
                    () => _repository.InsertNewEntity(_target),
                    "WorkOrder does not properly validate for Main Break information when necessary");
                MyAssert.Throws<DomainLogicException>(
                    () => _repository.UpdateCurrentEntity(_target),
                    "WorkOrder does not properly validate for Main Break information when necessary");

                // no repair time range value
                _target.CustomerImpactRange = new CustomerImpactRange();
                _target.SignificantTrafficImpact = true;
                _target.LostWater = 1;

                MyAssert.Throws<DomainLogicException>(
                    () => _repository.InsertNewEntity(_target),
                    "WorkOrder does not properly validate for RepairTimeRange");
                MyAssert.Throws<DomainLogicException>(
                    () => _repository.UpdateCurrentEntity(_target),
                    "WorkOrder does not properly validate for RepairTimeRange");

                // no customer impact range value
                _target.CustomerImpactRange = null;
                _target.RepairTimeRange = new RepairTimeRange();

                MyAssert.Throws<DomainLogicException>(
                    () => _repository.InsertNewEntity(_target),
                    "WorkOrder does not properly validate for CustomerImpactRange");
                MyAssert.Throws<DomainLogicException>(
                    () => _repository.UpdateCurrentEntity(_target),
                    "WorkOrder does not properly validate for CustomerImpactRange");

                // no significant traffic impact value
                _target.CustomerImpactRange = new CustomerImpactRange();
                _target.SignificantTrafficImpact = null;

                MyAssert.Throws<DomainLogicException>(
                    () => _repository.InsertNewEntity(_target),
                    "WorkOrder does not properly validate for SignificantTrafficImpact");
                MyAssert.Throws<DomainLogicException>(
                    () => _repository.UpdateCurrentEntity(_target),
                    "WorkOrder does not properly validate for SignificantTrafficImpact");

                // no lost water
                _target.SignificantTrafficImpact = true;
                _target.LostWater = null;

                MyAssert.Throws<DomainLogicException>(
                    () => _repository.InsertNewEntity(_target));

                _target.MainBreaks.Add(new MainBreak());
                // should be valid
                _target.LostWater = 1;

                MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
                MyAssert.DoesNotThrow(() => _repository.UpdateCurrentEntity(_target));
            }
        }

        [TestMethod]
        public void TestCannotSaveWithPMATOverrideWithoutWBSChargedUnlessPBC()
        {
            var pmat = new PlantMaintenanceActivityType();
            
            _target =
                new TestWorkOrderBuilder().BuildForFinalization()
                    .WithPMATOverride(pmat);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.UpdateCurrentEntity(_target), "WorkOrder must include a WBS Number if PMAT is being overridden.");

            pmat = new PlantMaintenanceActivityType() { Id = PlantMaintenanceActivityType.Indices.PBC };

            _target =
                new TestWorkOrderBuilder().BuildForFinalization()
                    .WithPMATOverride(pmat);

            MyAssert.DoesNotThrow<DomainLogicException>(
                () => _repository.UpdateCurrentEntity(_target), "WorkOrder must include a WBS Number if PMAT is being overridden.");
        }

        [TestMethod]
        public void TestMarkoutExpirationDatePropertyReturnsExpirationDateOfCurrentMarkoutIfNotNull()
        {
            var expectedDate = DateTime.Now;
            _target = new TestWorkOrderBuilder()
                .WithMarkouts(new[] {
                    new Markout {
                        ExpirationDate = expectedDate
                    }
                });

            Assert.AreEqual(expectedDate, _target.MarkoutExpirationDate);
        }

        [TestMethod]
        public void TestSetMarkoutExpirationDoesNotSetExpirationIfMarkoutNotRequired()
        {
            _target.MarkoutRequirement = new MarkoutRequirement { MarkoutRequirementID = (int)MarkoutRequirementEnum.None};

            _target.SetCurrentMarkoutExpiration();

            Assert.IsNull(_target.CurrentMarkout);
        }

        [TestMethod]
        public void TestSetMarkoutExpirationExtendsMarkout()
        {
            _target.OperatingCenter.MarkoutsEditable = false;
            _target.MarkoutRequirement = new MarkoutRequirement { MarkoutRequirementID = (int)MarkoutRequirementEnum.Routine };
            _target.Markouts.Add(new Markout {
                DateOfRequest = DateTime.Today,
                ReadyDate = DateTime.Today.AddDays(1),
                ExpirationDate = DateTime.Today.AddDays(2)
            });

            _target.SetCurrentMarkoutExpiration();
            
            Assert.IsTrue(_target.CurrentMarkout.ExpirationDate > DateTime.Today.AddDays(2));
        }

        [TestMethod]
        public void TestSetMarkoutExpirationDoesNotExtendMarkoutIfMarkoutsEditable()
        {
            _target.OperatingCenter.MarkoutsEditable = true;
            _target.MarkoutRequirement = new MarkoutRequirement { MarkoutRequirementID = (int)MarkoutRequirementEnum.Routine };
            _target.Markouts.Add(new Markout
            {
                DateOfRequest = DateTime.Today,
                ReadyDate = DateTime.Today.AddDays(1),
                ExpirationDate = DateTime.Today.AddDays(2)
            });

            _target.SetCurrentMarkoutExpiration();

            Assert.AreEqual(_target.CurrentMarkout.ExpirationDate, DateTime.Today.AddDays(2));
        }

        [TestMethod]
        public void TestMaterialsApprovedIfMaterialsApprovedOnAndMaterialsApprovedByHaveValues()
        {
            _target = new TestWorkOrderBuilder()
                .WithMaterialsApprovedOn(DateTime.Today)
                .WithMaterialsApprovedBy(new Employee());

            Assert.IsTrue(_target.MaterialsApproved);
        }

        [TestMethod]
        public void TestMaterialsNotApprovedIfMaterialsApprovedOnOrMaterialsApprovedByDoNoHaveValue()
        {
            _target = new TestWorkOrderBuilder()
                .WithMaterialsApprovedOn(DateTime.Today)
                .WithMaterialsApprovedBy(null);

            Assert.IsFalse(_target.MaterialsApproved);

            _target = new TestWorkOrderBuilder()
                .WithMaterialsApprovedOn(null)
                .WithMaterialsApprovedBy(new Employee());

            Assert.IsFalse(_target.MaterialsApproved);
        }

        [TestMethod]
        public void TestWorkStartedReturnsFalseWhenNoCurrentAssignment()
        {
            Assert.IsFalse(_target.WorkStarted);
        }

        [TestMethod]
        public void TestWorkStartedReturnsTrueIfWorkStartedOnAnyAssignment()
        {
            _target.CrewAssignments.Add(new CrewAssignment {
                Crew = new Crew()
            });

            // assignment but work no date started, should be false
            Assert.IsFalse(_target.WorkStarted);

            // DateStarted gets a Time, don't change to date here.
            _target.CurrentAssignment.DateStarted = DateTime.Now;

            // assignment and work is started
            Assert.IsTrue(_target.WorkStarted);

            _target.CrewAssignments.Add(new CrewAssignment {
                Crew = new Crew()
            });

            // current assignment hasn't been started, but work has been
            Assert.IsTrue(_target.WorkCompleted);
        }

        [TestMethod]
        public void TestWorkStartedBetweenReturnsTrueWhenWorkStartedBetweenDates()
        {
            var start = DateTime.Today.AddDays(-1);
            var end = DateTime.Today.AddDays(1);
            _target.CrewAssignments.Add(new CrewAssignment {
                Crew = new Crew()
            });

            Assert.IsFalse(_target.WorkStartedBetween(start, end));

            _target.CrewAssignments.Add(new CrewAssignment {
                Crew = new Crew(),
                DateStarted = DateTime.Today.AddDays(-10)
            });

            Assert.IsFalse(_target.WorkStartedBetween(start, end));

            _target.CrewAssignments.Add(new CrewAssignment
            {
                Crew = new Crew(),
                DateStarted = DateTime.Today.AddDays(10)
            });

            Assert.IsFalse(_target.WorkStartedBetween(start, end));

            _target.CrewAssignments.Add(new CrewAssignment
            {
                Crew = new Crew(),
                DateStarted = DateTime.Today
            });

            Assert.IsTrue(_target.WorkStartedBetween(start, end));
        }


        [TestMethod]
        public void TestWorkCompletedReturnsTrueWhenDateCompletedIsNotNull()
        {
            _target = new TestWorkOrderBuilder()
                .WithDateCompleted(DateTime.Now);

            Assert.IsTrue(_target.WorkCompleted);
        }

        [TestMethod]
        public void TestWorkCompletedReturnsFalseWhenDateCompletedIsNull()
        {
            _target = new TestWorkOrderBuilder()
                .WithDateCompleted(null);

            Assert.IsFalse(_target.WorkCompleted);
        }

        [TestMethod]
        public void TestProcessTimeReturnsDifferenceBetweenDateReceivedAndDateCompleted()
        {
            var now = DateTime.Now;
            var then = DateTime.Now.AddDays(1).AddHours(1).AddMinutes(1);
            var expected = new TimeSpan(1, 1, 1, 0);

            _target = new TestWorkOrderBuilder()
                .WithDateReceived(now)
                .WithDateCompleted(then);

            Assert.AreEqual(expected, _target.ProcessTime);
        }

        [TestMethod]
        public void TestProcessTimeReturnsNullIfReceivedDateOrCompletedDateIsNull()
        {
            var now = DateTime.Now;

            _target = new TestWorkOrderBuilder()
                .WithDateReceived(now)
                .WithDateCompleted(null);

            Assert.IsNull(_target.ProcessTime);

            _target = new TestWorkOrderBuilder()
                .WithDateReceived(null)
                .WithDateCompleted(now);

            Assert.IsNull(_target.ProcessTime);
        }

        [TestMethod]
        public void TestSupervisorProcessTimeReturnsDifferenceBetweenDateCompletedAndDateApproved()
        {
            var now = DateTime.Now;
            var then = DateTime.Now.AddDays(1).AddHours(1).AddMinutes(1);
            var expected = new TimeSpan(1, 1, 1, 0);

            _target = new TestWorkOrderBuilder()
                .WithDateCompleted(now)
                .WithApprovedOn(then);

            Assert.AreEqual(expected, _target.SupervisorProcessTime);
        }

        [TestMethod]
        public void TestSupervisorProcessTimeReturnsNullIfDateCompletedOrDateApprovedIsNull()
        {
            var now = DateTime.Now;

            _target = new TestWorkOrderBuilder()
                .WithDateCompleted(now)
                .WithApprovedOn(null);

            Assert.IsNull(_target.SupervisorProcessTime);

            _target = new TestWorkOrderBuilder()
                .WithDateCompleted(null)
                .WithApprovedOn(now);

            Assert.IsNull(_target.SupervisorProcessTime);
        }

        [TestMethod]
        public void TestStockProcessTimeReturnsDifferenceBetweenDateApprovedAndDateMaterialsApproved()
        {
            var now = DateTime.Now;
            var then = DateTime.Now.AddDays(1).AddHours(1).AddMinutes(1);
            var expected = new TimeSpan(1, 1, 1, 0);

            _target = new TestWorkOrderBuilder()
                .WithApprovedOn(now)
                .WithMaterialsApprovedOn(then);

            Assert.AreEqual(expected, _target.StockProcessTime);
        }

        [TestMethod]
        public void TestStockProcessTimeReturnsNullIfDateApprovedOrDateMaterialsApprovedIsNull()
        {
            var now = DateTime.Now;

            _target = new TestWorkOrderBuilder()
                .WithApprovedOn(now)
                .WithMaterialsApprovedOn(null);

            Assert.IsNull(_target.StockProcessTime);

            _target = new TestWorkOrderBuilder()
                .WithApprovedOn(null)
                .WithMaterialsApprovedOn(now);

            Assert.IsNull(_target.StockProcessTime);
        }

        [TestMethod]
        public void TestTotalManHoursReturnsCalculatedManHoursForAllCompletedCrewAssignments()
        {
            var now = DateTime.Now;
            var then = now.AddHours(1);

            var assignments = new CrewAssignment[] {
                new TestCrewAssignmentBuilder().WithDateStarted(now).WithDateEnded(then).WithEmployeesOnJob(1),
                new TestCrewAssignmentBuilder().WithDateStarted(now).WithDateEnded(then).WithEmployeesOnJob(2)
            };

            _target = new TestWorkOrderBuilder().WithCrewAssignments(assignments);

            Assert.AreEqual(3, _target.TotalManHours);
        }

        [TestMethod]
        public void TestTotalManHoursDoesNotIncludeCrewAssignmentsWithNoCountForEmployeesOnJob()
        {
            var now = DateTime.Now;
            var then = now.AddHours(2);

            var assignments = new CrewAssignment[] {
                new TestCrewAssignmentBuilder().WithDateStarted(now).WithDateEnded(then).WithEmployeesOnJob(2),
                new TestCrewAssignmentBuilder().WithDateStarted(now).WithDateEnded(then).WithEmployeesOnJob(3),
                new TestCrewAssignmentBuilder().WithDateStarted(now).WithDateEnded(then).WithEmployeesOnJob(null)
            };

            _target = new TestWorkOrderBuilder().WithCrewAssignments(assignments);

            Assert.AreEqual(10, _target.TotalManHours);
        }

        [TestMethod]
        public void TestTotalManHoursReturnsNullIfThereAreAnyIncompleteAssignments()
        {
            var now = DateTime.Now;
            var then = now.AddHours(1);

            var assignments = new CrewAssignment[] {
                new TestCrewAssignmentBuilder().WithDateStarted(now).WithDateEnded(then).WithEmployeesOnJob(3),
                new TestCrewAssignmentBuilder().WithDateStarted(now).WithDateEnded(then).WithEmployeesOnJob(4),
                new TestCrewAssignmentBuilder().WithDateStarted(now).WithDateEnded(null)
            };

            _target = new TestWorkOrderBuilder().WithCrewAssignments(assignments);

            Assert.IsNull(_target.TotalManHours);
        }

        [TestMethod]
        public void TestOriginalOrderReturnsOriginalOrder()
        {
            var expected = new TestWorkOrder {
                WorkOrderID = 919
            };
            
            _target = new TestWorkOrder() { OriginalOrder = expected };

            Assert.AreSame(expected, _target.OriginalOrder);
        }

        [TestMethod]
        public void TestAccountingStringReturnsBusinessUnitWhenAccountingTypeOAndM()
        {
            var expected = "123456";
            var workDescription = _mocks.DynamicMock<WorkDescription>();
            _target = new TestWorkOrder {
                WorkDescription = workDescription,
                BusinessUnit = expected
            };

            using (_mocks.Record())
            {
                SetupResult.For(workDescription.GetAccountingTypeEnum()).Return(AccountingTypeEnum.OAndM);
            }
            using (_mocks.Playback())
            {
                Assert.AreEqual(expected + ".", _target.FirstAccountingString);
            }
        }

        [TestMethod]
        public void TestAccountingStringReturnsAccountChargedWhenAccountingTypeNotOAndM()
        {
            var expected = "12345678";
            var workDescription = _mocks.DynamicMock<WorkDescription>();
            _target = new TestWorkOrder {
                WorkDescription = workDescription,
                AccountCharged = expected
            };

            using (_mocks.Record())
            {
                SetupResult.For(workDescription.GetAccountingTypeEnum()).Return(AccountingTypeEnum.Capital);
            }
            using (_mocks.Playback())
            {
                Assert.AreEqual(expected + ".", _target.FirstAccountingString);
            }
        }

        [TestMethod]
        public void TestSecondAccountingStringReturnsEmptyStringWhenNoWorkDescriptionFirstRestorationAccountingString()
        {
            Assert.AreEqual(_target.SecondAccountingString, string.Empty);
        }

        [TestMethod]
        public void TestSecondAccountingStringReturnsFirstAccountingStringWhenWorkDescriptionSecondRestorationAccountingString()
        {
            var expected = "12345678";
            var workDescription = _mocks.DynamicMock<WorkDescription>();
            _target = new TestWorkOrder {
                WorkDescription = workDescription,
                AccountCharged = expected
            };

            using (_mocks.Record())
            {
                SetupResult.For(workDescription.SecondRestorationAccountingString).Return("foo");
                SetupResult.For(workDescription.GetAccountingTypeEnum()).Return(AccountingTypeEnum.Capital);
            }
            using (_mocks.Playback())
            {
                Assert.AreEqual(expected + ".", _target.SecondAccountingString);
            }
        }

        [TestMethod]
        public void TestActuallyCreatedByReturnsNullIfDateCompletedIsNull()
        {
            _target = new TestWorkOrder();
            Assert.IsNull(_target.DateCompleted);
            Assert.IsNull(_target.ActuallyCompletedBy);
        }

        [TestMethod]
        public void TestActuallyCreatedByReturnsCompletedByIfDateCompletedIsNotNullAndCompletedByIsNotNull()
        {
            var expected = new Employee();
            _target = new TestWorkOrder();
            _target.DateCompleted = DateTime.Today;
            _target.CompletedBy = expected;
            Assert.AreSame(expected, _target.ActuallyCompletedBy);
        }

        [TestMethod]
        public void TestActuallyCreatedReturnsAssignedContractorIfDateCompletedIsNotNullAndCompletedByIsNull()
        {
            var expected = new Contractor();
            _target = new TestWorkOrder();
            _target.DateCompleted = DateTime.Today;
            _target.AssignedContractor = expected;
            Assert.AreSame(expected, _target.ActuallyCompletedBy);
        }

        [TestMethod]
        public void TestPermitsUserNameReturnsCapitalForWorkDescriptionAccountingTypeCapital()
        {
            var expected = "foo@bar.com";
            var oc = new OperatingCenter {
                PermitsCapitalUserName = expected
            };
            var wd = new WorkDescription {
                AccountingTypeID = (int)AccountingTypeEnum.Capital
            };
            var workOrder = new WorkOrder {
                WorkDescription = wd,
                OperatingCenter = oc
            };

            Assert.AreEqual(expected, workOrder.PermitsUserName);            
        }

        [TestMethod]
        public void TestPermitsUserNameReturnsCapitalForWorkDescriptionAccountingTypeRetirement()
        {
            var expected = "foo@bar.com";
            var oc = new OperatingCenter {
                PermitsCapitalUserName = expected
            };
            var wd = new WorkDescription {
                AccountingTypeID = (int)AccountingTypeEnum.Retirement
            };
            var workOrder = new WorkOrder {
                WorkDescription = wd,
                OperatingCenter = oc
            };

            Assert.AreEqual(expected, workOrder.PermitsUserName);
        }

        [TestMethod]
        public void TestPermitsUserNameReturnsCapitalForWorkDescriptionAccountingTypeOandM()
        {
            var expected = "foo@bar.com";
            var oc = new OperatingCenter {
                PermitsOMUserName = expected
            };
            var wd = new WorkDescription {
                AccountingTypeID = (int)AccountingTypeEnum.OAndM
            };
            var workOrder = new WorkOrder {
                WorkDescription = wd, 
                OperatingCenter = oc
            };

            Assert.AreEqual(expected, workOrder.PermitsUserName);
        }

        [TestMethod]
        public void TestCanBeApprovedReturnsProperlyForServiceWorkOrderWithAndWithOutService()
        {
            _target = new TestWorkOrder {
                AssetTypeID = AssetTypeRepository.Indices.SERVICE,
                WorkDescriptionID = WorkDescriptionRepository.SERVICE_LINE_RENEWAL,
                Service = new Service { DateInstalled = DateTime.Now }
            };

            Assert.IsFalse(_target.CanBeApproved.Value);

            for (var x = 1; x < 999; x++)
            {
                _target.WorkDescriptionID = x;
                if (WorkDescriptionRepository.SERVICE_APPROVAL_WORK_DESCRIPTIONS.Contains(x) ||
                    MapCall.Common.Model.Entities.WorkDescription.INVESTIGATIVE.Contains(x))
                    Assert.IsFalse(_target.CanBeApproved.Value);
                else
                    Assert.IsTrue(_target.CanBeApproved.Value);
            }

            _target.ServiceID = 44; //
            Assert.IsTrue(_target.CanBeApproved.Value);

            for (var x = 1; x < 999; x++)
            {
                _target.WorkDescriptionID = x;
                if (!MapCall.Common.Model.Entities.WorkDescription.INVESTIGATIVE.Contains(x))
                {
                    Assert.IsTrue(_target.CanBeApproved.Value);
                }
            }
        }

        [TestMethod]
        public void TestCanBeApprovedReturnsFalseForInvestigativeWorkDescription()
        {
            foreach (var i in MapCall.Common.Model.Entities.WorkDescription.INVESTIGATIVE)
            {
                _target = new TestWorkOrder {
                    WorkDescriptionID = i
                };
                Assert.IsFalse(_target.CanBeApproved.Value);
            }

            _target.AssetTypeID = AssetTypeRepository.Indices.SERVICE;
            _target.Service = new Service {Id = 1, DateInstalled = null};
            _target.WorkDescriptionID =
                WorkDescriptionRepository.SERVICE_LINE_RENEWAL;

            Assert.IsFalse(_target.CanBeApproved.Value);
        }

        [TestMethod]
        public void TestCanBeApprovedReturnsTrueForNonServiceAssetTypes()
        {
            var assetTypeIDs =
                from d in typeof(AssetTypeRepository.Indices).GetFields()
                select d.GetRawConstantValue();

            foreach (var assetTypeID in assetTypeIDs)
            {
                if ((short)assetTypeID == AssetTypeRepository.Indices.SERVICE)
                    continue;
                _target = new TestWorkOrder {AssetTypeID = (short)assetTypeID};
                Assert.IsTrue(_target.CanBeApproved.Value);
            }
        }

        [TestMethod]
        public void TestCanBeApprovedReturnsFalseForSapNotReleasedWorkOrder()
        {
            _target = new TestWorkOrder {
                SAPErrorCode = "Error: Order 90941453 is not released"
            };

            Assert.IsFalse(_target.CanBeApproved.Value);

            _target.SAPErrorCode = "WorkOrder Released";

            Assert.IsTrue(_target.CanBeApproved.Value);

            _target.SAPErrorCode = "WorkOrder Release rejected";

            Assert.IsFalse(_target.CanBeApproved.Value);
        }

        [TestMethod]
        public void TestIsNewServiceInstallationReturnsTrueFor()
        {
            var operatingCenter = new OperatingCenter { SAPEnabled = true, SAPWorkOrdersEnabled = true, IsContractedOperations = false };
            foreach (var workDescriptionId in WorkDescriptionRepository.NEW_SERVICE_INSTALLATION)
            {
                _target = new TestWorkOrder {
                    AssetTypeID = (AssetTypeRepository.Indices.SERVICE),
                    WorkDescriptionID = workDescriptionId, 
                    PremiseNumber = "1231231231",
                    OperatingCenter = operatingCenter
                };
                Assert.IsTrue(_target.IsNewServiceInstallation);
            }
        }

        [TestMethod]
        public void TestSewerOverflowsVisibleReturnsTrueForSewerOverflowVisibles()
        {
            var workDescription = new WorkDescription { Description = "sewer main overflow" };
            var target = new WorkOrder {
                WorkDescription = workDescription
            };
            Assert.IsTrue(target.SewerOverflowsVisible);

            workDescription.Description = "sewer service overflow";
            Assert.IsTrue(target.SewerOverflowsVisible);

            workDescription.Description = "Sewers not Skewers";
            Assert.IsFalse(target.SewerOverflowsVisible);
        }

        [TestMethod]
        public void TestIsRevisitReturnsFalseWhenWorkDescriptionIsNotRevisit()
        {
            var workDescription = new WorkDescription
            {
                Revisit = false
            };
            var workOrder = new WorkOrder
            {
                WorkDescription = workDescription
            };

            Assert.IsFalse(workOrder.IsRevisit);
        }

        [TestMethod]
        public void TestArcCollectorLink()
        {
            var workOrder = new WorkOrder {
                OperatingCenter = new OperatingCenter {
                    ArcMobileMapId = "15fdc279b4234fcb85f455ee70897a9e",
                    DataCollectionMapUrl = "arcgis-collector://"
                },
                Latitude = 40.247169,
                Longitude = -73.992704
            };

            Assert.AreEqual("arcgis-collector://?referenceContext=center&itemID=15fdc279b4234fcb85f455ee70897a9e&center=40.247169%2c-73.992704&scale=3000", workOrder.ArcCollectorLink);
        }

        [TestMethod]
        public void TestIsRevisitReturnsTrueWhenWorkDescriptionRevisit()
        {
            var workDescription = new WorkDescription {
                Revisit = true
            };
            var workOrder = new WorkOrder {
                WorkDescription = workDescription
            };

            Assert.IsTrue(workOrder.IsRevisit);
        }

        [TestMethod]
        public void TestApartmentAddtlOnAWorkOrderCanBeSetAndSaved()
        {
            var ApartmentAddtl = "Apt A";
            var wo = new TestWorkOrderBuilder().WithApartmentAddtl(ApartmentAddtl).Build();

            Assert.AreEqual(wo.ApartmentAddtl, ApartmentAddtl);
            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(wo));
        }

        [TestMethod]
        public void TestSettingAssignedContractorToNullSetsAssignedContractorIdToNull()
        {
            var contractor = new Contractor {
                ContractorID = 1234
            };
            var wo = new WorkOrder {
                AssignedContractor = contractor
            };
            
            // sanity check
            Assert.AreEqual(contractor.ContractorID, wo.AssignedContractorID);

            wo.AssignedContractor = null;
            
            Assert.IsNull(wo.AssignedContractorID);
        }
    }

    internal class TestWorkOrderBuilder : TestDataBuilder<TestWorkOrder>
    {
        #region Constants

        internal class Defaults
        {
            public const double LATITUDE = 1.1;
            public const double LONGITUDE = 2.2;
            public const string CUSTOMER_NAME = "Some Guy";
            public const string PHONE_NUMBER = "(888)555-1212";
            public const string STREET_NUMBER = "123";
            public static Town Town { get { return new Town(); } }
            public static Street Street { get { return new Street(); } }
            public static Street CrossStreet { get { return new Street(); } }
            public static AssetType AssetType
            {
                get { return new TestAssetTypeBuilder<Valve>(); }
            }
            public static Valve Valve
            {
                get
                {
                    return new Valve {
                        Coordinate = new Coordinate {
                            Latitude = LATITUDE,
                            Longitude = LONGITUDE
                        }
                    };
                }
            }
            public static WorkOrderRequester Requester
            {
                get { return TestWorkOrderRequesterBuilder.Customer; }
            }
            public static WorkDescription WorkDescription
            {
                get { return new WorkDescription { AssetType = AssetType }; }
            }
            public static MarkoutRequirement MarkoutRequirement
            {
                get
                {
                    return TestMarkoutRequirementBuilder.Routine;
                }
            }
            public static OperatingCenter OperatingCenter
            {
                get
                {
                    return new OperatingCenter();
                }
            }
        }

        #endregion

        #region Private Members

        private string _premiseNumber;
        private TestWorkOrder _baseOrder;
        private MarkoutRequirement _markoutRequirement =
            TestMarkoutRequirementBuilder.Routine;

        private AssetType _assetType = Defaults.AssetType;
        private Valve _valve = Defaults.Valve;
        private Hydrant _hydrant;
        private SewerOpening _sewerOpening;
        private StormCatch _stormCatch;
        private Equipment _equipment;
        private Town _town = new Town();
        private TownSection _townSection = new TownSection();
        private Street _street, _nearestCrossStreet;
 //       private StreetSuffix _suffix;
  //      private StreetPrefix _prefix;

        private WorkOrderRequester _requester = Defaults.Requester;

        private string _customerName = Defaults.CUSTOMER_NAME,
            _phoneNumber = Defaults.PHONE_NUMBER,
            _streetNumber = Defaults.STREET_NUMBER,
            _apartmentAddtl,
            _zipCode;
        private WorkDescription _description = Defaults.WorkDescription;
        private Markout[] _markouts;
        private CrewAssignment[] _crewAssignments;
        private Employee _createdBy = new Employee(),
                         _requestingEmployee = new Employee(),
                         _materialsApprovedBy = new Employee();
        private WorkOrderPriority _priority = new WorkOrderPriority();
        private WorkOrderPurpose _drivenBy = new WorkOrderPurpose();

        private DateTime? _materialsApprovedOn = DateTime.Now,
                          _dateCompleted = DateTime.Now,
                          _dateReceived = DateTime.Now;

        private DateTime? _approvedOn;
        private bool _isPremiseLinkedToSampleSite;

        private OrcomOrderCompletion[] _orcomOrderCompletions;

        private OperatingCenter _operatingCenter = new OperatingCenter();

        private PlantMaintenanceActivityType _pmatOveride;// = new PlantMaintenanceActivityType();

        private Employee _completedBy;

        private bool? _sopRequired;

        private TestWorkOrder baseOrder
        {
            get
            {
                if (_baseOrder == null)
                    _baseOrder = new TestWorkOrder();
                return _baseOrder;
            }
        }

        private bool _addExpiredMarkout,
                     _addCurrentMarkout,
                     _addCurrentAssignment;

        private int? _officeAssignmentID,
                     _completedByID,
                     _approvedByID,
                     _workOrderID;
        private string _accountCharged;
        private string _notes;
        private CustomerImpactRange _customerImpact;
        private RepairTimeRange _repairTime;
        private bool? _trafficImpact;
        private string _alertId, _sapErrorCode, _recordUrl;
        private long? _sapNumber;

        #endregion

        #region Private Methods

        private void AddMarkouts(WorkOrder order)
        {
            order.Markouts.AddRange(_markouts);
        }

        private void AddOrcomOrderCompletions(WorkOrder order)
        {
            order.OrcomOrderCompletions.AddRange(_orcomOrderCompletions);
        }

        private void AddCrewAssignments(WorkOrder order)
        {
            order.CrewAssignments.AddRange(_crewAssignments);
        }

        #endregion

        #region Exposed Methods

        public override TestWorkOrder Build()
        {
            if (_markoutRequirement != null)
                baseOrder.MarkoutRequirement = _markoutRequirement;
            if (_assetType != null)
                baseOrder.AssetType = _assetType;
            if (_valve != null)
                baseOrder.Valve = _valve;
            if (_hydrant != null)
                baseOrder.Hydrant = _hydrant;
            if (_sewerOpening != null)
                baseOrder.SewerOpening = _sewerOpening;
            if (_stormCatch != null)
                baseOrder.StormCatch = _stormCatch;
            if (_town != null)
                baseOrder.Town = _town;
            if (_townSection != null)
                baseOrder.TownSection = _townSection;
            if (_street != null)
                baseOrder.Street = _street;
            if (_nearestCrossStreet != null)
                baseOrder.NearestCrossStreet = _nearestCrossStreet;
            if (_requester != null)
                baseOrder.RequestedBy = _requester;
            if (_customerName != null)
                baseOrder.CustomerName = _customerName;
            if (_phoneNumber != null)
                baseOrder.PhoneNumber = _phoneNumber;
            if (_streetNumber != null)
                baseOrder.StreetNumber = _streetNumber;
            if (_apartmentAddtl != null)
                baseOrder.ApartmentAddtl = _apartmentAddtl;
            if (_description != null)
                baseOrder.WorkDescription = _description;
            if (_markouts != null)
                AddMarkouts(baseOrder);
            if (_zipCode != null)
                baseOrder.ZipCode = _zipCode;
            if (_equipment != null)
                baseOrder.Equipment = _equipment;
            else
            {
                if (_addExpiredMarkout)
                    baseOrder.Markouts.Add(
                        new TestMarkoutBuilder()
                            .WithWorkOrder(baseOrder)
                            .WithDateOfRequest(DateTime.Today.AddWeeks(-2))
                            .WithExpirationDate(DateTime.Today.AddWeeks(-1)));

                if (_addCurrentMarkout)
                    baseOrder.Markouts.Add(
                        new TestMarkoutBuilder()
                            .WithWorkOrder(baseOrder)
                            .WithDateOfRequest(DateTime.Today)
                            .WithExpirationDate(DateTime.Today.GetNextWeek()));
            }
            if (_addCurrentAssignment)
                baseOrder.CrewAssignments.Add(
                    new TestCrewAssignmentBuilder()
                        .WithCrew(new TestCrewBuilder()));
            if (_createdBy != null)
                baseOrder.CreatedBy = _createdBy;
            if (_requestingEmployee != null)
                baseOrder.RequestingEmployee = _requestingEmployee;
            if (_requester != null)
                baseOrder.RequestedBy = _requester;
            if (_priority != null)
                baseOrder.Priority = _priority;
            if (_drivenBy != null)
                baseOrder.DrivenBy = _drivenBy;
            if (_materialsApprovedBy != null)
                baseOrder.MaterialsApprovedBy = _materialsApprovedBy;
            if (_materialsApprovedOn != null)
                baseOrder.MaterialsApprovedOn = _materialsApprovedOn;
            if (_dateCompleted != null)
                baseOrder.DateCompleted = _dateCompleted;
            if (_dateReceived != null)
                baseOrder.DateReceived = _dateReceived;
            if (_premiseNumber != null)
                baseOrder.PremiseNumber = _premiseNumber;
            if (_operatingCenter != null)
                baseOrder.OperatingCenter = _operatingCenter;
            if (_orcomOrderCompletions != null)
                AddOrcomOrderCompletions(baseOrder);
            if (_completedBy != null)
                baseOrder.CompletedBy = _completedBy;
            if (_sopRequired != null)
                baseOrder.StreetOpeningPermitRequired = _sopRequired.Value;
            if (_officeAssignmentID != null)
                baseOrder.OfficeAssignmentID = _officeAssignmentID;
            if (_crewAssignments != null)
                AddCrewAssignments(baseOrder);
            if (_approvedOn != null)
                baseOrder.ApprovedOn = _approvedOn;
            if (_crewAssignments != null)
                AddCrewAssignments(baseOrder);
            if (_completedByID != null)
                baseOrder.CompletedByID = _completedByID;
            if (_approvedByID != null)
                baseOrder.ApprovedByID = _approvedByID;
            if (_accountCharged != null)
                baseOrder.AccountCharged = _accountCharged;
            if (_workOrderID != null)
                baseOrder.WorkOrderID = _workOrderID.Value;
            if (_notes != null)
                baseOrder.Notes = _notes;
            if (_customerImpact != null)
                baseOrder.CustomerImpactRange = _customerImpact;
            if (_repairTime != null)
                baseOrder.RepairTimeRange = _repairTime;
            if (_trafficImpact.HasValue)
                baseOrder.SignificantTrafficImpact = _trafficImpact;
            if (_alertId != null)
                baseOrder.AlertID = _alertId;
            if (_sapNumber != null)
                baseOrder.SAPWorkOrderNumber = _sapNumber;
            if (_pmatOveride != null)
                baseOrder.PlantMaintenanceActivityTypeOverride = _pmatOveride;
            if (!string.IsNullOrEmpty(_sapErrorCode))
                baseOrder.SAPErrorCode = _sapErrorCode;
            if (!string.IsNullOrEmpty(_apartmentAddtl))
                baseOrder.ApartmentAddtl = _apartmentAddtl;
            if (!string.IsNullOrEmpty(_recordUrl))
                baseOrder.RecordUrl = _recordUrl;
            baseOrder.IsPremiseLinkedToSampleSite = _isPremiseLinkedToSampleSite;
            return baseOrder;
        }

        public TestWorkOrderBuilder BuildForPlanning()
        {
            return WithTown(Defaults.Town)
                .WithStreet(Defaults.Street)
                .WithNearestCrossStreet(Defaults.CrossStreet)
                .WithAssetType(Defaults.AssetType)
                .WithValve(Defaults.Valve)
                .WithRequester(Defaults.Requester)
                .WithCustomerName(Defaults.CUSTOMER_NAME)
                .WithPhoneNumber(Defaults.PHONE_NUMBER)
                .WithStreetNumber(Defaults.STREET_NUMBER)
                .WithWorkDescription(Defaults.WorkDescription)
                .WithMarkoutRequirement(Defaults.MarkoutRequirement)
                .WithOperatingCenter(Defaults.OperatingCenter)
                .WithDateCompleted(null)
                .WithMarkouts(null);
        }

        public TestWorkOrderBuilder BuildForScheduling()
        {
            return BuildForPlanning()
                .WithExpiredMarkout()
                .WithCurrentMarkout()
                .WithDateCompleted(null)
                .WithOperatingCenter(Defaults.OperatingCenter);
        }

        public TestWorkOrderBuilder BuildForFinalization()
        {
            return BuildForScheduling()
                .WithCurrentAssignment()
                .WithOperatingCenter(Defaults.OperatingCenter);
        }

        public TestWorkOrderBuilder BuildCompleteOrder()
        {
            _dateCompleted = DateTime.Now;
            return this;
        }

        public TestWorkOrderBuilder BuildIncompleteOrder()
        {
            _dateCompleted = null;
            return this;
        }

        public TestWorkOrderBuilder WithMarkoutRequirement(MarkoutRequirement requirement)
        {
            _markoutRequirement = requirement;
            return this;
        }

        public TestWorkOrderBuilder WithAssetType(AssetType assetType)
        {
            _assetType = assetType;
            return this;
        }

        public TestWorkOrderBuilder WithValve(Valve valve)
        {
            _valve = valve;
            return this;
        }

        public TestWorkOrderBuilder WithHydrant(Hydrant hydrant)
        {
            _hydrant = hydrant;
            return this;
        }

        public TestWorkOrderBuilder WithSewerOpening(SewerOpening sewerOpening)
        {
            _sewerOpening = sewerOpening;
            return this;
        }

        public TestWorkOrderBuilder WithStormCatch(StormCatch stormCatch)
        {
            _stormCatch = stormCatch;
            return this;
        }

        public TestWorkOrderBuilder WithApartmentAddtl(string apartmentAddtl)
        {
            _apartmentAddtl = apartmentAddtl;
            return this;
        }

        public TestWorkOrderBuilder WithEquipment(Equipment equipment)
        {
            _equipment = equipment;
            return this;
        }

        public TestWorkOrderBuilder WithTown(string townName)
        {
            return WithTown(new TestTownBuilder().WithName(townName));
        }

        public TestWorkOrderBuilder WithTown(Town town)
        {
            _town = town;
            return this;
        }

        public TestWorkOrderBuilder WithTownSection(TownSection townSection)
        {
            _townSection = townSection;
            return this;
        }

        public TestWorkOrderBuilder WithStreet(Street street)
        {
            _street = street;
            return this;
        }


        public TestWorkOrderBuilder WithStreet(string streetName)
        {
            return WithStreet(new TestStreetBuilder().WithStreetName(streetName).WithSuffix(streetName));
        }

        public TestWorkOrderBuilder WithNearestCrossStreet(Street street)
        {
            _nearestCrossStreet = street;
            return this;
        }

        public TestWorkOrderBuilder WithRequester(WorkOrderRequester requester)
        {
            _requester = requester;
            return this;
        }

        public TestWorkOrderBuilder WithRequestingEmployee(Employee employee)
        {
            _requestingEmployee = employee;
            return this;
        }

        public TestWorkOrderBuilder WithCustomerName(string customerName)
        {
            _customerName = customerName;
            return this;
        }

        public TestWorkOrderBuilder WithPhoneNumber(string phoneNumber)
        {
            _phoneNumber = phoneNumber;
            return this;
        }

        public TestWorkOrderBuilder WithStreetNumber(string streetNumber)
        {
            _streetNumber = streetNumber;
            return this;
        }

        public TestWorkOrderBuilder WithWorkDescription(WorkDescription description)
        {
            _description = description;
            return this;
        }

        public TestWorkOrderBuilder WithMarkouts(Markout[] markouts)
        {
            _markouts = markouts;
            return this;
        }

        public TestWorkOrderBuilder WithExpiredMarkout()
        {
            _addExpiredMarkout = true;
            return this;
        }

        public TestWorkOrderBuilder WithCurrentMarkout()
        {
            _addCurrentMarkout = true;
            return this;
        }

        public TestWorkOrderBuilder WithCurrentAssignment()
        {
            _addCurrentAssignment = true;
            return this;
        }

        public TestWorkOrderBuilder WithZipCode(String zipCode)
        {
            _zipCode = zipCode;
            return this;
        }

        public TestWorkOrderBuilder WithPurpose(WorkOrderPurpose purpose)
        {
            _drivenBy = purpose;
            return this;
        }

        public TestWorkOrderBuilder WithCreatedBy(Employee createdBy)
        {
            _createdBy = createdBy;
            return this;
        }

        public TestWorkOrderBuilder WithPriority(WorkOrderPriority priority)
        {
            _priority = priority;
            return this;
        }

        public TestWorkOrderBuilder WithMaterialsApprovedBy(Employee approvedBy)
        {
            _materialsApprovedBy = approvedBy;
            return this;
        }

        public TestWorkOrderBuilder WithMaterialsApprovedOn(DateTime? approvedOn)
        {
            _materialsApprovedOn = approvedOn;
            return this;
        }

        public TestWorkOrderBuilder WithDateCompleted(DateTime? dateCompleted)
        {
            _dateCompleted = dateCompleted;
            return this;
        }

        public TestWorkOrderBuilder WithDateReceived(DateTime? dateReceived)
        {
            _dateReceived = dateReceived;
            return this;
        }

        public TestWorkOrderBuilder WithApprovedOn(DateTime? approvedOn)
        {
            _approvedOn = approvedOn;
            return this;
        }

        public TestWorkOrderBuilder WithPremiseNumber(string num)
        {
            _premiseNumber = num;
            return this;
        }

        public TestWorkOrderBuilder WithIsPremiseLinkedToSampleSite(bool isPremiseLinkedToSampleSite)
        {
            _isPremiseLinkedToSampleSite = isPremiseLinkedToSampleSite;
            return this;
        }

        public TestWorkOrderBuilder WithOperatingCenter(OperatingCenter operatingCenter)
        {
            _operatingCenter = operatingCenter;
            return this;
        }

        public TestWorkOrderBuilder WithOrcomOrderCompletions(OrcomOrderCompletion[] orcomOrderCompletions)
        {
            _orcomOrderCompletions = orcomOrderCompletions;
            return this;
        }

        public TestWorkOrderBuilder WithCompletedBy(Employee completedBy)
        {
            _completedBy = completedBy;
            return this;
        }

        public TestWorkOrderBuilder WithSOPRequired(bool? required)
        {
            _sopRequired = required;
            return this;
        }

        public TestWorkOrderBuilder WithOfficeAssignmentID(int id)
        {
            _officeAssignmentID = id;
            return this;
        }

        public TestWorkOrderBuilder WithCrewAssignments(CrewAssignment[] assignments)
        {
            _crewAssignments = assignments;
            return this;
        }

        public TestWorkOrderBuilder WithCompletedByID(int? id)
        {
            _completedByID = id;
            return this;
        }

        public TestWorkOrderBuilder WithApprovedByID(int? id)
        {
            _approvedByID = id;
            return this;
        }

        public TestWorkOrderBuilder WithAccountCharged(string s)
        {
            _accountCharged = s;
            return this;
        }

        public TestWorkOrderBuilder WithWorkOrderID(int? id)
        {
            _workOrderID = id;
            return this;
        }

        public TestWorkOrderBuilder WithNotes(string notes)
        {
            _notes = notes;
            return this;
        }

        public TestWorkOrderBuilder WithWorkDescription(string workDescription)
        {
            return
                WithWorkDescription(
                    new TestWorkDescriptionBuilder().WithDescription(
                        workDescription));
        }

        public TestWorkOrderBuilder WithOperatingCenter(string opCode, string operatingCenter)
        {
            return WithOperatingCenter(new OperatingCenter {OpCntr = opCode, OpCntrName = operatingCenter});
        }

        public TestWorkOrderBuilder WithNearestCrossStreet(string nearestCrossStreet)
        {
            return
                WithNearestCrossStreet(
                    new TestStreetBuilder().WithStreetName(nearestCrossStreet));
        }

        public TestWorkOrderBuilder WithEstimatedCustomerImpact(string customerImpact)
        {
            _customerImpact = new CustomerImpactRange {
                Description = customerImpact
            };
            return this;
        }

        public TestWorkOrderBuilder WithRepairTimeRange(string repairTime)
        {
            _repairTime = new RepairTimeRange {
                Description = repairTime
            };
            return this;
        }

        public TestWorkOrderBuilder WithTrafficImpact(bool trafficImpact)
        {
            _trafficImpact = trafficImpact;
            return this;
        }

        public TestWorkOrderBuilder WithAlertId(string alertId)
        {
            _alertId = alertId;
            return this;
        }

        public TestWorkOrderBuilder WithSAPWorkOrderNumber(long? sapNumber)
        {
            _sapNumber = sapNumber;
            return this;
        }

        public TestWorkOrderBuilder WithPMATOverride(PlantMaintenanceActivityType pmat)
        {
            _pmatOveride = pmat;
            return this;
        }

        public TestWorkOrderBuilder WithSAPErrorCode(string errorcode)
        {
            _sapErrorCode = errorcode;
            return this;
        }

        public TestWorkOrderBuilder WithRecordUrl(int? id)
        {
            _recordUrl = string.Format(
                "https://mapcall.awapps.com/Modules/WorkOrders/Views/WorkOrders/General/WorkOrderGeneralResourceRPCPage.aspx?cmd=view&arg={0}",
                id);
            return this;
        }

        #endregion
    }

    internal class TestWorkOrder : WorkOrder
    {
    }

    internal class MockWorkOrderRepository : MockRepository<WorkOrder>
    {
    }
}