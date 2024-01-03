-- This script will create the 271 database, with necessary values already populated into the
-- "resource" tables.  You'll need to set the specific db you're using before running.

-----------------------------------------------------------------------------------------------------
---------------------------------------------WORK ORDERS---------------------------------------------
-----------------------------------------------------------------------------------------------------

---------------------------------------------WORK ORDERS---------------------------------------------
--NEED A SCREEN FOR SUPERVISORS TO ASSIGN THESE, BASED ON
--THEIR AGE, PRIORITY, (EVENTUALLY PROXIMITY).
--NEED TIME TO COMPLETE FOR THE VARIOUS JOB DESCRIPTIONS.
CREATE TABLE [WorkOrders] ( --tblWorkInputTable
	[WorkOrderID] int unique identity not null, --Order Number
	[OldWorkOrderNumber] int null,
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
	[CustomerAccountNumber] varchar(12) null, --Customer Account Number
-- 20080910 Replaced RequestedBy varchar field with reference to WorkOrderRequester
--	[RequestedBy] varchar(16) null, --Requested By
	[RequesterID] int not null,
	[OfficialInfo] text null,
	[ServiceNumber] varchar(50) null, --Service Number (already have in services)
-- 20090205 Added ORCOMServiceOrderNumber
	[ORCOMServiceOrderNumber] varchar(20) null,
	--these should be 8 chars.  15 here is erroneous.
	[AccountCharged] varchar(30) null, --Account Charged (task number)
-- 20081009 MarkoutRequired has become a logical property based on the value of MarkoutRequirement
--	[MarkoutRequired] bit not null, --Markout Required Y or N
	[PriorityID] int not null, --Job Priority (lookup to WorkOrderPriorities table)
	[DateCompleted] smalldatetime null, --Date Completed
	[CompletedByID] int null,
	[Notes] text null,
	[DatePrinted] smalldatetime null, --print record (date this was "viewed" (perhaps "printed", and who printed it))
	--print record ? (seems to mostly be dates, but not entirely)
	[DateReportSent] smalldatetime null, --Date Report Sent
	[ApprovedByID] int null, --Supervisor Approval (lookup to employees table)
	--not needed, covered by Town and TownSection
	--Municipality Code ? (only 1 value, "0")
	[ApprovedOn] smalldatetime null, --Supervisor Approval
	[BackhoeOperator] int null, --Backhoe Operator (tblEmployeeID?)
	[ExcavationDate] smalldatetime null, --Date of Excavation
	[DateCompletedPC] smalldatetime null, --Date Completed On PC (still needed?)
	[PremiseNumber] varchar(9) null, --Premise Number (lookup?) (i think this can be char(8))
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
-- 20090205
	[StreetOpeningPermitRequired] bit not null,
-- 20080930 added ValveID field, lookup to Valves
	[ValveID] int null,
-- 20081118 added HydrantID field, lookup to Hydrants
	[HydrantID] int null,
-- 20090127 added LostWater field (to be replaced by a table reference later)
	[LostWater] int null,
-- 20090206 added RestorationID (reference to Restorations table)
	[RestorationID] int null,
-- 20090223 added MainLineID lookup to main lines (which doesn't exist)
	[MainLineID] int null,
-- 20090304 added NumberOfOfficersRequired
	[NumberOfOfficersRequired] int null,
-- 20090306 added Latitude/Longitude -arr
	[Latitude] float null, 
	[Longitude] float null,
	[OperatingCenterID] int null,
	[MaterialsApprovedByID] int null,
	[MaterialsApprovedOn] smalldatetime null,
	[MaterialsDocID] varchar(15) null,
	[SewerManholeID] int null,
	[DistanceFromCrossStreet] decimal(18, 2) null,
	/* Added 2010-04-05 */
	[OfficeAssignmentID] int null,
	[OfficeAssignedOn] smalldatetime null,
	/* Added 2010-09-28 Bug #: 622*/
	[OriginalOrderNumber] int null,
	/* Added 2010-10-27 Bug #: 730 */
	[BusinessUnit] char(6) NULL,
-- 20101011 -added stormwaterAssetID field, lookup to stormwaterAssets
	[StormCatchID] int null,
	[CustomerImpactRangeID] int null,
	[RepairTimeRangeID]	int	null,
	[AlertID] varchar(20) null,
	[SignificantTrafficImpact] bit null,
	[MarkoutToBeCalled] smalldatetime null,
	[MarkoutTypeNeededID] int null,
    [RequiredMarkoutNote] text null,
	[AssignedContractorID] int null,
	[AssignedToContractorOn] smalldatetime null,
-- 20120308 -added UpdatedMobileGIS field. Bug #1159
	[UpdatedMobileGIS] bit null,
	/*
	-- RESTORATION: --
	[RestorationTypeID] int null, -- Type of Restoration
	[PavingSquareFootage] int null, -- Paving Square Footage
	[LinearFeetOfCurb] int null, -- Linear Ft of Curb
	[RestorationNotes] text null, -- Restoration Notes
	[PartialRestorationInvoiceNumber] varchar(12) null, -- Partial Restoration Invoice Number
	[PartialRestorationDate] smalldatetime null,
	[PartialRestorationCompletedBy] varchar(10) null, -- Partial Restoration Completed By
	[PartialPavingBreakOutEightInches] int null, -- Partial Paving Break out 8"
	[PartialPavingBreakOutTenInches] int null, -- Partial Paving Break out 10"
	[PartialSawCutting] int null, -- Partial Saw Cutting
	[PartialPavingSquareFootage] int null,
	[DaysToPartialPaveHole] int null, -- Days to Partical Pave hole
	[TrafficControlHoursPartialRestoration] int null, -- Traffic Control Hours Partial Restoration
	[FinalRestorationInvoiceNumber] varchar(12) null, -- Final Restoration Invoice Number
	[FinalRestorationDate] smalldatetime null,
	[FinalRestorationCompletedBy] varchar(10) null, -- Final Restoration Completed By
	[FinalPavingBreakOutEightInches] int null, -- Final Concrete Break out 8"
	[FinalPavingBreakOutTenInches] int null, -- Final Concrete Break out 8"
	[FinalSawCutting] int null, -- Final Saw cutting
	[FinalPavingSquareFootage] int null,
	[DaysToFinalPaveHole] int null, -- Days to Final Pave hole
	[TrafficControlHoursFinalRestoration] int null, -- Traffic Control Hours Final Restoration
	[ApprovedByID] int null, -- Approved By
	[DateApproved] smalldatetime null, -- Date Approved
	[RejectedByID] int null,
	[DateRejected] smalldatetime null, -- Date Rejected
	[TypeOfAsphalt] varchar(35) null, -- Type of Asphalt
	[EightInchStabilizeBaseByCompanyForces] bit default 0 not null, -- 8" Stab Base By Company Forces
	[SawCutByCompanyForces] bit default 0 not null, -- Saw Cut By Company Forces
	*/

	CONSTRAINT [PK_WorkOrders] PRIMARY KEY CLUSTERED (
	[WorkOrderID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [WorkOrders] ADD CONSTRAINT [FK_WorkOrders_Streets_StreetID] FOREIGN KEY (
	[StreetID]
) REFERENCES [Streets] (
	[StreetID]
)
GO
ALTER TABLE [WorkOrders] ADD CONSTRAINT [FK_WorkOrders_Streets_NearestCrossStreetID] FOREIGN KEY (
	[NearestCrossStreetID]
) REFERENCES [Streets] (
	[StreetID]
)
GO
ALTER TABLE [WorkOrders] ADD CONSTRAINT [FK_WorkOrders_Towns_TownID] FOREIGN KEY (
	[TownID]
) REFERENCES [Towns] (
	[TownID]
)
GO
ALTER TABLE [WorkOrders] ADD CONSTRAINT [FK_WorkOrders_TownSections_TownSectionID] FOREIGN KEY (
	[TownSectionID]
) REFERENCES [TownSections] (
	[TownSectionID]
)
GO
ALTER TABLE [WorkOrders] ADD CONSTRAINT [FK_WorkOrders_TblPermissions_RequestingEmployeeID] FOREIGN KEY (
	[RequestingEmployeeID]
) REFERENCES [TblPermissions] (
	[RecID]
)
GO
ALTER TABLE [WorkOrders] ADD CONSTRAINT [FK_WorkOrders_tblPermissions_CompletedByID] FOREIGN KEY (
	[CompletedByID]
) REFERENCES [tblPermissions] (
	[RecID]
)
GO
ALTER TABLE [WorkOrders] ADD CONSTRAINT [FK_WorkOrders_tblNJAWValves_ValveID] FOREIGN KEY (
	[ValveID]
) REFERENCES [tblNJAWValves] (
	[RecID]
)
GO
ALTER TABLE [WorkOrders] ADD CONSTRAINT [FK_WorkOrders_tblNJAWHydrant_HydrantID] FOREIGN KEY (
	[HydrantID]
) REFERENCES [tblNJAWHydrant] (
	[RecID]
)
GO
ALTER TABLE [WorkOrders] ADD CONSTRAINT [FK_WorkOrders_SewerManholes_SewerManholeID] FOREIGN KEY (
	[SewerManholeID]
) REFERENCES [SewerManholes] (
	[SewerManholeID]
)
GO
ALTER TABLE [WorkOrders] WITH NOCHECK ADD CONSTRAINT [FK_WorkOrders_StormWaterAssets_StormWaterAssetID] FOREIGN KEY (
[StormCatchID]
) REFERENCES [StormWaterAssets] (
[StormWaterAssetID]
)
GO
ALTER TABLE [WorkOrders] ADD CONSTRAINT [FK_WorkOrders_TblPermissions_CreatorID] FOREIGN KEY (
	[CreatorID]
) REFERENCES [TblPermissions] (
	[RecID]
)
GO
ALTER TABLE [WorkOrders] ADD CONSTRAINT [FK_WorkOrders_TblPermissions_MaterialsApprovedByID] FOREIGN KEY (
	[MaterialsApprovedByID]
) REFERENCES [TblPermissions] (
	[RecID]
)
GO
ALTER TABLE [WorkOrders] WITH NOCHECK ADD CONSTRAINT [FK_WorkOrders_tblPermissions_OfficeAssignmentID] FOREIGN KEY (
	[OfficeAssignmentID]
) REFERENCES [tblPermissions] (
	[RecID]
)
GO
/* Added 2010-09-28 Bug #: 622*/
ALTER TABLE [WorkOrders] WITH NOCHECK ADD CONSTRAINT [FK_WorkOrders_WorkOrders_OriginalOrderNumber] FOREIGN KEY (
[OriginalOrderNumber]
) REFERENCES [WorkOrders] (
[WorkOrderID]
)
GO

ALTER TABLE [WorkOrders]  WITH NOCHECK ADD CONSTRAINT [FK_WorkOrders_Contractors_AssignedContractorID] FOREIGN KEY (
[AssignedContractorID]
) REFERENCES [Contractors] (
[ContractorID]
)
GO

---------------------------------------CUSTOMER IMPACT RANGES---------------------------------------
CREATE TABLE [CustomerImpactRanges] (
	[CustomerImpactRangeID] int unique identity not null,
	[Description] varchar(10) unique not null,
	CONSTRAINT [PK_CustomerImactRanges] PRIMARY KEY CLUSTERED (
		[CustomerImpactRangeID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [WorkOrders] ADD CONSTRAINT [FK_WorkOrders_CustomerImpactRanges_CustomerImpactRangeID] FOREIGN KEY (
	[CustomerImpactRangeID]
) REFERENCES [CustomerImpactRanges] (
	[CustomerImpactRangeID]
)
GO

-----------------------------------------REPAIR TIME RANGES-----------------------------------------
CREATE TABLE [RepairTimeRanges] (
	[RepairTimeRangeID] int unique identity not null,
	[Description] varchar(15) unique not null,
	CONSTRAINT [PK_RepairTimeRanges] PRIMARY KEY CLUSTERED (
		[RepairTimeRangeID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [WorkOrders] ADD CONSTRAINT [FK_WorkOrders_RepairTimeRanges_RepairTimeRangeID] FOREIGN KEY (
	[RepairTimeRangeID]
) REFERENCES [RepairTimeRanges] (
	[RepairTimeRangeID]
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
ALTER TABLE [EmployeeWorkOrders] ADD CONSTRAINT [FK_EmployeeWorkOrders_WorkOrders_WorkOrderID] FOREIGN KEY (
	[WorkOrderID]
) REFERENCES [WorkOrders] (
	[WorkOrderID]
)
GO
ALTER TABLE [EmployeeWorkOrders] ADD CONSTRAINT [FK_EmployeeWorkOrders_tblPermissions_AssignedTo] FOREIGN KEY (
	[AssignedTo]
) REFERENCES [tblPermissions] (
	[RecID]
)
GO
ALTER TABLE [EmployeeWorkOrders] ADD CONSTRAINT [FK_EmployeeWorkOrders_tblPermissions_ApprovedBy] FOREIGN KEY (
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
ALTER TABLE [LostWater] ADD CONSTRAINT [FK_LostWater_WorkOrders_WorkOrderID] FOREIGN KEY (
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
ALTER TABLE [WorkOrders] ADD CONSTRAINT [FK_WorkOrders_WorkOrderPriorities_PriorityID] FOREIGN KEY (
	[PriorityID]
) REFERENCES [WorkOrderPriorities] (
	[WorkOrderPriorityID]
)
GO

-----------------------------------------WORK ORDER PURPOSES-----------------------------------------
CREATE TABLE [WorkOrderPurposes] (
	[WorkOrderPurposeID] int unique identity not null,
	[Description] varchar(20) not null,
	CONSTRAINT [PK_WorkOrderPurposes] PRIMARY KEY CLUSTERED (
	[WorkOrderPurposeID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [WorkOrders] ADD CONSTRAINT [FK_WorkOrders_WorkOrderPurposes_PurposeID] FOREIGN KEY (
	[PurposeID]
) REFERENCES [WorkOrderPurposes] (
	[WorkOrderPurposeID]
)
GO

----------------------------------------WORK ORDER REQUESTERS----------------------------------------
CREATE TABLE [WorkOrderRequesters] (
	[WorkOrderRequesterID] int unique identity not null,
	[Description] varchar(16) not null,
	CONSTRAINT [PK_WorkOrderRequester] PRIMARY KEY CLUSTERED (
	[WorkOrderRequesterID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [WorkOrders] ADD CONSTRAINT [FK_WorkOrders_WorkOrderRequesters_RequesterID] FOREIGN KEY (
	[RequesterID]
) REFERENCES [WorkOrderRequesters] (
	[WorkOrderRequesterID]
)
GO

---------------------------------------------ASSET TYPES---------------------------------------------
CREATE TABLE [AssetTypes] (
	[AssetTypeID] int unique identity not null,
	[Description] varchar(13) not null
	CONSTRAINT [PK_AssetTypes] PRIMARY KEY CLUSTERED (
	[AssetTypeID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [WorkOrders] ADD CONSTRAINT [FK_WorkOrders_AssetTypes_AssetTypeID] FOREIGN KEY (
	[AssetTypeID]
) REFERENCES [AssetTypes] (
	[AssetTypeID]
)
GO

-------------------------------------------WORK CATEGORIES-------------------------------------------
CREATE TABLE [WorkCategories] (
	[WorkCategoryID] int unique identity not null,
	[Description] varchar(35) not null,
	CONSTRAINT [PK_WorkCategories] PRIMARY KEY CLUSTERED (
		[WorkCategoryID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]


------------------------------------------ACCOUNTING TYPES-------------------------------------------
CREATE TABLE [dbo].[AccountingTypes] (
	[AccountingTypeID] int unique identity not null,
	[Description] varchar(25) not null,
	CONSTRAINT [PK_AccountingTypes] PRIMARY KEY CLUSTERED (
		[AccountingTypeID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]

------------------------------------------WORK DESCRIPTIONS------------------------------------------
CREATE TABLE [WorkDescriptions] (
	[WorkDescriptionID] int unique identity not null,
	[WorkCategoryID] int null,
	[Description] varchar(50) not null,
	[AssetTypeID] int not null,
	[TimeToComplete] decimal(18, 2) not null,
	[AccountingTypeID] [int] NULL,
	[Revisit] [bit] NOT NULL CONSTRAINT [DF_WorkDescriptions_Revisit]  DEFAULT ((0)),
	[FirstRestorationAccountingCodeID] int not null,
	[FirstRestorationCostBreakdown] tinyint not null,
	[FirstRestorationProductCodeID] int not null,
	[SecondRestorationAccountingCodeID] int null,
	[SecondRestorationCostBreakdown] tinyint null,
	[SecondRestorationProductCodeID] int null,
	[ShowBusinessUnit] bit not null,
	[ShowApprovalAccounting] bit not null,
	[EditOnly] bit not null,
	CONSTRAINT [PK_WorkDescriptions] PRIMARY KEY CLUSTERED (
		[WorkDescriptionID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [WorkDescriptions] ADD CONSTRAINT [FK_WorkDescriptions_AssetTypes_AssetTypeID] FOREIGN KEY (
	[AssetTypeID]
) REFERENCES [AssetTypes] (
	[AssetTypeID]
)
ALTER TABLE [WorkDescriptions] ADD CONSTRAINT [FK_WorkDescriptions_WorkCategories_WorkCategoryID] FOREIGN KEY (
	[WorkCategoryID]
) REFERENCES [WorkCategories] (
	[WorkCategoryID]
)
ALTER TABLE [WorkOrders] ADD CONSTRAINT [FK_WorkOrders_WorkDescriptions_WorkDescriptionID] FOREIGN KEY (
	[WorkDescriptionID]
) REFERENCES [WorkDescriptions] (
	[WorkDescriptionID]
)
ALTER TABLE [WorkDescriptions] ADD CONSTRAINT [FK_WorkDescriptions_AccountingTypes_AccountingTypeID] FOREIGN KEY (
	[AccountingTypeID]
) REFERENCES [AccountingTypes] (
	[AccountingTypeID]
)
GO
-----------------------------------WORK ORDER DESCRIPTION CHANGES------------------------------------
CREATE TABLE [WorkOrderDescriptionChanges] (
	[WorkOrderDescriptionChangeID] int unique identity not null,
	[WorkOrderID] int not null,
	[FromWorkDescriptionID] int null,
	[ToWorkDescriptionID] int not null,
	[ResponsibleEmployeeID] int not null,
	[DateOfChange] smalldatetime not null
	CONSTRAINT [PK_WorkOrderDescriptionChanges] PRIMARY KEY CLUSTERED (
	[WorkOrderDescriptionChangeID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [WorkOrderDescriptionChanges] ADD CONSTRAINT [FK_WorkDescriptionChanges_WorkOrders_WorkOrderDescriptionChangeID] FOREIGN KEY (
	[WorkOrderID]
) REFERENCES [WorkOrders] (
	[WorkOrderID]
)
GO
ALTER TABLE [WorkOrderDescriptionChanges] ADD CONSTRAINT [FK_WorkOrderDescriptionChanges_WorkDescriptions_FromWorkDescriptionID] FOREIGN KEY (
	[FromWorkDescriptionID]
) REFERENCES [WorkDescriptions] (
	[WorkDescriptionID]
)
GO
ALTER TABLE [WorkOrderDescriptionChanges] ADD CONSTRAINT [FK_WorkOrderDescriptionChanges_WorkDescriptions_ToWorkDescriptionID] FOREIGN KEY (
	[ToWorkDescriptionID]
) REFERENCES [WorkDescriptions] (
	[WorkDescriptionID]
)
GO
ALTER TABLE [WorkOrderDescriptionChanges] ADD CONSTRAINT [FK_WorkOrderDescriptionChanges_TblPermissions_ResponsibleEmployeeID] FOREIGN KEY (
	[ResponsibleEmployeeID]
) REFERENCES [TblPermissions] (
	[RecID]
)
GO

------------------------------------RESTORATION ACCOUNTING CODES-------------------------------------
CREATE TABLE [RestorationAccountingCodes] (
	[RestorationAccountingCodeID] int unique identity not null,
	[Code] varchar(8) not null,
	[SubCode] varchar(2) null
	CONSTRAINT [PK_RestorationAccountingCodes] PRIMARY KEY CLUSTERED (
		[RestorationAccountingCodeID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [WorkDescriptions]  ADD CONSTRAINT [FK_WorkDescriptions_RestorationAccountingCodes_FirstRestorationAccountingCodeID] FOREIGN KEY (
	[FirstRestorationAccountingCodeID]
) REFERENCES [RestorationAccountingCodes] (
	[RestorationAccountingCodeID]
)
GO
ALTER TABLE [WorkDescriptions]  ADD CONSTRAINT [FK_WorkDescriptions_RestorationAccountingCodes_SecondRestorationAccountingCodeID] FOREIGN KEY (
	[SecondRestorationAccountingCodeID]
) REFERENCES [RestorationAccountingCodes] (
	[RestorationAccountingCodeID]
)
GO

--------------------------------------------PRODUCT CODES--------------------------------------------
CREATE TABLE [RestorationProductCodes] (
	[RestorationProductCodeID] int unique identity not null,
	[Code] varchar(4) not null
	CONSTRAINT [PK_ProductCode] PRIMARY KEY CLUSTERED (
		[RestorationProductCodeID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [WorkDescriptions]  ADD CONSTRAINT [FK_WorkDescriptions_RestorationProductCodes_FirstRestorationProductCodeID] FOREIGN KEY (
	[FirstRestorationProductCodeID]
) REFERENCES [RestorationProductCodes] (
	[RestorationProductCodeID]
)
GO
ALTER TABLE [WorkDescriptions]  ADD CONSTRAINT [FK_WorkDescriptions_RestorationProductCodes_SecondRestorationProductCodeID] FOREIGN KEY (
	[SecondRestorationProductCodeID]
) REFERENCES [RestorationProductCodes] (
	[RestorationProductCodeID]
)
GO

-----------------------------------------------------------------------------------------------------
---------------------------------------------MAIN BREAKS---------------------------------------------
-----------------------------------------------------------------------------------------------------
CREATE TABLE [dbo].[MainBreakValveOperations]
(
	[MainBreakValveOperationID] [int] IDENTITY(1,1) NOT NULL,
	[MainBreakID] [int] NOT NULL,
	[ValveID] [int] NOT NULL,
		CONSTRAINT [PK_MainBreakValveOperations] PRIMARY KEY CLUSTERED 
		(
			[MainBreakValveOperationID] ASC
		) ON [PRIMARY],
		CONSTRAINT [UQ__MainBreakValveOp__20AD9DE2] UNIQUE NONCLUSTERED 
		(
			[MainBreakValveOperationID] ASC
		) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[MainBreakValveOperations]  ADD  CONSTRAINT [FK_MainBreakValveOperations_tblNJAWValves_ValveID] FOREIGN KEY (
	[ValveID]
) REFERENCES [dbo].[tblNJAWValves] (
	[RecID]
)
GO
ALTER TABLE [dbo].[MainBreakValveOperations] CHECK CONSTRAINT [FK_MainBreakValveOperations_tblNJAWValves_ValveID]
GO
---

CREATE TABLE [dbo].[MainBreakDisinfectionMethods]
(
[MainBreakDisinfectionMethodID] [int] NOT NULL IDENTITY(1, 1),
[Description] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
ALTER TABLE [dbo].[MainBreakDisinfectionMethods] ADD PRIMARY KEY CLUSTERED  ([MainBreakDisinfectionMethodID])
GO

CREATE TABLE [dbo].[MainBreakFlushMethods]
(
[MainBreakFlushMethodID] [int] NOT NULL IDENTITY(1, 1),
[Description] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
ALTER TABLE [dbo].[MainBreakFlushMethods] ADD PRIMARY KEY CLUSTERED  ([MainBreakFlushMethodID])
GO

CREATE TABLE [dbo].[MainBreakMaterials]
(
[MainBreakMaterialID] [int] NOT NULL IDENTITY(1, 1),
[Description] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
ALTER TABLE [dbo].[MainBreakMaterials] ADD PRIMARY KEY CLUSTERED  ([MainBreakMaterialID])
GO

CREATE TABLE [dbo].[MainBreakSoilConditions]
(
[MainBreakSoilConditionID] [int] NOT NULL IDENTITY(1, 1),
[Description] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
ALTER TABLE [dbo].[MainBreakSoilConditions] ADD PRIMARY KEY CLUSTERED  ([MainBreakSoilConditionID])
GO

CREATE TABLE [dbo].[MainFailureTypes](
	[MainFailureTypeID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_MainFailureTypes] PRIMARY KEY CLUSTERED 
(
	[MainFailureTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[MainFailureTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[MainConditions](
	[MainConditionID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_MainConditions] PRIMARY KEY CLUSTERED 
(
	[MainConditionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UQ__MainConditions__2D5FEF8F] UNIQUE NONCLUSTERED 
(
	[MainConditionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[MainBreaks]
(
[MainBreakID] [int] NOT NULL IDENTITY(1, 1),
[WorkOrderID] [int] NOT NULL,
[MainFailureTypeID] [int] NOT NULL,
[MainBreakMaterialID] [int] NOT NULL,
[Depth] [decimal] (18, 2) NOT NULL,
[MainConditionID] [int] NOT NULL,
[CustomersAffected] [int] NOT NULL,
[ShutdownTime] [decimal] (5, 2) NOT NULL,
[MainBreakSoilConditionID] [int] NOT NULL,
[MainBreakDisinfectionMethodID] [int] NOT NULL,
[MainBreakFlushMethodID] [int] NOT NULL,
[ChlorineResidual] [decimal] (3, 1) NULL,
[BoilAlertIssued] [tinyint] NOT NULL,
[ServiceSizeID] [int] NOT NULL
)
GO
ALTER TABLE [dbo].[MainBreaks] ADD PRIMARY KEY CLUSTERED  ([MainBreakID])
GO
ALTER TABLE [dbo].[MainBreaks] ADD UNIQUE NONCLUSTERED  ([MainBreakID])
GO
ALTER TABLE [MainBreaks] WITH CHECK ADD CONSTRAINT [FK_MainBreaks_MainBreakDisinfectionMethods_MainBreakDisinfectionMethodID] FOREIGN KEY (
	[MainBreakDisinfectionMethodID]
) REFERENCES [MainBreakDisinfectionMethods] (
	[MainBreakDisinfectionMethodID]
)
GO
ALTER TABLE [MainBreaks] WITH CHECK ADD CONSTRAINT [FK_MainBreaks_MainBreakFlushMethods_MainBreakFlushMethodID] FOREIGN KEY (
	[MainBreakFlushMethodID]
) REFERENCES [MainBreakFlushMethods] (
	[MainBreakFlushMethodID]
)
GO
ALTER TABLE [MainBreaks] WITH CHECK ADD CONSTRAINT [FK_MainBreaks_MainBreakMaterials_MainBreakMaterialID] FOREIGN KEY (
	[MainBreakMaterialID]
) REFERENCES [MainBreakMaterials] (
	[MainBreakMaterialID]
)
GO
ALTER TABLE [MainBreaks] WITH CHECK ADD CONSTRAINT [FK_MainBreaks_MainBreakSoilConditions_MainBreakSoilConditionID] FOREIGN KEY (
	[MainBreakSoilConditionID]
) REFERENCES [MainBreakSoilConditions] (
	[MainBreakSoilConditionID]
)
GO
ALTER TABLE [MainBreaks] WITH CHECK ADD CONSTRAINT [FK_MainBreaks_MainConditions_MainConditionID] FOREIGN KEY (
	[MainConditionID]
) REFERENCES [MainConditions] (
	[MainConditionID]
)
GO
ALTER TABLE [MainBreaks] WITH CHECK ADD CONSTRAINT [FK_MainBreaks_MainFailureTypes_MainFailureTypeID] FOREIGN KEY (
	[MainFailureTypeID]
) REFERENCES [MainFailureTypes] (
	[MainFailureTypeID]
)
GO
ALTER TABLE [MainBreaks] WITH CHECK ADD CONSTRAINT [FK_MainBreaks_WorkOrders_WorkOrderID] FOREIGN KEY (
	[WorkOrderID]
) REFERENCES [WorkOrders] (
	[WorkOrderID]
)
GO
ALTER TABLE [MainBreaks] WITH CHECK ADD CONSTRAINT [FK_MainBreaks_tblNJAWSizeServ_ServiceSizeID] FOREIGN KEY (
	[ServiceSizeID]
) REFERENCES [tblNJAWSizeServ] (
	[recID]
)
GO

-----------------------------------------------------------------------------------------------------
----------------------------------------------MARKOUTS-----------------------------------------------
-----------------------------------------------------------------------------------------------------

----------------------------------------------MARKOUTS-----------------------------------------------

CREATE TABLE [Markouts] (
	[MarkoutID] int unique identity not null,
	[WorkOrderID] int not null,
	[MarkoutNumber] varchar(9) not null,
	-- 20090304 MarkoutTypes are superseded by
	-- MarkoutRequirements, which are tracked by
	-- the parent WorkOrder
--	[MarkoutTypeID] int not null,
	-- 20090224 needed to drop the not null constraint here
	-- in order to import
	[DateOfRequest] smalldatetime null,
	[ReadyDate] smalldatetime null,
	[ExpirationDate] smalldatetime null,
	-- 20090303 added radius column
	-- 20100921 added markouttypeid, removed radius
	[MarkoutTypeID] int null,
	[Note] text null,
	[CreatorID] int null,
	CONSTRAINT [PK_Markouts] PRIMARY KEY CLUSTERED (
		[MarkoutID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [Markouts] ADD CONSTRAINT [FK_Markouts_WorkOrders_WorkOrderID] FOREIGN KEY (
	[WorkOrderID]
) REFERENCES [WorkOrders] (
	[WorkOrderID]
)
GO

ALTER TABLE [Markouts]  WITH NOCHECK ADD CONSTRAINT [FK_Markouts_tblPermissions_CreatorID] FOREIGN KEY (
[CreatorID]
) REFERENCES [tblPermissions] (
[RecID]
)
GO

--------------------------------------------MARKOUT STATI--------------------------------------------
CREATE TABLE [MarkoutStatuses] (
	[MarkoutStatusID] int unique identity not null,
	[Description] varchar(10) not null,
	CONSTRAINT [PK_MarkoutStatuses] PRIMARY KEY CLUSTERED (
		[MarkoutStatusID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
GO

----------------------------------------MARKOUT REQUIREMENTS----------------------------------------
CREATE TABLE [MarkoutRequirements] (
	[MarkoutRequirementID] int unique identity not null,
	[Description] varchar(10) not null,
	CONSTRAINT [PK_MarkoutRequirements] PRIMARY KEY CLUSTERED (
	[MarkoutRequirementID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [WorkOrders] ADD CONSTRAINT [FK_WorkOrders_MarkoutRequirements_MarkoutRequirementID] FOREIGN KEY (
	[MarkoutRequirementID]
) REFERENCES [MarkoutRequirements] (
	[MarkoutRequirementID]
)
GO

--------------------------------------------MARKOUT TYPES-------------------------------------------
CREATE TABLE [MarkoutTypes] (
[MarkoutTypeID] int unique identity not null,
[Description] varchar(120) not null, 
[Order] int not null,
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

ALTER TABLE [WorkOrders]  ADD CONSTRAINT [FK_WorkOrders_MarkoutTypes_MarkoutTypeNeededID] FOREIGN KEY (
[MarkoutTypeNeededID]
) REFERENCES [MarkoutTypes] (
[MarkoutTypeID]
)
GO

----------------------------------------------------------------------------------------------------
--------------------------------------------RESTORATIONS--------------------------------------------
----------------------------------------------------------------------------------------------------

------------------------------------------RESTORATION TYPES------------------------------------------
-- table of possible methods of restoration for work
-- orders.  this is a "resource table"
CREATE TABLE [RestorationTypes] ( --TblMethods of Final Restoration
	[RestorationTypeID] int unique identity not null, --ID
	[Description] varchar(30) null, --Method of Final Restoration
	CONSTRAINT [PK_RestorationTypes] PRIMARY KEY CLUSTERED (
	[RestorationTypeID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
GO

-----------------------------------------RESTORATION METHODS-----------------------------------------
CREATE TABLE [RestorationMethods] (
	[RestorationMethodID] int unique identity not null,
	[Description] varchar(35) not null,
	CONSTRAINT [PK_RestorationMethods] PRIMARY KEY CLUSTERED (
		[RestorationMethodID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
GO

--------------------------------RESTORATION METHODS/RESTORATION TYPES--------------------------------
CREATE TABLE [RestorationMethodsRestorationTypes] (
	[RestorationMethodRestorationTypeID] int unique identity not null,
	[RestorationMethodID] int not null,
	[RestorationTypeID] int not null,
	[InitialMethod] bit default 0 not null,
	[FinalMethod] bit default 0 not null,
	CONSTRAINT [PK_RestorationMethodsRestorationTypes] PRIMARY KEY CLUSTERED (
		[RestorationMethodRestorationTypeID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [RestorationMethodsRestorationTypes] ADD CONSTRAINT [FK_RestorationMethodsRestorationTypes_RestorationMethods_RestorationMethodID] FOREIGN KEY (
	[RestorationMethodID]
) REFERENCES [RestorationMethods] (
	[RestorationMethodID]
)
GO

ALTER TABLE [RestorationMethodsRestorationTypes] ADD CONSTRAINT [FK_RestorationMethodsRestorationTypes_RestorationTypes_RestorationTypeID] FOREIGN KEY (
	[RestorationTypeID]
) REFERENCES [RestorationTypes] (
	[RestorationTypeID]
)
GO

--------------------------------------RESTORATION METHOD COSTS---------------------------------------
CREATE TABLE [RestorationTypeCosts] (
	[RestorationTypeCostID] int unique identity not null,
	[OperatingCenterID] int not null,
	[RestorationTypeID] int not null,
	[Cost] float not null,
	CONSTRAINT [PK_RestorationTypeCosts] PRIMARY KEY CLUSTERED (
		[RestorationTypeCostID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [RestorationTypeCosts] ADD CONSTRAINT [FK_RestorationTypeCosts_OperatingCenters_OperatingCenterID] FOREIGN KEY (
	[OperatingCenterID]
) REFERENCES [OperatingCenters] (
	[OperatingCenterID]
)
GO

ALTER TABLE [RestorationTypeCosts] ADD CONSTRAINT [FK_RestorationTypeCosts_RestorationType_RestorationMethodID] FOREIGN KEY (
	[RestorationTypeID]
) REFERENCES [RestorationTypes] (
	[RestorationTypeID]
)
GO

----------------------------------RESTORATION RESPONSE PRIORITIES-----------------------------------
CREATE TABLE [RestorationResponsePriorities] (
	[RestorationResponsePriorityID] int unique identity not null,
	[Description] varchar(25) not null,
	CONSTRAINT [PK_RestorationResponsePriorities] PRIMARY KEY CLUSTERED (
		[RestorationResponsePriorityID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]

--------------------------------------------RESTORATIONS--------------------------------------------
CREATE TABLE [Restorations] (
	[RestorationID] int unique identity not null,
	[WorkOrderID] int not null,
	[RestorationTypeID] int not null, -- Type of Restoration
	[PavingSquareFootage] decimal(9,2) null, -- Paving Square Footage
	[LinearFeetOfCurb] decimal(9,2) null, -- Linear Ft of Curb
	[RestorationNotes] text null, -- Restoration Notes
	[PartialRestorationMethodID] int null,
	[PartialRestorationInvoiceNumber] varchar(12) null, -- Partial Restoration Invoice Number
	[PartialRestorationDate] smalldatetime null,
	[PartialRestorationCompletedBy] varchar(10) null, -- Partial Restoration Completed By
	[PartialPavingBreakOutEightInches] int null, -- Partial Paving Break out 8"
	[PartialPavingBreakOutTenInches] int null, -- Partial Paving Break out 10"
	[PartialSawCutting] int null, -- Partial Saw Cutting
	[PartialPavingSquareFootage] int null,
	[DaysToPartialPaveHole] int null, -- Days to Partical Pave hole
	[TrafficControlCostPartialRestoration] int null, -- Traffic Control Cost Partial Restoration
	[FinalRestorationMethodID] int null,
	[FinalRestorationInvoiceNumber] varchar(12) null, -- Final Restoration Invoice Number
	[FinalRestorationDate] smalldatetime null,
	[FinalRestorationCompletedBy] varchar(15) null, -- Final Restoration Completed By
	[FinalPavingBreakOutEightInches] int null, -- Final Concrete Break out 8"
	[FinalPavingBreakOutTenInches] int null, -- Final Concrete Break out 8"
	[FinalSawCutting] int null, -- Final Saw cutting
	[FinalPavingSquareFootage] int null,
	[DaysToFinalPaveHole] int null, -- Days to Final Pave hole
	[TrafficControlCostFinalRestoration] int null, -- Traffic Control Cost Final Restoration
	[ApprovedByID] int null, -- Approved By
	[DateApproved] smalldatetime null, -- Date Approved
	[RejectedByID] int null,
	[DateRejected] smalldatetime null, -- Date Rejected
--	[TypeOfAsphalt] varchar(35) null, -- Type of Asphalt
	[EightInchStabilizeBaseByCompanyForces] bit default 0 not null, -- 8" Stab Base By Company Forces
	[SawCutByCompanyForces] bit default 0 not null, -- Saw Cut By Company Forces
	[TotalAccruedCost] decimal(18,2) null, 
	[TotalInitialActualCost] decimal(18,2) null, 
	[FinalRestorationActualCost] decimal(18,2) null,
	[ResponsePriorityID] int null,
	CONSTRAINT [PK_Restorations] PRIMARY KEY CLUSTERED (
		[RestorationID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [Restorations] ADD CONSTRAINT [FK_Restorations_RestorationTypes_RestorationTypeID] FOREIGN KEY (
	[RestorationTypeID]
) REFERENCES [RestorationTypes] (
	[RestorationTypeID]
)
GO

ALTER TABLE [Restorations] ADD CONSTRAINT [FK_Restorations_RestorationMethods_PartialRestorationMethodID] FOREIGN KEY (
	[PartialRestorationMethodID]
) REFERENCES [RestorationMethods] (
	[RestorationMethodID]
)
GO

ALTER TABLE [Restorations] ADD CONSTRAINT [FK_Restorations_RestorationMethods_FinalRestorationMethodID] FOREIGN KEY (
	[FinalRestorationMethodID]
) REFERENCES [RestorationMethods] (
	[RestorationMethodID]
)
GO

ALTER TABLE [Restorations] ADD CONSTRAINT [FK_Restorations_tblPermissions_ApprovedByID] FOREIGN KEY (
	[ApprovedByID]
) REFERENCES [tblPermissions] (
	[RecID]
)
GO

ALTER TABLE [Restorations] ADD CONSTRAINT [FK_Restorations_tblPermissions_RejectedByID] FOREIGN KEY (
	[RejectedByID]
) REFERENCES [tblPermissions] (
	[RecID]
)
GO

ALTER TABLE [Restorations] ADD CONSTRAINT [FK_Restorations_WorkOrders_WorkOrderID] FOREIGN KEY (
	[WorkOrderID]
) REFERENCES [WorkOrders] (
	[WorkOrderID]
)
GO

ALTER TABLE [Restorations]  ADD CONSTRAINT [FK_Restorations_RestorationResponsePriorities_ResponsePriorityID] FOREIGN KEY (
	[ResponsePriorityID]
) REFERENCES [RestorationResponsePriorities] (
	[RestorationResponsePriorityID]
)
GO

-----------------------------------------------------------------------------------------------------
-----------------------------------------------SPOILS------------------------------------------------
-----------------------------------------------------------------------------------------------------

-----------------------------------------------SPOILS------------------------------------------------
CREATE TABLE [Spoils] (
	[SpoilID] int unique identity not null,
	[WorkOrderID] int not null,
	[Quantity] decimal(6, 2) not null,
	[SpoilStorageLocationID] int not null,
	CONSTRAINT [PK_Spoils] PRIMARY KEY CLUSTERED (
		[SpoilID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [Spoils] ADD CONSTRAINT [FK_Spoils_WorkOrders_WorkOrderID] FOREIGN KEY (
	[WorkOrderID]
) REFERENCES [WorkOrders] (
	[WorkOrderID]
)
GO

------------------------------------OPERATING CENTER SPOIL REMOVAL COSTS-------------------------------------
CREATE TABLE [OperatingCenterSpoilRemovalCosts] (
	[OperatingCenterSpoilRemovalCostID] int unique identity not null,
	[OperatingCenterID] int not null,
	[Cost] smallint not null,
	CONSTRAINT [PK_OperatingCenterSpoilRemovalCosts] PRIMARY KEY CLUSTERED (
		[OperatingCenterSpoilRemovalCostID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [OperatingCenterSpoilRemovalCosts] ADD CONSTRAINT [FK_OperatingCenterSpoilRemovalCosts_OperatingCenters_OperatingCenterID] FOREIGN KEY (
	[OperatingCenterID]
) REFERENCES [OperatingCenters] (
	[OperatingCenterID]
)
GO

---------------------------------------SPOIL STORAGE LOCATIONS---------------------------------------
CREATE TABLE [SpoilStorageLocations] (
	[SpoilStorageLocationID] int unique identity not null,
	[Name] varchar(30) not null,
	[OperatingCenterID] int not null,
	[TownID] int null,
	[StreetID] int null,
	CONSTRAINT [PK_SpoilStorageLocations] PRIMARY KEY CLUSTERED (
		[SpoilStorageLocationID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [SpoilStorageLocations] ADD CONSTRAINT [FK_SpoilStorageLocations_OperatingCenters_OperatingCenterID] FOREIGN KEY (
	[OperatingCenterID]
) REFERENCES [OperatingCenters] (
	[OperatingCenterID]
)
GO
ALTER TABLE [SpoilStorageLocations] ADD CONSTRAINT [FK_SpoilStorageLocations_Towns_TownID] FOREIGN KEY (
	[TownID]
) REFERENCES [Towns] (
	[TownID]
)
GO
ALTER TABLE [SpoilStorageLocations] ADD CONSTRAINT [FK_SpoilStorageLocations_Streets_StreetID] FOREIGN KEY (
	[StreetID]
) REFERENCES [Streets] (
	[StreetID]
)
GO

----------------------------------SPOIL FINAL PROCESSING LOCATIONS-----------------------------------
CREATE TABLE [SpoilFinalProcessingLocations] (
	[SpoilFinalProcessingLocationID] int unique identity not null,
	[Name] varchar(30) not null,
	[OperatingCenterID] int not null,
	[TownID] int null,
	[StreetID] int null,
	CONSTRAINT [PK_SpoilFinalProcessingLocations] PRIMARY KEY CLUSTERED (
		[SpoilFinalProcessingLocationID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [SpoilFinalProcessingLocations] ADD CONSTRAINT [FK_SpoilFinalProcessingLocations_OperatingCenters_OperatingCenterID] FOREIGN KEY (
	[OperatingCenterID]
) REFERENCES [OperatingCenters] (
	[OperatingCenterID]
)
GO
ALTER TABLE [SpoilFinalProcessingLocations] ADD CONSTRAINT [FK_SpoilFinalProcessingLocations_Towns_TownID] FOREIGN KEY (
	[TownID]
) REFERENCES [Towns] (
	[TownID]
)
GO
ALTER TABLE [SpoilFinalProcessingLocations] ADD CONSTRAINT [FK_SpoilFinalProcessingLocations_Streets_StreetID] FOREIGN KEY (
	[StreetID]
) REFERENCES [Streets] (
	[StreetID]
)
GO

-------------------------------------------SPOIL REMOVALS--------------------------------------------
CREATE TABLE [SpoilRemovals] (
	[SpoilRemovalID] int unique identity not null,
	[DateRemoved] smalldatetime not null,
	[Quantity] decimal(6, 2) not null,
	[RemovedFromID] int not null,
	[FinalDestinationID] int not null,
	CONSTRAINT [PK_SpoilRemovals] PRIMARY KEY CLUSTERED (
		[SpoilRemovalID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [SpoilRemovals] ADD CONSTRAINT [FK_SpoilRemovals_SpoilStorageLocations_RemovedFromID] FOREIGN KEY (
	[RemovedFromID]
) REFERENCES [SpoilStorageLocations] (
	[SpoilStorageLocationID]
)
GO
ALTER TABLE [SpoilRemovals] ADD CONSTRAINT [FK_SpoilRemovals_SpoilFinalProcessingLocations_FinalDestinationID] FOREIGN KEY (
	[FinalDestinationID]
) REFERENCES [SpoilFinalProcessingLocations] (
	[SpoilFinalProcessingLocationID]
)
GO

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
	[StockLocationID] int null,
	CONSTRAINT [PK_MaterialsUsed] PRIMARY KEY CLUSTERED (
	[MaterialsUsedID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [MaterialsUsed] ADD CONSTRAINT [FK_MaterialsUsed_WorkOrders_WorkOrderID] FOREIGN KEY (
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
	[OldPartNumber] varchar(15) null, --Old pre-SAP Part Number
	[PartNumber] varchar(15) not null, --Part Number
	[Description] text null, --Description,
	[IsActive] bit not null DEFAULT 'true'
	--New Part Numbers
	CONSTRAINT [PK_Materials] PRIMARY KEY CLUSTERED (
	[MaterialID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [MaterialsUsed] ADD CONSTRAINT [FK_MaterialsUsed_Materials_MaterialID] FOREIGN KEY (
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
ALTER TABLE [OperatingCenterStockedMaterials] ADD CONSTRAINT [FK_OperatingCenterStockedMaterials_OperatingCenters_OperatingCenterID] FOREIGN KEY (
	[OperatingCenterID]
) REFERENCES [OperatingCenters] (
	[OperatingCenterID]
)
ALTER TABLE [OperatingCenterStockedMaterials] ADD CONSTRAINT [FK_OperatingCenterStockedMaterials_Materials_MaterialID] FOREIGN KEY (
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
ALTER TABLE [SafetyMarkers] ADD CONSTRAINT [FK_SafetyMarkers_WorkOrders_WorkOrderID] FOREIGN KEY (
	[WorkOrderID]
) REFERENCES [WorkOrders] (
	[WorkOrderID]
)
GO

-------------------------------------------STOCK LOCATIONS-------------------------------------------
CREATE TABLE [StockLocations] (
	[StockLocationID] int unique identity not null,
	[Description] varchar(25) not null,
	[OperatingCenterID] int not null,
	[SAPStockLocation] varchar(50) null,
	CONSTRAINT [PK_StockLocations] PRIMARY KEY CLUSTERED (
		[StockLocationID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [StockLocations] ADD CONSTRAINT [FK_StockLocations_OperatingCenters_OperatingCenterID] FOREIGN KEY (
	[OperatingCenterID]
) REFERENCES [OperatingCenters] (
	[OperatingCenterID]
)
ALTER TABLE [MaterialsUsed] ADD CONSTRAINT [FK_MaterialsUsed_StockLocations_StockLocationID] FOREIGN KEY (
	[StockLocationID]
) REFERENCES [StockLocations] (
	[StockLocationID]
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

ALTER TABLE [DetectedLeaks] ADD CONSTRAINT [FK_DetectedLeaks_WorkOrders_WorkOrderID] FOREIGN KEY (
	[WorkOrderID]
) REFERENCES [WorkOrders] (
	[WorkOrderID]
)
GO

ALTER TABLE [DetectedLeaks] ADD CONSTRAINT [FK_DetectedLeaks_tblNJAWValves_SurveyStartingPoint] FOREIGN KEY (
	[SurveyStartingPointID]
) REFERENCES [tblNJAWValves] (
	[RecID]
)
GO

ALTER TABLE [DetectedLeaks] ADD CONSTRAINT [FK_DetectedLeaks_tblNJAWValves_SurveyEndingPoint] FOREIGN KEY (
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

-------------------------------------------DETECTED LEAKS--------------------------------------------
ALTER TABLE [DetectedLeaks] ADD CONSTRAINT [FK_DetectedLeaks_WorkAreaTypes_WorkAreaTypeID] FOREIGN KEY (
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

ALTER TABLE [DetectedLeaks] ADD CONSTRAINT [FK_DetectedLeaks_LeakReportingSources_LeakReportingSourceID] FOREIGN KEY (
	[LeakReportingSourceID]
) REFERENCES [LeakReportingSources] (
	[LeakReportingSourceID]
)
GO

-------------------------------------------CONTRACTOR USERS------------------------------------------
CREATE TABLE [ContractorUsers] (
		[ContractorUserID] int unique identity not null,
		[ContractorID] int not null,
		[Email] varchar(40) unique not null,
		[Password] varchar(128) not null,
		[PasswordSalt] uniqueidentifier unique not null,
		[PasswordQuestion] varchar(256) not null,
		[PasswordAnswer] varchar(128) not null,
		[IsAdmin] bit not null,
		CONSTRAINT [PK_ContractorUsers] PRIMARY KEY CLUSTERED (
				[ContractorUserID] ASC
		) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [ContractorUsers]  WITH NOCHECK ADD CONSTRAINT [FK_ContractorUsers_Contractors_ContractorID] FOREIGN KEY (
		[ContractorID]
) REFERENCES [Contractors] (
		[ContractorID]
)
GO

ALTER TABLE [dbo].[ContractorUsers] ADD  CONSTRAINT [DF_ContractorUsers_IsAdmin]  DEFAULT ((0)) FOR [IsAdmin]
GO

------------------------------------------------CREWS------------------------------------------------
CREATE TABLE [Crews] (
	[CrewID] int unique identity not null,
	[Description] varchar(15) not null,
	[OperatingCenterID] int null,
	[Availability] decimal(18, 2) not null,
	[ContractorID] int null,
	CONSTRAINT [PK_Crews] PRIMARY KEY CLUSTERED (
		[CrewID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [Crews] ADD CONSTRAINT [FK_Crews_OperatingCenters_OperatingCenterID] FOREIGN KEY (
	[OperatingCenterID]
) REFERENCES [OperatingCenters] (
	[OperatingCenterID]
)
GO

ALTER TABLE [dbo].[Crews] WITH NOCHECK ADD CONSTRAINT [FK_Crews_Contractors_ContractorID] FOREIGN KEY (
		[ContractorID]
) REFERENCES [Contractors] (
		[ContractorID]
)
GO

------------------------------------------CREW ASSIGNMENTS------------------------------------------
CREATE TABLE [CrewAssignments] (
	[CrewAssignmentID] int unique identity not null,
	[CrewID] int not null,
	[WorkOrderID] int not null,
	[AssignedOn] smalldatetime not null,
	[AssignedFor] smalldatetime not null,
	[DateStarted] smalldatetime null,
	[DateEnded] smalldatetime null,
	[Priority] int not null,
	[EmployeesOnJob] float null,
	CONSTRAINT [PK_CrewAssignments] PRIMARY KEY CLUSTERED (
		[CrewAssignmentID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [CrewAssignments] ADD CONSTRAINT [FK_CrewAssignments_Crews_CrewAssignmentID] FOREIGN KEY (
	[CrewID]
) REFERENCES [Crews] (
	[CrewID]
)
GO

ALTER TABLE [CrewAssignments] ADD CONSTRAINT [FK_CrewAssignments_WorkOrders_CrewAssignmentID] FOREIGN KEY (
	[WorkOrderID]
) REFERENCES [WorkOrders] (
	[WorkOrderID]
)
GO

----------------------------------------STREETOPENINGPERMITS-----------------------------------------
CREATE TABLE [dbo].[StreetOpeningPermits] (
	[StreetOpeningPermitID] [int] IDENTITY(1,1) NOT NULL,
	[WorkOrderID] [int] NOT NULL,
	[StreetOpeningPermitNumber] [varchar](25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[DateRequested] [datetime] NOT NULL,
	[DateIssued] [datetime] NULL,
	[ExpirationDate] [datetime] NULL,
	[Notes] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[PermitId] int null,
	[HasMetDrawingRequirement] bit null, 
	[IsPaidFor] bit null,
	CONSTRAINT [PK_StreetOpeningPermits] PRIMARY KEY CLUSTERED 
	(
		[StreetOpeningPermitID] ASC
	) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [StreetOpeningPermits] ADD CONSTRAINT [FK_StreetOpeningPermits_WorkOrders_WorkOrderID] FOREIGN KEY (
	[WorkOrderID]
) REFERENCES [WorkOrders] (
	[WorkOrderID]
)
GO

/****** Object:  Table [dbo].[OperatingCenterAssetTypes]    Script Date: 09/24/2009 15:27:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OperatingCenterAssetTypes](
	[OperatingCenterAssetTypeID] [int] IDENTITY(1,1) NOT NULL,
	[OperatingCenterID] [int] NOT NULL,
	[AssetTypeID] [int] NOT NULL,
 CONSTRAINT [PK_OperatingCenterAssetTypes] PRIMARY KEY CLUSTERED 
(
	[OperatingCenterAssetTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [OperatingCenterAssetTypes] ADD CONSTRAINT [FK_OperatingCenterAssetTypes_AssetTypes_AssetTypeID] FOREIGN KEY (
	[AssetTypeID]
) REFERENCES [AssetTypes] (
	[AssetTypeID]
)
GO
ALTER TABLE [OperatingCenterAssetTypes] ADD CONSTRAINT [FK_OperatingCenterAssetTypes_OperatingCenters_OperatingCenterID] FOREIGN KEY (
	[OperatingCenterID]
) REFERENCES [OperatingCenters] (
	[OperatingCenterID]
)
GO
/****** Object:  Table [dbo].[OrcomOrderCompletions]    Script Date: 12/10/2009 09:20:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrcomOrderCompletions](
	[OrcomOrderCompletionID] [int] IDENTITY(1,1) NOT NULL,
	[WorkOrderID] [int] NOT NULL,
	[CompletedByID] [int] NOT NULL,
	[CompletedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_OrcomOrderCompletions] PRIMARY KEY CLUSTERED 
(
	[OrcomOrderCompletionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[OrcomOrderCompletions]  WITH CHECK ADD  CONSTRAINT [FK_OrcomOrderCompletions_tblPermissions] FOREIGN KEY([CompletedByID])
REFERENCES [dbo].[tblPermissions] ([RecID])
GO
ALTER TABLE [dbo].[OrcomOrderCompletions] CHECK CONSTRAINT [FK_OrcomOrderCompletions_tblPermissions]
GO
ALTER TABLE [dbo].[OrcomOrderCompletions]  WITH CHECK ADD  CONSTRAINT [FK_OrcomOrderCompletions_WorkOrders] FOREIGN KEY([WorkOrderID])
REFERENCES [dbo].[WorkOrders] ([WorkOrderID])
GO
ALTER TABLE [dbo].[OrcomOrderCompletions] CHECK CONSTRAINT [FK_OrcomOrderCompletions_WorkOrders]
GO

-----------------------------------------------------------------------------------------------------
----------------------------------------------DOCUMENTS----------------------------------------------
-----------------------------------------------------------------------------------------------------
CREATE TABLE [DocumentsWorkOrders] (
	[DocumentWorkOrderID] int unique identity not null,
	[DocumentID] int not null,
	[WorkOrderID] int not null
	CONSTRAINT [PK_DocumentsWorkOrders] PRIMARY KEY CLUSTERED (
		[DocumentWorkOrderID] ASC
	) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [DocumentsWorkOrders] ADD CONSTRAINT [FK_DocumentsWorkOrders_Documents_DocumentID] FOREIGN KEY (
	[DocumentID]
) REFERENCES [Document] (
	[DocumentID]
)
GO

ALTER TABLE [DocumentsWorkOrders] ADD CONSTRAINT [FK_DocumentsWorkOrders_WorkOrders_WorkOrderID] FOREIGN KEY (
	[WorkOrderID]
) REFERENCES [WorkOrders] (
	[WorkOrderID]
)
GO

-----------------------------------------------------------------------------------------------------
--------------------------------------------DEPARTMENTS----------------------------------------------
-----------------------------------------------------------------------------------------------------

CREATE TABLE [Departments] (
[DepartmentID] int unique identity not null,
[Code] char(2) not null,
[Description] varchar(50) not null,
CONSTRAINT [PK_Departments] PRIMARY KEY CLUSTERED (
[DepartmentID] ASC
) ON [PRIMARY]
) ON [PRIMARY]

-----------------------------------------------------------------------------------------------------
-------------------------------------------BUSINESS UNITS--------------------------------------------
-----------------------------------------------------------------------------------------------------

--CREATE TABLE [BusinessUnits] (
--[BusinessUnitID] int unique identity not null,
--[Description] char(6) not null,
--[OperatingCenterID] int not null,
--[DepartmentID] int not null,
--[Order] int not null,
--CONSTRAINT [PK_BusinessUnits] PRIMARY KEY CLUSTERED (
--[BusinessUnitID] ASC
--) ON [PRIMARY]
--) ON [PRIMARY]

--ALTER TABLE [BusinessUnits]  ADD CONSTRAINT [FK_BusinessUnits_OperatingCenters_OperatingCenterID] FOREIGN KEY (
--[OperatingCenterID]
--) REFERENCES [OperatingCenters] (
--[OperatingCenterID]
--)
--GO

--ALTER TABLE [BusinessUnits]  WITH NOCHECK ADD CONSTRAINT [FK_BusinessUnits_Departments_DepartmentID] FOREIGN KEY (
--[DepartmentID]
--) REFERENCES [Departments] (
--[DepartmentID]
--)
--GO

-----------------------------------------------------------------------------------------------------
-------------------------------------------REPORT VIEWINGS-------------------------------------------
-----------------------------------------------------------------------------------------------------

CREATE TABLE [ReportViewings] (
[ReportViewingID] int unique identity not null,
[EmployeeID] int not null,
[DateViewed] datetime not null,
[ReportName] varchar(50) not null,
CONSTRAINT [PK_ReportViewings] PRIMARY KEY CLUSTERED (
[ReportViewingID] ASC
) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [ReportViewings]  ADD CONSTRAINT [FK_ReportViewings_tblPermissions_EmployeeID] FOREIGN KEY (
[EmployeeID]
) REFERENCES [tblPermissions] (
[RecID]
)
GO

-----------------------------------------------------------------------------------------------------
-----------------------------------------PUBLIC RTO VIEWINGS-----------------------------------------
-----------------------------------------------------------------------------------------------------
CREATE TABLE [PublicRTOViewings] (
	[PublicRTOViewingID] int unique identity not null,
	[ViewDate] datetime unique not null,
	CONSTRAINT [PK_PublicRTOViewings] PRIMARY KEY CLUSTERED (
		[PublicRTOViewingID] ASC
	) ON [PRIMARY]
) ON [PRIMARY];
GO

-----------------------------------------------------------------------------------------------------
------------------------------------------------MISC.------------------------------------------------
-----------------------------------------------------------------------------------------------------

-- these should have already been there
ALTER TABLE [tblNJAWValves] ADD CONSTRAINT [FK_Valves_Streets_StName] FOREIGN KEY (
	[StName]
) REFERENCES [Streets] (
	[StreetID]
)
GO
ALTER TABLE [tblNJAWValves] ADD CONSTRAINT [FK_Valves_Towns_Town] FOREIGN KEY (
	[Town]
) REFERENCES [Towns] (
	[TownID]
)
GO

-----------------------------------------------INDEXES-----------------------------------------------
CREATE NONCLUSTERED INDEX [IDX_WorkOrders_OperatingCenterID_WorkDescriptionID_INCLUDE_DateCompleted] ON [dbo].[WorkOrders]
(
	[OperatingCenterID] ASC,
	[WorkDescriptionID] ASC
)
INCLUDE ( [DateCompleted]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
go

CREATE NONCLUSTERED INDEX [IDX_WorkDescriptions_WorkDescriptionID_WorkCategoryID] ON [dbo].[WorkDescriptions]
(
	[WorkDescriptionID],
	[WorkCategoryID]
)WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
go

CREATE NONCLUSTERED INDEX [IDX_Markouts_WorkOrderID_ExpirationDate] ON [dbo].[Markouts] 
(
	[WorkOrderID] ASC,
	[ExpirationDate] ASC
)
GO

CREATE NONCLUSTERED INDEX [IDX_WorkOrders_SchedulingSearch] ON [dbo].[WorkOrders] 
(
	[MarkoutRequirementID] ASC,
	[OperatingCenterID] ASC,
	[AssetTypeID] ASC,
	[TownID] ASC,
	[PriorityID] ASC,
	[StreetOpeningPermitRequired] ASC,
	[DateCompleted] ASC
)
GO

CREATE STATISTICS [STAT_WorkOrders_DateCompleted_WorkDescriptionID] ON [dbo].[WorkOrders]([DateCompleted], [WorkDescriptionID])
go

CREATE STATISTICS [STAT_WorkOrders_WorkDescriptionID_OperatingCenterID] ON [dbo].[WorkOrders]([WorkDescriptionID], [OperatingCenterID])
go

CREATE TABLE [RequisitionTypes] (
[RequisitionTypeID] int unique identity not null,
[Description] varchar(50) not null,
CONSTRAINT [PK_RequisitionTypes] PRIMARY KEY CLUSTERED (
[RequisitionTypeID] ASC
) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [Requisitions] (
[RequisitionID] int unique identity not null,
[RequisitionTypeID] int not null,
[SAPRequisitionNumber] varchar(50) null,
[WorkOrderID] int not null,
[CreatorID] int not null,
[CreatedOn] smalldatetime not null default getdate()
CONSTRAINT [PK_Requisitions] PRIMARY KEY CLUSTERED (
[RequisitionID] ASC
) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [Requisitions]  WITH NOCHECK ADD CONSTRAINT [FK_Requisitions_RequisitionType_RequisitionTypeID] FOREIGN KEY (
[RequisitionTypeID]
) REFERENCES [RequisitionTypes] (
[RequisitionTypeID]
)
GO

ALTER TABLE [Requisitions]  WITH NOCHECK ADD CONSTRAINT [FK_Requisitions_WorkOrders_WorkOrderID] FOREIGN KEY (
[WorkOrderID]
) REFERENCES [WorkOrders] (
[WorkOrderID]
)
GO

ALTER TABLE [Requisitions]  WITH NOCHECK ADD CONSTRAINT [FK_Requisitions_tblPermissions_CreatorID] FOREIGN KEY (
[CreatorID]
) REFERENCES [tblPermissions] (
[RecID]
)
GO
