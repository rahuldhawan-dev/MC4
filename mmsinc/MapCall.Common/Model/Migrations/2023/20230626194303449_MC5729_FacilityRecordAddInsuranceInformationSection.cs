using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230626194303449), Tags("Production")]
    public class MC5729_FacilityRecordAddInsuranceInformationSection : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("InsuranceScoreQuartiles", "1", "2", "3", "4");
            Alter.Table("tblFacilities")
                 .AddColumn("InsuranceScore").AsDecimal(5, 2).Nullable();
            Alter.Table("tblFacilities")
                 .AddForeignKeyColumn("InsuranceScoreQuartile", "InsuranceScoreQuartiles").Nullable();
            Alter.Table("tblFacilities")
                 .AddColumn("InsuranceVisitDate").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Column("InsuranceScore").FromTable("tblFacilities");
            Delete.ForeignKeyColumn("tblFacilities", "InsuranceScoreQuartile", "InsuranceScoreQuartiles");
            Delete.Column("InsuranceVisitDate").FromTable("tblFacilities");
        }
    }
}