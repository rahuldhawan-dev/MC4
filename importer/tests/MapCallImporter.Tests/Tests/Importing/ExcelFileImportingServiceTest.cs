using MapCall.Common.Model.Entities;
using MapCallImporter.Common;
using MapCallImporter.Importing;
using MapCallImporter.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MapCall.Common.Model.Repositories;
using MapCallImporter.Library.Testing;
using MapCallImporter.SampleValues;
using StructureMap;

namespace MapCallImporter.Tests.Importing
{
    [TestClass]
    public class ExcelFileImportingServiceTest : MapCallImporterInMemoryDatabaseTestBase<ExcelFileImportingService>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        #endregion

        #region Private Methods

        private string GetFailureMessage(TimedExcelFileMappingResult result)
        {
            return result.Result != ExcelFileProcessingResult.InvalidFileContents
                ? null
                : $"The following issues were encountered:{Environment.NewLine}{string.Join(Environment.NewLine, result.Issues)}";
        }

        private void TestValidFile<TEntity>(Action<IContainer> dataSeedFn, int expectedCount,
            string file)
        {
            dataSeedFn(_container);

            var validationResult = Validate<TEntity>(file);

            switch (validationResult.Result)
            {
                case ExcelFileProcessingResult.InvalidFileContents:
                    Assert.Fail(
                        $"Validation did not pass, the following issues were encountered:{Environment.NewLine}{string.Join(Environment.NewLine, validationResult.Issues)}");
                    break;
                case ExcelFileProcessingResult.CouldNotDetermineContentType:
                    Assert.Fail($"Unable to determine content type for file '{file}'.");
                    break;
                case ExcelFileProcessingResult.FileValid:
                    break;
                default:
                    Assert.Fail($@"Got unexpected result '{validationResult.Result}' processing file '{file}'.
Errors:
{string.Join(Environment.NewLine, validationResult.Issues)}");
                    break;
            }

            ExpectCountChange<TEntity>(expectedCount, () => {
                var importResult = _target.Handle(validationResult);

                Assert.AreEqual(ExcelFileProcessingResult.FileValid,
                    importResult.Result,
                    GetFailureMessage(importResult));
            });
        }

        private void TestValidFile<TEntity>(Func<IContainer, int> dataSeedFn, int expectedCount,
            string file)
        {
            TestValidFile<TEntity>(new Action<IContainer>((c) => dataSeedFn(c)), expectedCount,
                file);
        }

        private void TestValidEquipmentFile(string equipmentType, string filePath)
        {
            TestValidFile<Equipment>(
                (c) => TestDataHelper.CreateStuffForEquipmentInAberdeenNJ(c, equipmentType), 8, filePath);
        }

        private void TestInvalidFile<TEntity>(Action<IContainer> dataSeedFn, string file)
        {
            dataSeedFn(_container);

            ExpectCountChange<TEntity>(0, () => {
                var result = Validate<TEntity>(file);

                if (result.Result != ExcelFileProcessingResult.FileValid)
                {
                    return;
                }

                Assert.AreEqual(ExcelFileProcessingResult.InvalidFileContents,
                    _target.Handle(result).Result);
            });
        }

        private TimedExcelFileMappingResult Validate<TEntity>(string file)
        {
            var validatingService = new ExcelFileValidationService(new ExcelFileValidatorFinderService(Container));
            var result = validatingService.Handle(GetRelativePath(file));
            return result;
        }

        private void TestInvalidFile<TEntity>(Func<IContainer, int> dataSeedFn, string file)
        {
            TestInvalidFile<TEntity>(new Action<IContainer>((c) => dataSeedFn(c)), file);
        }

        #endregion

        #region Facilities

        [TestMethod]
        public void TestHandleImportsFacilitiesFileWithValidRecords()
        {
            TestValidFile<Facility>(TestDataHelper.CreateStuffForFacilitiesInAberdeenNJ, 4,
                SampleFiles.SampleFiles.Import.Facilities.VALID);
        }

        #endregion

        #region Streets

        [TestMethod]
        public void TestHandleImportsStreetsFileWithValidRecords()
        {
            TestValidFile<Street>(TestDataHelper.CreateAberdeenNJWithCountyAndStateAndSomeStreets, 4,
                SampleFiles.SampleFiles.Import.Streets.VALID);
        }

        [TestMethod]
        public void TestHandleDoesNotImportStreetsFileWithMissingTown()
        {
            TestInvalidFile<Street>(TestDataHelper.CreateAberdeenNJWithCountyAndStateAndSomeStreets,
                SampleFiles.SampleFiles.Import.Streets.MISSING_TOWN);
        }

        #endregion

        #region Valves

        [TestMethod]
        public void TestHandleImportsValvesFileWithValidRecords()
        {
            TestValidFile<Valve>(TestDataHelper.CreateStuffForValvesInAberdeenNJ, 4, SampleFiles.SampleFiles.Import.Valves.VALID);
        }

        #endregion

        #region ValveInspections

        [TestMethod]
        public void TestHandleImportsValveInspectionsFileWithValidRecords()
        {
            TestValidFile<ValveInspection>((c) => {
                TestDataHelper.CreateStuffForValvesInAberdeenNJ(c);
                TestDataHelper.CreateValidValvesLikeTheValidValvesInTheValidValvesFile(c);
            }, 4, SampleFiles.SampleFiles.Import.ValveInspections.VALID);
        }

        #endregion

        #region Hydrants

        [TestMethod]
        public void TestHandleImportsHydrantsFileWithValidRecords()
        {
            TestValidFile<Hydrant>(TestDataHelper.CreateStuffForHydrantsInAberdeenNJ, 4, SampleFiles.SampleFiles.Import.Hydrants.VALID);

            WithUnitOfWork(uow => {
                foreach (var hydrantNumber in new[] {"HAB-6666", "HAB-6667", "HAB-6668", "HAB-6669"})
                {
                    Assert.AreEqual(1, uow.SqlQuery("SELECT 1 FROM Hydrants WHERE HydrantNumber = :hydrantNumber")
                                          .SetString("hydrantNumber", hydrantNumber)
                                          .SafeUniqueIntResult());
                }
            });
        }

        [TestMethod]
        public void TestHandleDoesNotChokeOnHydrantsFileWithBadElevationValue()
        {
            TestInvalidFile<Hydrant>(TestDataHelper.CreateStuffForHydrantsInAberdeenNJ,
                SampleFiles.SampleFiles.Import.Hydrants.BAD_ELEVATION_VALUE);
        }

        #endregion

        #region HydrantInspections

        [TestMethod]
        public void TestHandleImportsHydrantInspectionsFileWithValidRecords()
        {
            TestValidFile<HydrantInspection>((c) => {
                TestDataHelper.CreateStuffForHydrantsInAberdeenNJ(c);
                TestDataHelper.CreateValidHydrantsLikeTheValidHydrantsInTheValidHydrantsFile(c);
            }, 4, SampleFiles.SampleFiles.Import.HydrantInspections.VALID);
        }

        #endregion

        #region Equipment

        [TestMethod]
        public void TestHandleImportsAdjustableSpeedDriveFileWithValidRecords()
        {
            TestValidEquipmentFile("ADJUSTABLE SPEED DRIVE",
                SampleFiles.SampleFiles.Import.Equipment.AdjustableSpeedDrive.VALID);
        }

        [TestMethod]
        public void TestHandleImportsAeratorFileWithValidRecords()
        {
            TestValidEquipmentFile("AERATOR",
                SampleFiles.SampleFiles.Import.Equipment.Aerator.VALID);
        }

        [TestMethod]
        public void TestHandleImportsAirCompressorFileWithValidRecords()
        {
            TestValidEquipmentFile("AIR COMPRESSOR",
                SampleFiles.SampleFiles.Import.Equipment.AirCompressor.VALID);
        }

        [TestMethod]
        public void TestHandleImportsAirVacuumTankFileWithValidRecords()
        {
            TestValidEquipmentFile("AIR/ VACUUM TANK",
                SampleFiles.SampleFiles.Import.Equipment.AirVacuumTank.VALID);
        }

        [TestMethod]
        public void TestHandleImportsAMIDATACOLLFileWithValidRecords()
        {
            TestValidEquipmentFile("AMIDATACOLL",
                SampleFiles.SampleFiles.Import.Equipment.AMIDATACOLL.VALID);
        }

        [TestMethod]
        public void TestHandleImportsArcFlashProtectionFileWithValidRecords()
        {
            TestValidEquipmentFile("ARC FLASH PROTECTION",
                SampleFiles.SampleFiles.Import.Equipment.ArcFlashProtection.VALID);
        }

        [TestMethod]
        public void TestHandleImportsBatteryFileWithValidRecords()
        {
            TestValidEquipmentFile("BATTERY",
                SampleFiles.SampleFiles.Import.Equipment.Battery.VALID);
        }

        [TestMethod]
        public void TestHandleImportsBatteryChargerFileWithValidRecords()
        {
            TestValidEquipmentFile("BATTERY CHARGER",
                SampleFiles.SampleFiles.Import.Equipment.BatteryCharger.VALID);
        }

        [TestMethod]
        public void TestHandleImportsBlowerFileWithValidRecords()
        {
            TestValidEquipmentFile("BLOWER",
                SampleFiles.SampleFiles.Import.Equipment.Blower.VALID);
        }

        [TestMethod]
        public void TestHandleImportsBlowOffValveFileWithValidRecords()
        {
            TestValidEquipmentFile("BLOW OFF VALVE",
                SampleFiles.SampleFiles.Import.Equipment.BlowOffValve.VALID);
        }

        [TestMethod]
        public void TestHandleImportsBoilerFileWithValidRecords()
        {
            TestValidEquipmentFile("BOILER",
                SampleFiles.SampleFiles.Import.Equipment.Boiler.VALID);
        }

        [TestMethod]
        public void TestHandleImportsBurnerFileWithValidRecords()
        {
            TestValidEquipmentFile("BURNER",
                SampleFiles.SampleFiles.Import.Equipment.Burner.VALID);
        }

        [TestMethod]
        public void TestHandleImportsCalibrationDeviceFileWithValidRecords()
        {
            TestValidEquipmentFile("CALIBRATION DEVICE",
                SampleFiles.SampleFiles.Import.Equipment.CalibrationDevice.VALID);
        }

        [TestMethod]
        public void TestHandleImportsCathodicProtectionFileWithValidRecords()
        {
            TestValidEquipmentFile("CATHODIC PROTECTION",
                SampleFiles.SampleFiles.Import.Equipment.CathodicProtection.VALID);
        }

        [TestMethod]
        public void TestHandleImportsChemicalDryFeederFileWithValidRecords()
        {
            TestValidEquipmentFile("CHEMICAL DRY FEEDER",
                SampleFiles.SampleFiles.Import.Equipment.ChemicalDryFeeder.VALID);
        }

        [TestMethod]
        public void TestHandleImportsChemicalGasFeederFileWithValidRecords()
        {
            TestValidEquipmentFile("CHEMICAL GAS FEEDER",
                SampleFiles.SampleFiles.Import.Equipment.ChemicalGasFeeder.VALID);
        }

        [TestMethod]
        public void TestHandleImportsChemicalGeneratorsFileWithValidRecords()
        {
            TestValidEquipmentFile("CHEMICAL GENERATORS",
                SampleFiles.SampleFiles.Import.Equipment.ChemicalGenerators.VALID);
        }

        [TestMethod]
        public void TestHandleImportsChemicalLiquidFeederFileWithValidRecords()
        {
            TestValidEquipmentFile("CHEMICAL LIQUID FEEDER",
                SampleFiles.SampleFiles.Import.Equipment.ChemicalLiquidFeeder.VALID);
        }

        [TestMethod]
        public void TestHandleImportsChemicalPipingFileWithValidRecords()
        {
            TestValidEquipmentFile("CHEMICAL PIPING",
                SampleFiles.SampleFiles.Import.Equipment.ChemicalPiping.VALID);
        }

        [TestMethod]
        public void TestHandleImportsChemicalTankFileWithValidRecords()
        {
            TestValidEquipmentFile("CHEMICAL TANK",
                SampleFiles.SampleFiles.Import.Equipment.ChemicalTank.VALID);
        }

        [TestMethod]
        public void TestHandleImportsClarifierFileWithValidRecords()
        {
            TestValidEquipmentFile("CLARIFIER",
                SampleFiles.SampleFiles.Import.Equipment.Clarifier.VALID);
        }

        [TestMethod]
        public void TestHandleImportsCleanOutFileWithValidRecords()
        {
            TestValidEquipmentFile("CLEAN OUT",
                SampleFiles.SampleFiles.Import.Equipment.CleanOut.VALID);
        }

        [TestMethod]
        public void TestHandleImportsCollectionSystemGeneralFileWithValidRecords()
        {
            TestValidEquipmentFile("COLLECTION SYSTEM GENERAL",
                SampleFiles.SampleFiles.Import.Equipment.CollectionSystemGeneral.VALID);
        }

        [TestMethod]
        public void TestHandleImportsControllerFileWithValidRecords()
        {
            TestValidEquipmentFile("CONTROLLER",
                SampleFiles.SampleFiles.Import.Equipment.Controller.VALID);
        }

        [TestMethod]
        public void TestHandleImportsControlPanelFileWithValidRecords()
        {
            TestValidEquipmentFile("CONTROL PANEL",
                SampleFiles.SampleFiles.Import.Equipment.ControlPanel.VALID);
        }

        [TestMethod]
        public void TestHandleImportsConveyorFileWithValidRecords()
        {
            TestValidEquipmentFile("CONVEYOR",
                SampleFiles.SampleFiles.Import.Equipment.Conveyor.VALID);
        }

        [TestMethod]
        public void TestHandleImportsDamFileWithValidRecords()
        {
            TestValidEquipmentFile("DAM",
                SampleFiles.SampleFiles.Import.Equipment.Dam.VALID);
        }

        [TestMethod]
        public void TestHandleImportsDefibrillatorFileWithValidRecords()
        {
            TestValidEquipmentFile("DEFIBRILLATOR",
                SampleFiles.SampleFiles.Import.Equipment.Defibrillator.VALID);
        }

        [TestMethod]
        public void TestHandleImportsDistributionSystemFileWithValidRecords()
        {
            TestValidEquipmentFile("DISTRIBUTION SYSTEM",
                SampleFiles.SampleFiles.Import.Equipment.DistributionSystem.VALID);
        }

        [TestMethod]
        public void TestHandleImportsDistributionToolFileWithValidRecords()
        {
            TestValidEquipmentFile("DISTRIBUTION TOOL",
                SampleFiles.SampleFiles.Import.Equipment.DistributionTool.VALID);
        }

        [TestMethod]
        public void TestHandleImportsElevatorFileWithValidRecords()
        {
            TestValidEquipmentFile("ELEVATOR",
                SampleFiles.SampleFiles.Import.Equipment.Elevator.VALID);
        }

        [TestMethod]
        public void TestHandleImportsEmergencyGeneratorFileWithValidRecords()
        {
            TestValidEquipmentFile("EMERGENCY GENERATOR",
                SampleFiles.SampleFiles.Import.Equipment.EmergencyGenerator.VALID);
        }

        [TestMethod]
        public void TestHandleImportsEmergencyLightFileWithValidRecords()
        {
            TestValidEquipmentFile("EMERGENCY LIGHT",
                SampleFiles.SampleFiles.Import.Equipment.EmergencyLight.VALID);
        }

        [TestMethod]
        public void TestHandleImportsEngineFileWithValidRecords()
        {
            TestValidEquipmentFile("ENGINE",
                SampleFiles.SampleFiles.Import.Equipment.Engine.VALID);
        }

        [TestMethod]
        public void TestHandleImportsEyewashFileWithValidRecords()
        {
            TestValidEquipmentFile("EYEWASH",
                SampleFiles.SampleFiles.Import.Equipment.Eyewash.VALID);
        }

        [TestMethod]
        public void TestHandleImportsFacilityAndGroundsFileWithValidRecords()
        {
            TestValidEquipmentFile("FACILITY AND GROUNDS",
                SampleFiles.SampleFiles.Import.Equipment.FacilityAndGrounds.VALID);
        }

        [TestMethod]
        public void TestHandleImportsFallProtectionFileWithValidRecords()
        {
            TestValidEquipmentFile("FALL PROTECTION",
                SampleFiles.SampleFiles.Import.Equipment.FallProtection.VALID);
        }

        [TestMethod]
        public void TestHandleImportsFilterFileWithValidRecords()
        {
            TestValidEquipmentFile("FILTER",
                SampleFiles.SampleFiles.Import.Equipment.Filter.VALID);
        }

        [TestMethod]
        public void TestHandleImportsFireAlarmFileWithValidRecords()
        {
            TestValidEquipmentFile("FIRE ALARM",
                SampleFiles.SampleFiles.Import.Equipment.FireAlarm.VALID);
        }

        [TestMethod]
        public void TestHandleImportsFireExtinguisherFileWithValidRecords()
        {
            TestValidEquipmentFile("FIRE EXTINGUISHER",
                SampleFiles.SampleFiles.Import.Equipment.FireExtinguisher.VALID);
        }

        [TestMethod]
        public void TestHandleImportsFireSuppressionFileWithValidRecords()
        {
            TestValidEquipmentFile("FIRE SUPPRESSION",
                SampleFiles.SampleFiles.Import.Equipment.FireSuppression.VALID);
        }

        [TestMethod]
        public void TestHandleImportsFirewallFileWithValidRecords()
        {
            TestValidEquipmentFile("FIREWALL",
                SampleFiles.SampleFiles.Import.Equipment.Firewall.VALID);
        }

        [TestMethod]
        public void TestHandleImportsFloatationDeviceFileWithValidRecords()
        {
            TestValidEquipmentFile("FLOATATION DEVICE",
                SampleFiles.SampleFiles.Import.Equipment.FloatationDevice.VALID);
        }

        [TestMethod]
        public void TestHandleImportsFlowMeterFileWithValidRecords()
        {
            TestValidEquipmentFile("FLOW METER (NON PREMISE)",
                SampleFiles.SampleFiles.Import.Equipment.FlowMeter.VALID);
        }

        [TestMethod]
        public void TestHandleImportsFlowWeirFileWithValidRecords()
        {
            TestValidEquipmentFile("FLOW WEIR",
                SampleFiles.SampleFiles.Import.Equipment.FlowWeir.VALID);
        }

        [TestMethod]
        public void TestHandleImportsFuelTankFileWithValidRecords()
        {
            TestValidEquipmentFile("FUEL TANK",
                SampleFiles.SampleFiles.Import.Equipment.FuelTank.VALID);
        }

        [TestMethod]
        public void TestHandleImportsGasDetectorFileWithValidRecords()
        {
            TestValidEquipmentFile("GAS DETECTOR",
                SampleFiles.SampleFiles.Import.Equipment.GasDetector.VALID);
        }

        [TestMethod]
        public void TestHandleImportsGearboxFileWithValidRecords()
        {
            TestValidEquipmentFile("GEARBOX",
                SampleFiles.SampleFiles.Import.Equipment.Gearbox.VALID);
        }

        [TestMethod]
        public void TestHandleImportsGravitySewerMainFileWithValidRecords()
        {
            TestValidEquipmentFile("GRAVITY SEWER MAIN",
                SampleFiles.SampleFiles.Import.Equipment.GravitySewerMain.VALID);
        }

        [TestMethod]
        public void TestHandleImportsGrinderFileWithValidRecords()
        {
            TestValidEquipmentFile("GRINDER",
                SampleFiles.SampleFiles.Import.Equipment.Grinder.VALID);
        }

        [TestMethod]
        public void TestHandleImportsHoistFileWithValidRecords()
        {
            TestValidEquipmentFile("HOIST",
                SampleFiles.SampleFiles.Import.Equipment.Hoist.VALID);
        }

        [TestMethod]
        public void TestHandleImportsHVACChillerFileWithValidRecords()
        {
            TestValidEquipmentFile("HVAC CHILLER",
                SampleFiles.SampleFiles.Import.Equipment.HVACChiller.VALID);
        }

        [TestMethod]
        public void TestHandleImportsHVACCombinationUnitFileWithValidRecords()
        {
            TestValidEquipmentFile("HVAC COMBINATION UNIT",
                SampleFiles.SampleFiles.Import.Equipment.HVACCombinationUnit.VALID);
        }

        [TestMethod]
        public void TestHandleImportsHVACDehumidifierFileWithValidRecords()
        {
            TestValidEquipmentFile("HVAC DEHUMIDIFIER",
                SampleFiles.SampleFiles.Import.Equipment.HVACDehumidifier.VALID);
        }

        [TestMethod]
        public void TestHandleImportsHeatExchangerFileWithValidRecords()
        {
            TestValidEquipmentFile("HEAT EXCHANGER",
                SampleFiles.SampleFiles.Import.Equipment.HeatExchanger.VALID);
        }

        [TestMethod]
        public void TestHandleImportsHVACHeaterFileWithValidRecords()
        {
            TestValidEquipmentFile("HVAC HEATER",
                SampleFiles.SampleFiles.Import.Equipment.HVACHeater.VALID);
        }

        [TestMethod]
        public void TestHandleImportsCoolingTowerFileWithValidRecords()
        {
            TestValidEquipmentFile("COOLING TOWER",
                SampleFiles.SampleFiles.Import.Equipment.CoolingTower.VALID);
        }

        [TestMethod]
        public void TestHandleImportsHVACVentilatorFileWithValidRecords()
        {
            TestValidEquipmentFile("HVAC VENTILATOR",
                SampleFiles.SampleFiles.Import.Equipment.HVACVentilator.VALID);
        }

        [TestMethod]
        public void TestHandleImportsWaterHeaterFileWithValidRecords()
        {
            TestValidEquipmentFile("WATER HEATER",
                SampleFiles.SampleFiles.Import.Equipment.WaterHeater.VALID);
        }

        [TestMethod]
        public void TestHandleImportsHydrantFileWithValidRecords()
        {
            TestValidEquipmentFile("HYDRANT",
                SampleFiles.SampleFiles.Import.Equipment.Hydrant.VALID);
        }

        [TestMethod]
        public void TestHandleImportsIndicatorFileWithValidRecords()
        {
            TestValidEquipmentFile("INDICATOR",
                SampleFiles.SampleFiles.Import.Equipment.Indicator.VALID);
        }

        [TestMethod]
        public void TestHandleImportsInstrumentSwitchFileWithValidRecords()
        {
            TestValidEquipmentFile("INSTRUMENT SWITCH",
                SampleFiles.SampleFiles.Import.Equipment.InstrumentSwitch.VALID);
        }

        [TestMethod]
        public void TestHandleImportsKitFileWithValidRecords()
        {
            TestValidEquipmentFile("KIT (SAFETY, REPAIR, HAZWOPR)",
                SampleFiles.SampleFiles.Import.Equipment.Kit.VALID);
        }

        [TestMethod]
        public void TestHandleImportsLabEquipmentFileWithValidRecords()
        {
            TestValidEquipmentFile("LAB EQUIPMENT",
                SampleFiles.SampleFiles.Import.Equipment.LabEquipment.VALID);
        }

        [TestMethod]
        public void TestHandleImportsLeakMonitorFileWithValidRecords()
        {
            TestValidEquipmentFile("LEAK MONITOR",
                SampleFiles.SampleFiles.Import.Equipment.LeakMonitor.VALID);
        }

        [TestMethod]
        public void TestHandleImportsManholeFileWithValidRecords()
        {
            TestValidEquipmentFile("MANHOLE",
                SampleFiles.SampleFiles.Import.Equipment.Manhole.VALID);
        }

        [TestMethod]
        public void TestHandleImportsMixerFileWithValidRecords()
        {
            TestValidEquipmentFile("MIXER",
                SampleFiles.SampleFiles.Import.Equipment.Mixer.VALID);
        }

        [TestMethod]
        public void TestHandleImportsModemFileWithValidRecords()
        {
            TestValidEquipmentFile("MODEM",
                SampleFiles.SampleFiles.Import.Equipment.Modem.VALID);
        }

        [TestMethod]
        public void TestHandleImportsMotorFileWithValidRecords()
        {
            TestValidEquipmentFile("MOTOR",
                SampleFiles.SampleFiles.Import.Equipment.Motor.VALID);
        }

        [TestMethod]
        public void TestHandleImportsMotorContactorFileWithValidRecords()
        {
            TestValidEquipmentFile("MOTOR CONTACTOR",
                SampleFiles.SampleFiles.Import.Equipment.MotorContactor.VALID);
        }

        [TestMethod]
        public void TestHandleImportsMotorStarterFileWithValidRecords()
        {
            TestValidEquipmentFile("MOTOR STARTER",
                SampleFiles.SampleFiles.Import.Equipment.MotorStarter.VALID);
        }

        [TestMethod]
        public void TestHandleImportsNetworkRouterFileWithValidRecords()
        {
            TestValidEquipmentFile("NETWORK ROUTER",
                SampleFiles.SampleFiles.Import.Equipment.NetworkRouter.VALID);
        }

        [TestMethod]
        public void TestHandleImportsNetworkSwitchFileWithValidRecords()
        {
            TestValidEquipmentFile("NETWORK SWITCH",
                SampleFiles.SampleFiles.Import.Equipment.NetworkSwitch.VALID);
        }

        [TestMethod]
        public void TestHandleImportsNonPotableWaterTankFileWithValidRecords()
        {
            TestValidEquipmentFile("NON POTABLE WATER TANK",
                SampleFiles.SampleFiles.Import.Equipment.NonPotableWaterTank.VALID);
        }

        [TestMethod]
        public void TestHandleImportsOperatorComputerTerminalFileWithValidRecords()
        {
            TestValidEquipmentFile("OPERATOR COMPUTER TERMINAL",
                SampleFiles.SampleFiles.Import.Equipment.OperatorComputerTerminal.VALID);
        }

        [TestMethod]
        public void TestHandleImportsPCFileWithValidRecords()
        {
            TestValidEquipmentFile("PC",
                SampleFiles.SampleFiles.Import.Equipment.PC.VALID);
        }

        [TestMethod]
        public void TestHandleImportsPDMToolFileWithValidRecords()
        {
            TestValidEquipmentFile("PDM TOOL",
                SampleFiles.SampleFiles.Import.Equipment.PDMTool.VALID);
        }

        [TestMethod]
        public void TestHandleImportsPhaseConverterFileWithValidRecords()
        {
            TestValidEquipmentFile("PHASE CONVERTER",
                SampleFiles.SampleFiles.Import.Equipment.PhaseConverter.VALID);
        }

        [TestMethod]
        public void TestHandleImportsPlantValveFileWithValidRecords()
        {
            TestValidEquipmentFile("PLANT VALVE",
                SampleFiles.SampleFiles.Import.Equipment.PlantValve.VALID);
        }

        [TestMethod]
        public void TestHandleImportsPotableWaterTankFileWithValidRecords()
        {
            TestValidEquipmentFile("POTABLE WATER TANK",
                SampleFiles.SampleFiles.Import.Equipment.PotableWaterTank.VALID);
        }

        [TestMethod]
        public void TestHandleImportsPowerBreakerFileWithValidRecords()
        {
            TestValidEquipmentFile("POWER BREAKER",
                SampleFiles.SampleFiles.Import.Equipment.PowerBreaker.VALID);
        }

        [TestMethod]
        public void TestHandleImportsPowerConditionerFileWithValidRecords()
        {
            TestValidEquipmentFile("POWER CONDITIONER",
                SampleFiles.SampleFiles.Import.Equipment.PowerConditioner.VALID);
        }

        [TestMethod]
        public void TestHandleImportsPowerDisconnectFileWithValidRecords()
        {
            TestValidEquipmentFile("POWER DISCONNECT",
                SampleFiles.SampleFiles.Import.Equipment.PowerDisconnect.VALID);
        }

        [TestMethod]
        public void TestHandleImportsPowerFeederCableFileWithValidRecords()
        {
            TestValidEquipmentFile("POWER FEEDER CABLE",
                SampleFiles.SampleFiles.Import.Equipment.PowerFeederCable.VALID);
        }

        [TestMethod]
        public void TestHandleImportsPowerMonitorFileWithValidRecords()
        {
            TestValidEquipmentFile("POWER MONITOR",
                SampleFiles.SampleFiles.Import.Equipment.PowerMonitor.VALID);
        }

        [TestMethod]
        public void TestHandleImportsPowerPanelFileWithValidRecords()
        {
            TestValidEquipmentFile("POWER PANEL",
                SampleFiles.SampleFiles.Import.Equipment.PowerPanel.VALID);
        }

        [TestMethod]
        public void TestHandleImportsPowerRelayFileWithValidRecords()
        {
            TestValidEquipmentFile("POWER RELAY",
                SampleFiles.SampleFiles.Import.Equipment.PowerRelay.VALID);
        }

        [TestMethod]
        public void TestHandleImportsPowerSurgeProtectionFileWithValidRecords()
        {
            TestValidEquipmentFile("POWER SURGE PROTECTION",
                SampleFiles.SampleFiles.Import.Equipment.PowerSurgeProtection.VALID);
        }

        [TestMethod]
        public void TestHandleImportsPowerTransferSwitchFileWithValidRecords()
        {
            TestValidEquipmentFile("POWER TRANSFER SWITCH",
                SampleFiles.SampleFiles.Import.Equipment.PowerTransferSwitch.VALID);
        }

        [TestMethod]
        public void TestHandleImportsPressureDamperFileWithValidRecords()
        {
            TestValidEquipmentFile("PRESSURE DAMPER",
                SampleFiles.SampleFiles.Import.Equipment.PressureDamper.VALID);
        }

        [TestMethod]
        public void TestHandleImportsPrinterFileWithValidRecords()
        {
            TestValidEquipmentFile("PRINTER",
                SampleFiles.SampleFiles.Import.Equipment.Printer.VALID);
        }

        [TestMethod]
        public void TestHandleImportsPumpCentrifugalFileWithValidRecords()
        {
            TestValidEquipmentFile("PUMP CENTRIFUGAL",
                SampleFiles.SampleFiles.Import.Equipment.PumpCentrifugal.VALID);
        }

        [TestMethod]
        public void TestHandleImportsPumpGrinderFileWithValidRecords()
        {
            TestValidEquipmentFile("PUMP GRINDER",
                SampleFiles.SampleFiles.Import.Equipment.PumpGrinder.VALID);
        }

        [TestMethod]
        public void TestHandleImportsPumpPositiveDisplacementFileWithValidRecords()
        {
            TestValidEquipmentFile("PUMP POSITIVE DISPLACEMENT",
                SampleFiles.SampleFiles.Import.Equipment.PumpPositiveDisplacement.VALID);
        }

        [TestMethod]
        public void TestHandleImportsRecorderFileWithValidRecords()
        {
            TestValidEquipmentFile("RECORDER",
                SampleFiles.SampleFiles.Import.Equipment.Recorder.VALID);
        }

        [TestMethod]
        public void TestHandleImportsRespiratorFileWithValidRecords()
        {
            TestValidEquipmentFile("RESPIRATOR",
                SampleFiles.SampleFiles.Import.Equipment.Respirator.VALID);
        }

        [TestMethod]
        public void TestHandleImportsRTUPLCFileWithValidRecords()
        {
            TestValidEquipmentFile("RTU - PLC",
                SampleFiles.SampleFiles.Import.Equipment.RTUPLC.VALID);
        }

        [TestMethod]
        public void TestHandleImportsSafetyShowerFileWithValidRecords()
        {
            TestValidEquipmentFile("SAFETY SHOWER",
                SampleFiles.SampleFiles.Import.Equipment.SafetyShower.VALID);
        }

        [TestMethod]
        public void TestHandleImportsSCADARadioFileWithValidRecords()
        {
            TestValidEquipmentFile("SCADA RADIO",
                SampleFiles.SampleFiles.Import.Equipment.SCADARadio.VALID);
        }

        [TestMethod]
        public void TestHandleImportsSCADASystemGenFileWithValidRecords()
        {
            TestValidEquipmentFile("SCADA SYSTEM GEN",
                SampleFiles.SampleFiles.Import.Equipment.SCADASystemGen.VALID);
        }

        [TestMethod]
        public void TestHandleImportsScaleFileWithValidRecords()
        {
            TestValidEquipmentFile("SCALE",
                SampleFiles.SampleFiles.Import.Equipment.Scale.VALID);
        }

        [TestMethod]
        public void TestHandleImportsScreenFileWithValidRecords()
        {
            TestValidEquipmentFile("SCREEN",
                SampleFiles.SampleFiles.Import.Equipment.Screen.VALID);
        }

        [TestMethod]
        public void TestHandleImportsScrubberFileWithValidRecords()
        {
            TestValidEquipmentFile("SCRUBBER",
                SampleFiles.SampleFiles.Import.Equipment.Scrubber.VALID);
        }

        [TestMethod]
        public void TestHandleImportsSecondaryContainmentFileWithValidRecords()
        {
            TestValidEquipmentFile("SECONDARY CONTAINMENT",
                SampleFiles.SampleFiles.Import.Equipment.SecondaryContainment.VALID);
        }

        [TestMethod]
        public void TestHandleImportsSecuritySystemFileWithValidRecords()
        {
            TestValidEquipmentFile("SECURITY SYSTEM",
                SampleFiles.SampleFiles.Import.Equipment.SecuritySystem.VALID);
        }

        [TestMethod]
        public void TestHandleImportsServerFileWithValidRecords()
        {
            TestValidEquipmentFile("SERVER",
                SampleFiles.SampleFiles.Import.Equipment.Server.VALID);
        }

        [TestMethod]
        public void TestHandleImportsSoftenerFileWithValidRecords()
        {
            TestValidEquipmentFile("SOFTENER",
                SampleFiles.SampleFiles.Import.Equipment.Softener.VALID);
        }

        [TestMethod]
        public void TestHandleImportsStreetValveFileWithValidRecords()
        {
            TestValidEquipmentFile("STREET VALVE",
                SampleFiles.SampleFiles.Import.Equipment.StreetValve.VALID);
        }

        [TestMethod]
        public void TestHandleImportsTelephoneFileWithValidRecords()
        {
            TestValidEquipmentFile("TELEPHONE",
                SampleFiles.SampleFiles.Import.Equipment.Telephone.VALID);
        }

        [TestMethod]
        public void TestHandleImportsToolFileWithValidRecords()
        {
            TestValidEquipmentFile("TOOL",
                SampleFiles.SampleFiles.Import.Equipment.Tool.VALID);
        }

        [TestMethod]
        public void TestHandleImportsTransformerFileWithValidRecords()
        {
            TestValidEquipmentFile("TRANSFORMER",
                SampleFiles.SampleFiles.Import.Equipment.Transformer.VALID);
        }

        [TestMethod]
        public void TestHandleImportsTransmitterFileWithValidRecords()
        {
            TestValidEquipmentFile("TRANSMITTER",
                SampleFiles.SampleFiles.Import.Equipment.Transmitter.VALID);
        }

        [TestMethod]
        public void TestHandleImportsUninteruptedPowerSupplyFileWithValidRecords()
        {
            TestValidEquipmentFile("UNINTERUPTED POWER SUPPLY",
                SampleFiles.SampleFiles.Import.Equipment.UninteruptedPowerSupply.VALID);
        }

        [TestMethod]
        public void TestHandleImportsUVSanitizerFileWithValidRecords()
        {
            TestValidEquipmentFile("UV SANITIZER",
                SampleFiles.SampleFiles.Import.Equipment.UVSanitizer.VALID);
        }

        [TestMethod]
        public void TestHandleImportsVehicleFileWithValidRecords()
        {
            TestValidEquipmentFile("VEHICLE",
                SampleFiles.SampleFiles.Import.Equipment.Vehicle.VALID);
        }

        [TestMethod]
        public void TestHandleImportsVOCStripperFileWithValidRecords()
        {
            TestValidEquipmentFile("VOC STRIPPER",
                SampleFiles.SampleFiles.Import.Equipment.VOCStripper.VALID);
        }

        [TestMethod]
        public void TestHandleImportsWasteTankFileWithValidRecords()
        {
            TestValidEquipmentFile("WASTE TANK",
                SampleFiles.SampleFiles.Import.Equipment.WasteTank.VALID);
        }

        [TestMethod]
        public void TestHandleImportsWaterQualityAnalyzerFileWithValidRecords()
        {
            TestValidEquipmentFile("WATER QUALITY ANALYZER",
                SampleFiles.SampleFiles.Import.Equipment.WaterQualityAnalyzer.VALID);
        }

        [TestMethod]
        public void TestHandleImportsWaterTreatmentContactorFileWithValidRecords()
        {
            TestValidEquipmentFile("WATER TREATMENT CONTACTOR",
                SampleFiles.SampleFiles.Import.Equipment.WaterTreatmentContactor.VALID);
        }

        [TestMethod]
        public void TestHandleImportsWellFileWithValidRecords()
        {
            TestValidEquipmentFile("WELL",
                SampleFiles.SampleFiles.Import.Equipment.Well.VALID);
        }

        #endregion

        #region Services

        [TestMethod]
        public void TestHandleImportsServicesFileWithValidRecords()
        {
            TestValidFile<Service>(TestDataHelper.CreateStuffForServicesInAberdeenNJ, 4, SampleFiles.SampleFiles.Import.Services.VALID);
        }

        #endregion

        #region Sewer Openings

        [TestMethod]
        public void TestHandleImportsSewerOpeningsFileWithValidRecords()
        {
            TestValidFile<SewerOpening>(TestDataHelper.CreateStuffForSewerOpeningsInAberdeenNJ, 4, SampleFiles.SampleFiles.Import.SewerOpenings.VALID);

            WithUnitOfWork(uow => {
                foreach (var openingNumber in new[] { "MAB-808", "MAB-838", "MAB-839", "MAB-840" })
                {
                    Assert.AreEqual(1, uow.SqlQuery("SELECT 1 FROM SewerOpenings WHERE OpeningNumber = :openingNumber")
                                          .SetString("openingNumber", openingNumber)
                                          .SafeUniqueIntResult());
                }
            });
        }

        #endregion

        #region UpdateEquipmentRiskCharacteristics

        [TestMethod]
        public void TestHandleImportsEquipmentRiskCharacteristicsFileWithValidRecords()
        {
            TestValidFile<Equipment>(TestDataHelper.CreateSomeAdjustableSpeedDrivesInAberdeenNJ, 0,
                SampleFiles.SampleFiles.Update.EquipmentRiskCharacteristics.VALID);
        }

        #endregion

        #region UpdateEquipmentInstallationInfo

        [TestMethod]
        public void TestHandleImportsEquipmentInstallationInfoFileWithValidRecords()
        {
            TestValidFile<Equipment>(TestDataHelper.CreateSomeAdjustableSpeedDrivesInAberdeenNJ, 0,
                SampleFiles.SampleFiles.Update.EquipmentInstallationInfo.VALID);
        }

        #endregion

        #region UpdateFacilityRiskCharacteristics

        [TestMethod]
        public void TestHandleImportsFacilityRiskCharacteristicsFileWithValidRecords()
        {
            TestValidFile<Facility>(TestDataHelper.CreateSomeFacilitiesInAberdeenNJ, 0,
                SampleFiles.SampleFiles.Update.FacilityRiskCharacteristics.VALID);
        }

        #endregion

        #region UpdateFacilityBooleanAttributes

        [TestMethod]
        public void TestHandleImportsFacilityBooleanAttributesFileWithValidRecords()
        {
            TestValidFile<Facility>(TestDataHelper.CreateSomeFacilitiesInAberdeenNJ, 0,
                SampleFiles.SampleFiles.Update.FacilityBooleanAttributes.VALID);
        }

        #endregion
    }
}
