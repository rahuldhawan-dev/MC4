BEGIN TRAN

SET IDENTITY_INSERT [dbo].[WorkDescriptions] ON;

INSERT INTO [dbo].[WorkDescriptions]([WorkDescriptionID], [Description], 
		[AssetTypeID], [WorkCategoryID], [TimeToComplete], [AccountingTypeID], 
		[FirstRestorationAccountingCodeID], [FirstRestorationCostBreakdown], [FirstRestorationProductCodeID], 
		[ShowBusinessUnit], [ShowApprovalAccounting], [EditOnly],[Revisit])

SELECT 191, 'VALVE RETIREMENT - STORM RESTORATION',		1, 17, 2,  3,	26, 100, 1,	 1, 1, 0, 0

SET IDENTITY_INSERT [dbo].[WorkDescriptions] OFF;

ROLLBACK