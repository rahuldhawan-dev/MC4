UPDATE [WorkDescriptions] SET [Description] = 'SERVICE LINE FLOW TEST' WHERE [Description] = 'FLOW TEST'
INSERT INTO [WorkDescriptions] ([WorkCategoryID], [Description], [AssetTypeID], [TimeToComplete], [AccountingTypeID])
SELECT
	(SELECT [WorkCategoryID] FROM [WorkCategories] WHERE [Description] = 'Flow Test') AS [WorkCategoryID],
	'HYDRAULIC FLOW TEST' AS [Description],
	(SELECT [AssetTypeID] FROM [AssetTypes] WHERE [Description] = 'Hydrant') AS [AssetTypeID],
	1 AS [TimeToComplete],
	(SELECT [AccountingTypeID] FROM [AccountingTypes] WHERE [Description] = 'O&M') AS [AccountingTypeID]
