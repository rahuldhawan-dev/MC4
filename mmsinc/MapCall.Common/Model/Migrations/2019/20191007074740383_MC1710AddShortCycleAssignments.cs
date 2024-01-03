using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20191007074740383), Tags("Production")]
    public class MC1710AddShortCycleAssignments : Migration
    {
        public override void Up()
        {
            Create.Table("ShortCycleAssignments")
                  .WithIdentityColumn()
                  .WithColumn("CallId").AsAnsiString(64).Nullable()
                  .WithColumn("Number").AsAnsiString(64).Nullable()
                  .WithColumn("Status").AsAnsiString(64).Nullable()
                  .WithColumn("Engineer").AsAnsiString(64).Nullable()
                  .WithColumn("Start").AsDateTime().NotNullable()
                  .WithColumn("Finish").AsDateTime().NotNullable()
                  .WithColumn("SAPErrorCode").AsCustom("nvarchar(MAX)").Nullable()
                  .WithColumn("SAPCommunicationError").AsBoolean().WithDefaultValue(false)
                  .WithColumn("ReceivedAt").AsDateTime().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("ShortCycleAssignments");
        }
    }
}
