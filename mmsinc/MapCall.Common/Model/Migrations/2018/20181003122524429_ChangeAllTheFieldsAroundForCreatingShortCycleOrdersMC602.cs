using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20181003122524429), Tags("Production")]
    public class ChangeAllTheFieldsAroundForCreatingShortCycleOrdersMC602 : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrderRequests")
                 .AddColumn("CompanyName").AsAnsiString()
                 .Nullable()
                 .AddColumn("CompanyNumber").AsAnsiString()
                 .Nullable()
                 .AddColumn("ContractorName").AsAnsiString()
                 .Nullable()
                 .AddColumn("ContractorPhone").AsAnsiString()
                 .Nullable()
                 .AddColumn("DayFrom").AsAnsiString().Nullable()
                 .AddColumn("DayTo").AsAnsiString().Nullable()
                 .AddColumn("HourAM").AsAnsiString().Nullable()
                 .AddColumn("HourPM").AsAnsiString().Nullable()
                 .AddColumn("LetterId").AsAnsiString().Nullable()
                 .AddColumn("Telephone").AsAnsiString().Nullable();
        }

        public override void Down()
        {
            Delete.Column("CompanyName").FromTable("ShortCycleWorkOrderRequests");
            Delete.Column("CompanyNumber").FromTable("ShortCycleWorkOrderRequests");
            Delete.Column("ContractorName").FromTable("ShortCycleWorkOrderRequests");
            Delete.Column("ContractorPhone").FromTable("ShortCycleWorkOrderRequests");
            Delete.Column("DayFrom").FromTable("ShortCycleWorkOrderRequests");
            Delete.Column("DayTo").FromTable("ShortCycleWorkOrderRequests");
            Delete.Column("HourAM").FromTable("ShortCycleWorkOrderRequests");
            Delete.Column("HourPM").FromTable("ShortCycleWorkOrderRequests");
            Delete.Column("LetterId").FromTable("ShortCycleWorkOrderRequests");
            Delete.Column("Telephone").FromTable("ShortCycleWorkOrderRequests");
        }
    }
}
