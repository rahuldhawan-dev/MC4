using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170811163123095), Tags("Production")]
    public class AddFieldsToPWSIDForBug3976 : Migration
    {
        public override void Up()
        {
            Alter.Table("PublicWaterSupplies")
                 .AddColumn("AWOwned").AsBoolean().Nullable()
                 .AddForeignKeyColumn("StateId", "States", "StateId");
        }

        public override void Down()
        {
            Delete.Column("AWOwned").FromTable("PublicWaterSupplies");
            Delete.ForeignKeyColumn("PublicWaterSupplies", "StateId", "States", "StateId");
        }
    }
}
