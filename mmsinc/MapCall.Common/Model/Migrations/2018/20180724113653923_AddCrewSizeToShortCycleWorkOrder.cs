using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180724113653923), Tags("Production")]
    public class AddCrewSizeToShortCycleWorkOrder : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrders")
                 .AddColumn("CrewSize").AsInt32().Nullable();
        }

        public override void Down()
        {
            Delete.Column("CrewSize").FromTable("ShortCycleWorkOrders");
        }
    }
}
