use [McProd]

BEGIN TRANSACTION;

SET IDENTITY_INSERT [dbo].[Materials] ON;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '5554', 'W5181065', 'Hydrant Delran 4'' Bury';
SET IDENTITY_INSERT [dbo].[Materials] OFF;

SET IDENTITY_INSERT [dbo].[OperatingCenterStockedMaterials] ON;
INSERT INTO [dbo].[OperatingCenterStockedMaterials]([OperatingCenterStockedMaterialID], [OperatingCenterID], [MaterialID])
SELECT '5454', '13', '1'
SET IDENTITY_INSERT [dbo].[OperatingCenterStockedMaterials] OFF;

COMMIT TRANSACTION;