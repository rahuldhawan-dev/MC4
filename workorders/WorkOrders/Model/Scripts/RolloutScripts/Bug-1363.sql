use [MCProd]
GO

INSERT INTO [NotificationPurposes] (ModuleID, Purpose)
SELECT
	(SELECT m.ModuleID FROM Modules m INNER JOIN Applications a ON m.ApplicationID = a.ApplicationID WHERE a.Name = 'Field Services' AND m.Name = 'Work Management'),
	'Service Line Renewal Entered'

INSERT INTO [NotificationPurposes] (ModuleID, Purpose)
SELECT
	(SELECT m.ModuleID FROM Modules m INNER JOIN Applications a ON m.ApplicationID = a.ApplicationID WHERE a.Name = 'Field Services' AND m.Name = 'Work Management'),
	'Service Line Installation Entered'
