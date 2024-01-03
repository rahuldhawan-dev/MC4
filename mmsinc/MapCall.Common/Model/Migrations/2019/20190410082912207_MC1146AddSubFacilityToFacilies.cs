using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190410082912207), Tags("Production")]
    public class MC1146AddSubFacilityToFacilies : Migration
    {
        public override void Up()
        {
            Alter.Table("tblFacilities")
                 .AddForeignKeyColumn("ParentFacilityId", "tblFacilities", "RecordId");
            Execute.Sql("UPDATE [tblFacilities] " +
                        "SET ParentFacilityId = (select Top 1 F2.recordId from tblFacilities F2 where F2.FunctionalLocationId <> F1.FunctionalLocationId AND F2.FunctionalLocationId = left(F1.functionallocationId, len(F1.FunctionalLocationId) - charindex('-', reverse(F1.FunctionalLocationId)))) " +
                        "FROM tblFacilities F1 " +
                        "WHERE F1.FunctionalLocationid IS NOT NULL");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("tblFacilities", "ParentFacilityId", "tblFacilities", "RecordId");
        }
    }
}
