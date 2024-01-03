use mcprod
go

begin tran
	update tblNJAWTownNames set Town = replace(Town, ' BORO', ' BOROUGH') where right(town, 5) = ' BORO'
	update aww.dbo.tblTowns set Town = replace(Town, ' BORO', ' BOROUGH') where right(town, 5) = ' BORO'
	update tblNJAWStreets set Town = replace(Town, ' BORO', ' BOROUGH') where right(town, 5) = ' BORO'
	update tblNJAWTwnSection set Town = replace(Town, ' BORO', ' BOROUGH') where right(town, 5) = ' BORO'
	update tblNJAWHydOS set Town = replace(Town, ' BORO', ' BOROUGH') where right(town, 5) = ' BORO'
	update aww_tap.dbo.njtap set F2Label = replace(F2Label, ' BORO', ' BOROUGH') where right(F2Label, 5) = ' BORO'
	update aww_tap.dbo.njvalve set F2Label = replace(F2Label, ' BORO', ' BOROUGH') where right(F2Label, 5) = ' BORO'
	update aww_tap.dbo.njmaps set F2Label = replace(F2Label, ' BORO', ' BOROUGH') where right(F2Label, 5) = ' BORO'
rollback tran