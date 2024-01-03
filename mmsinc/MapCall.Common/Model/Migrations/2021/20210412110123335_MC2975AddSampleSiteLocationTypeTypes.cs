using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210412110123335), Tags("Production")]
    public class MC2975AddSampleSiteLocationTypeTypes : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("SampleSiteAddressLocationTypes", "Premise", "Facility", "Custom");
            Alter.Table("SampleSites").AddForeignKeyColumn("SampleSiteAddressLocationTypeId", "SampleSiteAddressLocationTypes");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("SampleSites", "SampleSiteAddressLocationTypeId", "SampleSiteAddressLocationTypes");
            Delete.Table("SampleSiteAddressLocationTypes");
        }
    }
}

