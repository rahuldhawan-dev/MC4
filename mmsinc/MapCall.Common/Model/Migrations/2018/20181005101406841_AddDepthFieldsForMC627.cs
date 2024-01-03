using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20181005101406841), Tags("Production")]
    public class AddDepthFieldsForMC627 : Migration
    {
        public override void Up()
        {
            Alter.Table("Valves")
                 .AddColumn("DepthFeet").AsInt32().Nullable()
                 .AddColumn("DepthInches").AsInt32().Nullable();
        }

        public override void Down()
        {
            Delete.Column("DepthFeet").FromTable("Valves");
            Delete.Column("DepthInches").FromTable("Valves");
        }
    }
}
