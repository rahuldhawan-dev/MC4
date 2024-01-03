using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Data;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class SewerOpeningInspectionRepository : MapCallSecuredRepositoryBase<SewerOpeningInspection>,
        ISewerOpeningInspectionRepository
    {
        #region Properties

        public override ICriteria Criteria
        {
            get
            {
                var critter = base.Criteria
                                  .CreateAlias("SewerOpening", "sm")
                                  .CreateAlias("sm.OperatingCenter", "oc")
                                  .CreateAlias("sm.Coordinate", "coord", JoinType.LeftOuterJoin);

                if (CurrentUserCanAccessAllTheRecords)
                {
                    return critter;
                }

                return critter.Add(Restrictions.In("oc.Id", GetUserOperatingCenterIds()));
            }
        }

        public override IQueryable<SewerOpeningInspection> Linq
        {
            get
            {
                if (CurrentUserCanAccessAllTheRecords)
                {
                    return base.Linq;
                }

                var opCenterIds = GetUserOperatingCenterIds();
                return base.Linq.Where(x => opCenterIds.Contains(x.SewerOpening.OperatingCenter.Id));
            }
        }

        public override RoleModules Role
        {
            get { return RoleModules.FieldServicesAssets; }
        }

        #endregion

        #region Exposed Methods

        public IEnumerable<SewerOpeningInspectionSearchResultViewModel> SearchInspections(
            ISearchSewerOpeningInspection search)
        {
            var query = Session.QueryOver<SewerOpeningInspection>();
            SewerOpeningInspectionSearchResultViewModel result = null;

            SewerOpening sm = null;
            query.JoinAlias(x => x.SewerOpening, () => sm, JoinType.LeftOuterJoin);

            OperatingCenter opc = null;
            query.JoinAlias(x => sm.OperatingCenter, () => opc, JoinType.LeftOuterJoin);

            Town town = null;
            query.JoinAlias(x => sm.Town, () => town, JoinType.LeftOuterJoin);

            User inspectedBy = null;
            query.JoinAlias(x => x.InspectedBy, () => inspectedBy, JoinType.LeftOuterJoin);

            Coordinate coord = null;
            query.JoinAlias(x => sm.Coordinate, () => coord, JoinType.LeftOuterJoin);

            FunctionalLocation functionalLocation = null;
            query.JoinAlias(x => sm.FunctionalLocation, () => functionalLocation, JoinType.LeftOuterJoin);

            if (!CurrentUserCanAccessAllTheRecords)
            {
                query.Where(Restrictions.On<SewerOpeningInspection>(x => opc.Id).IsIn(GetUserOperatingCenterIds()));
            }

            query.SelectList(x => x
                                 .Select(si => si.Id).WithAlias(() => result.Id)
                                 .Select(si => sm.Id).WithAlias(() => result.SewerOpeningId)
                                 .Select(si => sm.OpeningNumber).WithAlias(() => result.OpeningNumber)
                                 .Select(si => sm.OpeningSuffix).WithAlias(() => result.OpeningSuffix)
                                 .Select(si => sm.Route).WithAlias(() => result.Route)
                                 .Select(si => opc.OperatingCenterCode).WithAlias(() => result.OperatingCenter)
                                 .Select(si => town.ShortName).WithAlias(() => result.Town)
                                 .Select(si => functionalLocation.Description)
                                 .WithAlias(() => result.FunctionalLocation)
                                 .Select(si => coord.Latitude).WithAlias(() => result.Latitude)
                                 .Select(si => coord.Longitude).WithAlias(() => result.Longitude)
                                 .Select(si => si.DateInspected).WithAlias(() => result.DateInspected)
                                 .Select(si => si.AmountOfDebrisGritCubicFeet)
                                 .WithAlias(() => result.AmountOfDebrisGritCubicFeet)
                                 .Select(si => si.Remarks).WithAlias(() => result.Remarks)
                                 .Select(si => inspectedBy.UserName).WithAlias(() => result.InspectedBy)
                                 .Select(si => si.CreatedAt).WithAlias(() => result.DateAdded)
            );

            query.TransformUsing(Transformers.AliasToBean<SewerOpeningInspectionSearchResultViewModel>());

            return Search(search, query);
        }

        #endregion

        #region Constructors

        public SewerOpeningInspectionRepository(ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo
        ) : base(session, container, authenticationService, roleRepo) { }

        #endregion
    }

    public interface ISewerOpeningInspectionRepository : IRepository<SewerOpeningInspection>
    {
        IEnumerable<SewerOpeningInspectionSearchResultViewModel>
            SearchInspections(ISearchSewerOpeningInspection search);
    }
}
