using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200821094400383), Tags("Production")]
    public class AddCompanyOwnedToServicesForMC2399 : Migration
    {
        public override void Up()
        {
            Alter.Table("Services").AddColumn("CompanyOwned").AsBoolean().Nullable();
            Execute.Sql("UPDATE Services SET CompanyOwned = 0 WHERE OperatingCenterId = 65");
        }

        public override void Down()
        {
            Delete.Column("CompanyOwned").FromTable("Services");
        }
    }
}
