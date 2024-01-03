use [mcprod]
go

SET IDENTITY_INSERT [dbo].[Materials] ON;
BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '4339', 'W2003636', 'Box, Meter Plastic 36"X36"'
COMMIT;

SET IDENTITY_INSERT [dbo].[Materials] OFF;

INSERT INTO [dbo].[OperatingCenterStockedMaterials]([OperatingCenterID], [MaterialID])
-- EW1
SELECT '15', '4339' UNION ALL
-- EW2
-- EW4
SELECT '19', '4339' UNION ALL
-- LWC
SELECT '18', '4339';
