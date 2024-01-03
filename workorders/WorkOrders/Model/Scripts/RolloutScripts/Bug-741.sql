use [McProd]

SET IDENTITY_INSERT [dbo].[OperatingCenterStockedMaterials] ON;
INSERT INTO [dbo].[OperatingCenterStockedMaterials]([OperatingCenterID], [MaterialID])
SELECT 12,16
SET IDENTITY_INSERT [dbo].[OperatingCenterStockedMaterials] OFF;
