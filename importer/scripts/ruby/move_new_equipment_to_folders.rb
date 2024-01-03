require 'fileutils'

Dir.glob('/solutions/mapcall-importer/doc/Samples/New Equipment/*.xlsx').each do |f|
  dir = f.sub '.xlsx', ''
  puts "creatig directory #{dir}"
  FileUtils.mkdir dir

  puts "moving #{f} to #{dir}/Originally Provided Sample.xlsx"
  FileUtils.mv f, File.join(dir, 'Originally Provided Sample.xlsx')
end
