using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20211208100401665), Tags("Production")]
    public class MC3679AddingFieldsToFacilitiesTable : Migration
    {
        public override void Up()
        {
            Alter.Table("tblFacilities")
                 .AddColumn("RiskBasedCompletedDate").AsDate().Nullable()
                 .AddColumn("CriticalFacilityIdentified").AsBoolean().Nullable()
                 .AddColumn("AssessmentCompletedOn").AsDate().Nullable();
        }

        public override void Down()
        {
            Delete.Column("RiskBasedCompletedDate").FromTable("tblFacilities");
            Delete.Column("CriticalFacilityIdentified").FromTable("tblFacilities");
            Delete.Column("AssessmentCompletedOn").FromTable("tblFacilities");
        }
    }
}

