USE MCProd
GO

INSERT INTO [WorkCategories] ([Description])
Values ('Service Response Work')

SET IDENTITY_INSERT [dbo].[WorkDescriptions] ON;

INSERT INTO [dbo].[WorkDescriptions]([WorkDescriptionID], [Description], [AssetTypeID], [TimeToComplete], [WorkCategoryID], [AccountingTypeID])

SELECT 113, N'SERVICE - TURN ON', 4, 0.5, 47, 2 UNION ALL
SELECT 114, N'SERVICE - TURN OFF', 4, 0.5, 47, 2 UNION ALL
SELECT 115, N'METER - OBTAIN READ', 4, 0.5, 47, 2 UNION ALL
SELECT 116, N'METER - FINAL/START READ', 4, 0.5, 47, 2 UNION ALL
SELECT 117, N'METER - REPAIR TOUCH PAD', 4, 0.5, 47, 2 

SET IDENTITY_INSERT [dbo].[WorkDescriptions] OFF; 

--DELETE WorkDescriptions where WorkDescriptionID in (112,113,114,115,116)
--DBCC CHECKIDENT(WorkDescriptions, reseed, 111)