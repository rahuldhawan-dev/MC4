-- CreateDatabase.sql

-- This script will create the 271 database, with
-- necessary values already populated into the
-- "resource" tables.

use [WorkOrdersTest]
GO

/*
	 THESE ARE NO LONGER USED.  THEY ARE ACTUALLY "TASK NUMBERS".
-- tracks the account numbers used
CREATE TABLE [AccountNumbers] ( --tblAccountNumbers
	   [AccountNumberID] int unique identity not null, --n/a
	   [AccountNumber] varchar(8) not null, --ACCOUNT NUMBER
	   [Description] text null, --DESCRIPTION
	   CONSTRAINT [PK_AccountNumbers] PRIMARY KEY CLUSTERED (
		  [AccountNumberID] ASC
	   ) ON [PRIMARY]
) ON [PRIMARY]
*/

-----------------------------------------------------------------------------------------------------
--------------------------------------------OTHER TABLES---------------------------------------------
-----------------------------------------------------------------------------------------------------

---------------------------------------------COORDINATES---------------------------------------------
CREATE TABLE [Coordinates] (
	[CoordinateID] int unique identity not null,
	[Latitude] decimal(18, 2) not null,
	[Longitude] decimal(18, 2) not null,
	CONSTRAINT [PK_Coordinates] PRIMARY KEY CLUSTERED (
	[CoordinateID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]

-----------------------------------------------------------------------------------------------------
---------------------------------------------WORK ORDERS---------------------------------------------
-----------------------------------------------------------------------------------------------------

---------------------------------------------WORK ORDERS---------------------------------------------
--NEED A SCREEN FOR SUPERVISORS TO ASSIGN THESE, BASED ON
--THEIR AGE, PRIORITY, (EVENTUALLY PROXIMITY).
--NEED TIME TO COMPLETE FOR THE VARIOUS JOB DESCRIPTIONS.
CREATE TABLE [WorkOrders] ( --tblWorkInputTable
	[WorkOrderID] int unique identity not null, --Order Number
	[CreatedOn] smalldatetime not null DEFAULT (GetDate()), --CreationDate
-- 20081015 Added CreatorID field, lookup to Employees
	[CreatorID] int not null,
	[DateReceived] smalldatetime null, --Date Received
	[DateStarted] smalldatetime null, --Date Started
	[CustomerName] varchar(30) null, --Customer Name
	[StreetNumber] varchar(20) null, --Street Number
	[StreetID] int null, --Street Name (lookup to streets table)
	[NearestCrossStreetID] int null, --Nearest Cross Street (lookup to streets table)
	[TownID] int not null, --Town (lookup to towns table)
	[TownSectionID] int null, --Town Section (lookup to town sections table)
-- 20081016 Added ZipCode field
	[ZipCode] varchar(10) null,
	[PhoneNumber] varchar(14) null, --Phone Number
-- 20081014 Added SecondaryPhoneNumber field
	[SecondaryPhoneNumber] varchar(14) null,
	[CustomerAccountNumber] varchar(11) null, --Customer Account Number
-- 20080910 Replaced RequestedBy varchar field with reference to WorkOrderRequester
--	[RequestedBy] varchar(16) null, --Requested By
	[RequesterID] int not null,
	[ServiceNumber] varchar(50) null, --Service Number (already have in services)
	--these should be 8 chars.  15 here is erroneous.
	[AccountCharged] varchar(15) null, --Account Charged (task number)
-- 20081009 MarkoutRequired has become a logical property based on the value of MarkoutRequirement
--	[MarkoutRequired] bit not null, --Markout Required Y or N
	[PriorityID] int not null, --Job Priority (lookup to WorkOrderPriorities table)
	[DateCompleted] smalldatetime null, --Date Completed
	[Notes] text null,
	[DatePrinted] smalldatetime null, --print record (date this was "viewed" (perhaps "printed", and who printed it))
	--print record ? (seems to mostly be dates, but not entirely)
	[DateReportSent] smalldatetime null, --Date Report Sent
	[SupervisorApproval] int null, --Supervisor Approval (lookup to employees table)
	--not needed, covered by Town and TownSection
	--Municipality Code ? (only 1 value, "0")
	[BackhoeOperator] int null, --Backhoe Operator (tblEmployeeID?)
	[ExcavationDate] smalldatetime null, --Date of Excavation
	[DateCompletedPC] smalldatetime null, --Date Completed On PC (still needed?)
	[PremiseNumber] varchar(8) null, --Premise Number (lookup?) (i think this can be char(8))
	--not needed:
	--Palm Work Order Number ?
	--UserName ? (name of user entering the data?)

	--this relates to who completed the work order.  we should
	--also record contractor information, if this field is filled.
	--we should already have a table of contractors in services.
	[InvoiceNumber] varchar(15) null, --InvoiceNumber (lookup?)
-- 20080910 added RequestingEmployeeID field, lookup to Employees
	[RequestingEmployeeID] int null,
-- 20080909 added PurposeID field, lookup to WorkOrderPurposes
	[PurposeID] int not null,
-- 20080918 added AssetTypeID field, lookup to AssetTypes
	[AssetTypeID] int not null,
-- 20080923 added WorkDescriptionID field, lookup to WorkDescriptions
	[WorkDescriptionID] int not null,
-- 20080926 added MarkoutRequirementID field, lookup to MarkoutRequirements
	[MarkoutRequirementID] int not null,
-- 20081229 added TrafficControlRequired field
	[TrafficControlRequired] bit not null,
-- 20080930 added ValveID field, lookup to Valves
	[ValveID] int null,
-- 20081118 added HydrantID field, lookup to Hydrants
	[HydrantID] int null
	CONSTRAINT [PK_WorkOrders] PRIMARY KEY CLUSTERED (
	[WorkOrderID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [WorkOrders] WITH NOCHECK ADD CONSTRAINT [FK_WorkOrders_tblNJAWStreets_StreetID] FOREIGN KEY (
	[StreetID]
) REFERENCES [tblNJAWStreets] (
	[RecID]
)
GO
ALTER TABLE [WorkOrders] WITH NOCHECK ADD CONSTRAINT [FK_WorkOrders_tblNJAWStreets_NearestCrossStreetID] FOREIGN KEY (
	[NearestCrossStreetID]
) REFERENCES [tblNJAWStreets] (
	[RecID]
)
GO
ALTER TABLE [WorkOrders] WITH NOCHECK ADD CONSTRAINT [FK_WorkOrders_tblNJAWTownNames_TownID] FOREIGN KEY (
	[TownID]
) REFERENCES [tblNJAWTownNames] (
	[RecID]
)
GO
ALTER TABLE [WorkOrders] WITH NOCHECK ADD CONSTRAINT [FK_WorkOrders_TblNJAWTwnSection_TownSectionID] FOREIGN KEY (
	[TownSectionID]
) REFERENCES [TblNJAWTwnSection] (
	[RecID]
)
GO
ALTER TABLE [WorkOrders] WITH NOCHECK ADD CONSTRAINT [FK_WorkOrders_TblPermissions_RequestingEmployeeID] FOREIGN KEY (
	[RequestingEmployeeID]
) REFERENCES [TblPermissions] (
	[RecID]
)
GO
ALTER TABLE [WorkOrders] WITH NOCHECK ADD CONSTRAINT [FK_WorkOrders_tblNJAWValves_ValveID] FOREIGN KEY (
	[ValveID]
) REFERENCES [tblNJAWValves] (
	[RecID]
)
GO
ALTER TABLE [WorkOrders] WITH NOCHECK ADD CONSTRAINT [FK_WorkOrders_TblPermissions_CreatorID] FOREIGN KEY (
	[CreatorID]
) REFERENCES [TblPermissions] (
	[RecID]
)
GO

----------------------------------------EMPLOYEE WORK ORDERS----------------------------------------
-- record of each instance of a worker going to a work site
-- an putting in work on a work order.  work orders will
-- have 1 to an infinite number of these.
CREATE TABLE [EmployeeWorkOrders] ( --tblEmployeeWorkOrder
	   [EmployeeWorkOrderID] int unique identity not null, --Job Number?
	   [WorkOrderID] int not null, --Order Number? (lookup to WorkOrders table)
	   [AssignedTo] int null, --Employee Assigned to Job (lookup to tblPermissions)
	   [DateAssigned] smalldatetime null, --Date Assigned
	   [ApprovedBy] int null, --Approved By (lookup to tblPermissions)
	   [NumberOfEmployees] smallint not null, --Total # of Employees on Job (at least 1)
	   [TimeArrivedOnJob] smalldatetime null, --Time Arrived on Job
	   [TimeLeftJob] smalldatetime null,
	   [TotalManHours] smallint null,
-- 20080909 renamed TimeJobCompleted to TimeLeftJob
--	   [TimeJobCompleted] smalldatetime null, --Total Time to Completed
-- 20080909 renamed TotalTimeToComplete to TotalManHours
--	   [TotalTimeToComplete] smallint null, --Total Time to Complete (hours)
-- 20080909 added WorkCompleted field
	   [WorkCompleted] bit null,
-- 20080909 added DateTimeArrivedOnJobSet, shadow field for TimeArrivedOnJob
	   [DateTimeArrivedOnJobSet] smalldatetime null,
-- 20080909 added DateTimeLeftJobSet, shadow field for TimeLeftJob
	   [DateTimeLeftJobSet] smalldatetime null,
	   [JobNotes] text null, --Job Notes
	   CONSTRAINT [PK_EmployeeWorkOrders] PRIMARY KEY CLUSTERED (
		  [EmployeeWorkOrderID] ASC
	   ) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [EmployeeWorkOrders] WITH NOCHECK ADD CONSTRAINT [FK_EmployeeWorkOrders_WorkOrders_WorkOrderID] FOREIGN KEY (
	[WorkOrderID]
) REFERENCES [WorkOrders] (
	[WorkOrderID]
)
GO
ALTER TABLE [EmployeeWorkOrders] WITH NOCHECK ADD CONSTRAINT [FK_EmployeeWorkOrders_tblPermissions_AssignedTo] FOREIGN KEY (
	[AssignedTo]
) REFERENCES [tblPermissions] (
	[RecID]
)
GO
ALTER TABLE [EmployeeWorkOrders] WITH NOCHECK ADD CONSTRAINT [FK_EmployeeWorkOrders_tblPermissions_ApprovedBy] FOREIGN KEY (
	[ApprovedBy]
) REFERENCES [tblPermissions] (
	[RecID]
)
GO

---------------------------------------------LOST WATER---------------------------------------------
-- record of (estimated) water lost for a given work order.
-- should work as both a collection of instances for a work
-- order, as well as a property that will total the gallons
-- together.
CREATE TABLE [LostWater] ( --tblLostWater
	[LostWaterID] int unique identity not null, --Lost Water Number
	[WorkOrderID] int not null, --Order Number (lookup to WorkOrders table)
	[Gallons] int not null, --Lost Water (gallons)
	CONSTRAINT [PK_LostWater] PRIMARY KEY CLUSTERED (
	[LostWaterID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [LostWater] WITH NOCHECK ADD CONSTRAINT [FK_LostWater_WorkOrders_WorkOrderID] FOREIGN KEY (
	[WorkOrderID]
) REFERENCES [WorkOrders] (
	[WorkOrderID]
)
GO

----------------------------------------WORK ORDER PRIORITIES----------------------------------------
-- table of possible priority values for WorkOrders.
-- this is a "resource table".
CREATE TABLE [WorkOrderPriorities] (
	[WorkOrderPriorityID] int unique identity not null,
	[Description] varchar(15) not null,
	CONSTRAINT [PK_WorkOrderPriorities] PRIMARY KEY CLUSTERED (
	[WorkOrderPriorityID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [WorkOrders] WITH NOCHECK ADD CONSTRAINT [FK_WorkOrders_WorkOrderPriorities_PriorityID] FOREIGN KEY (
	[PriorityID]
) REFERENCES [WorkOrderPriorities] (
	[WorkOrderPriorityID]
)
GO

INSERT INTO [WorkOrderPriorities] ([Description])
VALUES ('Emergency');
INSERT INTO [WorkOrderPriorities] ([Description])
VALUES ('High Priority');
INSERT INTO [WorkOrderPriorities] ([Description])
VALUES ('Revenue Related');
INSERT INTO [WorkOrderPriorities] ([Description])
VALUES ('Routine');

-----------------------------------------WORK ORDER PURPOSES-----------------------------------------
CREATE TABLE [WorkOrderPurposes] (
	[WorkOrderPurposeID] int unique identity not null,
	[Description] varchar(10) not null,
	CONSTRAINT [PK_WorkOrderPurposes] PRIMARY KEY CLUSTERED (
	[WorkOrderPurposeID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [WorkOrders] WITH NOCHECK ADD CONSTRAINT [FK_WorkOrders_WorkOrderPurposes_PurposeID] FOREIGN KEY (
	[PurposeID]
) REFERENCES [WorkOrderPurposes] (
	[WorkOrderPurposeID]
)
GO

INSERT INTO [WorkOrderPurposes] ([Description])
VALUES ('Customer');
INSERT INTO [WorkOrderPurposes] ([Description])
VALUES ('Revenue');
INSERT INTO [WorkOrderPurposes] ([Description])
VALUES ('Compliance');
INSERT INTO [WorkOrderPurposes] ([Description])
VALUES ('Safety');

----------------------------------------WORK ORDER REQUESTERS----------------------------------------
CREATE TABLE [WorkOrderRequesters] (
	[WorkOrderRequesterID] int unique identity not null,
	[Description] varchar(16) not null,
	CONSTRAINT [PK_WorkOrderRequester] PRIMARY KEY CLUSTERED (
	[WorkOrderRequesterID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [WorkOrders] WITH NOCHECK ADD CONSTRAINT [FK_WorkOrders_WorkOrderRequesters_RequesterID] FOREIGN KEY (
	[RequesterID]
) REFERENCES [WorkOrderRequesters] (
	[WorkOrderRequesterID]
)
GO

INSERT INTO [WorkOrderRequesters] ([Description])
VALUES ('Customer');
INSERT INTO [WorkOrderRequesters] ([Description])
VALUES ('Employee');
INSERT INTO [WorkOrderRequesters] ([Description])
VALUES ('Local Government');
INSERT INTO [WorkOrderRequesters] ([Description])
VALUES ('Call Center');

---------------------------------------------ASSET TYPES---------------------------------------------
CREATE TABLE [AssetTypes] (
	[AssetTypeID] int unique identity not null,
	[Description] varchar(10) not null
	CONSTRAINT [PK_AssetTypes] PRIMARY KEY CLUSTERED (
	[AssetTypeID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [WorkOrders] WITH NOCHECK ADD CONSTRAINT [FK_WorkOrders_AssetTypes_AssetTypeID] FOREIGN KEY (
	[AssetTypeID]
) REFERENCES [AssetTypes] (
	[AssetTypeID]
)
GO

INSERT INTO [AssetTypes] ([Description])
VALUES ('Valve');
INSERT INTO [AssetTypes] ([Description])
VALUES ('Hydrant');
INSERT INTO [AssetTypes] ([Description])
VALUES ('Main');
INSERT INTO [AssetTypes] ([Description])
VALUES ('Service');

------------------------------------------WORK DESCRIPTIONS------------------------------------------
CREATE TABLE [WorkDescriptions] (
	[WorkDescriptionID] int unique identity not null,
	[Description] varchar(50) not null,
	[AssetTypeID] int not null
	CONSTRAINT [PK_WorkDescriptions] PRIMARY KEY CLUSTERED (
	[WorkDescriptionID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [WorkDescriptions] WITH NOCHECK ADD CONSTRAINT [FK_WorkDescriptions_AssetTypes_AssetTypeID] FOREIGN KEY (
	[AssetTypeID]
) REFERENCES [AssetTypes] (
	[AssetTypeID]
)
ALTER TABLE [WorkOrders] WITH NOCHECK ADD CONSTRAINT [FK_WorkOrders_WorkDescriptions_WorkDescriptionID] FOREIGN KEY (
	[WorkDescriptionID]
) REFERENCES [WorkDescriptions] (
	[WorkDescriptionID]
)
GO

INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('BACKFLOW PREVENTION', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('BLEEDERS & BLOW OFFS', 3);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('CHANGE BURST METER', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('CHECK NO WATER', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('CURB BOX REPAIR', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('CURB BOX REPAIR/EST', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('CURB BOX REPAIR/NON-PAY', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('CURB BOX REPAIR/PT', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('CURB STOP REPAIR', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('CURB STOP REPAIR/EST', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('CURB STOP REPAIR/NON-PAY', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('CURB STOP REPAIR/PT', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('EMERGENCY SHUT OFF FOR REPAIRS', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('EXCAVATE METER TILE/SETTING', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('EXCAVATE METER TILE/SETTING/EST', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('EXCAVATE METER TILE/SETTING/NON-PAY', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('EXCAVATE METER TILE/SETTING/PT', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('FLOW TEST', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('FROZEN HYDRANT', 2);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('FROZEN METER SET', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('FROZEN SERVICE LINE COMPANY SIDE', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('FROZEN SERVICE LINE CUST. SIDE', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('GROUND WATER', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('HYDRANT FLUSHING', 2);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('HYDRANT INSPECTION', 2);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('HYDRANT INSTALLATION', 2);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('HYDRANT LEAKING', 2);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('HYDRANT NO DRIP', 2);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('HYDRANT REPAIR', 2);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('HYDRANT REPLACEMENT', 2);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('HYDRANT RETIREMENT', 2);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('INACTIVE ACCOUNT', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('INSTALL 2" METER SETTING', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('INSTALL BLOW OFF VALVE', 1);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('INSTALL FIRE SERVICE', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('INSTALL LINE STOPPER', 3);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('INSTALL METER', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('INTERIOR SETTING REPAIR', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('INTERIOR SETTING REPAIR PT', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('LEAK AT CURB', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('LEAK IN STREET', 3);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('LEAK IN TILE, INLET', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('LEAK IN TILE, OUTLET', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('LEAK IN TILE,OUTLET', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('LEAK SURVEY', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('MAINTENANCE OF MAIN', 3);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('MAINTENANCE OF SERVICE', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('MAKE METER SET AT CURB', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('MAKE METER SET AT CURB/EST', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('METER CHANGE', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('METER TILE ADJUSTMENT/RESETTER', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('METER TILE ADJUSTMENT/RESETTER/EST', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('METER TILE ADJUSTMENT/RESETTER/NON-PAY', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('METER TILE ADJUSTMENT/RESETTER/PT', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('NEW MAIN FLUSHING', 3);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('NOT OUR WATER/LEAK', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('SERVICE LINE INSTALLATION', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('SERVICE LINE LEAK, COMPANY SIDE', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('SERVICE LINE LEAK, CUST. SIDE', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('SERVICE LINE RENEWAL', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('SERVICE LINE RETIRE', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('SUMP PUMP', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('TEST SHUT DOWN/INVESTIGATION', 3);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('TURN ON WATER', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('VALVE BOX REPAIR', 1);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('VALVE BOX REPAIR/B.O.', 1);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('VALVE BOX REPAIR/PT', 1);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('VALVE INSPECTION', 1);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('VALVE LEAKING', 1);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('VALVE REPAIR', 1);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('VALVE REPAIR/B.O.', 1);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('VALVE REPLACEMENT/INSTALL', 1);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('VALVE RETIREMENT', 1);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('WATER BAN/RESTRICTION VIOLATOR', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('WATER MAIN BREAK', 3);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('WATER MAIN INSTALLATION', 3);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('WATER MAIN RETIREMENT', 3);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('WATER METER MAINTENANCE', 3);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('WATER QUALITY / DIRTY WATER', 4);
INSERT INTO [WorkDescriptions] ([Description], [AssetTypeID]) VALUES ('WATER QUALITY/PRESSURE PROBLEM', 4);

-----------------------------------WORK ORDER DESCRIPTION CHANGES-----------------------------------
CREATE TABLE [WorkOrderDescriptionChanges] (
	[WorkOrderDescriptionChangeID] int unique identity not null,
	[WorkOrderID] int not null,
	[WorkDescriptionID] int not null,
	[ResponsibleEmployeeID] int not null,
	[DateOfChange] smalldatetime not null
	CONSTRAINT [PK_WorkOrderDescriptionChanges] PRIMARY KEY CLUSTERED (
	[WorkOrderDescriptionChangeID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [WorkOrderDescriptionChanges] WITH NOCHECK ADD CONSTRAINT [FK_WorkDescriptionChanges_WorkOrders_WorkOrderDescriptionChangeID] FOREIGN KEY (
	[WorkOrderID]
) REFERENCES [WorkOrders] (
	[WorkOrderID]
)
GO
ALTER TABLE [WorkOrderDescriptionChanges] WITH NOCHECK ADD CONSTRAINT [FK_WorkOrderDescriptionChanges_WorkDescriptions_WorkDescriptionID] FOREIGN KEY (
	[WorkDescriptionID]
) REFERENCES [WorkDescriptions] (
	[WorkDescriptionID]
)
GO
ALTER TABLE [WorkOrderDescriptionChanges] WITH NOCHECK ADD CONSTRAINT [FK_WorkOrderDescriptionChanges_TblPermissions_ResponsibleEmployeeID] FOREIGN KEY (
	[ResponsibleEmployeeID]
) REFERENCES [TblPermissions] (
	[RecID]
)
GO

-----------------------------------------------------------------------------------------------------
---------------------------------------------MAIN BREAKS---------------------------------------------
-----------------------------------------------------------------------------------------------------

---------------------------------------------MAIN BREAKS---------------------------------------------
-- record of reported main breaks for a given work
-- order, where the order can have 0 to an infinite number
-- of these.  there is also a one-to-many relationship to
-- the MainBreakValveOperations table.
CREATE TABLE [MainBreaks] ( --tblMainLines
	[MainBreakID] int unique identity not null,
	[WorkOrderID] int not null, --Order Number (lookup to WorkOrders table)
	[TypeOfMainBreak] varchar(30) null, --Type Of Main Break (description, like a notes field)
	[MainFailureTypeID] int null, --Type Of Failure (lookup to MainFailureTypes table, call it TypeOfMainFailure)
	[MapPage] varchar(10) null, --Map Page
	[MainSizeID] int not null, --Size Of Main (lookup to MainSizes table, call it Size, display [Size] from that table)
	[Material] varchar(20) not null, --Material Of Main (cannot yet be a lookup, to many variations)
	[Depth] decimal(18, 1) not null, --Depth Of Main (ft)
	[MainConditionID] int not null, --Condition Of Main (lookup to MainConditions table, call it Condition)
	[CustomersAffected] int not null, --Number of Customers Affected
	[ShutdownTime] decimal(5, 1) not null, --Total TIme of Shut Down (Hrs)
	[CoordinateID] int null, --lookup to coordinates table
	CONSTRAINT [PK_MainBreaks] PRIMARY KEY CLUSTERED (
	[MainBreakID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [MainBreaks] WITH NOCHECK ADD CONSTRAINT [FK_MainBreaks_WorkOrders_WorkOrderID] FOREIGN KEY (
	[WorkOrderID]
) REFERENCES [WorkOrders] (
	[WorkOrderID]
)
GO
ALTER TABLE [MainBreaks] WITH NOCHECK ADD CONSTRAINT [FK_MainBreaks_Coordinates_CoordinateID] FOREIGN KEY (
	[CoordinateID]
) REFERENCES [Coordinates] (
	[CoordinateID]
)
GO

-------------------------------------MAIN BREAK VALVE OPERATIONS-------------------------------------
-- record of each valve that was operated in dealing with
-- a main break.  when importing, the tblMainLines table
-- will have 2 of these for each record.
CREATE TABLE [MainBreakValveOperations] (
	[MainBreakValveOperationID] int unique identity not null,
	[MainBreakID] int not null, --lookup to MainBreaks table
	[ValveID] int not null, --lookup to valves table
	CONSTRAINT [PK_MainBreakValveOperations] PRIMARY KEY CLUSTERED (
	[MainBreakValveOperationID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [MainBreakValveOperations] WITH NOCHECK ADD CONSTRAINT [FK_MainBreakValveOperations_MainBreaks_MainBreakID] FOREIGN KEY (
	[MainBreakID]
) REFERENCES [MainBreaks] (
	[MainBreakID]
)
GO
ALTER TABLE [MainBreakValveOperations] WITH NOCHECK ADD CONSTRAINT [FK_MainBreakValveOperations_tblNJAWValves_ValveID] FOREIGN KEY (
	[ValveID]
) REFERENCES [tblNJAWValves] (
	[RecID]
)
GO

-----------------------------------------MAIN FAILURE TYPES-----------------------------------------
-- table of Failure Type descriptions for the MainBreaks
-- table.  this is a "resource table".
CREATE TABLE [MainFailureTypes] (
	[MainFailureTypeID] int unique identity not null,
	[Description] varchar(20) not null,
	CONSTRAINT [PK_MainFailureTypes] PRIMARY KEY CLUSTERED (
	[MainFailureTypeID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [MainBreaks] WITH NOCHECK ADD CONSTRAINT [FK_MainBreaks_MainFailureTypes_TypeOfFailure] FOREIGN KEY (
	[MainFailureTypeID]
) REFERENCES [MainFailureTypes] (
	[MainFailureTypeID]
)
GO

INSERT INTO [MainFailureTypes] ([Description])
VALUES ('Circle Break');
INSERT INTO [MainFailureTypes] ([Description])
VALUES ('Clamp Leak');
INSERT INTO [MainFailureTypes] ([Description])
VALUES ('Collar Leak');
INSERT INTO [MainFailureTypes] ([Description])
VALUES ('Coupling Leak');
INSERT INTO [MainFailureTypes] ([Description])
VALUES ('Deterioration');
INSERT INTO [MainFailureTypes] ([Description])
VALUES ('Joint Leak');
INSERT INTO [MainFailureTypes] ([Description])
VALUES ('Large Hole');
INSERT INTO [MainFailureTypes] ([Description])
VALUES ('Misc.');
INSERT INTO [MainFailureTypes] ([Description])
VALUES ('No Data');
INSERT INTO [MainFailureTypes] ([Description])
VALUES ('Physical Damage');
INSERT INTO [MainFailureTypes] ([Description])
VALUES ('Pin Hole');
INSERT INTO [MainFailureTypes] ([Description])
VALUES ('Service Saddle');
INSERT INTO [MainFailureTypes] ([Description])
VALUES ('Split\Fracture');
INSERT INTO [MainFailureTypes] ([Description])
VALUES ('Valve Leak');

---------------------------------------------MAIN SIZES---------------------------------------------
-- table of Main Sizes for SizeOfMain in the MainBreaks
-- table.  this is a "resource table".
CREATE TABLE [MainSizes] (
	[MainSizeID] int unique identity not null,
	[Size] decimal(5, 1) not null,
	CONSTRAINT [PK_MainSizes] PRIMARY KEY CLUSTERED (
	[MainSizeID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [MainBreaks] WITH NOCHECK ADD CONSTRAINT [FK_MainBreaks_MainSizes_Size] FOREIGN KEY (
	[MainSizeID]
) REFERENCES [MainSizes] (
	[MainSizeID]
)
GO

INSERT INTO [MainSizes] ([Size])
VALUES (0.75);
INSERT INTO [MainSizes] ([Size])
VALUES (1);
INSERT INTO [MainSizes] ([Size])
VALUES (1.25);
INSERT INTO [MainSizes] ([Size])
VALUES (1.5);
INSERT INTO [MainSizes] ([Size])
VALUES (2);
INSERT INTO [MainSizes] ([Size])
VALUES (2.25);
INSERT INTO [MainSizes] ([Size])
VALUES (2.5);
INSERT INTO [MainSizes] ([Size])
VALUES (3);
INSERT INTO [MainSizes] ([Size])
VALUES (4);
INSERT INTO [MainSizes] ([Size])
VALUES (5);
INSERT INTO [MainSizes] ([Size])
VALUES (6);
INSERT INTO [MainSizes] ([Size])
VALUES (8);
INSERT INTO [MainSizes] ([Size])
VALUES (10);
INSERT INTO [MainSizes] ([Size])
VALUES (12);
INSERT INTO [MainSizes] ([Size])
VALUES (16);
INSERT INTO [MainSizes] ([Size])
VALUES (18);
INSERT INTO [MainSizes] ([Size])
VALUES (20);
INSERT INTO [MainSizes] ([Size])
VALUES (24);
INSERT INTO [MainSizes] ([Size])
VALUES (36);

-------------------------------------------MAIN CONDITIONS-------------------------------------------
-- table of descriptions for the condition of
-- mains, used in the MainBreaks table.
-- this is a "resource table".
CREATE TABLE [MainConditions] (
	[MainConditionID] int unique identity not null,
	[Description] varchar(20) not null,
	CONSTRAINT [PK_MainConditions] PRIMARY KEY CLUSTERED (
	[MainConditionID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [MainBreaks] WITH NOCHECK ADD CONSTRAINT [FK_MainBreaks_MainConditions_Condition] FOREIGN KEY (
	[MainConditionID]
) REFERENCES [MainConditions] (
	[MainConditionID]
)
GO

INSERT INTO [MainConditions] ([Description])
VALUES ('Bad');
INSERT INTO [MainConditions] ([Description])
VALUES ('Dead');
INSERT INTO [MainConditions] ([Description])
VALUES ('Excellent');
INSERT INTO [MainConditions] ([Description])
VALUES ('Fair');
INSERT INTO [MainConditions] ([Description])
VALUES ('Fair/Poor');
INSERT INTO [MainConditions] ([Description])
VALUES ('Good');
INSERT INTO [MainConditions] ([Description])
VALUES ('New');
INSERT INTO [MainConditions] ([Description])
VALUES ('Old');
INSERT INTO [MainConditions] ([Description])
VALUES ('Poor');
INSERT INTO [MainConditions] ([Description])
VALUES ('Poor (Old)');
INSERT INTO [MainConditions] ([Description])
VALUES ('Very Bad');
INSERT INTO [MainConditions] ([Description])
VALUES ('Very Poor');
INSERT INTO [MainConditions] ([Description])
VALUES ('Very Poor (Retired)');

-----------------------------------------------------------------------------------------------------
----------------------------------------------MARKOUTS-----------------------------------------------
-----------------------------------------------------------------------------------------------------

----------------------------------------------MARKOUTS-----------------------------------------------
-- record of each required markout for a given work order.
-- certain types of work orders will require one of these.
-- if the work isn't done within a certain amount of time
-- (10 days) from the markout date, the markout must be
-- redone and so a new one of these will be created.
-- if one of these was created for a work order, the work
-- can't be done until all the required fields here are
-- filled in.  there is a one-to-many relationship with
-- the table IndividualMarkouts, for each markout type
-- (gas, electric, etc.).
CREATE TABLE [Markouts] ( --tblMarkout
	[MarkoutID] int unique identity not null, --Markout ID
	[WorkOrderID] int not null, --Order Number (lookup to WorkOrders table)
	[MarkoutNumber] varchar(15) not null, --Markout Number (should/can this be a lookup?)
	[MarkoutTypeID] int not null, --Type Of Markout -lookup to MarkoutTypes table
	[DateOfRequest] smalldatetime not null, --Date of M O Request
	[DueDate] smalldatetime not null, --Markout Due Date
	-- this comes from the town, it can't be a lookup:
	[StreetOpeningNumber] varchar(15) null, --Street Opening #
	[DownTime] decimal(5, 1) null, --Markout Down Time
	--Time Of Markout Request (covered by DateOfRequest)
/* these get separated and inserted into [IndividualMarkouts]
	[GasMarkedAt] smalldatetime null, --Gas Marked At (just time)
	[ElectricMarkedAt] smalldatetime null, --Electric Marked At (just time)
	[ATTMarkedAt] smalldatetime null, --ATT Marked At (just time)
	[BellTelephoneMarkedAt] smalldatetime null, --Bell Telephone At (just time)
	[SewerMarkedAt] smalldatetime null, --Sewer Marked At (just time)
	[CableMarkedAt] smalldatetime null, --Cable Marked At (just time)
*/
	[DateEntered] smalldatetime not null, --MARKOUT ENTERED DATE (default to current date/time)
	CONSTRAINT [PK_Markouts] PRIMARY KEY CLUSTERED (
	[MarkoutID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [Markouts] WITH NOCHECK ADD CONSTRAINT [FK_Markouts_WorkOrders_WorkOrderID] FOREIGN KEY (
	[WorkOrderID]
) REFERENCES [WorkOrders] (
	[WorkOrderID]
)
GO

-----------------------------------------INDIVIDUAL MARKOUTS-----------------------------------------
-- record of each individual markout that needs to occur for a given work
-- order.
CREATE TABLE [IndividualMarkouts] (
	[IndividualMarkoutID] int unique identity not null,
	[MarkoutID] int not null, --lookup to Markouts table
	[Description] varchar(20) not null, --gas, electric, telephone, etc.  will probably eventually become a lookup
	[MarkedAt] smalldatetime null, --may just need to be time
	CONSTRAINT [PK_IndividualMarkouts] PRIMARY KEY CLUSTERED (
	[IndividualMarkoutID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [IndividualMarkouts] WITH NOCHECK ADD CONSTRAINT [FK_IndividualMarkouts_Markouts_MarkoutID] FOREIGN KEY (
	[MarkoutID]
) REFERENCES [Markouts] (
	[MarkoutID]
)
GO

-----------------------------------------RESTORATION METHODS-----------------------------------------
-- table of possible methods of restoration for work
-- orders.  this is a "resource table"
CREATE TABLE [RestorationMethods] ( --TblMethods of Final Restoration
	[RestorationMethodID] int unique identity not null, --ID
	--Method ID (this is erroneous)
	[Description] text null, --Method of Final Restoration
	CONSTRAINT [PK_Restorations] PRIMARY KEY CLUSTERED (
	[RestorationMethodID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]

-- table of possible values for TypeOfMarkout in the
-- Markouts table.  this is a "resource table".
CREATE TABLE [MarkoutTypes] (
	[MarkoutTypeID] int unique identity not null,
	[Description] varchar(12) not null,
	CONSTRAINT [PK_MarkoutTypes] PRIMARY KEY CLUSTERED (
	[MarkoutTypeID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [Markouts] WITH NOCHECK ADD CONSTRAINT [FK_Markouts_MarkoutTypes_MarkoutTypeID] FOREIGN KEY (
	[MarkoutTypeID]
) REFERENCES [MarkoutTypes] (
	[MarkoutTypeID]
)
GO

INSERT INTO [MarkoutTypes] ([Description])
VALUES ('Three Day');
INSERT INTO [MarkoutTypes] ([Description])
VALUES ('Emergency');
INSERT INTO [MarkoutTypes] ([Description])
VALUES ('Update');

----------------------------------------MARKOUT REQUIREMENTS----------------------------------------
CREATE TABLE [MarkoutRequirements] (
	[MarkoutRequirementID] int unique identity not null,
	[Description] varchar(10) not null,
	CONSTRAINT [PK_MarkoutRequirements] PRIMARY KEY CLUSTERED (
	[MarkoutRequirementID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [WorkOrders] WITH NOCHECK ADD CONSTRAINT [FK_WorkOrders_MarkoutRequirements_MarkoutRequirementID] FOREIGN KEY (
	[MarkoutRequirementID]
) REFERENCES [MarkoutRequirements] (
	[MarkoutRequirementID]
)
GO

INSERT INTO [MarkoutRequirements] ([Description])
VALUES ('None');
INSERT INTO [MarkoutRequirements] ([Description])
VALUES ('Routine');
INSERT INTO [MarkoutRequirements] ([Description])
VALUES ('Emergency');

--------------------------------------------MARKOUT STATI--------------------------------------------
CREATE TABLE [MarkoutStatuses] (
	[MarkoutStatusID] int unique identity not null,
	[Description] varchar(10) not null,
	CONSTRAINT [PK_MarkoutStatuses] PRIMARY KEY CLUSTERED (
		[MarkoutStatusID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]

INSERT INTO [MarkoutStatuses] ([Description])
VALUES ('Pending');
INSERT INTO [MarkoutStatuses] ([Description])
VALUES ('Ready');
INSERT INTO [MarkoutStatuses] ([Description])
VALUES ('Overdue');

-----------------------------------------------------------------------------------------------------
----------------------------------------------MATERIALS----------------------------------------------
-----------------------------------------------------------------------------------------------------

-------------------------------------------MATERIALS USED--------------------------------------------
-- record of the materials used for a given work order.
-- work orders will have from 0 to an infinite number
-- of these entries.  stock numbers are company-wide,
-- but certain parts are only stock at certain facilities.
-- nonstockdescription stores info on items that aren't
-- stocked.
CREATE TABLE [MaterialsUsed] ( --tblMaterialsUsed
	[MaterialsUsedID] int unique identity not null, --MaterialID
	[WorkOrderID] int not null, --Order Number
	[MaterialID] int null, --Material Used - lookup to [Materials] table (show Stock Number)
	[Quantity] smallint not null, --Quantity
	[NonStockDescription] text null, --Non stock material used
	CONSTRAINT [PK_MaterialsUsed] PRIMARY KEY CLUSTERED (
	[MaterialsUsedID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [MaterialsUsed] WITH NOCHECK ADD CONSTRAINT [FK_MaterialsUsed_WorkOrders_WorkOrderID] FOREIGN KEY (
	[WorkOrderID]
) REFERENCES [WorkOrders] (
	[WorkOrderID]
)
GO

----------------------------------------------MATERIALS----------------------------------------------
-- table of stocked materials for work orders. this
-- is a "resource table".  Certain items are stock
-- at certain op centers, and not at others, so
-- we need a way to differentiate and filter the
-- list based on op code.
CREATE TABLE [Materials] ( --tblPartNumbers
	[MaterialID] int unique identity not null, --n/a
	[PartNumber] varchar(15) not null, --Part Number
	[Size] varchar(15) null, --Size
	[Description] text null, --Description
	--New Part Numbers
	CONSTRAINT [PK_Materials] PRIMARY KEY CLUSTERED (
	[MaterialID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [MaterialsUsed] WITH NOCHECK ADD CONSTRAINT [FK_MaterialsUsed_Materials_MaterialID] FOREIGN KEY (
	[MaterialID]
) REFERENCES [Materials] (
	[MaterialID]
)
GO

---------------------------------OPERATING CENTER STOCKED MATERIALS---------------------------------
-- table of which items a given op center will have as stock.
-- this is a "resource table".
CREATE TABLE [OperatingCenterStockedMaterials] (
	[OperatingCenterStockedMaterialID] int unique identity not null,
	[OperatingCenterID] int not null, -- lookup to OperatingCenters table
	[MaterialID] int not null, -- lookup to Materials table
	CONSTRAINT [PK_OperatingCenterStockedMaterials] PRIMARY KEY CLUSTERED (
	[OperatingCenterStockedMaterialID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [OperatingCenterStockedMaterials] WITH NOCHECK ADD CONSTRAINT [FK_OperatingCenterStockedMaterials_tblOpCntr_OperatingCenterID] FOREIGN KEY (
	[OperatingCenterID]
) REFERENCES [tblOpCntr] (
	[RecID]
)
GO
ALTER TABLE [OperatingCenterStockedMaterials] WITH NOCHECK ADD CONSTRAINT [FK_OperatingCenterStockedMaterials_Materials_MaterialID] FOREIGN KEY (
	[MaterialID]
) REFERENCES [Materials] (
	[MaterialID]
)
GO

-------------------------------------------SAFETY MARKERS-------------------------------------------
-- record of safety markers used on a given work order,
-- so that they can be tracked and returned.  a given
-- work order will have 0 or 1 entry here.  the system
-- used "Safety Markers left on site" and
-- "Number of Markers" to track the type and count of
-- markers used, but in my discussion with Doug we came
-- up with this and expect it to work much better.
CREATE TABLE [SafetyMarkers] ( --tblSafetyMarkers
	[SafetyMarkerID] int unique identity not null, --Safety Markers Number
	[WorkOrderID] int not null, --Order Number (lookup to WorkOrders table)
	[ConesOnSite] smallint null, --n/a
	[BaracadesOnSite] smallint null, --n/a
	[MarkersRetrievedOn] smalldatetime null, --Safety Markers Retrieved On
	--Safety Markers left on site
	--Number of Markers
	CONSTRAINT [PK_SafetyMarkers] PRIMARY KEY CLUSTERED (
	[SafetyMarkerID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [SafetyMarkers] WITH NOCHECK ADD CONSTRAINT [FK_SafetyMarkers_WorkOrders_WorkOrderID] FOREIGN KEY (
	[WorkOrderID]
) REFERENCES [WorkOrders] (
	[WorkOrderID]
)
GO

-----------------------------------------------------------------------------------------------------
-------------------------------------------LEAK DETECTION--------------------------------------------
-----------------------------------------------------------------------------------------------------

-------------------------------------------DETECTED LEAKS--------------------------------------------
-- record of each leak that's been detected and corrected
-- related to given work orders.  work orders will have
-- 0 to an infinite number of these.
CREATE TABLE [DetectedLeaks] ( --tblLeakDetection
	[DetectedLeakID] int unique identity not null, --Leak Location Number
	[WorkOrderID] int not null, --Order Number?
	[WorkAreaTypeID] int not null, --Type Of Area (lookup to WorkAreaTypes table)
	[AccessPointsAndContacts] text null, --Access Points and Contacts
	[SoundRecorded] bit null, --Sound Recorded
	[LeakReportingSourceID] int null, --Leak Reported By (lookup to LeakReportingSources table)
	[Mileage] decimal(18, 2) null, --Mileage
	-- "Sounded" fields are counts (no decimal)
	[HydrantsSounded] smallint null, --Hydrants Sounded
	[MainsSounded] smallint null, --Mains Sounded
	[ServicesSounded] smallint null, --Services Sounded
	[MapDistance] decimal(18, 2) null, --Map Distance
	-- these are valve numbers
	[SurveyStartingPointID] int null, --Survey Starting Point (lookup to valves table)
	[SurveyEndingPointID] int null, --Survey Ending Point (lookup to valves table)
	[MapPage] varchar(10) null, --Map Page
	[EquipmentUsed] varchar(25) null, --Equipment Used
	CONSTRAINT [PK_DetectedLeaks] PRIMARY KEY CLUSTERED (
	[DetectedLeakID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [DetectedLeaks] WITH NOCHECK ADD CONSTRAINT [FK_DetectedLeaks_WorkOrders_WorkOrderID] FOREIGN KEY (
	[WorkOrderID]
) REFERENCES [WorkOrders] (
	[WorkOrderID]
)
GO

ALTER TABLE [DetectedLeaks] WITH NOCHECK ADD CONSTRAINT [FK_DetectedLeaks_tblNJAWValves_SurveyStartingPoint] FOREIGN KEY (
	[SurveyStartingPointID]
) REFERENCES [tblNJAWValves] (
	[RecID]
)
GO

ALTER TABLE [DetectedLeaks] WITH NOCHECK ADD CONSTRAINT [FK_DetectedLeaks_tblNJAWValves_SurveyEndingPoint] FOREIGN KEY (
	[SurveyEndingPointID]
) REFERENCES [tblNJAWValves] (
	[RecID]
)
GO

-------------------------------------------WORK AREA TYPES-------------------------------------------
-- table of the various AreaTypes for leak detection.
-- this is a "resource table".
CREATE TABLE [WorkAreaTypes] (
	[WorkAreaTypeID] int unique identity not null,
	[Description] varchar(20) not null,
	CONSTRAINT [PK_WorkAreaTypes] PRIMARY KEY CLUSTERED (
	[WorkAreaTypeID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]

INSERT INTO [WorkAreaTypes] ([Description])
VALUES ('BUSINESS');
INSERT INTO [WorkAreaTypes] ([Description])
VALUES ('BUSINESS/RESIDENTIAL');
INSERT INTO [WorkAreaTypes] ([Description])
VALUES ('INDUSTRIAL');
INSERT INTO [WorkAreaTypes] ([Description])
VALUES ('RESIDENTIAL');

-------------------------------------------DETECTED LEAKS--------------------------------------------
ALTER TABLE [DetectedLeaks] WITH NOCHECK ADD CONSTRAINT [FK_DetectedLeaks_WorkAreaTypes_WorkAreaTypeID] FOREIGN KEY (
	[WorkAreaTypeID]
) REFERENCES [WorkAreaTypes] (
	[WorkAreaTypeID]
)
GO

---------------------------------------LEAK REPORTING SOURCES----------------------------------------
-- table of the various ReportingSources
-- for detected leaks.
-- this is a "resource table".
CREATE TABLE [LeakReportingSources] (
	[LeakReportingSourceID] int unique identity not null,
	[Description] varchar(25) not null,
	CONSTRAINT [PK_LeakReportingSources] PRIMARY KEY CLUSTERED (
	[LeakReportingSourceID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]

INSERT INTO [LeakReportingSources] ([Description])
VALUES ('Field Service Rep.');
INSERT INTO [LeakReportingSources] ([Description])
VALUES ('MLOG');
INSERT INTO [LeakReportingSources] ([Description])
VALUES ('Survey');
INSERT INTO [LeakReportingSources] ([Description])
VALUES ('VILADE');

ALTER TABLE [DetectedLeaks] WITH NOCHECK ADD CONSTRAINT [FK_DetectedLeaks_LeakReportingSources_LeakReportingSourceID] FOREIGN KEY (
	[LeakReportingSourceID]
) REFERENCES [LeakReportingSources] (
	[LeakReportingSourceID]
)
GO

------------------------------------------------CREWS------------------------------------------------

CREATE TABLE [Crews] (
	[CrewID] int unique identity not null,
        [Description] varchar(15) not null,
        [OperatingCenterID] int not null,
	CONSTRAINT [PK_Crews] PRIMARY KEY CLUSTERED (
		[CrewID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [Crews] WITH NOCHECK ADD CONSTRAINT [FK_Crews_tblOpCntr_OperatingCenterID] FOREIGN KEY (
	[OperatingCenterID]
) REFERENCES [tblOpCntr] (
	[RecID]
)
GO

-----------------------------------------------------------------------------------------------------
------------------------------------------------MISC.------------------------------------------------
-----------------------------------------------------------------------------------------------------

-- these should have already been there
ALTER TABLE [tblNJAWValves] WITH NOCHECK ADD CONSTRAINT [FK_Valves_Streets_StName] FOREIGN KEY (
	[StName]
) REFERENCES [tblNJAWStreets] (
	[RecID]
)
GO
ALTER TABLE [tblNJAWValves] WITH NOCHECK ADD CONSTRAINT [FK_Valves_Towns_Town] FOREIGN KEY (
	[Town]
) REFERENCES [tblNJAWTownNames] (
	[RecID]
)
GO
