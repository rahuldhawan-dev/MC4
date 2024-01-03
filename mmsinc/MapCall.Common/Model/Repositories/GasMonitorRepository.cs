using System;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using System.Linq;
using MapCall.Common.Data;
using MMSINC.ClassExtensions.DateTimeExtensions;
using NHibernate;
using StructureMap;
using MMSINC.Authentication;
using MapCall.Common.Model.Entities.Users;
using NHibernate.Criterion;
using NHibernate.SqlCommand;

namespace MapCall.Common.Model.Repositories
{
    public class GasMonitorRepository : MapCallSecuredRepositoryBase<GasMonitor>, IGasMonitorRepository
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.OperationsLockoutForms;

        #endregion

        #region Properties

        public override RoleModules Role => ROLE;

        public override ICriteria Criteria
        {
            get
            {
                var crit = base.Criteria
                               .CreateAlias("Equipment", "criteriaEquipment", JoinType.LeftOuterJoin)
                               .CreateAlias("criteriaEquipment.Facility", "criteriaFacility",
                                    JoinType.LeftOuterJoin)
                               .CreateAlias("criteriaFacility.OperatingCenter", "criteriaOperatingCenter",
                                    JoinType.LeftOuterJoin);

                if (!CurrentUserCanAccessAllTheRecords)
                {
                    var operatingCenterIds = GetUserOperatingCenterIds();
                    crit = crit.Add(Restrictions.In("criteriaOperatingCenter.Id", operatingCenterIds));
                }

                return crit;
            }
        }

        public override IQueryable<GasMonitor> Linq
        {
            get
            {
                var linq = base.Linq;

                if (!CurrentUserCanAccessAllTheRecords)
                {
                    var operatingCenterIds = GetUserOperatingCenterIds();
                    linq = linq.Where(x => operatingCenterIds.Contains(x.Equipment.OperatingCenter.Id));
                }

                return linq;
            }
        }

        #endregion

        #region Constructor

        public GasMonitorRepository(ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo) : base(session, container,
            authenticationService, roleRepo) { }

        #endregion
    }

    public interface IGasMonitorRepository : IRepository<GasMonitor> { }

    public static class GasMonitorRepositoryExtensions
    {
        /// <summary>
        /// GasMonitors that do not have a valid calibration that is within
        /// the last "CalibrationFrequencyDays" from the date provided
        /// </summary>
        /// <param name="that"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static IQueryable<GasMonitor> GetWithCalibrationDueSevenDaysFrom(this IRepository<GasMonitor> that,
            DateTime date)
        {
            var sevenDaysFromNow = date.AddDays(7);
            var beginningOfDay = sevenDaysFromNow.BeginningOfDay();
            var endOfDay = sevenDaysFromNow.EndOfDay();
            return that.Where(x =>
                x.MostRecentPassingGasMonitorCalibration.NextDueDate >= beginningOfDay
                &&
                x.MostRecentPassingGasMonitorCalibration.NextDueDate <= endOfDay
            );
        }
    }
}
