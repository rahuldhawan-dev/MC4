
begin tran

ALTER TABLE
	[StreetOpeningPermits]
ADD 
	[PermitId] int null,
	[HasMetDrawingRequirement] bit null, 
	[IsPaidFor] bit null

commit tran
