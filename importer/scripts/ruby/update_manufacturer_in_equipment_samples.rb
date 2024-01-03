require 'rubyXL'

def map_equipment_file paths
  paths.map do |path|
    {
      type: path.sub(/^.+\/([^\/]+)\/[^\/]+$/, '\1'),
      path: path
    }
  end
end

def fix_sheet_name name
  name.gsub!(/\s+/, '')

  case name
  when 'EmergencyGenerators'
    'EmergencyGenerator'
  when 'ChemicalGenerators'
    'ChemicalGenerator'
  when 'Facility&Grounds'
    'FacilityAndGrounds'
  else
    name
  end
end

def insert_mfr_column to_path, from_sheet, max_rows = nil
  to_workbook = RubyXL::Parser.parse to_path
  sheet = to_workbook.worksheets[0]
  sheet.insert_column 1

  from_sheet.each_with_index do |row, idx|
    value = row.cells[1].value
    if idx == 0
      value = 'Manufacturer'
    elsif !value
      puts "No value found at #{idx}:1, breaking"
      break
    end

    break if max_rows && idx == max_rows

    puts "Setting #{to_path} cell #{idx}:1 to value '#{value}'"

    sheet.insert_cell idx, 1, value
  end

  to_workbook.save
end

doc_path = File.expand_path(File.join('..', '..', 'doc'))
equipment_glob_base = File.join doc_path, 'Samples', 'Equipment', '*'
new_file = File.join doc_path, 'NJ 3 Updated.xlsx'
valid = map_equipment_file(Dir.glob(File.join(equipment_glob_base, 'Valid *.xlsx')))
original = map_equipment_file(Dir.glob(File.join(equipment_glob_base, 'Originally Provided Sample.xlsx')))
second = map_equipment_file(Dir.glob(File.join(equipment_glob_base, 'Second Provided Sample.xlsx')))

new_workbook = RubyXL::Parser.parse new_file

new_workbook.worksheets.each do |sheet|
  sheet_name = fix_sheet_name sheet.sheet_name
  next if ['Overview', 'SAPVlookup'].include? sheet_name

  v = valid.select { |f| f[:type] == sheet_name }.first
  o = original.select { |f| f[:type] == sheet_name }.first
  s = second.select { |f| f[:type] == sheet_name }.first

  puts "#{sheet_name}:"
  if !v
    raise 'valid file could not be found'
  end
  if !o
    raise 'original file could not be found'
  end

  insert_mfr_column v[:path], sheet, 5
  insert_mfr_column o[:path], sheet

  if !s
    puts 'no secondary file found'
  else
    insert_mfr_column s[:path], sheet
  end
end
