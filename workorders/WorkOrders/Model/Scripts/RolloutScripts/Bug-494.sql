USE [MCProd]
GO

SET IDENTITY_INSERT [dbo].[WorkDescriptions] ON;

INSERT INTO [dbo].[WorkDescriptions]([WorkDescriptionID], [Description], [AssetTypeID], [TimeToComplete], [WorkCategoryID], [AccountingTypeID])

SELECT 112, N'GROUND WATER-MAIN', 3, 0.75, 9, 2 

SET IDENTITY_INSERT [dbo].[WorkDescriptions] OFF;

UPDATE [dbo].[WorkDescriptions]
SET [Description] = 'GROUND WATER-SERVICE' WHERE WorkDescriptionID = 23;
