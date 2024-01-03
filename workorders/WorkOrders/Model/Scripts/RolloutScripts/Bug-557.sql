use [McProd];
GO

ALTER TABLE [CrewAssignments]
ADD [EmployeesOnJob] float null;

ALTER TABLE [WorkOrders]
DROP COLUMN [TotalManHours];
