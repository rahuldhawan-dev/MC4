def get_equipment_type f
  f.to_s.sub(/^.+\/([^\/]+)$/, '\1')
end

new = Dir.glob('/solutions/mapcall-importer/doc/Samples/Equipment/*').map { |f| get_equipment_type f }
old = Dir.glob('/solutions/mapcall-importer/doc/Samples/Old_Equipment/*').map { |f| get_equipment_type f }

new_not_in_old = new.select { |f| !old.include? f }
old_not_in_new = old.select { |f| !new.include? f }

puts "New not in old:"
puts new_not_in_old.inspect

puts '======================================================='

puts "Old not in new:"
puts old_not_in_new.inspect
