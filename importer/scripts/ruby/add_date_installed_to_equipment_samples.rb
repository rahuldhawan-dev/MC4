require 'rubyXL'

doc_path = File.expand_path(File.join('..', '..', 'doc'))
equipment_glob_base = File.join doc_path, 'Samples', 'Import', 'Equipment', '*', 'Valid *.xlsx'

def determine_new_col_index worksheet
  worksheet[0].cells.length
end

Dir.glob(equipment_glob_base) do |f|
  puts "Considering file #{f}..."
  
  workbook = RubyXL::Parser.parse f
  worksheet = workbook.worksheets[0]

  new_col_index = determine_new_col_index worksheet

  worksheet.add_cell 0, new_col_index, 'Date Installed'

  workbook.write f
end
