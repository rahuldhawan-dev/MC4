require 'rubyXL'

doc_path = File.expand_path(File.join('..', '..', 'doc'))
equipment_samples_glob_base = File.join doc_path, 'Samples', 'Equipment', '*', 'Valid *.xlsx'

def gather_columns worksheet
  worksheet[0].cells.map { |c| c.value}
end

new_workbook = RubyXL::Workbook.new
new_worksheet = new_workbook.worksheets[0]
file_i = 0

Dir.glob(equipment_samples_glob_base) do |f|
  workbook = RubyXL::Parser.parse f
  worksheet = workbook.worksheets[0]
  type = File.basename(File.dirname(f))
  columns = gather_columns worksheet

  new_worksheet.insert_cell file_i, 0, type

  columns.each_with_index do |column, idx|
    new_worksheet.insert_cell file_i + 1, idx, column
  end

  file_i += 2

  puts '=' * 20
  puts "Type #{type} has the following columns:"
  puts columns.inspect
end

new_workbook.write '/users/duncanj/documents/Column Names For Jim.xlsx'
