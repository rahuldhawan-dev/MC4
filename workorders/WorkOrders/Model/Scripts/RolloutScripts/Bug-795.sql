use [McProd];
GO

ALTER TABLE [WorkOrders]
ADD [CustomerAlert] bit null;

ALTER TABLE [WorkOrders]
ADD [SignificantTrafficImpact] bit null;
GO

UPDATE [WorkOrders]
SET
[CustomerAlert] = 0,
[SignificantTrafficImpact] = 0
WHERE
[WorkDescriptionID] IN (74, 80) AND
[CustomerAlert] IS NULL AND [SignificantTrafficImpact] IS NULL;

UPDATE [WorkOrders]
SET
[RepairTimeRangeID] = 1,
[CustomerImpactRangeID] = 1
WHERE
[WorkDescriptionID] IN (74, 80) AND
[RepairTimeRangeID] IS NULL AND [CustomerImpactRangeID] IS NULL;
