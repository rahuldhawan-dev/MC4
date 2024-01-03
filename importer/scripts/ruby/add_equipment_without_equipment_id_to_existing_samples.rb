require 'rubyXL'

Dir.glob('/solutions/mapcall-importer/doc/Samples/Equipment/*/Valid*.xlsx').each do |f|
  workbook = RubyXL::Parser.parse f
  sheet = workbook.worksheets[0]

  (1..4).each do |idx|
    sheet.add_cell 4 + idx, 0, ''

    row = sheet[idx]
    row.cells[1..row.cells.size].each_with_index do |cell, cell_idx|
      sheet.add_cell 4 + idx, cell_idx + 1, cell ? cell.value : ''
    end
  end

  workbook.write f
end
