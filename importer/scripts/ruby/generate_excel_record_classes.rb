#              public class AMIDATACOLLExcelRecord : EquipmentExcelRecordBase<AMIDATACOLLExcelRecord>
#              {
#                #region Constants

#                public const int SAP_EQUIPMENT_TYPE = -1,
#                                 EQUIPMENT_TYPE = -1;

#                #endregion

#                #region Properties

#                protected override int SAPEquipmentTypeId => SAP_EQUIPMENT_TYPE;

#                protected override string SAPEquipmentType => throw new System.NotImplementedException();

#                protected override int EquipmentTypeId => EQUIPMENT_TYPE;

#                #endregion

#                #region Private Methods

#                protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(EquipmentCharacteristicMapper mapper)
#                {
#                  throw new System.NotImplementedException();
#                }

#                #endregion
#              }

def indent str, times
  puts (' ' * 4 * times) + str
end

puts 'namespace MapCallImporter.Models.Equipment'
puts '{'

Dir.glob('/solutions/mapcall-importer/doc/Samples/Equipment/*') do |file|
  file = File.basename file

  indent "public class #{file}ExcelRecord : EquipmentExcelRecordBase<#{file}ExcelRecord>", 1
  indent '{', 1
  indent "#region Constants", 2
  puts ''
  indent "public const int SAP_EQUIPMENT_TYPE = -1,", 2
  indent "EQUIPMENT_TYPE = -1;", 2
  puts ''
  indent "#endregion", 2
  puts ''
  indent "#region Properties", 2
  puts ''
  indent "protected override int SAPEquipmentTypeId => SAP_EQUIPMENT_TYPE;", 2
  puts ''
  indent "protected override string SAPEquipmentType => throw new System.NotImplementedException();", 2
  puts ''
  indent "protected override int EquipmentTypeId => EQUIPMENT_TYPE;", 2
  puts ''
  indent "#endregion", 2
  puts ''
  indent "#region Private Methods", 2
  puts ''
  indent "protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(EquipmentCharacteristicMapper mapper)", 2
  indent '{', 2
  indent "throw new System.NotImplementedException();", 3
  indent '}', 2
  puts ''
  indent "#endregion", 2
  indent '}', 1
end

puts '}'
