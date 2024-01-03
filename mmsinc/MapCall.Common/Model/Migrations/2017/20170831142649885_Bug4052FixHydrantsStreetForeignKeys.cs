using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170831142649885), Tags("Production")]
    public class Bug4052FixHydrantsStreetForeignKeys : Migration
    {
        public override void Up()
        {
            Create.ForeignKey("FK_Hydrants_Streets_StreetId")
                  .FromTable("Hydrants")
                  .ForeignColumn("StreetId")
                  .ToTable("Streets")
                  .PrimaryColumn("StreetId");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_Hydrants_Streets_StreetId").OnTable("Hydrants");
        }
    }
}
