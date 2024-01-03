using System.Collections.Generic;
using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220712121759884), Tags("Production")]
    // ReSharper disable once InconsistentNaming
    public class MC4133_SupportForMorePointOfUseTreatmentTypes : Migration
    {
        public override void Up()
        {
            this.AddLookupEntities("SampleSitePointOfUseTreatmentTypes", new Dictionary<int, string> {
                { 4, "Faucet Filter" },
                { 5, "Water Softener" },
                { 6, "Whole Home Filter" },
                { 7, "Other" }
            });

            Alter.Table("SampleSites").AddColumn("PointOfUseTreatmentTypeOtherReason").AsAnsiString(1024).Nullable();
        }

        public override void Down()
        {
            Execute.Sql("update SampleSites set SampleSitePointOfUseTreatmentTypeId = 1 where SampleSitePointOfUseTreatmentTypeId in (4, 5, 6, 7)");

            Delete.Column("PointOfUseTreatmentTypeOtherReason").FromTable("SampleSites");

            this.DeleteLookupEntities("SampleSitePointOfUseTreatmentTypes", new[] { 4, 5, 6, 7 });
        }
    }
}

