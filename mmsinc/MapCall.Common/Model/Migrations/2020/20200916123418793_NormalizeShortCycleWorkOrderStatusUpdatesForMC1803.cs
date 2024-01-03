using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200916123418793), Tags("Production")]
    public class NormalizeShortCycleWorkOrderStatusUpdatesForMC1803 : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrderStatusUpdates")
                 .AlterColumn("OperationNumber").AsInt32().Nullable();
        }

        public override void Down()
        {
            Alter.Table("ShortCycleWorkOrderStatusUpdates")
                 .AlterColumn("OperationNumber").AsString(4).Nullable();
        }
    }
}
