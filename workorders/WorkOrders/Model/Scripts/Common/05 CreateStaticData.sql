-- This script will create all of the static data used by this system.  This should also be run on the
-- live server when 271 is rolled out.  You'll need to set the specific db you're using before running.

----------------------------------------WORK ORDER PRIORITIES----------------------------------------
--INSERT INTO [WorkOrderPriorities] ([Description]) VALUES ('Emergency');
--INSERT INTO [WorkOrderPriorities] ([Description]) VALUES ('High Priority');
--INSERT INTO [WorkOrderPriorities] ([Description]) VALUES ('Revenue Related');
--INSERT INTO [WorkOrderPriorities] ([Description]) VALUES ('Routine');

-----------------------------------------WORK ORDER PURPOSES-----------------------------------------
--INSERT INTO [WorkOrderPurposes] ([Description]) VALUES ('Customer');		-- 1
--INSERT INTO [WorkOrderPurposes] ([Description]) VALUES ('Revenue');			-- 2
--INSERT INTO [WorkOrderPurposes] ([Description]) VALUES ('Compliance');		-- 3
--INSERT INTO [WorkOrderPurposes] ([Description]) VALUES ('Safety');			-- 4
--INSERT INTO [WorkOrderPurposes] ([Description]) VALUES ('Leak Detection');	-- 5
--INSERT INTO [WorkOrderPurposes] ([Description]) VALUES ('Revenue 150-500');	-- 6
--INSERT INTO [WorkOrderPurposes] ([Description]) VALUES ('Revenue 500-1000');-- 7
--INSERT INTO [WorkOrderPurposes] ([Description]) VALUES ('Revenue >1000');	-- 8
--INSERT INTO [WorkOrderPurposes] ([Description]) VALUES ('Damaged/Billable');-- 9
--INSERT INTO [WorkOrderPurposes] ([Description]) VALUES ('Estimates');		-- 10
--INSERT INTO [WorkOrderPurposes] ([Description]) VALUES ('Water Quality');	-- 11
--INSERT INTO [WorkOrderPurposes] ([Description]) VALUES ('');				-- 12 - empty
--INSERT INTO [WorkOrderPurposes] ([Description]) VALUES ('Asset Record Control') --13
--INSERT INTO [WorkOrderPurposes] ([Description]) VALUES ('Seasonal')			-- 14
--INSERT INTO [WorkOrderPurposes] ([Description]) VALUES ('Demolition')		-- 15
--INSERT INTO [WorkOrderPurposes] ([Description]) VALUES ('BPU')				-- 16	
--INSERT INTO [WorkOrderPurposes] ([Description]) VALUES ('');				-- 17 - empty
--INSERT INTO [WorkOrderPurposes] ([Description]) VALUES ('Hurricane Sandy')	-- 18

----------------------------------------WORK ORDER REQUESTERS----------------------------------------
--INSERT INTO [WorkOrderRequesters] ([Description]) VALUES ('Customer');
--INSERT INTO [WorkOrderRequesters] ([Description]) VALUES ('Employee');
--INSERT INTO [WorkOrderRequesters] ([Description]) VALUES ('Local Government');
--INSERT INTO [WorkOrderRequesters] ([Description]) VALUES ('Call Center');

-----------------------------------------------ASSET TYPES---------------------------------------------
--INSERT INTO [AssetTypes] ([Description]) VALUES ('Valve');
--INSERT INTO [AssetTypes] ([Description]) VALUES ('Hydrant');
--INSERT INTO [AssetTypes] ([Description]) VALUES ('Main');
--INSERT INTO [AssetTypes] ([Description]) VALUES ('Service');
--INSERT INTO [AssetTypes] ([Description]) VALUES ('Sewer Manhole');
--INSERT INTO [AssetTypes] ([Description]) VALUES ('Sewer Lateral');
--INSERT INTO [AssetTypes] ([Description]) VALUES ('Sewer Main');
--INSERT INTO [AssetTypes] ([Description]) VALUES ('Storm/Catch');
--INSERT INTO [AssetTypes] ([Description]) VALUES ('Equipment');

-------------------------------------------WORK CATEGORIES-------------------------------------------
SET IDENTITY_INSERT [dbo].[WorkCategories] ON;
BEGIN TRANSACTION;
INSERT INTO [dbo].[WorkCategories]([WorkCategoryID], [Description])
SELECT 55, N'Equipment'
COMMIT;
RAISERROR (N'[dbo].[WorkCategories]: Insert Batch: 1.....Done!', 10, 1) WITH NOWAIT;
GO

SET IDENTITY_INSERT [dbo].[WorkCategories] OFF;

------------------------------------------ACCOUNTING TYPES-------------------------------------------
INSERT INTO [dbo].[AccountingTypes] ([Description])
VALUES ('Capital')
INSERT INTO [dbo].[AccountingTypes] ([Description])
SELECT ('O&M')
INSERT INTO [dbo].[AccountingTypes] ([Description])
SELECT ('Retirement')

----------------------------RESTORATION PRODUCT CODES----------------------------
--SET IDENTITY_INSERT [dbo].[RestorationProductCodes] ON;

--INSERT INTO RestorationProductCodes(RestorationProductCodeID, Code) Values(1, 'TB04')

--SET IDENTITY_INSERT [dbo].[RestorationProductCodes] OFF;

--------------------------RESTORATION ACCOUNTING CODES--------------------------

--SET IDENTITY_INSERT [dbo].[RestorationAccountingCodes] ON;
--BEGIN TRANSACTION;
--INSERT INTO [dbo].[RestorationAccountingCodes]([RestorationAccountingCodeID], [Code], [SubCode])
--SELECT 1, N'105300', N'21' UNION ALL
--SELECT 2, N'675650', N'24' UNION ALL
--SELECT 3, N'675651', N'24' UNION ALL
--SELECT 4, N'675652', N'24' UNION ALL
--SELECT 5, N'675653', N'24' UNION ALL
--SELECT 6, N'675654', N'24' UNION ALL
--SELECT 7, N'675655', N'24' UNION ALL
--SELECT 8, N'675656', N'24' UNION ALL
--SELECT 9, N'675657', N'24' UNION ALL
--SELECT 10, N'675658', N'24' UNION ALL
--SELECT 11, N'675659', N'24' UNION ALL
--SELECT 12, N'675660', N'24' UNION ALL
--SELECT 13, N'675661', N'24' UNION ALL
--SELECT 14, N'675662', N'24' UNION ALL
--SELECT 15, N'675663', N'24' UNION ALL
--SELECT 16, N'675664', N'24' UNION ALL
--SELECT 17, N'675665', N'24' UNION ALL
--SELECT 18, N'675666', N'24' UNION ALL
--SELECT 19, N'675667', N'24' UNION ALL
--SELECT 20, N'675668', N'24' UNION ALL
--SELECT 21, N'675669', N'24' UNION ALL
--SELECT 22, N'675670', N'24' UNION ALL
--SELECT 23, N'675671', N'24' UNION ALL
--SELECT 24, N'675672', N'24' UNION ALL
--SELECT 25, N'675673', N'24' UNION ALL
--SELECT 26, N'185300', NULL
--COMMIT;
--RAISERROR (N'[dbo].[RestorationAccountingCodes]: Insert Batch: 1.....Done!', 10, 1) WITH NOWAIT;
--GO

--SET IDENTITY_INSERT [dbo].[RestorationAccountingCodes] OFF;

------------------------------------------WORK DESCRIPTIONS------------------------------------------

SET IDENTITY_INSERT WorkDescriptions ON

INSERT INTO [dbo].[WorkDescriptions]
	([WorkDescriptionID], [Description], [AssetTypeID], [TimeToComplete], [WorkCategoryID], [AccountingTypeID], [FirstRestorationAccountingCodeID], [FirstRestorationCostBreakdown], [FirstRestorationProductCodeID], [SecondRestorationAccountingCodeID], [SecondRestorationCostBreakdown], [SecondRestorationProductCodeID], [ShowBusinessUnit], [ShowApprovalAccounting], [EditOnly],[Revisit])
SELECT N'208' AS [WorkDescriptionID], N'Z-LWC/EW4 - 3 CONSECUTIVE MTHS OF 0 USAGE [ZERO]' AS [Description], N'4' AS [AssetTypeID], N'1.50' AS [TimeToComplete], N'26' AS [WorkCategoryID], N'1' AS [AccountingTypeID], N'1' AS [FirstRestorationAccountingCodeID], N'100' AS [FirstRestorationCostBreakdown], N'1' AS [FirstRestorationProductCodeID], NULL AS [SecondRestorationAccountingCodeID], NULL AS [SecondRestorationCostBreakdown], N'1' AS [SecondRestorationProductCodeID], N'0' AS [ShowBusinessUnit], N'1' AS [ShowApprovalAccounting], N'0' AS [EditOnly], N'0' AS [Revisit] UNION ALL 
SELECT N'209' AS [WorkDescriptionID], N'Z-LWC/EW4 - CHECK METER NON-EMERGENCY [CKMTR]' AS [Description], N'4' AS [AssetTypeID], N'1.50' AS [TimeToComplete], N'26' AS [WorkCategoryID], N'1' AS [AccountingTypeID], N'1' AS [FirstRestorationAccountingCodeID], N'100' AS [FirstRestorationCostBreakdown], N'1' AS [FirstRestorationProductCodeID], NULL AS [SecondRestorationAccountingCodeID], NULL AS [SecondRestorationCostBreakdown], N'1' AS [SecondRestorationProductCodeID], N'0' AS [ShowBusinessUnit], N'1' AS [ShowApprovalAccounting], N'0' AS [EditOnly], N'0' AS [Revisit] UNION ALL 
SELECT N'210' AS [WorkDescriptionID], N'Z-LWC/EW4 - DEMOLITION CLOSED ACCOUNT [DEMOC]' AS [Description], N'4' AS [AssetTypeID], N'1.50' AS [TimeToComplete], N'26' AS [WorkCategoryID], N'1' AS [AccountingTypeID], N'1' AS [FirstRestorationAccountingCodeID], N'100' AS [FirstRestorationCostBreakdown], N'1' AS [FirstRestorationProductCodeID], NULL AS [SecondRestorationAccountingCodeID], NULL AS [SecondRestorationCostBreakdown], N'1' AS [SecondRestorationProductCodeID], N'0' AS [ShowBusinessUnit], N'1' AS [ShowApprovalAccounting], N'0' AS [EditOnly], N'0' AS [Revisit] UNION ALL 
SELECT N'211' AS [WorkDescriptionID], N'Z-LWC/EW4 - METER CHANGE-OUT [MTRCH]' AS [Description], N'4' AS [AssetTypeID], N'1.50' AS [TimeToComplete], N'26' AS [WorkCategoryID], N'1' AS [AccountingTypeID], N'1' AS [FirstRestorationAccountingCodeID], N'100' AS [FirstRestorationCostBreakdown], N'1' AS [FirstRestorationProductCodeID], NULL AS [SecondRestorationAccountingCodeID], NULL AS [SecondRestorationCostBreakdown], N'1' AS [SecondRestorationProductCodeID], N'0' AS [ShowBusinessUnit], N'1' AS [ShowApprovalAccounting], N'0' AS [EditOnly], N'0' AS [Revisit] UNION ALL 
SELECT N'212' AS [WorkDescriptionID], N'Z-LWC/EW4 - READ MR EDIT - LOCAL OPS ONLY [MREDT]' AS [Description], N'4' AS [AssetTypeID], N'1.50' AS [TimeToComplete], N'26' AS [WorkCategoryID], N'1' AS [AccountingTypeID], N'1' AS [FirstRestorationAccountingCodeID], N'100' AS [FirstRestorationCostBreakdown], N'1' AS [FirstRestorationProductCodeID], NULL AS [SecondRestorationAccountingCodeID], NULL AS [SecondRestorationCostBreakdown], N'1' AS [SecondRestorationProductCodeID], N'0' AS [ShowBusinessUnit], N'1' AS [ShowApprovalAccounting], N'0' AS [EditOnly], N'0' AS [Revisit] UNION ALL 
SELECT N'213' AS [WorkDescriptionID], N'Z-LWC/EW4 - READ TO STOP ESTIMATE [EST]' AS [Description], N'4' AS [AssetTypeID], N'1.50' AS [TimeToComplete], N'26' AS [WorkCategoryID], N'1' AS [AccountingTypeID], N'1' AS [FirstRestorationAccountingCodeID], N'100' AS [FirstRestorationCostBreakdown], N'1' AS [FirstRestorationProductCodeID], NULL AS [SecondRestorationAccountingCodeID], NULL AS [SecondRestorationCostBreakdown], N'1' AS [SecondRestorationProductCodeID], N'0' AS [ShowBusinessUnit], N'1' AS [ShowApprovalAccounting], N'0' AS [EditOnly], N'0' AS [Revisit] UNION ALL 
SELECT N'214' AS [WorkDescriptionID], N'Z-LWC/EW4 - REPAIR/INSTALL READING DEVICE [REM]' AS [Description], N'4' AS [AssetTypeID], N'1.50' AS [TimeToComplete], N'26' AS [WorkCategoryID], N'1' AS [AccountingTypeID], N'1' AS [FirstRestorationAccountingCodeID], N'100' AS [FirstRestorationCostBreakdown], N'1' AS [FirstRestorationProductCodeID], NULL AS [SecondRestorationAccountingCodeID], NULL AS [SecondRestorationCostBreakdown], N'1' AS [SecondRestorationProductCodeID], N'0' AS [ShowBusinessUnit], N'1' AS [ShowApprovalAccounting], N'0' AS [EditOnly], N'0' AS [Revisit] UNION ALL 
SELECT N'215' AS [WorkDescriptionID], N'Z-LWC/EW4 - REREAD AND/OR INSPECT FOR LEAK [HILOW]' AS [Description], N'4' AS [AssetTypeID], N'1.50' AS [TimeToComplete], N'26' AS [WorkCategoryID], N'1' AS [AccountingTypeID], N'1' AS [FirstRestorationAccountingCodeID], N'100' AS [FirstRestorationCostBreakdown], N'1' AS [FirstRestorationProductCodeID], NULL AS [SecondRestorationAccountingCodeID], NULL AS [SecondRestorationCostBreakdown], N'1' AS [SecondRestorationProductCodeID], N'0' AS [ShowBusinessUnit], N'1' AS [ShowApprovalAccounting], N'0' AS [EditOnly], N'0' AS [Revisit] UNION ALL 
SELECT N'216' AS [WorkDescriptionID], N'Z-LWC/EW4 - SET METER/TURN ON & READ [ONSET]' AS [Description], N'4' AS [AssetTypeID], N'1.50' AS [TimeToComplete], N'26' AS [WorkCategoryID], N'1' AS [AccountingTypeID], N'1' AS [FirstRestorationAccountingCodeID], N'100' AS [FirstRestorationCostBreakdown], N'1' AS [FirstRestorationProductCodeID], NULL AS [SecondRestorationAccountingCodeID], NULL AS [SecondRestorationCostBreakdown], N'1' AS [SecondRestorationProductCodeID], N'0' AS [ShowBusinessUnit], N'1' AS [ShowApprovalAccounting], N'0' AS [EditOnly], N'0' AS [Revisit] UNION ALL 
SELECT N'217' AS [WorkDescriptionID], N'Z-LWC/EW4 - TURN ON WATER [ON]' AS [Description], N'4' AS [AssetTypeID], N'1.50' AS [TimeToComplete], N'26' AS [WorkCategoryID], N'1' AS [AccountingTypeID], N'1' AS [FirstRestorationAccountingCodeID], N'100' AS [FirstRestorationCostBreakdown], N'1' AS [FirstRestorationProductCodeID], NULL AS [SecondRestorationAccountingCodeID], NULL AS [SecondRestorationCostBreakdown], N'1' AS [SecondRestorationProductCodeID], N'0' AS [ShowBusinessUnit], N'1' AS [ShowApprovalAccounting], N'0' AS [EditOnly], N'0' AS [Revisit] UNION ALL
SELECT N'203' AS [WorkDescriptionID], N'Pump Repair' AS [Description], N'9' AS [AssetTypeID], N'2.00' AS [TimeToComplete], N'55' AS [WorkCategoryID], N'2' AS [AccountingTypeID], N'1' AS [FirstRestorationAccountingCodeID], N'100' AS [FirstRestorationCostBreakdown], N'1' AS [FirstRestorationProductCodeID], NULL AS [SecondRestorationAccountingCodeID], NULL AS [SecondRestorationCostBreakdown], NULL AS [SecondRestorationProductCodeID], N'0' AS [ShowBusinessUnit], N'0' AS [ShowApprovalAccounting], N'0' AS [EditOnly], N'0' AS [Revisit] UNION ALL
SELECT N'204' AS [WorkDescriptionID], N'Line Stop Repair' AS [Description], N'9' AS [AssetTypeID], N'2.00' AS [TimeToComplete], N'55' AS [WorkCategoryID], N'2' AS [AccountingTypeID], N'1' AS [FirstRestorationAccountingCodeID], N'100' AS [FirstRestorationCostBreakdown], N'1' AS [FirstRestorationProductCodeID], NULL AS [SecondRestorationAccountingCodeID], NULL AS [SecondRestorationCostBreakdown], NULL AS [SecondRestorationProductCodeID], N'0' AS [ShowBusinessUnit], N'0' AS [ShowApprovalAccounting], N'0' AS [EditOnly], N'0' AS [Revisit] UNION ALL
SELECT N'205' AS [WorkDescriptionID], N'Saw Repair' AS [Description], N'9' AS [AssetTypeID], N'2.00' AS [TimeToComplete], N'55' AS [WorkCategoryID], N'2' AS [AccountingTypeID], N'1' AS [FirstRestorationAccountingCodeID], N'100' AS [FirstRestorationCostBreakdown], N'1' AS [FirstRestorationProductCodeID], NULL AS [SecondRestorationAccountingCodeID], NULL AS [SecondRestorationCostBreakdown], NULL AS [SecondRestorationProductCodeID], N'0' AS [ShowBusinessUnit], N'0' AS [ShowApprovalAccounting], N'0' AS [EditOnly], N'0' AS [Revisit] UNION ALL
SELECT N'206' AS [WorkDescriptionID], N'Vehicle Repair' AS [Description], N'9' AS [AssetTypeID], N'2.00' AS [TimeToComplete], N'55' AS [WorkCategoryID], N'2' AS [AccountingTypeID], N'1' AS [FirstRestorationAccountingCodeID], N'100' AS [FirstRestorationCostBreakdown], N'1' AS [FirstRestorationProductCodeID], NULL AS [SecondRestorationAccountingCodeID], NULL AS [SecondRestorationCostBreakdown], NULL AS [SecondRestorationProductCodeID], N'0' AS [ShowBusinessUnit], N'0' AS [ShowApprovalAccounting], N'0' AS [EditOnly], N'0' AS [Revisit] UNION ALL
SELECT N'207' AS [WorkDescriptionID], N'Misc repair' AS [Description], N'9' AS [AssetTypeID], N'2.00' AS [TimeToComplete], N'55' AS [WorkCategoryID], N'2' AS [AccountingTypeID], N'1' AS [FirstRestorationAccountingCodeID], N'100' AS [FirstRestorationCostBreakdown], N'1' AS [FirstRestorationProductCodeID], NULL AS [SecondRestorationAccountingCodeID], NULL AS [SecondRestorationCostBreakdown], NULL AS [SecondRestorationProductCodeID], N'0' AS [ShowBusinessUnit], N'0' AS [ShowApprovalAccounting], N'0' AS [EditOnly], N'0' AS [Revisit] 

SET IDENTITY_INSERT WorkDescriptions OFF

--SET IDENTITY_INSERT [dbo].[WorkDescriptions] ON;
--BEGIN TRANSACTION;
--INSERT INTO [dbo].[WorkDescriptions]([WorkDescriptionID], [WorkCategoryID], [Description], [AssetTypeID], [TimeToComplete], [AccountingTypeID], [FirstRestorationAccountingCodeID], [FirstRestorationCostBreakdown], [FirstRestorationProductCodeID], [SecondRestorationAccountingCodeID], [SecondRestorationCostBreakdown], [SecondRestorationProductCodeID], [ShowBusinessUnit], [ShowApprovalAccounting], [EditOnly])
--SELECT 2, 23, N'WATER MAIN BLEEDERS', 3, 0.50, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 3, 26, N'CHANGE BURST METER', 4, 0.75, 1, 1, 56, 1, 26, 44, 1, 0, 1, 1 UNION ALL
--SELECT 4, 9, N'CHECK NO WATER', 4, 0.75, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 1 UNION ALL
--SELECT 5, 1, N'CURB BOX REPAIR', 4, 1.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 9, 1, N'BALL/CURB STOP REPAIR', 4, 1.25, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 14, 1, N'EXCAVATE METER BOX/SETTER', 4, 1.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 18, 2, N'SERVICE LINE FLOW TEST', 4, 1.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 19, 4, N'HYDRANT FROZEN', 2, 0.75, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 20, 1, N'FROZEN METER SET', 4, 0.75, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 21, 1, N'FROZEN SERVICE LINE COMPANY SIDE', 4, 2.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 22, 9, N'FROZEN SERVICE LINE CUST. SIDE', 4, 0.50, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 23, 9, N'GROUND WATER-SERVICE', 4, 0.75, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 24, 27, N'HYDRANT FLUSHING', 2, 1.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 25, 9, N'HYDRANT INVESTIGATION', 2, 1.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 26, 3, N'HYDRANT INSTALLATION', 2, 3.50, 1, 1, 100, 1, NULL, NULL, NULL, 0, 1, 0 UNION ALL
--SELECT 27, 4, N'HYDRANT LEAKING', 2, 0.75, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 28, 4, N'HYDRANT NO DRIP', 2, 1.50, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 29, 4, N'HYDRANT REPAIR', 2, 1.50, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 30, 5, N'HYDRANT REPLACEMENT', 2, 3.00, 1, 1, 50, 1, 26, 50, 1, 0, 1, 0 UNION ALL
--SELECT 31, 6, N'HYDRANT RETIREMENT', 2, 3.00, 3, 26, 100, 1, NULL, NULL, NULL, 0, 1, 0 UNION ALL
--SELECT 32, 9, N'INACTIVE ACCOUNT', 4, 0.50, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 1 UNION ALL
----SELECT 33, 1, N'METER BOX/SETTER INSTALLATION', 4, 1.25, 1, 1, 100, 1, NULL, NULL, NULL, 0, 0, 0 UNION ALL
--SELECT 34, 16, N'VALVE BLOW OFF INSTALLATION', 1, 3.50, 1, 1, 100, 1, NULL, NULL, NULL, 0, 1, 0 UNION ALL
--SELECT 35, 13, N'FIRE SERVICE INSTALLATION', 4, 3.50, 1, 1, 100, 1, NULL, NULL, NULL, 0, 1, 0 UNION ALL
--SELECT 36, 23, N'INSTALL LINE STOPPER', 3, 5.50, 1, 1, 100, 1, NULL, NULL, NULL, 0, 1, 0 UNION ALL
--SELECT 37, 25, N'INSTALL METER', 4, 1.25, 1, 1, 100, 1, NULL, NULL, NULL, 0, 1, 1 UNION ALL
--SELECT 38, 8, N'INTERIOR SETTING REPAIR', 4, 1.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
----SELECT 39, 8, N'INTERIOR SETTING REPAIR', 4, 0.50, 2, 1, 100, 1, NULL, NULL, NULL, 0, 0, 0 UNION ALL
--SELECT 40, 9, N'SERVICE INVESTIGATION', 4, 1.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 41, 9, N'MAIN INVESTIGATION', 3, 1.25, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 42, 1, N'LEAK IN METER BOX, INLET', 4, 1.50, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 43, 1, N'LEAK IN METER BOX, OUTLET', 4, 1.75, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 44, 10, N'LEAK SURVEY', 4, 1.50, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 47, 28, N'METER BOX/SETTER INSTALLATION', 4, 2.00, 1, 1, 100, 1, NULL, NULL, NULL, 0, 1, 0 UNION ALL
----SELECT 48, 28, N'MAKE METER SET AT CURB/EST', 4, 2.00, 1, 1, 100, 1, NULL, NULL, NULL, 0, 0, 0 UNION ALL
--SELECT 49, 26, N'METER CHANGE', 4, 1.50, 1, 1, 56, 1, 26, 44, 1, 0, 1, 1 UNION ALL
--SELECT 50, 1, N'METER BOX ADJUSTMENT/RESETTER', 4, 0.75, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 54, 20, N'NEW MAIN FLUSHING', 3, 1.25, 1, 1, 100, 1, NULL, NULL, NULL, 0, 1, 0 UNION ALL
--SELECT 56, 13, N'SERVICE LINE INSTALLATION', 4, 3.25, 1, 1, 100, 1, NULL, NULL, NULL, 0, 1, 0 UNION ALL
----SELECT 57, 12, N'SERVICE LINE REPAIR', 4, 2.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 58, 9, N'SERVICE LINE LEAK, CUST. SIDE', 4, 1.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 59, 14, N'SERVICE LINE RENEWAL', 4, 2.75, 1, 1, 60, 1, 26, 40, 1, 0, 1, 0 UNION ALL
--SELECT 60, 15, N'SERVICE LINE RETIRE', 4, 2.50, 3, 26, 100, 1, NULL, NULL, NULL, 0, 1, 0 UNION ALL
--SELECT 61, 9, N'SUMP PUMP', 4, 0.75, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 62, 9, N'TEST SHUT DOWN', 3, 1.75, 1, 1, 100, 1, NULL, NULL, NULL, 0, 1, 0 UNION ALL
--SELECT 64, 17, N'VALVE BOX REPAIR', 1, 1.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 65, 17, N'VALVE BOX BLOW OFF REPAIR', 1, 1.25, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 66, 17, N'SERVICE LINE/VALVE BOX REPAIR', 4, 1.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 67, 9, N'VALVE INVESTIGATION', 1, 1.50, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 68, 17, N'VALVE LEAKING', 1, 2.25, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0
--COMMIT;
--RAISERROR (N'[dbo].[WorkDescriptions]: Insert Batch: 1.....Done!', 10, 1) WITH NOWAIT;
--GO

--BEGIN TRANSACTION;
--INSERT INTO [dbo].[WorkDescriptions]([WorkDescriptionID], [WorkCategoryID], [Description], [AssetTypeID], [TimeToComplete], [AccountingTypeID], [FirstRestorationAccountingCodeID], [FirstRestorationCostBreakdown], [FirstRestorationProductCodeID], [SecondRestorationAccountingCodeID], [SecondRestorationCostBreakdown], [SecondRestorationProductCodeID], [ShowBusinessUnit], [ShowApprovalAccounting], [EditOnly])
--SELECT 69, 17, N'VALVE REPAIR', 1, 2.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 70, 17, N'VALVE BLOW OFF REPAIR', 1, 2.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 71, 18, N'VALVE REPLACEMENT', 1, 3.50, 1, 1, 57, 1, 26, 43, 1, 0, 1, 0 UNION ALL
--SELECT 72, 19, N'VALVE RETIREMENT', 1, 3.50, 3, 26, 100, 1, NULL, NULL, NULL, 0, 1, 0 UNION ALL
--SELECT 73, 9, N'WATER BAN/RESTRICTION VIOLATOR', 4, 0.50, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 74, 22, N'WATER MAIN BREAK REPAIR', 3, 3.50, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 75, 20, N'WATER MAIN INSTALLATION', 3, 4.25, 1, 1, 100, 1, NULL, NULL, NULL, 0, 1, 0 UNION ALL
--SELECT 76, 24, N'WATER MAIN RETIREMENT', 3, 3.25, 3, 26, 100, 1, NULL, NULL, NULL, 0, 1, 0 UNION ALL
--SELECT 78, 9, N'FLUSHING-SERVICE', 4, 1.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 80, 21, N'WATER MAIN BREAK REPLACE', 3, 3.50, 1, 1, 49, 1, 26, 51, 1, 0, 1, 0 UNION ALL
--SELECT 81, 28, N'METER BOX/SETTER REPLACE', 4, 2.00, 1, 1, 60, 1, 26, 40, 1, 0, 1, 0 UNION ALL
--SELECT 82, 29, N'SEWER MAIN BREAK-REPAIR', 7, 6.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 83, 30, N'SEWER MAIN BREAK-REPLACE', 7, 6.00, 1, 1, 49, 1, 26, 51, 1, 0, 1, 0 UNION ALL
--SELECT 84, 31, N'SEWER MAIN RETIREMENT', 7, 4.00, 3, 26, 100, 1, NULL, NULL, NULL, 0, 1, 0 UNION ALL
--SELECT 85, 32, N'SEWER MAIN INSTALLATION', 7, 8.00, 1, 1, 100, 1, NULL, NULL, NULL, 0, 1, 0 UNION ALL
--SELECT 86, 33, N'SEWER MAIN CLEANING', 7, 8.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 87, 34, N'SEWER LATERAL-INSTALLATION', 6, 6.00, 1, 1, 100, 1, NULL, NULL, NULL, 0, 1, 0 UNION ALL
--SELECT 88, 35, N'SEWER LATERAL-REPAIR', 6, 4.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 89, 36, N'SEWER LATERAL-REPLACE', 6, 4.00, 1, 1, 60, 1, 26, 40, 1, 0, 1, 0 UNION ALL
--SELECT 90, 37, N'SEWER LATERAL-RETIRE', 6, 4.00, 3, 26, 100, 1, NULL, NULL, NULL, 0, 1, 0 UNION ALL
--SELECT 91, 38, N'SEWER LATERAL-CUSTOMER SIDE', 6, 2.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 92, 39, N'SEWER MANHOLE REPAIR', 5, 4.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 93, 40, N'SEWER MANHOLE REPLACE', 5, 8.00, 1, 1, 60, 1, 26, 40, 1, 0, 1, 0 UNION ALL
--SELECT 94, 41, N'SEWER MANHOLE INSTALLATION', 5, 8.00, 1, 1, 100, 1, NULL, NULL, NULL, 0, 1, 0 UNION ALL
--SELECT 95, 42, N'SEWER MAIN OVERFLOW', 7, 2.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 96, 43, N'SEWER BACKUP-COMPANY SIDE', 6, 2.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 97, 44, N'SEWER BACKUP-CUSTOMER SIDE', 7, 2.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 98, 2, N'HYDRAULIC FLOW TEST', 2, 1.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 99, 23, N'MARKOUT-CREW', 3, 1.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 1 UNION ALL
--SELECT 100, 17, N'VALVE BOX REPLACEMENT', 1, 1.50, 1, 1, 57, 1, 26, 43, 1, 0, 1, 0 UNION ALL
--SELECT 101, 13, N'SITE INSPECTION/SURVEY NEW SERVICE', 4, 0.50, 1, 1, 100, 1, NULL, NULL, NULL, 0, 1, 0 UNION ALL
--SELECT 102, 14, N'SITE INSPECTION/SURVEY SERVICE RENEWAL', 4, 0.50, 1, 1, 60, 1, 26, 40, 1, 0, 1, 0 UNION ALL
--SELECT 103, 12, N'SERVICE LINE REPAIR', 4, 0.50, 1, 2, 100, 1, NULL, NULL, NULL, 0, 1, 0 UNION ALL
--SELECT 104, 45, N'SEWER CLEAN OUT INSTALLATION', 6, 3.00, 1, 1, 100, 1, NULL, NULL, NULL, 0, 1, 0 UNION ALL
--SELECT 105, 45, N'SEWER CLEAN OUT REPAIR', 6, 0.50, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 106, 9, N'SEWER CAMERA SERVICE', 6, 8.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 107, 9, N'SEWER CAMERA MAIN', 7, 8.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 108, 45, N'SEWER DEMOLITION INSPECTION', 6, 1.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 109, 46, N'SEWER MAIN TEST HOLES', 7, 8.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 110, 9, N'WATER MAIN TEST HOLES', 3, 8.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 111, 17, N'VALVE BROKEN', 1, 4.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 112, 9, N'GROUND WATER-MAIN', 3, 0.75, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
--SELECT 113, 47, N'SERVICE-TURN ON', 4, 0.50, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 1 UNION ALL
--SELECT 114, 47, N'SERVICE-TURN OFF', 4, 0.50, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 1 UNION ALL
--SELECT 115, 47, N'METER-OBTAIN READ', 4, 0.50, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 1 UNION ALL
--SELECT 116, 47, N'METER-FINAL/START READ', 4, 0.50, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 1 UNION ALL
--SELECT 117, 47, N'METER-REPAIR TOUCH PAD', 4, 0.50, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 1 UNION ALL
--SELECT 118, 16, N'VALVE INSTALLATION', 1, 3.50, 1, 1, 100, 1, NULL, NULL, NULL, 0, 1, 0 UNION ALL
--SELECT 119, 18, N'VALVE BLOW OFF REPLACEMENT', 1, 3.50, 1, 1, 57, 1, 26, 43, 1, 0, 1, 0 UNION ALL
--SELECT 120, 4, N'HYDRANT PAINT', 2, 0.50, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0
--COMMIT;
--RAISERROR (N'[dbo].[WorkDescriptions]: Insert Batch: 2.....Done!', 10, 1) WITH NOWAIT;
--GO

--BEGIN TRANSACTION;
--INSERT INTO [dbo].[WorkDescriptions]([WorkDescriptionID], [WorkCategoryID], [Description], [AssetTypeID], [TimeToComplete], [AccountingTypeID], [FirstRestorationAccountingCodeID], [FirstRestorationCostBreakdown], [FirstRestorationProductCodeID], [SecondRestorationAccountingCodeID], [SecondRestorationCostBreakdown], [SecondRestorationProductCodeID], [ShowBusinessUnit], [ShowApprovalAccounting], [EditOnly], [Revisit])
--SELECT 121, 28, N'BALL/CURB STOP REPLACE', 4, 3.00, 1, 1, 60, 1, 26, 40, 1, 0, 1, 0, 0 UNION ALL
--SELECT 122, 17, N'VALVE BLOW OFF RETIREMENT', 1, 2.00, 3, 26, 100, 1, NULL, NULL, NULL, 0, 1, 0, 0 UNION ALL
--SELECT 123, 17, N'VALVE BLOW OFF BROKEN', 1, 2.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0, 0 UNION ALL
--SELECT 124, 48, N'WATER MAIN RELOCATION', 3, 8.00, 1, 1, 49, 1, 26, 51, 1, 0, 1, 0, 0 UNION ALL
--SELECT 125, 48, N'HYDRANT RELOCATION', 2, 8.00, 1, 1, 100, 1, NULL, NULL, NULL, 1, 0, 0, 0 UNION ALL
--SELECT 126, 48, N'SERVICE RELOCATION', 4, 4.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0, 0 UNION ALL
--SELECT 127, 46, N'SEWER INVESTIGATION-MAIN', 7, 2.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0, 0 UNION ALL
--SELECT 128, 42, N'SEWER SERVICE OVERFLOW', 6, 1.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0, 0 UNION ALL
--SELECT 129, 34, N'SEWER INVESTIGATION-LATERAL', 6, 1.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0, 0 UNION ALL
--SELECT 130, 46, N'SEWER INVESTIGATION-MANHOLE', 5, 1.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0, 0 UNION ALL
--SELECT 131, 50, N'SEWER LIFT STATION REPAIR', 7, 4.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0, 0 UNION ALL
--SELECT 132, 28, N'CURB BOX REPLACE', 4, 1.00, 1, 1, 60, 1, 26, 40, 1, 1, 0, 0, 0 UNION ALL
--SELECT 133, 28, N'SERVICE LINE/VALVE BOX REPLACE', 4, 1.00, 1, 1, 60, 1, 26, 40, 1, 1, 0, 0, 0 UNION ALL
--SELECT 134, 52, N'STORM/CATCH REPAIR', 8, 1.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0, 0 UNION ALL
--SELECT 135, 53, N'STORM/CATCH REPLACE', 8, 1.00, 1, 1, 100, 1, NULL, NULL, NULL, 0, 1, 0, 0 UNION ALL
--SELECT 136, 51, N'STORM/CATCH INSTALLATION', 8, 1.00, 1, 1, 100, 1, NULL, NULL, NULL, 0, 1, 0, 0 UNION ALL
--SELECT 137, 9, N'STORM/CATCH INVESTIGATION', 8, 1.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0, 0 UNION ALL
--SELECT 138, 49, N'HYDRANT LANDSCAPING', 2, 1.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0, 1 UNION ALL
--SELECT 139, 49, N'HYDRANT RESTORATION INVESTIGATION', 2, 0.50, 2, 3, 100, 1, NULL, NULL, NULL, 1, 0, 0, 1 UNION ALL
--SELECT 140, 49, N'HYDRANT RESTORATION REPAIR', 2, 1.00, 2, 4, 100, 1, NULL, NULL, NULL, 1, 0, 0, 1 UNION ALL
--SELECT 141, 49, N'MAIN LANDSCAPING', 3, 1.00, 2, 5, 100, 1, NULL, NULL, NULL, 1, 0, 0, 1 UNION ALL
--SELECT 142, 49, N'MAIN RESTORATION INVESTIGATION', 3, 0.50, 2, 6, 100, 1, NULL, NULL, NULL, 1, 0, 0, 1 UNION ALL
--SELECT 143, 49, N'MAIN RESTORATION REPAIR', 3, 1.00, 2, 7, 100, 1, NULL, NULL, NULL, 1, 0, 0, 1 UNION ALL
--SELECT 144, 49, N'SERVICE LANDSCAPING', 4, 0.75, 2, 8, 100, 1, NULL, NULL, NULL, 1, 0, 0, 1 UNION ALL
--SELECT 145, 49, N'SERVICE RESTORATION INVESTIGATION', 4, 0.50, 2, 9, 100, 1, NULL, NULL, NULL, 1, 0, 0, 1 UNION ALL
--SELECT 146, 49, N'SERVICE RESTORATION REPAIR', 4, 0.75, 2, 10, 100, 1, NULL, NULL, NULL, 1, 0, 0, 1 UNION ALL
--SELECT 147, 49, N'SEWER LATERAL LANDSCAPING', 6, 0.50, 2, 11, 100, 1, NULL, NULL, NULL, 1, 0, 0, 1 UNION ALL
--SELECT 148, 49, N'SEWER LATERAL RESTORATION INVESTIGATION', 6, 0.50, 2, 12, 100, 1, NULL, NULL, NULL, 1, 0, 0, 1 UNION ALL
--SELECT 149, 49, N'SEWER LATERAL RESTORATION REPAIR', 6, 1.00, 2, 13, 100, 1, NULL, NULL, NULL, 1, 0, 0, 1 UNION ALL
--SELECT 150, 49, N'SEWER MAIN LANDSCAPING', 7, 1.00, 2, 14, 100, 1, NULL, NULL, NULL, 1, 0, 0, 1 UNION ALL
--SELECT 151, 49, N'SEWER MAIN RESTORATION INVESTIGATION', 7, 0.50, 2, 15, 100, 1, NULL, NULL, NULL, 1, 0, 0, 1 UNION ALL
--SELECT 152, 49, N'SEWER MAIN RESTORATION REPAIR', 7, 1.00, 2, 16, 100, 1, NULL, NULL, NULL, 1, 0, 0, 1 UNION ALL
--SELECT 153, 49, N'SEWER MANHOLE LANDSCAPING', 5, 1.00, 2, 17, 100, 1, NULL, NULL, NULL, 1, 0, 0, 1 UNION ALL
--SELECT 154, 49, N'SEWER MANHOLE RESTORATION INVESTIGATION', 5, 0.50, 2, 18, 100, 1, NULL, NULL, NULL, 1, 0, 0, 1 UNION ALL
--SELECT 155, 49, N'SEWER MANHOLE RESTORATION REPAIR', 5, 1.00, 2, 19, 100, 1, NULL, NULL, NULL, 1, 0, 0, 1 UNION ALL
----SELECT 156, 49, N'STORM/CATCH LANDSCAPING', 8, 1.00, 2, 20, 100, 1, NULL, NULL, NULL, 1, 0, 0, 1 UNION ALL
----SELECT 157, 49, N'STORM/CATCH RESTORATION INVESTIGATION', 8, 0.50, 2, 21, 100, 1, NULL, NULL, NULL, 1, 0, 0, 1 UNION ALL
----SELECT 158, 49, N'STORM/CATCH RESTORATION REPAIR', 8, 1.00, 2, 22, 100, 1, NULL, NULL, NULL, 1, 0, 0, 1 UNION ALL
--SELECT 159, 49, N'VALVE LANDSCAPING', 1, 0.50, 2, 23, 100, 1, NULL, NULL, NULL, 1, 0, 0, 1 UNION ALL
--SELECT 160, 49, N'VALVE RESTORATION INVESTIGATION', 1, 0.50, 2, 24, 100, 1, NULL, NULL, NULL, 1, 0, 0, 1 UNION ALL
--SELECT 161, 49, N'VALVE RESTORATION REPAIR', 1, 0.50, 2, 25, 100, 1, NULL, NULL, NULL, 1, 0, 0, 1
--COMMIT;
--RAISERROR (N'[dbo].[WorkDescriptions]: Insert Batch: 3.....Done!', 10, 1) WITH NOWAIT;
--GO

--SET IDENTITY_INSERT [dbo].[WorkDescriptions] OFF;

--SET IDENTITY_INSERT [dbo].[WorkDescriptions] ON;

--INSERT INTO [dbo].[WorkDescriptions]([WorkDescriptionID], [Description], [AssetTypeID], [WorkCategoryID], [TimeToComplete], [AccountingTypeID], [FirstRestorationAccountingCodeID], [FirstRestorationCostBreakdown], [FirstRestorationProductCodeID], [ShowBusinessUnit], [ShowApprovalAccounting], [EditOnly],[Revisit])
--SELECT 162 as [WorkDescriptionID], 'STORM/CATCH LANDSCAPING', 8, 49, 1, 2, 20, 100, 1, 1, 0, 0, 1 UNION ALL
--SELECT 163, 'STORM/CATCH RESTORATION INVESTIGATION', 8, 49, 0.5, 2, 21, 100, 1, 1, 0, 0, 1 UNION ALL
--SELECT 164, 'STORM/CATCH RESTORATION REPAIR', 8, 49, 1, 2, 22, 100, 1, 1, 0, 0, 1 UNION ALL

--SELECT 165, 'RSTRN-RESTORATION INQUIRY', 3, 49, 0.5, 2, 6, 100, 1, 1, 0, 0, 1 UNION ALL
--SELECT 166, 'RSTRN-RESTORATION INQUIRY', 4, 49, 0.5, 2, 9, 100, 1, 1, 0, 0, 1 UNION ALL
--SELECT 167, 'RSTRN-RESTORATION INQUIRY', 6, 49, 0.5, 2, 13, 100, 1, 1, 0, 0, 1 UNION ALL
--SELECT 168, 'RSTRN-RESTORATION INQUIRY', 7, 49, 0.5, 2, 16, 100, 1, 1, 0, 0, 1 UNION ALL

----SELECT 169, 'RSTRN-RESTORATION INQUIRY', 3, 49, 0.5, 2, 6, 100, 1, 1, 0, 0, 0 UNION ALL
----SELECT 170, 'RSTRN-RESTORATION INQUIRY', 4, 49, 0.5, 2, 9, 100, 1, 1, 0, 0, 0 UNION ALL
----SELECT 171, 'RSTRN-RESTORATION INQUIRY', 6, 49, 0.5, 2, 13, 100, 1, 1, 0, 0, 0 UNION ALL
----SELECT 172, 'RSTRN-RESTORATION INQUIRY', 7, 49, 0.5, 2, 16, 100, 1, 1, 0, 0, 0

--SELECT 169, 'SERVICE OFF AT MAIN-STORM RESTORATION',		4, 4, 2,   2,   1, 100, 1,   1, 1, 0, 0 UNION ALL
--SELECT 170, 'SERVICE OFF AT CURB STOP-STORM RESTORATION',	4, 4, 0.5, 2,   1, 100, 1,   1, 1, 0, 0 UNION ALL
--SELECT 171, 'SERVICE OFF AT METER PIT-STORM RESTORATION',	4, 4, 0.5, 2,   1, 100, 1,   1, 1, 0, 0 UNION ALL
--SELECT 172, 'VALVE TURNED OFF STORM RESTORATION',			1, 2, 0.5, 2,   1, 100, 1,   1, 1, 0, 0 UNION ALL
--SELECT 173, 'MAIN REPAIRED-STORM RESTORATION',				3, 3, 4,   2,   1, 100, 1,   1, 1, 0, 0 UNION ALL
--SELECT 174, 'MAIN REPLACED - STORM RESTORATION',			3, 3, 8,   1,   1, 100, 1,   1, 1, 0, 0 UNION ALL
--SELECT 175, 'HYDRANT TURNED OFF - STORM RESTORATION',		2, 4, 0.5, 2,   1, 100, 1,   1, 1, 0, 0 UNION ALL
--SELECT 176, 'HYDRANT REPLACED - STORM RESTORATION',			2, 5, 4,   1,   1, 100, 1,   1, 1, 0, 0 UNION ALL
--SELECT 177, 'VALVE INSTALLATION - STORM RESTORATION',		1, 2, 4,   1,   1, 100, 1,   1, 1, 0, 0 UNION ALL
--SELECT 178, 'VALVE REPLACEMENT - STORM RESTORATION',		1, 2, 4,   1,   1, 100, 1,   1, 1, 0, 0 UNION ALL
--SELECT 179, 'CURB BOX LOCATE - STORM RESTORATION',			4, 4, 0.5, 2,   1, 100, 1,   1, 1, 0, 0 UNION ALL
--SELECT 190, 'METER PIT LOCATE - STORM RESTORATION',			4, 4, 0.5, 2,   1, 100, 1,   1, 1, 0, 0 UNION ALL
--SELECT 191, 'VALVE RETIREMENT - STORM RESTORATION',			1, 17, 2,  3,	26, 100, 1,	 1, 1, 0, 0 UNION ALL
--SELECT 192, 'EXCAVATE METER PIT- STORM RESTORATION',		4, 1, 0.5, 2,	1,  100, 1,  1, 1, 0, 0 UNION ALL
--SELECT 193, 'SERVICE LINE RENEWAL - STORM RESTORATION', 	4, 1, 4, 1,		1,  100, 1,  1, 1, 0, 0 UNION ALL
--SELECT 194, 'CURB BOX REPLACEMENT - STORM RESTORATION', 	4, 28, 1, 1,		1,  100, 1,  1, 1, 0, 0 UNION ALL
--SELECT 195, 'WATER MAIN RETIREMENT - STORM RESTORATION', 	4, 3, 4, 1,		1,  100, 1,  1, 1, 0, 0 UNION ALL
--SELECT 196, 'SERVICE LINE RETIREMENT - STORM RESTORATION', 	4, 4, 2, 1,		1,  100, 1,  1, 1, 0, 0 UNION ALL
--SELECT 197, 'SERVICE LINE RETIREMENT - STORM RESTORATION', 	4, 4, 0.5, 2,		1,  100, 1,  1, 1, 0, 0 
--SET IDENTITY_INSERT [dbo].[WorkDescriptions] OFF;


--MAINBREAKMATERIALS--
SET IDENTITY_INSERT [dbo].[MainBreakMaterials] ON;
GO
INSERT INTO [dbo].[MainBreakMaterials]([MainBreakMaterialID], [Description])
SELECT 1, N'Cast Iron' UNION ALL
SELECT 2, N'Cement' UNION ALL
SELECT 3, N'Galvanized' UNION ALL
SELECT 4, N'DICL' UNION ALL
SELECT 5, N'PVC' UNION ALL
SELECT 6, N'HDPE' UNION ALL
SELECT 7, N'Lockjoint PSCP' UNION ALL
SELECT 8, N'Asbestos Cement'
RAISERROR (N'[dbo].[MainBreakMaterials]: Insert Batch: 1.....Done!', 10, 1) WITH NOWAIT;
GO
SET IDENTITY_INSERT [dbo].[MainBreakMaterials] OFF;

--MAINBREAKSOILCONDITIONS--
SET IDENTITY_INSERT [dbo].[MainBreakSoilConditions] ON;
GO
INSERT INTO [dbo].[MainBreakSoilConditions]([MainBreakSoilConditionID], [Description])
SELECT 1, N'Gravel' UNION ALL
SELECT 2, N'Sandy' UNION ALL
SELECT 3, N'Rocky' UNION ALL
SELECT 4, N'Clay' UNION ALL
SELECT 5, N'Loam' UNION ALL
SELECT 6, N'Silty'
RAISERROR (N'[dbo].[MainBreakSoilConditions]: Insert Batch: 1.....Done!', 10, 1) WITH NOWAIT;
GO
SET IDENTITY_INSERT [dbo].[MainBreakSoilConditions] OFF;

--MAINCONDITIONS--
SET IDENTITY_INSERT [dbo].[MainConditions] ON;
GO
INSERT INTO [dbo].[MainConditions]([MainConditionID], [Description])
SELECT 1, N'Good' UNION ALL
SELECT 2, N'Fair' UNION ALL
SELECT 3, N'Poor'
RAISERROR (N'[dbo].[MainConditions]: Insert Batch: 1.....Done!', 10, 1) WITH NOWAIT;
GO
SET IDENTITY_INSERT [dbo].[MainConditions] OFF;

--MAINFAILURETYPES--
SET IDENTITY_INSERT [dbo].[MainFailureTypes] ON;
GO
INSERT INTO [dbo].[MainFailureTypes]([MainFailureTypeID], [Description])
SELECT 1, N'Split' UNION ALL
SELECT 2, N'Circular' UNION ALL
SELECT 3, N'Deterioration' UNION ALL
SELECT 4, N'Physical Damage' UNION ALL
SELECT 5, N'Joint Leak' UNION ALL
SELECT 6, N'Pinhole' UNION ALL
SELECT 7, N'Stress'
RAISERROR (N'[dbo].[MainFailureTypes]: Insert Batch: 1.....Done!', 10, 1) WITH NOWAIT;
GO
SET IDENTITY_INSERT [dbo].[MainFailureTypes] OFF;

--MAINBREAKDISINFECTIONMETHODS--
SET IDENTITY_INSERT [dbo].[MainBreakDisinfectionMethods] ON;
GO
INSERT INTO [dbo].[MainBreakDisinfectionMethods]([MainBreakDisinfectionMethodID], [Description])
SELECT 1, N'Disinfection of Fittings\Pipe' UNION ALL
SELECT 2, N'Chlorination'
RAISERROR (N'[dbo].[MainBreakDisinfectionMethods]: Insert Batch: 1.....Done!', 10, 1) WITH NOWAIT;
GO
SET IDENTITY_INSERT [dbo].[MainBreakDisinfectionMethods] OFF;

--MAINBREAKFLUSHMETHODS--
SET IDENTITY_INSERT [dbo].[MainBreakFlushMethods] ON;
GO
INSERT INTO [dbo].[MainBreakFlushMethods]([MainBreakFlushMethodID], [Description])
SELECT 1, N'Hydrant' UNION ALL
SELECT 2, N'Blowoff' UNION ALL
SELECT 3, N'Service' UNION ALL
SELECT 4, N'Main' 
RAISERROR (N'[dbo].[MainBreakFlushMethods]: Insert Batch: 1.....Done!', 10, 1) WITH NOWAIT;
GO
SET IDENTITY_INSERT [dbo].[MainBreakFlushMethods] OFF;

------------------------------------------RESTORATION TYPES------------------------------------------
INSERT INTO RestorationTypes ([Description]) VALUES ('ASPHALT-STREET');
INSERT INTO RestorationTypes ([Description]) VALUES ('ASPHALT-DRIVEWAY');
INSERT INTO RestorationTypes ([Description]) VALUES ('CONCRETE STREET');
INSERT INTO RestorationTypes ([Description]) VALUES ('CURB RESTORATION');
INSERT INTO RestorationTypes ([Description]) VALUES ('CURB/GUTTER RESTORATION');
INSERT INTO RestorationTypes ([Description]) VALUES ('DRIVEWAY APRON RESTORATION');
INSERT INTO RestorationTypes ([Description]) VALUES ('GROUND RESTORATION');
INSERT INTO RestorationTypes ([Description]) VALUES ('SIDEWALK RESTORATION');

-----------------------------------------RESTORATION METHODS-----------------------------------------
-- Final
INSERT INTO [RestorationMethods] ([Description]) VALUES ('Infra Red Restoration');
INSERT INTO [RestorationMethods] ([Description]) VALUES ('Mill and Pave 2"');
INSERT INTO [RestorationMethods] ([Description]) VALUES ('2" Top Overlay');
INSERT INTO [RestorationMethods] ([Description]) VALUES ('6" Stab. Base and 2" FABC');
INSERT INTO [RestorationMethods] ([Description]) VALUES ('Concrete');
INSERT INTO [RestorationMethods] ([Description]) VALUES ('Top Soil and Seed');
INSERT INTO [RestorationMethods] ([Description]) VALUES ('Sod');
-- Initial
INSERT INTO [RestorationMethods] ([Description]) VALUES ('Cold Patch');
INSERT INTO [RestorationMethods] ([Description]) VALUES ('Stone');
INSERT INTO [RestorationMethods] ([Description]) VALUES ('Dirt');
INSERT INTO [RestorationMethods] ([Description]) VALUES ('4" Stab Base');
INSERT INTO [RestorationMethods] ([Description]) VALUES ('6" Stab Base');
INSERT INTO [RestorationMethods] ([Description]) VALUES ('8" Stab Base');
INSERT INTO [RestorationMethods] ([Description]) VALUES ('10" Stab Base');

--------------------------------RESTORATION METHODS/RESTORATION TYPES--------------------------------
-- ASPHALT-STREET
---- Infra Red Restoration
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (1, 1, 0, 1);
---- Mill and Pave 2"
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (1, 2, 0, 1);
---- 2" Top Overlay
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (1, 3, 1, 1);
---- 6" Stab. Base and 2" FABC
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (1, 4, 0, 1);
---- 4" Stab Base
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (1, 11, 1, 0);
---- 6" Stab Base
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (1, 12, 1, 0);
---- 8" Stab Base
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (1, 13, 1, 0);
---- 10" Stab Base
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (1, 14, 1, 0);

-- ASPHALT-DRIVEWAY
---- Infra Red Restoration
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (2, 1, 0, 1);
---- Mill and Pave 2"
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (2, 2, 0, 1);
---- 2" Top Overlay
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (2, 3, 0, 1);
---- 6" Stab. Base and 2" FABC
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (2, 4, 0, 1);
---- 4" Stab Base
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (2, 11, 1, 0);

-- CONCRETE STREET
---- 2" Top Overlay
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (3, 3, 1, 0);
---- 6" Stab. Base and 2" FABC
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (3, 4, 0, 1);
---- Concrete
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (3, 5, 0, 1);
---- 4" Stab Base
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (3, 11, 1, 0);
---- 6" Stab Base
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (3, 12, 1, 0);
---- 8" Stab Base
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (3, 13, 1, 0);
---- 10" Stab Base
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (3, 14, 1, 0);

-- CURB RESTORATION
---- Mill and Pave 2"
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (4, 2, 0, 1);
---- 2" Top Overlay
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (4, 3, 0, 1);
---- Concrete
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (4, 5, 0, 1);
---- Stone
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (4, 8, 1, 0);
---- 4" Stab Base
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (4, 11, 1, 0);
---- 6" Stab Base
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (4, 12, 1, 0);

-- CURB/GUTTER RESTORATION
---- Mill and Pave 2"
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (5, 2, 0, 1);
---- 2" Top Overlay
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (5, 3, 0, 1);
---- Concrete
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (5, 5, 0, 1);
---- Top Soil and Seed
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (5, 6, 0, 1);
---- Sod
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (5, 7, 0, 1);
---- Cold Patch
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (5, 8, 1, 0);
---- Stone
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (5, 9, 1, 0);
---- 4" Stab Base
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (5, 11, 1, 0);
---- 6" Stab Base
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (5, 12, 1, 0);

-- DRIVEWAY APRON RESTORATION
---- Mill and Pave 2"
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (6, 2, 0, 1);
---- 2" Top Overlay
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (6, 3, 0, 1);
---- Concrete
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (6, 5, 0, 1);
---- Cold Patch
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (6, 8, 1, 0);
---- Stone
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (6, 9, 1, 0);
---- 4" Stab Base
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (6, 11, 1, 0);

-- GROUND RESTORATION
---- Top Soil and Seed
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (7, 6, 0, 1);
---- Sod
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (7, 7, 0, 1);
---- Dirt
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (7, 10, 1, 0);

-- SIDEWALK RESTORATION
---- 2" Top Overlay
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (8, 3, 0, 1);
---- Concrete
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (8, 5, 0, 1);
---- Cold Patch
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (8, 8, 1, 0);
---- Stone
INSERT INTO [RestorationMethodsRestorationTypes] ([RestorationTypeID], [RestorationMethodID], [InitialMethod], [FinalMethod])
VALUES (8, 9, 1, 0);

--------------------------------------------MARKOUT TYPES--------------------------------------------
INSERT INTO [MarkoutTypes] Values('C TO 10FT BHD C',1)
INSERT INTO [MarkoutTypes] Values('C TO 15FT BHD C',2)
INSERT INTO [MarkoutTypes] Values('C TO 20FT BHD C',3)
INSERT INTO [MarkoutTypes] Values('C TO 5FT BHD C',4)
INSERT INTO [MarkoutTypes] Values('C TO C',5)
INSERT INTO [MarkoutTypes] Values('C TO C, 25FT RADIUS OF HYDRANT',6)
INSERT INTO [MarkoutTypes] Values('C TO C, C TO 10FT BHD ALL, C''S M/O BEG AT C/L OF INT & EXT 75FT IN ALL DIR',7)
INSERT INTO [MarkoutTypes] Values('C TO C, C TO 10FT BHD BOTH C''S',8)
INSERT INTO [MarkoutTypes] Values('C TO C, C TO 10FT BHD OPP C',9)
INSERT INTO [MarkoutTypes] Values('C TO C, C TO 15FT BHD C',10)
INSERT INTO [MarkoutTypes] Values('C TO C, C TO 20FT BHD ALL, C''S M/O BEG AT C/L OF INT & EXT 50FT IN ALL DIR',11)
INSERT INTO [MarkoutTypes] Values('C TO C, C TO 20FT BHD C, C TO 20FT BHD OPP C',12)
INSERT INTO [MarkoutTypes] Values('C TO C, C TO 20FT BHD C.',13)
INSERT INTO [MarkoutTypes] Values('C TO C, C TO 20FT BHD OPP C',14)
INSERT INTO [MarkoutTypes] Values('C TO C, C TO 25FT BHD C',15)
INSERT INTO [MarkoutTypes] Values('C TO C, C TO 30FT BHD C OPP C',16)
INSERT INTO [MarkoutTypes] Values('C TO C, C TO 5FT BHD C',17)
INSERT INTO [MarkoutTypes] Values('C TO C, C TO 5FT BHD OPP C',18)
INSERT INTO [MarkoutTypes] Values('C TO C, C TO ENT PROP',19)
INSERT INTO [MarkoutTypes] Values('C TO C, C TO ENT PROP, C TO 10FT BHD OPP C',20)
INSERT INTO [MarkoutTypes] Values('C TO C, ENT PROP, C/L OF S C, C TO 10FT BHD C, C TO 10FT BHD OPP C',21)
INSERT INTO [MarkoutTypes] Values('C TO C. C TO 10FT BHD C',22)
INSERT INTO [MarkoutTypes] Values('C TO ENT PROP',23)
INSERT INTO [MarkoutTypes] Values('C TO ENT PROP, C TO C',24)
INSERT INTO [MarkoutTypes] Values('C TO ENT PROP, C TO C, C TO 10FT BHD OPP C',25)
INSERT INTO [MarkoutTypes] Values('M/O C/L OF INT AND EXT 100FT IN ALL DIR',26)
INSERT INTO [MarkoutTypes] Values('M/O C/L OF INT AND EXT 100FT IN ALL DIR, C TO C, C TO 10FT BHD ALL C''S',27)
INSERT INTO [MarkoutTypes] Values('M/O C/L OF INT AND EXT 100FT IN ALL DIR, C TO C, C TO 15FT BHD ALL C''S',28)
INSERT INTO [MarkoutTypes] Values('M/O C/L OF INT AND EXT 100FT IN ALL DIR, C TO C, C TO 20FT BHD ALL C''S',29)
INSERT INTO [MarkoutTypes] Values('M/O C/L OF INT AND EXT 50FT IN ALL DIR',30)
INSERT INTO [MarkoutTypes] Values('M/O C/L OF INT AND EXT 50FT IN ALL DIR, C TO C, C TO 10FT BHD ALL C''S',31)
INSERT INTO [MarkoutTypes] Values('M/O C/L OF INT AND EXT 50FT IN ALL DIR, C TO C, C TO 15FT BHD ALL C''S',32)
INSERT INTO [MarkoutTypes] Values('M/O C/L OF INT AND EXT 50FT IN ALL DIR, C TO C, C TO 20FT BHD ALL C''S',33)
INSERT INTO [MarkoutTypes] Values('M/O C/L OF INT AND EXT 75FT IN ALL DIR',34)
INSERT INTO [MarkoutTypes] Values('M/O C/L OF INT AND EXT 75FT IN ALL DIR, C TO C, C TO 10FT BHD ALL C''S',35)
INSERT INTO [MarkoutTypes] Values('M/O C/L OF INT AND EXT 75FT IN ALL DIR, C TO C, C TO 15FT BHD ALL C''S',36)
INSERT INTO [MarkoutTypes] Values('M/O C/L OF INT AND EXT 75FT IN ALL DIR, C TO C, C TO 20FT BHD ALL C''S',37)
INSERT INTO [MarkoutTypes] Values('NOT LISTED',38)

----------------------------------------MARKOUT REQUIREMENTS----------------------------------------
--INSERT INTO [MarkoutRequirements] ([Description]) VALUES ('None');
--INSERT INTO [MarkoutRequirements] ([Description]) VALUES ('Routine');
--INSERT INTO [MarkoutRequirements] ([Description]) VALUES ('Emergency');

--------------------------------------------MARKOUT STATI--------------------------------------------
INSERT INTO [MarkoutStatuses] ([Description]) VALUES ('Pending');
INSERT INTO [MarkoutStatuses] ([Description]) VALUES ('Ready');
INSERT INTO [MarkoutStatuses] ([Description]) VALUES ('Overdue');

-------------------------------------------WORK AREA TYPES-------------------------------------------
INSERT INTO [WorkAreaTypes] ([Description]) VALUES ('BUSINESS');
INSERT INTO [WorkAreaTypes] ([Description]) VALUES ('BUSINESS/RESIDENTIAL');
INSERT INTO [WorkAreaTypes] ([Description]) VALUES ('INDUSTRIAL');
INSERT INTO [WorkAreaTypes] ([Description]) VALUES ('RESIDENTIAL');

---------------------------------------LEAK REPORTING SOURCES----------------------------------------
INSERT INTO [LeakReportingSources] ([Description]) VALUES ('Field Service Rep.');
INSERT INTO [LeakReportingSources] ([Description]) VALUES ('MLOG');
INSERT INTO [LeakReportingSources] ([Description]) VALUES ('Survey');
INSERT INTO [LeakReportingSources] ([Description]) VALUES ('VILADE');

--------------------------------------WORK DESCRIPTION REMOVAL---------------------------------------
-- they no longer want the WorkDescriptions which will have been created at these indices.
-- they needed to be inserted though so that all the remaining ones will have the correct
-- indices.
--DELETE FROM [WorkDescriptions] WHERE [WorkDescriptionID] IN (1, 13, 45, 46, 55, 63, 77);

------------------------------------PURPOSE AND PRIORITY REMOVAL-------------------------------------
-- no longer needed, there are revenue related purpose values
--DELETE FROM WorkOrderPriorities WHERE Description = 'Revenue Related';
-- no longer needed, there are revenue related purposes with specific dollar amounts
--DELETE FROM WorkOrderPurposes WHERE Description = 'Revenue';

-----------------------------------RESTORATION RESPONSE PRIORITIES-----------------------------------
INSERT INTO [RestorationResponsePriorities] ([Description]) VALUES ('Emergency 5 day');
INSERT INTO [RestorationResponsePriorities] ([Description]) VALUES ('Emergency 24 hour');
INSERT INTO [RestorationResponsePriorities] ([Description]) VALUES ('Emergency 48 hour');
INSERT INTO [RestorationResponsePriorities] ([Description]) VALUES ('On Demand OT/Holiday');
INSERT INTO [RestorationResponsePriorities] ([Description]) VALUES ('On Demand Same Day');
INSERT INTO [RestorationResponsePriorities] ([Description]) VALUES ('Priority (10 days)');
INSERT INTO [RestorationResponsePriorities] ([Description]) VALUES ('Standard (30 days)');

-------------------------------------------BUSINESS UNITS--------------------------------------------
--SET IDENTITY_INSERT [dbo].[BusinessUnits] ON;
--BEGIN TRANSACTION;
--INSERT INTO [dbo].[BusinessUnits]([BusinessUnitID], [Description], [OperatingCenterID], [DepartmentID], [Order])
--SELECT 1, N'522602', 15, 1, 1 UNION ALL
--SELECT 2, N'522602', 15, 1, 1 UNION ALL
--SELECT 3, N'522606', 15, 2, 1 UNION ALL
--SELECT 4, N'522502', 16, 1, 1 UNION ALL
--SELECT 5, N'522506', 16, 2, 1 UNION ALL
--SELECT 6, N'533002', 17, 1, 1 UNION ALL
--SELECT 7, N'533006', 17, 2, 99 UNION ALL
--SELECT 8, N'558202', 19, 1, 1 UNION ALL
--SELECT 9, N'548102', 18, 1, 1 UNION ALL
--SELECT 10, N'181202', 11, 1, 1 UNION ALL
--SELECT 11, N'181206', 11, 2, 99 UNION ALL
--SELECT 12, N'182202', 11, 1, 2 UNION ALL
--SELECT 13, N'182206', 11, 2, 99 UNION ALL
--SELECT 14, N'189102', 11, 1, 3 UNION ALL
--SELECT 15, N'189106', 11, 2, 99 UNION ALL
--SELECT 16, N'189116', 11, 5, 99 UNION ALL
--SELECT 17, N'189202', 11, 1, 4 UNION ALL
--SELECT 18, N'189206', 11, 2, 99 UNION ALL
--SELECT 19, N'181902', 14, 1, 1 UNION ALL
--SELECT 20, N'181906', 14, 2, 99 UNION ALL
--SELECT 21, N'181102', 14, 1, 3 UNION ALL
--SELECT 22, N'181106', 14, 2, 99 UNION ALL
--SELECT 23, N'182302', 14, 1, 4 UNION ALL
--SELECT 24, N'182306', 14, 2, 99 UNION ALL
--SELECT 25, N'181302', 13, 1, 1 UNION ALL
--SELECT 26, N'181306', 13, 2, 99 UNION ALL
--SELECT 27, N'183102', 13, 1, 2 UNION ALL
--SELECT 28, N'183106', 13, 2, 99 UNION ALL
--SELECT 29, N'183202', 13, 1, 3 UNION ALL
--SELECT 30, N'183206', 13, 2, 99 UNION ALL
--SELECT 31, N'181502', 12, 1, 1 UNION ALL
--SELECT 32, N'181506', 12, 2, 99 UNION ALL
--SELECT 33, N'181702', 12, 1, 99 UNION ALL
--SELECT 34, N'181706', 12, 2, 99 UNION ALL
--SELECT 35, N'181802', 10, 1, 1 UNION ALL
--SELECT 36, N'181806', 10, 2, 99
--COMMIT;
--RAISERROR (N'[dbo].[BusinessUnits]: Insert Batch: 1.....Done!', 10, 1) WITH NOWAIT;
--GO

--SET IDENTITY_INSERT [dbo].[BusinessUnits] OFF;

-- INSERT INTO [BusinessUnits](OperatingCenterID, Description, DepartmentID, [Order]) SELECT (select RecID from tblOpCntr where opCntr = 'NJ1'), '180102', (Select DepartmentID from Departments where [Code] = '02'), 1
-- INSERT INTO [BusinessUnits](OperatingCenterID, Description, DepartmentID, [Order]) SELECT (select RecID from tblOpCntr where opCntr = 'NJ1'), '180103', (Select DepartmentID from Departments where [Code] = '03'), 99
-- INSERT INTO [BusinessUnits](OperatingCenterID, Description, DepartmentID, [Order]) SELECT (select RecID from tblOpCntr where opCntr = 'NJ1'), '180123', (Select DepartmentID from Departments where [Code] = '23'), 99
-- INSERT INTO [BusinessUnits](OperatingCenterID, Description, DepartmentID, [Order]) SELECT (select RecID from tblOpCntr where opCntr = 'NJ2'), '180106', (Select DepartmentID from Departments where [Code] = '06'), 99

---------------------------------------CUSTOMER IMPACT RANGES---------------------------------------
--INSERT INTO [CustomerImpactRanges] ([Description]) VALUES ('0-50');
--INSERT INTO [CustomerImpactRanges] ([Description]) VALUES ('51-100');
--INSERT INTO [CustomerImpactRanges] ([Description]) VALUES ('101-200');
--INSERT INTO [CustomerImpactRanges] ([Description]) VALUES ('>200');

-----------------------------------------REPAIR TIME RANGES-----------------------------------------
--INSERT INTO [RepairTimeRanges] ([Description]) VALUES ('4-6');
--INSERT INTO [RepairTimeRanges] ([Description]) VALUES ('8-10');
--INSERT INTO [RepairTimeRanges] ([Description]) VALUES ('10-12');

------------------------------------------CONTRACTOR USERS------------------------------------------
INSERT INTO [dbo].[ContractorUsers]([ContractorID], [Email], [Password], [PasswordSalt], [PasswordQuestion], [PasswordAnswer], [IsAdmin])
SELECT 50, N'admin@mmsinc.com', N'7ccf4e256e7acf41bcba45b325e468a270f1e330af039b2bd8eb6e98a5935f9b98043c29661474f194084837e94483ed8e592631810530a734ff3fe3fca01630', N'89781e60-b8d9-4524-bb2a-65627a06e804', N'Huh?', N'6394418470610e27301735075eff0aea718f3d84cd5a6b479d14ca9cf54a9026d58d67c6c9f57bbba247dca47f770ef8fb7a703065b5583bc2aea3c200825f17', 1


INSERT INTO RequisitionTypes Values('Paving')
INSERT INTO RequisitionTypes Values('Contracted Service')
INSERT INTO RequisitionTypes Values('Traffic Control')
INSERT INTO RequisitionTypes Values('Spoils')
