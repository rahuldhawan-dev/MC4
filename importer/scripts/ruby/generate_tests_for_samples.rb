# [TestMethod]
# public void TestHandleImportsAdjustableSpeedDriveFileWithValidRecords()
# {
#   TestValidEquipmentFile("ADJUSTABLE SPEED DRIVE",
#                          SampleFiles.Equipment.AdjustableSpeedDrive.VALID);
# }

def indent str, times
  puts (' ' * 4 * times) + str
end

def type_name file
  /Equipment\/([^\/]+)\/Valid/.match(file)[1]
end

def upcase_type_name file
  parts = type_name(file).scan(/(?:[A-Z][a-z]+|[A-Z][A-Z]+(?![a-z]))/)
  parts.join(' ').upcase
end

Dir.glob('/solutions/mapcall-importer/doc/Samples/Equipment/*/Valid*.xlsx') do |file|
  indent "[TestMethod]", 2
  indent "public void TestHandleImports#{type_name(file)}FileWithValidRecords()", 2
  indent '{', 2
  indent "TestValidEquipmentFile(\"#{upcase_type_name(file)}\",", 3
  indent "SampleFiles.Equipment.#{type_name(file)}.VALID);", 4
  indent '}', 2
  puts
end
