using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20181031143614163), Tags("Production")]
    public class NormalizeFacilitiesPWSIDForMC548 : Migration
    {
        public override void Up()
        {
            Alter.Table("tblFacilities").AddForeignKeyColumn("PublicWaterSupplyId", "PublicWaterSupplies");

            Execute.Sql(@"
UPDATE tblFacilities
SET PublicWaterSupplyId = pws.Id
FROM PublicWaterSupplies pws
WHERE pws.PWSID = right('00000' + replace(replace(replace(replace(replace(tblFacilities.PWSID, 'nj', ''), 'ny', ''), 'il', ''), 'pa', ''), '-', ''), 9)
OR pws.PWSID = right('00000' + replace(replace(replace(replace(replace(tblFacilities.PWSID, 'nj', ''), 'ny', ''), 'il', ''), 'pa', ''), '-', ''), 7)
");

            Delete.Column("PWSID").FromTable("tblFacilities");
        }

        public override void Down()
        {
            Alter.Table("tblFacilities").AddColumn("PWSID").AsString(10).Nullable();

            Execute.Sql(@"
UPDATE tblFacilities
SET PWSID = pwd.PWSID
FROM PublicWaterSupplies pws
WHERE pws.Id = tblFacilities.PublicWaterSupplyId");

            Delete.ForeignKeyColumn("tblFacilities", "PublicWaterSupplyId", "PublicWaterSupplies");
        }
    }
}
