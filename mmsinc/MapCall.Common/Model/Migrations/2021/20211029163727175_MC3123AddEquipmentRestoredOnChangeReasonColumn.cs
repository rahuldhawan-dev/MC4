using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20211029163727175), Tags("Production")]
    public class MC3123AddEquipmentRestoredOnChangeReasonColumn : Migration
    {
        public override void Up()
        {
            Alter.Table("RedTagPermits")
                 .AddColumn("EquipmentRestoredOnChangeReason")
                 .AsAnsiString(255)
                 .Nullable();
        }

        public override void Down()
        {
            Delete.Column("EquipmentRestoredOnChangeReason").FromTable("RedTagPermits");
        }
    }
}

