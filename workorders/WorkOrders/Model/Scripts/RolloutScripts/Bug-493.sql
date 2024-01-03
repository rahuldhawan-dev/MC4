USE [MCProd]

SET IDENTITY_INSERT [dbo].[WorkCategories] ON;

INSERT INTO [dbo].WorkCategories (WorkCategoryID, Description)
SELECT 45, 'Sewer Lateral Repair' UNION ALL
SELECT 46, 'Sewer Investigative'

SET IDENTITY_INSERT [dbo].[WorkCategories] OFF;


SET IDENTITY_INSERT [dbo].[WorkDescriptions] ON;

INSERT INTO [dbo].[WorkDescriptions]([WorkDescriptionID], [Description], [AssetTypeID], [TimeToComplete], [WorkCategoryID], [AccountingTypeID])

SELECT 104, N'SEWER CLEAN OUT INSTALLATION',	6, 3, 45, 1 UNION ALL
SELECT 105, N'SEWER CLEAN OUT REPAIR', 6, 0.5, 45, 2 UNION ALL
SELECT 106, N'SEWER CAMERA SERVICE',	6, 8, 9, 2 UNION ALL
SELECT 107, N'SEWER CAMERA MAIN', 7, 8, 9, 2 UNION ALL
SELECT 108, N'SEWER DEMOLITION INSPECTION',	6, 1, 45, 2 UNION ALL
SELECT 109, N'SEWER MAIN TEST HOLES', 7, 8, 46, 2 UNION ALL
SELECT 110, N'WATER MAIN TEST HOLES', 3, 8, 9, 2 

SET IDENTITY_INSERT [dbo].[WorkDescriptions] OFF;
select * from assetTypes