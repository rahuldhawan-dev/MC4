using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220406121608952), Tags("Production")]
    // ReSharper disable once InconsistentNaming
    public class MC4184_AddInjectionSiteToFacilitiesForSystemDelivery : AutoReversingMigration
    {
        public override void Up() =>
            Create.Column("IsInjectionSite")
                  .OnTable("FacilitiesSystemDeliveryEntryTypes")
                  .AsBoolean()
                  .NotNullable()
                  .WithDefaultValue(false);
    }
}

