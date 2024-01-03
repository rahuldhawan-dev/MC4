SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

---------------------------------------------WORK ORDERS---------------------------------------------
SET IDENTITY_INSERT [dbo].[WorkOrders] ON;
BEGIN TRANSACTION;
INSERT INTO [dbo].[WorkOrders]([WorkOrderID], [OldWorkOrderNumber], [CreatedOn], [CreatorID], [DateReceived], [DateStarted], [CustomerName], [StreetNumber], [StreetID], [NearestCrossStreetID], [TownID], [TownSectionID], [ZipCode], [PhoneNumber], [SecondaryPhoneNumber], [CustomerAccountNumber], [RequesterID], [OfficialInfo], [ServiceNumber], [ORCOMServiceOrderNumber], [AccountCharged], [PriorityID], [DateCompleted], [CompletedByID], [Notes], [DatePrinted], [DateReportSent], [ApprovedByID], [ApprovedOn], [BackhoeOperator], [ExcavationDate], [DateCompletedPC], [PremiseNumber], [InvoiceNumber], [RequestingEmployeeID], [PurposeID], [AssetTypeID], [WorkDescriptionID], [MarkoutRequirementID], [TrafficControlRequired], [StreetOpeningPermitRequired], [ValveID], [HydrantID], [LostWater], [RestorationID], [MainLineID], [NumberOfOfficersRequired], [Latitude], [Longitude], [OperatingCenterID], [MaterialsApprovedByID], [MaterialsApprovedOn], [MaterialsDocID], [SewerManholeID], [DistanceFromCrossStreet], [OfficeAssignmentID], [OfficeAssignedOn], [OriginalOrderNumber], [BusinessUnit], [StormCatchID], [CustomerImpactRangeID], [RepairTimeRangeID], [AlertID], [SignificantTrafficImpact])
---- NJ7 ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
SELECT '1', NULL, '20100326 09:31:00.000', '291', '20100326 09:31:00.000', '20100326 09:31:00.000', 'John Smith', '123', '7067', '706', '62', '33', '07720', '(888)555-1212', '(888)555-1212', NULL, '1', NULL, NULL, NULL, NULL, '2', '20100326 09:31:00.000', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '1', '1', '65', '1', '1', '0', '52841', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '10', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL UNION ALL
SELECT '2', NULL, '20100326 09:31:00.000', '291', '20100326 09:31:00.000', NULL, 'John Smith', '123', '7067', '706', '62', '33', '07720', '(888)555-1212', NULL, NULL, '1', NULL, NULL, NULL, NULL, '4', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '1', '1', '65', '2', '0', '0', '52841', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '10', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL UNION ALL
SELECT '3', NULL, '20100326 09:31:00.000', '291', '20100326 09:31:00.000', NULL, 'John Smith', '123', '968', '3718', '47', NULL, '07757-1125', '(888)555-1212', '(888)555-1212', NULL, '1', NULL, NULL, NULL, NULL, '4', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '1', '1', '65', '2', '1', '1', '40794', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '10', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL UNION ALL
SELECT '4', '23', '20100326 09:31:00.000', '291', '20100326 09:31:00.000', NULL, 'John Smith', '123', '968', '3718', '47', NULL, '07757-1125', '(888)555-1212', NULL, NULL, '1', NULL, NULL, NULL, NULL, '2', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '4', '2', '19', '1', '0', '0', NULL, '659', NULL, NULL, NULL, NULL, NULL, NULL, '10', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL UNION ALL
SELECT '5', NULL, '20100326 09:31:00.000', '291', '20100326 09:31:00.000', '20100326 09:31:00.000', 'John Smith', '123', '968', '3718', '47', NULL, '07757-1125', '(888)555-1212', '(888)555-1212', NULL, '1', NULL, NULL, NULL, '12345678', '1', '20100326 09:31:00.000', NULL, NULL, NULL, NULL, '402', '20100326 09:31:00.000', NULL, NULL, NULL, NULL, NULL, NULL, '4', '2', '29', '3', '1', '1', NULL, '659', '100000', NULL, NULL, NULL, NULL, NULL, '10', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL UNION ALL
SELECT '40', NULL, '20100326 09:31:00.000', '291', '20100326 09:31:00.000', NULL, 'John Smith', '123', '968', '3718', '47', NULL, '07757-1125', '(888)555-1212', NULL, NULL, '1', NULL, NULL, NULL, NULL, '2', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '4', '2', '19', '1', '0', '1', NULL, '659', NULL, NULL, NULL, NULL, NULL, NULL, '10', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL UNION ALL
SELECT '7', NULL, '20100326 09:31:00.000', '291', '20100326 09:31:00.000', NULL, '', '123', '968', '3718', '47', NULL, '12345', '', '', NULL, '2', NULL, NULL, NULL, NULL, '2', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '123456789', NULL, '397', '1', '4', '59', '1', '0', '0', NULL, NULL, NULL, NULL, NULL, NULL, '40.2044109668778', '-74.0080690383911', '10', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL UNION ALL
SELECT '8', NULL, '20100326 09:31:00.000', '291', '20100326 09:31:00.000', NULL, '', '123', '968', '3718', '47', NULL, '12345', '', '', NULL, '2', NULL, NULL, NULL, NULL, '2', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '191', '1', '1', '64', '2', '0', '0', '40794', NULL, NULL, NULL, NULL, NULL, '40.2043557', '-74.0080703', '10', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL UNION ALL
---- NJ4 ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
SELECT '9', NULL, '20100326 09:31:00.000', '291', '20100326 09:31:00.000', '20100326 09:31:00.000', 'John Smith', '123', '36989', '29662', '193', NULL, '07720', '(888)555-1212', '(888)555-1212', NULL, '1', NULL, NULL, NULL, NULL, '2', '20100326 09:31:00.000', NULL, 'Finalization/Supervisor Approval - Valve Order', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '1', '1', '65', '1', '1', '0', '88423', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '14', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL UNION ALL
SELECT '10', NULL, '20100326 09:31:00.000', '291', '20100326 09:31:00.000', NULL, 'John Smith', '123', '36914', '29662', '193', NULL, '07720', '(888)555-1212', NULL, NULL, '1', NULL, NULL, NULL, NULL, '4', NULL, NULL, 'I am writing notes
I hope that this will not break
Well here goes nothing', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '1', '1', '65', '2', '0', '0', '88359', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '14', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL UNION ALL
SELECT '11', NULL, '20100326 09:31:00.000', '291', '20100326 09:31:00.000', NULL, 'John Smith', '123', '33844', '29675', '193', NULL, '07757-1125', '(888)555-1212', '(888)555-1212', NULL, '1', NULL, NULL, NULL, NULL, '4', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '1', '1', '65', '2', '1', '1', '88332', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '14', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL UNION ALL
SELECT '41', '23', '20100326 09:31:00.000', '291', '20100326 09:31:00.000', NULL, 'John Smith', '123', '31754', '56337', '193', NULL, '07757-1125', '(888)555-1212', NULL, NULL, '1', NULL, NULL, NULL, NULL, '2', NULL, NULL, 'Scheduling', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '4', '2', '19', '1', '0', '0', NULL, '11644', NULL, NULL, NULL, NULL, NULL, NULL, '14', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL UNION ALL
SELECT '13', NULL, '20100326 09:31:00.000', '291', '20100326 09:31:00.000', '20100326 09:31:00.000', 'John Smith', '123', '56337', '32242', '193', NULL, '07757-1125', '(888)555-1212', '(888)555-1212', NULL, '1', NULL, NULL, NULL, '12345678', '1', '20100326 09:31:00.000', NULL, 'Finalization/Stock To Issue/Restoration Processing', NULL, NULL, '402', '20100326 09:31:00.000', NULL, NULL, NULL, NULL, NULL, NULL, '4', '2', '29', '3', '1', '1', NULL, '11649', '100000', NULL, NULL, NULL, NULL, NULL, '14', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL UNION ALL
SELECT '14', NULL, '20100326 09:31:00.000', '291', '20100326 09:31:00.000', NULL, 'John Smith', '123', '56337', '32242', '193', NULL, '07757-1125', '(888)555-1212', NULL, NULL, '1', NULL, NULL, NULL, NULL, '2', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '4', '2', '19', '1', '0', '1', NULL, '11649', NULL, NULL, NULL, NULL, NULL, NULL, '14', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL UNION ALL
SELECT '43', NULL, '20100326 09:31:00.000', '291', '20100326 09:31:00.000', NULL, '', '17', '37328', NULL, '193', NULL, '12345', '', '', NULL, '2', NULL, NULL, NULL, NULL, '2', NULL, NULL, 'Scheduling', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '180463970', NULL, '397', '1', '4', '59', '1', '0', '0', NULL, NULL, NULL, NULL, NULL, NULL, '40.0072882', '-74.0580982', '14', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL UNION ALL
SELECT '16', NULL, '20100326 09:31:00.000', '291', '20100326 09:31:00.000', NULL, '', '123', '33844', '29675', '193', NULL, '12345', '', '', NULL, '2', NULL, NULL, NULL, NULL, '2', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '191', '1', '1', '64', '2', '0', '0', '88332', NULL, NULL, NULL, NULL, NULL, '40.2043557', '-74.0080703', '10', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL UNION ALL
SELECT '17', NULL, '20100326 09:31:00.000', '291', '20100326 09:31:00.000', '20100326 09:31:00.000', 'John Smith', '321', '56337', '32242', '193', NULL, '07757-1125', '(888)555-1313', '(888)555-1214', NULL, '1', NULL, NULL, NULL, '12345678', '1', '20100326 09:31:00.000', NULL, 'Finalization/Stock To Issue/Restoration Processing', NULL, NULL, '402', '20100326 09:31:00.000', NULL, NULL, NULL, NULL, NULL, NULL, '4', '2', '29', '3', '1', '1', NULL, '11649', '100000', NULL, NULL, NULL, NULL, NULL, '14', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL UNION ALL
SELECT '18', NULL, '20100326 09:31:00.000', '291', '20100326 09:31:00.000', '20100326 09:31:00.000', 'John Smith', '123', '36914', '29662', '193', NULL, '07720', '(888)555-1212', '(888)555-1212', NULL, '1', NULL, NULL, NULL, NULL, '2', '20100326 09:31:00.000', NULL, 'Finalization/Supervisor Approval', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '1', '1', '65', '1', '1', '0', '88359', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '14', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL UNION ALL
---- MAINS --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
SELECT '19', NULL, '20100326 09:31:00.000', '291', '20100326 09:31:00.000', NULL, '', '123', '33844', '29675', '193', NULL, '12345', '', '', NULL, '2', NULL, NULL, NULL, NULL, '2', NULL, NULL, 'Planning, is for a main break', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '191', '1', '3', '74', '2', '0', '0', '88332', NULL, NULL, NULL, NULL, NULL, '40.2043557', '-74.0080703', '14', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '4', '3', '1', '0' UNION ALL
SELECT '20', NULL, '20090930 09:16:00.000', '291', '20090930 00:00:00.000', NULL, '', '228', '5457', '1115', '42', NULL, '', '', '', NULL, '3', NULL, '', '', NULL, '2', NULL, NULL, 'Test', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', NULL, NULL, '1', '3', '54', '1', '0', '0', NULL, NULL, NULL, NULL, NULL, NULL, '40.23402', '-74.00059', '10', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL UNION ALL
SELECT '21', NULL, '20090930 10:57:00.000', '291', '20090930 00:00:00.000', NULL, '', '10', '30883', '37226', '189', NULL, '', '', '', NULL, '3', NULL, '', '', NULL, '2', NULL, NULL, '', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', NULL, NULL, '5', '5', '92', '1', '0', '0', NULL, NULL, NULL, NULL, NULL, NULL, '40.18188', '-74.25994', '14', NULL, NULL, NULL, '3509', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL UNION ALL
SELECT '22', NULL, '20090930 11:10:00.000', '291', '20090930 00:00:00.000', NULL, '', '12', '30883', '37226', '189', NULL, '', '', '', NULL, '3', NULL, '32165498', '', NULL, '1', NULL, NULL, '', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '321654987', NULL, NULL, '6', '6', '87', '1', '0', '0', NULL, NULL, NULL, NULL, NULL, NULL, '40.1838', '-74.258', '14', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL UNION ALL
SELECT '23', NULL, '20090930 11:13:00.000', '291', '20090930 00:00:00.000', NULL, '', '11', '30883', '37226', '189', NULL, '', '', '', NULL, '3', NULL, '', '', NULL, '4', NULL, NULL, 'test', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', NULL, NULL, '4', '7', '86', '2', '0', '0', NULL, NULL, NULL, NULL, NULL, NULL, '40.182', '-74.256', '14', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL UNION ALL
SELECT '24', NULL, '20101012 14:28:00.000', '291', '20101012 00:00:00.000', NULL, '', '10', '9522', '9508', '74', NULL, '', '', '', NULL, '3', NULL, '', '', NULL, '4', NULL, NULL, '', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', NULL, NULL, '4', '8', '137', '1', '0', '0', NULL, NULL, NULL, NULL, NULL, NULL, '39.076909213154', '-74.7214508056641', '11', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '5', NULL, NULL, NULL, NULL UNION ALL
---- MAIN IN FINALIZATION -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
SELECT '42', NULL, '20100326 09:31:00.000', '291', '20100326 09:31:00.000', '20100326 09:31:00.000', 'John Smith', '123', '7067', '706', '62', '33', '07720', '(888)555-1212', '(888)555-1212', NULL, '1', NULL, NULL, NULL, NULL, '2', '20100326 09:31:00.000', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '1', '3', '2', '1', '1', '0', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '10', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL UNION ALL
---- MAIN BREAK / started / with open crew assignment / for wateroutage.com
SELECT '26', NULL, '20100326 09:31:00.000', '291', '20100326 09:31:00.000', '20100326 09:31:00.000', 'John Smith', '123', '7067', '706', '62', '33', '07720', '(888)555-1212', '(888)555-1212', NULL, '1', NULL, NULL, NULL, NULL, '2', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '1', '3', '74', '1', '1', '0', NULL, NULL, NULL, NULL, NULL, NULL, '40.44328', '-73.98948', '10', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 1, 1, 1
COMMIT;
RAISERROR (N'[dbo].[WorkOrders]: Insert Batch: 1.....Done!', 10, 1) WITH NOWAIT;
GO

-- orcom order completion
INSERT [dbo].[WorkOrders] ([WorkOrderID], [OldWorkOrderNumber], [CreatedOn], [CreatorID], [DateReceived], [DateStarted], [CustomerName], [StreetNumber], [StreetID], [NearestCrossStreetID], [TownID], [TownSectionID], [ZipCode], [PhoneNumber], [SecondaryPhoneNumber], [CustomerAccountNumber], [RequesterID], [OfficialInfo], [ServiceNumber], [ORCOMServiceOrderNumber], [AccountCharged], [PriorityID], [DateCompleted], [CompletedByID], [Notes], [DatePrinted], [DateReportSent], [ApprovedByID], [ApprovedOn], [BackhoeOperator], [ExcavationDate], [DateCompletedPC], [PremiseNumber], [InvoiceNumber], [RequestingEmployeeID], [PurposeID], [AssetTypeID], [WorkDescriptionID], [MarkoutRequirementID], [TrafficControlRequired], [StreetOpeningPermitRequired], [ValveID], [HydrantID], [LostWater], [RestorationID], [MainLineID], [NumberOfOfficersRequired], [Latitude], [Longitude], [OperatingCenterID], [MaterialsApprovedByID], [MaterialsApprovedOn], [MaterialsDocID], [SewerManholeID], [DistanceFromCrossStreet], [OfficeAssignmentID], [OfficeAssignedOn], [OriginalOrderNumber], [BusinessUnit], [StormCatchID], [CustomerImpactRangeID], [RepairTimeRangeID], [AlertID], [SignificantTrafficImpact]) 
	VALUES (44, NULL, CAST(0x9EC9031C AS SmallDateTime), 291, CAST(0x9EC90000 AS SmallDateTime), NULL, N'', N'228', 5457, 1115, 42, NULL, N'07711', N'', N'', NULL, 3, NULL, N'', N'123456789', N'12345678', 4, CAST(0x9EC90000 AS SmallDateTime), 291, N'Orcom Order Completion', NULL, NULL, 291, CAST(0x9EC9031E AS SmallDateTime), NULL, NULL, NULL, N'', NULL, NULL, 1, 2, 27, 1, 0, 0, NULL, 252, NULL, NULL, NULL, NULL, 40.23402797746, -74.00059997798, 10, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'181802', NULL, NULL, NULL, N'', NULL)

SET IDENTITY_INSERT [dbo].[WorkOrders] OFF;

------------------------------------------------CREWS------------------------------------------------

-- SET IDENTITY_INSERT [dbo].[Crews] ON;
-- BEGIN TRANSACTION;
-- INSERT INTO [dbo].[Crews]([CrewID], [Description], [OperatingCenterID], [Availability])
-- SELECT '1', 'Crew 1', (SELECT [OperatingCenterID] FROM [OperatingCenters] WHERE [OperatingCenterCode] = 'NJ7'), '6' UNION ALL
-- SELECT '2', 'Crew 2', (SELECT [OperatingCenterID] FROM [OperatingCenters] WHERE [OperatingCenterCode] = 'NJ7'), '8' UNION ALL
-- SELECT '3', 'Crew 1', (SELECT [OperatingCenterID] FROM [OperatingCenters] WHERE [OperatingCenterCode] = 'NJ4'), '6' UNION ALL
-- SELECT '4', 'Crew 2', (SELECT [OperatingCenterID] FROM [OperatingCenters] WHERE [OperatingCenterCode] = 'NJ4'), '8' UNION ALL
-- SELECT '5', 'Crew 1', (SELECT [OperatingCenterID] FROM [OperatingCenters] WHERE [OperatingCenterCode] = 'EW3'), '6' UNION ALL
-- SELECT '6', 'Crew 2', (SELECT [OperatingCenterID] FROM [OperatingCenters] WHERE [OperatingCenterCode] = 'EW3'), '8' UNION ALL
-- SELECT '7', 'Crew 1', (SELECT [OperatingCenterID] FROM [OperatingCenters] WHERE [OperatingCenterCode] = 'EW1'), '6' UNION ALL
-- SELECT '8', 'Crew 2', (SELECT [OperatingCenterID] FROM [OperatingCenters] WHERE [OperatingCenterCode] = 'EW1'), '8'
-- COMMIT;
-- RAISERROR (N'[dbo].[Crews]: Insert Batch: 1.....Done!', 10, 1) WITH NOWAIT;
-- GO

-- SET IDENTITY_INSERT [dbo].[Crews] OFF;

-- ------------------------------------------CREW ASSIGNMENTS-------------------------------------------
-- SET IDENTITY_INSERT [dbo].[CrewAssignments] ON;
-- BEGIN TRANSACTION;
-- INSERT INTO [dbo].[CrewAssignments]([CrewAssignmentID], [CrewID], [WorkOrderID], [AssignedOn], [AssignedFor], [Priority], [DateStarted])
-- SELECT '1', '1', '1', getdate(), dateadd(dd,0, datediff(dd,0,getDate())), '1', NULL UNION ALL
-- SELECT '2', '1', '5', getDate(), dateadd(dd,0, datediff(dd,0,getDate())), '2', NULL UNION ALL
-- SELECT '3', '3', '9', getdate(), dateadd(dd,0, datediff(dd,0,getDate())), '1', NULL UNION ALL
-- SELECT '4', '3', '13', getDate(), dateadd(dd,0, datediff(dd,0,getDate())), '2', NULL UNION ALL
-- SELECT '5', '1', '25', getdate(), dateadd(dd,0, datediff(dd,0,getDate())), '3', NULL UNION ALL
-- SELECT '6', '3', '26', getDate(), getDate(), '1', getDate()
-- COMMIT;
-- RAISERROR (N'[dbo].[CrewAssignments]: Insert Batch: 1.....Done!', 10, 1) WITH NOWAIT;
-- GO

-- SET IDENTITY_INSERT [dbo].[CrewAssignments] OFF;

-------------------------------------------STOCK LOCATIONS-------------------------------------------

SET IDENTITY_INSERT [dbo].[StockLocations] ON;
BEGIN TRANSACTION;
INSERT INTO [dbo].[StockLocations]([StockLocationID], [Description], [OperatingCenterID])
SELECT '1', 'H&M', (SELECT [OperatingCenterID] FROM [OperatingCenters] WHERE [OperatingCenterCode] = 'NJ7') UNION ALL
SELECT '2', 'H&M', (SELECT [OperatingCenterID] FROM [OperatingCenters] WHERE [OperatingCenterCode] = 'NJ4') UNION ALL
SELECT '3', 'H&M', (SELECT [OperatingCenterID] FROM [OperatingCenters] WHERE [OperatingCenterCode] = 'EW3')
COMMIT;
RAISERROR (N'[dbo].[StockLocations]: Insert Batch: 1.....Done!', 10, 1) WITH NOWAIT;
GO

SET IDENTITY_INSERT [dbo].[StockLocations] OFF;

-------------------------------------------MATERIALS USED--------------------------------------------

SET IDENTITY_INSERT [dbo].[MaterialsUsed] ON;
BEGIN TRANSACTION;
INSERT INTO [dbo].[MaterialsUsed]([MaterialsUsedID], [WorkOrderID], [MaterialID], [Quantity], [NonStockDescription], [StockLocationID])
SELECT '1', '5', '115', '2', NULL, '1' UNION ALL
SELECT '2', '5', NULL, '1', 'Sealant', NULL UNION ALL
SELECT '3', '13', '115', '2', NULL, '1' UNION ALL
SELECT '4', '13', NULL, '1', 'Sealant', NULL 
COMMIT;
RAISERROR (N'[dbo].[MaterialsUsed]: Insert Batch: 1.....Done!', 10, 1) WITH NOWAIT;
GO

SET IDENTITY_INSERT [dbo].[MaterialsUsed] OFF;

-------------------------------------------DEBUGGING USER--------------------------------------------

INSERT INTO [dbo].[tblPermissions]([Add1], [CDCCode], [CellNum], [City], [Company], [EMail], [EmpNum], [FaxNum], [FullName], [Location], [Inactive], [OpCntr1], [OpCntr2], [OpCntr3], [OpCntr4], [OpCntr5], [OpCntr6], [Password], [PhoneNum], [Region], [St], [UserInact], [UserLevel], [UserName], [Zip], [FBUserName], [FBPassWord], [XYMUserName], [XYMPassword], [userID], [uid], [DefaultOperatingCenterID])
SELECT N'3 Sheila Dr', N'', N'732-530-3280', N'Shrewsbury', N'NJAW', N'arystrom@mmsinc.com', N'00000000', N'732-530-3280', N'Mr. D. Buggin', N'', N'', N'NJ4', N'NJ5', N'NJ6', N'NJ7', N'', N'', N'devpwd3#', N'732-530-3280', N'Northeast', N'NJ', NULL, N'5   ', N'Mr. D. Buggin', N'07701', NULL, NULL, NULL, NULL, 970, N'1C307570-85C7-41D3-8D1E-10E2F6CBF820', 14 

----------------------------------------------DOCUMENTS-----------------------------------------------

SET IDENTITY_INSERT [dbo].[Document] ON;

INSERT INTO [Document] ([documentID], [documentTypeID], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Description], [BinaryData], [File_Size], [File_Name], [CreatedByID], [ModifiedByID])
SELECT
	[DocumentID],
	(SELECT DocumentTypeID FROM [DocumentType] WHERE [DOCUMENT_TYPE] = 'Traffic Control'),
	CreatedBy,
	CreatedOn,
	ModifiedBy,
	ModifiedOn,
	Description,
	BinaryData,
	File_Size,
	File_Name,
	(Select top 1 RecID from tblPermissions where FullName = 'Mr. D. Buggin'), 
	(Select top 1 RecID from tblPermissions where FullName = 'Mr. D. Buggin')
FROM
	fatman.[mapcall_2013_02_04].dbo.document
WHERE
	[DocumentID] IN (112,555,568,577,633,647,648,649,652,654)
SET IDENTITY_INSERT [dbo].[Document] OFF;

----------------------------------------DOCUMENTS WORK ORDERS----------------------------------------
INSERT INTO [DocumentsWorkOrders]
SELECT 112, 1 UNION ALL
SELECT 555, 1 UNION ALL
SELECT 568, 1 UNION ALL
SELECT 577, 1 UNION ALL
SELECT 633, 1 UNION ALL
SELECT 647, 1 UNION ALL
SELECT 648, 1 UNION ALL
SELECT 649, 1 UNION ALL
SELECT 652, 1 UNION ALL
SELECT 654, 1;


---------------------------------------------RESTORATIONS----------------------------------------------
SET IDENTITY_INSERT [dbo].[Restorations] ON;
BEGIN TRANSACTION;
INSERT INTO [dbo].[Restorations]([RestorationID], [WorkOrderID], [RestorationTypeID], [PavingSquareFootage], [LinearFeetOfCurb], [RestorationNotes], [PartialRestorationMethodID], [PartialRestorationInvoiceNumber], [PartialRestorationDate], [PartialRestorationCompletedBy], [PartialPavingBreakOutEightInches], [PartialPavingBreakOutTenInches], [PartialSawCutting], [PartialPavingSquareFootage], [DaysToPartialPaveHole], [TrafficControlCostPartialRestoration], [FinalRestorationMethodID], [FinalRestorationInvoiceNumber], [FinalRestorationDate], [FinalRestorationCompletedBy], [FinalPavingBreakOutEightInches], [FinalPavingBreakOutTenInches], [FinalSawCutting], [FinalPavingSquareFootage], [DaysToFinalPaveHole], [TrafficControlCostFinalRestoration], [ApprovedByID], [DateApproved], [RejectedByID], [DateRejected], [EightInchStabilizeBaseByCompanyForces], [SawCutByCompanyForces], [TotalAccruedCost], [TotalInitialActualCost], [FinalRestorationActualCost])
SELECT '4', '1', '1', '12', NULL, '', NULL, '', NULL, '', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', NULL, '', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '0', '0', '144.00', NULL, NULL UNION ALL
SELECT '5', '42', '1', '12', NULL, '', NULL, '', NULL, '', NULL, NULL, NULL, NULL, NULL, NULL, NULL, '', NULL, '', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '0', '0', '144.00', NULL, NULL
COMMIT;
RAISERROR (N'[dbo].[Restorations]: Insert Batch: 1.....Done!', 10, 1) WITH NOWAIT;
GO

SET IDENTITY_INSERT [dbo].[Restorations] OFF;

UPDATE [Restorations] SET [ResponsePriorityID] = (SELECT [RestorationResponsePriorityID] FROM [RestorationResponsePriorities] WHERE [Description] = 'Standard (30 days)');

--------------------------------------------------------------------------------
---------------------------------NOTIFICATIONS----------------------------------
--------------------------------------------------------------------------------

-- THIS WILL FAIL DUE TO THE CURRENT SETUP, NEED TO ADD COLUMNS
DECLARE @contactId int 
INSERT INTO [Contacts] (FirstName, LastName, CreatedBy, Email) VALUES ('Jason', 'Duncan', 'mcadmin', 'jduncan@mmsinc.com');
set @contactId = (select @@IDENTITY)

INSERT INTO [NotificationPurposes] (ModuleID, Purpose) VALUES (34, 'Curb-Pit Compliance');
INSERT INTO [NotificationPurposes] (ModuleID, Purpose) VALUES (34, 'Curb-Pit Revenue');
INSERT INTO [NotificationPurposes] (ModuleID, Purpose) VALUES (34, 'Curb-Pit Estimate');
INSERT INTO [NotificationPurposes] (ModuleID, Purpose) VALUES (34, 'Supervisor Approval');
INSERT INTO [NotificationPurposes] (ModuleID, Purpose) VALUES (34, 'Main Break Entered');
INSERT INTO [NotificationPurposes] (ModuleID, Purpose) VALUES (34, 'Service Line Installation Entered');
INSERT INTO [NotificationPurposes] (ModuleID, Purpose) VALUES (34, 'Service Line Renewal Entered');

INSERT INTO [NotificationConfigurations] (ContactID, OperatingCenterID, NotificationPurposeID) VALUES (@contactId, 10, 1);
INSERT INTO [NotificationConfigurations] (ContactID, OperatingCenterID, NotificationPurposeID) VALUES (@contactId, 14, 1);
INSERT INTO [NotificationConfigurations] (ContactID, OperatingCenterID, NotificationPurposeID) VALUES (@contactId, 10, 2);
INSERT INTO [NotificationConfigurations] (ContactID, OperatingCenterID, NotificationPurposeID) VALUES (@contactId, 14, 2);
INSERT INTO [NotificationConfigurations] (ContactID, OperatingCenterID, NotificationPurposeID) VALUES (@contactId, 10, 3);
INSERT INTO [NotificationConfigurations] (ContactID, OperatingCenterID, NotificationPurposeID) VALUES (@contactId, 14, 3);


-- Tap/Valve Images
INSERT INTO [dbo].[tblNJAWService]([OpCntr], [PremNum], [ServNum], [Town])
	select OperatingCenterCode, PremiseNumber, ServiceNumber, TownID from WorkOrders wo join OperatingCenters OC on OC.OperatingCenterID = WO.OperatingCenterID where isNull(ServiceNumber, '') <> ''
	AND TownID in (Select TownID from Towns) AND ISNUMERIC(ServiceNumber) = 1

SET IDENTITY_INSERT dbo.TapImages ON;
INSERT INTO [dbo].[TapImages]([TapImageID], [OperatingCenter], [FileList], [fld], [TownID], [ServiceID], [PremiseNumber], [ServiceNumber], [OldTapID], [State])
SELECT TOP 10 
	[TapImageID], [OperatingCenter], [FileList], [fld], [TownID], [ServiceID], '321654987' as [PremiseNumber], '321654987' as [ServiceNumber], [OldTapID], [State] 
FROM 
	[Fatman].[mapcall_2013_02_04].dbo.TapImages 
where 
	OldTapID is not null 
AND 
	ServiceID in (Select RecID from tblNJAWService)
AND
	TownID in (Select TownID from Towns)
	
SET IDENTITY_INSERT dbo.TapImages OFF;

SET IDENTITY_INSERT dbo.ValveImages ON;
INSERT INTO ValveImages([ValveImageID], [FileList], [fld], [ValveID], TownID, State, OldValveID, OperatingCenter)
	SELECT [ValveImageID], [FileList], [fld], [ValveID], TownID, State, OldValveID, OperatingCenter FROM [Fatman].[mapcall_2013_02_04].dbo.ValveImages 
	WHERE 
		OldValveID is not null
	AND
		ValveID in (Select ValveID from WorkOrders where ValveID is Not Null)
	AND 
		TownID in (Select TownID from Towns)
	AND 
		IsNull(OperatingCenter, '') <> ''		
SET IDENTITY_INSERT dbo.ValveImages OFF;
