using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Data;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using NHibernate.Type;
using NHibernate.Util;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class BlowOffInspectionRepository : MapCallSecuredRepositoryBase<BlowOffInspection>,
        IBlowOffInspectionRepository
    {
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
                                   // Really annoying that NHibernate can't figure out the proper join type based on mapping.
                                  .CreateAlias("v.Coordinate", "coord", JoinType.LeftOuterJoin);

                if (CurrentUserCanAccessAllTheRecords)
                {
                    return critter;
                }

                return critter.Add(Restrictions.In("oc.Id", GetUserOperatingCenterIds()));
            }
        }

        public override IQueryable<BlowOffInspection> Linq
        {
            get
            {
                if (CurrentUserCanAccessAllTheRecords)
                {
                    return base.Linq;
                }

                var opCenterIds = GetUserOperatingCenterIds();
                return base.Linq.Where(x => opCenterIds.Contains(x.Valve.OperatingCenter.Id));
            }
        }

        public override RoleModules Role
        {
            get { return RoleModules.FieldServicesAssets; }
        }

        #endregion

        #region Constructors

        public BlowOffInspectionRepository(IRepository<AggregateRole> roleRepo, ISession session, IContainer container,
            IAuthenticationService<User> authenticationService) : base(session, container, authenticationService,
            roleRepo) { }

        #endregion

        #region Exposed Methods

        public override void Delete(BlowOffInspection entity)
        {
            // This needs to be removed from the valve's inspections or else there's a cascade error when deleting.
            entity.Valve.BlowOffInspections.Remove(entity);
            base.Delete(entity);
        }

        public IEnumerable<BlowOffInspectionSearchResultViewModel> SearchInspections(ISearchBlowOffInspection search)
        {
            var query = Session.QueryOver<BlowOffInspection>();

            BlowOffInspectionSearchResultViewModel result = null;
            Valve val = null;
            query.JoinAlias(x => x.Valve, () => val, JoinType.LeftOuterJoin);
            OperatingCenter opc = null;
            query.JoinAlias(x => val.OperatingCenter, () => opc, JoinType.LeftOuterJoin);
            Town town = null;
            query.JoinAlias(x => val.Town, () => town, JoinType.LeftOuterJoin);
            User inspectedBy = null;
            query.JoinAlias(x => x.InspectedBy, () => inspectedBy, JoinType.LeftOuterJoin);
            HydrantInspectionType hit = null;
            query.JoinAlias(x => x.HydrantInspectionType, () => hit, JoinType.LeftOuterJoin);
            WorkOrderRequest wor1 = null;
            query.JoinAlias(x => x.WorkOrderRequestOne, () => wor1, JoinType.LeftOuterJoin);
            Coordinate coord = null;
            query.JoinAlias(x => val.Coordinate, () => coord, JoinType.LeftOuterJoin);
            NoReadReason freeNoRead = null;
            query.JoinAlias(x => x.FreeNoReadReason, () => freeNoRead, JoinType.LeftOuterJoin);
            NoReadReason totalNoRead = null;
            query.JoinAlias(x => x.TotalNoReadReason, () => totalNoRead, JoinType.LeftOuterJoin);

            if (!CurrentUserCanAccessAllTheRecords)
            {
                query.Where(Restrictions.On<BlowOffInspection>(x => opc.Id).IsIn(GetUserOperatingCenterIds()));
            }

            query.SelectList(x => x
                                 .Select(bi => bi.Id).WithAlias(() => result.Id)
                                 .Select(bi => bi.DateInspected).WithAlias(() => result.DateInspected)
                                 .Select(bi => bi.CreatedAt).WithAlias(() => result.DateAdded)
                                 .Select(bi => bi.Remarks).WithAlias(() => result.Remarks)
                                 .Select(bi => bi.GallonsFlowed).WithAlias(() => result.GallonsFlowed)
                                 .Select(bi => bi.FullFlow).WithAlias(() => result.FullFlow)
                                 .Select(bi => bi.GPM).WithAlias(() => result.GPM)
                                 .Select(bi => bi.MinutesFlowed).WithAlias(() => result.MinutesFlowed)
                                 .Select(bi => bi.PreResidualChlorine).WithAlias(() => result.PreResidualChlorine)
                                 .Select(bi => bi.ResidualChlorine).WithAlias(() => result.ResidualChlorine)
                                 .Select(bi => bi.StaticPressure).WithAlias(() => result.StaticPressure)
                                 .Select(bi => bi.PreTotalChlorine).WithAlias(() => result.PreTotalChlorine)
                                 .Select(bi => bi.TotalChlorine).WithAlias(() => result.TotalChlorine)
                                 .Select(_ => val.Id).WithAlias(() => result.ValveId)
                                 .Select(_ => val.ValveNumber).WithAlias(() => result.ValveNumber)
                                 .Select(_ => coord.Latitude).WithAlias(() => result.Latitude)
                                 .Select(_ => coord.Longitude).WithAlias(() => result.Longitude)
                                 .Select(_ => opc.OperatingCenterCode).WithAlias(() => result.OperatingCenter)
                                 .Select(_ => town.ShortName).WithAlias(() => result.Town)
                                 .Select(_ => inspectedBy.UserName).WithAlias(() => result.InspectedBy)
                                 .Select(_ => hit.Description).WithAlias(() => result.HydrantInspectionType)
                                 .Select(_ => wor1.Description).WithAlias(() => result.WorkOrderRequestOne)
                                 .Select(bi => bi.SAPErrorCode).WithAlias(() => result.SAPErrorCode)
                                 .Select(_ => freeNoRead.Description).WithAlias(() => result.FreeNoReadReason)
                                 .Select(_ => totalNoRead.Description).WithAlias(() => result.TotalNoReadReason)
            );

            query.TransformUsing(Transformers.AliasToBean<BlowOffInspectionSearchResultViewModel>());

            var blowOffInspections = Search(search, query);

            blowOffInspections.ForEach(x => x.GallonsFlowed = (int?)(x.GPM * x.MinutesFlowed));

            return blowOffInspections;
        }

        public IEnumerable<AssetCoordinate> SearchBlowOffInspectionsForMap(ISearchBlowOffInspection search)
        {
            // This method's for maps only so we don't want paging.
            // Also the sorting is being hardcoded because the maps 

            var query = Session.QueryOver<BlowOffInspection>();
            // Setup aliases
            Valve val = null;
            query.JoinAlias(x => x.Valve, () => val, JoinType.LeftOuterJoin);
            OperatingCenter opc = null;
            query.JoinAlias(x => val.OperatingCenter, () => opc, JoinType.LeftOuterJoin);
            Town town = null;
            query.JoinAlias(x => val.Town, () => town, JoinType.LeftOuterJoin);
            User inspectedBy = null;
            query.JoinAlias(x => x.InspectedBy, () => inspectedBy, JoinType.LeftOuterJoin);
            HydrantInspectionType hit = null;
            query.JoinAlias(x => x.HydrantInspectionType, () => hit, JoinType.LeftOuterJoin);
            WorkOrderRequest wor1 = null;
            query.JoinAlias(x => x.WorkOrderRequestOne, () => wor1, JoinType.LeftOuterJoin);
            Coordinate coord = null;
            query.JoinAlias(x => val.Coordinate, () => coord, JoinType.LeftOuterJoin);

            if (!CurrentUserCanAccessAllTheRecords)
            {
                query.Where(Restrictions.On<BlowOffInspection>(x => opc.Id).IsIn(GetUserOperatingCenterIds()));
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

        public IEnumerable<InspectionProductivityReportItem> GetInspectionProductivityReport(
            ISearchInspectionProductivity search)
        {
            var startDate = search.StartDate.Value.Date;
            var endDate = startDate.AddDays(search.GetDays());

            search.EnablePaging = false;
            var query = Session.QueryOver<BlowOffInspection>();
            InspectionProductivityReportItem result = null;
            Valve asset = null;
            query.JoinAlias(x => x.Valve, () => asset);
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
                                   .Select(() => "BlowOff").WithAlias(() => result.AssetType)
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

        public IEnumerable<BlowOffInspection> GetBlowOffInspectionsWithSapRetryIssues()
        {
            return this.GetBlowOffInspectionsWithSapRetryIssuesImpl();
        }

        #endregion
    }

    public static class IBlowOffInspectionRepositoryExtensions
    {
        public static IEnumerable<BlowOffInspection> GetBlowOffInspectionsWithSapRetryIssuesImpl(
            this IRepository<BlowOffInspection> that)
        {
            return that.Where(x =>
                x.SAPErrorCode != null && x.SAPErrorCode != "" && x.SAPErrorCode.StartsWith("RETRY"));
        }
    }

    public interface IBlowOffInspectionRepository : IRepository<BlowOffInspection>
    {
        IEnumerable<BlowOffInspection> GetBlowOffInspectionsWithSapRetryIssues();
        IEnumerable<BlowOffInspectionSearchResultViewModel> SearchInspections(ISearchBlowOffInspection search);
        IEnumerable<AssetCoordinate> SearchBlowOffInspectionsForMap(ISearchBlowOffInspection search);

        IEnumerable<InspectionProductivityReportItem> GetInspectionProductivityReport(
            ISearchInspectionProductivity search);
    }
}
