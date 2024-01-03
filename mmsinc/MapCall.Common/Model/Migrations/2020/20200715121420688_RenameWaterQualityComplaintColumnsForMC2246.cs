using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200715121420688), Tags("Production")]
    public class RenameWaterQualityComplaintColumnsForMC2246 : Migration
    {
        public override void Up()
        {
            Rename.Column("EnteredBy").OnTable("WaterQualityComplaints").To("NotificationCreatedBy");
            Rename.Column("ComplaintCloseDate").OnTable("WaterQualityComplaints").To("NotificationCompletionDate");
            Rename.Column("ClosedBy").OnTable("WaterQualityComplaints").To("NotificationCompletedBy");
        }

        public override void Down()
        {
            Rename.Column("NotificationCreatedBy").OnTable("WaterQualityComplaints").To("EnteredBy");
            Rename.Column("NotificationCompletionDate").OnTable("WaterQualityComplaints").To("ComplaintCloseDate");
            Rename.Column("NotificationCompletedBy").OnTable("WaterQualityComplaints").To("ClosedBy");
        }
    }
}
