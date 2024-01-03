USE [MCProd]
GO

SET IDENTITY_INSERT [dbo].[WorkDescriptions] ON;

INSERT INTO [dbo].[WorkDescriptions]([WorkDescriptionID], [Description], [AssetTypeID], [TimeToComplete], [WorkCategoryID], [AccountingTypeID])
SELECT 111, N'VALVE BROKEN', 1, 4, 17, 2 

SET IDENTITY_INSERT [dbo].[WorkDescriptions] OFF;
