using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Data;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using StructureMap;
using NHibernate.Transform;

namespace MapCall.Common.Model.Repositories
{
    public class NpdesRegulatorInspectionRepository : MapCallSecuredRepositoryBase<NpdesRegulatorInspection>,
        INpdesRegulatorInspectionRepository
    {
        #region Properties

        public override ICriteria Criteria
        {
            get
            {
                var critter = base.Criteria
                                  .CreateAlias("SewerOpening", "sm")
                                  .CreateAlias("sm.OperatingCenter", "oc", JoinType.LeftOuterJoin);

                if (CurrentUserCanAccessAllTheRecords)
                {
                    return critter;
                }

                return critter.Add(Restrictions.In("oc.Id", GetUserOperatingCenterIds()));
            }
        }

        public override IQueryable<NpdesRegulatorInspection> Linq
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

        public override RoleModules Role => RoleModules.FieldServicesAssets; 

        #endregion

        #region Exposed Methods

        public IEnumerable<int> GetDistinctYearsCompleted()
        {
            return
                (from vi in base.Linq
                 select vi.DepartureDateTime.Year
                ).Distinct();
        }

        public IEnumerable<NpdesRegulatorInspectionReportItem> SearchNpdesRegulatorInspectionReport(
            ISearchNpdesRegulatorInspectionReport search)
        {
            NpdesRegulatorInspectionReportItem result = null;
            var query = Session.QueryOver<SewerOpening>();

            NpdesRegulatorInspection npdesRegulatorInspection = null;
            query.JoinAlias(x => x.NpdesRegulatorInspections, () => npdesRegulatorInspection);

            OperatingCenter opc = null;
            query.JoinAlias(x => x.OperatingCenter, () => opc);

            Town town = null;
            query.JoinAlias(x => x.Town, () => town);

            WasteWaterSystem wws = null;
            query.JoinAlias(x => x.WasteWaterSystem, () => wws);

            if (search.Year != null)
            {
                query = query.Where(x => npdesRegulatorInspection.DepartureDateTime.Date.Year == search.Year);
            }

            if (search.DepartureDateTime != null && search.DepartureDateTime.End != null)
            {
                switch (search.DepartureDateTime.Operator)
                {
                    case RangeOperator.Between:
                        query = query.Where(x =>
                            npdesRegulatorInspection.DepartureDateTime >= search.DepartureDateTime.Start.Value.BeginningOfDay() &&
                            npdesRegulatorInspection.DepartureDateTime <= search.DepartureDateTime.End.Value.EndOfDay());
                        break;
                    case RangeOperator.Equal:
                        query = query.Where(x => npdesRegulatorInspection.DepartureDateTime.Date == search.DepartureDateTime.End.Value.Date);
                        break;
                    case RangeOperator.GreaterThan:
                        query = query.Where(x => npdesRegulatorInspection.DepartureDateTime > search.DepartureDateTime.End.Value.EndOfDay());
                        break;
                    case RangeOperator.GreaterThanOrEqualTo:
                        query = query.Where(x => npdesRegulatorInspection.DepartureDateTime >= search.DepartureDateTime.End.Value.BeginningOfDay());
                        break;
                    case RangeOperator.LessThan:
                        query = query.Where(x => npdesRegulatorInspection.DepartureDateTime < search.DepartureDateTime.End.Value.BeginningOfDay());
                        break;
                    case RangeOperator.LessThanOrEqualTo:
                        query = query.Where(x => npdesRegulatorInspection.DepartureDateTime <= search.DepartureDateTime.End.Value.EndOfDay());
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }

            query.SelectList(x =>
                x.Select(h => npdesRegulatorInspection.Id).WithAlias(() => result.InspectionId)
                 .Select(h => town.ShortName).WithAlias(() => result.Town)
                 .Select(h => h.LocationDescription).WithAlias(() => result.LocationDescription)
                 .Select(h => h.WasteWaterSystem).WithAlias(() => result.WasteWaterSystem)
                 .Select(h => wws.PermitNumber).WithAlias(() => result.PermitNumber)
                 .Select(h => h.OutfallNumber).WithAlias(() => result.OutfallNumber)
                 .Select(h => npdesRegulatorInspection.DepartureDateTime).WithAlias(() => result.DepartureDateTime)
                 .Select(h => npdesRegulatorInspection.InspectedBy).WithAlias(() => result.InspectedBy)
                 .Select(h => npdesRegulatorInspection.NpdesRegulatorInspectionType).WithAlias(() => result.InspectionType)
                 .Select(h => npdesRegulatorInspection.BlockCondition).WithAlias(() => result.BlockCondition)
                 .Select(h => npdesRegulatorInspection.IsDischargePresent).WithAlias(() => result.DischargePresent)
                 .Select(h => npdesRegulatorInspection.WeatherCondition).WithAlias(() => result.WeatherRelated)
                 .Select(h => npdesRegulatorInspection.RainfallEstimate).WithAlias(() => result.RainfallEstimate)
                 .Select(h => npdesRegulatorInspection.DischargeFlow).WithAlias(() => result.DischargeFlow)
                 .Select(h => npdesRegulatorInspection.DischargeDuration).WithAlias(() => result.DischargeDuration)
                 .Select(h => npdesRegulatorInspection.DischargeCause).WithAlias(() => result.DischargeCause)
                 .Select(h => h.BodyOfWater).WithAlias(() => result.BodyOfWater)
                 .Select(h => npdesRegulatorInspection.Remarks).WithAlias(() => result.Remarks));

            query.TransformUsing(Transformers.AliasToBean<NpdesRegulatorInspectionReportItem>());
            return Search(search, query);
        }

        #endregion

        #region Constructors

        public NpdesRegulatorInspectionRepository(ISession session, IContainer container, IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo) 
            : base(session, container, authenticationService, roleRepo) { }

        #endregion
    }

    public interface INpdesRegulatorInspectionRepository : IRepository<NpdesRegulatorInspection>
    {
        IEnumerable<int> GetDistinctYearsCompleted();
        IEnumerable<NpdesRegulatorInspectionReportItem> SearchNpdesRegulatorInspectionReport(
            ISearchNpdesRegulatorInspectionReport model);
    }
}