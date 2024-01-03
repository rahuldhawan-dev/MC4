using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210915103514402), Tags("Production")]
    public class MC3683_AddingDataCollectionMapToOperatingCenterTable : Migration
    {
        public override void Up()
        {
            Alter.Table("OperatingCenters").AddColumn("DataCollectionMapUrl").AsString(50).Nullable();
        }

        public override void Down()
        {
            Delete.Column("DataCollectionMapUrl").FromTable("OperatingCenters");
        }
    }
}

