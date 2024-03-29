-- https://dba.stackexchange.com/questions/121208/ddl-admin-vs-db-owner-permissions#121235 -- Reference
USE [MapCallQA] -- Change this to what ever DB you need
GO
/****** Object:  User [mapcall]    Script Date: 5/8/2020 11:09:09 AM ******/
ALTER USER [mapcall] WITH LOGIN = [mapcall] /* Since a mapcall user exists in prod, we need to alter not create */
GO
/****** Object:  User [migrations]    Script Date: 5/8/2020 11:09:09 AM ******/
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'migrations')
CREATE USER [migrations] FOR LOGIN [migrations] WITH DEFAULT_SCHEMA=[dbo]
GO
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'FEALYN1')
CREATE USER [FEALYN1] FOR LOGIN [FEALYN1] WITH DEFAULT_SCHEMA=[dbo]
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
ALTER ROLE [db_ddladmin] ADD MEMBER [FEALYN1]
GO
ALTER ROLE [db_datareader] ADD MEMBER [FEALYN1]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [FEALYN1]
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

-- Data things
DELETE FROM CovidIssues
TRUNCATE TABLE ASPXCookieTable
update tblPermissions set HasAccess = 1 where username in ('FSRTEST1', 'FSRTEST2', 'FSRTEST3', 'PAWARR','SS3','GARCIAAX','KUMARR2','ADOFFG','KUMARR2')
​
update 
    tblPermissions 
set
    IsUserAdministrator = 1
where 
    RecID = 1932
​
if not exists (select count(1) from roles where userid = 1932 and ApplicationID = 3 and moduleId = 29 and actionId = 1) 
    insert into roles values(null, 3, 29, 1, 1932)
if not exists (select count(1) from roles where userid = 1932 and ApplicationID = 2 and moduleId = 8 and actionId = 1) 
    insert into roles values(null, 2, 8, 1, 1932)
​
--Production Work Orders
if not exists (select count(1) from roles where userid = 1932 and ApplicationID = 2 and moduleId = 78 and actionId = 1) 
    insert into roles values(null, 2, 78, 1, 1932) 
if not exists (select count(1) from roles where userid = 3176 and ApplicationID = 2 and moduleId = 78 and actionId = 1) 
    insert into roles values(null, 2, 78, 1, 3176) 
if not exists (select count(1) from roles where userid = 2793 and ApplicationID = 2 and moduleId = 78 and actionId = 1) 
    insert into roles values(null, 2, 78, 1, 2793) 
if not exists (select count(1) from roles where userid = 1932 and ApplicationID = 2 and moduleId = 78 and actionId = 1) 
    insert into roles values(null, 2, 78, 1, 1932)
if (select count(1) from roles where userid = 5451 and ApplicationID = 2 and moduleId = 78 and actionId = 1) = 0
    insert into roles values(null, 2, 78, 1, 5451)
​
--T&D Work Orders
if not exists (select count(1) from roles where userid = 1932 and ApplicationID = 1 and moduleId = 34 and actionId = 1) 
    insert into roles values(null, 1, 34, 1, 1932)
if not exists (select count(1) from roles where userid = 2793 and ApplicationID = 1 and moduleId = 34 and actionId = 1) 
    insert into roles values(null, 1, 34, 1, 2793)
​
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
update
    Roles
set
    ActionID = 1
where exists (select 1 from tblPermissions where Email = 'Akash.Agrawal@amwater.com' and tblPermissions.RecID = Roles.UserId)
​
update tblPermissions set HasAccess = 1, IsSiteAdministrator = 1 where UserName = 'HERBERA'