BEGIN TRAN

SET IDENTITY_INSERT [dbo].[WorkDescriptions] ON;

INSERT INTO [dbo].[WorkDescriptions]([WorkDescriptionID], [Description], 
		[AssetTypeID], [TimeToComplete], [WorkCategoryID],  [AccountingTypeID], 
		[FirstRestorationAccountingCodeID], [FirstRestorationCostBreakdown], [FirstRestorationProductCodeID], 
		[ShowBusinessUnit], [ShowApprovalAccounting], [EditOnly],[Revisit])
SELECT 
	194 as [WorkDescriptionID],
	'CURB BOX REPLACEMENT - STORM RESTORATION' as [Description],
	4 as [AssetTypeID],
	1 as [TimeToComplete],
	28 as [WorkCategoryID],
	1 as [AccountingTypeID],
	1 as [FirstRestorationAccountingCodeID],
	100 as [FirstRestorationCostBreakdown],
	1 as [FirstRestorationProductCodeID],
	1 as [ShowBusinessUnit],
	1 as [ShowApprovalAccounting],
	0 as [EditOnly],
	0 as [Revisit]

	
INSERT INTO [dbo].[WorkDescriptions]([WorkDescriptionID], [Description], 
		[AssetTypeID], [TimeToComplete], [WorkCategoryID],  [AccountingTypeID], 
		[FirstRestorationAccountingCodeID], [FirstRestorationCostBreakdown], [FirstRestorationProductCodeID], 
		[ShowBusinessUnit], [ShowApprovalAccounting], [EditOnly],[Revisit])
SELECT 
	195 as [WorkDescriptionID],
	'WATER MAIN RETIREMENT - STORM RESTORATION' as [Description],
	4 as [AssetTypeID],
	4 as [TimeToComplete],
	3 as [WorkCategoryID],
	1 as [AccountingTypeID],
	1 as [FirstRestorationAccountingCodeID],
	100 as [FirstRestorationCostBreakdown],
	1 as [FirstRestorationProductCodeID],
	1 as [ShowBusinessUnit],
	1 as [ShowApprovalAccounting],
	0 as [EditOnly],
	0 as [Revisit]


INSERT INTO [dbo].[WorkDescriptions]([WorkDescriptionID], [Description], 
		[AssetTypeID], [TimeToComplete], [WorkCategoryID],  [AccountingTypeID], 
		[FirstRestorationAccountingCodeID], [FirstRestorationCostBreakdown], [FirstRestorationProductCodeID], 
		[ShowBusinessUnit], [ShowApprovalAccounting], [EditOnly],[Revisit])
SELECT 
	196 as [WorkDescriptionID],
	'SERVICE LINE RETIREMENT - STORM RESTORATION' as [Description],
	4 as [AssetTypeID],
	2 as [TimeToComplete],
	4 as [WorkCategoryID],
	1 as [AccountingTypeID],
	1 as [FirstRestorationAccountingCodeID],
	100 as [FirstRestorationCostBreakdown],
	1 as [FirstRestorationProductCodeID],
	1 as [ShowBusinessUnit],
	1 as [ShowApprovalAccounting],
	0 as [EditOnly],
	0 as [Revisit]


SET IDENTITY_INSERT [dbo].[WorkDescriptions] OFF;

select * from WorkDescriptions

ROLLBACK