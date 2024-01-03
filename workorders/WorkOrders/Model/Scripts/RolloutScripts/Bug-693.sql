use mcprod
go

update 
	workdescriptions
set 
	AccountingTypeID = (select AccountingTypeID from accountingTypes where [Description] = 'O&M')
where
	workDescriptionID = 103
and
	[Description] = 'SERVICE LINE REPAIR'