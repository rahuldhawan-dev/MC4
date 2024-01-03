using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20170103141657786), Tags("Production")]
    public class AddSewerMainFunctionalLocationForBug3303 : Migration
    {
        public override void Up()
        {
            Alter.Table("Towns")
                 .AddColumn("SewerMainSAPEquipmentId").AsInt32().Nullable()
                 .AddColumn("SewerMainSAPFunctionalLocationId").AsInt32().Nullable()
                 .ForeignKey("FK_Towns_FunctionalLocations_SewerMainSAPFunctionalLocationId", "FunctionalLocations",
                      "FunctionalLocationId");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_Towns_FunctionalLocations_SewerMainSAPFunctionalLocationId").OnTable("Towns");
            Delete.Column("SewerMainSAPFunctionalLocationId").FromTable("Towns");
            Delete.Column("SewerMainSAPEquipmentId").FromTable("Towns");
        }
    }
}
