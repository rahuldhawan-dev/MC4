# public struct Equipment
# {
#   public struct AdjustableSpeedDrive
#   {
#     public const string VALID =
#                         "SampleFiles\\Equipment\\AdjustableSpeedDrive\\Valid Adjustable Speed Drives.xlsx";
#   }
#
# ...
#
# }

def indent str, times
  puts (' ' * 4 * times) + str
end

def type_name file
  /Equipment\/([^\/]+)\/Valid/.match(file)[1]
end

def path file
  file.sub('/solutions/mapcall-importer/doc/Samples', 'SampleFiles').gsub('/', '\\\\\\\\')
end

indent 'public struct Equipment', 2
indent '{', 2

Dir.glob('/solutions/mapcall-importer/doc/Samples/Equipment/*/Valid*.xlsx') do |file|
  indent "public struct #{type_name(file)}", 3
  indent '{', 3
  indent "public const string VALID = \"#{path(file)}\";", 4
  indent '}', 3
  puts
end

indent '}', 2
