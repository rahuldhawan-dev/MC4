USE MCPROD
GO

SET IDENTITY_INSERT [dbo].[WorkDescriptions] ON;

INSERT INTO [dbo].[WorkDescriptions]([WorkDescriptionID], [Description], [AssetTypeID], [WorkCategoryID], [TimeToComplete], [AccountingTypeID], [FirstRestorationAccountingCodeID], [FirstRestorationCostBreakdown], [FirstRestorationProductCodeID], [ShowBusinessUnit], [ShowApprovalAccounting], [EditOnly],[Revisit])

SELECT 165, 'RSTRN-RESTORATION INQUIRY', 3, 49, 0.5, 2, 6, 100, 1, 1, 0, 0, 1 UNION ALL
SELECT 166, 'RSTRN-RESTORATION INQUIRY', 4, 49, 0.5, 2, 9, 100, 1, 1, 0, 0, 1 UNION ALL
SELECT 167, 'RSTRN-RESTORATION INQUIRY', 6, 49, 0.5, 2, 13, 100, 1, 1, 0, 0, 1 UNION ALL
SELECT 168, 'RSTRN-RESTORATION INQUIRY', 7, 49, 0.5, 2, 16, 100, 1, 1, 0, 0, 1 UNION ALL
SELECT 169, 'RSTRN-RESTORATION INQUIRY', 3, 49, 0.5, 2, 6, 100, 1, 1, 0, 0, 0 UNION ALL
SELECT 170, 'RSTRN-RESTORATION INQUIRY', 4, 49, 0.5, 2, 9, 100, 1, 1, 0, 0, 0 UNION ALL
SELECT 171, 'RSTRN-RESTORATION INQUIRY', 6, 49, 0.5, 2, 13, 100, 1, 1, 0, 0, 0 UNION ALL
SELECT 172, 'RSTRN-RESTORATION INQUIRY', 7, 49, 0.5, 2, 16, 100, 1, 1, 0, 0, 0

SET IDENTITY_INSERT [dbo].[WorkDescriptions] OFF;