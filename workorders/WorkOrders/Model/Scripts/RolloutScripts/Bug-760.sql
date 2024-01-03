USE [McProd]
GO

UPDATE [MaterialsUsed]
SET [StockLocationID] = (SELECT [StockLocationID] FROM [StockLocations] WHERE [OperatingCenterID] = 12 AND [Description] COLLATE Latin1_General_CS_AS = 'WSYD')
WHERE [StockLocationID] = (SELECT [StockLocationID] FROM [StockLocations] WHERE [OperatingCenterID] = 12 AND [Description] COLLATE Latin1_General_CS_AS = 'wsyd');

DELETE FROM [StockLocations] WHERE [OperatingCenterID] = 12 AND [Description] COLLATE Latin1_General_CS_AS = 'wsyd';