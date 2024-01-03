# Go through each folder under doc/Samples/Equipment, each containing an "Originally Provided Sample.xlsx"
# file.  Generate a new file called "Valid <EquipmentType>s.xlsx" containing the top 4 rows of the
# originally provided sample.  FunctionalLocation, PlanningPlant, and CreatedBy are all munged to use data
# which will be present in the test database.

require 'rubyXL'
require 'active_support/inflector'

def valid_filename f
  name = f.to_s.sub(/^.+\/([^\/]+)$/, '\1')
  parts = name =~ /^[A-Z]+$/ ? [name] : name.scan(/(?:[A-Z][a-z]+|[A-Z][A-Z]+(?![a-z]))/)
  idx = parts.size == 1 ? 0 : -1
  parts[idx] = parts[idx].pluralize
  "Valid #{parts.join ' '}.xlsx"
end

class ValueMunger
  def munge header, cell
    munge_table.keys.include?(header.to_sym) ?
      munge_table[header.to_sym] :
      cell && cell.value
  end

  private

  def munge_table
    @munge_table ||= {
      'Functional Loc.': 'NJMM-MM-ABTNK-MCC',
      'Planning plant': 'P218',
      'Created by': 'mcadmin'
    }
  end
end

class ValidFileCopier
  ROW_COUNT = 4

  def initialize from_sheet, to_sheet, rows, columns
    @from_sheet = from_sheet
    @to_sheet = to_sheet
    @rows = rows
    @columns = columns
    @munger = ValueMunger.new
  end

  def copy_and_munge
    take = rows > ROW_COUNT ? ROW_COUNT : rows
    generate = rows > (ROW_COUNT - 1) ? 0 : ROW_COUNT - rows
    headers = copy_headers from_sheet, to_sheet

    puts "Taking #{take} rows, generating #{generate}"

    copy_rows from_sheet, to_sheet, take, headers

    generate_rows from_sheet, to_sheet, generate, headers
  end

  attr_reader :from_sheet, :to_sheet, :rows, :columns, :munger

  private

  def generate_rows from_sheet, to_sheet, generate, headers
    return if generate == 0

    copy_row = from_sheet[ROW_COUNT - generate]
    start_id = copy_row[0].value.to_i

    (1..generate).each do |idx|
      row_idx = ROW_COUNT - (generate - idx)

      to_sheet.add_cell row_idx, 0, (start_id + idx).to_s

      copy_row.cells[1..copy_row.cells.size].each_with_index do |cell, cell_idx|
        copy_cell to_sheet, row_idx, cell_idx + 1, headers, cell
      end
    end
  end

  def copy_rows from_sheet, to_sheet, take, headers
    from_sheet[1..take].each_with_index do |row, row_idx|
      row && row.cells.each_with_index do |cell, cell_idx|
        copy_cell to_sheet, row_idx + 1, cell_idx, headers, cell
      end
    end
  end

  def copy_cell to_sheet, row_idx, cell_idx, headers, cell
    value = munger.munge headers[cell_idx], cell
    to_sheet.add_cell row_idx, cell_idx, value
  end

  def copy_headers from_sheet, to_sheet
    headers = []

    from_sheet[0].cells.each_with_index do |cell, idx|
      headers.push cell.value
      to_sheet.add_cell 0, idx, cell.value
    end

    headers
  end
end

class ValidFileGenerator
  def initialize original, valid
    @original = original
    @valid = valid
  end

  def generate
    puts ('=' * 80)
    puts "Generating #{valid} from #{original}"
    from_workbook = RubyXL::Parser.parse original
    from_sheet = from_workbook.worksheets[0]
    to_workbook = RubyXL::Workbook.new
    to_sheet = to_workbook.worksheets[0]

    validate from_sheet
    rows = count_rows from_sheet
    columns = count_columns from_sheet

    puts "Found #{rows} rows and #{columns} columns with data"

    copy_and_munge from_sheet, to_sheet, rows, columns
    to_workbook.write valid

    puts ''
  end

  attr_reader :valid, :original

  private

  def copy_and_munge from_sheet, to_sheet, rows, columns
    ValidFileCopier.new(from_sheet, to_sheet, rows, columns).copy_and_munge
  end

  def count_rows from_sheet
    from_sheet.each_with_index do |row, idx|
      return idx - 1 if row.nil? || row[0].nil? || row[0].value == ''
    end

    from_sheet.count - 1
  end

  def count_columns from_sheet
    row = from_sheet[0]

    row.cells.each_with_index do |cell, idx|
      return idx if cell.nil? || cell.value == ''
    end

    row.cells.count
  end

  def validate from_sheet
    first_header = from_sheet[0][0].value
    raise "Original file #{original} does not start with 'Equipment' (found '#{first_header}' instead)" if first_header != 'Equipment'
  end
end

Dir.glob('/solutions/mapcall-importer/doc/Samples/Equipment/*').each do |f|
  original = File.join f, 'Originally Provided Sample.xlsx'
  valid = File.join f, valid_filename(f)

  ValidFileGenerator.new(original, valid).generate
end
