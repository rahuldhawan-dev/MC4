using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20231121110139505), Tags("Production")]
    public class MC6442_CreateColumn_IsAutomationEnabled_OnTable_FacilitiesSystemDeliveryEntryTypes : AutoReversingMigration
    {
        public override void Up() =>
            Create.Column("IsAutomationEnabled")
                  .OnTable("FacilitiesSystemDeliveryEntryTypes")
                  .AsBoolean()
                  .NotNullable()
                  .WithDefaultValue(false);
    }
}
