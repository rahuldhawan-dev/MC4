using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220331150617307), Tags("Production")]
    public class MC4372SampleSitesRemoveUnusedFields : Migration
    {
        public override void Up()
        {
            Delete.Column("SampleLocationNotes").FromTable("SampleSites");
            Delete.Column("Stage_1_DBP_Site").FromTable("SampleSites");
            Delete.Column("Stage_2_DBP_Site").FromTable("SampleSites");
            Delete.Column("Process_Control_Site").FromTable("SampleSites");
            Delete.Column("NJAW_Owned").FromTable("SampleSites");
            Delete.Column("Frequency_Per_Month").FromTable("SampleSites");
            Delete.Column("Route").FromTable("SampleSites");
            Delete.Column("Route_Sequence").FromTable("SampleSites");
            Delete.Column("SamplingInstructions").FromTable("SampleSites");
            Delete.Column("SafetyConcerns").FromTable("SampleSites");
            Delete.Column("ChloramineMonitorSite").FromTable("SampleSites");
            Delete.Column("UnregulatedContaminantMonitoringRuleSite").FromTable("SampleSites");
            Delete.Column("IsSentToSample1View").FromTable("SampleSites");
            Delete.Column("IsPermitRequired").FromTable("SampleSites");
            Delete.Column("ObjectId").FromTable("SampleSites");
        }

        public override void Down()
        {
            Alter.Table("SampleSites").AddColumn("SampleLocationNotes").AsAnsiString(120).Nullable();
            Alter.Table("SampleSites").AddColumn("Stage_1_DBP_Site").AsBoolean().Nullable().WithDefaultValue(false);
            Alter.Table("SampleSites").AddColumn("Stage_2_DBP_Site").AsBoolean().Nullable().WithDefaultValue(false);
            Alter.Table("SampleSites").AddColumn("Process_Control_Site").AsBoolean().Nullable().WithDefaultValue(false);
            Alter.Table("SampleSites").AddColumn("NJAW_Owned").AsBoolean().Nullable().WithDefaultValue(false);
            Alter.Table("SampleSites").AddColumn("Frequency_Per_Month").AsAnsiString(50).Nullable();
            Alter.Table("SampleSites").AddColumn("Route").AsAnsiString(50).Nullable();
            Alter.Table("SampleSites").AddColumn("Route_Sequence").AsAnsiString(50).Nullable();
            Alter.Table("SampleSites").AddColumn("SamplingInstructions").AsAnsiString(300).Nullable();
            Alter.Table("SampleSites").AddColumn("SafetyConcerns").AsAnsiString(300).Nullable();
            Alter.Table("SampleSites").AddColumn("ChloramineMonitorSite").AsBoolean().Nullable().WithDefaultValue(false);
            Alter.Table("SampleSites").AddColumn("UnregulatedContaminantMonitoringRuleSite").AsBoolean().Nullable().WithDefaultValue(false);
            Alter.Table("SampleSites").AddColumn("IsSentToSample1View").AsBoolean().Nullable().WithDefaultValue(false);
            Alter.Table("SampleSites").AddColumn("IsPermitRequired").AsBoolean().Nullable().WithDefaultValue(false);
            Alter.Table("SampleSites").AddColumn("ObjectId").AsInt32().Nullable();
        }
    }
}

