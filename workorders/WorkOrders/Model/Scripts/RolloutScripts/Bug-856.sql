use [McProd];
GO

SET IDENTITY_INSERT [dbo].[OperatingCenterStockedMaterials] ON;

INSERT INTO [dbo].[OperatingCenterStockedMaterials]([OperatingCenterStockedMaterialID], [OperatingCenterID], [MaterialID])
SELECT 5859,12,17

SET IDENTITY_INSERT [dbo].[OperatingCenterStockedMaterials] OFF;
