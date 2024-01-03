use [MCProd]
GO

INSERT INTO [NotificationPurposes] (ModuleID, Purpose)
SELECT
	(SELECT m.ModuleID FROM Modules m INNER JOIN Applications a ON m.ApplicationID = a.ApplicationID WHERE a.Name = 'Field Services' AND m.Name = 'Work Management'),
	'Curb-Pit Compliance'
INSERT INTO [NotificationPurposes] (ModuleID, Purpose)
SELECT
	(SELECT m.ModuleID FROM Modules m INNER JOIN Applications a ON m.ApplicationID = a.ApplicationID WHERE a.Name = 'Field Services' AND m.Name = 'Work Management'),
	'Curb-Pit Revenue'
INSERT INTO [NotificationPurposes] (ModuleID, Purpose)
SELECT
	(SELECT m.ModuleID FROM Modules m INNER JOIN Applications a ON m.ApplicationID = a.ApplicationID WHERE a.Name = 'Field Services' AND m.Name = 'Work Management'),
	'Curb-Pit Estimate'