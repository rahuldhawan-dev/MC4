use mapcalldev

declare @sapEqiupmentTypeId int;
select @sapEqiupmentTypeId = Id from SAPEquipmentTypes where Description like 'pump positive displacement'

select * from SAPEquipmentTypes where id = @sapEqiupmentTypeId;
select * from EquipmentTypes where SAPEquipmentTypeId = @sapEqiupmentTypeId;

select 
	REPLACE(f.FieldName, '-', '_') + ' = ' + cast(f.id as varchar) + ',',
	f.FieldName,
ft.DataType as [FieldType] from EquipmentCharacteristicFields f
inner join EquipmentCharacteristicFieldTypes ft
on ft.Id = f.FieldTypeId
where f.SAPEquipmentTypeId = @sapEqiupmentTypeId
order by f.FieldName

select ddv.*
from EquipmentCharacteristicDropDownValues ddv inner join EquipmentCharacteristicFields f on ddv.FieldId = f.Id where f.SAPEquipmentTypeId = @sapEqiupmentTypeId order by f.FieldName

select * from SAPEquipmentManufacturers where SAPEquipmentTypeId = @sapEqiupmentTypeId