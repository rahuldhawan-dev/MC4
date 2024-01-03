USE [MCProd]
GO

INSERT INTO [dbo].[OperatingCenterStockedMaterials]([OperatingCenterID], [MaterialID])
SELECT 
	(SELECT RecID FROM tblOpCntr WHERE OpCntr = 'NJ7'), 
	(SELECT MaterialID FROM Materials WHERE PartNumber = 'W2002430')
UNION ALL
SELECT 
	(SELECT RecID FROM tblOpCntr WHERE OpCntr = 'NJ7'),
	(SELECT MaterialID FROM Materials WHERE PartNumber = 'W2043600')