using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20221228113913970), Tags("Production")]
    public class MC4834_ChangeSewerMainCleaningsBlockageFoundToBoolean : Migration
    {
        public override void Up()
        {
            Create.Column("BlockageFound").OnTable("SewerMainCleanings").AsBoolean().Nullable();
            
            Update.Table("SewerMainCleanings")
                  .Set(new {BlockageFound = false})
                  .Where(new {BlockageFoundId = (int?)null});
            Update.Table("SewerMainCleanings")
                  .Set(new {BlockageFound = true})
                  .Where(new {BlockageFound = (bool?)null});

            Alter.Column("BlockageFound").OnTable("SewerMainCleanings").AsBoolean().NotNullable();

            Delete.ForeignKeyColumn("SewerMainCleanings", "BlockageFoundId", "TypesOfBlockagesFound");

            Delete.Table("TypesOfBlockagesFound");
        }

        public override void Down()
        {
            this.CreateLookupTableWithValues("TypesOfBlockagesFound", "Full", "Partial");

            Alter.Table("SewerMainCleanings")
                 .AddForeignKeyColumn("BlockageFoundId", "TypesOfBlockagesFound");
            
            // best we can do is set these back to 'Partial'
            Update.Table("SewerMainCleanings")
                  .Set(new {BlockageFoundId = 2})
                  .Where(new {BlockageFound = true});

            Delete.Column("BlockageFound").FromTable("SewerMainCleanings");
        }
    }
}

