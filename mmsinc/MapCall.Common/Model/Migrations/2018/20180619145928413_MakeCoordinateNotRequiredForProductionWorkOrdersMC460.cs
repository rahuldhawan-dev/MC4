using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180619145928413), Tags("Production")]
    public class MakeCoordinateNotRequiredForProductionWorkOrdersMC460 : Migration
    {
        public override void Up()
        {
            Alter.Table("ProductionWorkOrders").AlterColumn("CoordinateId").AsInt32().Nullable();
        }

        public override void Down() { }
    }
}
