require 'tiny_tds'

#client = TinyTds::Client.new username: 'sa', password: 'mapcall#1', host: 'localhost', port: 1433, database: 'MapCallDev'
client = TinyTds::Client.new username: '', password: '', host: 'hsynwmaps002.amwaternp.net', port: 1433, database: 'MapCallQA'

qry = %{
select Description + ';' + cast(SAPEquipmentManufacturerID as varchar)
from EquipmentModels
where SAPEquipmentManufacturerID IS NOT NULL
group by Description + ';' + cast(SAPEquipmentManufacturerID as varchar)
having count(Description + ';' + cast(SAPEquipmentManufacturerID as varchar)) > 1
order by count(Description + ';' + cast(SAPEquipmentManufacturerID as varchar)) desc
}

values = []

client.execute(qry).each do |value|
  value = value[''].split(';')
  values.push({desc: value[0], mfr_id: value[1].to_i})
end

values.each do |value|
  mfr = client.execute("select Description from SAPEquipmentManufacturers where Id = #{value[:mfr_id]}").each[0]['Description']

  count = client.execute("select count(1) from EquipmentModels where Description = '#{value[:desc]}' and SAPEquipmentManufacturerID = #{value[:mfr_id]}").each[0]['']

  puts "#{mfr} #{value[:desc]} is duplicated #{count} time(s)"

  first_id = client.execute("select top 1 EquipmentModelId from EquipmentModels where Description = '#{value[:desc]}' and SAPEquipmentManufacturerID = '#{value[:mfr_id]}'").each[0]['EquipmentModelId']

  puts "Planning to delete all but #{first_id}"

  extra_ids = []

  client.execute("select EquipmentModelId from EquipmentModels where EquipmentModelId <> #{first_id} and Description = '#{value[:desc]}' and SAPEquipmentManufacturerID = '#{value[:mfr_id]}'").each do |model_id|
    extra_ids.push model_id['EquipmentModelId']
  end

  extra_ids.each do |model_id|
    client.execute("UPDATE Equipment SET ModelID = #{first_id} WHERE ModelID = #{model_id}").do
    client.execute("DELETE FROM EquipmentModels WHERE EquipmentModelId = #{model_id}").do
  end
end
