using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220908154531660), Tags("Production")]
    public class MC4919_AddWeeklyTotalColumn : Migration
    {
        public override void Up()
        {
            Alter.Table("SystemDeliveryFacilityEntries").AddColumn("WeeklyTotal").AsDecimal(19, 5).Nullable();
        }

        public override void Down()
        {
            Delete.Column("WeeklyTotal").FromTable("SystemDeliveryFacilityEntries");
        }
    }
}

