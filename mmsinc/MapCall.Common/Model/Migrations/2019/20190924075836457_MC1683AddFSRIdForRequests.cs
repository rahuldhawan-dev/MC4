using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190923102116214), Tags("Production")]
    public class MC1683AddFSRIdForRequests : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrderRequests").AddColumn("FSRId").AsInt32().Nullable();
        }

        public override void Down()
        {
            Delete.Column("FSRId").FromTable("ShortCycleWorkOrderRequests");
        }
    }
}
