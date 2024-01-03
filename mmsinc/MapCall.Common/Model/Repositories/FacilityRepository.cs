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
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class FacilityRepository : MapCallSecuredRepositoryBase<Facility>, IFacilityRepository
    {
        #region Fields

        private readonly ISensorRepository _sensorRepository;

        #endregion

        #region Constructors

        public FacilityRepository(ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo,
            ISensorRepository sensorRepository) : base(session, container, authenticationService, roleRepo)
        {
            _sensorRepository = sensorRepository;
        }

        #endregion

        #region Properties

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

        public override IQueryable<Facility> Linq
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

        public override RoleModules Role => RoleModules.ProductionFacilities;

        #endregion

        #region Exposed Methods

        public IQueryable<Facility> GetByOperatingCenterId(int operatingCenterId)
        {
            return Linq.Where(x => x.OperatingCenter.Id == operatingCenterId);
        }

        public IQueryable<Facility> GetByPublicWaterSupplyId(int publicWaterSupplyId)
        {
            return Linq.Where(x => x.PublicWaterSupply.Id == publicWaterSupplyId);
        }

        public IQueryable<Facility> GetByTownId(int? townId)
        {
            return (from f in Linq where f.Town.Id == townId select f);
        }

        public IQueryable<Facility> GetByOperatingCenterId(int? opCenterId)
        {
            return (from f in Linq where f.OperatingCenter.Id == opCenterId select f);
        }

        public IQueryable<Facility> GetByOperatingCenterIdAndCommunityRightToKnowIsTrue(int? operatingCenterId)
        {
            return Linq.Where(x => x.OperatingCenter.Id == operatingCenterId && x.CommunityRightToKnow == true);
        }

        public IDictionary<DateTime, double> GetReadings(int facilityId, ReadingGroupType interval, DateTime startDate,
            DateTime endDate)
        {
            startDate = startDate.BeginningOfDay();
            endDate = endDate.BeginningOfDay().AddDays(1);

            // NHibernate's Linq provider refuses to work when this is all one single query
            // because it doesn't know how to do things with GroupBy and Select news and SelectMany. 
            // Splitting this up works for whatever reason.
            // 
            // Having said that, the second query manages to do everything on the sql side so this
            // method will not read potentially hundreds of thousands of Reading records into memory
            // on the web server.
            var sensors = (from f in Linq
                           from e in f.Equipment
                           from s in e.Sensors
                           from r in s.Sensor.Readings
                           where
                               f.Id ==
                               facilityId // s.Sensor.MeasurementType == measType // go get all the readings instead
                           select s.Sensor).Distinct().ToArray();

            var readingSums = _sensorRepository.GetGroupedReadingCalculations(sensors, interval, startDate, endDate);

            return readingSums.Totals;
        }

        public IQueryable<Facility> GetByOperatingCenterIds(int[] operatingCenterIds)
        {
            return from f in Linq
                   where operatingCenterIds.Contains(f.OperatingCenter.Id)
                   select f;
        }

        public Facility FindWithEagerJoin(int id)
        {
            // Don't call base.
            // No reason to hit the database if id = 0, seeing as it can't exist.
            if (id <= 0)
            {
                return null;
            }

            // NOTE: This does not return from the session cache!
            var ret = Criteria
                     .SetFetchMode("Equipment", FetchMode.Eager)
                     .Add(GetIdEqCriterion(id)).UniqueResult<Facility>();

            if (ret != null)
            {
                _container.BuildUp(ret);
            }

            return ret;
        }

        #endregion
    }

    public static class FacilityRepositoryExtensions
    {
        public static int GetNextEquipmentNumberForFacilityByEquipmentPurposeId(this IRepository<Facility> that,
            int facilityId, int equipmentPurposeId)
        {
            var facility = that.Find(facilityId);
            var eq = (equipmentPurposeId == 0)
                ? facility.Equipment.Where(e => e.EquipmentPurpose == null).ToList()
                : facility.Equipment.Where(e => e.EquipmentPurpose != null && e.EquipmentPurpose.Id == equipmentPurposeId)
                          .ToList();
            var max = eq.Max(e => e.Number);

            return (max ?? 0) + 1;
        }

        public static IQueryable<Facility> GetForSelect(this IRepository<Facility> that,
            Expression<Func<Facility, bool>> filterP = null)
        {
            var data = filterP == null
                ? that.GetAll()
                : that.Where(filterP);
            return data
                  .OrderBy(f => f.FacilityName)
                  .Select(f => new Facility {
                       Id = f.Id,
                       FacilityName = f.FacilityName,
                       OperatingCenter = f.OperatingCenter,
                       Department = f.Department
                   });
        }
    }

    public interface IFacilityRepository : IRepository<Facility>
    {
        #region Abstract Methods

        IQueryable<Facility> GetByOperatingCenterId(int operatingCenterId);
        IQueryable<Facility> GetByPublicWaterSupplyId(int publicWaterSupplyId);
        IQueryable<Facility> GetByTownId(int? townId);
        IQueryable<Facility> GetByOperatingCenterId(int? opCenterId);

        IDictionary<DateTime, double> GetReadings(int facilityId, ReadingGroupType interval, DateTime startDate,
            DateTime endDate);

        IQueryable<Facility> GetByOperatingCenterIds(int[] operatingCenterIds);

        IQueryable<Facility> GetByOperatingCenterIdAndCommunityRightToKnowIsTrue(int? opCenterId);

        #endregion

        Facility FindWithEagerJoin(int id);
    }
}
