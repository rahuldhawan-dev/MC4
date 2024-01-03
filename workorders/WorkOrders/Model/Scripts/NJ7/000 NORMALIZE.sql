/*
 * This script will normalize the imported 271 database in place, in the [WorkOrdersImport] db.
 */

use [WorkOrdersImport]
GO

/*
 * HERE ARE THE NOTES ON WHAT YOU'RE GOING TO RUN INTO IMPORTING THE ACCESS FILE.
 * APPARENTLY THERE'S NO WAY YOU CAN RELY ON SSIS (DTSX) FOR THIS (THOUGH IT HAS
 * BEEN WORKING LATELY).
 *
 * [tblMarkout].[Time Of Markout Request] - set to 'allow nulls'
 * [tblMarkout].[Date of M O Request] - change to varchar(666), set to 'allow nulls'
 * [tblMarkout].[Markout Due Date] - change to varchar(666), set to 'allow nulls'
 * [tblPaving].[Final Restoration Completion Date] - change to varchar(666)
 * [tblPaving].[Date Approved] - change to varchar(666)
 * [tblEmployeeWorkOrder].[Date Assigned] - change to varchar(666)
 * [tblWorkInputTable].[Markout Required Y or N] - set to 'allow nulls'
 * [tblWorkInputTable].[Date Started] - change to varchar(666)
 */

-----------------------------------------------------------------------------------------------------
----------------------------------------tblEmployeeWorkOrder-----------------------------------------
-----------------------------------------------------------------------------------------------------
-- fix bad dates in [tblEmployeeWorkOrder].[Date Assigned]
update
	[tblEmployeeWorkOrder]
set
	[Date Assigned] = null
where
	[Date Assigned] IS NOT NULL
and
	isDate([Date Assigned]) = 0;

-- make [tblEmployeeWorkOrder].[Date Assigned] a real datetime column
alter table [tblEmployeeWorkOrder]
alter column [Date Assigned] datetime null;

-----------------------------------------------------------------------------------------------------
---------------------------------------------tblMarkout----------------------------------------------
-----------------------------------------------------------------------------------------------------
-- fix bad dates in [tblMarkout].[Date of M O Request]
update
	[tblMarkout]
set
	[Date of M O Request] = null
where
	[Date of M O Request] IS NOT NULL
and
	isDate([Date of M O Request]) = 0;

-- fix bad dates in [tblMarkout].[Markout Due Date]
update
	[tblMarkout]
set
	[Markout Due Date] = null
where
	[Markout Due Date] IS NOT NULL
and
	isDate([Markout Due Date]) = 0;

-- make [tblMarkout].[Date of M O Request] a real datetime column
alter table [tblMarkout]
alter column [Date of M O Request] datetime null

-- make [tblMarkout].[Markout Due Date] a real datetime column
alter table [tblMarkout]
alter column [Markout Due Date] datetime null;

-- sorry about this. [Time of Markout Request] has some spaces in some weird spots.
-- this will fix that.  WEEEEEE!!!!! AIRPLANE!!!!!
update
	[tblMarkout]
set
	[Time of Markout Request] =
		Replace(
			Replace(
				Replace(
					Replace(
						Replace(
							Replace(
								Replace(
									Replace(
										Replace(
											Replace(
												Replace(
													Replace([Time of Markout Request], '12 :', '12:'),
													'11 :', '11:'),
												'10 :', '10:'),
											'9 :', '9:'),
										'8 :', '8:'),
									'7 :', '7:'),
								'6 :', '6:'),
							'5 :', '5:'),
						'4 :', '4:'),
					'3 :', '3:'),
				'2 :', '2:'),
			'1 :', '1:');

-- get rid of a bunch of beat values in [tblMarkout].[Time of Markout Request]
update
	[tblMarkout]
set
	[Time of Markout Request] = null
where
	[Time of Markout Request] <> ''
and
	[Time of Markout Request] is not null
and
	isdate([Time of Markout Request]) = 0

-- combine [tblMarkout].[Date of M O Request] with .[Time of Markout Request]
update
	[tblMarkout]
set
	[Date of M O Request] =
		cast(
			cast(datepart(year, [Date of M O Request]) as varchar) + '-' +
			cast(datepart(month, [Date of M O Request]) as varchar) + '-' +
			cast(datepart(day, [Date of M O Request]) as varchar) + ' ' +
			[Time of Markout Request]
		as datetime)
where
	[Date of M O Request] is not null
and
	[Time of Markout Request] is not null
and
	[Time of Markout Request] <> ''

-- get rid of a bunch of values in [tblMarkout].[Date of M O Request] where
-- the date value doesn't fit in a smalldatetime (and therefore is COMPLETELY
-- invalid).
update
	[tblMarkout]
set
	[Date of M O Request] = null
where
	[Date of M O Request] < cast('1900-01-01' as datetime)
or
	[Date of M O Request] > cast('2079-06-06' as datetime)

-- get rid of a bunch of values in [tblMarkout].[Markout Due Date] where
-- the date value doesn't fit in a smalldatetime (and therefore is COMPLETELY
-- invalid).
update
	[tblMarkout]
set
	[Markout Due Date] = null
where
	[Markout Due Date] < cast('1900-01-01' as datetime)
or
	[Markout Due Date] > cast('2079-06-06' as datetime)

-- need a [Date of M O Request], or the system will eat itself alive when
-- the data rolls out.  this will pull it from [Markout Due Date] 
update
	[tblMarkout]
set
	[Date of M O Request] = [Markout Due Date]
where
	[Date of M O Request] IS NULL

-- again, [Date of M O Request] is pretty darned important.  any remaining
-- records without it get the axe here.
delete from
	[tblMarkout]
where
	[Date of M O Request] IS NULL

-----------------------------------------------------------------------------------------------------
----------------------------------------------tblPaving----------------------------------------------
-----------------------------------------------------------------------------------------------------
-- add some ID lookup fields for importing
alter table
	[tblPaving]
add
	[RestorationTypeID] int null,
	[EightInchStabilizeBaseByCompanyForces] bit null,
	[SawCutByCompanyForces] bit null;
GO

-- fix bad dates in [tblPaving].[Final Restoration Completion Date]
update
	[tblPaving]
set
	[Final Restoration Completion Date] = null
where
	[Final Restoration Completion Date] IS NOT NULL
and
	isDate([Final Restoration Completion Date]) = 0;

-- fix bad dates in [tblPaving].[Date Approved]
update
	[tblPaving]
set
	[Date Approved] = null
where
	[Date Approved] IS NOT NULL
and
	isDate([Date Approved]) = 0;

-- make [tblPaving].[Final Restoration Completion Date] a real datetime column
alter table [tblPaving]
alter column [Final Restoration Completion Date] datetime null;

-- make [tblPaving].[Date Approved] a real datetime column
alter table [tblPaving]
alter column [Date Approved] datetime null;

-- fix some beat values in [tblPaving].[Type of Restoration]
update
	[tblPaving]
set
	[Type of Restoration] = CASE [Type of Restoration]
		WHEN 'CONCRETE-STREET' THEN 'CONCRETE STREET'
		WHEN 'ASPHALT- STREET' THEN 'ASPHALT-STREET'
		ELSE [Type of Restoration]
	END;

-- normalize from [Type of Restoration] (varchar) to
-- [RestorationTypeID] (int, lookup)
update
	[tblPaving]
set
	[RestorationTypeID] = rt.[RestorationTypeID]
from
	[WorkOrdersMerged].dbo.[RestorationTypes] as rt
where
	[tblPaving].[Type of Restoration] = rt.[Description]

-- [8" STAB BASE BY COMPANY FORCES] and [SAW CUT BY COMPANY FORCES]
-- should be bit values, so normalize them to [EightInchStabilizeBaseByCompanyForces]
-- and [SawCutByCompanyForces], respectively
update
	[tblPaving]
set
	[EightInchStabilizeBaseByCompanyForces] = CASE
		when [8" STAB BASE BY COMPANY FORCES] IS NOT NULL AND [8" STAB BASE BY COMPANY FORCES] <> 'No'
			then 1
		else 0
	end,
	[SawCutByCompanyForces] = CASE
		when [SAW CUT BY COMPANY FORCES] IS NOT NULL AND [SAW CUT BY COMPANY FORCES] <> 'No'
			then 1
		else 0
	end;

-- remove some weird values from [Partial Restoration Date],
-- [Final Restoration Completion Date], and [Date Approved]
update
	[tblPaving]
set
	[Partial Restoration Date] = null
where
	[Partial Restoration Date] < cast('1900-01-01' as datetime)
or
	[Partial Restoration Date] > cast('2079-06-06' as datetime)


update
	[tblPaving]
set
	[Final Restoration Completion Date] = null
where
	[Final Restoration Completion Date] < cast('1900-01-01' as datetime)
or
	[Final Restoration Completion Date] > cast('2079-06-06' as datetime)

update
	[tblPaving]
set
	[Date Approved] = null
where
	[Date Approved] < cast('1900-01-01' as datetime)
or
	[Date Approved] > cast('2079-06-06' as datetime)

-----------------------------------------------------------------------------------------------------
------------------------------------------tblWorkInputTable------------------------------------------
-----------------------------------------------------------------------------------------------------
-- fix bad dates in [tblWorkInputTable].[Date Started]
update
	[tblWorkInputTable]
set
	[Date Started] = null
where
	[Date Started] IS NOT NULL
and
	isDate([Date Started]) = 0;

-- make [tblWorkInputTable].[Date Started] a real datetime column
alter table [tblWorkInputTable]
alter column [Date Started] datetime null;

-- these were meant to be bit values
update
	[tblWorkInputTable]
set
	[Markout Required Y or N] =
		case [Markout Required Y or N]
			when 'Yes' then 1
			when 'No' then 0
			else null
		end

-- make [tblWorkInputTable].[Markout Required Y or N] a real bit column
alter table [tblWorkInputTable]
alter column [Markout Required Y or N] bit null;

-- there are some bad Latitude and Longitude values, this nulls them out
update
	[tblWorkInputTable]
set
	[Latitude] = 0,
	[Longitude] = 0
where
	IsNumeric([Latitude]) = 0
or
	IsNumeric([Longitude]) = 0

-- some of the Latitude values are too long, so this will cut them down a bit
update
	[tblWorkInputTable]
set
	[Latitude] = left(Latitude, 9)

-- make [tblWorkInputTable].[Latitude] a real float column
alter table [tblWorkInputTable]
alter column [Latitude] float null;

-- make [tblWorkInputTable].[Longitude] a real float column
alter table [tblWorkInputTable]
alter column [Longitude] float null;

-- add some ID lookup fields to aide in importing
alter table [tblWorkInputTable]
add	[CreatorID] int null,
	[StreetID] int null,
	[NearestCrossStreetID] int null,
	[TownID] int null,
	[TownSectionID] int null,
	[PriorityID] int null,
	[SupervisorApproval] int null,
	[BackhoeOperator] int null,
	[RequestingEmployeeID] int null,
	[WorkDescriptionID] int null,
	[OfficialInfo] text null,
	[RequesterID] int null,
	[Notes] text null,
	[PurposeID] int null,
	[ValveID] int null,
	[HydrantID] int null,
	[MainLineID] int null,
	[AssetTypeID] int null,
	[MarkoutRequirementID] int null;
GO

/* INSERT BIG ASSED CURSOR NONSENSE HERE */

-- set all [CreatorID] values to the userID of mcAdmin
-- as there were no usable values in the UserName column
update
	[tblWorkInputTable]
set
	[CreatorID] = p.[RecID]
from
	[Sql2000].[McProd].dbo.[tblPermissions] as p
where
	p.[UserName] = 'mcadmin'

-- this is just to be thorough.  Doug already normalized
-- these out for me (thanks Doug!)
update
	[tblWorkInputTable]
set
	[StreetID] = [StreetName]

-- there were 3 records where [StreetID] was zero
-- this will delete those (they were completed on the 13th)
delete from
	[tblWorkInputTable]
where
	[StreetID] = 0

-- normalize from [Nearest Cross Street] (varchar) to
-- [NearestCrossStreetID] (int, lookup)
update
	[tblWorkInputTable]
set
	[NearestCrossStreetID] = st.[RecID]
from
	[Sql2000].[McProd].dbo.[tblNJAWStreets] as st
where
	[Nearest Cross Street] is not null
and
	[tblWorkInputTable].[Town] is not null
and
	[tblWorkInputTable].[Nearest Cross Street] = st.[FullStName]
and
	[tblWorkInputTable].[Town] = st.[Town]

-- Loch Arbour uses the british spelling.
update
	[tblWorkInputTable]
set
	[Town] = 'LOCH ARBOUR'
where
	[Town] = 'Loch Arbor'

-- normalize from [Town] (varchar) to [TownID] (int, lookup)
update
	[tblWorkInputTable]
set
	[TownID] = tn.[RecID]
from
	[Sql2000].[McProd].dbo.[tblNJAWTownNames] as tn
where
	tn.[Town] = [tblWorkInputTable].[Town]

-- normalize from [Town Section] (varchar) to [TownSectionID] (int, lookup)
update
	[tblWorkInputTable]
set
	[TownSectionID] = ts.[RecID]
from
	[Sql2000].[McProd].dbo.[tblNJAWTwnSection] as ts
where
	ts.[Town] = [tblWorkInputTable].[Town]
and
	ts.[TwnSection] = [tblWorkInputTable].[Town Section]

-- fix an errant value in the [Job Priority] column
update
	[tblWorkInputTable]
set
	[Job Priority] = 'EMERGENCY'
where
	[Job Priority] = 'EMERGY'

-- set all [Job Priority] values to ROUTINE where
-- currently null.  there's no guessing at this really
update
	[tblWorkInputTable]
set
	[Job Priority] = 'ROUTINE'
where
	[Job Priority] IS NULL

-- normalize from [Job Priority] (varchar) to [PriorityID] (int, lookup)
update
	[tblWorkInputTable]
set
	PriorityID = p.[WorkOrderPriorityID]
from
	[WorkOrdersMerged].dbo.[WorkOrderPriorities] as p
where
	p.[Description] = [tblWorkInputTable].[Job Priority]

-- remove any non-employeeid values from [Backhoe Operator]
update
	[tblWorkInputTable]
set
	[Backhoe Operator] = null
where
	isNumeric([Backhoe Operator]) = 0

-- normalize from [Backhoe Operator] (varchar) to [BackhoeOperator] (int, lookup)
update
	[tblWorkInputTable]
set
	[BackhoeOperator] = emp.[RecID]
from
	[Sql2000].[McProd].dbo.[tblPermissions] as emp
where
	isNumeric([Backhoe Operator]) = 1
and
	[Backhoe Operator] = emp.[EmpNum]

-- add some [OfficialInfo] data, before normalizing the [Requested By] field
update
	[tblWorkInputTable]
set
	[OfficialInfo] = case [Requested By]
		when 'BORO' then 'Boro'
		when 'COUNTY' then 'County'
		when 'FIRE CO' then 'Fire Company'
		when 'FIRE DEPT' then 'Fire Department'
		when 'LBPD' then 'Police Department'
		when 'MIDD PUBLIC WORK' then 'Public Works Department'
		when 'POLICE' then 'Police Department'
		when 'STATE' then 'State'
		when 'TOWN' then 'Town'
		when 'TWP' then 'Township'
		when 'TWSP' then 'Township'
		when 'UNIN BCH' then 'Township'
	end;

-- normalize the string values in [Requested by]
update
	[tblWorkInputTable]
set
	[Requested By] = case [Requested By]
		when 'BORO' then 'Local Government'
		when 'CALL CEN' then 'Call Center'
		when 'CALL CENTER' then 'Call Center'
		when 'CO FORCES' then 'Employee'
		when 'COUNTY' then 'Local Government'
		when 'CUST' then 'Customer'
		when 'CUST.' then 'Customer'
		when 'CUSTOMER' then 'Customer'
		when 'FIRE CO' then 'Local Government'
		when 'FIRE DEPT' then 'Local Government'
		when 'LBPD' then 'Local Government'
		when 'MIDD PUBLIC WORK' then 'Local Government'
		when 'Office' then 'Employee'
		when 'OWNER' then 'Customer'
		when 'POLICE' then 'Local Government'
		when 'PRODUCTION' then 'Employee'
		when 'STATE' then 'Local Government'
		when 'TOWN' then 'Local Government'
		when 'TWP' then 'Local Government'
		when 'TWSP' then 'Local Government'
		when 'UNIN BCH' then 'Local Government'
	end
where
	[Requested By] IS NOT NULL
and
	isNumeric([Requested By]) = 0

-- normalize from [Requested By] to [RequestingEmployeeID]
update
	[tblWorkInputTable]
set
	[RequestingEmployeeID] = emp.[RecID],
	[Requested By] = 'Employee'
from
	[Sql2000].[McProd].dbo.[tblPermissions] as emp
where
	[tblWorkInputTable].[Requested By] IS NOT NULL
and
	isNumeric([tblWorkInputTable].[Requested By]) = 1
and
	[tblWorkInputTable].[Requested By] = emp.[EmpNum]
and
	emp.[FullName] <> 'Timothy Lake' -- idk how this happened

-- set [Requested By] to employee in cases where there was
-- an employee number, but it did not match any site users.
-- this will also append the employee number to the notes field
update
	[tblWorkInputTable]
set
	[Notes] = isNull(cast([Notes] as varchar) + char(13) + char(10), '') + 'Requested by Employee # ' + [Requested By],
	[Requested By] = 'Employee'
where
	[RequestingEmployeeID] is null
and
	isNumeric([Requested By]) = 1

-- set [Requested By] to customer where there is a customer name
-- and [Requested By] is still not set.
update
	[tblWorkInputTable]
set
	[Requested By] = 'Customer'
where
	[Requested By] is null
and
	[Customer Name] is not null

-- set [Requested By] to employee where [Requested By] is still
-- not set.  this also appends 'No Requester Information' to the
-- notes field
update
	[tblWorkInputTable]
set
	[Requested By] = 'Employee',
	[Notes] = isNull(cast([Notes] as varchar) + char(13) + char(10), '') + 'NO REQUESTER INFORMATION'
where
	[Requested By] IS NULL

-- normalize from [Requested By] to [RequesterID]
update
	[tblWorkInputTable]
set
	[RequesterID] = req.[WorkOrderRequesterID]
from
	[WorkOrdersMerged].dbo.[WorkOrderRequesters] as req
where
	[Requested By] = req.[Description]

-- fix some errant values in [Description of Job]
update
	[tblWorkInputTable]
set
	[Description of Job] = case [Description of Job]
		when 'LEAK IN TILE,OUTLET' then 'LEAK IN TILE, OUTLET'
		when 'NOT OUR WATER' then 'NOT OUR WATER/LEAK'
		when 'SERVIVE LINE LEAK ,CUST SIDE' then 'SERVICE LINE LEAK, CUST. SIDE'
		else [Description of Job]
	end;

-- delete some unnecessary records based on [Description of Job]
delete from
	[tblWorkInputTable]
where
	[Description of Job] in (
		'DAMAGE REPORT',
		'MAINTENANCE OF BUILDING/GROUNDS/VEHICLES',
		'NSPECTION',
		'TRAINING/MEETING',
		'Unknown'
)

-- copy some existing [Description of Job] values to the [Notes] field
update
	[tblWorkInputTable]
set
	[Notes] = isNull(cast([Notes] as varchar) + char(13) + char(10), '') + [Description of Job]
where
	[Description of Job] not in (select [Description] from [WorkOrdersMerged].dbo.[WorkDescriptions])

-- merge down some more values in [Description of Job]
update
	[tblWorkInputTable]
set
	[Description of Job] = case [Description of Job]
		when 'CONTRACTORS EXCAVATION' then 'SERVICE LINE RENEWAL'
		when 'CONTRACTORS LEAK' then 'SERVICE LINE RENEWAL'
		when 'MARKOUT' then 'MAINTENANCE OF MAIN'
		when 'RESTORATION REPAIR' then 'MAINTENANCE OF MAIN'
		when 'RESTORATION REPAIR COMPANY COMPLETED SERVICE' then 'SERVICE LINE LEAK, COMPANY SIDE'
		when 'TILE REPLACEMENT' then 'EXCAVATE METER TILE/SETTING'
		else [Description of Job]
	end;

-- normalize from [Description of Job] to [WorkDescriptionID]
update
	[tblWorkInputTable]
set
	[WorkDescriptionID] = wd.[WorkDescriptionID]
from
	[WorkOrdersMerged].dbo.[WorkDescriptions] as wd
where
	wd.[Description] = [tblWorkInputTable].[Description of Job]

-- they no longer want orders where WorkDescription is one of the following
delete from [tblWorkInputTable] where [WorkDescriptionID] IN (1, 13, 45, 46, 55, 63, 77);

-- there were 48 records where [WorkDescriptionID] could
-- not be ascertained because there was no value in
-- [Description of Job]. in the interest of time, this
-- will delete those
delete from [tblWorkInputTable]
where
	[WorkDescriptionID] is null

-- remove some errant values in [Date of Excavation]
update
	[tblWorkInputTable]
set
	[Date of Excavation] = null
where
	[Date of Excavation] < cast('1900-01-01' as datetime)
or
	[Date of Excavation] > cast('2079-06-06' as datetime)

-- set all PurposeID values to 'Compliance' for now.
-- if i remember correctly, there's a way to determine
-- purpose by work description, but i could be wrong
-- about that.
update
	[tblWorkInputTable]
set
	[PurposeID] = wp.[WorkOrderPurposeID]
from
	[WorkOrdersMerged].dbo.[WorkOrderPurposes] as wp
where
	wp.Description = 'Compliance'

-- normalize [Service Number] to [ValveID] where
-- [Service Number] is that of a valve
update
	[tblWorkInputTable]
set
	[ValveID] = v.[RecID]
from
	[Sql2000].[McProd].dbo.[tblNJAWValves] as v
where
	[tblWorkInputTable].[Service Number] IS NOT NULL
and
	[tblWorkInputTable].[Service Number] = v.[ValNum]
and	-- THIS NEEDS TO CHANGE IN SUBSEQUENT RUNS
	v.[OpCntr] = 'NJ7'

-- normalize [Service Number] to [HydrantID] where
-- [Service Number] is that of a hydrant
update
	[tblWorkInputTable]
set
	[HydrantID] = h.[RecID]
from
	[Sql2000].[McProd].dbo.[tblNJAWHydrant] as h
where
	[tblWorkInputTable].[Service Number] IS NOT NULL
and
	[tblWorkInputTable].[Service Number] = h.[HydNum]

-- someone over there has problems with the keys T and R on their keyboard
update
	[tblWorkInputTable]
set
	[Service Number] = CASE [Service Number]
		WHEN 'HST-7' THEN 'HSR-7'
		WHEN 'HST-3' THEN 'HSR-3'
		WHEN 'HST-4' THEN 'HSR-4'
		WHEN 'HST-22' THEN 'HSR-22'
		ELSE [Service Number]
	END;


-- normalize in from [tblMainLines] to [MainLineID]
update
	[tblWorkInputTable]
set
	[MainLineID] = m.[Main Line Number]
from
	[tblMainLines] as m
where
	m.[Order Number] = [tblWorkInputTable].[Order Number]

-- set [AssetTypeID] to it's proper value, based on
-- the various asset IDs
/* valves */
update
	[tblWorkInputTable]
set
	[AssetTypeID] = at.[AssetTypeID]
from
	[WorkOrdersMerged].dbo.[AssetTypes] as at
where
	[tblWorkInputTable].[Service Number] like 'V%'
and
	at.[Description] = 'Valve'

/* hydrants */
update
	[tblWorkInputTable]
set
	[AssetTypeID] = at.[AssetTypeID]
from
	[WorkOrdersMerged].dbo.[AssetTypes] as at
where
	[tblWorkInputTable].[Service Number] like 'H%'
and
	at.[Description] = 'Hydrant'

/* services */
update
	[tblWorkInputTable]
set
	[AssetTypeID] = at.[AssetTypeID]
from
	[WorkOrdersMerged].dbo.[AssetTypes] as at
where
	[tblWorkInputTable].[AssetTypeID] IS NULL
and
	[tblWorkInputTable].[Service Number] IS NOT NULL
and
	at.[Description] = 'Service'

/* mains */
update
	[tblWorkInputTable]
set
	[AssetTypeID] = at.[AssetTypeID]
from
	[WorkOrdersMerged].dbo.[AssetTypes] as at
where
	[tblWorkInputTable].[AssetTypeID] IS NULL
and
	at.[Description] = 'Main'

-- some hydrant and valve records from the imported data
-- didn't match the ones in the live database.  this will
-- append those asset numbers to the notes column.
/* valves */
update
	[tblWorkInputTable]
set
	[Notes] = isNull(cast([Notes] as varchar) + char(13) + char(10), '') + 'Valve # ' + [Service Number]
where
	[Service Number] like 'V%'
and
	[ValveID] IS NULL

/* hydrants */
update
	[tblWorkInputTable]
set
	[Notes] = isNull(cast([Notes] as varchar) + char(13) + char(10), '') + 'Hydrant # ' + [Service Number]
where
	[Service Number] like 'H%'
and
	[HydrantID] IS NULL

-- some of the records in [tblWorkInputTable] don't
-- link to any asset record.  actually, 10891 of them
-- don't.  this sets all those to 'Service', and adds
-- to the notes column
update
	[tblWorkInputTable]
set
	[Notes] = isNull(cast([Notes] as varchar) + char(13) + char(10), '') + 'NO ASSET INFORMATION',
	[AssetTypeID] = at.[AssetTypeID]
from
	[WorkOrdersMerged].dbo.[AssetTypes] as at
where
	[tblWorkInputTable].[AssetTypeID] IS NULL
and
	[tblWorkInputTable].[Service Number] IS NULL
and
	at.[Description] = 'Service'

-- there's just no way of having [WorkOrders] in the system
-- where [AssetType] is 'Valve' but [ValveID] is null
delete from
	[tblWorkInputTable]
where
	[AssetTypeID] = (select [AssetTypeID] from [WorkOrdersMerged].dbo.[AssetTypes] where [Description] = 'Valve')
and
	[ValveID] IS NULL

-- just like Valves there's just no way of having [WorkOrders] in the system
-- where [AssetType] is 'Hydrant' but [HydrantID] is null
delete from
	[tblWorkInputTable]
where
	[AssetTypeID] = (select [AssetTypeID] from [WorkOrdersMerged].dbo.[AssetTypes] where [Description] = 'Hydrant')
and
	[HydrantID] IS NULL

-- set [MarkoutRequirementID] to 'none' where [Markout Required Y or N]
-- is 0 or null
update
	[tblWorkInputTable]
set
	[MarkoutRequirementID] = mr.[MarkoutRequirementID]
from
	[WorkOrdersMerged].dbo.[MarkoutRequirements] as mr
where
	([Markout Required Y or N] is null or [Markout Required Y or N] = 0)
and
	mr.[Description] = 'None'

-- i figure that if an emergency markout were required, the
-- work would already be done.  this just sets [MarkoutRequirementID]
-- to Routine where it's still null
update
	[tblWorkInputTable]
set
	[MarkoutRequirementID] = mr.[MarkoutRequirementID]
from
	[WorkOrdersMerged].dbo.[MarkoutRequirements] as mr
where
	[Markout Required Y or N] = 1
and
	mr.[Description] = 'Routine'

-- that last statement was wrong-ish.  this will set the markout requirement to
-- 'emergency' where priority is 'emergency'.
update
	[tblWorkInputTable]
set
	[MarkoutRequirementID] = mr.[MarkoutRequirementID]
from
	[WorkOrdersMerged].dbo.[MarkoutRequirements] as mr
where
	[Markout Required Y or N] = 1
and
	[Job Priority] = 'EMERGENCY'
and
	mr.[Description] = 'Emergency'

-- setup tblWorkInputTable.[Order Number] as an identity column, make it the primary key,
BEGIN TRANSACTION
CREATE TABLE dbo.Tmp_tblWorkInputTable
	(
	[Order Number] int NOT NULL IDENTITY (1, 1),
	CreationDate datetime NULL,
	[Date Received] datetime NOT NULL,
	[Date Started] datetime NULL,
	[Customer Name] nvarchar(30) NULL,
	[Street Number] nvarchar(15) NULL,
	[Street Name] nvarchar(35) NULL,
	StreetName int NULL,
	[Nearest Cross Street] nvarchar(30) NULL,
	Town nvarchar(40) NULL,
	[Town Section] nvarchar(25) NULL,
	[Phone Number] nvarchar(15) NULL,
	[Customer Account Number] float(53) NULL,
	[Requested By] nvarchar(16) NULL,
	[Service Number] nvarchar(50) NULL,
	[Account Charged] nvarchar(15) NULL,
	[Description of Job] nvarchar(45) NULL,
	[Markout Required Y or N] bit NULL,
	[Job Priority] nvarchar(25) NULL,
	[Date Completed] datetime NULL,
	[print record] nvarchar(5) NULL,
	[Date Report Sent] datetime NULL,
	[Supervisor Approval] nvarchar(50) NULL,
	[Municipality Code] int NULL,
	[Backhoe Operator] nvarchar(250) NULL,
	[Date of Excavation] datetime NULL,
	[Date Completed On PC] datetime NULL,
	[Premise Number] int NULL,
	[Palm Work Order Number] int NULL,
	UserName nvarchar(50) NULL,
	InvoiceNumber nvarchar(50) NULL,
	Latitude float(53) NULL,
	Longitude float(53) NULL,
	CreatorID int NULL,
	StreetID int NULL,
	NearestCrossStreetID int NULL,
	TownID int NULL,
	TownSectionID int NULL,
	PriorityID int NULL,
	SupervisorApproval int NULL,
	BackhoeOperator int NULL,
	RequestingEmployeeID int NULL,
	WorkDescriptionID int NULL,
	OfficialInfo text NULL,
	RequesterID int NULL,
	Notes text NULL,
	PurposeID int NULL,
	ValveID int NULL,
	HydrantID int NULL,
	MainLineID int NULL,
	AssetTypeID int NULL,
	MarkoutRequirementID int NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Tmp_tblWorkInputTable ON
GO
IF EXISTS(SELECT * FROM dbo.tblWorkInputTable)
	 EXEC('INSERT INTO dbo.Tmp_tblWorkInputTable ([Order Number], CreationDate, [Date Received], [Date Started], [Customer Name], [Street Number], [Street Name], StreetName, [Nearest Cross Street], Town, [Town Section], [Phone Number], [Customer Account Number], [Requested By], [Service Number], [Account Charged], [Description of Job], [Markout Required Y or N], [Job Priority], [Date Completed], [print record], [Date Report Sent], [Supervisor Approval], [Municipality Code], [Backhoe Operator], [Date of Excavation], [Date Completed On PC], [Premise Number], [Palm Work Order Number], UserName, InvoiceNumber, Latitude, Longitude, CreatorID, StreetID, NearestCrossStreetID, TownID, TownSectionID, PriorityID, SupervisorApproval, BackhoeOperator, RequestingEmployeeID, WorkDescriptionID, OfficialInfo, RequesterID, Notes, PurposeID, ValveID, HydrantID, MainLineID, AssetTypeID, MarkoutRequirementID)
		SELECT [Order Number], CreationDate, [Date Received], [Date Started], [Customer Name], [Street Number], [Street Name], StreetName, [Nearest Cross Street], Town, [Town Section], [Phone Number], [Customer Account Number], [Requested By], [Service Number], [Account Charged], [Description of Job], [Markout Required Y or N], [Job Priority], [Date Completed], [print record], [Date Report Sent], [Supervisor Approval], [Municipality Code], [Backhoe Operator], [Date of Excavation], [Date Completed On PC], [Premise Number], [Palm Work Order Number], UserName, InvoiceNumber, Latitude, Longitude, CreatorID, StreetID, NearestCrossStreetID, TownID, TownSectionID, PriorityID, SupervisorApproval, BackhoeOperator, RequestingEmployeeID, WorkDescriptionID, OfficialInfo, RequesterID, Notes, PurposeID, ValveID, HydrantID, MainLineID, AssetTypeID, MarkoutRequirementID FROM dbo.tblWorkInputTable WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_tblWorkInputTable OFF
GO
DROP TABLE dbo.tblWorkInputTable
GO
EXECUTE sp_rename N'dbo.Tmp_tblWorkInputTable', N'tblWorkInputTable', 'OBJECT' 
GO
ALTER TABLE dbo.tblWorkInputTable ADD CONSTRAINT
	PK_tblWorkInputTable PRIMARY KEY CLUSTERED 
	(
	[Order Number]
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT

-- setup [tblEmployeeWorkOrder].[Job Number] as a primary key, and make it an identity column
-- and add a required constraint between [Order Number] in tblWorkInputTable
-- and tblEmployeeWorkOrder
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_tblEmployeeWorkOrder
	(
	[Job Number] int NOT NULL IDENTITY (1, 1),
	[Order Number] int NULL,
	[Employee Assigned to Job] nvarchar(15) NULL,
	[Date Assigned] datetime NULL,
	[Truck Number] float(53) NULL,
	[Total # of Employees on Job] float(53) NULL,
	[Total Time to Complete] float(53) NULL,
	[Job Notes] nvarchar(MAX) NULL,
	[Approved By] nvarchar(10) NULL,
	[Time Arrived On Job] datetime NULL,
	[Total Time to Completed] datetime NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Tmp_tblEmployeeWorkOrder ON
GO
IF EXISTS(SELECT * FROM dbo.tblEmployeeWorkOrder)
	 EXEC('INSERT INTO dbo.Tmp_tblEmployeeWorkOrder ([Job Number], [Order Number], [Employee Assigned to Job], [Date Assigned], [Truck Number], [Total # of Employees on Job], [Total Time to Complete], [Job Notes], [Approved By], [Time Arrived On Job], [Total Time to Completed])
		SELECT [Job Number], [Order Number], [Employee Assigned to Job], [Date Assigned], [Truck Number], [Total # of Employees on Job], [Total Time to Complete], [Job Notes], [Approved By], [Time Arrived On Job], [Total Time to Completed] FROM dbo.tblEmployeeWorkOrder WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_tblEmployeeWorkOrder OFF
GO
DROP TABLE dbo.tblEmployeeWorkOrder
GO
EXECUTE sp_rename N'dbo.Tmp_tblEmployeeWorkOrder', N'tblEmployeeWorkOrder', 'OBJECT' 
GO
ALTER TABLE dbo.tblEmployeeWorkOrder ADD CONSTRAINT
	PK_tblEmployeeWorkOrder PRIMARY KEY CLUSTERED 
	(
	[Job Number]
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.tblEmployeeWorkOrder WITH NOCHECK ADD CONSTRAINT
	FK_tblEmployeeWorkOrder_tblWorkInputTable_Order_Number FOREIGN KEY
	(
	[Order Number]
	) REFERENCES dbo.tblWorkInputTable
	(
	[Order Number]
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
