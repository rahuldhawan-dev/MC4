using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MapCall.Common.Data;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.ViewModels;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using NHibernate.Type;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class ValveInspectionRepository : MapCallSecuredRepositoryBase<ValveInspection>, IValveInspectionRepository
    {
        #region Constructors

        public ValveInspectionRepository(IRepository<AggregateRole> roleRepo, ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IValveRepository valveRepository) : base(session,
            container,
            authenticationService, roleRepo)
        {
            _valveRepository = valveRepository;
        }

        #endregion

        #region Fields

        private readonly IValveRepository _valveRepository;

        #endregion

        #region Properties

        public override ICriteria Criteria
        {
            get
            {
                // Aliases need to be set for both admin and restricted users
                // in order for searching to work.
                // Also there's a potential performance issue here because adding
                // each alias makes NHibernate select * for all the joined tables 
                // which is almost never needed.
                var critter = base.Criteria
                                  .CreateAlias("Valve", "v")
                                  .CreateAlias("v.OperatingCenter", "oc")
                                  .CreateAlias("v.Coordinate", "coord", JoinType.LeftOuterJoin);
                if (CurrentUserCanAccessAllTheRecords)
                {
                    return critter;
                }

                return critter.Add(Restrictions.In("oc.Id", GetUserOperatingCenterIds()));
            }
        }

        public override IQueryable<ValveInspection> Linq
        {
            get
            {
                if (CurrentUserCanAccessAllTheRecords)
                {
                    return base.Linq;
                }

                var operatingCenterIds = GetUserOperatingCenterIds();
                return base.Linq.Where(x => operatingCenterIds.Contains(x.Valve.OperatingCenter.Id));
            }
        }

        public override RoleModules Role
        {
            get { return RoleModules.FieldServicesAssets; }
        }

        #endregion

        #region Exposed Methods

        public IEnumerable<ValveInspectionSearchResultViewModel> SearchInspections(ISearchValveInspection search)
        {
            var query = Session.QueryOver<ValveInspection>();
            ValveInspectionSearchResultViewModel result = null;

            Valve val = null;
            query.JoinAlias(x => x.Valve, () => val, JoinType.LeftOuterJoin);

            OperatingCenter opc = null;
            query.JoinAlias(x => val.OperatingCenter, () => opc, JoinType.LeftOuterJoin);

            Town town = null;
            query.JoinAlias(x => val.Town, () => town, JoinType.LeftOuterJoin);

            User inspectedBy = null;
            query.JoinAlias(x => x.InspectedBy, () => inspectedBy, JoinType.LeftOuterJoin);

            Employee inspectedByEmployee = null;
            query.JoinAlias(() => inspectedBy.Employee, () => inspectedByEmployee, JoinType.LeftOuterJoin);

            ValveWorkOrderRequest wor1 = null;
            query.JoinAlias(x => x.WorkOrderRequestOne, () => wor1, JoinType.LeftOuterJoin);

            Coordinate coord = null;
            query.JoinAlias(x => val.Coordinate, () => coord, JoinType.LeftOuterJoin);

            FunctionalLocation functionalLocation = null;
            query.JoinAlias(x => val.FunctionalLocation, () => functionalLocation, JoinType.LeftOuterJoin);

            ValveSize valveSize = null;
            query.JoinAlias(x => val.ValveSize, () => valveSize, JoinType.LeftOuterJoin);

            ValveNormalPosition positionFound = null, positionLeft = null;
            query.JoinAlias(x => x.PositionFound, () => positionFound, JoinType.LeftOuterJoin);
            query.JoinAlias(x => x.PositionLeft, () => positionLeft, JoinType.LeftOuterJoin);

            ValveZone zone = null;
            query.JoinAlias(() => val.ValveZone, () => zone, JoinType.LeftOuterJoin);

            if (!CurrentUserCanAccessAllTheRecords)
            {
                query.Where(Restrictions.On<ValveInspection>(x => opc.Id).IsIn(GetUserOperatingCenterIds()));
            }

            query.SelectList(x => x
                                 .Select(vi => vi.Id).WithAlias(() => result.Id)
                                 .Select(vi => val.Id).WithAlias(() => result.ValveId)
                                 .Select(vi => val.ValveNumber).WithAlias(() => result.ValveNumber)
                                 .Select(vi => opc.OperatingCenterCode).WithAlias(() => result.OperatingCenter)
                                 .Select(vi => town.ShortName).WithAlias(() => result.Town)
                                 .Select(vi => functionalLocation.Description)
                                 .WithAlias(() => result.FunctionalLocation)
                                 .Select(vi => val.SAPEquipmentId).WithAlias(() => result.SAPEquipmentId)
                                 .Select(vi => coord.Latitude).WithAlias(() => result.Latitude)
                                 .Select(vi => coord.Longitude).WithAlias(() => result.Longitude)
                                 .Select(vi => vi.DateInspected).WithAlias(() => result.DateInspected)
                                 .Select(vi => vi.Inspected).WithAlias(() => result.Inspected)
                                 .Select(vi => positionFound.Description).WithAlias(() => result.PositionFound)
                                 .Select(vi => positionLeft.Description).WithAlias(() => result.PositionLeft)
                                 .Select(vi => vi.Turns).WithAlias(() => result.Turns)
                                 .Select(vi => vi.Remarks).WithAlias(() => result.Remarks)
                                 .Select(vi => inspectedBy.UserName).WithAlias(() => result.InspectedBy)
                                 .Select(vi => inspectedByEmployee.EmployeeId).WithAlias(() => result.EmployeeId)
                                 .Select(vi => vi.CreatedAt).WithAlias(() => result.DateAdded)
                                 .Select(vi => valveSize.Size).WithAlias(() => result.ValveSize)
                                 .Select(() => zone.Description).WithAlias(() => result.ValveZone)
                                 .Select(vi => vi.SAPErrorCode).WithAlias(() => result.SAPErrorCode)
                                 .Select(vi => vi.SAPNotificationNumber).WithAlias(() => result.SAPNotificationNumber)
            );

            query.TransformUsing(Transformers.AliasToBean<ValveInspectionSearchResultViewModel>());

            return Search(search, query);
        }

        public IEnumerable<InspectionProductivityReportItem> GetInspectionProductivityReport(
            ISearchInspectionProductivity search)
        {
            var startDate = search.StartDate.Value.Date;
            var endDate = startDate.AddDays(search.GetDays());

            search.EnablePaging = false;
            var query = Session.QueryOver<ValveInspection>();
            InspectionProductivityReportItem result = null;
            Valve asset = null;
            query.JoinAlias(x => x.Valve, () => asset);
            OperatingCenter opc = null;
            query.JoinAlias(x => asset.OperatingCenter, () => opc);
            ValveSize valveSize = null;
            query.JoinAlias(() => asset.ValveSize, () => valveSize);
            User inspectedBy = null;
            query.JoinAlias(x => x.InspectedBy, () => inspectedBy);

            query.Where(x => x.DateInspected >= startDate && x.DateInspected < endDate);

            query.SelectList(x => x.SelectGroup(y => opc.OperatingCenterCode).WithAlias(() => result.OperatingCenter)
                                   .SelectGroup(y => inspectedBy.FullName).WithAlias(() => result.InspectedBy)
                                   .SelectGroup(() => valveSize.SizeRange).WithAlias(() => result.ValveSize)
                                   .SelectGroup(y => y.Inspected).WithAlias(() => result.ValveOperated)
                                   .Select(() => "Valve").WithAlias(() => result.AssetType)
                                    // NHibernate is garbage and you can't use the DateTime.YearPart/MonthPart/DayPart methods in a SelectGroup.
                                   .Select(Projections.SqlGroupProjection("YEAR(DateInspected) as DateInspectedYear",
                                        "YEAR(DateInspected)", new[] {"DateInspectedYear"},
                                        new IType[] {NHibernateUtil.Int32})).WithAlias(() => result.DateInspectedYear)
                                   .Select(Projections.SqlGroupProjection("MONTH(DateInspected) as DateInspectedMonth",
                                        "MONTH(DateInspected)", new[] {"DateInspectedMonth"},
                                        new IType[] {NHibernateUtil.Int32})).WithAlias(() => result.DateInspectedMonth)
                                   .Select(Projections.SqlGroupProjection("DAY(DateInspected) as DateInspectedDay",
                                        "DAY(DateInspected)", new[] {"DateInspectedDay"},
                                        new IType[] {NHibernateUtil.Int32})).WithAlias(() => result.DateInspectedDay)
                                   .SelectCount(y => y.Id).WithAlias(() => result.Count));

            query.OrderBy(() => opc.OperatingCenterCode).Asc();
            query.OrderBy(() => inspectedBy.FullName).Asc();
            query.OrderBy(() => valveSize.SizeRange).Asc();

            query.TransformUsing(Transformers.AliasToBean<InspectionProductivityReportItem>());
            var ret = Search(search, query);
            return ret;
        }

        public IEnumerable<AssetCoordinate> SearchValveInspectionsForMap(ISearchValveInspection search)
        {
            // This method's for maps only so we don't want paging.
            // Also the sorting is being hardcoded because the maps 

            var query = Session.QueryOver<ValveInspection>();
            // Setup aliases
            Valve val = null;
            query.JoinAlias(x => x.Valve, () => val, JoinType.LeftOuterJoin);
            OperatingCenter opc = null;
            query.JoinAlias(x => val.OperatingCenter, () => opc, JoinType.LeftOuterJoin);
            Town town = null;
            query.JoinAlias(x => val.Town, () => town, JoinType.LeftOuterJoin);
            User inspectedBy = null;
            query.JoinAlias(x => x.InspectedBy, () => inspectedBy, JoinType.LeftOuterJoin);
            ValveWorkOrderRequest wor1 = null;
            query.JoinAlias(x => x.WorkOrderRequestOne, () => wor1, JoinType.LeftOuterJoin);
            Coordinate coord = null;
            query.JoinAlias(x => val.Coordinate, () => coord, JoinType.LeftOuterJoin);
            FunctionalLocation functionalLocation = null;
            query.JoinAlias(x => val.FunctionalLocation, () => functionalLocation, JoinType.LeftOuterJoin);
            ValveSize valveSize = null;
            query.JoinAlias(x => val.ValveSize, () => valveSize, JoinType.LeftOuterJoin);
            ValveNormalPosition positionFound = null, positionLeft = null;
            query.JoinAlias(x => x.PositionFound, () => positionFound, JoinType.LeftOuterJoin);
            query.JoinAlias(x => x.PositionLeft, () => positionLeft, JoinType.LeftOuterJoin);
            ValveZone zone = null;
            query.JoinAlias(() => val.ValveZone, () => zone, JoinType.LeftOuterJoin);

            if (!CurrentUserCanAccessAllTheRecords)
            {
                query.Where(Restrictions.On<ValveInspection>(x => opc.Id).IsIn(GetUserOperatingCenterIds()));
            }

            // Get all the search params mapped correctly.
            ApplySearchMapping(search, query.RootCriteria);
            ApplySearchSorting(search, query.RootCriteria);

            // Okay, NHibernate is seriously just garbage here. QueryOver does not support selecting only a joined
            // entity. You end up having to do a subquery. But the subquery will not work if you need to sort the
            // parent entity(SQL Server no likey). So in order to do this you have to do a second query to select
            // the valves. 

            query.Select(x => val.Id);

            // Need the ToList after the List because Restrictions won't accept an IList<int>.
            var valveIds = query.List<int>().Distinct().ToList();
            var valves = new List<Valve>();

            // It's actually 2100 parameters, but leaving a lot of room for additional search parameters
            // that could be populated here.
            const int MAX_PARAMETERS_ALLOWED = 2000;

            for (var i = 0; i < valveIds.Count; i += MAX_PARAMETERS_ALLOWED)
            {
                var v = Session.QueryOver<Valve>()
                               .JoinAlias(x => x.Coordinate, () => coord, JoinType.LeftOuterJoin)
                               .Where(Restrictions.In("Id", valveIds.Skip(i).Take(MAX_PARAMETERS_ALLOWED).ToList()))
                               .List<Valve>();
                valves.AddRange(v);
            }

            var valvesById = valves.ToDictionary(x => x.Id, x => x);

            return valveIds.Select(x => valvesById[x].ToAssetCoordinate()).ToList();
        }

        public IEnumerable<ValveInspection> GetValveInspectionsWithSapRetryIssues()
        {
            return this.GetValveInspectionsWithSapRetryIssuesImpl();
        }

        #endregion

        #region Reports

        /// <summary>
        /// Allowed to skip over filtering of operating centers
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> GetDistinctYearsCompleted()
        {
            return
                (from vi in base.Linq
                 select vi.DateInspected.Year
                ).Distinct();
        }

        #region ValveInspectionsByMonth

        // Distinct Inspections that were required to be inspected.
        // If one was performed later in the year only that one is returned
        internal IEnumerable<ValveInspectionsByMonthReportItem> GetValveInspectionsByMonthReportItems(
            ISearchValveInspectionsByMonthReport search)
        {
            var year = search.Year.GetValueOrDefault(DateTime.Today.Year);

            // (somewhat)static counts
            var smallValvesDueInspectionTotal =
                _valveRepository.GetCountOfInspectionsRequiredForYear(search.Year.Value, false, search.OperatingCenter)
                                .ToList();
            var smallValvesTotal =
                _valveRepository.GetCountOfExistingValvesThatAreInspectableForYear(search.Year.Value, false,
                                     search.OperatingCenter)
                                .ToList();
            var largeValvesDueInspectionTotal =
                _valveRepository.GetCountOfInspectionsRequiredForYear(search.Year.Value, true, search.OperatingCenter)
                                .ToList();
            var largeValvesTotal =
                _valveRepository.GetCountOfExistingValvesThatAreInspectableForYear(search.Year.Value, true,
                                     search.OperatingCenter)
                                .ToList();

            var sqlite = ConfigurationManager.AppSettings["DatabaseType"] != null &&
                         ConfigurationManager.AppSettings["DatabaseType"].ToLower() == "sqlite";
            var requiresInspection = String.Format(
                (sqlite)
                    ? ValveMap.Sql.REQUIRES_INSPECTION_FORMAT_STRING_SQLITE
                    : ValveMap.Sql.REQUIRES_INSPECTION_FORMAT_STRING + " = 1",
                "Size",
                year,
                string.Empty,
                "UsesValveInspectionFrequency");
            var operatingCenters = string.Join(", ", search.OperatingCenter.Select(x => x.ToString()));
            var query = Session.CreateSQLQuery($@"
WITH viMax AS (
    select max(viMax.Id) as Id from valveInspections viMax inner join Valves v on v.Id = viMax.ValveID where v.OperatingCenterId in ({operatingCenters}) and year(viMax.dateinspected) = {year} and Inspected = 1 group by viMax.valveID    
)
SELECT	month(this_.DateInspected) as Month,
		count(month(this_.DateInspected)) as TotalInspected,
		count(distinct this_.ValveId) as TotalDistinctValvesInspected,
		year(DateInspected) as Year,
		(CASE WHEN (Size >= 12.0) THEN '>= 12' ELSE '< 12' END) as SizeRange,
		oc2_.OperatingCenterCode as OperatingCenter
FROM ValveInspections this_
INNER JOIN Valves v1_ on this_.ValveId=v1_.Id
INNER JOIN OperatingCenters oc2_ on v1_.OperatingCenterId=oc2_.OperatingCenterID
INNER JOIN ValveSizes vs3_ on v1_.ValveSizeId=vs3_.Id
WHERE year(this_.DateInspected) = {year}
AND oc2_.OperatingCenterID in ({operatingCenters})
AND this_.Inspected = 1
AND {requiresInspection}
AND EXISTS (SELECT 1 FROM viMax WHERE viMax.Id = this_.id)
AND (oc2_.OperatingCenterID IN ({operatingCenters}))
GROUP BY month(DateInspected), year(DateInspected), (CASE WHEN (Size >= 12.0) THEN '>= 12' ELSE '< 12' END), oc2_.OperatingCenterCode
ORDER BY 1 asc, 5 desc");

            query.SetResultTransformer(Transformers.AliasToBean(typeof(ValveInspectionsByMonthReportItem)));

            var result = query.List<ValveInspectionsByMonthReportItem>();

            foreach (var thingy in result)
            {
                thingy.TotalValves =
                    (thingy.SizeRange == Valve.Display.SIZE_RANGE_SMALL_VALVE ? smallValvesTotal : largeValvesTotal)
                   .Single(count => count.OperatingCenter == thingy.OperatingCenter).Count;
                thingy.TotalRequired =
                    (thingy.SizeRange == Valve.Display.SIZE_RANGE_SMALL_VALVE
                        ? smallValvesDueInspectionTotal
                        : largeValvesDueInspectionTotal)
                   .Single(count => count.OperatingCenter == thingy.OperatingCenter).Count;
            }

            return result.OrderBy(i => i.OperatingCenter);
        }

        public IEnumerable<ValveInspectionsByMonthReport> GetValveInspectionsByMonthReport(
            ISearchValveInspectionsByMonthReport search)
        {
            var results = GetValveInspectionsByMonthReportItems(search);
            // ReSharper disable PossibleMultipleEnumeration
            if (!results.Any())
            {
                return Enumerable.Empty<ValveInspectionsByMonthReport>();
            }

            var report = new List<ValveInspectionsByMonthReport>();

            var sizeRanges = results.Select(x => x.SizeRange).Distinct().OrderBy(x => x);
            //  var firstResult = results.First();
            var opCenters = results.Select(x => x.OperatingCenter).Distinct().ToArray();
            // var opCntr = firstResult.OperatingCenter;
            var year = search.Year.Value;

            var totalJan = 0;
            var totalFeb = 0;
            var totalMar = 0;
            var totalApr = 0;
            var totalMay = 0;
            var totalJun = 0;
            var totalJul = 0;
            var totalAug = 0;
            var totalSep = 0;
            var totalOct = 0;
            var totalNov = 0;
            var totalDec = 0;

            // var totalDue = 0;
            var totalRequired = (long)0;

            foreach (var opCntr in opCenters)
            {
                var rowsWithOpCenter = results.Where(x => x.OperatingCenter == opCntr).ToList();

                foreach (var sizeRange in sizeRanges)
                {
                    var rowResult = rowsWithOpCenter.Where(x => x.SizeRange == sizeRange).ToList();
                    var required = (rowResult.Count > 0) ? rowResult.First().TotalRequired : 0;
                    totalRequired += required;
                    // totalDue += totalRequired;
                    if (rowResult.Any())
                    {
                        Func<int, int> getCount = (month) => {
                            var first = rowResult.FirstOrDefault(x => x.Month == month);
                            return (first != null) ? (int)first.TotalInspected : 0;
                        };

                        totalJan += getCount(1);
                        totalFeb += getCount(2);
                        totalMar += getCount(3);
                        totalApr += getCount(4);
                        totalMay += getCount(5);
                        totalJun += getCount(6);
                        totalJul += getCount(7);
                        totalAug += getCount(8);
                        totalSep += getCount(9);
                        totalOct += getCount(10);
                        totalNov += getCount(11);
                        totalDec += getCount(12);

                        report.Add(new ValveInspectionsByMonthReport {
                            SizeRange = sizeRange,
                            OperatingCenter = opCntr,
                            Year = year,
                            Jan = getCount(1),
                            Feb = getCount(2),
                            Mar = getCount(3),
                            Apr = getCount(4),
                            May = getCount(5),
                            Jun = getCount(6),
                            Jul = getCount(7),
                            Aug = getCount(8),
                            Sep = getCount(9),
                            Oct = getCount(10),
                            Nov = getCount(11),
                            Dec = getCount(12),
                            TotalRequired = required
                        });
                    }
                }
            }

            report.Add(new ValveInspectionsByMonthReport {
                SizeRange = "Total",
                Year = year,
                Jan = totalJan,
                Feb = totalFeb,
                Mar = totalMar,
                Apr = totalApr,
                May = totalMay,
                Jun = totalJun,
                Jul = totalJul,
                Aug = totalAug,
                Sep = totalSep,
                Oct = totalOct,
                Nov = totalNov,
                Dec = totalDec,
                TotalRequired = totalRequired
            });

            return report;
            // ReSharper restore PossibleMultipleEnumeration
        }

        #endregion

        #region RequiredValvesOperatedByMonth

        // Inspections for valves that were required, and had in inspection regardless of of it was operated or not
        public IEnumerable<RequiredValvesOperatedByMonthReportItem> GetRequiredValvesOperatedByMonthReportItems(
            ISearchRequiredValvesOperatedByMonthReport search)
        {
            search.EnablePaging = false;
            var query = Session.QueryOver<ValveInspection>();

            //joins
            Valve v = null;
            query.JoinAlias(x => x.Valve, () => v);
            OperatingCenter oc = null;
            query.JoinAlias(x => v.OperatingCenter, () => oc);
            ValveSize vs = null;
            query.JoinAlias(x => v.ValveSize, () => vs);

            //search params
            var year = search.Year.GetValueOrDefault(DateTime.Today.Year);
            query.Where(x => x.DateInspected.Year == year);
            query.WhereRestrictionOn(() => oc.Id).IsIn(search.OperatingCenter);
            //query.Where(x => oc.Id == search.OperatingCenter);
            var sqlite = ConfigurationManager.AppSettings["DatabaseType"]?.ToLower() == "sqlite";
            query.Where(
                Expression.Sql(
                    String.Format(
                        (sqlite)
                            ? ValveMap.Sql.REQUIRES_INSPECTION_FORMAT_STRING_SQLITE
                            : ValveMap.Sql.REQUIRES_INSPECTION_FORMAT_STRING + " = 1",
                        "Size",
                        year,
                        string.Empty,
                        "UsesValveInspectionFrequency")));
            //group
            RequiredValvesOperatedByMonthReportItem report = null;
            query.SelectList(x => x
                                 .Select(Projections.SqlGroupProjection("MONTH(DateInspected) as MonthCompleted",
                                      "MONTH(DateInspected)", new[] {"MonthCompleted"},
                                      new IType[] {NHibernateUtil.Int32})).WithAlias(() => report.Month)
                                 .SelectCount(y => y.DateInspected.Month).WithAlias(() => report.TotalInspected)
                                 .Select(Projections.SqlGroupProjection("YEAR(DateInspected) as Year",
                                      "YEAR(DateInspected)", new[] {"Year"}, new IType[] {NHibernateUtil.Int32}))
                                 .WithAlias(() => report.Year)
                                 .SelectGroup(y => vs.SizeRange).WithAlias(() => report.SizeRange)
                                 .SelectGroup(y => oc.OperatingCenterCode).WithAlias(() => report.OperatingCenter)
                                 .SelectGroup(y => y.Inspected).WithAlias(() => report.Operated)
            );
            // sort
            query.UnderlyingCriteria.AddOrder(new Order("Month", true));
            query
               .OrderBy(x => vs.SizeRange).Desc()
               .OrderBy(x => x.Inspected);

            query.TransformUsing(Transformers.AliasToBean<RequiredValvesOperatedByMonthReportItem>());

            return Search(search, query);
        }

        public IEnumerable<RequiredValvesOperatedByMonthReport> GetRequiredValvesOperatedByMonthReport(
            ISearchRequiredValvesOperatedByMonthReport search)
        {
            var report = new List<RequiredValvesOperatedByMonthReport>();
            var results = GetRequiredValvesOperatedByMonthReportItems(search);
            if (!results.Any())
                return report;

            var sizeRanges = results.Select(x => x.SizeRange).Distinct().OrderBy(x => x);
            var opCntr = results.First().OperatingCenter;
            var year = results.First().Year;
            var operateds = results.Select(x => x.Operated).Distinct().OrderByDescending(x => x);

            var totalJan = (long)0;
            var totalFeb = (long)0;
            var totalMar = (long)0;
            var totalApr = (long)0;
            var totalMay = (long)0;
            var totalJun = (long)0;
            var totalJul = (long)0;
            var totalAug = (long)0;
            var totalSep = (long)0;
            var totalOct = (long)0;
            var totalNov = (long)0;
            var totalDec = (long)0;
            var subTotalJan = (long)0;
            var subTotalFeb = (long)0;
            var subTotalMar = (long)0;
            var subTotalApr = (long)0;
            var subTotalMay = (long)0;
            var subTotalJun = (long)0;
            var subTotalJul = (long)0;
            var subTotalAug = (long)0;
            var subTotalSep = (long)0;
            var subTotalOct = (long)0;
            var subTotalNov = (long)0;
            var subTotalDec = (long)0;
            var currentJan = (long)0;
            var currentFeb = (long)0;
            var currentMar = (long)0;
            var currentApr = (long)0;
            var currentMay = (long)0;
            var currentJun = (long)0;
            var currentJul = (long)0;
            var currentAug = (long)0;
            var currentSep = (long)0;
            var currentOct = (long)0;
            var currentNov = (long)0;
            var currentDec = (long)0;

            foreach (var sizeRange in sizeRanges)
            {
                subTotalJan = 0;
                subTotalFeb = 0;
                subTotalMar = 0;
                subTotalApr = 0;
                subTotalMay = 0;
                subTotalJun = 0;
                subTotalJul = 0;
                subTotalAug = 0;
                subTotalSep = 0;
                subTotalOct = 0;
                subTotalNov = 0;
                subTotalDec = 0;

                foreach (var operated in operateds)
                {
                    var rowResult = results.Where(x => x.SizeRange == sizeRange && x.Operated == operated);
                    if (rowResult.Any())
                    {
                        Func<int, long> getCount = (month) => {
                            var first = rowResult.FirstOrDefault(x => x.Month == month);
                            return (first != null) ? (long)first.GetPropertyValueByName("TotalInspected") : 0;
                        };
                        currentJan = getCount(1);
                        currentFeb = getCount(2);
                        currentMar = getCount(3);
                        currentApr = getCount(4);
                        currentMay = getCount(5);
                        currentJun = getCount(6);
                        currentJul = getCount(7);
                        currentAug = getCount(8);
                        currentSep = getCount(9);
                        currentOct = getCount(10);
                        currentNov = getCount(11);
                        currentDec = getCount(12);

                        totalJan += currentJan;
                        totalFeb += currentFeb;
                        totalMar += currentMar;
                        totalApr += currentApr;
                        totalMay += currentMay;
                        totalJun += currentJun;
                        totalJul += currentJul;
                        totalAug += currentAug;
                        totalSep += currentSep;
                        totalOct += currentOct;
                        totalNov += currentNov;
                        totalDec += currentDec;

                        subTotalJan += currentJan;
                        subTotalFeb += currentFeb;
                        subTotalMar += currentMar;
                        subTotalApr += currentApr;
                        subTotalMay += currentMay;
                        subTotalJun += currentJun;
                        subTotalJul += currentJul;
                        subTotalAug += currentAug;
                        subTotalSep += currentSep;
                        subTotalOct += currentOct;
                        subTotalNov += currentNov;
                        subTotalDec += currentDec;

                        report.Add(new RequiredValvesOperatedByMonthReport {
                            SizeRange = sizeRange,
                            OperatingCenter = opCntr,
                            Year = year,
                            Operated = (operated) ? "YES" : "NO",
                            Jan = currentJan,
                            Feb = currentFeb,
                            Mar = currentMar,
                            Apr = currentApr,
                            May = currentMay,
                            Jun = currentJun,
                            Jul = currentJul,
                            Aug = currentAug,
                            Sep = currentSep,
                            Oct = currentOct,
                            Nov = currentNov,
                            Dec = currentDec
                        });
                    }
                }

                report.Add(new RequiredValvesOperatedByMonthReport {
                    SizeRange = sizeRange,
                    OperatingCenter = opCntr,
                    Year = year,
                    Operated = "Total",
                    Jan = subTotalJan,
                    Feb = subTotalFeb,
                    Mar = subTotalMar,
                    Apr = subTotalApr,
                    May = subTotalMay,
                    Jun = subTotalJun,
                    Jul = subTotalJul,
                    Aug = subTotalAug,
                    Sep = subTotalSep,
                    Oct = subTotalOct,
                    Nov = subTotalNov,
                    Dec = subTotalDec
                });
            }

            report.Add(new RequiredValvesOperatedByMonthReport {
                SizeRange = "Total",
                OperatingCenter = opCntr,
                Year = year,
                Jan = totalJan,
                Feb = totalFeb,
                Mar = totalMar,
                Apr = totalApr,
                May = totalMay,
                Jun = totalJun,
                Jul = totalJul,
                Aug = totalAug,
                Sep = totalSep,
                Oct = totalOct,
                Nov = totalNov,
                Dec = totalDec
            });
            return report;
        }

        #endregion

        #region ValvesOperatedByMonth

        // All valves inspected regardless of requirement
        public IEnumerable<ValvesOperatedByMonthReportItem> GetValvesOperatedByMonthReportItems(
            ISearchValvesOperatedByMonthReport search)
        {
            search.EnablePaging = false;
            var query = Session.QueryOver<ValveInspection>();

            //joins
            Valve v = null;
            query.JoinAlias(x => x.Valve, () => v);
            OperatingCenter oc = null;
            query.JoinAlias(x => v.OperatingCenter, () => oc);
            ValveSize vs = null;
            query.Left.JoinAlias(x => v.ValveSize, () => vs);

            //search params
            var year = search.Year.GetValueOrDefault(DateTime.Today.Year);
            query.Where(x => x.DateInspected.Year == year);
            query.WhereRestrictionOn(() => oc.Id).IsIn(search.OperatingCenter);
            //query.Where(x => oc.Id == search.OperatingCenter);
            //group
            ValvesOperatedByMonthReportItem report = null;
            query.SelectList(x => x
                                 .Select(Projections.SqlGroupProjection("MONTH(DateInspected) as MonthCompleted",
                                      "MONTH(DateInspected)", new[] {"MonthCompleted"},
                                      new IType[] {NHibernateUtil.Int32})).WithAlias(() => report.Month)
                                 .SelectCount(y => y.DateInspected.Month).WithAlias(() => report.TotalInspected)
                                 .Select(Projections.SqlGroupProjection("YEAR(DateInspected) as Year",
                                      "YEAR(DateInspected)", new[] {"Year"}, new IType[] {NHibernateUtil.Int32}))
                                 .WithAlias(() => report.Year)
                                 .SelectGroup(y => vs.SizeRange).WithAlias(() => report.SizeRange)
                                 .SelectGroup(y => oc.OperatingCenterCode).WithAlias(() => report.OperatingCenter)
                                 .SelectGroup(y => y.Inspected).WithAlias(() => report.Operated)
            );
            // sort
            query.UnderlyingCriteria.AddOrder(new Order("Month", true));
            query
               .OrderBy(x => vs.SizeRange).Desc()
               .OrderBy(x => x.Inspected).Desc();

            query.TransformUsing(Transformers.AliasToBean<ValvesOperatedByMonthReportItem>());

            return Search(search, query);
        }

        public IEnumerable<ValvesOperatedByMonthReport> GetValvesOperatedByMonthReport(
            ISearchValvesOperatedByMonthReport search)
        {
            var report = new List<ValvesOperatedByMonthReport>();
            var results = GetValvesOperatedByMonthReportItems(search);
            if (!results.Any())
                return report;
            var sizeRanges = results.Select(x => x.SizeRange).Distinct().OrderBy(x => x);
            var opCntr = string.Join(",",
                results.OrderBy(x => x.OperatingCenter).Select(x => x.OperatingCenter)
                       .Distinct());
            var year = results.First().Year;
            var operateds = results.Select(x => x.Operated).Distinct().OrderByDescending(x => x);

            var totalJan = (long)0;
            var totalFeb = (long)0;
            var totalMar = (long)0;
            var totalApr = (long)0;
            var totalMay = (long)0;
            var totalJun = (long)0;
            var totalJul = (long)0;
            var totalAug = (long)0;
            var totalSep = (long)0;
            var totalOct = (long)0;
            var totalNov = (long)0;
            var totalDec = (long)0;
            var subTotalJan = (long)0;
            var subTotalFeb = (long)0;
            var subTotalMar = (long)0;
            var subTotalApr = (long)0;
            var subTotalMay = (long)0;
            var subTotalJun = (long)0;
            var subTotalJul = (long)0;
            var subTotalAug = (long)0;
            var subTotalSep = (long)0;
            var subTotalOct = (long)0;
            var subTotalNov = (long)0;
            var subTotalDec = (long)0;
            var currentJan = (long)0;
            var currentFeb = (long)0;
            var currentMar = (long)0;
            var currentApr = (long)0;
            var currentMay = (long)0;
            var currentJun = (long)0;
            var currentJul = (long)0;
            var currentAug = (long)0;
            var currentSep = (long)0;
            var currentOct = (long)0;
            var currentNov = (long)0;
            var currentDec = (long)0;

            foreach (var sizeRange in sizeRanges)
            {
                subTotalJan = 0;
                subTotalFeb = 0;
                subTotalMar = 0;
                subTotalApr = 0;
                subTotalMay = 0;
                subTotalJun = 0;
                subTotalJul = 0;
                subTotalAug = 0;
                subTotalSep = 0;
                subTotalOct = 0;
                subTotalNov = 0;
                subTotalDec = 0;

                foreach (var operated in operateds)
                {
                    var rowResult = results.Where(x => x.SizeRange == sizeRange && x.Operated == operated);
                    if (rowResult.Any())
                    {
                        Func<int, long> getCount = (month) => {
                            var allRows = rowResult.Where(x => x.Month == month);
                            return (allRows.Any()) ? allRows.Sum(x => x.TotalInspected) : 0;
                        };
                        currentJan = getCount(1);
                        currentFeb = getCount(2);
                        currentMar = getCount(3);
                        currentApr = getCount(4);
                        currentMay = getCount(5);
                        currentJun = getCount(6);
                        currentJul = getCount(7);
                        currentAug = getCount(8);
                        currentSep = getCount(9);
                        currentOct = getCount(10);
                        currentNov = getCount(11);
                        currentDec = getCount(12);

                        totalJan += currentJan;
                        totalFeb += currentFeb;
                        totalMar += currentMar;
                        totalApr += currentApr;
                        totalMay += currentMay;
                        totalJun += currentJun;
                        totalJul += currentJul;
                        totalAug += currentAug;
                        totalSep += currentSep;
                        totalOct += currentOct;
                        totalNov += currentNov;
                        totalDec += currentDec;

                        subTotalJan += currentJan;
                        subTotalFeb += currentFeb;
                        subTotalMar += currentMar;
                        subTotalApr += currentApr;
                        subTotalMay += currentMay;
                        subTotalJun += currentJun;
                        subTotalJul += currentJul;
                        subTotalAug += currentAug;
                        subTotalSep += currentSep;
                        subTotalOct += currentOct;
                        subTotalNov += currentNov;
                        subTotalDec += currentDec;

                        report.Add(new ValvesOperatedByMonthReport {
                            SizeRange = sizeRange,
                            OperatingCenter = opCntr,
                            Year = year,
                            Operated = (operated) ? "YES" : "NO",
                            Jan = currentJan,
                            Feb = currentFeb,
                            Mar = currentMar,
                            Apr = currentApr,
                            May = currentMay,
                            Jun = currentJun,
                            Jul = currentJul,
                            Aug = currentAug,
                            Sep = currentSep,
                            Oct = currentOct,
                            Nov = currentNov,
                            Dec = currentDec
                        });
                    }
                }

                report.Add(new ValvesOperatedByMonthReport {
                    SizeRange = sizeRange,
                    OperatingCenter = opCntr,
                    Year = year,
                    Operated = "Total",
                    Jan = subTotalJan,
                    Feb = subTotalFeb,
                    Mar = subTotalMar,
                    Apr = subTotalApr,
                    May = subTotalMay,
                    Jun = subTotalJun,
                    Jul = subTotalJul,
                    Aug = subTotalAug,
                    Sep = subTotalSep,
                    Oct = subTotalOct,
                    Nov = subTotalNov,
                    Dec = subTotalDec
                });
            }

            report.Add(new ValvesOperatedByMonthReport {
                SizeRange = "Total",
                OperatingCenter = opCntr,
                Year = year,
                Jan = totalJan,
                Feb = totalFeb,
                Mar = totalMar,
                Apr = totalApr,
                May = totalMay,
                Jun = totalJun,
                Jul = totalJul,
                Aug = totalAug,
                Sep = totalSep,
                Oct = totalOct,
                Nov = totalNov,
                Dec = totalDec
            });
            return report;
        }

        #endregion

        #endregion
    }

    public static class IValveInspectionRepositoryExtensions
    {
        public static IEnumerable<ValveInspection> GetValveInspectionsWithSapRetryIssuesImpl(
            this IRepository<ValveInspection> that)
        {
            return that.Where(x =>
                x.SAPErrorCode != null && x.SAPErrorCode != "" && x.SAPErrorCode.StartsWith("RETRY"));
        }
    }

    public interface IValveInspectionRepository : IRepository<ValveInspection>
    {
        IEnumerable<ValveInspection> GetValveInspectionsWithSapRetryIssues();
        IEnumerable<ValveInspectionSearchResultViewModel> SearchInspections(ISearchValveInspection search);

        IEnumerable<InspectionProductivityReportItem> GetInspectionProductivityReport(
            ISearchInspectionProductivity search);

        IEnumerable<int> GetDistinctYearsCompleted();

        IEnumerable<ValveInspectionsByMonthReport> GetValveInspectionsByMonthReport(
            ISearchValveInspectionsByMonthReport search);

        IEnumerable<ValvesOperatedByMonthReportItem> GetValvesOperatedByMonthReportItems(
            ISearchValvesOperatedByMonthReport search);

        IEnumerable<ValvesOperatedByMonthReport> GetValvesOperatedByMonthReport(
            ISearchValvesOperatedByMonthReport search);

        IEnumerable<RequiredValvesOperatedByMonthReportItem> GetRequiredValvesOperatedByMonthReportItems(
            ISearchRequiredValvesOperatedByMonthReport search);

        IEnumerable<RequiredValvesOperatedByMonthReport> GetRequiredValvesOperatedByMonthReport(
            ISearchRequiredValvesOperatedByMonthReport search);

        IEnumerable<AssetCoordinate> SearchValveInspectionsForMap(ISearchValveInspection search);
    }
}
