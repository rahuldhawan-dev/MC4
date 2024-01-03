-- GetObjectCounts.sql

-- Useful for testing/development, to see if any data has been
-- left behind by the unit tests.

USE [WorkOrdersTest]
GO

DECLARE @tblName varchar(50);
DECLARE @statement nvarchar(4000);
DECLARE @len int;
DECLARE tbls CURSOR FOR
SELECT
	[name]
FROM
	sysobjects
WHERE
	xtype = 'u'
ORDER BY
	[name]

OPEN tbls

FETCH NEXT FROM tbls
INTO @tblName;

SET @statement = '';

WHILE @@FETCH_STATUS = 0
BEGIN
	SET @statement = @statement + 'SELECT ''' + @tblName + ''' AS Name, count(1) ' +
		CASE @tblName
			/* STATIC DATA */
			WHEN 'AccountingTypes' THEN '- 3'
			WHEN 'AssetTypes' THEN '- 4'
			WHEN 'LeakReportingSources' THEN '- 4'
			WHEN 'MainConditions' THEN '- 13'
			WHEN 'MainFailureTypes' THEN '- 14'
			WHEN 'MainSizes' THEN '- 19'
			WHEN 'MarkoutRequirements' THEN '- 3'
			WHEN 'MarkoutStatuses' THEN '- 3'
			WHEN 'MarkoutTypes' THEN '- 3'
			WHEN 'Materials' THEN '- 487'
			WHEN 'OperatingCenterStockedMaterials' THEN '- 487'
			WHEN 'RestorationMethodsRestorationTypes' THEN '- 48'
			WHEN 'RestorationMethods' THEN '- 14'
			WHEN 'RestorationTypes' THEN '- 8'
			WHEN 'RestorationTypeCosts' THEN '- 8'
			WHEN 'StockLocations ' THEN '- 1'
			WHEN 'WorkAreaTypes' THEN '- 4'
			WHEN 'WorkCategories' THEN '- 27'
			WHEN 'WorkDescriptions' THEN '- 73'
			WHEN 'WorkOrderPriorities' THEN '- 4'
			WHEN 'WorkOrderPurposes' THEN '- 5'
			WHEN 'WorkOrderRequesters' THEN '- 4'
			/* SITE DATA */
			WHEN 'tblNJAWHydrant' THEN '- 6171'
			WHEN 'tblNJAWStreets' THEN '- 64398'
			WHEN 'tblNJAWTownNames' THEN '- 35'
			WHEN 'tblNJAWTwnSection' THEN '- 34'
			WHEN 'tblNJAWValves' THEN '- 26619'
			WHEN 'tblPermissions' THEN '- 959'
			WHEN 'tblOpCntr' THEN '- 18'
			/* SAMPLE DATA */
			WHEN 'Crews' THEN '- 2'
			WHEN 'CrewAssignments' THEN '- 2'
			WHEN 'MaterialsUsed' THEN '- 2'
			WHEN 'WorkOrders' THEN '- 8'
			ELSE ''
		END + 'AS Count FROM [' + @tblName + '] UNION ';
	FETCH NEXT FROM tbls
	INTO @tblName;
END

CLOSE tbls
DEALLOCATE tbls

SET @len = len(@statement) - 6;
SET @statement = left(@statement, @len)
EXEC sp_executesql @statement
