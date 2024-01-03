use [McProd]
GO

ALTER TABLE [Restorations] ALTER COLUMN [PavingSquareFootage] decimal(9,2);
ALTER TABLE [Restorations] ALTER COLUMN [LinearFeetOfCurb] decimal(9,2);

ALTER TABLE [Restorations] ADD [TrafficControlCostPartialRestoration] int null;
ALTER TABLE [Restorations] ADD [TrafficControlCostFinalRestoration] int null;
GO

UPDATE [Restorations]
SET
[TrafficControlCostPartialRestoration] = [TrafficControlHoursPartialRestoration] * 60,
[TrafficControlCostFinalRestoration] = [TrafficControlHoursFinalRestoration] * 60;

ALTER TABLE [Restorations] DROP COLUMN [TrafficControlHoursPartialRestoration];
ALTER TABLE [Restorations] DROP COLUMN [TrafficControlHoursFinalRestoration];
