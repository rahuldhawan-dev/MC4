use [McProd];
GO

SET IDENTITY_INSERT [dbo].[Materials] ON;
BEGIN TRANSACTION;
INSERT INTO [dbo].[Materials]([MaterialID], [PartNumber], [Description])
SELECT '5555', 'W0540H0H', 'Coupling, 3/4" CF X CF'
COMMIT;
SET IDENTITY_INSERT [dbo].[Materials] OFF;

INSERT INTO [dbo].[OperatingCenterStockedMaterials]([OperatingCenterID], [MaterialID])
SELECT '13', '5555';
