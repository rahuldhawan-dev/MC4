require 'rubyXL'

root_path = File.expand_path(File.join('..', '..'))
doc_path = File.join(root_path, 'doc')
equipment_doc_glob_base = File.join doc_path, 'Samples', 'Equipment', '*', 'Valid *.xlsx'

def find_functional_loc_column worksheet
  worksheet[0].cells.each_with_index do |cell, idx|
    return idx if cell.value == 'Functional Loc.'
  end
end

Dir.glob(equipment_doc_glob_base) do |f|
  workbook = RubyXL::Parser.parse f
  worksheet = workbook.worksheets[0]

  old_idx = find_functional_loc_column worksheet
  new_idx = worksheet[0].cells.length

  worksheet.insert_column new_idx
  worksheet.add_cell 0, new_idx, 'Facility MC'

  (1..4).each do |i|
    worksheet.add_cell i, new_idx, worksheet[i].cells[old_idx].value
  end

  workbook.write f
end
