require 'rubyXL'

root_path = File.expand_path(File.join('..', '..'))
doc_path = File.join(root_path, 'doc')
equipment_doc_glob_base = File.join doc_path, 'Samples', 'Equipment', '*', 'Valid *.xlsx'
equipment_models_path = File.join root_path, 'src', 'MapCallImporter.Core', 'Models', 'Equipment'

skip_columns = [
  'Equipment',
  'Manufacturer',
  'Description',
  'Functional Loc.',
  'Planning plant',
  'User status',
  'System status',
  'Created by',
  'ABC indic.',
  'ManufSerialNo.',
  'Model number',
  'NARUC Special Mtn Note',
  'NARUC Special Mtn Note Det',
]

def clean_column_name column_name
  [" ", ".", "#", "/", "'", "(", ")", "-", "?", "%", "&"].each { |c| column_name = column_name.gsub c, '' }
  column_name
end

def gather_column_names worksheet, skip_columns
  worksheet[0].cells
    .map { |c| c.value }
    .reject { |v| skip_columns.include? v }
    .map { |v| clean_column_name v }
end

def gather_fields model_path
  ret = []

  File.readlines(model_path).each do |line|
    match = /public [^ ]+ ([^ ]+) {/.match line

    ret << match[1] if match
  end

  ret
end

Dir.glob(equipment_doc_glob_base) do |f|
  workbook = RubyXL::Parser.parse f
  worksheet = workbook.worksheets[0]
  model_name = File.basename(File.dirname(f)) + 'ExcelRecord.cs'
  model_path = File.join equipment_models_path, model_name

  columns = gather_column_names worksheet, skip_columns

  raise "Missing model #{model_name} at path #{model_path}" if !File.exist? model_path

  fields = gather_fields model_path

  missing_fields = columns.reject { |c| fields.include? c }

  if !missing_fields.empty?
    puts "Model #{model_name} is missing the following fields:"
    puts missing_fields.inspect
    puts '=' * 20
  end
end
