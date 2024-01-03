BEGIN TRAN

SET IDENTITY_INSERT [dbo].[WorkDescriptions] ON;

INSERT INTO [dbo].[WorkDescriptions]([WorkDescriptionID], [Description], 
		[AssetTypeID], [TimeToComplete], [WorkCategoryID],  [AccountingTypeID], 
		[FirstRestorationAccountingCodeID], [FirstRestorationCostBreakdown], [FirstRestorationProductCodeID], 
		[ShowBusinessUnit], [ShowApprovalAccounting], [EditOnly],[Revisit])
SELECT 
	197 as [WorkDescriptionID],
	'FRAME AND COVER REPLACE - STORM RESTORATION' as [Description],
	4 as [AssetTypeID],
	0.5 as [TimeToComplete],
	4 as [WorkCategoryID],
	2 as [AccountingTypeID],
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