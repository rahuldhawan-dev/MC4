use mcprod
go

----------------------------------ASSET TYPES----------------------------------
SET IDENTITY_INSERT [dbo].[AssetTypes] ON;

INSERT INTO [AssetTypes] ([AssetTypeID], [Description]) VALUES (8, 'Storm/Catch');

SET IDENTITY_INSERT [dbo].[AssetTypes] OFF;

INSERT INTO [OperatingCenterAssetTypes] Values(11, 8)


-------------------------------WORK DESCRIPTIONS-------------------------------

SET IDENTITY_INSERT [dbo].[WorkDescriptions] ON;

INSERT INTO [dbo].[WorkDescriptions]([WorkDescriptionID], [WorkCategoryID], [Description], [AssetTypeID], [TimeToComplete], [AccountingTypeID], [FirstRestorationAccountingCodeID], [FirstRestorationCostBreakdown], [FirstRestorationProductCodeID], [SecondRestorationAccountingCodeID], [SecondRestorationCostBreakdown], [SecondRestorationProductCodeID], [ShowBusinessUnit], [ShowApprovalAccounting], [EditOnly])

SELECT 134, 52, N'STORM/CATCH REPAIR', 8, 1.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0 UNION ALL
SELECT 135, 53, N'STORM/CATCH REPLACE', 8, 1.00, 1, 1, 100, 1, NULL, NULL, NULL, 0, 1, 0 UNION ALL
SELECT 136, 51, N'STORM/CATCH INSTALLATION', 8, 1.00, 1, 1, 100, 1, NULL, NULL, NULL, 0, 1, 0 UNION ALL
SELECT 137, 9, N'STORM/CATCH INVESTIGATION', 8, 1.00, 2, 2, 100, 1, NULL, NULL, NULL, 1, 0, 0

SET IDENTITY_INSERT [dbo].[WorkDescriptions] OFF;

SET IDENTITY_INSERT [dbo].[WorkDescriptions] ON;

INSERT INTO [dbo].[WorkDescriptions](
		[WorkDescriptionID], 
		[Description], 
		[AssetTypeID], 
		[WorkCategoryID], 
		[TimeToComplete], 
		[AccountingTypeID], 
		[FirstRestorationAccountingCodeID],  
		[FirstRestorationCostBreakdown], 
		[FirstRestorationProductCodeID], 
		[ShowBusinessUnit], 
		[ShowApprovalAccounting], 
		[EditOnly],
		[Revisit])
SELECT 
		162 as [WorkDescriptionID],  
		'STORM/CATCH LANDSCAPING' as [Description],
		8 as AssetTypeID,
		49 as WorkCategoryID,
		1 as TimeToComplete,
		2 as AccountingTypeID,
		20 as [FirstRestorationAccountingCodeID],
		100 as [FirstRestorationCostBreakDown],
		1 as [FirstRestorationProductCodeID],
		1 as ShowBusinessUnit,
		0 as ShowApprovalAccounting, 
		0 as EditOnly, 
		1 as Revisit UNION ALL
SELECT
		163,
		'STORM/CATCH RESTORATION INVESTIGATION',
		8,
		49, 
		0.5,
		2,
		21,
		100,
		1,
		1,
		0,
		0, 
		1 as Revisit UNION ALL
SELECT 
		164,
		'STORM/CATCH RESTORATION REPAIR',
		8,
		49,
		1,
		2,
		22,
		100,
		1,
		1,
		0,
		0, 
		1 as Revisit 

SET IDENTITY_INSERT [dbo].[WorkDescriptions] OFF;


----------------------------------WORK ORDERS----------------------------------

ALTER TABLE WorkOrders Add StormCatchID int null

ALTER TABLE [WorkOrders] WITH NOCHECK ADD CONSTRAINT [FK_WorkOrders_StormWaterAssets_StormWaterAssetID] FOREIGN KEY (
[StormCatchID]
) REFERENCES [StormWaterAssets] (
[StormWaterAssetID]
)
GO