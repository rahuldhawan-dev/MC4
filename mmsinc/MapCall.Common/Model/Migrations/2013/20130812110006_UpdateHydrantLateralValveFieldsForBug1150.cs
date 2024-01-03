using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130812110006), Tags("Production")]
    public class UpdateHydrantLateralValveFieldsForBug1150 : Migration
    {
        #region Constants

        public struct Sql
        {
            public const string UPDATE_LATERAL_VALVES_BEDMINSTER_RENAME =
                                    "UPDATE tblNJAWHydrant SET LatValNum = REPLACE(LatValNum, 'VBD-', 'VBED-') WHERE LatValNum like ('VBD-%') AND Town = 85",
                                UPDATE_HAS_NO_LATERAL_VALVE =
                                    "UPDATE tblNJAWHydrant SET HasNoLateralValve = 1 where LatValNum = 'N/A';",
                                UPDATE_LATERAL_VALVE_ID =
                                    "UPDATE tblNJAWHydrant SET LatValNum = V.RecID FROM tblNJAWHydrant H LEFT JOIN tblNJAWValves V ON V.ValNum = H.LatValNum AND V.Town = H.Town WHERE ISNULL(LatValNum,'') <> '';" +
                                    "UPDATE tblNJAWHydrant SET LatValNum = null where LatValNum = 0;",
                                DESTROY_STATISTICS =
                                    "if  exists (select * from sys.stats where name = N'_dta_stat_580913141_32_18_29_27_5_6' and object_id = object_id(N'[tblNJAWHydrant]'))" +
                                    "   DROP STATISTICS [dbo].[tblNJAWHydrant].[_dta_stat_580913141_32_18_29_27_5_6];",
                                RESTORE_LATERAL_VALVE_NUMBERS =
                                    "UPDATE tblNJAWHydrant SET LatValNum = V.ValNum FROM tblNJAWHydrant H JOIN tblNJAWValves V ON H.LatValNum = V.RecID",
                                UPDATE_STORED_PROCEDURES =
                                    @"IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RptHydrantLog]') AND type in (N'P', N'PC'))
                                        BEGIN
                                        EXEC dbo.sp_executesql @statement = N'

                                        /****** Object:  StoredProcedure [dbo].[RptHydrantLog]    Script Date: 05/31/2011 11:27:29 ******/
                                         ALTER Procedure [RptHydrantLog] (@opCntr varchar(4), @town int)
                                         AS
                                         IF (@town = 0 OR @town is null)
                                          BEGIN
                                           Select 
                                            tblNJAWHydrant.RecID, tblNJAWHydrant.OpCntr, 
                                            Towns.Town, tblNJAWHydrant.HydNum, 
                                            S.FullStName, tblNJAWHydrant.StName, 
                                            tblNJAWHydrant.Location, tblNJAWHydrant.CrossStreet, 
                                            tblNJAWHydrant.HydSize, M.Name as Manufacturer, 
                                            tblNJAWHydrant.SizeofMain, ValNum as LatValNum, 
                                            tblNJAWHydrant.WONum, 
                                            UPPER(tblNJAWHydrant.ActRet) as ActRet, 
                                            tblNJAWValves.RecID as ''ValRecID'',
                                            Upper(tblNJAWHydrant.BillInfo) as ''BillInfo'',
                                            IsNull((Select District_Name from FireDistrict where FireDistrict.FireDistrictID = tblNJAWHydrant.FireDistrictID), ''No fire district.'') as [FireDistrict], 
                                            LatSize, 
                                            tblNJAWHydrant.Lat, 
                                            tblNJAWHydrant.Lon
                                           from tblNJAWHydrant
                                           LEFT JOIN Towns on Towns.TownID = tblNJAWHydrant.Town
                                           LEFT JOIN Streets S on S.StreetID = tblNJAWHydrant.StName
                                           LEFT JOIN tblNJAWValves on tblNJAWValves.RecID = tblNJAWHydrant.LatValNum
                                           LEFT JOIN Manufacturers M on M.ManufacturerID = tblNJAWHydrant.ManufacturerID
                                           where tblNJAWHydrant.OpCntr = @opCntr
                                           order by Towns.Town, tblNJAWHydrant.hydsuf
                                          END
                                         ELSE
                                          BEGIN
                                           Select 
                                            tblNJAWHydrant.RecID, tblNJAWHydrant.OpCntr, 
                                            Towns.Town, tblNJAWHydrant.HydNum, 
                                            S.FullStName, tblNJAWHydrant.StName, 
                                            tblNJAWHydrant.Location, tblNJAWHydrant.CrossStreet, 
                                            tblNJAWHydrant.HydSize, M.Name as Manufacturer, 
                                            tblNJAWHydrant.SizeofMain, ValNum as LatValNum, 
                                            tblNJAWHydrant.WONum, 
                                            UPPER(tblNJAWHydrant.ActRet) as ActRet, 
                                            tblNJAWValves.RecID as ''ValRecID'',
                                            Upper(tblNJAWHydrant.BillInfo) as ''BillInfo'', 
                                            IsNull((Select District_Name from FireDistrict where FireDistrict.FireDistrictID = tblNJAWHydrant.FireDistrictID), ''No fire district.'') as [FireDistrict], 
                                            LatSize, 
                                            tblNJAWHydrant.Lat, 
                                            tblNJAWHydrant.Lon
                                           from tblNJAWHydrant
                                           LEFT JOIN Towns on Towns.TownID = tblNJAWHydrant.Town
                                           LEFT JOIN Streets S on S.StreetID = tblNJAWHydrant.StName
                                           LEFT JOIN tblNJAWValves on tblNJAWValves.RecID = tblNJAWHydrant.LatValNum 
                                           LEFT JOIN Manufacturers M on M.ManufacturerID = tblNJAWHydrant.ManufacturerID
                                           where tblNJAWHydrant.OpCntr = @opCntr and tblnjawhydrant.town = @town
                                           order by Towns.Town, tblNJAWHydrant.hydsuf
                                          END

                                        ' 
                                        END",
                                RESTORE_STORED_PROCEDURES =
                                    @"IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RptHydrantLog]') AND type in (N'P', N'PC'))
                                        BEGIN
                                        EXEC dbo.sp_executesql @statement = N'

                                        /****** Object:  StoredProcedure [dbo].[RptHydrantLog]    Script Date: 05/31/2011 11:27:29 ******/
                                            ALTER Procedure [RptHydrantLog] (@opCntr varchar(4), @town int)
                                            AS
                                            IF (@town = 0 OR @town is null)
                                            BEGIN
                                            Select 
                                            tblNJAWHydrant.RecID, tblNJAWHydrant.OpCntr, 
                                            Towns.Town, tblNJAWHydrant.HydNum, 
                                            S.FullStName, tblNJAWHydrant.StName, 
                                            tblNJAWHydrant.Location, tblNJAWHydrant.CrossStreet, 
                                            tblNJAWHydrant.HydSize, M.Name as Manufacturer, 
                                            tblNJAWHydrant.SizeofMain, tblNJAWHydrant.LatValNum, 
                                            tblNJAWHydrant.WONum, 
                                            UPPER(tblNJAWHydrant.ActRet) as ActRet, 
                                            tblNJAWValves.RecID as ''ValRecID'',
                                            Upper(tblNJAWHydrant.BillInfo) as ''BillInfo'',
                                            IsNull((Select District_Name from FireDistrict where FireDistrict.FireDistrictID = tblNJAWHydrant.FireDistrictID), ''No fire district.'') as [FireDistrict], 
                                            LatSize, 
                                            tblNJAWHydrant.Lat, 
                                            tblNJAWHydrant.Lon
                                            from tblNJAWHydrant
                                            LEFT JOIN Towns on Towns.TownID = tblNJAWHydrant.Town
                                            LEFT JOIN Streets S on S.StreetID = tblNJAWHydrant.StName
                                            LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWHydrant.LatValNum and tblNJAWValves.opCntr = tblNJAWHydrant.opCntr
                                            LEFT JOIN Manufacturers M on M.ManufacturerID = tblNJAWHydrant.ManufacturerID
                                            where tblNJAWHydrant.OpCntr = @opCntr
                                            order by Towns.Town, tblNJAWHydrant.hydsuf
                                            END
                                            ELSE
                                            BEGIN
                                            Select 
                                            tblNJAWHydrant.RecID, tblNJAWHydrant.OpCntr, 
                                            Towns.Town, tblNJAWHydrant.HydNum, 
                                            S.FullStName, tblNJAWHydrant.StName, 
                                            tblNJAWHydrant.Location, tblNJAWHydrant.CrossStreet, 
                                            tblNJAWHydrant.HydSize, M.Name as Manufacturer, 
                                            tblNJAWHydrant.SizeofMain, tblNJAWHydrant.LatValNum, 
                                            tblNJAWHydrant.WONum, 
                                            UPPER(tblNJAWHydrant.ActRet) as ActRet, 
                                            tblNJAWValves.RecID as ''ValRecID'',
                                            Upper(tblNJAWHydrant.BillInfo) as ''BillInfo'', 
                                            IsNull((Select District_Name from FireDistrict where FireDistrict.FireDistrictID = tblNJAWHydrant.FireDistrictID), ''No fire district.'') as [FireDistrict], 
                                            LatSize, 
                                            tblNJAWHydrant.Lat, 
                                            tblNJAWHydrant.Lon
                                            from tblNJAWHydrant
                                            LEFT JOIN Towns on Towns.TownID = tblNJAWHydrant.Town
                                            LEFT JOIN Streets S on S.StreetID = tblNJAWHydrant.StName
                                            LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWHydrant.LatValNum and tblNJAWValves.opCntr = tblNJAWHydrant.opCntr
                                            LEFT JOIN Manufacturers M on M.ManufacturerID = tblNJAWHydrant.ManufacturerID
                                            where tblNJAWHydrant.OpCntr = @opCntr and tblnjawhydrant.town = @town
                                            order by Towns.Town, tblNJAWHydrant.hydsuf
                                            END' 
                                        END";
        }

        public struct Tables
        {
            public const string HYDRANTS = "tblNJAWHydrant",
                                VALVES = "tblNJAWValves";
        }

        public struct Columns
        {
            public const string LATERAL_VALVE = "LatValNum",
                                HAS_NO_LATERAL_VALVE = "HasNoLateralValve",
                                VALVE_ID = "RecID",
                                HYDRANT_ID = "RecID";
        }

        public struct ForeignKeys
        {
            public const string FK_HYDRANTS_VALVES_LATERAL_VALVE = "FK_tblNJAWHydrant_tblNJAWValves_LatValNum";
        }

        public struct StringLengths
        {
            public const int LATERAL_VALVE = 10;
        }

        #endregion

        public override void Up()
        {
            Alter.Table(Tables.HYDRANTS).AddColumn(Columns.HAS_NO_LATERAL_VALVE).AsBoolean().Nullable();

            Execute.Sql(Sql.DESTROY_STATISTICS);
            Execute.Sql(Sql.UPDATE_HAS_NO_LATERAL_VALVE);
            Execute.Sql(Sql.UPDATE_LATERAL_VALVES_BEDMINSTER_RENAME);
            Execute.Sql(Sql.UPDATE_LATERAL_VALVE_ID);

            Alter.Table(Tables.HYDRANTS).AlterColumn(Columns.LATERAL_VALVE).AsInt32().Nullable();

            //-- add ForeignKey constraint
            Create.ForeignKey(ForeignKeys.FK_HYDRANTS_VALVES_LATERAL_VALVE)
                  .FromTable(Tables.HYDRANTS).ForeignColumn(Columns.LATERAL_VALVE)
                  .ToTable(Tables.VALVES).PrimaryColumn(Columns.VALVE_ID);

            Execute.Sql(Sql.UPDATE_STORED_PROCEDURES);
        }

        public override void Down()
        {
            Delete.ForeignKey(ForeignKeys.FK_HYDRANTS_VALVES_LATERAL_VALVE).OnTable(Tables.HYDRANTS);

            Delete.Column(Columns.HAS_NO_LATERAL_VALVE).FromTable(Tables.HYDRANTS);

            Alter.Table(Tables.HYDRANTS).AlterColumn(Columns.LATERAL_VALVE).AsAnsiString(StringLengths.LATERAL_VALVE)
                 .Nullable();

            Execute.Sql(Sql.RESTORE_LATERAL_VALVE_NUMBERS);
            Execute.Sql(Sql.RESTORE_STORED_PROCEDURES);
        }
    }
}
