using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20181217115502972), Tags("Production")]
    public class MC757_AdjustColumnLength : Migration
    {
        public override void Up()
        {
            Alter.Column("ServiceType").OnTable("ShortCycleWorkOrdersEquipmentIds").AsAnsiString(4).Nullable();
        }

        public override void Down() { }
    }
}
