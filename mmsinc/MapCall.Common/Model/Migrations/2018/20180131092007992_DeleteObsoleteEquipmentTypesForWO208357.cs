using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180131092007992), Tags("Production")]
    public class DeleteObsoleteEquipmentTypesForWO208357 : Migration
    {
        public override void Up()
        {
            const string equipment = "Equipment",
                         equipmentTypes = "EquipmentTypes",
                         sapEquipmentTypes = "SAPEquipmentTypes";

            Execute.Sql($@"
UPDATE
	{equipment} SET TypeId = (
		SELECT TOP 1 EquipmentTypeId
		FROM {equipmentTypes} it
		INNER JOIN {sapEquipmentTypes} sap
		ON sap.Id = it.SAPEquipmentTypeId
		WHERE it.Description = sap.Description
		AND sap.Id = {equipment}.SAPEquipmentTypeId)
FROM {equipmentTypes} t
WHERE t.EquipmentTypeId = {equipment}.TypeId
AND t.Description LIKE 'Delete%';
DELETE FROM {equipmentTypes} WHERE Description LIKE 'Delete%';");

            Execute.Sql($@"
UPDATE {equipmentTypes} SET Description = REPLACE(Description, 'Replace with ', '');
UPDATE {equipment} SET TypeId = cast(t.Description as int) FROM EquipmentTypes t WHERE t.EquipmentTypeId = TypeId AND isnumeric(t.Description) = 1;
DELETE FROM {equipmentTypes} WHERE isnumeric(Description) = 1;");
        }

        public override void Down() { }
    }
}
