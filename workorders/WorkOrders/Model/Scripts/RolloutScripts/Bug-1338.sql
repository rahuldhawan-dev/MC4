BEGIN TRAN

SET IDENTITY_INSERT [dbo].[WorkDescriptions] ON;

INSERT INTO [dbo].[WorkDescriptions]([WorkDescriptionID], [Description], 
		[AssetTypeID], [WorkCategoryID], [TimeToComplete], [AccountingTypeID], 
		[FirstRestorationAccountingCodeID], [FirstRestorationCostBreakdown], [FirstRestorationProductCodeID], 
		[ShowBusinessUnit], [ShowApprovalAccounting], [EditOnly],[Revisit])

SELECT 192, 'EXCAVATE METER PIT- STORM RESTORATION',		4, 1, 0.5, 2,	1,  100, 1,  1, 1, 0, 0

SET IDENTITY_INSERT [dbo].[WorkDescriptions] OFF;

ROLLBACK