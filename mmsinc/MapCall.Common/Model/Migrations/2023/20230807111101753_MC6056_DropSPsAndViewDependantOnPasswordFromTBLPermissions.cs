using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230807111101753), Tags("Production")]
    public class MC6056DropObjectsDpendantOnPasswordFromTBLPermissionsFor : Migration
    {
        public struct Sql
        {
            #region SPs/Views

            public const string
                SQL_REMOVE_VALVE_SPS_AND_VIEWS = @"
                if exists (select 1 from sysobjects where name = 'sp_AddUsr') drop procedure sp_AddUsr
                if exists (select 1 from sysobjects where name = 'ViewPasswords') drop view ViewPasswords",
                SQL_RESTORE_VALVE_SPS_AND_VIEWS = @"
                CREATE PROCEDURE [dbo].[sp_AddUsr] 
	                @Add1 varchar(50),
	                @CDCCode varchar(4),
	                @CellNum varchar(12),
	                @City varchar(30),
	                @Company varchar(4),
	                @EMail varchar(50),
	                @EmpNum varchar(15),
	                @FaxNum varchar(12),
	                @FullName varchar(50),
	                @Location varchar(3),
	                @lstRegion varchar(15),
	                @OpCntr1 varchar(4),
	                @OpCntr2 varchar(4),
	                @OpCntr3 varchar(4),
	                @OpCntr4 varchar(4),
	                @OpCntr5 varchar(4),
	                @OpCntr6 varchar(4),
	                @Password varchar(20),
	                @PhoneNum varchar(12),
	                @St varchar(2),
	                @UserLevel varchar(2),
	                @UserName varchar(20),
	                @Zip varchar(10),
	                @DefaultOperatingCenterID int, 
	                @WorkBasket varchar(9)
                AS
                insert into tblPermissions(Add1, CDCCode, CellNum, City, Company, EMail, EmpNum, FaxNum, FullName, Location, Region, OpCntr1, OpCntr2, OpCntr3, OpCntr4, OpCntr5, OpCntr6, Password, PhoneNum, St, UserLevel, UserName, Zip, DefaultOperatingCenterID, WorkBasket)
                values(@Add1, @CDCCode, @CellNum, @City, @Company, @EMail, @EmpNum, @FaxNum, @FullName, @Location, @lstRegion, @OpCntr1, @OpCntr2, @OpCntr3, @OpCntr4, @OpCntr5, @OpCntr6, @Password, @PhoneNum, @St, @UserLevel, @UserName, @Zip, @DefaultOperatingCenterID, @WorkBasket)
                if (@OpCntr1 <> '') insert into OperatingCentersUsers(UserID, OperatingCenterID) Values(@@Identity, (select operatingCenterID from OperatingCenters where OperatingCenterCode = @OpCntr1))
                if (@OpCntr2 <> '') insert into OperatingCentersUsers(UserID, OperatingCenterID) Values(@@Identity, (select operatingCenterID from OperatingCenters where OperatingCenterCode = @OpCntr2))
                if (@OpCntr3 <> '') insert into OperatingCentersUsers(UserID, OperatingCenterID) Values(@@Identity, (select operatingCenterID from OperatingCenters where OperatingCenterCode = @OpCntr3))
                if (@OpCntr4 <> '') insert into OperatingCentersUsers(UserID, OperatingCenterID) Values(@@Identity, (select operatingCenterID from OperatingCenters where OperatingCenterCode = @OpCntr4))
                if (@OpCntr5 <> '') insert into OperatingCentersUsers(UserID, OperatingCenterID) Values(@@Identity, (select operatingCenterID from OperatingCenters where OperatingCenterCode = @OpCntr5))
                if (@OpCntr6 <> '') insert into OperatingCentersUsers(UserID, OperatingCenterID) Values(@@Identity, (select operatingCenterID from OperatingCenters where OperatingCenterCode = @OpCntr6))

                GO

                CREATE VIEW dbo.ViewPasswords
                AS
                SELECT     TOP 100 PERCENT UserName, Password, FullName, EMail, OpCntr1, OpCntr2, OpCntr3, OpCntr4, OpCntr5, OpCntr6
                FROM         dbo.tblPermissions
                WHERE     (UserLevel = '2') OR
                                      (UserLevel = '5') OR
                                      (UserLevel = '7')
                ORDER BY FullName";

            #endregion
        }
        public override void Up()
        {
            Execute.Sql(Sql.SQL_REMOVE_VALVE_SPS_AND_VIEWS);
        }

        public override void Down()
        {
            Execute.Sql(Sql.SQL_RESTORE_VALVE_SPS_AND_VIEWS);
        }
    }
}

