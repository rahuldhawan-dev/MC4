/*
 * This script will import the access imported data, after it has been normalized,
 * to the [WorkOrdersMerged] db.
 */

use [WorkOrdersMerged]
GO

-----------------------------------------------------------------------------------------------------
-----------------------------------WorkOrders / tblWorkOrderInput------------------------------------
-----------------------------------------------------------------------------------------------------
INSERT INTO [WorkOrdersMerged].dbo.[WorkOrders] (
	[OldWorkOrderNumber], -- [Order Number]
	[CreatedOn], -- [CreationDate]
	[CreatorID],	-- (lookup) (there were no usable values in [UserName],
					-- so the userID of 'mcAdmin' in tblPermissions was
					-- used across the board. [CreatorID]
	[DateReceived], -- [Date Received]
	[DateStarted], -- [Date Started]
	[CustomerName], -- [Customer Name]
	[StreetNumber], -- [Street Number]
	[StreetID],	-- (lookup) [StreetID]
	[NearestCrossStreetID],	-- (lookup) [NearestCrossStreetID]
	[TownID],	-- (lookup) [TownID]
	[TownSectionID],	-- (lookup) [TownSectionID]
--	[ZipCode] (nothing matching in source table)
	[PhoneNumber], -- [Phone Number]
--	[SecondaryPhoneNumber] (nothing matching in source table)
	[CustomerAccountNumber], -- [Customer Account Number]
	[RequesterID],	-- (lookup) [RequesterID]
	[ServiceNumber], -- [Service Number]
--	[ORCOMServiceOrderNumber] (nothing matching in source table)
	[AccountCharged], -- [Account Charged]
	[PriorityID],	-- (lookup) [PriorityID]
	[DateCompleted], -- [Date Completed]
--	No Matching [print record] Column
	[DateReportSent], -- [Date Report Sent]
--	[SupervisorApproval] (lookup) (ignored.  too much variation, too little time)
--	No Matching [Minicipality Code] column
	[BackhoeOperator],	-- (lookup) [BackhoeOperator]
	[ExcavationDate], -- [Date of Excavation]
	[DateCompletedPC], -- [Date Completed On PC]
	[PremiseNumber], -- [Premise Number]
	[ValveID], -- (lookup) [ValveID]
	[HydrantID], -- (lookup) [HydrantID]
	[MainLineID], -- (lookup) [MainLineID]
--	No Matching [Palm Work Order Number] column
	[InvoiceNumber], -- [InvoiceNumber]
	[OfficialInfo], -- [OfficialInfo]
--	Need to pull latitude into asset record (or not)
--	Need to pull longitude into asset record (or not)
	[PurposeID], -- (lookup) [PurposeID]
	[RequestingEmployeeID],	-- (lookup) [RequestingEmployeeID]
	[Notes], -- [Notes]
--	[PurposeID] (nothing matching in source table)
	[AssetTypeID], -- (lookup) [AssetTypeID]
	[WorkDescriptionID],	-- (lookup) [WorkDescriptionID]
	[MarkoutRequirementID],	-- (lookup) [MarkoutRequirementID]
	[TrafficControlRequired],	-- static value of 0
	[StreetOpeningPermitRequired], -- static value of 0
	[Latitude],
	[Longitude]
--	[ValveID] (nothing matching in source table)
--	[HydrantID] (nothing matching in source table)
--	[TotalManHours] (nothing matching in source table)
--	[LostWater] (nothing matching in source table)
--	[RestorationID] (nothing matching in source table)
) SELECT
	[Order Number],
	IsNull([CreationDate], GetDate()),
	[CreatorID],
--	[UserName] (varchar, convert to lookup) (normalized to CreatorID)
	[Date Received],
	[Date Started],
	[Customer Name],
	[Street Number],
	[StreetID],
--	[Street Name] (varchar, convert to lookup) (normalized to StreetID)
--	[StreetName] (lookup?) (normalized to StreetID)
	[NearestCrossStreetID],
--	[Nearest Cross Street] (varchar, convert to lookup) (normalized to NearestCrossStreetID)
	[TownID],
--	[Town] (varchar, convert to lookup) (normalized to TownID)
	[TownSectionID],
--	[Town Section] (varchar, convert to lookup) (normalized to TownSectionID)
--	No Matching ZipCode Column
	[Phone Number],
--	No Matching SecondaryPhoneNumber column
	cast([Customer Account Number] as varchar),
	[RequesterID],
	[Service Number],
--	No Matching ORCOMServiceOrderNumber column
	[Account Charged],
	[PriorityID],
--	[Job Priority] (varchar, convert to lookup) (normalized to PriorityID)
	[Date Completed],
--	[print record] (nothing matching in destination table)
	[Date Report Sent],
--	[Supervisor Approval] (varchar, convert to lookup) (ignored.  too much variation, too little time)
--	[Municipality Code] (nothing matching in destination table)
	[BackhoeOperator],
--	[Backhoe Operator] (varchar, convert to lookup) (normalized to BackhoeOperator)
	[Date of Excavation],
	[Date Completed On PC],
	[Premise Number],
	[ValveID],
	[HydrantID],
	[MainLineID],
--	[Palm Work Order Number] (nothing matching in destination table)
	[InvoiceNumber],
	[OfficialInfo],
--	[Latitude] (ties to asset record)
--	[Longitude] (ties to asset record)
	[PurposeID],
	[RequestingEmployeeID],
	[Notes],
--	[Requested By] (varchar, convert to lookup) (normalized to RequestingEmployeeID, OfficialInfo, and RequesterID)
	[AssetTypeID],
	[WorkDescriptionID],
	[MarkoutRequirementID],
	0 as [TrafficControlRequired],
	0 as [StreetOpeningPermitRequired],
--	[Description of Job] (varchar, convert to lookup) (normalized to WorkDescriptionID)
	[Latitude],
	[Longitude]
FROM
	[WorkOrdersImport].dbo.[tblWorkInputTable]

-----------------------------------------------------------------------------------------------------
----------------------------------------Markouts / tblMarkout----------------------------------------
-----------------------------------------------------------------------------------------------------
INSERT INTO [WorkOrdersMerged].dbo.[Markouts] (
	[WorkOrderID], -- (lookup) wo.[WorkOrderID]
	[MarkoutNumber], -- mo.[Markout Number]
	[DateOfRequest], -- [Date of M O Request]
	[ReadyDate] -- [Markout Due Date]
--	[ExpirationDate] (nothing matching. need to put something in here based on DateOfRequest.
--                        does not matter if the work order is completed, but for ones that aren't
--                        this will probably need to be run through the date generation code in the site)
) SELECT
	wo.[WorkOrderID],
	mo.[Markout Number],
	[Date of M O Request],
	[MARKOUT ENTERED DATE]
FROM
	[WorkOrdersImport].dbo.[tblMarkout] as mo
INNER JOIN
	[WorkOrdersMerged].dbo.[WorkOrders] as wo
ON
	mo.[Order Number] = wo.[OldWorkOrderNumber]

-----------------------------------------------------------------------------------------------------
--------------------------------------Restorations / tblPaving---------------------------------------
-----------------------------------------------------------------------------------------------------
INSERT INTO [WorkOrdersMerged].dbo.[Restorations] (
	[WorkOrderID], -- (lookup) wo.[WorkOrderID]
	[RestorationTypeID], -- (lookup) p.[RestorationTypeID]
	[PavingSquareFootage], -- p.[Paving Square Footage]
	[LinearFeetOfCurb], -- p.[Linear Ft of Curb]
	[RestorationNotes], -- p.[Restoration Notes]
--	[PartialRestorationMethodID] (nothing matching in source table)
	[PartialRestorationInvoiceNumber], -- p.[Partial Restoration Invoice Number]
	[PartialRestorationDate], -- p.[Partial Restoration Date]
	[PartialRestorationCompletedBy], -- p.[Partial Restoration Completed By]
	[PartialPavingBreakOutEightInches], -- p.[Partial Paving Break out 8"]
	[PartialPavingBreakOutTenInches], -- p.[Partial Paving Break out 10"]
	[PartialSawCutting], -- p.[Partial Saw cutting]
	[PartialPavingSquareFootage], -- p.[Partial Paving Square Footage]
	[DaysToPartialPaveHole], -- p.[Days to Partical Pave hole]
	[TrafficControlHoursPartialRestoration], -- p.[Traffic Control Hours Partial Restoration]
--	[FinalRestorationMethodID], (nothing matching in source table)
	[FinalRestorationInvoiceNumber], -- p.[Final Restoration Invoice Number]
	[FinalRestorationDate], -- p.[Final Restoration Completion Date]
	[FinalRestorationCompletedBy], -- p.[Final Restoration Completed By]
	[FinalPavingBreakOutEightInches], -- p.[Final Concrete Break out 8"]
	[FinalPavingBreakOutTenInches], -- p.[Final Concrete Break out 10"]
	[FinalSawCutting], -- p.[Final Saw cutting]
	[FinalPavingSquareFootage], -- p.[Final Paving Square Footage]
	[DaysToFinalPaveHole], -- p.[Days to Final Pave hole]
	[TrafficControlHoursFinalRestoration], -- p.[Traffic Control Hours Final  Restoration]
--	[ApprovedByID] (cannot match atm)
	[DateApproved], -- p.[Date Approved]
--	[RejectedByID] (cannot match atm)
	[DateRejected], -- p.[Date Rejected]
	[EightInchStabilizeBaseByCompanyForces], -- p.[EightInchStabilizeBaseByCompanyForces]
	[SawCutByCompanyForces] -- p.[SawCutByCompanyForces]
) SELECT
	wo.[WorkOrderID],
	p.[RestorationTypeID],
	p.[Paving Square Footage],
	p.[Linear Ft of Curb],
	p.[Restoration Notes],
	p.[Partial Restoration Invoice Number],
	p.[Partial Restoration Date],
	p.[Partial Restoration Completed By],
	p.[Partial Paving Break out 8"],
	p.[Partial Paving Break out 10"],
	p.[Partial Saw cutting],
	p.[Partial Paving Square Footage],
	p.[Days to Partical Pave hole],
	p.[Traffic Control Hours Partial Restoration],
	p.[Final Restoration Invoice Number],
	p.[Final Restoration Completion Date],
	p.[Final Restoration Completed By],
	p.[Final Concrete Break out 8"],
	p.[Final Concrete Break out 10"],
	p.[Final Saw cutting],
	p.[Final Paving Square Footage],
	p.[Days to Final Pave hole],
	p.[Traffic Control Hours Final  Restoration],
	p.[Date Approved],
	p.[Date Rejected],
	p.[EightInchStabilizeBaseByCompanyForces],
	p.[SawCutByCompanyForces]
FROM
	[WorkOrdersImport].dbo.[tblPaving] as p
INNER JOIN
	[WorkOrdersMerged].dbo.[WorkOrders] as wo
ON
	p.[Order Number] = wo.[OldWorkOrderNumber]