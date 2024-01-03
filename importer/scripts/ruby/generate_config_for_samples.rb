# <Content Include="..\..\doc\Samples\Equipment\AdjustableSpeedDrive\Valid Adjustable Speed Drives.xlsx">
#     <Link>SampleFiles\Equipment\AdjustableSpeedDrive\Valid Adjustable Speed Drives.xlsx</Link>
#     <CopyToOutputDirectory>Always</CopyToOutputDirectory>
# </Content>

def fix_separators file
  file.gsub('/', '\\')
end

def physical_path file
  fix_separators file.sub('/solutions/mapcall-importer', '../..')
end

def link_path file
  fix_separators file.sub('/solutions/mapcall-importer/doc/Samples', 'SampleFiles')
end

Dir.glob('/solutions/mapcall-importer/doc/Samples/Equipment/*/Valid*.xlsx') do |file|
  puts "    <Content Include=\"#{physical_path(file)}\">"
  puts "      <Link>#{link_path(file)}</Link>"
  puts "      <CopyToOutputDirectory>Always</CopyToOutputDirectory>"
  puts "    </Content>"
end
