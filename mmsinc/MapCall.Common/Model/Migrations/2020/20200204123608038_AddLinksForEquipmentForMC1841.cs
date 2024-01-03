using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200204123608038), Tags("Production")]
    public class AddLinksForEquipmentForMC1841 : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("LinkTypes", "Training", "PPE Document", "PPE Video", "Safety",
                "Additional Content");
            Create.Table("EquipmentLinks")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("EquipmentId", "Equipment", "EquipmentID").NotNullable()
                  .WithForeignKeyColumn("LinkTypeId", "LinkTypes").NotNullable()
                  .WithColumn("URL").AsAnsiString(StringLengths.MAX_DEFAULT_VALUE).NotNullable();
        }

        public override void Down()
        {
            Delete.Table("EquipmentLinks");
            Delete.Table("LinkTypes");
        }
    }
}
