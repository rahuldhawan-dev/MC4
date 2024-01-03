using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using NHibernate.Criterion;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class VehicleRepository : RepositoryBase<Vehicle>, IVehicleRepository
    {
        #region Constructor

        public VehicleRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Public Methods

        public IEnumerable<Vehicle> GetByOperatingCenterId(int id)
        {
            return Linq.Where(x => x.OperatingCenter.Id == id);
        }

        public IEnumerable<VehicleUtilizationReportItem> SearchVehicleUtilization(
            ISearchVehicleUtilizationReport search)
        {
            var query = Session.QueryOver<Vehicle>();

            VehicleUtilizationReportItem result = null;
            OperatingCenter opc = null;
            query.JoinAlias(x => x.OperatingCenter, () => opc, JoinType.InnerJoin);
            VehicleStatus vStat = null;
            query.JoinAlias(x => x.Status, () => vStat, JoinType.LeftOuterJoin);
            VehicleAssignmentStatus assStat = null;
            query.JoinAlias(x => x.AssignmentStatus, () => assStat, JoinType.LeftOuterJoin);
            VehicleAssignmentCategory assCat = null;
            query.JoinAlias(x => x.AssignmentCategory, () => assCat, JoinType.LeftOuterJoin);
            VehicleAssignmentJustification assJust = null;
            query.JoinAlias(x => x.AssignmentJustification, () => assJust, JoinType.LeftOuterJoin);
            VehicleAccountingRequirement var = null;
            query.JoinAlias(x => x.AccountingRequirement, () => var, JoinType.LeftOuterJoin);
            Facility fac = null;
            query.JoinAlias(x => x.Facility, () => fac, JoinType.LeftOuterJoin);
            VehicleType vType = null;
            query.JoinAlias(x => x.Type, () => vType, JoinType.LeftOuterJoin);
            VehiclePrimaryUse vpu = null;
            query.JoinAlias(x => x.PrimaryVehicleUse, () => vpu, JoinType.LeftOuterJoin);
            Vehicle replacement = null;
            query.JoinAlias(x => x.ReplacementVehicle, () => replacement, JoinType.LeftOuterJoin);
            Employee primeDrive = null;
            query.JoinAlias(x => x.PrimaryDriver, () => primeDrive, JoinType.LeftOuterJoin);
            Employee manager = null;
            query.JoinAlias(x => x.Manager, () => manager, JoinType.InnerJoin);
            VehicleDepartment dep = null;
            query.JoinAlias(x => x.Department, () => dep, JoinType.LeftOuterJoin);

            query.SelectList(x => x.Select(() => opc.OperatingCenterCode).WithAlias(() => result.OperatingCenter)
                                   .Select(y => y.Flag).WithAlias(() => result.Flag)
                                   .Select(() => vStat.Description).WithAlias(() => result.VehicleStatus)
                                   .Select(() => assStat.Description).WithAlias(() => result.AssignmentStatus)
                                   .Select(() => assCat.Description).WithAlias(() => result.AssignmentCategory)
                                   .Select(() => assJust.Description).WithAlias(() => result.AssignmentJustification)
                                   .Select(() => var.Description).WithAlias(() => result.AccountingRequirement)
                                   .Select(y => y.Id).WithAlias(() => result.VehicleId)
                                   .Select(() => fac.FacilityName).WithAlias(() => result.Facility)
                                   .Select(y => y.PoolUse).WithAlias(() => result.PoolUse)
                                   .Select(() => vType.Description).WithAlias(() => result.VehicleType)
                                   .Select(y => y.Model).WithAlias(() => result.Model)
                                   .Select(y => y.PlateNumber).WithAlias(() => result.PlateNumber)
                                   .Select(() => vpu.Description).WithAlias(() => result.PrimaryVehicleUse)
                                   .Select(() => replacement.Id).WithAlias(() => result.ReplacementVehicleId)
                                   .Select(() => primeDrive.FirstName).WithAlias(() => result.PrimaryDriverFirstName)
                                   .Select(() => primeDrive.LastName).WithAlias(() => result.PrimaryDriverLastName)
                                   .Select(() => primeDrive.MiddleName).WithAlias(() => result.PrimaryDriverMiddle)
                                   .Select(() => manager.FirstName).WithAlias(() => result.ManagerFirstName)
                                   .Select(() => manager.LastName).WithAlias(() => result.ManagerLastName)
                                   .Select(() => manager.MiddleName).WithAlias(() => result.ManagerMiddle)
                                   .Select(() => dep.Description).WithAlias(() => result.Department)
            );

            query.TransformUsing(Transformers.AliasToBean<VehicleUtilizationReportItem>());

            // So, all of this data has to be displayed, so there's no reason to pull hair out
            // over getting NHibernate to somehow play nice with all the grouping and ordering
            // involved. Doing this in memory with Linq is fast.
            var results = Search(search, query);

            search.Results = results.GroupBy(x => new {
                                         x.OperatingCenter,
                                         x.Department,
                                         x.Manager,
                                     }).OrderBy(x => x.Key.Manager)
                                    .ThenBy(x => x.Key.OperatingCenter)
                                    .ThenBy(x => x.Key.Department)
                                    .SelectMany(y => y.OrderBy(x => x.Flag)
                                                      .ThenBy(x => x.AssignmentCategory)).ToList();

            return search.Results;
        }

        #endregion
    }

    public interface IVehicleRepository : IRepository<Vehicle>
    {
        IEnumerable<Vehicle> GetByOperatingCenterId(int id);
        IEnumerable<VehicleUtilizationReportItem> SearchVehicleUtilization(ISearchVehicleUtilizationReport search);
    }
}
