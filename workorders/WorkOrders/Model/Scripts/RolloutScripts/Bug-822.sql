use [McProd];
GO

SET IDENTITY_INSERT [dbo].[OperatingCenterStockedMaterials] ON;

INSERT INTO [dbo].[OperatingCenterStockedMaterials]([OperatingCenterStockedMaterialID], [OperatingCenterID], [MaterialID])
SELECT 5858,12,957;

SET IDENTITY_INSERT [dbo].[OperatingCenterStockedMaterials] OFF;
