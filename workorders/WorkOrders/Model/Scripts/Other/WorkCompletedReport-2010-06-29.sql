DECLARE @nj7 AS int;
DECLARE @StartDate AS varchar(10), @EndDate AS varchar(10);
SELECT
	@nj7 = [RecID],
	@StartDate = '2009-01-01',
	@EndDate = '2009-12-31'
FROM
	[tblOpCntr]
WHERE
	[OpCntr] = 'NJ7';

SELECT COUNT(1) AS [Total Orders] FROM [WorkOrders] WHERE [OperatingCenterID] = @nj7 AND ([DateCompleted] BETWEEN @StartDate AND @EndDate);

(SELECT
	1 AS [MonthID],
	'January' AS [Month],
	wd.[Description],
	COUNT(wo.[WorkDescriptionID]) AS [Count]
FROM
	[WorkOrders] AS wo
INNER JOIN
	[WorkDescriptions] AS wd
ON
	wo.[WorkDescriptionID] = wd.[WorkDescriptionID]
WHERE
	wo.[OperatingCenterID] = @nj7
AND
	DATEPART(month, wo.[DateCompleted]) = 1
AND
	(wo.[DateCompleted] BETWEEN @StartDate AND @EndDate)
GROUP BY
	wd.[Description])

UNION ALL

(SELECT
	2 AS [MonthID],
	'February' AS [Month],
	wd.[Description],
	COUNT(wo.[WorkDescriptionID]) AS [Count]
FROM
	[WorkOrders] AS wo
INNER JOIN
	[WorkDescriptions] AS wd
ON
	wo.[WorkDescriptionID] = wd.[WorkDescriptionID]
WHERE
	wo.[OperatingCenterID] = @nj7
AND
	DATEPART(month, wo.[DateCompleted]) = 2
AND
	(wo.[DateCompleted] BETWEEN @StartDate AND @EndDate)
GROUP BY
	wd.[Description])

UNION ALL

(SELECT
	3 AS [MonthID],
	'March' AS [Month],
	wd.[Description],
	COUNT(wo.[WorkDescriptionID]) AS [Count]
FROM
	[WorkOrders] AS wo
INNER JOIN
	[WorkDescriptions] AS wd
ON
	wo.[WorkDescriptionID] = wd.[WorkDescriptionID]
WHERE
	wo.[OperatingCenterID] = @nj7
AND
	DATEPART(month, wo.[DateCompleted]) = 3
AND
	(wo.[DateCompleted] BETWEEN @StartDate AND @EndDate)
GROUP BY
	wd.[Description])

UNION ALL

(SELECT
	4 AS [MonthID],
	'April' AS [Month],
	wd.[Description],
	COUNT(wo.[WorkDescriptionID]) AS [Count]
FROM
	[WorkOrders] AS wo
INNER JOIN
	[WorkDescriptions] AS wd
ON
	wo.[WorkDescriptionID] = wd.[WorkDescriptionID]
WHERE
	wo.[OperatingCenterID] = @nj7
AND
	DATEPART(month, wo.[DateCompleted]) = 4
AND
	(wo.[DateCompleted] BETWEEN @StartDate AND @EndDate)
GROUP BY
	wd.[Description])

UNION ALL

(SELECT
	5 AS [MonthID],
	'May' AS [Month],
	wd.[Description],
	COUNT(wo.[WorkDescriptionID]) AS [Count]
FROM
	[WorkOrders] AS wo
INNER JOIN
	[WorkDescriptions] AS wd
ON
	wo.[WorkDescriptionID] = wd.[WorkDescriptionID]
WHERE
	wo.[OperatingCenterID] = @nj7
AND
	DATEPART(month, wo.[DateCompleted]) = 5
AND
	(wo.[DateCompleted] BETWEEN @StartDate AND @EndDate)
GROUP BY
	wd.[Description])

UNION ALL

(SELECT
	6 AS [MonthID],
	'June' AS [Month],
	wd.[Description],
	COUNT(wo.[WorkDescriptionID]) AS [Count]
FROM
	[WorkOrders] AS wo
INNER JOIN
	[WorkDescriptions] AS wd
ON
	wo.[WorkDescriptionID] = wd.[WorkDescriptionID]
WHERE
	wo.[OperatingCenterID] = @nj7
AND
	DATEPART(month, wo.[DateCompleted]) = 6
AND
	(wo.[DateCompleted] BETWEEN @StartDate AND @EndDate)
GROUP BY
	wd.[Description])

UNION ALL

(SELECT
	7 AS [MonthID],
	'July' AS [Month],
	wd.[Description],
	COUNT(wo.[WorkDescriptionID]) AS [Count]
FROM
	[WorkOrders] AS wo
INNER JOIN
	[WorkDescriptions] AS wd
ON
	wo.[WorkDescriptionID] = wd.[WorkDescriptionID]
WHERE
	wo.[OperatingCenterID] = @nj7
AND
	DATEPART(month, wo.[DateCompleted]) = 7
AND
	(wo.[DateCompleted] BETWEEN @StartDate AND @EndDate)
GROUP BY
	wd.[Description])

UNION ALL

(SELECT
	8 AS [MonthID],
	'August' AS [Month],
	wd.[Description],
	COUNT(wo.[WorkDescriptionID]) AS [Count]
FROM
	[WorkOrders] AS wo
INNER JOIN
	[WorkDescriptions] AS wd
ON
	wo.[WorkDescriptionID] = wd.[WorkDescriptionID]
WHERE
	wo.[OperatingCenterID] = @nj7
AND
	DATEPART(month, wo.[DateCompleted]) = 8
AND
	(wo.[DateCompleted] BETWEEN @StartDate AND @EndDate)
GROUP BY
	wd.[Description])

UNION ALL

(SELECT
	9 AS [MonthID],
	'September' AS [Month],
	wd.[Description],
	COUNT(wo.[WorkDescriptionID]) AS [Count]
FROM
	[WorkOrders] AS wo
INNER JOIN
	[WorkDescriptions] AS wd
ON
	wo.[WorkDescriptionID] = wd.[WorkDescriptionID]
WHERE
	wo.[OperatingCenterID] = @nj7
AND
	DATEPART(month, wo.[DateCompleted]) = 9
AND
	(wo.[DateCompleted] BETWEEN @StartDate AND @EndDate)
GROUP BY
	wd.[Description])

UNION ALL

(SELECT
	10 AS [MonthID],
	'October' AS [Month],
	wd.[Description],
	COUNT(wo.[WorkDescriptionID]) AS [Count]
FROM
	[WorkOrders] AS wo
INNER JOIN
	[WorkDescriptions] AS wd
ON
	wo.[WorkDescriptionID] = wd.[WorkDescriptionID]
WHERE
	wo.[OperatingCenterID] = @nj7
AND
	DATEPART(month, wo.[DateCompleted]) = 10
AND
	(wo.[DateCompleted] BETWEEN @StartDate AND @EndDate)
GROUP BY
	wd.[Description])

UNION ALL

(SELECT
	11 AS [MonthID],
	'November' AS [Month],
	wd.[Description],
	COUNT(wo.[WorkDescriptionID]) AS [Count]
FROM
	[WorkOrders] AS wo
INNER JOIN
	[WorkDescriptions] AS wd
ON
	wo.[WorkDescriptionID] = wd.[WorkDescriptionID]
WHERE
	wo.[OperatingCenterID] = @nj7
AND
	DATEPART(month, wo.[DateCompleted]) = 11
AND
	(wo.[DateCompleted] BETWEEN @StartDate AND @EndDate)
GROUP BY
	wd.[Description])

UNION ALL

(SELECT
	12 AS [MonthID],
	'December' AS [Month],
	wd.[Description],
	COUNT(wo.[WorkDescriptionID]) AS [Count]
FROM
	[WorkOrders] AS wo
INNER JOIN
	[WorkDescriptions] AS wd
ON
	wo.[WorkDescriptionID] = wd.[WorkDescriptionID]
WHERE
	wo.[OperatingCenterID] = @nj7
AND
	DATEPART(month, wo.[DateCompleted]) = 12
AND
	(wo.[DateCompleted] BETWEEN @StartDate AND @EndDate)
GROUP BY
	wd.[Description])

ORDER BY 1, 3;