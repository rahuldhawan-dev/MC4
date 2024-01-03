using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MapCall.Common.Data;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using StructureMap;
using Expression = System.Linq.Expressions.Expression;

namespace MapCall.Common.Model.Repositories
{
    public interface IHydrantRepository : IRepository<Hydrant>
    {
        IEnumerable<int> GetUnusedHydrantNumbers(OperatingCenter operatingCenter, Town town, TownSection townSection,
            FireDistrict fireDistrict);

        int GetTotalNumberOfActiveHydrantsForPremise(string premiseNumber);
        int GetCountOfInspectionsRequiredForYear(int year, int operatingCenterId);
        IEnumerable<ActiveHydrantReportItem> GetActiveHydrantCounts(ISearchSet<ActiveHydrantReportItem> search);

        IEnumerable<ActiveHydrantDetailReportItem> GetActiveHydrantDetailCounts(
            ISearchSet<ActiveHydrantDetailReportItem> search);

        IEnumerable<HydrantDueInspectionReportItem> GetHydrantsDueInspection(
            ISearchSet<HydrantDueInspectionReportItem> search);
        IEnumerable<HydrantDuePaintingReportItem> GetHydrantsDuePainting(
            ISearchSet<HydrantDuePaintingReportItem> search);

        IEnumerable<PublicHydrantCountReportItem> GetPublicHydrantCounts(
            ISearchSet<PublicHydrantCountReportItem> search);

        IEnumerable<AssetCoordinate> GetAssetCoordinates(IAssetCoordinateSearch search);
        IEnumerable<int> RouteByTownId(int townId);
        IEnumerable<Hydrant> GetHydrantsWithSapRetryIssues();
        IEnumerable<Hydrant> FindByStreetId(int townId);
        IQueryable<Hydrant> FindByStreetIdForWorkOrders(int townId);
        IEnumerable<HydrantRouteReportItem> GetRoutes(ISearchSet<HydrantRouteReportItem> search);
        IEnumerable<AgedPendingAssetReportItem> GetAgedPendingAssets(ISearchSet<AgedPendingAssetReportItem> search);

        IEnumerable<HydrantWorkOrdersByDescriptionReportItem> GetHydrantWorkOrdersByDescription(
            ISearchHydrantWorkOrdersByDescriptionReportItem search);

        IEnumerable<int> RouteByOperatingCenterIdAndOrTownId(int operatingCenterId, int? townId);
        IEnumerable<Hydrant> Search(ISearchHydrant search);
        
        /// <summary>
        /// Perform a search, mapping the results to <see cref="HydrantAssetCoordinate"/> instances to be
        /// displayed on a map.
        /// </summary>
        IEnumerable<HydrantAssetCoordinate> SearchForMap(ISearchHydrantForMap search);
    }

    /// <inheritdoc cref="IHydrantRepository" />
    public class HydrantRepository : MapCallSecuredRepositoryBase<Hydrant>, IHydrantRepository
    {
        #region Consts

        public const string HYDRANT_PREFIX = "H";
        public const int MIN_HYDRANT_SUFFIX_VALUE = 1;

        #endregion

        #region Fields

        private RoleMatch _roleMatch;
        protected readonly IAbbreviationTypeRepository _abbreviationTypeRepository;
        protected readonly IAssetStatusRepository _hydrantStatusRepository;
        protected readonly IDateTimeProvider _dateTimeProvider;

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

                // Performance critical for searches/indexes, make sure associations being displayed are eager loaded.
                crit = crit.SetFetchMode("Street", FetchMode.Eager)
                           .SetFetchMode("CrossStreet", FetchMode.Eager)
                           .SetFetchMode("HydrantManufacturer", FetchMode.Eager)
                           .SetFetchMode("HydrantMainSize", FetchMode.Eager)
                           .SetFetchMode("HydrantStatus", FetchMode.Eager)
                           .SetFetchMode("LateralSize", FetchMode.Eager);
                return crit;
            }
        }

        public override IQueryable<Hydrant> Linq
        {
            get
            {
                var linq = base.Linq;

                // BIG NOTE: If the Linq includes the op center check, then anyone
                //           trying to add a .Where() afterwards will get an error 
                //           because NHibernate is awful. Fetching now has to be done
                //           with the AddFetchToLinqQuery method.
                if (!CurrentUserCanAccessAllTheRecords)
                {
                    var opCenterIds = GetUserOperatingCenterIds();
                    linq = linq.Where(x => opCenterIds.Contains(x.OperatingCenter.Id));
                }

                return linq;
            }
        }

        public override RoleModules Role
        {
            get { return RoleModules.FieldServicesAssets; }
        }

        #endregion

        #region Private Methods

        // This is dead code at the moment but it's probably gonna be needed down the line due to 
        // exporting/searching.
        private IQueryable<Hydrant> AddFetchToLinqQuery(IQueryable<Hydrant> query)
        {
            // Performance critical for searches/indexes, make sure associations being displayed are eager loaded.
            return query.Fetch(x => x.Street)
                        .Fetch(x => x.CrossStreet)
                        .Fetch(x => x.HydrantManufacturer)
                        .Fetch(x => x.HydrantMainSize)
                        .Fetch(x => x.Status)
                        .Fetch(x => x.LateralSize);
        }

        private static int GetPendingHydrantStatus()
        {
            return AssetStatus.Indices.PENDING;
        }

        private static int GetPublicHydrantBilling()
        {
            return HydrantBilling.Indices.PUBLIC;
        }

        private static IQueryOver<Hydrant, Hydrant> SelectHydrantAssetCoordinateList(
            IQueryOver<Hydrant, Hydrant> query)
        {
            Coordinate coord = null;
            // We don't want null coordinates included, so inner join
            query.JoinAlias(x => x.Coordinate, () => coord, JoinType.InnerJoin);
            OperatingCenter opc = null;
            query.JoinAlias(x => x.OperatingCenter, () => opc);
            AssetStatus status = null;
            query.JoinAlias(x => x.Status, () => status);
            HydrantBilling billing = null;
            query.JoinAlias(x => x.HydrantBilling, () => billing);
            HydrantDueInspection dueInspection = null;
            query.Left.JoinAlias(x => x.HydrantDueInspection, () => dueInspection);
            HydrantDuePainting duePainting = null;
            query.Left.JoinAlias(x => x.HydrantDuePainting, () => duePainting);
            
            HydrantAssetCoordinate result = null;
            query.SelectList(x =>
                x.Select(h => h.Id).WithAlias(() => result.Id)
                 .Select(() => coord.Latitude).WithAlias(() => result.Latitude)
                 .Select(() => coord.Longitude).WithAlias(() => result.Longitude)
                 .Select(() => dueInspection.RequiresInspection).WithAlias(() => result.RequiresInspection)
                 .Select(() => dueInspection.LastInspectionDate).WithAlias(() => result.LastInspection)
                 .Select(() => duePainting.RequiresPainting).WithAlias(() => result.RequiresPainting)
                 .Select(h => h.HasOpenWorkOrder).WithAlias(() => result.HasOpenWorkOrder)
                 .Select(h => h.OutOfService).WithAlias(() => result.OutOfService)
                 .Select(h => h.Stop).WithAlias(() => result.Stop)
                 .Select(Projections.Conditional(
                      Restrictions.In(
                          Projections.Property(() => status.Id),
                          AssetStatus.ACTIVE_STATUSES.ToArray()),
                      Projections.Constant(true),
                      Projections.Constant(false)
                  )).WithAlias(() => result.IsActive)
                 .Select(Projections.Conditional(
                      Restrictions.Eq(
                          Projections.Property(() => billing.Id), HydrantBilling.Indices.PUBLIC),
                      Projections.Constant(true),
                      Projections.Constant(false)
                  )).WithAlias(() => result.IsPublic));

            // This query need an order by statement or else it will return the results in 
            // a random order for some reason. This is to ensure the data's always returned
            // in the exact same order so it can be displayed in the same order. 
            query.OrderBy(x => x.Id).Asc();

            return query.TransformUsing(Transformers.AliasToBean<HydrantAssetCoordinate>());
        }

        private IEnumerable<TReportItem> GetHydrantsDueSomething<TReportItem>(
            ISearchSet<TReportItem> search,
            Action<IQueryOver<Hydrant, Hydrant>> applyFilterFn)
            where TReportItem : HydrantDueSomethingReportItem
        {
            // NOTE: This query is slow when not filtering by anything.
            // NOTE 2: It's also slow when filtering by some op centers, like NJ7.
            // Must ignore role matching for this report as the original MapCall report did not filter by this.
            var query = Session.QueryOver<Hydrant>();

            OperatingCenter opc = null;
            query.JoinAlias(x => x.OperatingCenter, () => opc);
            Town town = null;
            query.JoinAlias(x => x.Town, () => town);

            applyFilterFn(query);

            TReportItem report = null;

            query.SelectList(x => x.SelectGroup(y => opc.OperatingCenterCode)
                                   .WithAlias(() => report.OperatingCenter)
                                   .SelectGroup(o => opc.Id).WithAlias(() => report.OperatingCenterId)
                                   .SelectGroup(y => town.ShortName).WithAlias(() => report.Town)
                                   .SelectGroup(t => town.Id).WithAlias(() => report.TownId)
                                   .SelectCount(y => y.Town).WithAlias(() => report.Count));

            query.OrderBy(() => opc.OperatingCenterCode).Asc()
                 .OrderBy(() => town.ShortName).Asc();

            // The TransformUsing call has to happen after SelectList or it straight up doesn't work.
            query.TransformUsing(Transformers.AliasToBean<TReportItem>());
            return Search(search, query);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns an Expression that can be used in a linq filter for finding hydrants in a
        /// specific area based on a town's abbreviation type. 
        /// </summary>
        public static Expression<Func<Hydrant, bool>> GetLinqForSpecificNumberingSystem(
            IAbbreviationTypeRepository abbrRepo, OperatingCenter operatingCenter, Town town, TownSection townSection,
            FireDistrict fireDistrict)
        {
            if (town.AbbreviationType == abbrRepo.GetTownAbbreviationType())
            {
                return x => x.Town == town && x.OperatingCenter == operatingCenter;
            }

            if (town.AbbreviationType == abbrRepo.GetTownSectionAbbreviationType())
            {
                if (townSection != null && !String.IsNullOrWhiteSpace(townSection.Abbreviation))
                    return x => x.Town == town && x.OperatingCenter == operatingCenter && x.TownSection == townSection;
                // ok, so it's town section abbreviation, but we don't have a value for the town section or its abbreviation
                // there could be hydrant without a town section, hydrant with another town section that has no abbreviation
                // we want to return all the hydrants in the town that don't have town sections, or town sections with no abbreviations
                if ((townSection == null) || String.IsNullOrWhiteSpace(townSection.Abbreviation))
                    return x => x.Town == town && x.OperatingCenter == operatingCenter && (x.TownSection == null ||
                        x.TownSection.Abbreviation == null || x.TownSection.Abbreviation == "");
            }

            if (town.AbbreviationType == abbrRepo.GetFireDistrictAbbreviationType())
            {
                if (fireDistrict != null && !string.IsNullOrWhiteSpace(fireDistrict.Abbreviation))
                    return x => x.Town == town && x.OperatingCenter == operatingCenter &&
                                x.FireDistrict == fireDistrict;
                // all that stuff whoever (ross probably) said about town section.
                if ((fireDistrict == null) || String.IsNullOrWhiteSpace(fireDistrict.Abbreviation))
                    return x => x.Town == town && x.OperatingCenter == operatingCenter && (x.FireDistrict == null ||
                        x.FireDistrict.Abbreviation == null || x.FireDistrict.Abbreviation == "");
            }

            throw new NotSupportedException();
        }

        public override void Delete(Hydrant entity)
        {
            throw new NotSupportedException(
                "Deleting a hydrant is not supported. Doing so will mess up any work orders associated with the hydrant.");
        }

        public override Hydrant Save(Hydrant entity)
        {
            entity = base.Save(entity);
            // Refresh needs to be called to update any formula fields that might be stale.
            Session.Refresh(entity);
            return entity;
        }

        #region Helpers

        public IEnumerable<int> GetUnusedHydrantNumbers(OperatingCenter operatingCenter, Town town,
            TownSection townSection, FireDistrict fireDistrict)
        {
            // OperatingCenter isn't being used here, so we need to use base.Linq to bypass the role stuff. We don't
            // want someone somehow entering a found hydrant that's used but not found in the query due to operating center
            // limitations and towns having multiple operating centers and all that junk.
            var usedNumbers =
                base.Linq
                    .Where(GetLinqForSpecificNumberingSystem(_abbreviationTypeRepository, operatingCenter, town,
                         townSection, fireDistrict))
                    .Select(x => x.HydrantSuffix)
                    .ToList();

            if (!usedNumbers.Any())
            {
                return Enumerable.Empty<int>();
            }

            // NOTE: This will return an empty list when there's a single hydrant
            //       match and its suffix == MIN_HYDRANT_SUFFIX_VALUE.
            var range = Enumerable.Range(MIN_HYDRANT_SUFFIX_VALUE, usedNumbers.Max());
            return range.Where(x => !usedNumbers.Contains(x)).ToList();
        }

        public int GetTotalNumberOfActiveHydrantsForPremise(string premiseNumber)
        {
            return Linq.Count(x =>
                x.FireDistrict != null && x.FireDistrict.PremiseNumber == premiseNumber &&
                AssetStatus.ACTIVE_STATUSES.Contains(x.Status.Id) &&
                x.HydrantBilling.Id == HydrantBilling.Indices.PUBLIC);
        }

        public IEnumerable<AssetCoordinate> GetAssetCoordinates(IAssetCoordinateSearch search)
        {
            // NOTE: NO ROLE FILTERING IS DONE FOR THIS QUERY. Alex said so. -Ross 4/15/2015
            // NOTE 2: If you fix something in here then the same change most likely needs to be made in ValveRepository.
            //         And also in Hydrant.ToAssetCoordinate.

            Coordinate coord = null;
            search.EnablePaging = false;
            var query = Session.QueryOver<Hydrant>();

            query.Where(() => coord.Latitude >= search.LatitudeMin && coord.Latitude <= search.LatitudeMax)
                 .Where(() => coord.Longitude >= search.LongitudeMin && coord.Longitude <= search.LongitudeMax);

            SelectHydrantAssetCoordinateList(query);
            
            return Search(search, query);
        }

        public IEnumerable<int> RouteByTownId(int townId)
        {
            return
                Linq.Where(x => x.Town.Id == townId && x.Route.HasValue)
                    .OrderBy(x => x.Route.Value)
                    .Select(x => x.Route.Value)
                    .Distinct();
        }

        public IEnumerable<int> RouteByOperatingCenterIdAndOrTownId(int operatingCenterId, int? townId)
        {
            var results = Linq.Where(x => x.OperatingCenter.Id == operatingCenterId && x.Route.HasValue)
                              .Select(x => new {x.Route, x.OperatingCenter, x.Town});
            if (townId.HasValue)
                results = results.Where(x => x.Town.Id == townId.Value && x.Route.HasValue);
            return results.OrderBy(x => x.Route.Value).Select(x => x.Route.Value).Distinct();
        }

        public IEnumerable<Hydrant> Search(ISearchHydrant search)
        {
            Hydrant hydrant = null;
            var query = Session.QueryOver(() => hydrant);

            if (search.OpenWorkOrderWorkDescription.HasValue)
            {
                query.WithSubquery.WhereExists(QueryOver.Of<WorkOrder>().Where(wo =>
                    wo.CancelledAt == null
                    && wo.DateCompleted == null
                    && wo.Hydrant.Id == hydrant.Id
                    && wo.WorkDescription.Id == search.OpenWorkOrderWorkDescription.Value
                ).Select(Projections.Constant(1)));
            }

            return Search(search, query);
        }

        public IEnumerable<HydrantAssetCoordinate> SearchForMap(ISearchHydrantForMap search)
        {
            Hydrant hydrant = null;
            var query = Session.QueryOver(() => hydrant);

            if (search.OpenWorkOrderWorkDescription.HasValue)
            {
                query.WithSubquery.WhereExists(QueryOver.Of<WorkOrder>().Where(wo =>
                    wo.CancelledAt == null
                    && wo.DateCompleted == null
                    && wo.Hydrant.Id == hydrant.Id
                    && wo.WorkDescription.Id == search.OpenWorkOrderWorkDescription.Value
                ).Select(Projections.Constant(1)));
            }

            SelectHydrantAssetCoordinateList(query);

            return Search(search, query);
        }

        public IEnumerable<Hydrant> FindByStreetId(int streetId)
        {
            return Linq.Where(x => x.Street.Id == streetId).OrderBy(x => x.HydrantSuffix).ToList();
        }

        public IQueryable<Hydrant> FindByStreetIdForWorkOrders(int streetId)
        {
            return
                Linq.Where(x => x.Street.Id == streetId && 
                                x.Status != null &&
                                x.Status.Id != AssetStatus.Indices.RETIRED &&
                                x.Status.Id != AssetStatus.Indices.REMOVED &&
                                x.Status.Id != AssetStatus.Indices.CANCELLED)
                    .OrderBy(x => x.HydrantSuffix);
        }

        public IEnumerable<Hydrant> GetHydrantsWithSapRetryIssues()
        {
            return this.GetHydrantsWithSapRetryIssuesImpl();
        }

        #endregion

        #region Reports

        private IQueryOver<Hydrant, Hydrant> GetActiveBPUHydrantsQueryOver()
        {
            return Session.QueryOver<Hydrant>()
                          .WhereRestrictionOn(x => x.Status.Id).IsIn(AssetStatus.ACTIVE_STATUSES.ToArray())
                          .Where(x => !x.IsNonBPUKPI); // Need to exclude hydrants that are not bpu/kpi bug-3204
        }

        public IEnumerable<ActiveHydrantReportItem> GetActiveHydrantCounts(ISearchSet<ActiveHydrantReportItem> search)
        {
            // NOTE: This method needs to ignore role operating centers! The report that uses this does not 
            //       care about the role in the slightest.

            // Using the nhibernate linq provider doesn't work here.
            // Criteria is ugly and makes baby jesus cry
            // QueryOver is maybe 2% better than criteria but still involves a bunch of garbage code.
            // Having said that, this generates a nice efficient query.
            // The fact that this is needed is really ugly. Shame, NHibernate, shame!
            ActiveHydrantReportItem result = null;
            OperatingCenter opc = null;
            HydrantBilling billing = null;

            // NOTE: Unlike Criteria, any of the methods called on the object
            //       IQueryOver instance will modify the instance, it will not
            //       return new objects. 
            var query = GetActiveBPUHydrantsQueryOver();
            query.JoinAlias(x => x.OperatingCenter, () => opc);
            query.JoinAlias(x => x.HydrantBilling, () => billing);

            // NOTE: The column being grouped and the column being ordered by must be the same or else SQL Server cries. Sqlite doesn't seem to mind.
            query.SelectList(x => x.SelectGroup(y => opc.OperatingCenterCode).WithAlias(() => result.OperatingCenter)
                                   .SelectGroup(y => billing.Description).WithAlias(() => result.HydrantBilling)
                                   .SelectCount(y => y.Id).WithAlias(() => result.Count));

            query.OrderBy(() => opc.OperatingCenterCode).Asc()
                 .OrderBy(() => billing.Description).Asc();

            // TransformUsing must come after SelectList or else nothing happens.
            query.TransformUsing(Transformers.AliasToBean<ActiveHydrantReportItem>());
            return Search(search, query);
        }

        public IEnumerable<ActiveHydrantDetailReportItem> GetActiveHydrantDetailCounts(
            ISearchSet<ActiveHydrantDetailReportItem> search)
        {
            // This might need to become criteria because there's a lot of search parameters. Ugh.
            var query = GetActiveBPUHydrantsQueryOver();

            // So ugly just to get OrderBy to work on non-primary key fields.
            OperatingCenter opc = null;
            query.JoinAlias(x => x.OperatingCenter, () => opc);
            Town town = null;
            query.JoinAlias(x => x.Town, () => town);
            HydrantBilling billing = null;
            query.JoinAlias(x => x.HydrantBilling, () => billing);
            LateralSize latSize = null;
            query.JoinAlias(x => x.LateralSize, () => latSize, NHibernate.SqlCommand.JoinType.LeftOuterJoin);
            HydrantSize hydSize = null;
            query.JoinAlias(x => x.HydrantSize, () => hydSize, NHibernate.SqlCommand.JoinType.LeftOuterJoin);
            ActiveHydrantDetailReportItem report = null;

            // It really pains me that I have to write this much to do what Linq can do with Select(new {}). Maybe
            // one day NHibernate's linq provider won't be as terrible.
            query.SelectList(x => x.SelectGroup(y => opc.OperatingCenterCode).WithAlias(() => report.OperatingCenter)
                                   .SelectGroup(y => town.ShortName).WithAlias(() => report.Town)
                                   .SelectGroup(y => billing.Description).WithAlias(() => report.HydrantBilling)
                                   .SelectGroup(y => latSize.Description).WithAlias(() => report.LateralSize)
                                   .SelectGroup(y => hydSize.Description).WithAlias(() => report.HydrantSize)
                                   .SelectCount(y => y.Id).WithAlias(() => report.Count));

            // NOTE: If you don't include the Desc or Asc call then the order by statement is never added.
            query.OrderBy(() => opc.OperatingCenterCode).Asc()
                 .OrderBy(() => town.ShortName).Asc()
                 .OrderBy(() => billing.Description).Asc()
                 .OrderBy(() => latSize.Description).Asc()
                 .OrderBy(() => hydSize.Description).Asc();

            // The TransformUsing call has to happen after SelectList or it straight up doesn't work.
            query.TransformUsing(Transformers.AliasToBean<ActiveHydrantDetailReportItem>());

            return Search(search, query);
        }

        public IEnumerable<AgedPendingAssetReportItem> GetAgedPendingAssets(
            ISearchSet<AgedPendingAssetReportItem> search)
        {
            var now = _dateTimeProvider.GetCurrentDate().BeginningOfDay();
            Hydrant hydrant = null;
            AgedPendingAssetReportItem result = null;
            OperatingCenter opc = null;
            var pendingHydrantStatus = GetPendingHydrantStatus();

            var query = Session.QueryOver<Hydrant>().Where(x => x.Status.Id == pendingHydrantStatus);
            // subquery counts
            var zeroToNinety = QueryOver.Of<Hydrant>()
                                        .Where(tx => tx.OperatingCenter.Id == opc.Id
                                                     && tx.Status.Id == pendingHydrantStatus
                                                     && tx.CreatedAt > now.AddDays(-90)).ToRowCountQuery();
            var ninetyOneToOneEighty = QueryOver.Of<Hydrant>()
                                                .Where(tx => tx.OperatingCenter.Id == opc.Id
                                                             && tx.Status.Id == pendingHydrantStatus
                                                             && tx.CreatedAt < now.AddDays(-90)
                                                             && tx.CreatedAt >= now.AddDays(-180)).ToRowCountQuery();
            var oneEightyToThreeSixty = QueryOver.Of<Hydrant>()
                                                 .Where(tx => tx.OperatingCenter.Id == opc.Id
                                                              && tx.Status.Id == pendingHydrantStatus
                                                              && tx.CreatedAt < now.AddDays(-180)
                                                              && tx.CreatedAt >= now.AddDays(-360)).ToRowCountQuery();
            var threeSixtyPlus = QueryOver.Of<Hydrant>()
                                          .Where(tx => tx.OperatingCenter.Id == opc.Id
                                                       && tx.Status.Id == pendingHydrantStatus
                                                       && tx.CreatedAt < now.AddDays(-360)).ToRowCountQuery();

            query.JoinAlias(x => x.OperatingCenter, () => opc);

            query.SelectList(x => x
                                 .Select(_ => "Hydrant").WithAlias(() => result.AssetType)
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

        public IQueryable<Hydrant> GetHydrantsRequiringInspectionInYear(int year, int operatingCenterId)
        {
            // skipping security intentionally
            return Session.Query<Hydrant>().GetHydrantsRequiringInspectionInYear(year, operatingCenterId);
        }

        public int GetCountOfInspectionsRequiredForYear(int year, int operatingCenterId)
        {
            var hydrants = GetHydrantsRequiringInspectionInYear(year, operatingCenterId)
                          .Select(h => new {
                               InspectionFrequencyUnitId =
                                   h.InspectionFrequencyUnit != null ? (int?)h.InspectionFrequencyUnit.Id : null,
                               h.InspectionFrequency,
                               h.Zone,
                               OperatingCenterInspectionFrequency = h.OperatingCenter.HydrantInspectionFrequency,
                               OperatingCenterInspectionFrequencyUnitId =
                                   h.OperatingCenter.HydrantInspectionFrequencyUnit != null
                                       ? (int?)h.OperatingCenter.HydrantInspectionFrequencyUnit.Id
                                       : null,
                               h.OperatingCenter.ZoneStartYear
                           }).ToList();

            return (int)Math.Round(hydrants.Aggregate(0m, (i, h) =>
                i +
                (h.InspectionFrequencyUnitId != null && h.InspectionFrequency != null
                    ? (h.InspectionFrequencyUnitId == RecurringFrequencyUnit.Indices.YEAR
                        ? 1m / h.InspectionFrequency.Value
                        : (h.InspectionFrequencyUnitId == RecurringFrequencyUnit.Indices.MONTH
                            ? 12m / h.InspectionFrequency.Value
                            : (h.InspectionFrequencyUnitId == RecurringFrequencyUnit.Indices.WEEK
                                ? 52m / h.InspectionFrequency.Value
                                : (h.InspectionFrequencyUnitId == RecurringFrequencyUnit.Indices.DAY
                                    ? 365m / h.InspectionFrequency.Value
                                    : 0m))))
                    : (h.OperatingCenterInspectionFrequencyUnitId != null
                        ? (h.ZoneStartYear != null && h.Zone != null && h.Zone ==
                            ((year - h.ZoneStartYear) % h.OperatingCenterInspectionFrequency) + 1
                                ? 1m
                                : (h.OperatingCenterInspectionFrequencyUnitId == RecurringFrequencyUnit.Indices.YEAR
                                    ? 1m / h.OperatingCenterInspectionFrequency
                                    : (h.OperatingCenterInspectionFrequencyUnitId ==
                                       RecurringFrequencyUnit.Indices.MONTH
                                        ? 12m / h.OperatingCenterInspectionFrequency
                                        : (h.OperatingCenterInspectionFrequencyUnitId ==
                                           RecurringFrequencyUnit.Indices.WEEK
                                            ? 52m / h.OperatingCenterInspectionFrequency
                                            : (h.OperatingCenterInspectionFrequencyUnitId ==
                                               RecurringFrequencyUnit.Indices.DAY
                                                ? 365m / h.OperatingCenterInspectionFrequency
                                                : 0m)))))
                        : 0m))));

            //            var ret = q.Select(Projections.SqlProjection($@"
            //CASE
            //    WHEN this_.InspectionFrequencyUnitId IS NOT NULL
            //    AND this_.InspectionFrequency IS NOT NULL
            //    THEN CASE this_.InspectionFrequencyUnitId
            //        WHEN {RecurringFrequencyUnit.Indices.YEAR}
            //        THEN 1 / this_.InspectionFrequency
            //        WHEN {RecurringFrequencyUnit.Indices.MONTH}
            //        THEN 12 / this_.InspectionFrequency
            //        WHEN {RecurringFrequencyUnit.Indices.WEEK}
            //        THEN 52 / this_.InspectionFrequency
            //        WHEN {RecurringFrequencyUnit.Indices.DAY}
            //        THEN 365 / this_.InspectionFrequency
            //    END
            //    WHEN oc1_.HydrantInspectionFrequencyUnitId IS NOT NULL AND oc1_.HydrantInspectionFrequency IS NOT NULL
            //    THEN CASE
            //        WHEN oc1_.ZoneStartYear IS NOT NULL AND this_.Zone IS NOT NULL AND this_.Zone = ((ABS(oc1_.ZoneStartYear - {year}) % oc1_.HydrantInspectionFrequency) + 1)
            //        THEN 1
            //        ELSE CASE oc1_.HydrantInspectionFrequencyUnitId
            //            WHEN {RecurringFrequencyUnit.Indices.YEAR}
            //            THEN 1 / oc1_.HydrantInspectionFrequency
            //            WHEN {RecurringFrequencyUnit.Indices.MONTH}
            //            THEN 12 / oc1_.HydrantInspectionFrequency
            //            WHEN {RecurringFrequencyUnit.Indices.WEEK}            //            THEN 52 / oc1_.HydrantInspectionFrequency
            //            WHEN {RecurringFrequencyUnit.Indices.DAY}
            //            THEN 365 / oc1_.HydrantInspectionFrequency
            //        END
            //    END
            //    ELSE 1
            //END as InspectionsRequired", new [] {"InspectionsRequired"}, new IType[] {NHibernateUtil.Decimal})).List<decimal>().Sum();

            //            return (int)Math.Round(ret);
        }

        public IEnumerable<HydrantDueInspectionReportItem> GetHydrantsDueInspection(
            ISearchSet<HydrantDueInspectionReportItem> search)
        {
            return GetHydrantsDueSomething(search, q => {
                HydrantDueInspection dueInspection = null;
                q.JoinAlias(x => x.HydrantDueInspection, () => dueInspection);

                q.Where(() => dueInspection.RequiresInspection);
            });
        }

        public IEnumerable<HydrantDuePaintingReportItem> GetHydrantsDuePainting(
            ISearchSet<HydrantDuePaintingReportItem> search)
        {
            return GetHydrantsDueSomething(search, q => {
                HydrantDuePainting duePainting = null;
                q.JoinAlias(x => x.HydrantDuePainting, () => duePainting);

                q.Where(() => duePainting.RequiresPainting);
            });
        }

        public IEnumerable<PublicHydrantCountReportItem> GetPublicHydrantCounts(
            ISearchSet<PublicHydrantCountReportItem> search)
        {
            // NOTE: This method needs to ignore role operating centers! The report that uses this does not 
            //       care about the role in the slightest.

            PublicHydrantCountReportItem result = null;

            var publicBilling = GetPublicHydrantBilling();
            var query = Session.QueryOver<Hydrant>()
                               .Where(x => x.HydrantBilling.Id == publicBilling);

            OperatingCenter opc = null;
            query.JoinAlias(x => x.OperatingCenter, () => opc);
            Town town = null;
            query.JoinAlias(x => x.Town, () => town);
            FireDistrict fd = null;
            query.JoinAlias(x => x.FireDistrict, () => fd, NHibernate.SqlCommand.JoinType.LeftOuterJoin);
            AssetStatus status = null;
            query.JoinAlias(x => x.Status, () => status);

            // NOTE: The column being grouped and the column being ordered by must be the same or else SQL Server cries. Sqlite doesn't seem to mind.
            query.SelectList(x => x.SelectGroup(y => opc.OperatingCenterCode).WithAlias(() => result.OperatingCenter)
                                   .SelectGroup(y => town.ShortName).WithAlias(() => result.Town)
                                   .SelectGroup(y => fd.DistrictName).WithAlias(() => result.FireDistrict)
                                   .SelectGroup(y => status.Description).WithAlias(() => result.Status)
                                   .SelectGroup(y => fd.PremiseNumber).WithAlias(() => result.PremiseNumber)
                                   .SelectCount(y => y.Id).WithAlias(() => result.Total));

            query.OrderBy(() => opc.OperatingCenterCode).Asc()
                 .OrderBy(() => town.ShortName).Asc()
                 .OrderBy(() => fd.DistrictName).Asc()
                 .OrderBy(() => status.Description).Asc()
                 .OrderBy(x => x.PremiseNumber).Asc();

            // TransformUsing must come after SelectList or else nothing happens.
            query.TransformUsing(Transformers.AliasToBean<PublicHydrantCountReportItem>());
            return Search(search, query);
        }

        public IEnumerable<HydrantRouteReportItem> GetRoutes(ISearchSet<HydrantRouteReportItem> search)
        {
            HydrantRouteReportItem result = null;
            OperatingCenter opc = null;
            Town town = null;
            AssetStatus status = null;

            var query = Session.QueryOver<Hydrant>()
                               .JoinAlias(x => x.OperatingCenter, () => opc)
                               .JoinAlias(x => x.Town, () => town)
                               .JoinAlias(x => x.Status, () => status);

            query.SelectList(x => x
                                 .SelectGroup(y => y.OperatingCenter).WithAlias(() => result.OperatingCenter)
                                 .SelectGroup(y => y.Town).WithAlias(() => result.Town)
                                 .SelectGroup(y => y.Status).WithAlias(() => result.HydrantStatus)
                                 .SelectGroup(y => opc.OperatingCenterCode)
                                 .SelectGroup(y => town.ShortName)
                                 .SelectGroup(y => status.Description)
                                 .SelectGroup(y => y.Route.Value).WithAlias(() => result.Route)
                                 .SelectCount(y => y.Route).WithAlias(() => result.Total)
            );

            query
               .OrderBy(x => opc.OperatingCenterCode).Asc()
               .OrderBy(x => town.ShortName).Asc()
               .OrderBy(x => x.Route).Asc()
               .OrderBy(x => status.Description);

            query.TransformUsing(Transformers.AliasToBean<HydrantRouteReportItem>());

            return Search(search, query);
        }

        public IEnumerable<HydrantWorkOrdersByDescriptionReportItem> GetHydrantWorkOrdersByDescription(
            ISearchHydrantWorkOrdersByDescriptionReportItem search)
        {
            Hydrant hydrant = null;
            HydrantWorkOrdersByDescriptionReportItem result = null;
            var query = Session.QueryOver(() => hydrant);
            WorkOrder workOrder = null;
            query.JoinAlias(x => x.WorkOrders, () => workOrder, JoinType.LeftOuterJoin);

            if (search.HasWorkOrderWithWorkDescriptions != null
                && search.HasWorkOrderWithWorkDescriptions.Length > 0)
            {
                query.WithSubquery.WhereExists(QueryOver.Of<WorkOrder>().Where(wo =>
                    wo.CancelledAt == null
                    && wo.Hydrant.Id == hydrant.Id
                    && wo.WorkDescription.Id.IsIn(search.HasWorkOrderWithWorkDescriptions)
                ).Select(Projections.Constant(1)));
            }

            query.SelectList(x => x
                                 .SelectGroup(y => y.OperatingCenter).WithAlias(() => result.OperatingCenter)
                                 .SelectGroup(y => y.HydrantNumber).WithAlias(() => result.HydrantNumber)
                                 .SelectGroup(y => y.Town).WithAlias(() => result.Town)
                                 .SelectGroup(y => y.HydrantSuffix).WithAlias(() => result.HydrantSuffix)
                                 .SelectGroup(y => y.FireDistrict).WithAlias(() => result.FireDistrict)
                                 .SelectGroup(y => y.StreetNumber).WithAlias(() => result.StreetNumber)
                                 .SelectGroup(y => y.Street).WithAlias(() => result.Street)
                                 .SelectGroup(y => y.CrossStreet).WithAlias(() => result.CrossStreet)
                                 .SelectGroup(y => y.HydrantManufacturer).WithAlias(() => result.HydrantManufacturer)
                                 .SelectGroup(y => y.YearManufactured).WithAlias(() => result.YearManufactured)
                                 .SelectGroup(y => y.DateInstalled).WithAlias(() => result.DateInstalled)
                                 .SelectGroup(y => y.Status).WithAlias(() => result.HydrantStatus)
                                 .SelectGroup(y => y.HydrantBilling).WithAlias(() => result.HydrantBilling)
                                 .SelectGroup(y => workOrder.WorkDescription).WithAlias(() => result.WorkDescription)
                                 .SelectGroup(y => y.Id).WithAlias(() => result.Id));
            query.TransformUsing(Transformers.AliasToBean<HydrantWorkOrdersByDescriptionReportItem>());

            return Search(search, query);
        }

        #endregion

        #endregion

        public HydrantRepository(ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo,
            IAbbreviationTypeRepository abbreviationTypeRepository, IAssetStatusRepository hydrantStatusRepository,
            IDateTimeProvider dateTimeProvider) : base(session, container, authenticationService,
            roleRepo)
        {
            _abbreviationTypeRepository = abbreviationTypeRepository;
            _hydrantStatusRepository = hydrantStatusRepository;
            _dateTimeProvider = dateTimeProvider;
        }
    }

    // This is used in the scheduler too if you're like me and wondering why it exists. Though I still don't actually know why it exists. -Ross 4/27/2017
    public static class IHydrantRepositoryExtensions
    {
        public static IEnumerable<Hydrant> GetHydrantsWithSapRetryIssuesImpl(this IRepository<Hydrant> that)
        {
            return
                that.Where(x => x.SAPErrorCode != null && x.SAPErrorCode != "" && x.SAPErrorCode.StartsWith("RETRY"));
        }

        public static IQueryable<Hydrant> GetHydrantsByOperatingCenter(this IRepository<Hydrant> instance, params int[] ids)
        {
            return (ids?.Length == 0) 
                ? instance.Linq
                : instance.Linq.Where(x => ids.Contains(x.OperatingCenter.Id));
        }

        public static HydrantNumber GetMaxHydrantNumber(this IRepository<Hydrant> that,
            RepositoryBase<Hydrant> hydrantRepo, IAbbreviationTypeRepository abbreviationTypeRepository,
            OperatingCenter operatingCenter,
            Town town, TownSection townSection, FireDistrict fireDistrict)
        {
            return new HydrantNumber {
                Prefix = that.GenerateHydrantPrefix(operatingCenter, town, townSection,
                    fireDistrict), // Defaults to town abbreviation for 99.9% of cases
                Suffix = GetMaxHydrantSuffix(hydrantRepo,
                    HydrantRepository.GetLinqForSpecificNumberingSystem(abbreviationTypeRepository, operatingCenter,
                        town, townSection, fireDistrict))
            };
        }

        public static string GenerateHydrantPrefix(this IRepository<Hydrant> that, OperatingCenter operatingCenter,
            Town town, TownSection townSection, FireDistrict fireDistrict)
        {
            // 99.9% of our hydrants just use the town's abbreviation for hydrant numbers.
            // Neptune, for unknown reasons, uses town section.
            // Hempstead, for unknown reasons, used to use fire district.
            var prefix = town.OperatingCentersTowns.FirstOrDefault(x => x.OperatingCenter == operatingCenter)
                             .Abbreviation; // Defaults to town abbreviation for 99.9% of cases

            if (town.AbbreviationType.Id == AbbreviationType.Indices.TOWN_SECTION)
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
                    "Unable to generate a hydrant prefix because the town or town section abbreviation is not set.");
            }

            return HydrantRepository.HYDRANT_PREFIX + prefix;
        }

        public static IQueryable<Hydrant> FindByOperatingCenterAndHydrantNumber(this IRepository<Hydrant> that,
            OperatingCenter oc, string hydrantNumber)
        {
            // This *should* be returning one result, but there is bad data in the database in which duplicates are returned.
            return that.Where(x => x.HydrantNumber == hydrantNumber && x.OperatingCenter == oc);
        }

        /// <summary>
        /// Gets the next hydrant number that's valid for a hydrant. 
        /// </summary>
        public static HydrantNumber GenerateNextHydrantNumber(this IRepository<Hydrant> that,
            IAbbreviationTypeRepository abbreviationTypeRepository, RepositoryBase<Hydrant> hydrantRepo,
            OperatingCenter operatingCenter, Town town, TownSection townSection, FireDistrict fireDistrict)
        {
            var hydNum = that.GetMaxHydrantNumber(hydrantRepo, abbreviationTypeRepository, operatingCenter, town,
                townSection, fireDistrict);
            hydNum.Suffix += 1;
            return hydNum;
        }

        // Use base.Linq (via RepositoryBase<Hydrant>) to query for this. The
        // calculation needs to include all towns/operating centers that the
        // current user might not have access to for whatever reason. 
        private static int GetMaxHydrantSuffix(RepositoryBase<Hydrant> hydrantRepo, Expression<Func<Hydrant, bool>> exp)
        {
            const int NO_MATCH_SUFFIX = 0;

            var query = hydrantRepo.Where(exp);
            if (query.Any())
            {
                return query.Max(x => x.HydrantSuffix);
            }

            return NO_MATCH_SUFFIX;
        }

        // Internal for testing only. Should be private.
        internal static IQueryable<Hydrant> GetHydrantsRequiringInspectionInYear(this IQueryable<Hydrant> that,
            int year,
            int operatingCenterId)
        {
            return that.Where(h => AssetStatus.ACTIVE_STATUSES.Contains(h.Status.Id))
                       .Where(h => !h.IsNonBPUKPI)
                       .Where(h => h.OperatingCenter.Id == operatingCenterId)
                       .Where(h => h.HydrantBilling == null ||
                                   new[] {HydrantBilling.Indices.PUBLIC, HydrantBilling.Indices.PRIVATE}.Contains(
                                       h.HydrantBilling.Id))
                       .Where(h => h.DateInstalled == null || h.DateInstalled.Value.Year <= year)
                       .Where(h =>
                            h.InspectionFrequency != null && h.InspectionFrequencyUnit != null
                                // hydrant has inspection frequency of its own
                                ? // frequency greater than yearly
                                h.InspectionFrequencyUnit.Id != RecurringFrequencyUnit.Indices.YEAR ||
                                // no inspections prior to specified year
                                !h.HydrantInspections.Any(hi => hi.DateInspected.Year < year) ||
                                // the last inspection prior to the year 'year' is outside inspection frequency
                                year - h.HydrantInspections.Where(hi => hi.DateInspected.Year < year)
                                        .Max(hi => hi.DateInspected.Year) >= h.InspectionFrequency
                                // hydrant has no inspection frequency set, use zone or operating center frequency if set
                                : h.OperatingCenter.HydrantInspectionFrequencyUnit != null &&
                                  (h.OperatingCenter.ZoneStartYear != null && h.Zone != null
                                      // operating center has zone start year and hydrant has zone, so return
                                      // whether or not the hydrant is in the zone for 'year'
                                      ? h.Zone == (year - h.OperatingCenter.ZoneStartYear) %
                                      h.OperatingCenter.HydrantInspectionFrequency + 1
                                      : h.OperatingCenter.HydrantInspectionFrequencyUnit != null &&
                                        // frequency greater than yearly
                                        h.OperatingCenter.HydrantInspectionFrequencyUnit.Id !=
                                        RecurringFrequencyUnit.Indices.YEAR ||
                                        // no inspections prior to specified year
                                        !h.HydrantInspections.Any(hi => hi.DateInspected.Year < year) ||
                                        // last inspection prior to the year 'year' is outside oc inspection frequency
                                        year - h.HydrantInspections.Where(hi => hi.DateInspected.Year < year)
                                                .Max(hi => hi.DateInspected.Year) >=
                                        h.OperatingCenter.HydrantInspectionFrequency));
        }
        
        public static IQueryable<Hydrant> FindByTownId(this IRepository<Hydrant> that, int townId)
        {
            return that.Where(x => x.Town.Id == townId).OrderBy(x => x.HydrantSuffix);
        }
        
        public static IQueryable<Hydrant> FindActiveByTownId(this IRepository<Hydrant> that, int townId)
        {
            return that.Where(x => x.Town.Id == townId &&
                                   x.Status != null &&
                                   x.Status.Id != AssetStatus.Indices.RETIRED &&
                                   x.Status.Id != AssetStatus.Indices.REMOVED &&
                                   x.Status.Id != AssetStatus.Indices.CANCELLED &&
                                   x.Status.Id != AssetStatus.Indices.INACTIVE)
                       .OrderBy(x => x.HydrantSuffix);
        }
    }
}
