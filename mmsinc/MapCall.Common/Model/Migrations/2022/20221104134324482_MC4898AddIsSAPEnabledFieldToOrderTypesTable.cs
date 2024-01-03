using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20221104134324482), Tags("Production")]
    public class MC4898AddIsSAPEnabledFieldToOrderTypesTable : Migration
    {
        private const int ROUTINE_ORDER_TYPE = 5;

        public override void Up()
        {
            Alter.Table("OrderTypes")
                 .AddColumn("IsSAPEnabled")
                 .AsBoolean()
                 .NotNullable()
                 .WithDefaultValue(false);

            Execute.Sql($"UPDATE OrderTypes SET IsSAPEnabled = 1 WHERE Id != {ROUTINE_ORDER_TYPE}");
        }

        public override void Down()
        {
            Delete.Column("IsSAPEnabled").FromTable("OrderTypes");
        }
    }
}

