using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220921131956011), Tags("Production")]
    public class MC4926ActivatingMainSize : Migration
    {
        public override void Up()
        {
            Execute.Sql("Update ServiceSizes set Main = 1 where ServiceSizeDescription = '1 1/2'");
        }

        public override void Down()
        {
            Execute.Sql("Update ServiceSizes set Main = 0 where ServiceSizeDescription = '1 1/2'");
        }
    }
}

