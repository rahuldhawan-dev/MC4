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
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class EquipmentRepository : MapCallSecuredRepositoryBase<Equipment>, IEquipmentRepository
    {
        #region Constants

        public const int T_AND_D_DEPARTMENT_ID = 1, PRODUCTION_DEPARTMENT_ID = 3;

        #endregion

        #region Constructors

        public EquipmentRepository(ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo) : base(session, container,
            authenticationService, roleRepo) { }

        #endregion

        #region Properties

        public override ICriteria Criteria
        {
            get
            {
                var crit = base.Criteria
                               .CreateAlias("Facility", "criteriaFacility", JoinType.LeftOuterJoin)
                               .CreateAlias("criteriaFacility.OperatingCenter", "criteriaOperatingCenter",
                                    JoinType.LeftOuterJoin);
                if (!CurrentUserCanAccessAllTheRecords)
                {
                    var opCenterIds = GetUserOperatingCenterIds();
                    crit = crit.Add(Restrictions.In("criteriaOperatingCenter.Id", opCenterIds));
                }

                return crit;
            }
        }

        public override IQueryable<Equipment> Linq
        {
            get
            {
                var linq = base.Linq;
                if (!CurrentUserCanAccessAllTheRecords)
                {
                    var opCenterIds = GetUserOperatingCenterIds();
                    linq = linq.Where(x => opCenterIds.Contains(x.Facility.OperatingCenter.Id));
                }

                return linq;
            }
        }

        public override RoleModules Role => RoleModules.ProductionEquipment;

        #endregion

        #region Exposed Methods

        public IQueryable<Equipment> GetByFacilityId(int facilityId)
        {
            return (from e in Linq where e.Facility.Id == facilityId select e);
        }

        public IQueryable<Equipment> GetByFacilityIds(int[] facilityIds)
        {
            return from f in Linq where facilityIds.Contains(f.Facility.Id) select f;
        }

        public IQueryable<Equipment> GetByTownIdForWorkOrders(int townId)
        {
            return (from e in Linq
                    where e.Facility != null
                          && e.Facility.Department != null
                          && e.Facility.Town != null
                          && e.Facility.Town.Id == townId
                          && (
                              e.Facility.Department.Id == T_AND_D_DEPARTMENT_ID ||
                              (e.Facility.Department.Id == PRODUCTION_DEPARTMENT_ID &&
                               e.Facility.OperatingCenter.HasWorkOrderInvoicing)
                          )
                    orderby e.Facility.FacilityName, e.EquipmentPurpose, e.Description
                    select e);
        }

        public IEnumerable<Equipment> SearchLinkedEquipment(ISearchEquipment search)
        {
            var criteria = Criteria.Add(Restrictions.Not(Restrictions.Eq("Id", search.NotEqualEntityId)));
            criteria.Add(
                Restrictions.Or(
                    Restrictions.Eq("IsReplacement", true),
                    Restrictions.Eq("Id", search.OriginalEquipmentId)));

            return Search(search, criteria);
        }

        /// <summary>
        /// This gets all <see cref="EquipmentCharacteristicDropDownValue"/> records from the specified <see cref="EquipmentCharacteristicField"/>
        /// that are currently being used by one or more <see cref="Equipment"/> record(s).
        /// </summary>
        /// <returns>
        /// A collection of Ids that correspond to all drop down records in the specified <see cref="EquipmentCharacteristicField"/> that are being used by one or more equipment.
        /// </returns>
        public IEnumerable<int> GetAllCharacteristicDropDownValuesCurrentlyInUse(EquipmentCharacteristicField model)
        {
            if (model.FieldType.DataType != EquipmentCharacteristicFieldType.DataTypes.DROPDOWN)
            {
                // Nothing to do here, model is not a DropDown
                return Enumerable.Empty<int>();
            }

            return GetAll()
                  .SelectMany(x => x.Characteristics)
                  .Where(x => x.Field.Id == model.Id)

                   // This could throw, but if Value is not an int Id for a EquipmentCharacteristicDropDownValue,
                   // then that would mean something went seriously wrong elsewhere, and I think it should throw. -Sean F.
                  .Select(x => int.Parse(x.Value))
                  .Distinct()
                  .ToList()
                  .OrderBy(x => x);
        }

        public bool IsCharacteristicDropDownValueCurrentlyInUse(EquipmentCharacteristicField model, int dropDownValueId)
            => GetAllCharacteristicDropDownValuesCurrentlyInUse(model).Contains(dropDownValueId);

        #endregion
    }

    // This is used in the scheduler too if you're like me and wondering why it exists. Though I still don't actually know why it exists. -Ross 4/27/2017
    public static class IEquipmentRepositoryExtensions
    {
        public static IEnumerable<Equipment> GetEquipmentWithSapRetryIssuesImpl(this IRepository<Equipment> that)
        {
            return
                that.Where(x => x.SAPErrorCode != null && x.SAPErrorCode != "" && x.SAPErrorCode.StartsWith("RETRY"));
        }
    }

    public interface IEquipmentRepository : IRepository<Equipment>
    {
        #region Abstract Methods

        IQueryable<Equipment> GetByFacilityId(int facilityId);
        IQueryable<Equipment> GetByFacilityIds(int[] facilityIds);
        IQueryable<Equipment> GetByTownIdForWorkOrders(int townId);
        IEnumerable<Equipment> SearchLinkedEquipment(ISearchEquipment search);
        IEnumerable<int> GetAllCharacteristicDropDownValuesCurrentlyInUse(EquipmentCharacteristicField model);
        bool IsCharacteristicDropDownValueCurrentlyInUse(EquipmentCharacteristicField model, int dropDownValueId);

        #endregion
    }
}
