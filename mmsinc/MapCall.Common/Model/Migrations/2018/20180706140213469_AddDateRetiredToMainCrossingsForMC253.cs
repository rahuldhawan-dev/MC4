using System.Collections.Generic;
using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180706140213469), Tags("Production")]
    public class AddDateRetiredToMainCrossingsForMC253 : Migration
    {
        public override void Up()
        {
            Alter.Table("MainCrossings").AddColumn("DateRetired").AsDateTime().Nullable();
            Insert.IntoTable("WorkOrderCancellationReasons")
                  .Row(new Dictionary<string, object> {{"Status", "ARET"}, {"Description", "Asset Retired"}});
        }

        public override void Down()
        {
            Delete.Column("DateRetired").FromTable("MainCrossings");
            Execute.Sql("DELETE FROM WorkOrderCancellationReasons WHERE Status = 'ARET'");
        }
    }
}
