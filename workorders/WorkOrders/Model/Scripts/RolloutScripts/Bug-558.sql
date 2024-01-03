USE MCProd
GO
SET IDENTITY_INSERT [dbo].[OperatingCenterStockedMaterials] ON;

INSERT INTO [dbo].[OperatingCenterStockedMaterials]([OperatingCenterStockedMaterialID], [OperatingCenterID], [MaterialID])
SELECT '5455', '15', '967' UNION ALL
SELECT '5456', '16', '967' UNION ALL
SELECT '5457', '17', '967' UNION ALL
SELECT '5458', '19', '967' UNION ALL
SELECT '5459', '18', '967' 

SET IDENTITY_INSERT [dbo].[OperatingCenterStockedMaterials] OFF;