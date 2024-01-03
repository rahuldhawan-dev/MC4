using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20131217135127), Tags("Production")]
    public class DropUnusedDocumentColumns : Migration
    {
        public override void Up()
        {
            Execute.WithConnection((conn, tran) => {
                // 40 zeroes
                var hash = "0000000000000000000000000000000000000000";

                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tran;

                    // This part of the migration is for DEVELOPMENT ONLY. The live database, along with the staging
                    // database, already has this data populated correctly. We need to feed crap data
                    // to the database during dataimport, though, which will not have this data.
                    // During dataimport is the only time this command should ever return something greater
                    // than zero.

                    // NOTE: It's impossible for this migration to actually create a file on the drive because
                    //       migrations do not have access to a site's web.config, so there's no access to the
                    //       DocumentDataDirectory setting.

                    cmd.CommandText = "SELECT COUNT(DocumentId) FROM [Document] WHERE DocumentDataId is null";
                    var docsWithoutData = (int)cmd.ExecuteScalar();

                    if (docsWithoutData > 0)
                    {
                        cmd.CommandText =
                            "INSERT INTO [DocumentData] (Hash, FileSize) VALUES ('" + hash +
                            "', 0); SELECT SCOPE_IDENTITY();";
                        var docDataId = (int)(decimal)cmd.ExecuteScalar();

                        cmd.CommandText = "UPDATE [Document] SET DocumentDataId = " + docDataId +
                                          " WHERE DocumentDataId is null";
                        cmd.ExecuteNonQuery();
                    }
                }
            });

            Execute.Sql(
                "IF EXISTS (SELECT 1 FROM sysindexes where [name] = '_dta_stat_1975014117_1_3_4_5_6_7_9_10_11_12') DROP STATISTICS Document._dta_stat_1975014117_1_3_4_5_6_7_9_10_11_12");
            Execute.Sql(
                "IF EXISTS (SELECT 1 FROM sysindexes where [name] = '_dta_stat_1975014117_3_4_5_6_7_9_10_11_12_2_1') DROP STATISTICS Document._dta_stat_1975014117_3_4_5_6_7_9_10_11_12_2_1");

            Delete.Column("Description").FromTable("Document");
            Delete.Column("BinaryData").FromTable("Document");
            Delete.Column("File_Size").FromTable("Document");
            Alter.Column("DocumentDataId").OnTable("Document").AsInt32().NotNullable();
        }

        public override void Down()
        {
            Alter.Table("Document")
                 .AddColumn("BinaryData").AsCustom("image").Nullable()
                 .AddColumn("Description").AsString(255).Nullable()
                 .AddColumn("File_Size").AsInt32().Nullable();
            Alter.Column("DocumentDataId").OnTable("Document").AsInt32().Nullable();
        }
    }
}
