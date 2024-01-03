/*
This script includes all the various scripts that are necessary for updating the nonprod databases
1. Restoring
2. Cleaning
3. Shrinking 
*/

------------------------------------
-- 1. DROP AND RESTORE A DATABASE --
------------------------------------
IF EXISTS(SELECT * FROM sys.databases WHERE name = N'MapCallQA')
exec msdb.dbo.rds_cdc_disable_db 'MapCallQA'

IF EXISTS(SELECT * FROM sys.databases WHERE name = N'MapCallQA')
exec msdb.dbo.rds_drop_database  N'MapCallQA'

-- RESTORES FROM THE LATEST PROD BACKUP IN S3
exec msdb.dbo.rds_restore_database 
	@restore_db_name='MapCallQA', 
	@s3_arn_to_restore_from='arn:aws:s3:::mapcall-va-np-bucket/MapCallProdBackups/MCProdBackup.bak'

-- CHECK THE STATUS OF THE RESTORE
exec msdb.dbo.rds_task_status 'MapcallQA'


------------------------------------------------------
-- 2. CLEAN UP AND OBFUSCATE DATA AFTER THE RESTORE --
------------------------------------------------------

-- FIX THIS IMPORTANT USER
ALTER USER [mapcall] WITH LOGIN = [mapcall]
GO

-- ADD BACK CUSTOM USERS
IF EXISTS (SELECT * FROM sys.database_Principals WHERE name = N'FEALYN1')
	DROP USER [FEALYN1]
GO
IF EXISTS (SELECT * FROM sys.database_Principals WHERE name = N'KINSELVL')
	DROP USER [KINSELVL]
GO
IF EXISTS (SELECT * FROM sys.database_Principals WHERE name = N'alessam')
	DROP USER [alessam]
GO
CREATE USER [FEALYN1] FOR LOGIN [FEALYN1]
GO
ALTER USER [FEALYN1] WITH LOGIN = [FEALYN1]
GO
CREATE USER [KINSELVL] FOR LOGIN [KINSELVL]
GO
ALTER USER [KINSELVL] WITH LOGIN = [KINSELVL]
GO
CREATE USER [alessam] FOR LOGIN [alessam]
GO
ALTER USER [alessam] WITH LOGIN = [alessam]
GO
ALTER USER [ReadOnlyKafka] WITH LOGIN = [ReadOnlyKafka]
GO
ALTER ROLE [db_datareader] ADD MEMBER [ReadOnlyKafka]
GO
ALTER ROLE [db_datareader] ADD MEMBER [FEALYN1]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [FEALYN1]
GO
ALTER ROLE [db_ddladmin] ADD MEMBER [FEALYN1]
GO
ALTER ROLE [db_datareader] ADD MEMBER [KINSELVL]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [KINSELVL]
GO
ALTER ROLE [db_ddladmin] ADD MEMBER [KINSELVL]
GO
ALTER ROLE [db_datareader] ADD MEMBER [alessam]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [alessam]
GO
ALTER ROLE [db_ddladmin] ADD MEMBER [alessam]
GO

IF EXISTS (SELECT * FROM sys.database_Principals WHERE name = N'Migrations')
DROP USER [migrations]
GO
CREATE USER [migrations] FOR LOGIN [migrations]
GO
ALTER USER [migrations] WITH LOGIN = [migrations]
GO
ALTER ROLE [db_datareader] ADD MEMBER [mapcall]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [mapcall]
GO
ALTER ROLE [db_ddladmin] ADD MEMBER [migrations]
GO
ALTER ROLE [db_datareader] ADD MEMBER [migrations]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [migrations]
GO

GRANT EXECUTE ON dbo.aspnet_CheckSchemaVersion TO mapcall
GO
GRANT EXECUTE ON dbo.aspnet_Membership_GetPasswordWithFormat TO mapcall
GO
GRANT EXECUTE ON dbo.aspnet_Profile_GetProperties TO mapcall
GO
GRANT EXECUTE ON dbo.aspnet_Membership_GetUserByName TO mapcall
GO
GRANT EXECUTE ON dbo.aspnet_Membership_UnlockUser TO mapcall
GO
GRANT EXECUTE ON dbo.aspnet_Membership_UpdateUser TO mapcall
GO
GRANT EXECUTE ON dbo.aspnet_Profile_SetProperties TO mapcall
GO

-- Just to make sure we cover all SP's we might need in proper
GRANT EXECUTE TO [MapCall]

-- CLEAN UP DATA
update PositionGroups set GroupCode = 'A' + cast(Id as varchar)
DELETE FROM CovidIssues
TRUNCATE TABLE ASPXCookieTable

update 
	tblEmployee 
set 
	EmergencyContactName = null, 
	EmergencyContactPhone = null,
	Drivers_License = null,
	[Address] = null, 
	City = null, 
	[State] = null ,
	Phone_Cellular = null,
	Phone_Home = null,
	Phone_Work = null,
	Purchase_Card_Number = null;
	
Update 
	Incidents
SET
	QuestionWhatHappened = 'An incident occurred',
	MedicalProviderName = null, 
	MedicalProviderPhone = null,
	SupervisorEmployeeId = null,
	IncidentSummary = 'An incident occurred',
	AnyImmediateCorrectiveActionsApplied = 'n/a',
	ICRResults = 'n./a',
	ClaimsCarrierId = null;

delete from DocumentLink where DataTypeID in (92, 173); -- fmla, hepp;

update DriversLicenses set LicenseNumber = 'A12341234512345';

update 
	GeneralLiabilityClaims
set
	PhoneNumber = null,
	Address = null, 
	Email = null, 
	DriverName = null, 
	DriverPhone = null, 
	OtherDriver = null, 
	OtherDriverAddress = null,
	OtherDriverPhone = null, 
	VehicleVIN = null, 
	LicenseNumber = null;


-- UPDATE MAPCALL USERS 
update 
	tblPermissions 
set 
	HasAccess = 1 
where 
	username in ('FSRTEST1', 'FSRTEST2', 'FSRTEST3', 'PAWARR','SS3','GARCIAAX','KUMARR2','ADOFFG')

 -- Add some roles for Michael Lewis​
if not exists (select count(1) from roles where userid = 1932 and ApplicationID = 3 and moduleId = 29 and actionId = 1) 
    insert into roles values(null, 3, 29, 1, 1932)
if not exists (select count(1) from roles where userid = 1932 and ApplicationID = 2 and moduleId = 8 and actionId = 1) 
    insert into roles values(null, 2, 8, 1, 1932)
--Production Work Orders
if not exists (select count(1) from roles where userid = 1932 and ApplicationID = 2 and moduleId = 78 and actionId = 1) 
    insert into roles values(null, 2, 78, 1, 1932) 

if not exists (select count(1) from roles where userid = 2793 and ApplicationID = 2 and moduleId = 78 and actionId = 1) 
    insert into roles values(null, 2, 78, 1, 2793) 
if not exists (select count(1) from roles where userid = 1932 and ApplicationID = 2 and moduleId = 78 and actionId = 1) 
    insert into roles values(null, 2, 78, 1, 1932)
if not exists (select count(1) from roles where userid = 5451 and ApplicationID = 2 and moduleId = 78 and actionId = 1)
    insert into roles values(null, 2, 78, 1, 5451)
​
--T&D Work Orders
if not exists (select count(1) from roles where userid = 1932 and ApplicationID = 1 and moduleId = 34 and actionId = 1) 
    insert into roles values(null, 1, 34, 1, 1932)
if not exists (select count(1) from roles where userid = 2793 and ApplicationID = 1 and moduleId = 34 and actionId = 1) 
    insert into roles values(null, 1, 34, 1, 2793)
if not exists (select count(1) from roles where userid = 1932 and ApplicationID = 1 and moduleId = 55 and actionId = 1) 
    insert into roles values(null, 1, 55, 1, 1932)
​
-- 73 -- images 4783
if ((select count(1) from roles where userid = 4783 and ApplicationID = 1 and moduleId = 73 and actionId = 1) = 0)
    insert into roles values(null, 1, 73, 1, 4783)
if (select count(1) from roles where userid = 4783 and ApplicationID = 3 and moduleId = 29 and actionId = 1) = 0
    insert into roles values(null, 3, 29, 1, 4783)
​
-- STEINAB/2481 - assets/datalookups/images/reports/workmgmt
if not exists (select count(1) from roles where userid = 2481 and ApplicationID = 1 and moduleId = 1 and actionId = 1) 
    insert into roles values(null, 1, 1, 1, 2481)
if not exists (select count(1) from roles where userid = 2481 and ApplicationID = 1 and moduleId = 73 and actionId = 1) 
    insert into roles values(null, 1, 73, 1, 2481)
if not exists (select count(1) from roles where userid = 2481 and ApplicationID = 1 and moduleId = 55 and actionId = 1) 
    insert into roles values(null, 1, 55, 1, 2481)
if not exists (select count(1) from roles where userid = 2481 and ApplicationID = 1 and moduleId = 2 and actionId = 1) 
    insert into roles values(null, 1, 2, 1, 2481)
if not exists (select count(1) from roles where userid = 2481 and ApplicationID = 1 and moduleId = 34 and actionId = 1) 
    insert into roles values(null, 1, 34, 1, 2481)
​
--Environmental
if not exists (select count(1) from roles where userid = 1910 and ApplicationID = 18 and moduleId = 58 and actionId = 1)
    insert into roles values(null, 18, 58, 1, 1910) 
--Water Quality
if not exists (select count(1) from roles where userid = 1910 and ApplicationID = 14 and moduleId = 51 and actionId = 1)
    insert into roles values(null, 14, 51, 1, 1910) 
--Human Resources -> Facilities
if not exists (select count(1) from roles where userid = 1910 and ApplicationID = 3 and moduleId = 29 and actionId = 4)
    insert into roles values(null, 3, 29, 4, 1910) 
​
update tblPermissions set HasAccess = 1, IsSiteAdministrator = 1 where UserName in ('HERBERA','lewismd','schwenl')

-- Clean up audit log entries
SELECT TOP 1000 * INTO #AuditLogEntries FROM AuditLogEntries ORDER BY ID DESC;
DROP TABLE AuditLogEntries;
SELECT TOP 1000 * INTO AuditLogEntries FROM #AuditLogEntries;
DROP TABLE #AuditLogEntries;

update OperatingCenters set MapId ='0b39fbc4f5694257817b4b584e58c822', ArcMobileMapId = '6b964837e5bd4ffd9211d8013124b0ce'

DECLARE @OrphanedUsers TABLE
(
  IndexKey Int IDENTITY(1,1) PRIMARY KEY,
  UserName SysName,--nVarChar(128)
  UserSID  VarBinary(85)
)
INSERT INTO @OrphanedUsers
    EXEC sp_change_users_login 'report'

DECLARE @CRLF as nVarChar
    SET @CRLF = CHAR(10) + '&' + CHAR(13)--NOTE: Carriage-Return/Line-Feed will only appear in PRINT statements, not SELECT statements.
DECLARE @Sql as nVarChar(MAX)
    SET @Sql = N''
DECLARE @IndexKey as Int
    SET @IndexKey = 1
DECLARE @MaxIndexKey as Int
    SET @MaxIndexKey = (SELECT COUNT(*) FROM @OrphanedUsers)
DECLARE @Count as Int
    SET @Count = 0
DECLARE @UsersFixed as nVarChar(MAX)
    SET @UsersFixed = N''
DECLARE @UserName as SysName--This is an orphaned Database user.

WHILE (@IndexKey <= @MaxIndexKey)
  BEGIN
    SET @UserName = (SELECT UserName FROM @OrphanedUsers WHERE IndexKey = @IndexKey)
    IF 1 = (SELECT COUNT(*) FROM sys.server_principals WHERE Name = @UserName)--Look for a match in the Server Logins.
      BEGIN
        SET @Sql = @Sql + 'EXEC sp_change_users_login ''update_one'', [' + @UserName + '], [' + @UserName + ']' + @CRLF
        SET @UsersFixed = @UsersFixed + @UserName + ', '
        SET @Count = @Count + 1
      END
    SET @IndexKey = @IndexKey + 1
  END

--PRINT @Sql
EXEC sp_executesql @Sql
--PRINT   'Total fixed: ' + CAST(@Count as VarChar) + '.  Users Fixed: ' + @UsersFixed
--SELECT ('Total fixed: ' + CAST(@Count as VarChar) + '.  Users Fixed: ' + @UsersFixed)[Fixed]
--EXEC sp_change_users_login 'report'--See all orphaned users still in the database.


------------------------------------------------
-- 3. SHRINK A DATABSE USING A SECONDARY FILE --
------------------------------------------------
-- Adding secondary file \"AnotherMapCallQA\" to primary data group");
-- This Could exist already in that case you'll have to give it different names
ALTER DATABASE [MapCallQA]
	ADD FILE(
		NAME = N'AnotherMapCallQA',
	FILENAME = N'D:\rdsdbdata\DATA\AnotherMapCallQA.ndf',
	SIZE = 20000000KB , FILEGROWTH = 10 %)
	TO
	FILEGROUP[PRIMARY]

-- SHRINK THE DATA FILE USING EMPTYFILE
-- Always fails expectedly with:
-- Cannot move all contents of file "MCProd_Data" to other places to complete the emptyfile operation.
DBCC SHRINKFILE(N'MCProd_Data' , EMPTYFILE);

--SHRINK THE LOG FILE AGAIN
DBCC SHRINKFILE(N'MCProd_Log' , 0, TRUNCATEONLY);

--SHRINK THE ORIGINAL DB FILE NOW THAT IT'S EMPTY
DBCC SHRINKFILE(N'MCProd_Data' , 5);

--SHRINK THE LOG FILE AGAIN
DBCC SHRINKFILE(N'MCProd_Log' , 0, TRUNCATEONLY);
