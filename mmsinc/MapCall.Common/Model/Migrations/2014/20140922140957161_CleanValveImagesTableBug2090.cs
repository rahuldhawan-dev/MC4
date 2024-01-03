using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140922140957161), Tags("Production")]
    public class CleanValveImagesTableBug2090 : Migration
    {
        public const string VALVE_IMAGES = "ValveImages";
        public const int MAX_DESCRIPTION_LENGTH = 50;

        private void CleanValveOpensColumn()
        {
            Create.Table("ValveOpenDirections")
                  .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
                  .WithColumn("Description").AsString(MAX_DESCRIPTION_LENGTH).Unique().NotNullable();

            Insert.IntoTable("ValveOpenDirections").Row(new {Description = "Right"});
            Insert.IntoTable("ValveOpenDirections").Row(new {Description = "Left"});

            Alter.Table("ValveImages")
                 .AddColumn("ValveOpenDirectionId")
                 .AsInt32()
                 .Nullable()
                 .ForeignKey("FK_ValveImages_ValveOpenDirections_ValveOpenDirectionId", "ValveOpenDirections", "Id");

            Execute.Sql(@"
                declare @rightDirection int
                declare @leftDirection int
                set @rightDirection = (select top 1 Id from ValveOpenDirections where Description = 'Right')
                set @leftDirection = (select top 1 Id from ValveOpenDirections where Description = 'Left')
                
                update [ValveImages] set [ValveOpenDirectionId] = @rightDirection where ValveOpens = 'Right'
                update [ValveImages] set [ValveOpenDirectionId] = @rightDirection where ValveOpens = 'ight'
                update [ValveImages] set [ValveOpenDirectionId] = @rightDirection where ValveOpens = 'R'
                update [ValveImages] set [ValveOpenDirectionId] = @rightDirection where ValveOpens = 'R.'

                update [ValveImages] set [ValveOpenDirectionId] = @leftDirection where ValveOpens = 'LEFF'
                update [ValveImages] set [ValveOpenDirectionId] = @leftDirection where ValveOpens = 'LEFI'
                update [ValveImages] set [ValveOpenDirectionId] = @leftDirection where ValveOpens = 'LEFT'
                update [ValveImages] set [ValveOpenDirectionId] = @leftDirection where ValveOpens = 'LEST'
                update [ValveImages] set [ValveOpenDirectionId] = @leftDirection where ValveOpens = 'LFFT'
                update [ValveImages] set [ValveOpenDirectionId] = @leftDirection where ValveOpens = 'LIFT'
                ");

            Delete.Column("ValveOpens").FromTable(VALVE_IMAGES);
        }

        private void CleanNormalPositionColumn()
        {
            Create.Table("ValveNormalPositions")
                  .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
                  .WithColumn("Description").AsString(MAX_DESCRIPTION_LENGTH).Unique().NotNullable();

            Insert.IntoTable("ValveNormalPositions").Row(new {Description = "Open"});
            Insert.IntoTable("ValveNormalPositions").Row(new {Description = "Closed"});

            Alter.Table("ValveImages")
                 .AddColumn("ValveNormalPositionId")
                 .AsInt32()
                 .Nullable()
                 .ForeignKey("FK_ValveImages_ValveNormalPositions_ValveNormalPositionId", "ValveNormalPositions", "Id");

            Execute.Sql(@"
                declare @openPosition int
                declare @closedPosition int
                set @openPosition = (select top 1 Id from ValveNormalPositions where Description = 'Open')
                set @closedPosition = (select top 1 Id from ValveNormalPositions where Description = 'Closed')
                
                update [ValveImages] set [ValveNormalPositionId] = @openPosition where NormalPosition = 'MALLY OPEN I'
                update [ValveImages] set [ValveNormalPositionId] = @openPosition where NormalPosition = 'NOMALLY OPEN'
                update [ValveImages] set [ValveNormalPositionId] = @openPosition where NormalPosition = 'NORAMALLY OPEN'
                update [ValveImages] set [ValveNormalPositionId] = @openPosition where NormalPosition = 'NORMAILY OPEN'
                update [ValveImages] set [ValveNormalPositionId] = @openPosition where NormalPosition = 'NORMALL OPEN'
                update [ValveImages] set [ValveNormalPositionId] = @openPosition where NormalPosition = 'NORMALLLY OPEN'
                update [ValveImages] set [ValveNormalPositionId] = @openPosition where NormalPosition = 'NORMALLX OPEN'
                update [ValveImages] set [ValveNormalPositionId] = @openPosition where NormalPosition = 'NORMALLY  OPEN'
                update [ValveImages] set [ValveNormalPositionId] = @openPosition where NormalPosition = 'NORMALLY DPEN'
                update [ValveImages] set [ValveNormalPositionId] = @openPosition where NormalPosition = 'NORMALLY OFEN'
                update [ValveImages] set [ValveNormalPositionId] = @openPosition where NormalPosition = 'NORMALLY OPBN'
                update [ValveImages] set [ValveNormalPositionId] = @openPosition where NormalPosition = 'NORMALLY OPEN'
                update [ValveImages] set [ValveNormalPositionId] = @openPosition where NormalPosition = 'NORMALLY OPEN.'
                update [ValveImages] set [ValveNormalPositionId] = @openPosition where NormalPosition = 'NORMALLY OPEN]'
                update [ValveImages] set [ValveNormalPositionId] = @openPosition where NormalPosition = 'NORMALLY OPENS'
                update [ValveImages] set [ValveNormalPositionId] = @openPosition where NormalPosition = 'ORMALLY OPFN'
                update [ValveImages] set [ValveNormalPositionId] = @openPosition where NormalPosition = 'NORMALLYOPEN'
                update [ValveImages] set [ValveNormalPositionId] = @openPosition where NormalPosition = 'NORMALY OPEN'
                update [ValveImages] set [ValveNormalPositionId] = @openPosition where NormalPosition = 'Open'

                update [ValveImages] set [ValveNormalPositionId] = @closedPosition where NormalPosition = 'CLOSED'
                update [ValveImages] set [ValveNormalPositionId] = @closedPosition where NormalPosition = 'NORMALIY CIOSED'
                update [ValveImages] set [ValveNormalPositionId] = @closedPosition where NormalPosition = 'NORMALLY  CLOSED'
                update [ValveImages] set [ValveNormalPositionId] = @closedPosition where NormalPosition = 'NORMALLY CLOSE'
                update [ValveImages] set [ValveNormalPositionId] = @closedPosition where NormalPosition = 'NORMALLY CLOSED'
                update [ValveImages] set [ValveNormalPositionId] = @closedPosition where NormalPosition = 'NORMALLYCLOSED'
                update [ValveImages] set [ValveNormalPositionId] = @closedPosition where NormalPosition = 'NC'
            
                ");

            Delete.Column("NormalPosition").FromTable(VALVE_IMAGES);
            CleanUpBadStringData();
        }

        private void CleanOperatingCenterData()
        {
            Alter.Table(VALVE_IMAGES)
                 .AddColumn("OperatingCenterId")
                 .AsInt32()
                 .Nullable()
                 .ForeignKey("FK_ValveImages_OperatingCenters_OperatingCenterId", "OperatingCenters",
                      "OperatingCenterId");

            // execute update
            Execute.Sql(@"
            update [ValveImages] set ValveImages.OperatingCenterId = oc.OperatingCenterId
            from [ValveImages]
            inner join [OperatingCenters] oc on ValveImages.OperatingCenter = oc.OperatingCenterCode 
            ");

            Execute.Sql(@"
                update [ValveImages] set OperatingCenterId = oct.OperatingCenterId
                from [ValveImages]
                left join [OperatingCentersTowns] oct on oct.TownID = ValveImages.TownID 
                where [ValveImages].OperatingCenterId is null
                ");

            Delete.Column("OperatingCenter").FromTable(VALVE_IMAGES);

            Alter.Table(VALVE_IMAGES)
                 .AlterColumn("OperatingCenterId").AsInt32().NotNullable();
        }

        private void CleanUpBadStringData()
        {
            Execute.Sql(@"
                update [ValveImages] set TownSection = null where TownSection = ''
                update [ValveImages] set ValveNumber = null where ValveNumber = ''
                update [ValveImages] set StreetNumber = null where StreetNumber = ''
                update [ValveImages] set StreetPrefix = null where StreetPrefix = ''
                update [ValveImages] set Street = null where Street = ''
                update [ValveImages] set StreetSuffix = null where StreetSuffix = ''
                update [ValveImages] set XStreetPrefix = null where XStreetPrefix = ''
                update [ValveImages] set CrossStreet = null where CrossStreet = ''
                update [ValveImages] set XStreetSuffix = null where XStreetSuffix = ''
                update [ValveImages] set DateCompleted = null where DateCompleted = ''
                update [ValveImages] set Location = null where Location = ''
                update [ValveImages] set WorkOrderNumber = null where WorkOrderNumber = ''
                update [ValveImages] set NumberOfTurns = null where NumberOfTurns = '' or NumberOfTurns = 'UNINDEXED'
");
        }

        public override void Up()
        {
            Delete.Column("Town").FromTable(VALVE_IMAGES);
            Delete.Column("State").FromTable(VALVE_IMAGES);
            Delete.Column("ZipCode").FromTable(VALVE_IMAGES);
            Delete.Column("OldValveID").FromTable(VALVE_IMAGES);
            Delete.Column("imgInData").FromTable(VALVE_IMAGES);
            // Only two rows using this column.
            Delete.Column("ApartmentNumber").FromTable(VALVE_IMAGES);

            // 27 rows have data, all of it is bad.
            Delete.Column("Gradient").FromTable(VALVE_IMAGES);

            // This can be retrieved via town.
            Delete.Column("DistrictID").FromTable(VALVE_IMAGES);

            // These stats/indexes need to get killed before modifying OperatingCenters.
            Execute.Sql(
                "IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[ValveImages]') AND name = N'_dta_index_NJValve_c_22_719055__K26_K2') DROP INDEX [_dta_index_NJValve_c_22_719055__K26_K2] ON [dbo].[ValveImages]");

            Execute.Sql(
                "IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[ValveImages]') AND name = N'_dta_index_ValveImages_5_1653685039__K26_K2_K36_K28_1') DROP INDEX [_dta_index_ValveImages_5_1653685039__K26_K2_K36_K28_1] ON [dbo].[ValveImages]");

            Execute.Sql(
                "IF EXISTS (SELECT 1 FROM sysindexes where [name] = '_dta_stat_1653685039_35_36_28_26') DROP STATISTICS ValveImages._dta_stat_1653685039_35_36_28_26");
            Execute.Sql(
                "IF EXISTS (SELECT 1 FROM sysindexes where [name] = '_dta_stat_1653685039_2_35_26_36_28') DROP STATISTICS ValveImages._dta_stat_1653685039_2_35_26_36_28");

            CleanOperatingCenterData();

            CleanNormalPositionColumn();
            CleanValveOpensColumn();
        }

        public override void Down()
        {
            Alter.Table(VALVE_IMAGES)
                 .AddColumn("Town").AsString(50).Nullable();
            Alter.Table(VALVE_IMAGES)
                 .AddColumn("State").AsString(2).Nullable();
            Alter.Table(VALVE_IMAGES)
                 .AddColumn("ZipCode").AsString(50).Nullable();
            Alter.Table(VALVE_IMAGES)
                 .AddColumn("ApartmentNumber").AsString(50).Nullable();
            Alter.Table(VALVE_IMAGES)
                 .AddColumn("Gradient").AsString(50).Nullable();
            Alter.Table(VALVE_IMAGES)
                 .AddColumn("OldValveID").AsInt32().Nullable();
            Alter.Table(VALVE_IMAGES)
                 .AddColumn("imgInData").AsBoolean().Nullable();
            Alter.Table(VALVE_IMAGES)
                 .AddColumn("DistrictID").AsInt32().Nullable();
            Alter.Table(VALVE_IMAGES)
                 .AddColumn("OperatingCenter").AsString(50).Nullable();

            Execute.Sql(@"
update [ValveImages] set ValveImages.OperatingCenter = oc.OperatingCenterCode
from [ValveImages]
inner join [OperatingCenters] oc on ValveImages.OperatingCenterId = oc.OperatingCenterId 
");

            Alter.Table(VALVE_IMAGES).AddColumn("ValveOpens").AsString(50).Nullable();

            Execute.Sql(@"
update [ValveImages] set ValveImages.ValveOpens = vod.Description
from [ValveImages]
inner join [ValveOpenDirections] vod on ValveImages.ValveOpenDirectionId = vod.Id 
");

            Alter.Table(VALVE_IMAGES).AddColumn("NormalPosition").AsString(50).Nullable();

            Execute.Sql(@"
update [ValveImages] set ValveImages.NormalPosition = vod.Description
from [ValveImages]
inner join [ValveNormalPositions] vod on ValveImages.ValveNormalPositionId = vod.Id 
");

            Delete.ForeignKey("FK_ValveImages_OperatingCenters_OperatingCenterId").OnTable(VALVE_IMAGES);
            Delete.Column("OperatingCenterId").FromTable(VALVE_IMAGES);

            Delete.ForeignKey("FK_ValveImages_ValveOpenDirections_ValveOpenDirectionId").OnTable(VALVE_IMAGES);
            Delete.Column("ValveOpenDirectionId").FromTable(VALVE_IMAGES);
            Delete.Table("ValveOpenDirections");

            Delete.ForeignKey("FK_ValveImages_ValveNormalPositions_ValveNormalPositionId").OnTable(VALVE_IMAGES);
            Delete.Column("ValveNormalPositionId").FromTable(VALVE_IMAGES);
            Delete.Table("ValveNormalPositions");
        }
    }
}
