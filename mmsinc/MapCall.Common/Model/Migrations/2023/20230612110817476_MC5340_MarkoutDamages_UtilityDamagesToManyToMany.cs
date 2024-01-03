using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Data;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230612110817476), Tags("Production")]
    public class MC5340_MarkoutDamages_UtilityDamagesToManyToMany : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("MarkoutDamageUtilityDamageTypes",
                "Communication", "Electric", "Gas", "Sewer", "Water");

            Create.Table("MarkoutDamagesMarkoutDamageUtilityDamageTypes")
                  .WithForeignKeyColumn("MarkoutDamageId", "MarkoutDamages", "Id", nullable: false)
                  .WithForeignKeyColumn("MarkoutDamageUtilityDamageTypeId", "MarkoutDamageUtilityDamageTypes", "Id", nullable: false);

            this.CreateNotificationPurpose("Field Services", "Work Management", "Markout Damage SIF Or SIFP Event");
        }

        public override void Down()
        {
            // Need to use the full method path due to ambiguous references if
            // trying to use this as an extension method.
            Data.MigrationExtensions.RemoveNotificationPurpose(this, "Field Services", "Work Management", "Markout Damage SIF Or SIFP Event");
            Delete.Table("MarkoutDamagesMarkoutDamageUtilityDamageTypes");
            Delete.Table("MarkoutDamageUtilityDamageTypes");
        }
    }
}
