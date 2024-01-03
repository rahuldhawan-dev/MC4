using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using MapCall.Common.Data;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.ViewModels;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Criterion.Lambda;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using StructureMap;
using Expression = NHibernate.Criterion.Expression;

namespace MapCall.Common.Model.Repositories
{
    public interface IValveRepository : IRepository<Valve>
    {
        IEnumerable<Valve> FindByStreetId(int streetId);
        IQueryable<Valve> FindByStreetIdForWorkOrders(int streetId);
        IEnumerable<Valve> FindByTownId(int townId);
        IEnumerable<EntityLookup> FindByTownIdOther(int townId);
        IEnumerable<int> RouteByTownId(int townId);
        IEnumerable<int> RouteByOperatingCenterIdAndOrTownId(int operatingCenterId, int? townId);
        IEnumerable<int> GetUnusedValveNumbers(OperatingCenter operatingCenter, Town town, TownSection townSection);
        IEnumerable<ValveBPUReportItem> GetValveBPUCounts(ISearchSet<ValveBPUReportItem> search);

        IEnumerable<ValveDueInspectionReportItem> GetValvesDueInspection(
            ISearchSet<ValveDueInspectionReportItem> search);

        IEnumerable<AssetCoordinate> GetValveAssetCoordinates(IAssetCoordinateSearch search);
        IEnumerable<AssetCoordinate> GetBlowOffAssetCoordinates(IAssetCoordinateSearch search);
        IEnumerable<ValveRouteReportItem> GetRoutes(ISearchSet<ValveRouteReportItem> search);
        IEnumerable<AgedPendingAssetReportItem> GetAgedPendingAssets(ISearchSet<AgedPendingAssetReportItem> search);

        IEnumerable<(string OperatingCenter, int Count)> GetCountOfInspectionsRequiredForYear(int year,
            bool? isLargeValve = null, params int[] operatingCenterId);

        IEnumerable<(string OperatingCenter, int Count)> GetCountOfExistingValvesThatAreInspectableForYear(int year,
            bool? isLargeValve = null, params int[] operatingCenterId);

        /// <summary>
        /// Perform a search, mapping the results to <see cref="ValveAssetCoordinate"/> instances to be
        /// displayed on a map.
        /// </summary>
        IEnumerable<ValveAssetCoordinate> SearchForMap(ISearchValveForMap search);
        /// <summary>
        /// Perform a search of valves for which blow-off inspections can be performed, mapping the results
        /// to <see cref="BlowOffAssetCoordinate"/> instances to be displayed on a map.
        /// </summary>
        IEnumerable<BlowOffAssetCoordinate> SearchBlowOffsForMap(ISearchBlowOffForMap search);
    }

    /// <inheritdoc cref="IValveRepository" />
    public class ValveRepository : MapCallSecuredRepositoryBase<Valve>, IValveRepository
    {
        #region Constants

        public const int MIN_VALVE_SUFFIX_VALUE = 1;
        public const string VALVE_PREFIX = "V";

        #endregion

        #region Fields

        private readonly IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Properties

        public override ICriteria Criteria
        {
            get
            {
                var crit = base.Criteria
                               .CreateAlias("Coordinate", "c", JoinType.LeftOuterJoin);

                if (!CurrentUserCanAccessAllTheRecords)
                {
                    var opCenterIds = GetUserOperatingCenterIds();
                    crit = crit.Add(Restrictions.In("OperatingCenter.Id", opCenterIds));
                }

                crit = crit.SetFetchMode("Street", FetchMode.Eager)
                           .SetFetchMode("CrossStreet", FetchMode.Eager);

                return crit;
            }
        }

        public override IQueryable<Valve> Linq
        {
            get
            {
                var linq = base.Linq;

                if (!CurrentUserCanAccessAllTheRecords)
                {
                    var opCenterIds = GetUserOperatingCenterIds();
                    linq = linq.Where(x => opCenterIds.Contains(x.OperatingCenter.Id));
                }

                //linq = linq.Fetch(x => x.Street)
                //    .Fetch(x => x.CrossStreet);

                return linq;
            }
        }

        public override RoleModules Role
        {
            get { return RoleModules.FieldServicesAssets; }
        }

        #endregion

        #region Constructor

        public ValveRepository(IRepository<AggregateRole> roleRepo, ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IDateTimeProvider dateTimeProvider) : base(session,
            container,
            authenticationService, roleRepo)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Returns an Expression that can be used in a linq filter for finding valves in a
        /// specific area based on a town's abbreviation type. 
        /// </summary>
        /// <param name="town"></param>
        /// <param name="townSection"></param>
        /// <returns></returns>
        public static Expression<Func<Valve, bool>> GetLinqForSpecificNumberingSystem(OperatingCenter operatingCenter,
            Town town, TownSection townSection)
        {
            if (town.AbbreviationType.Id == AbbreviationType.Indices.TOWN)
            {
                return x => x.Town == town && x.OperatingCenter == operatingCenter;
            }

            // INC000000123337: Valves do not support FireDistrict abbreviation type. Default to TownSection abbreviation type if FD is used.

            // NOTE: townSection can be null, and that's ok, and this should explicitly check that the
            // town section is null.
            if (townSection == null || String.IsNullOrWhiteSpace(townSection.Abbreviation))
            { // if the town section has no abbreviation, assume the town's 
                return x => x.Town == town && x.OperatingCenter == operatingCenter &&
                            (x.TownSection == null || x.TownSection.Abbreviation == null ||
                             x.TownSection.Abbreviation == "");
            }

            return x => x.Town == town && x.OperatingCenter == operatingCenter && x.TownSection == townSection;
        }

        private static int GetPendingValveStatus()
        {
            return AssetStatus.Indices.PENDING;
        }

        private static IQueryOver<Valve, Valve> SelectAssetCoordinateList<TAssetCoordinate>(
            IQueryOver<Valve, Valve> query,
            Func<IQueryOver<Valve, Valve>, IQueryOver<Valve, Valve>> joinAliasFn = null,
            Func<QueryOverProjectionBuilder<Valve>, QueryOverProjectionBuilder<Valve>>
                projectFn = null)
            where TAssetCoordinate : AssetCoordinate
        {
            joinAliasFn = joinAliasFn ?? (x => x);
            projectFn = projectFn ?? (x => x);
            
            Coordinate coord = null;
            // We don't want null coordinates included, so inner join
            query.JoinAlias(x => x.Coordinate, () => coord, JoinType.InnerJoin);
            OperatingCenter opc = null;
            query.JoinAlias(x => x.OperatingCenter, () => opc);
            AssetStatus status = null;
            query.JoinAlias(x => x.Status, () => status);
            ValveBilling billing = null;
            query.JoinAlias(x => x.ValveBilling, () => billing);

            query = joinAliasFn(query);

            ValveAssetCoordinate result = null;
            query.SelectList(x => {
                x.Select(v => v.Id).WithAlias(() => result.Id)
                 .Select(() => coord.Latitude).WithAlias(() => result.Latitude)
                 .Select(() => coord.Longitude).WithAlias(() => result.Longitude)
                 .Select(v => v.LastInspectionDate).WithAlias(() => result.LastInspection)
                 .Select(v => v.RequiresInspection).WithAlias(() => result.RequiresInspection)
                 .Select(v => v.HasOpenWorkOrder).WithAlias(() => result.HasOpenWorkOrder)
                 .Select(Projections.Conditional(
                      Restrictions.In(Projections.Property(() => status.Id),
                          AssetStatus.ACTIVE_STATUSES.ToArray()),
                      Projections.Constant(true),
                      Projections.Constant(false)
                  )).WithAlias(() => result.IsActive)
                 .Select(Projections.Conditional(
                      Restrictions.Eq(
                          Projections.Property(() => billing.Id), ValveBilling.Indices.PUBLIC),
                      Projections.Constant(true),
                      Projections.Constant(false)
                  )).WithAlias(() => result.IsPublic);

                return projectFn(x);
            });

            // This query need an order by statement or else it will return the results in 
            // a random order for some reason. This is to ensure the data's always returned
            // in the exact same order so it can be displayed in the same order. 
            query.OrderBy(x => x.Id).Asc();

            return query.TransformUsing(Transformers.AliasToBean<TAssetCoordinate>());            
        }

        private static IQueryOver<Valve, Valve> SelectValveAssetCoordinateList(
            IQueryOver<Valve, Valve> query)
        {
            ValveAssetCoordinate result = null;
            ValveNormalPosition normPos = null;

            return SelectAssetCoordinateList<ValveAssetCoordinate>(
                query,
                q => q.JoinAlias(x => x.NormalPosition, () => normPos, JoinType.LeftOuterJoin),
                x => x.Select(v => v.NormalPosition).WithAlias(() => result.NormalPosition)
                      .Select(v => v.InNormalPosition).WithAlias(() => result.InNormalPosition)
                      .Select(v => v.RequiresInspection).WithAlias(() => result.RequiresInspection));
        }

        private static IQueryOver<Valve, Valve> SelectBlowOffAssetCoordinateList(
            IQueryOver<Valve, Valve> query)
        {
            BlowOffAssetCoordinate result = null;

            return SelectAssetCoordinateList<BlowOffAssetCoordinate>(
                query,
                projectFn:
                x => x.Select(v => v.RequiresBlowOffInspection).WithAlias(() => result.RequiresInspection));
        }
        
        #endregion

        #region Public Methods

        #region Cascades

        public IEnumerable<Valve> FindByStreetId(int streetId)
        {
            return Linq.Where(x => x.Street != null && x.Street.Id == streetId).OrderBy(x => x.ValveSuffix).ToList();
        }

        public IQueryable<Valve> FindByStreetIdForWorkOrders(int streetId)
        {
            return Linq.Where(x => x.Street != null && x.Street.Id == streetId
                                                    && x.Status != null
                                                    && x.Status.Id != AssetStatus.Indices.RETIRED
                                                    && x.Status.Id != AssetStatus.Indices.REMOVED
                                                    && x.Status.Id != AssetStatus.Indices.CANCELLED)
                       .OrderBy(x => x.ValveSuffix);
        }

        public IEnumerable<Valve> FindByTownId(int townId)
        {
            return Linq.Where(x => x.Town.Id == townId).OrderBy(x => x.ValveSuffix).ToList();
        }

        public IEnumerable<EntityLookup> FindByTownIdOther(int townId)
        {
            return Linq.Where(x => x.Town.Id == townId).OrderBy(x => x.ValveSuffix)
                       .Select(v => new EntityLookup {Id = v.Id, Description = v.ValveNumber}).ToList();
        }

        public IEnumerable<int> RouteByTownId(int townId)
        {
            var results =
                Linq.Where(x => x.Town.Id == townId && x.Route.HasValue)
                    .OrderBy(x => x.Route.Value)
                    .Select(x => x.Route.Value)
                    .Distinct();
            return results;
        }

        public IEnumerable<int> RouteByOperatingCenterIdAndOrTownId(int operatingCenterId, int? townId)
        {
            var results = Linq.Where(x => x.OperatingCenter.Id == operatingCenterId && x.Route.HasValue)
                              .Select(x => new {x.Route, x.OperatingCenter, x.Town});
            if (townId.HasValue)
                results = results.Where(x => x.Town.Id == townId.Value && x.Route.HasValue);
            return results.OrderBy(x => x.Route.Value).Select(x => x.Route.Value).Distinct();
        }

        #endregion

        #region Helpers for lack of a better word

        public IEnumerable<int> GetUnusedValveNumbers(OperatingCenter operatingCenter, Town town,
            TownSection townSection)
        {
            // OperatingCenter isn't being used here, so we need to use base.Linq to bypass the role stuff. We don't
            // want someone somehow entering a found hydrant that's used but not found in the query due to operating center
            // limitations and towns having multiple operating centers and all that junk.
            var usedNumbers =
                base.Linq
                    .Where(GetLinqForSpecificNumberingSystem(operatingCenter, town, townSection))
                    .Select(x => x.ValveSuffix)
                    .ToList();

            if (!usedNumbers.Any())
            {
                return Enumerable.Empty<int>();
            }

            // NOTE: This will return an empty list when there's a single hydrant
            //       match and its suffix == MIN_HYDRANT_SUFFIX_VALUE.
            var range = Enumerable.Range(MIN_VALVE_SUFFIX_VALUE, usedNumbers.Max());
            return range.Where(x => !usedNumbers.Contains(x)).ToList();
        }

        public IEnumerable<AssetCoordinate> GetValveAssetCoordinates(IAssetCoordinateSearch search)
        {
            // NOTE: NO ROLE FILTERING IS DONE FOR THIS QUERY. Alex said so. -Ross 4/15/2015
            // NOTE 2: If you fix something in here then the same change most likely needs to be made in HydrantRepository.
            // NOTE 3: If you fix something in here then the same change most likely needs to be made in GetBlowOffAssetCoordinates.
            // NOTE 4: NormalPosition/InNormalPosition is only for Valve coordinates. Blowoffs do not use this.

            search.EnablePaging = false;
            var query = Session.QueryOver<Valve>();

            Coordinate coord = null;
            query.Where(v => v.CanHaveBlowOffInspections == false)
                 .Where(() => coord.Latitude >= search.LatitudeMin && coord.Latitude <= search.LatitudeMax)
                 .Where(() => coord.Longitude >= search.LongitudeMin && coord.Longitude <= search.LongitudeMax);

            SelectValveAssetCoordinateList(query);
            
            return Search(search, query);
        }

        public IEnumerable<AssetCoordinate> GetBlowOffAssetCoordinates(IAssetCoordinateSearch search)
        {
            // NOTE: NO ROLE FILTERING IS DONE FOR THIS QUERY. Alex said so. -Ross 4/15/2015
            // NOTE 2: If you fix something in here then the same change most likely needs to be made in HydrantRepository.
            // NOTE 3: If you fix something in here then the same change most likely needs to be made in GetValveAssetCoordinates.
            // NOTE 4: NormalPosition/InNormalPosition is only for Valve coordinates. Blowoffs do not use this.

            search.EnablePaging = false;
            var query = Session.QueryOver<Valve>();

            Coordinate coord = null;
            query.Where(v => v.CanHaveBlowOffInspections == true)
                 .Where(() => coord.Latitude >= search.LatitudeMin && coord.Latitude <= search.LatitudeMax)
                 .Where(() => coord.Longitude >= search.LongitudeMin && coord.Longitude <= search.LongitudeMax);

            SelectBlowOffAssetCoordinateList(query);
            
            return Search(search, query);
        }

        public IEnumerable<BlowOffAssetCoordinate> SearchBlowOffsForMap(ISearchBlowOffForMap search)
        {
            var query = Session.QueryOver<Valve>();

            SelectBlowOffAssetCoordinateList(query);

            return Search(search, query);
        }

        public IEnumerable<ValveAssetCoordinate> SearchForMap(ISearchValveForMap search)
        {
            var query = Session.QueryOver<Valve>();

            SelectValveAssetCoordinateList(query);

            return Search(search, query);
        }

        public IEnumerable<(string OperatingCenter, int Count)> GetCountOfInspectionsRequiredForYear(int year,
            bool? isLargeValve = null, params int[] operatingCenterId)
        {
            OperatingCenter opc = null;
            ValveSize vs = null;
            var sqlite = ConfigurationManager.AppSettings["DatabaseType"]?.ToLower() == "sqlite";
            var where = String.Format("{0} = 1",
                String.Format(
                    (sqlite)
                        ? ValveMap.Sql.REQUIRES_INSPECTION_FORMAT_STRING_SQLITE
                        : ValveMap.Sql.REQUIRES_INSPECTION_FORMAT_STRING,
                    "Size",
                    year,
                    String.Format("WHEN (Year(DateInstalled) > {0}) THEN 0", year),
                    "UsesValveInspectionFrequency"));
            // This needs to include all towns/operating centers
            // that the current user might not have access to for whatever reason. 
            var query = Session.QueryOver<Valve>()
                               .JoinAlias(x => x.OperatingCenter, () => opc)
                               .JoinAlias(x => x.ValveSize, () => vs)
                               .Where(Expression.Sql(where))
                               .WhereRestrictionOn(() => opc.Id).IsIn(operatingCenterId);

            if (isLargeValve.HasValue)
            {
                query.Where(x => x.IsLargeValve == isLargeValve.Value);
            }

            var item = (OperatingCenter: (string)null, Count: 0);
            var list = new List<(string OperatingCenter, int Count)>();

            query.SelectList(x => x
                                 .SelectGroup(_ => opc.OperatingCenterCode).WithAlias(() => item.OperatingCenter)
                                 .Select(Projections.Alias(Projections.Count("opc.OperatingCenterCode"), "Item2")));
            query.TransformUsing(Transformers.AliasToBean<(string OperatingCenter, int Count)>());
            query.UnderlyingCriteria.List(list);

            return list;
        }

        public IEnumerable<(string OperatingCenter, int Count)> GetCountOfExistingValvesThatAreInspectableForYear(
            int year, bool? isLargeValve = null, params int[] operatingCenterId)
        {
            OperatingCenter opc = null;
            ValveSize vs = null;
            var query =
                Session.QueryOver<Valve>()
                       .JoinAlias(x => x.OperatingCenter, () => opc)
                       .JoinAlias(x => x.ValveSize, () => vs)
                       .Where(Expression.Sql(string.Format(ValveMap.Sql.INSPECTABLE_FORMAT_STRING, "Size")))
                       .WhereRestrictionOn(() => opc.Id).IsIn(operatingCenterId)
                       .Where(x => x.DateInstalled != null && x.DateInstalled.Value <= new DateTime(year, 12, 31));

            if (isLargeValve.HasValue)
            {
                query.Where(x => x.IsLargeValve == isLargeValve.Value);
            }

            var item = (OperatingCenter: (string)null, Count: 0);
            var list = new List<(string OperatingCenter, int Count)>();

            query.SelectList(x => x
                                 .SelectGroup(_ => opc.OperatingCenterCode).WithAlias(() => item.OperatingCenter)
                                 .Select(Projections.Alias(Projections.Count("opc.OperatingCenterCode"), "Item2")));
            query.TransformUsing(Transformers.AliasToBean<(string OperatingCenter, int Count)>());
            query.UnderlyingCriteria.List(list);

            return list;
        }

        public IEnumerable<Valve> GetValvesWithSapRetryIssues()
        {
            return this.GetValvesWithSapRetryIssuesImpl();
        }

        #endregion

        #region Reports

        public IEnumerable<ValveRouteReportItem> GetRoutes(ISearchSet<ValveRouteReportItem> search)
        {
            ValveRouteReportItem result = null;
            OperatingCenter opc = null;
            Town town = null;
            AssetStatus status = null;

            var query = Session.QueryOver<Valve>()
                               .JoinAlias(x => x.OperatingCenter, () => opc)
                               .JoinAlias(x => x.Town, () => town)
                               .JoinAlias(x => x.Status, () => status);

            query.SelectList(x => x
                                 .SelectGroup(y => y.OperatingCenter).WithAlias(() => result.OperatingCenter)
                                 .SelectGroup(y => y.Town).WithAlias(() => result.Town)
                                 .SelectGroup(y => y.Status).WithAlias(() => result.ValveStatus)
                                 .SelectGroup(y => opc.OperatingCenterCode) // select these so we can order by them?
                                 .SelectGroup(y => town.ShortName) // select these so we can order by them?
                                 .SelectGroup(y => status.Description) // select these so we can order by them?
                                 .SelectGroup(y => y.Route.Value).WithAlias(() => result.Route)
                                 .SelectCount(y => y.Route).WithAlias(() => result.Total)
            );

            query
               .OrderBy(x => opc.OperatingCenterCode).Asc()
               .OrderBy(x => town.ShortName).Asc()
               .OrderBy(x => x.Route).Asc()
               .OrderBy(x => status.Description).Asc();

            query.TransformUsing(Transformers.AliasToBean<ValveRouteReportItem>());

            return Search(search, query);
        }

        public IEnumerable<ValveBPUReportItem> GetValveBPUCounts(ISearchSet<ValveBPUReportItem> search)
        {
            ValveBPUReportItem result = null;
            OperatingCenter opc = null;
            ValveBilling billing = null;
            AssetStatus status = null;
            ValveSize sizeThingus = null;

            var query = Session.QueryOver<Valve>()
                               .Where(x => !x.BPUKPI)
                               .JoinAlias(x => x.OperatingCenter, () => opc)
                               .JoinAlias(x => x.ValveBilling, () => billing)
                               .JoinAlias(x => x.Status, () => status)
                               .JoinAlias(x => x.ValveSize, () => sizeThingus);

            query.SelectList(x => x
                                 .SelectGroup(y => opc.OperatingCenterCode).WithAlias(() => result.OperatingCenter)
                                 .SelectGroup(y => billing.Description).WithAlias(() => result.ValveBilling)
                                 .SelectGroup(y => status.Description).WithAlias(() => result.ValveStatus)
                                 .SelectGroup(y => sizeThingus.SizeRange).WithAlias(() => result.SizeRange)
                                 .SelectCount(y => y.Id).WithAlias(() => result.Total));

            query.OrderBy(() => opc.OperatingCenterCode).Asc()
                 .OrderBy(() => billing.Description).Asc()
                 .OrderBy(() => status.Description).Asc();

            query.TransformUsing(Transformers.AliasToBean<ValveBPUReportItem>());
            return Search(search, query);
        }

        public IEnumerable<ValveDueInspectionReportItem> GetValvesDueInspection(
            ISearchSet<ValveDueInspectionReportItem> search)
        {
            var query = Session.QueryOver<Valve>().Where(x => x.RequiresInspection);

            OperatingCenter opc = null;
            query.JoinAlias(x => x.OperatingCenter, () => opc);
            Town town = null;
            query.JoinAlias(x => x.Town, () => town);
            ValveDueInspectionReportItem report = null;
            query.SelectList(x => x
                                 .SelectGroup(o => opc.OperatingCenterCode).WithAlias(() => report.OperatingCenter)
                                 .SelectGroup(o => opc.Id).WithAlias(() => report.OperatingCenterId)
                                 .SelectGroup(t => town.ShortName).WithAlias(() => report.Town)
                                 .SelectGroup(t => town.Id).WithAlias(() => report.TownId)
                                 .SelectCount(c => c.Town).WithAlias(() => report.Count));
            query
               .OrderBy(() => opc.OperatingCenterCode).Asc()
               .OrderBy(() => town.ShortName).Asc();

            query.TransformUsing(Transformers.AliasToBean<ValveDueInspectionReportItem>());
            return Search(search, query);
        }

        public IEnumerable<AgedPendingAssetReportItem> GetAgedPendingAssets(
            ISearchSet<AgedPendingAssetReportItem> search)
        {
            var now = _dateTimeProvider.GetCurrentDate().BeginningOfDay();
            Valve valve = null;
            AgedPendingAssetReportItem result = null;
            OperatingCenter opc = null;
            var pendingValveStatus = GetPendingValveStatus();

            var query = Session.QueryOver<Valve>().Where(x => x.Status.Id == pendingValveStatus);
            // subquery counts
            var zeroToNinety = QueryOver.Of(() => valve)
                                        .Where(tx => tx.OperatingCenter.Id == opc.Id
                                                     && tx.Status.Id == pendingValveStatus
                                                     && tx.CreatedAt > now.AddDays(-90)).ToRowCountQuery();
            var ninetyOneToOneEighty = QueryOver.Of<Valve>()
                                                .Where(tx => tx.OperatingCenter.Id == opc.Id
                                                             && tx.Status.Id == pendingValveStatus
                                                             && tx.CreatedAt < now.AddDays(-90)
                                                             && tx.CreatedAt >= now.AddDays(-180)).ToRowCountQuery();
            var oneEightyToThreeSixty = QueryOver.Of<Valve>()
                                                 .Where(tx => tx.OperatingCenter.Id == opc.Id
                                                              && tx.Status.Id == pendingValveStatus
                                                              && tx.CreatedAt < now.AddDays(-180)
                                                              && tx.CreatedAt >= now.AddDays(-360)).ToRowCountQuery();
            var threeSixtyPlus = QueryOver.Of<Valve>()
                                          .Where(tx => tx.OperatingCenter.Id == opc.Id
                                                       && tx.Status.Id == pendingValveStatus
                                                       && tx.CreatedAt < now.AddDays(-360)).ToRowCountQuery();

            query.JoinAlias(x => x.OperatingCenter, () => opc);

            query.SelectList(x => x
                                 .Select(_ => "Valve").WithAlias(() => result.AssetType)
                                 .SelectGroup(z => z.OperatingCenter).WithAlias(() => result.OperatingCenter)
                                 .SelectGroup(z => opc.Id).WithAlias(() => result.OperatingCenterId)
                                 .SelectCount(z => z.OperatingCenter).WithAlias(() => result.Total)
                                 .SelectSubQuery(zeroToNinety).WithAlias(() => result.ZeroToNinety)
                                 .SelectSubQuery(ninetyOneToOneEighty).WithAlias(() => result.NinetyOneToOneEighty)
                                 .SelectSubQuery(oneEightyToThreeSixty).WithAlias(() => result.OneEightyToThreeSixty)
                                 .SelectSubQuery(threeSixtyPlus).WithAlias(() => result.ThreeSixtyPlus)
            );

            query.TransformUsing(Transformers.AliasToBean<AgedPendingAssetReportItem>());
            return Search(search, query);
        }

        #endregion

        #endregion
    }

    public static class IValveRepositoryExtensions
    {
        public static IEnumerable<Valve> GetValvesWithSapRetryIssuesImpl(this IRepository<Valve> that)
        {
            return that.Where(x =>
                x.SAPErrorCode != null && x.SAPErrorCode != "" && x.SAPErrorCode.StartsWith("RETRY"));
        }

        public static IQueryable<Valve> GetValvesByOperatingCenter(this IRepository<Valve> instance, params int[] ids)
        {
            return ids?.Length == 0 
                ? instance.Linq
                : instance.Where(x => ids.Contains(x.OperatingCenter.Id));
        }

        public static IQueryable<Valve> FindByTownIdAndOperatingCenterId(this IRepository<Valve> that, int townId,
            int oc)
        {
            return that.Where(x => x.Town.Id == townId && x.OperatingCenter.Id == oc).OrderBy(x => x.ValveSuffix);
        }

        public static string GenerateValvePrefix(this IRepository<Valve> that, OperatingCenter operatingCenter,
            Town town, TownSection townSection)
        {
            var prefix = town.OperatingCentersTowns.FirstOrDefault(x => x.OperatingCenter.Id == operatingCenter.Id)
                             .Abbreviation;

            if (town.AbbreviationType.Id == AbbreviationType.Indices.TOWN_SECTION ||
                town.AbbreviationType.Id == AbbreviationType.Indices.FIRE_DISTRICT)
            {
                if (townSection != null && !string.IsNullOrWhiteSpace(townSection.Abbreviation))
                {
                    prefix = townSection.Abbreviation;
                }

                // else default to town.Abbreviation which is done by default.
            }

            if (string.IsNullOrWhiteSpace(prefix))
            {
                throw new InvalidOperationException(
                    "Unable to generate a valve prefix because the town or town section abbreviation is not set.");
            }

            return ValveRepository.VALVE_PREFIX + prefix;
        }

        public static ValveNumber GenerateNextValveNumber(this IRepository<Valve> that, RepositoryBase<Valve> valveRepo,
            OperatingCenter operatingCenter, Town town, TownSection townSection)
        {
            var valNum = that.GetMaxValveNumber(valveRepo, operatingCenter, town, townSection);
            valNum.Suffix += 1;
            return valNum;
        }

        public static ValveNumber GetMaxValveNumber(this IRepository<Valve> that, RepositoryBase<Valve> valveRepo,
            OperatingCenter operatingCenter, Town town, TownSection townSection)
        {
            var valNum = new ValveNumber {
                Prefix = that.GenerateValvePrefix(operatingCenter, town, townSection),
                Suffix = GetMaxValveSuffix(valveRepo,
                    ValveRepository.GetLinqForSpecificNumberingSystem(operatingCenter, town, townSection))
            };
            return valNum;
        }

        public static IQueryable<Valve> FindByOperatingCenterAndValveNumber(this IRepository<Valve> that,
            OperatingCenter operatingCenter, string valveNumber)
        {
            return that.Where(x => x.ValveNumber == valveNumber && x.OperatingCenter == operatingCenter);
        }

        /// <summary>
        /// Gets the max valve suffix number for a given query. If there are no records
        /// that match, zero is returned.
        /// </summary>
        private static int GetMaxValveSuffix(RepositoryBase<Valve> valveRepository, Expression<Func<Valve, bool>> exp)
        {
            const int NO_MATCH_SUFFIX = 0;

            // Use base.Linq to query for this. The calculation needs to include all towns/operating centers
            // that the current user might not have access to for whatever reason. 
            var query = valveRepository.Where(exp);

            if (query.Any())
            {
                return query.Max(x => x.ValveSuffix);
            }

            return NO_MATCH_SUFFIX;
        }
    }
}
