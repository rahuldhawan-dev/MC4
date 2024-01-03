using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210713131708530), Tags("Production")]
    public class Mc3007RemoveRegulatedFromNearMiss : Migration
    {
        public override void Up()
        {
            Alter.Table("NearMisses")
                 .AlterColumn("ReportedBy").AsString(100).Nullable();
            Delete.Column("Regulated").FromTable("NearMisses");
        }

        public override void Down()
        {
            Alter.Table("NearMisses")
                 .AddColumn("Regulated").AsString(100).Nullable();
        }
    }
}

