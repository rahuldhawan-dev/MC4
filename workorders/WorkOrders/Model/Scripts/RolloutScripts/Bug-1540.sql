
create table #parts (part varchar(7), size varchar(8), description varchar(50))

insert into #parts values('1411948', '1"', '1" Corporation 90');
insert into #parts values('1411946', '1"', '1" Corporation 45');
insert into #parts values('1411841', '1"', '1" Curb Stop  I x I');
insert into #parts values('1411600', '1"', '1" Curb Stop C x C');
insert into #parts values('1411869', '1"', '1" Curb Stop C x C');
insert into #parts values('1412037', '1"', '1" Male Adapter');
insert into #parts values('1412536', '1"', '1" Adams Male Adapter');
insert into #parts values('1412001', '1"', '1" Union');
insert into #parts values('1412007', '1"', '1" x 3/4" Union');
insert into #parts values('1412009', '3/4"', '3/4" Union');
insert into #parts values('1412222', '1"', '1" Meter Setter');
insert into #parts values('1412017', '1"', '1" Meter Idler');
insert into #parts values('1412221', '1"', '1" Meter Insetter');
insert into #parts values('1411851', '1 1/2"', '1 1/2" Corporation');
insert into #parts values('1412347', '1 1/2"', '1 1/2" Corporation 90');
insert into #parts values('1411957', '1 1/2"', '1 1/2" Curb Stop C x C');
insert into #parts values('1411842', '1 1/2"', '1 1/2" Curb Stop I x I');
insert into #parts values('1412038', '1 1/2"', '1 1/2" Male Adapter');
insert into #parts values('1411934', '1 1/2"', '1 1/2" Tube Nut');
insert into #parts values('1412537', '1 1/2"', '1 1/2" Adams Male Adapter');
insert into #parts values('1412002', '1 1/2"', '1 1/2" Union');
insert into #parts values('1412168', '1 1/2"', '1 1/2" Meter Setter');
insert into #parts values('1411852', '2"', '2" Corporation');
insert into #parts values('1412348', '2"', '2" Corporation 90');
insert into #parts values('1411871', '2"', '2" Curb Stop C x C');
insert into #parts values('1411843', '2"', '2" Curb Stop I x I');
insert into #parts values('1412045', '2"', '2" Male Adapter');
insert into #parts values('1411936', '2"', '2" Tube Nut');
insert into #parts values('1412538', '2"', '2" Adams Male Adapter');
insert into #parts values('1412008', '2"', '2" Union');
insert into #parts values('1412171', '2"', '2" Meter Setter');
insert into #parts values('1412066', '2"', '2" Straight Connection');
insert into #parts values('1412483', '2"', '2" Ell, 90 Comp x Comp');
insert into #parts values('1412492', '2"', '2" Ell, 90 Comp x Mipt');
insert into #parts values('1412496', '2"', '2" Ell, 90 Comp x Fipt');

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
and 
	Oc.OperatingCenterCode in ('EW1','EW2')

drop table #parts
