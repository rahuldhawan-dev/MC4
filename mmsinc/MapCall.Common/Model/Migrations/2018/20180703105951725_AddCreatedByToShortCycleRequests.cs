using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180703105951725), Tags("Production")]
    public class AddCreatedByToShortCycleRequests : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrderRequests").AddColumn("CreatedBy")
                 .AsAnsiString().Nullable();
        }

        public override void Down()
        {
            Delete.Column("CreatedBy").FromTable("ShortCycleWorkOrderRequests");
        }
    }
}
