using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190930144846207), Tags("Production")]
    public class RemoveFinalizedAndInitialCategoryFieldsMakeSystemIDNullableForMC1617 : Migration
    {
        public override void Up()
        {
            Delete.Column("InitialCategory").FromTable("NoticesOfViolation");

            Delete.Column("FinalizedCategory").FromTable("NoticesOfViolation");

            Execute.Sql(
                $"ALTER TABLE NoticesOfViolation DROP CONSTRAINT [FK_NoticesOfViolation_PublicWaterSupplies_SystemId]");

            Alter.Column("SystemId").OnTable("NoticesOfViolation")
                 .AsForeignKey("SystemID", "PublicWaterSupplies", "Id");
        }

        public override void Down()
        {
            Alter.Table("NoticesOfViolation")
                 .AddColumn("InitialCategory").AsAnsiString(50).Nullable()
                 .AddColumn("FinalizedCategory").AsAnsiString(50).Nullable();

            Execute.Sql(
                $"ALTER TABLE NoticesOfViolation DROP CONSTRAINT [FK_NoticesOfViolation_PublicWaterSupplies_SystemId]");

            Alter.Column("SystemId").OnTable("NoticesOfViolation")
                 .AsForeignKey("SystemID", "PublicWaterSupplies", "Id", false);
        }
    }
}
