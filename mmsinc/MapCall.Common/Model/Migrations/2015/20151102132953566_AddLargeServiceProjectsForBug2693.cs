using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20151102132953566), Tags("Production")]
    public class AddLargeServiceProjectsForBug2693 : Migration
    {
        public override void Up()
        {
            Create.Table("LargeServiceProjects")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("OperatingCenterId", "OperatingCenters", "OperatingCenterId", false)
                  .WithForeignKeyColumn("TownId", "Towns", "TownId", false)
                  .WithColumn("WBSNumber").AsAnsiString(18).Nullable()
                  .WithColumn("ProjectTitle").AsAnsiString(255).NotNullable()
                  .WithColumn("ProjectAddress").AsAnsiString(255).Nullable()
                  .WithForeignKeyColumn("AssetCategoryId", "AssetCategories")
                  .WithForeignKeyColumn("AssetTypeId", "AssetTypes", "AssetTypeID")
                  .WithForeignKeyColumn("ProposedDiameterId", "PipeDiameters", "PipeDiameterID")
                  .WithColumn("ContactName").AsAnsiString(100).Nullable()
                  .WithColumn("ContactEmail").AsAnsiString(100).Nullable()
                  .WithColumn("ContactPhone").AsAnsiString(20).Nullable()
                  .WithColumn("InitialContactDate").AsDateTime().Nullable()
                  .WithForeignKeyColumn("CoordinateId", "Coordinates", "CoordinateID");

            Execute.Sql(@"
                declare @dataTypeId int
                insert into [DataType] (Data_Type, Table_Name) values('LargeServiceProjects', 'LargeServiceProjects')
                set @dataTypeId = (select @@IDENTITY)
                insert into [DocumentType] (Document_Type, DataTypeID) values('Document', @dataTypeId)");
        }

        public override void Down()
        {
            this.RemoveDataType("LargeServiceProjects");
            Delete.Table("LargeServiceProjects");
        }
    }
}
