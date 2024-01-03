USE [McProd]
GO

SET IDENTITY_INSERT [dbo].[WorkDescriptions] ON;
BEGIN TRANSACTION;
INSERT INTO [dbo].[WorkDescriptions]([WorkDescriptionID], [Description], [AssetTypeID], [TimeToComplete], [WorkCategoryID], [AccountingTypeID])
SELECT 101, N'SITE INSPECTION/SURVEY NEW SERVICE', 4, 0.5, 13, 1 UNION ALL
SELECT 102, N'SITE INSPECTION/SURVEY SERVICE RENEWAL', 4, 0.5, 14, 1
COMMIT;
SET IDENTITY_INSERT [dbo].[WorkDescriptions] OFF;
