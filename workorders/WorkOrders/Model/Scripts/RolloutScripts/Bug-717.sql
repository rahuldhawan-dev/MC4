use mcprod
go
SET IDENTITY_INSERT OperatingCenterStockedMaterials ON
INSERT INTO [dbo].[OperatingCenterStockedMaterials]([OperatingCenterStockedMaterialID], [OperatingCenterID], [MaterialID])
SELECT '5853', '11', '1693'
SET IDENTITY_INSERT OperatingCenterStockedMaterials OFF