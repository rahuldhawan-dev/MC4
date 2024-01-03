using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Data;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using NHibernate.Type;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class HydrantInspectionRepository : MapCallSecuredRepositoryBase<HydrantInspection>,
        IHydrantInspectionRepository
    {
        #region Fields

        private RoleMatch _roleMatch;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IOperatingCenterRepository _operatingCenterRepository;
        private readonly IHydrantRepository _hydrantRepository;

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
                                  .CreateAlias("Hydrant", "h")
                                  .CreateAlias("h.OperatingCenter", "oc")
                                   // Really annoying that NHibernate can't figure out the proper join type based on mapping.
                                  .CreateAlias("h.Coordinate", "coord", JoinType.LeftOuterJoin);

                if (CurrentUserCanAccessAllTheRecords)
                {
                    return critter;
                }

                return critter.Add(Restrictions.In("oc.Id", GetUserOperatingCenterIds()));
            }
        }

        public override IQueryable<HydrantInspection> Linq
        {
            get
            {
                if (CurrentUserCanAccessAllTheRecords)
                {
                    return base.Linq;
                }

                var opCenterIds = GetUserOperatingCenterIds();
                return base.Linq.Where(x => opCenterIds.Contains(x.Hydrant.OperatingCenter.Id));
            }
        }

        public override RoleModules Role
        {
            get { return RoleModules.FieldServicesAssets; }
        }

        #endregion

        #region Constructor(s)

        public HydrantInspectionRepository(IRepository<AggregateRole> roleRepo, ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IDateTimeProvider dateTimeProvider,
            IOperatingCenterRepository operatingCenterRepository, IHydrantRepository hydrantRepository) : base(session,
            container,
            authenticationService, roleRepo)
        {
            _dateTimeProvider = dateTimeProvider;
            _operatingCenterRepository = operatingCenterRepository;
            _hydrantRepository = hydrantRepository;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Use this method over the regular Search method. This uses a view model to reduce the amount of query noise
        /// being generated.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public IEnumerable<HydrantInspectionSearchResultViewModel> SearchInspections(ISearchHydrantInspection search)
        {
            var query = Session.QueryOver<HydrantInspection>();
            // Setup aliases
            HydrantInspectionSearchResultViewModel result = null;
            Hydrant hyd = null;
            query.JoinAlias(x => x.Hydrant, () => hyd, JoinType.LeftOuterJoin);
            OperatingCenter opc = null;
            query.JoinAlias(x => hyd.OperatingCenter, () => opc, JoinType.LeftOuterJoin);
            Town town = null;
            query.JoinAlias(x => hyd.Town, () => town, JoinType.LeftOuterJoin);
            FunctionalLocation functionalLocation = null;
            query.JoinAlias(x => hyd.FunctionalLocation, () => functionalLocation, JoinType.LeftOuterJoin);
            User inspectedBy = null;
            query.JoinAlias(x => x.InspectedBy, () => inspectedBy, JoinType.LeftOuterJoin);
            HydrantInspectionType hit = null;
            query.JoinAlias(x => x.HydrantInspectionType, () => hit, JoinType.LeftOuterJoin);
            HydrantTagStatus tagStat = null;
            query.JoinAlias(x => x.HydrantTagStatus, () => tagStat, JoinType.LeftOuterJoin);
            WorkOrderRequest wor1 = null;
            query.JoinAlias(x => x.WorkOrderRequestOne, () => wor1, JoinType.LeftOuterJoin);
            Coordinate coord = null;
            query.JoinAlias(x => hyd.Coordinate, () => coord, JoinType.LeftOuterJoin);
            NoReadReason freeNoRead = null;
            query.JoinAlias(x => x.FreeNoReadReason, () => freeNoRead, JoinType.LeftOuterJoin);
            NoReadReason totalNoRead = null;
            query.JoinAlias(x => x.TotalNoReadReason, () => totalNoRead, JoinType.LeftOuterJoin);

            if (!CurrentUserCanAccessAllTheRecords)
            {
                query.Where(Restrictions.On<HydrantInspection>(x => opc.Id).IsIn(GetUserOperatingCenterIds()));
            }

            query.SelectList(x => x.Select(hi => hi.Id).WithAlias(() => result.Id)
                                   .Select(hi => hi.DateInspected).WithAlias(() => result.DateInspected)
                                   .Select(hi => hi.CreatedAt).WithAlias(() => result.DateAdded)
                                   .Select(hi => hi.Remarks).WithAlias(() => result.Remarks)
                                   .Select(hi => hi.GallonsFlowed).WithAlias(() => result.GallonsFlowed)
                                   .Select(hi => hi.FullFlow).WithAlias(() => result.FullFlow)
                                   .Select(hi => hi.GPM).WithAlias(() => result.GPM)
                                   .Select(hi => hi.MinutesFlowed).WithAlias(() => result.MinutesFlowed)
                                   .Select(hi => hi.PreResidualChlorine).WithAlias(() => result.PreResidualChlorine)
                                   .Select(hi => hi.ResidualChlorine).WithAlias(() => result.ResidualChlorine)
                                   .Select(hi => hi.StaticPressure).WithAlias(() => result.StaticPressure)
                                   .Select(hi => hi.PreTotalChlorine).WithAlias(() => result.PreTotalChlorine)
                                   .Select(hi => hi.TotalChlorine).WithAlias(() => result.TotalChlorine)
                                   .Select(hi => hyd.Id).WithAlias(() => result.HydrantId)
                                   .Select(hi => hyd.HydrantNumber).WithAlias(() => result.HydrantNumber)
                                   .Select(hi => coord.Latitude).WithAlias(() => result.Latitude)
                                   .Select(hi => coord.Longitude).WithAlias(() => result.Longitude)
                                   .Select(hi => opc.OperatingCenterCode).WithAlias(() => result.OperatingCenter)
                                   .Select(hi => town.ShortName).WithAlias(() => result.Town)
                                   .Select(hi => functionalLocation.Description)
                                   .WithAlias(() => result.FunctionalLocation)
                                   .Select(hi => hyd.SAPEquipmentId).WithAlias(() => result.SAPEquipmentId)
                                   .Select(hi => inspectedBy.UserName).WithAlias(() => result.InspectedBy)
                                   .Select(hi => tagStat.Description).WithAlias(() => result.HydrantTagStatus)
                                   .Select(hi => hit.Description).WithAlias(() => result.HydrantInspectionType)
                                   .Select(hi => wor1.Description).WithAlias(() => result.WorkOrderRequestOne)
                                   .Select(hi => hi.SAPErrorCode).WithAlias(() => result.SAPErrorCode)
                                   .Select(hi => hi.SAPNotificationNumber).WithAlias(() => result.SAPNotificationNumber)
                                   .Select(_ => freeNoRead.Description).WithAlias(() => result.FreeNoReadReason)
                                   .Select(_ => totalNoRead.Description).WithAlias(() => result.TotalNoReadReason)
            );

            query.TransformUsing(Transformers.AliasToBean<HydrantInspectionSearchResultViewModel>());

            return Search(search, query);
        }

        public IEnumerable<AssetCoordinate> SearchHydrantInspectionsForMap(ISearchHydrantInspection search)
        {
            // This method's for maps only so we don't want paging.
            // Also the sorting is being hardcoded because the maps 

            var query = Session.QueryOver<HydrantInspection>();
            // Setup aliases
            Hydrant hyd = null;
            query.JoinAlias(x => x.Hydrant, () => hyd, JoinType.LeftOuterJoin);
            OperatingCenter opc = null;
            query.JoinAlias(x => hyd.OperatingCenter, () => opc, JoinType.LeftOuterJoin);
            Town town = null;
            query.JoinAlias(x => hyd.Town, () => town, JoinType.LeftOuterJoin);
            User inspectedBy = null;
            query.JoinAlias(x => x.InspectedBy, () => inspectedBy, JoinType.LeftOuterJoin);
            HydrantInspectionType hit = null;
            query.JoinAlias(x => x.HydrantInspectionType, () => hit, JoinType.LeftOuterJoin);
            WorkOrderRequest wor1 = null;
            query.JoinAlias(x => x.WorkOrderRequestOne, () => wor1, JoinType.LeftOuterJoin);
            Coordinate coord = null;
            query.JoinAlias(x => hyd.Coordinate, () => coord, JoinType.LeftOuterJoin);

            if (!CurrentUserCanAccessAllTheRecords)
            {
                query.Where(Restrictions.On<HydrantInspection>(x => opc.Id).IsIn(GetUserOperatingCenterIds()));
            }

            // Get all the search params mapped correctly.
            ApplySearchMapping(search, query.RootCriteria);
            ApplySearchSorting(search, query.RootCriteria);

            // Okay, NHibernate is seriously just garbage here. QueryOver does not support selecting only a joined
            // entity. You end up having to do a subquery. But the subquery will not work if you need to sort the
            // parent entity(SQL Server no likey). So in order to do this you have to do a second query to select
            // the hydrants. 

            query.Select(x => hyd.Id);

            // Need the ToList after the List because Restrictions won't accept an IList<int>.
            var hydrantIds = query.List<int>().Distinct().ToList();
            var hydrants = new List<Hydrant>();

            // It's actually 2100 parameters, but leaving a lot of room for additional search parameters
            // that could be populated here.
            const int MAX_PARAMETERS_ALLOWED = 2000;

            for (var i = 0; i < hydrantIds.Count; i += MAX_PARAMETERS_ALLOWED)
            {
                var h = Session.QueryOver<Hydrant>()
                               .JoinAlias(x => x.Coordinate, () => coord, JoinType.LeftOuterJoin)
                               .Where(Restrictions.In("Id", hydrantIds.Skip(i).Take(MAX_PARAMETERS_ALLOWED).ToList()))
                               .List<Hydrant>();
                hydrants.AddRange(h);
            }

            var hydrantsById = hydrants.ToDictionary(x => x.Id, x => x);

            return hydrantIds.Select(x => hydrantsById[x].ToAssetCoordinate()).ToList();
        }

        public IEnumerable<HydrantFlushingReportItem> GetFlushingReport(ISearchHydrantFlushingReport search)
        {
            // NOTE: This whole method bypasses the base Search method. Don't use it. Too much specialized
            // junk going on here.
            //
            // NOTE 2: Refactor this if at all possible.

            // This should only ever return 12 items, paging would be dumb. 
            search.EnablePaging = false;
            var query = Session.QueryOver<HydrantInspection>();
            // Setup aliases
            Hydrant hyd = null;
            query.JoinAlias(x => x.Hydrant, () => hyd);
            OperatingCenter opc = null;
            query.JoinAlias(x => hyd.OperatingCenter, () => opc);

            FlushReportQueryModel report = null;

            // NHibernate doesn't have a way of grouping on dateparts. So this reads all 
            // of the necessary data into memory and then does the grouping here instead.
            // So because of this, year and operating center must be supplied because we
            // don't want people reading all 500k+ records.

            // Filter all of this out to reduce the amount of junk data being returned.
            query.Where(x => x.MinutesFlowed > 0 || x.GallonsFlowed != null);

            var year = search.Year.GetValueOrDefault(DateTime.Today.Year);
            query.Where(x => x.DateInspected.Year == year)
                 .Where(x => opc.Id == search.OperatingCenter);

            query.SelectList(x => x.Select(y => y.DateInspected.Month).WithAlias(() => report.Month)
                                   .Select(y => y.BusinessUnit).WithAlias(() => report.BusinessUnit)
                                   .Select(y => y.GPM).WithAlias(() => report.GPM)
                                   .Select(y => y.MinutesFlowed).WithAlias(() => report.MinutesFlowed)
                                   .Select(y => y.GallonsFlowed).WithAlias(() => report.GallonsFlowed));

            query.TransformUsing(Transformers.AliasToBean<FlushReportQueryModel>());

            var queryBlowOff = Session.QueryOver<BlowOffInspection>();
            // Setup aliases
            Valve valve = null;
            queryBlowOff.JoinAlias(x => x.Valve, () => valve);
            queryBlowOff.JoinAlias(x => valve.OperatingCenter, () => opc);

            queryBlowOff.Where(x => x.MinutesFlowed > 0 || x.GallonsFlowed != null)
                        .Where(x => x.DateInspected.Year == year)
                        .Where(x => opc.Id == search.OperatingCenter);

            queryBlowOff.SelectList(x => x.Select(y => y.DateInspected.Month).WithAlias(() => report.Month)
                                          .Select(y => y.BusinessUnit).WithAlias(() => report.BusinessUnit)
                                          .Select(y => y.GPM).WithAlias(() => report.GPM)
                                          .Select(y => y.MinutesFlowed).WithAlias(() => report.MinutesFlowed)
                                          .Select(y => y.GallonsFlowed).WithAlias(() => report.GallonsFlowed));

            queryBlowOff.TransformUsing(Transformers.AliasToBean<FlushReportQueryModel>());

            // Don't call base Search method as it will duplicate the search parameters.
            var hydrantResult = query.List<FlushReportQueryModel>();
            var blowOffResult = queryBlowOff.List<FlushReportQueryModel>();

            var result = hydrantResult.Concat(blowOffResult).GroupBy(x => x.Month);
            var result2 = hydrantResult.Concat(blowOffResult).GroupBy(x => new {x.Month, x.BusinessUnit});
            var realResult = new List<HydrantFlushingReportItem>();

            var missingMonths = Enumerable.Range(1, 12).ToList();

            foreach (var key in result2)
            {
                missingMonths.Remove(key.Key.Month);
                realResult.Add(new HydrantFlushingReportItem {
                    Month = key.Key.Month,
                    BusinessUnit = key.Key.BusinessUnit,
                    TotalGallons = key.Sum(x => x.GetTotalGallons())
                });
            }

            // Add empty reports for the months that weren't included.
            realResult.AddRange(missingMonths.Select(m => new HydrantFlushingReportItem {
                Month = m
            }));

            search.Results = realResult.OrderBy(x => x.Month);
            search.Count = search.Results.Count();
            return search.Results;
        }

        #region KPI Hydrants Inspected Report

        // Internal for testing only.
        internal IEnumerable<KPIHydrantsInspectedReportItem> GetKPIHydrantsInspected(
            ISearchKPIHydrantInspectionReport search)
        {
            var year = search.Year.GetValueOrDefault(DateTime.Today.Year);
            var qry = GetDistinctInspectionsForYear(year);

            if (search.OperatingCenter != null && search.OperatingCenter.Any())
            {
                qry = qry.Where(hi => search.OperatingCenter.Contains(hi.Hydrant.OperatingCenter.Id));
            }

            return qry.GroupBy(hi => new {
                hi.DateInspected.Month,
                hi.DateInspected.Year,
                hi.Hydrant.OperatingCenter.OperatingCenterCode,
                InspectionType = hi.HydrantInspectionType.Description
            }).Select(group => new KPIHydrantsInspectedReportItem {
                MonthCompleted = group.Key.Month,
                HydrantInspectionType = group.Key.InspectionType,
                MonthTotal = group.Count(),
                OperatingCenter = group.Key.OperatingCenterCode,
                Year = group.Key.Year
            });
        }

        public IEnumerable<KPIHydrantsInspectedReport> GetKPIHydrantsInspectedReport(
            ISearchKPIHydrantInspectionReport search)
        {
            var report = new List<KPIHydrantsInspectedReport>();

            var results = GetKPIHydrantsInspected(search);

            var years = results.Select(x => x.Year).Distinct().OrderBy(x => x);
            var hydrantInspectionTypes = results.Select(x => x.HydrantInspectionType).Distinct().OrderBy(x => x);
            var operatingCenters = results.Select(x => x.OperatingCenter).Distinct().OrderBy(x => x);

            if (!results.Any())
            {
                if (search.Year.HasValue)
                {
                    years = new[] {search.Year.Value}.OrderBy(x => x);
                }

                if (search.OperatingCenter != null && search.OperatingCenter.Any())
                {
                    operatingCenters = _operatingCenterRepository
                                      .Where(oc => search.OperatingCenter.Contains(oc.Id))
                                      .Select(oc => oc.OperatingCenterCode).ToList().OrderBy(x => x);
                }
            }

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
            var totalTotalRequired = 0;
            var totalTotalDistinct = 0;
            var subTotalJan = 0;
            var subTotalFeb = 0;
            var subTotalMar = 0;
            var subTotalApr = 0;
            var subTotalMay = 0;
            var subTotalJun = 0;
            var subTotalJul = 0;
            var subTotalAug = 0;
            var subTotalSep = 0;
            var subTotalOct = 0;
            var subTotalNov = 0;
            var subTotalDec = 0;
            var currentJan = 0;
            var currentFeb = 0;
            var currentMar = 0;
            var currentApr = 0;
            var currentMay = 0;
            var currentJun = 0;
            var currentJul = 0;
            var currentAug = 0;
            var currentSep = 0;
            var currentOct = 0;
            var currentNov = 0;
            var currentDec = 0;

            foreach (var year in years)
            {
                foreach (var oc in operatingCenters)
                {
                    var operatingCenterId = _operatingCenterRepository.Where(x => x.OperatingCenterCode == oc)
                                                                      .SingleOrDefault().Id;
                    var totalRequired =
                        _hydrantRepository.GetCountOfInspectionsRequiredForYear(year, operatingCenterId);
                    var totalDistinct = this.GetCountOfDistinctInspectionsForYear(year, operatingCenterId);

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
                    totalTotalRequired += totalRequired;
                    totalTotalDistinct += totalDistinct;
                    foreach (var hit in hydrantInspectionTypes)
                    {
                        var rowResult =
                            results.Where(x => x.OperatingCenter == oc && x.HydrantInspectionType == hit).ToArray();
                        if (rowResult.Any())
                        {
                            Func<int, int> getCount = (month) => {
                                var first = rowResult.FirstOrDefault(x => x.MonthCompleted == month);
                                return (first != null) ? (int)first.MonthTotal : 0;
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

                            report.Add(new KPIHydrantsInspectedReport {
                                OperatingCenter = oc,
                                HydrantInspectionType = hit,
                                Year = year,
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

                    report.Add(new KPIHydrantsInspectedReport {
                        OperatingCenter = oc,
                        HydrantInspectionType = "Total",
                        Year = year,
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
                        Dec = subTotalDec,
                        Completed = (totalRequired == 0) ? null : (decimal?)totalDistinct / totalRequired,
                        //TotalDistinct = totalDistinct,
                        TotalRequired = totalRequired
                    });
                }

                report.Add(new KPIHydrantsInspectedReport {
                    OperatingCenter = "Total",
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
                    Completed = (totalTotalRequired == 0) ? null : (decimal?)totalTotalDistinct / totalTotalRequired,
                    //TotalDistinct = totalTotalDistinct,
                    TotalRequired = totalTotalRequired
                });
            }

            return report;
        }

        // NOTE: this is copied from HydrantRepository.GetHydrantsRequiringInspectionInYear
        private IQueryable<HydrantInspection> GetDistinctInspectionsForYear(int year, int? operatingCenterId = null)
        {
            // skipping security on purpose here
            var search = Session.Query<HydrantInspection>()
                                .Where(hi => hi.DateInspected.Year == year)
                                .Where(hi => AssetStatus.ACTIVE_STATUSES.Contains(hi.Hydrant.Status.Id))
                                .Where(hi => !hi.Hydrant.IsNonBPUKPI)
                                .Where(hi => hi.Hydrant.HydrantBilling == null ||
                                             new[] {HydrantBilling.Indices.PUBLIC, HydrantBilling.Indices.PRIVATE}
                                                .Contains(
                                                     hi.Hydrant.HydrantBilling.Id))
                                .Where(hi =>
                                     hi.Hydrant.DateInstalled == null || hi.Hydrant.DateInstalled.Value.Year <= year)
                                 // no prior inspections in same year (so we get the first for a given hydrant per year)
                                .Where(hi => !hi.Hydrant.HydrantInspections.Any(hi2 =>
                                     hi2.DateInspected.Year == hi.DateInspected.Year &&
                                     hi2.DateInspected < hi.DateInspected))
                                .Where(hi =>
                                     hi.Hydrant.InspectionFrequency != null &&
                                     hi.Hydrant.InspectionFrequencyUnit != null
                                         // hydrant has inspection frequency of its own
                                         ? ( // frequency greater than yearly
                                             hi.Hydrant.InspectionFrequencyUnit.Id !=
                                             RecurringFrequencyUnit.Indices.YEAR ||
                                             // no inspections prior to specified year
                                             !hi.Hydrant.HydrantInspections.Any(hi2 => hi2.DateInspected.Year < year) ||
                                             // the last inspection prior to the year 'year' is outside inspection frequency
                                             year - hi.Hydrant.HydrantInspections
                                                      .Where(hi2 => hi2.DateInspected.Year < year)
                                                      .Max(hi2 => hi2.DateInspected.Year) >=
                                             hi.Hydrant.InspectionFrequency)
                                         // hydrant has no inspection frequency set, use zone or operating center frequency if set
                                         : hi.Hydrant.OperatingCenter.HydrantInspectionFrequencyUnit != null &&
                                           (hi.Hydrant.OperatingCenter.ZoneStartYear != null && hi.Hydrant.Zone != null
                                               // operating center has zone start year and hydrant has zone, so return
                                               // whether or not the hydrant is in the zone for 'year'
                                               ? hi.Hydrant.Zone == (year - hi.Hydrant.OperatingCenter.ZoneStartYear) %
                                               hi.Hydrant.OperatingCenter.HydrantInspectionFrequency + 1
                                               : hi.Hydrant.OperatingCenter.HydrantInspectionFrequencyUnit != null &&
                                                 // frequency greater than yearly
                                                 hi.Hydrant.OperatingCenter.HydrantInspectionFrequencyUnit.Id !=
                                                 RecurringFrequencyUnit.Indices.YEAR ||
                                                 // no inspections prior to specified year
                                                 !hi.Hydrant.HydrantInspections.Any(
                                                     hi2 => hi2.DateInspected.Year < year) ||
                                                 // last inspection prior to the year 'year' is outside oc inspection frequency
                                                 year - hi.Hydrant.HydrantInspections
                                                          .Where(hi2 => hi2.DateInspected.Year < year)
                                                          .Max(hi2 => hi2.DateInspected.Year) >=
                                                 hi.Hydrant.OperatingCenter.HydrantInspectionFrequency));

            if (operatingCenterId.HasValue)
            {
                search = search.Where(hi => hi.Hydrant.OperatingCenter.Id == operatingCenterId);
            }

            return search;
        }

        // Internal for testing purposes only, might as well be private.
        internal int GetCountOfDistinctInspectionsForYear(int year, int? operatingCenterId)
        {
            return GetDistinctInspectionsForYear(year, operatingCenterId).Select(hi => hi.Hydrant.Id).Distinct()
                                                                         .Count();
        }

        #endregion

        /// <summary>
        /// Allowed to skip over filtering of operating centers
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> GetDistinctYearsCompleted()
        {
            return
                (from hi in base.Linq
                 select hi.DateInspected.Year
                ).Distinct();
        }

        public IEnumerable<InspectionProductivityReportItem> GetInspectionProductivityReport(
            ISearchInspectionProductivity search)
        {
            // TODO: Date filtering thing one week or two weeks

            var startDate = search.StartDate.Value.Date;
            var endDate = startDate.AddDays(search.GetDays());

            search.EnablePaging = false;
            var query = Session.QueryOver<HydrantInspection>();
            InspectionProductivityReportItem result = null;
            Hydrant asset = null;
            query.JoinAlias(x => x.Hydrant, () => asset);
            OperatingCenter opc = null;
            query.JoinAlias(x => asset.OperatingCenter, () => opc);
            HydrantInspectionType hit = null;
            query.JoinAlias(x => x.HydrantInspectionType, () => hit);
            User inspectedBy = null;
            query.JoinAlias(x => x.InspectedBy, () => inspectedBy);

            query.Where(x => x.DateInspected >= startDate && x.DateInspected < endDate);

            query.SelectList(x => x.SelectGroup(y => opc.OperatingCenterCode).WithAlias(() => result.OperatingCenter)
                                   .SelectGroup(y => inspectedBy.FullName).WithAlias(() => result.InspectedBy)
                                   .SelectGroup(() => hit.Description).WithAlias(() => result.InspectionType)
                                   .Select(() => "Hydrant").WithAlias(() => result.AssetType)
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
            query.OrderBy(() => hit.Description).Asc();

            query.TransformUsing(Transformers.AliasToBean<InspectionProductivityReportItem>());
            var ret = Search(search, query);
            return ret;
        }

        #endregion

        #region Private classes

        private class FlushReportQueryModel
        {
            public int Month { get; set; }
            public decimal GPM { get; set; }
            public decimal MinutesFlowed { get; set; }
            public int GallonsFlowed { get; set; }
            
            public string BusinessUnit { get; set; }

            public decimal GetTotalGallons()
            {
                if (MinutesFlowed > 0)
                {
                    return MinutesFlowed * GPM;
                }

                return GallonsFlowed;
            }
        }

        #endregion

        public IEnumerable<HydrantInspection> GetFromPastMonth()
        {
            return this.GetFromPastMonthImpl(_dateTimeProvider);
        }

        public IEnumerable<HydrantInspection> GetHydrantInspectionsWithSapRetryIssues()
        {
            return this.GetHydrantInspectionsWithSapRetryIssuesImpl();
        }
    }

    public static class IHydrantInspectionRepositoryExtensions
    {
        public static IQueryable<HydrantInspection> GetFromPastMonthImpl(this IRepository<HydrantInspection> that,
            IDateTimeProvider dateTimeProvider)
        {
            var yestermonth = dateTimeProvider.GetCurrentDate().AddMonths(-1).Date;

            return that.Where(hi => hi.DateInspected < yestermonth.AddMonths(1) && hi.DateInspected >= yestermonth);
        }

        public static IEnumerable<HydrantInspection> GetHydrantInspectionsWithSapRetryIssuesImpl(
            this IRepository<HydrantInspection> that)
        {
            return that.Where(x =>
                x.SAPErrorCode != null && x.SAPErrorCode != "" && x.SAPErrorCode.StartsWith("RETRY"));
        }
    }

    public interface IHydrantInspectionRepository : IRepository<HydrantInspection>
    {
        IEnumerable<HydrantInspection> GetFromPastMonth();
        IEnumerable<HydrantInspection> GetHydrantInspectionsWithSapRetryIssues();
        IEnumerable<HydrantInspectionSearchResultViewModel> SearchInspections(ISearchHydrantInspection search);
        IEnumerable<AssetCoordinate> SearchHydrantInspectionsForMap(ISearchHydrantInspection search);
        IEnumerable<HydrantFlushingReportItem> GetFlushingReport(ISearchHydrantFlushingReport search);
        IEnumerable<int> GetDistinctYearsCompleted();
        IEnumerable<KPIHydrantsInspectedReport> GetKPIHydrantsInspectedReport(ISearchKPIHydrantInspectionReport search);

        IEnumerable<InspectionProductivityReportItem> GetInspectionProductivityReport(
            ISearchInspectionProductivity search);
    }
}
