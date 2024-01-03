using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160505151115263), Tags("Production")]
    public class AddFieldsToIncidentsForBug2856 : Migration
    {
        public struct StringLengths
        {
            public const int SAP_WORK_ORDER_ID = 50;
        }

        public override void Up()
        {
            Alter.Table("GeneralLiabilityClaims").AddColumn("SAPWorkOrderId")
                 .AsAnsiString(StringLengths.SAP_WORK_ORDER_ID).Nullable();
        }

        public override void Down()
        {
            Delete.Column("SAPWorkOrderId").FromTable("GeneralLiabilityClaims");
        }
    }
}
