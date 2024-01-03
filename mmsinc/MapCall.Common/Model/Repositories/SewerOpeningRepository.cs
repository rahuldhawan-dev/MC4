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
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface ISewerOpeningRepository : IRepository<SewerOpening>
    {
        IEnumerable<int> RouteByTownId(int townId);
        IEnumerable<SewerOpening> FindActiveByTownId(int townId);
        IEnumerable<SewerOpening> ByStreetId(int streetId);
        IQueryable<SewerOpening> ByStreetIdForWorkOrders(int streetId);
        IEnumerable<SewerOpening> GetAllForDropDown();
        IEnumerable<AssetCoordinate> GetAssetCoordinates(IAssetCoordinateSearch search);

        IEnumerable<SewerOpening> GetNpdesRegulatorsDueInspection(
            ISearchNpdesRegulatorsDueInspectionItem search);

        IEnumerable<SewerOpeningAssetCoordinate> SearchNpdesRegulatorsDueInspectionForMap(
            ISearchNpdesRegulatorsDueInspectionForMap search);

        IEnumerable<SewerOpeningAssetCoordinate> SearchForMap(ISearchSewerOpeningForMap search);

        IEnumerable<NpdesRegulatorsDueInspectionReportItem> GetNpdesRegulatorsDueInspectionReport(
            ISearchNpdesRegulatorsDueInspectionReportItem search);
    }

    public static class SewerOpeningRepositoryExtensions
    {
        #region Constants

        private const string SEWER_OPENING_PREFIX = "M";

        #endregion

        public static SewerOpeningNumber GenerateNextOpeningNumber(this IRepository<SewerOpening> that,
            IAbbreviationTypeRepository abbreviationTypeRepository, RepositoryBase<SewerOpening> smRepo,
            OperatingCenter operatingCenter, Town town, TownSection townSection)
        {
            var num = that.GetMaxOpeningNumber(abbreviationTypeRepository, smRepo, operatingCenter, town, townSection);
            num.Suffix += 1;
            return num;
        }

        public static SewerOpeningNumber GetMaxOpeningNumber(this IRepository<SewerOpening> that,
            IAbbreviationTypeRepository abbreviationTypeRepository, RepositoryBase<SewerOpening> smRepo,
            OperatingCenter operatingCenter, Town town, TownSection townSection)
        {
            return new SewerOpeningNumber {
                Prefix = GenerateOpeningPrefix(operatingCenter, town, townSection),
                Suffix = that.GetMaxOpeningSuffix(smRepo,
                    SewerOpeningRepository.GetLinqForSpecificNumberingSystem(abbreviationTypeRepository,
                        operatingCenter, town, townSection))
            };
        }

        private static string GenerateOpeningPrefix(OperatingCenter operatingCenter, Town town, TownSection townSection)
        {
            var prefix = town.OperatingCentersTowns.FirstOrDefault(x => x.OperatingCenter == operatingCenter)
                            ?.Abbreviation;

            if (town.AbbreviationType.Id == AbbreviationType.Indices.TOWN_SECTION)
            {
                if (townSection != null && !string.IsNullOrWhiteSpace(townSection.Abbreviation))
                {
                    prefix = townSection.Abbreviation;
                }

                // else default to town.Abbreviation
            }

            if (string.IsNullOrWhiteSpace(prefix))
            {
                throw new InvalidOperationException(
                    "Unable to generate a opening prefix because the town or town section abbreviation is not set.");
            }

            return SEWER_OPENING_PREFIX + prefix;
        }

        public static int GetMaxOpeningSuffix(this IRepository<SewerOpening> that, RepositoryBase<SewerOpening> smRepo,
            Expression<Func<SewerOpening, bool>> expression)
        {
            const int NO_MATCH_SUFFIX = 0;

            var query = smRepo.Where(expression);
            if (query.Any())
            {
                return query.Max(x => x.OpeningSuffix);
            }

            return NO_MATCH_SUFFIX;
        }

        public static IQueryable<SewerOpening> FindByOperatingCenterAndOpeningNumber(
            this IRepository<SewerOpening> that, OperatingCenter operatingCenter, string openingNumber)
        {
            return that.Where(x => x.OpeningNumber == openingNumber && x.OperatingCenter == operatingCenter);
        }

        public static IQueryable<SewerOpening> FindByTownId(this IRepository<SewerOpening> that, int townId)
        {
            return that.Where(x => x.Town.Id == townId);
        }

        public static IEnumerable<SewerOpening> FindByPartialOpeningMatchByTown(this IRepository<SewerOpening> that,
            string partialId, int townId)
        {
            if (string.IsNullOrWhiteSpace(partialId))
            {
                return Enumerable.Empty<SewerOpening>();
            }

            return that.Where(i =>
                i.OpeningNumber.Contains(partialId) && i.Town.Id == townId &&
                !AssetStatus.INACTIVE_STATUSES.Contains(i.Status.Id));
        }
    }

    public class SewerOpeningRepository : MapCallSecuredRepositoryBase<SewerOpening>, ISewerOpeningRepository
    {
        #region Constants

        private const string SEWER_OPENING_PREFIX = "M";

        #endregion

        #region Properties

        public override RoleModules Role => RoleModules.FieldServicesAssets;

        public override ICriteria Criteria
        {
            get
            {
                var crit = base.Criteria;

                if (!CurrentUserCanAccessAllTheRecords)
                {
                    var opCenterIds = GetUserOperatingCenterIds();
                    crit = crit.Add(Restrictions.In("OperatingCenter.Id", opCenterIds));
                }

                return crit;
            }
        }

        public override IQueryable<SewerOpening> Linq
        {
            get
            {
                var linq = base.Linq;

                if (!CurrentUserCanAccessAllTheRecords)
                {
                    var opCenterIds = GetUserOperatingCenterIds();
                    linq = linq.Where(x => opCenterIds.Contains(x.OperatingCenter.Id));
                }

                return linq;
            }
        }

        #endregion

        #region Private Methods

        private IQueryOver<SewerOpening, SewerOpening> GetNpdesRegulatorDueInspectionSearchQuery<TResult>(ISearchNpdesRegulatorDueInspection<TResult> search)
        {
            SewerOpening sewerOpening = null;
            OperatingCenter opc = null;
            Town town = null;
            AssetStatus status = null;
            BodyOfWater bw = null;
            WasteWaterSystem wws = null;

            var subQuery = QueryOver.Of<NpdesRegulatorInspection>().Where(x => x.SewerOpening.Id == sewerOpening.Id);
            switch (search.DepartureDateTime.Operator)
            {
                case RangeOperator.Between:
                    subQuery = subQuery.Where(x => x.DepartureDateTime >= ((DateTime)search.DepartureDateTime.Start).BeginningOfDay() && 
                                                   x.DepartureDateTime <= ((DateTime)search.DepartureDateTime.End).BeginningOfDay().AddDays(1));
                    break;
                case RangeOperator.Equal:
                    subQuery = subQuery.Where(x => x.DepartureDateTime >= ((DateTime)search.DepartureDateTime.End).BeginningOfDay() &&
                                                   x.DepartureDateTime <= ((DateTime)search.DepartureDateTime.End).BeginningOfDay().AddDays(1));
                    break;
                case RangeOperator.GreaterThan:
                    subQuery = subQuery.Where(x => x.DepartureDateTime >= ((DateTime)search.DepartureDateTime.End).BeginningOfDay().AddDays(1));
                    break;
                case RangeOperator.GreaterThanOrEqualTo:
                    subQuery = subQuery.Where(x => x.DepartureDateTime >= ((DateTime)search.DepartureDateTime.End).BeginningOfDay());
                    break;
                case RangeOperator.LessThan:
                    subQuery = subQuery.Where(x => x.DepartureDateTime < ((DateTime)search.DepartureDateTime.End).BeginningOfDay().AddDays(1));
                    break;
                case RangeOperator.LessThanOrEqualTo:
                    subQuery = subQuery.Where(x => x.DepartureDateTime < ((DateTime)search.DepartureDateTime.End).BeginningOfDay());
                    break;
            }

            subQuery = subQuery.SelectList(x => x.Select(y => y.SewerOpening.Id));

            var query = Session.QueryOver(() => sewerOpening)
                               .JoinAlias(x => x.OperatingCenter, () => opc)
                               .JoinAlias(x => x.Town, () => town)
                               .JoinAlias(x => x.Status, () => status, JoinType.LeftOuterJoin)
                               .JoinAlias(x => x.BodyOfWater, () => bw, JoinType.LeftOuterJoin)
                               .JoinAlias(x => x.WasteWaterSystem, () => wws, JoinType.LeftOuterJoin)
                               .Where(x => x.SewerOpeningType.Id == SewerOpeningType.Indices.NPDES_REGULATOR)
                               .Where(x => x.Town.Id == search.Town)
                               .Where(x => x.OperatingCenter.Id == search.OperatingCenter)
                               .Where(x => x.Status != null && (x.Status.Id == AssetStatus.Indices.ACTIVE ||
                                                                x.Status.Id == AssetStatus.Indices.INSTALLED ||
                                                                x.Status.Id == AssetStatus.Indices.REQUEST_RETIREMENT))
                               .WithSubquery.WhereNotExists(subQuery);

            return query;
        }

        #endregion

        #region Exposed Methods

        public IEnumerable<int> RouteByTownId(int townId)
        {
            return
                Linq.Where(x => x.Town.Id == townId && x.Route.HasValue)
                    .OrderBy(x => x.Route.Value)
                    .Select(x => x.Route.Value)
                    .Distinct();
        }

        public IEnumerable<SewerOpening> FindActiveByTownId(int townId)
        {
            return Linq.Where(x => x.Town.Id == townId)
                       .Where(x => x.Status != null && x.Status.Id == AssetStatus.Indices.ACTIVE)
                       .OrderBy(x => x.OpeningSuffix).ToList();
        }

        public IEnumerable<SewerOpening> ByStreetId(int streetId)
        {
            return Linq.Where(x => x.Street.Id == streetId).OrderBy(x => x.OpeningSuffix).ToList();
        }

        public IQueryable<SewerOpening> ByStreetIdForWorkOrders(int streetId)
        {
            return Linq.Where(x => x.Street.Id == streetId && x.Status != null
                                                           && x.Status.Id != AssetStatus.Indices.RETIRED
                                                           && x.Status.Id != AssetStatus.Indices.REMOVED
                                                           && x.Status.Id != AssetStatus.Indices.CANCELLED)
                       .OrderBy(x => x.OpeningSuffix);
        }

        public IEnumerable<SewerOpening> GetAllForDropDown()
        {
            return
                Linq.OrderBy(x => x.Town)
                    .ThenBy(x => x.OpeningSuffix).Select(
                         x => new SewerOpening
                             { Id = x.Id, OpeningNumber = x.OpeningNumber, OpeningSuffix = x.OpeningSuffix })
                ;
        }

        public IEnumerable<SewerOpening> GetSewerOpeningsWithSapRetryIssues()
        {
            return this.GetSewerOpeningsWithSapRetryIssuesImpl();
        }

        public IEnumerable<AssetCoordinate> GetAssetCoordinates(IAssetCoordinateSearch search)
        {
            // NOTE: NO ROLE FILTERING IS DONE FOR THIS QUERY. Alex said so. -Ross 4/15/2015
            // NOTE 2: If you fix something in here then the same change most likely needs to be made in ValveRepository.
            //         And also in Hydrant.ToAssetCoordinate.

            search.EnablePaging = false;
            var query = Session.QueryOver<SewerOpening>();

            Coordinate coord = null;
            // We don't want null coordinates included, so inner join
            query.JoinAlias(x => x.Coordinate, () => coord, JoinType.InnerJoin);
            OperatingCenter opc = null;
            query.JoinAlias(x => x.OperatingCenter, () => opc);

            query.Where(() => coord.Latitude >= search.LatitudeMin && coord.Latitude <= search.LatitudeMax)
                 .Where(() => coord.Longitude >= search.LongitudeMin && coord.Longitude <= search.LongitudeMax);

            HydrantAssetCoordinate result = null;
            query.SelectList(x => x.Select(h => h.Id).WithAlias(() => result.Id)
                                   .Select(() => coord.Latitude).WithAlias(() => result.Latitude)
                                   .Select(() => coord.Longitude).WithAlias(() => result.Longitude)
                                   .Select(() => true).WithAlias(() => result.IsActive)
                                   .Select(() => true).WithAlias(() => result.IsPublic));

            // This query need an order by statement or else it will return the results in 
            // a random order for some reason. This is to ensure the data's always returned
            // in the exact same order so it can be displayed in the same order. 
            query.OrderBy(x => x.Id).Asc();

            query.TransformUsing(Transformers.AliasToBean<SewerOpeningAssetCoordinate>());
            return Search(search, query);
        }

        public IEnumerable<SewerOpeningAssetCoordinate> SearchForMap(ISearchSewerOpeningForMap search)
        {
            SewerOpening sewerOpening = null;
            Coordinate coord = null;
            SewerOpeningAssetCoordinate result = null;
            AssetStatus status = null;

            var query = Session.QueryOver(() => sewerOpening)
                               .JoinAlias(x => x.Coordinate, () => coord, JoinType.InnerJoin)
                               .JoinAlias(x => x.Status, () => status)
                               .SelectList(x =>
                                    x.Select(h => h.Id).WithAlias(() => result.Id)
                                     .Select(() => coord.Latitude).WithAlias(() => result.Latitude)
                                     .Select(() => coord.Longitude).WithAlias(() => result.Longitude)
                                     .Select(() => false).WithAlias(() => result.RequiresInspection)
                                     .Select(() => true).WithAlias(() => result.IsPublic)
                                     .Select(Projections.Conditional(
                                          Restrictions.In(
                                              Projections.Property(() => status.Id),
                                              AssetStatus.ACTIVE_STATUSES.ToArray()),
                                          Projections.Constant(true),
                                          Projections.Constant(false)
                                      )).WithAlias(() => result.IsActive))
                               .OrderBy(x => x.Id).Asc()
                               .TransformUsing(Transformers.AliasToBean<SewerOpeningAssetCoordinate>());

            return Search(search, query);
        }

        public IEnumerable<SewerOpening> GetNpdesRegulatorsDueInspection(ISearchNpdesRegulatorsDueInspectionItem search)
        {
            var query = GetNpdesRegulatorDueInspectionSearchQuery(search);
            return Search(search, query);
        }

        public IEnumerable<SewerOpeningAssetCoordinate> SearchNpdesRegulatorsDueInspectionForMap(
            ISearchNpdesRegulatorsDueInspectionForMap search)
        {
            Coordinate coord = null;
            SewerOpeningAssetCoordinate result = null;

            var query = GetNpdesRegulatorDueInspectionSearchQuery(search)
                       .JoinAlias(x => x.Coordinate, () => coord, JoinType.InnerJoin)
                       .SelectList(x =>
                            x.Select(h => h.Id).WithAlias(() => result.Id)
                             .Select(() => coord.Latitude).WithAlias(() => result.Latitude)
                             .Select(() => coord.Longitude).WithAlias(() => result.Longitude)
                             .Select(() => true).WithAlias(() => result.RequiresInspection)
                             .Select(() => true).WithAlias(() => result.IsPublic)
                             .Select(() => true).WithAlias(() => result.IsActive))
                       .OrderBy(x => x.Id).Asc()
                       .TransformUsing(Transformers.AliasToBean<SewerOpeningAssetCoordinate>());

            return Search(search, query);
        }

        public IEnumerable<NpdesRegulatorsDueInspectionReportItem> GetNpdesRegulatorsDueInspectionReport(
            ISearchNpdesRegulatorsDueInspectionReportItem search)
        {
            SewerOpening sewerOpening = null;
            OperatingCenter opc = null;
            Town town = null;
            AssetStatus status = null;

            var subQuery = QueryOver.Of<NpdesRegulatorInspection>().Where(x => x.SewerOpening.Id == sewerOpening.Id);
            switch (search.DepartureDateTime.Operator)
            {
                case RangeOperator.Between:
                    subQuery = subQuery.Where(x => x.DepartureDateTime >=
                                                   ((DateTime)search.DepartureDateTime.Start)
                                                  .BeginningOfDay() &&
                                                   x.DepartureDateTime <= ((DateTime)search.DepartureDateTime.End)
                                                                         .BeginningOfDay().AddDays(1));
                    break;
                case RangeOperator.Equal:
                    subQuery = subQuery.Where(x => x.DepartureDateTime >=
                                                   ((DateTime)search.DepartureDateTime.End)
                                                  .BeginningOfDay() &&
                                                   x.DepartureDateTime <= ((DateTime)search.DepartureDateTime.End)
                                                                         .BeginningOfDay().AddDays(1));
                    break;
                case RangeOperator.GreaterThan:
                    subQuery = subQuery.Where(x => x.DepartureDateTime >=
                                                   ((DateTime)search.DepartureDateTime.End).BeginningOfDay()
                                                  .AddDays(1));
                    break;
                case RangeOperator.GreaterThanOrEqualTo:
                    subQuery = subQuery.Where(x => x.DepartureDateTime >=
                                                   ((DateTime)search.DepartureDateTime.End).BeginningOfDay());
                    break;
                case RangeOperator.LessThan:
                    subQuery = subQuery.Where(x => x.DepartureDateTime <
                                                   ((DateTime)search.DepartureDateTime.End).BeginningOfDay()
                                                  .AddDays(1));
                    break;
                case RangeOperator.LessThanOrEqualTo:
                    subQuery = subQuery.Where(x => x.DepartureDateTime <
                                                   ((DateTime)search.DepartureDateTime.End).BeginningOfDay());
                    break;
            }

            subQuery = subQuery.SelectList(x =>
                x.Select(y => y.SewerOpening.Id));

            var query = Session.QueryOver(() => sewerOpening)
                               .JoinAlias(x => x.OperatingCenter, () => opc)
                               .JoinAlias(x => x.Town, () => town)
                               .JoinAlias(x => x.Status, () => status, JoinType.LeftOuterJoin)
                               .Where(x => x.SewerOpeningType.Id == SewerOpeningType.Indices.NPDES_REGULATOR)
                               .Where(x => x.OperatingCenter.Id == search.OperatingCenter)
                               .Where(x => x.Status != null && (x.Status.Id == AssetStatus.Indices.ACTIVE ||
                                                                x.Status.Id == AssetStatus.Indices.INSTALLED ||
                                                                x.Status.Id == AssetStatus.Indices.REQUEST_RETIREMENT))
                               .WithSubquery.WhereNotExists(subQuery);

            query.SelectList(x => x.SelectGroup(so => opc.OperatingCenterCode).WithAlias(() => search.OperatingCenter)
                                   .SelectGroup(o => opc.Id).WithAlias(() => search.OperatingCenterId)
                                   .SelectGroup(so => town.ShortName).WithAlias(() => search.Town)
                                   .SelectGroup(o => town.Id).WithAlias(() => search.TownId)
                                   .SelectGroup(s => status.Description).WithAlias(() => search.Status)
                                   .SelectGroup(o => status.Id).WithAlias(() => search.StatusId)
                                   .SelectCount(y => y.Town).WithAlias(() => search.Count));

            query.TransformUsing(Transformers.AliasToBean<NpdesRegulatorsDueInspectionReportItem>());
            return Search(search, query);
        }

        public static Expression<Func<SewerOpening, bool>> GetLinqForSpecificNumberingSystem(
            IAbbreviationTypeRepository abbrRepo, OperatingCenter operatingCenter, Town town, TownSection townSection)
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

            throw new NotSupportedException();
        }

        #endregion

        public SewerOpeningRepository(ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo) : base(session,
            container,
            authenticationService, roleRepo) { }
    }

    public static class ISewerOpeningRepositoryExtensions
    {
        public static IEnumerable<SewerOpening> GetSewerOpeningsWithSapRetryIssuesImpl(
            this IRepository<SewerOpening> that)
        {
            return that.Where(x =>
                x.SAPErrorCode != null && x.SAPErrorCode != "" && x.SAPErrorCode.StartsWith("RETRY"));
        }
    }
}
