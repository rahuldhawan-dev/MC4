using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180830105851355), Tags("Production")]
    public class MC599_AddLocalCertifiedStateIdtoPWSID : Migration
    {
        public override void Up()
        {
            Create.Column("LocalCertifiedStateId").OnTable("PublicWaterSupplies")
                  .AsString(10).Nullable();
        }

        public override void Down()
        {
            Delete.Column("LocalCertifiedStateId").FromTable("PublicWaterSupplies");
        }
    }
}
