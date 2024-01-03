
create table #parts (part varchar(7), size varchar(8), description varchar(50))
insert into #parts values('1406378', '10"', '10" Class 52 DIP')
insert into #parts values('1404389', '2"', '2" COMP. x COMP. curb stop')
insert into #parts values('1410842', '2"', '2" FIPT x FIPT curb stop')
insert into #parts values('1411600', '1"', '1" COMP. x COMP. curb stop')
insert into #parts values('1411952', '1"', '1" CC x FIPT curb stop, NL')
insert into #parts values('1411953', '1 1/2"', '1 1/2" CC x FIPT curb stop, NL')
insert into #parts values('1411954', '2"', '2" CC x FIPT curb stop, NL')
insert into #parts values('1411955', '3/4"', '3/4" CC x FIPT curb stop, NL')
insert into #parts values('1412674', '', 'Curb Box Top Section')
insert into #parts values('1412671', '', 'Curb Box Bottom Section')
insert into #parts values('1412673', '', 'Curb Box Lid')
insert into #parts values('1412672', '', 'Curb Box Extension')
insert into #parts values('1412676', '', 'Roadway Box Top Section')
insert into #parts values('1409329', '', 'Roadway Box Bottom Section')
insert into #parts values('1412675', '', 'Roadway Box Lid')

--- ADD MATERIALS TO TABLE IF THEY DON'T EXIST
insert into Materials(Size, Description, PartNumber, IsActive)
select 
	size, description, part, 1
from 
	#parts 
where 
	not exists (select 1 from Materials where isNull(PartNumber,'') = isNull(part,''))

-- ADD MATERIALS TO OPERATING CENTER STOCKED MATERIALS IF THEY DON'T EXIST ALREADY
insert into	
	OperatingCenterStockedMaterials(OperatingCenterID, MaterialID)
select
	OperatingCenterID, M.MaterialID
from
	OperatingCenters OC, Materials M
where
	M.PartNumber in (select part from #parts)
and
	Not exists (select 1 from OperatingCenterStockedMaterials ocsm where ocsm.MaterialID = M.MaterialID and ocsm.OperatingCenterID = oc.OperatingCenterID)

drop table #parts
