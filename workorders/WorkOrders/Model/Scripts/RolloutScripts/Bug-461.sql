use [McProd]
GO

SET IDENTITY_INSERT [dbo].[WorkDescriptions] ON;
BEGIN TRANSACTION;
INSERT INTO [dbo].[WorkDescriptions]([WorkDescriptionID], [Description], [AssetTypeID], [TimeToComplete], [WorkCategoryID], [AccountingTypeID])
SELECT 103, N'SERVICE LINE REPAIR', 4, 0.5, 12, 1
COMMIT;
SET IDENTITY_INSERT [dbo].[WorkDescriptions] OFF;
