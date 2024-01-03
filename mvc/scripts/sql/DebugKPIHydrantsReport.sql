SELECT DISTINCT
h.Id as HydrantId,
hb.[Description] as Billing,
hb.Id as BillingId,
hs.[Description] as Status,
hs.Id as StatusId,
oc.ZoneStartYear,
h.InspectionFrequency,
rfu.Description as InspectionFrequencyUnit,
oc.HydrantInspectionFrequency as OCInspectionFrequency,
rfu2.Description as OCInspectionFrequencyUnit
from HydrantInspections as hi
inner join Hydrants as h
on h.Id = hi.HydrantId
left outer join RecurringFrequencyUnits as rfu
on rfu.Id = h.InspectionFrequencyUnitId
inner join OperatingCenters as oc
on oc.OperatingCenterId = h.OperatingCenterId
left outer join RecurringFrequencyUnits as rfu2
on rfu2.Id = oc.HydrantInspectionFrequencyUnitId
inner join HydrantBillings as hb
on hb.Id = h.HydrantBillingId
inner join HydrantStatuses hs
on hs.Id = h.HydrantStatusId
where oc.OperatingCenterID = 42
and datepart(year, hi.DateInspected) = 2018
and not (exists (select hi2.Id from HydrantInspections hi2 where h.Id = hi2.HydrantId and datepart(year, hi2.DateInspected) = datepart(year, hi.DateInspected) and hi2.DateInspected < hi.DateInspected))
