BEGIN TRAN

SET IDENTITY_INSERT [dbo].[WorkDescriptions] ON;

INSERT INTO [dbo].[WorkDescriptions]([WorkDescriptionID], [Description], 
		[AssetTypeID], [WorkCategoryID], [TimeToComplete], [AccountingTypeID], 
		[FirstRestorationAccountingCodeID], [FirstRestorationCostBreakdown], [FirstRestorationProductCodeID], 
		[ShowBusinessUnit], [ShowApprovalAccounting], [EditOnly],[Revisit])

SELECT 193, 'SERVICE LINE RENEWAL - STORM RESTORATION', 	4, 1, 4, 1,		1,  100, 1,  1, 1, 0, 0

SET IDENTITY_INSERT [dbo].[WorkDescriptions] OFF;

ROLLBACK