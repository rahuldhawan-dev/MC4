    # [TestClass]
    # public class #{type}ExcelRecordTest : EquipmentExcelRecordTestBase<#{type}ExcelRecord>
    # {
    #     #region Private Methods

    #     protected override #{type}ExcelRecord CreateTarget()
    #     {
    #         var ret = base.CreateTarget();

    #         return ret;
    #     }

    #     #endregion

    #     #region Init/Cleanup

    #     [TestInitialize]
    #     public void TestInitialize()
    #     {
    #         BaseTestInitialize();
    #     }

    #     [TestCleanup]
    #     public void TestCleanup()
    #     {
    #         BaseTestCleanup();
    #     }

    #     #endregion

    #     protected override string ExpectedIdentifier => throw new NotImplementedException();

    #     #region Mapping

    #     protected override void TestCharacteristicMappings(
    #         EquipmentCharacteristicMappingTester<#{type}ExcelRecord> test)
    #     {
    #        Assert.Inconclusive("test not yet written");
    #     }

    #     [TestMethod]
    #     public override void TestMappings()
    #     {
    #         base.TestMappings();
    #     }

    #     #endregion
    # }

TYPES = [
  'Aerator',
  'AirVacuumTank',
  'AMIDATACOLL',
  'ArcFlashProtection',
  'BlowOffValve',
  'Boiler',
  'Burner',
  'CalibrationDevice',
  'CathodicProtection',
  'ChemicalDryFeeder',
  'ChemicalGasFeeder',
  'CleanOut',
  'CollectionSystemGeneral',
  'Controller',
  'ControlPanel',
  'Conveyor',
  'CoolingTower',
  'Dam',
  'Defibrillator',
  'DistributionSystem',
  'DistributionTool',
  'Elevator',
  'EmergencyLight',
  'Eyewash',
  'FireExtinguisher',
  'Firewall',
  'FloatationDevice',
  'FlowWeir',
  'Gearbox',
  'GravitySewerMain',
  'Grinder',
  'HeatExchanger',
  'Hoist',
  'HVACChiller',
  'HVACCombinationUnit',
  'Hydrant',
  'Indicator',
  'Kit',
  'LabEquipment',
  'LeakMonitor',
  'Manhole',
  'Mixer',
  'Modem',
  'MotorContactor',
  'NetworkRouter',
  'NetworkSwitch',
  'OperatorComputerTerminal',
  'PDMTool',
  'PhaseConverter',
  'PowerConditioner',
  'PowerFeederCable',
  'PowerPanel',
  'PowerRelay',
  'PressureDamper',
  'Printer',
  'Recorder',
  'Respirator',
  'SafetyShower',
  'SCADASystemGen',
  'Scale',
  'Screen',
  'Scrubber',
  'Server',
  'Softener',
  'Telephone',
  'Tool',
  'UninteruptedPowerSupply',
  'UVSanitizer',
  'Vehicle',
  'WasteTank',
  'WaterHeater',
  'WaterQualityAnalyzer',
  'WaterTreatmentContactor',
]

def indent str, times
  puts (' ' * 4 * times) + str
end

puts 'namespace MapCallImporter.Tests.Models.Equipment'
puts '{'

TYPES.each do |type|
indent "[TestClass]", 1
indent "public class #{type}ExcelRecordTest : EquipmentExcelRecordTestBase<#{type}ExcelRecord>", 1
indent "{", 1
indent "#region Private Methods", 2
puts ''
indent "protected override #{type}ExcelRecord CreateTarget()", 2
indent "{", 2
indent "var ret = base.CreateTarget();", 3
puts ''
indent "return ret;", 3
indent "}", 2
puts ''
indent "#endregion", 2
puts ''
indent "#region Init/Cleanup", 2
puts ''
indent "[TestInitialize]", 2
indent "public void TestInitialize()", 2
indent "{", 2
indent "BaseTestInitialize();", 3
indent "}", 2
puts ''
indent "[TestCleanup]", 2
indent "public void TestCleanup()", 2
indent "{", 2
indent "BaseTestCleanup();", 3
indent "}", 2
puts ''
indent "#endregion", 2
puts ''
indent "#region Init/Cleanup", 2
puts ''
indent "protected override string ExpectedIdentifier => throw new NotImplementedException();", 2
puts ''
indent '#endregion', 2
puts ''
indent "#region Mapping", 2
puts ''
indent "protected override void TestCharacteristicMappings(", 2
indent "EquipmentCharacteristicMappingTester<#{type}ExcelRecord> test)", 3
indent "{", 2
indent "Assert.Inconclusive(\"test not yet written\");", 3
indent "}", 2
puts ''
indent "[TestMethod]", 2
indent "public override void TestMappings()", 2
indent "{", 2
indent "base.TestMappings();", 3
indent "}", 2
puts ''
indent "#endregion", 2
indent "}", 1
end

puts '}'
