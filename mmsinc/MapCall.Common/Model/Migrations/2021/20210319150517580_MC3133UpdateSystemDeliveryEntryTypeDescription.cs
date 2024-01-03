using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210319150517580), Tags("Production")]
    public class MC3133UpdateSystemDeliveryEntryTypeDescription : Migration
    {
        private string sqlUp = @"UPDATE SystemDeliveryEntryTypes
                                 SET [Description] = 'Purchased Water'
                                 WHERE Id = 1";
        private string sqlDown = @"UPDATE SystemDeliveryEntryTypes
                                 SET [Description] = 'Purchase Point'
                                 WHERE Id = 1";
        public override void Up()
        {
            Execute.Sql(sqlUp);
        }

        public override void Down()
        {
            Execute.Sql(sqlDown);
        }
    }
}

