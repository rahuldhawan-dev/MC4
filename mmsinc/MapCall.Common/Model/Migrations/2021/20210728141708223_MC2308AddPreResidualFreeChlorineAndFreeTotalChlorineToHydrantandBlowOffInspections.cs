using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210728141708223)]
    [Tags("Production")]
    public class MC2308AddPreResidualFreeChlorineAndFreeTotalChlorineToHydrantandBlowOffInspections : Migration
    {
        #region Exposed Methods

        public override void Up()
        {
            Alter.Table("HydrantInspections").AddColumn("PreResidualChlorine").AsDecimal(3, 2).Nullable();
            Alter.Table("HydrantInspections").AddColumn("PreTotalChlorine").AsDecimal(3, 2).Nullable();
            Alter.Table("BlowOffInspections").AddColumn("PreResidualChlorine").AsDecimal(3, 2).Nullable();
            Alter.Table("BlowOffInspections").AddColumn("PreTotalChlorine").AsDecimal(3, 2).Nullable();
        }

        public override void Down()
        {
            Delete.Column("PreResidualChlorine").FromTable("HydrantInspections");
            Delete.Column("PreTotalChlorine").FromTable("HydrantInspections");
            Delete.Column("PreResidualChlorine").FromTable("BlowOffInspections");
            Delete.Column("PreTotalChlorine").FromTable("BlowOffInspections");
        }

        #endregion
    }
}
