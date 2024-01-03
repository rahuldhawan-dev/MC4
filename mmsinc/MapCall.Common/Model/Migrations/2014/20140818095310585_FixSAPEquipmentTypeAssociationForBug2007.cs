using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140818095310585), Tags("Production")]
    public class FixSAPEquipmentTypeAssociationForBug2007 : Migration
    {
        public override void Up()
        {
            Create.Column("SAPEquipmentTypeId").OnTable("EquipmentTypes").AsInt32().Nullable();
            Create.Column("SAPEquipmentTypeId")
                  .OnTable("Equipment").AsInt32().Nullable()
                  .ForeignKey("FK_Equipment_SAPEquipmentTypes_SAPEquipmentTypeId", "SAPEquipmentTypes", "Id");

            Execute.Sql(
                "UPDATE EquipmentTypes SET SAPEquipmentTypeId = sap.Id FROM SAPEquipmentTypes sap WHERE sap.EquipmentTypeId = EquipmentTypes.EquipmentTypeId;");
            Execute.Sql(
                "UPDATE Equipment SET SAPEquipmentTypeID = sap.Id FROM SAPEquipmentTypes sap INNER JOIN EquipmentTypes e on e.SAPEquipmentTypeId = sap.Id WHERE e.EquipmentTypeId = Equipment.TypeId");

            Alter.Column("SAPEquipmentTypeId")
                 .OnTable("EquipmentTypes").AsInt32().Nullable()
                 .ForeignKey("FK_EquipmentTypes_SAPEquipmentTypes_SAPEquipmentTypeId", "SAPEquipmentTypes", "Id");

            Delete.ForeignKey("FK_SAPEquipmentTypes_EquipmentTypes_EquipmentTypeId").OnTable("SAPEquipmentTypes");
            Delete.Column("EquipmentTypeId").FromTable("SAPEquipmentTypes");

            Alter.Column("TypeId")
                 .OnTable("Equipment").AsInt32().Nullable();
        }

        public override void Down()
        {
            Create.Column("EquipmentTypeId").OnTable("SAPEquipmentTypes").AsInt32().Nullable();

            Execute.Sql(
                "UPDATE SAPEquipmentTypes set EquipmentTypeId = e.EquipmentTypeId FROM EquipmentTypes e WHERE e.SAPEquipmentTypeId = SAPEquipmentTypes.Id");

            Alter.Column("EquipmentTypeId")
                 .OnTable("SAPEquipmentTypes").AsInt32().Nullable().ForeignKey(
                      "FK_SAPEquipmentTypes_EquipmentTypes_EquipmentTypeId", "EquipmentTypes", "EquipmentTypeId");

            Delete.ForeignKey("FK_EquipmentTypes_SAPEquipmentTypes_SAPEquipmentTypeId").OnTable("EquipmentTypes");
            Delete.Column("SAPEquipmentTypeId").FromTable("EquipmentTypes");

            Delete.ForeignKey("FK_Equipment_SAPEquipmentTypes_SAPEquipmentTypeId").OnTable("Equipment");
            Delete.Column("SAPEquipmentTypeId").FromTable("Equipment");
        }
    }
}
