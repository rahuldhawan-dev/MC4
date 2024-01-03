require 'tiny_tds'

client = TinyTds::Client.new username: 'sa', password: 'mapcall#1', host: 'localhost', port: 1433, database: 'MapCallDev'

duplicates_qry = %{
select Identifier
from Equipment
where convert(date, CreatedAt) in ('2018-04-26', '2018-03-05')
group by Identifier
having count(Identifier) > 1
}

duplicates = []

def find_next_number client, identifier
  base = identifier.sub(/-\d+$/, '')

  client.execute("select max(Number) from Equipment where Identifier like '#{base}-%'").each[0][''].to_i + 1
end

client.execute(duplicates_qry).each do |value|
  duplicates << value['Identifier']
end

puts "Found #{duplicates.length} duplicate identifiers"

duplicates.each do |dup|
  duplicate_ids = []

  client.execute("select EquipmentId from Equipment where Identifier = '#{dup}'").each do |id|
    duplicate_ids << id['EquipmentId'].to_i
  end

  puts "Found #{duplicate_ids.length} records with Identifier '#{dup}'"

  duplicate_ids.drop(1).each do |dup_id|
    num = find_next_number client, dup

    new_identifier = dup.sub(/-\d+$/, '-' + num.to_s)

    client.execute("UPDATE Equipment SET Number = #{num}, Identifier = '#{new_identifier}' WHERE EquipmentId = #{dup_id}").do
  end
end
