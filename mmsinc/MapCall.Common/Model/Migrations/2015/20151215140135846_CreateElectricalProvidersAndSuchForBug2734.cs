using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20151215140135846), Tags("Production")]
    public class CreateElectricalProvidersAndSuchForBug2734 : Migration
    {
        public const string TABLE_NAME = "ElectricalProviders";
        public const int DESCRIPTION_LENGTH = 30, ACCOUNT_NUMBER_LENGTH = 14;

        public override void Up()
        {
            Create.LookupTable(TABLE_NAME, DESCRIPTION_LENGTH);

            Alter.Table("tblFacilities").AddForeignKeyColumn("ElectricalProviderId", TABLE_NAME);

            Alter.Table("tblFacilities").AddColumn("ElectricalAccountNumber").AsString(ACCOUNT_NUMBER_LENGTH)
                 .Nullable();
        }

        public override void Down()
        {
            Delete.Column("ElectricalAccountNumber").FromTable("tblFacilities");

            Delete.ForeignKeyColumn("tblFacilities", "ElectricalProviderId", TABLE_NAME);

            Delete.Table(TABLE_NAME);
        }
    }
}
