using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220823094715774), Tags("Production")]
    public class MC4896_UpdateUnknownValuesInWorkOrders : Migration
    {
        public override void Up()
        {
            Execute.Sql(
                "update WorkOrders set CompanyServiceLineMaterialId = null where CompanyServiceLineMaterialId = 11;" +
                "update WorkOrders set CustomerServiceLineMaterialID = null where CustomerServiceLineMaterialID = 11;");
        }

        public override void Down() { }
    }
}

