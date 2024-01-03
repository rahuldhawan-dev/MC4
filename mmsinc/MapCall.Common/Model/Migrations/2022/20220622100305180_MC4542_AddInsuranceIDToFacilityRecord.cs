using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220622100305180), Tags("Production")]
    public class MC4542_AddInsuranceIDToFacilityRecord : Migration
    {
        public override void Up()
        {
            Alter.Table("tblFacilities").AddColumn("InsuranceId").AsString(20).Nullable();
        }

        public override void Down()
        {
            Delete.Column("InsuranceId").FromTable("tblFacilities");
        }
    }
}

