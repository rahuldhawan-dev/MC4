using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150216113756695), Tags("Production")]
    public class AddMapIdFieldToGISLayerUpdatesForBug2095 : Migration
    {
        public override void Up()
        {
            Execute.Sql("DELETE FROM GISLayerUpdates WHERE Updated = '2014-10-26';");

            Alter.Table("GISLayerUpdates")
                 .AddColumn("MapId").AsFixedLengthString(32).NotNullable();

            Execute.Sql(
                "INSERT INTO GISLayerUpdates (Updated, CreatedAt, CreatedById, IsActive, MapId) SELECT '2014-10-26', GetDate(), RecId, 1, 'c46c400b974e402f84361e65efa3f611' FROM tblPermissions WHERE UserName = 'mcadmin';");
        }

        public override void Down()
        {
            Execute.Sql("DELETE FROM GISLayerUpdates WHERE Updated = '2014-10-26';");

            Delete.Column("MapId").FromTable("GISLayerUpdates");

            Execute.Sql(
                "INSERT INTO GISLayerUpdates (Updated, CreatedAt, CreatedById, IsActive) SELECT '2014-10-26', GetDate(), RecId, 1 FROM tblPermissions WHERE UserName = 'mcadmin';");
        }
    }
}
