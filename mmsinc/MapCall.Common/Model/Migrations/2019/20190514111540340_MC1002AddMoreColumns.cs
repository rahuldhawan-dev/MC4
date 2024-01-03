using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190514111540340), Tags("Production")]
    public class MC1002AddMoreColumns : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrderCompletions")
                 .AddColumn("LeakDetectedNonCompany").AsBoolean().Nullable()
                 .AddColumn("LeakDetectedDate").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Column("LeakDetectedDate").FromTable("ShortCycleWorkOrderCompletions");
            Delete.Column("LeakDetectedNonCompany").FromTable("ShortCycleWorkOrderCompletions");
        }
    }
}
