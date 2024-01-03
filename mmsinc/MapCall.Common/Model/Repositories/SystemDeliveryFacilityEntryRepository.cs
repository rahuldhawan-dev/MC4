using MapCall.Common.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using MMSINC.Data.NHibernate;
using MapCall.Common.Data;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using MMSINC.Authentication;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class SystemDeliveryFacilityEntryRepository : MapCallSecuredRepositoryBase<SystemDeliveryFacilityEntry>,
        ISystemDeliveryFacilityEntryRepository
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ProductionSystemDeliveryEntry;

        #endregion

        #region Properties

        public override RoleModules Role => ROLE;

        #endregion

        #region Constructor

        public SystemDeliveryFacilityEntryRepository(ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo) : base(session, container,
            authenticationService, roleRepo) { }

        #endregion
    }

    public static class SystemDeliveryFacilityEntryRepositoryExtensions
    {
        #region GetEntriesForFacility

        public static IQueryable<FacilitySystemDeliveryHistoryViewModel> GetEntriesForFacility(
            this IRepository<SystemDeliveryFacilityEntry> that, int facilityId, DateTime startDate, DateTime endDate)
        {
            var results = new List<FacilitySystemDeliveryHistoryViewModel>();
            return that.Where(x => x.Facility.Id == facilityId && 
                                   x.SystemDeliveryEntry.WeekOf >= startDate &&
                                   x.SystemDeliveryEntry.WeekOf <= endDate &&
                                   x.SystemDeliveryEntry.IsValidated.GetValueOrDefault())
                       .GroupBy(x => new {
                            x.SystemDeliveryEntryType.Description, 
                            x.SystemDeliveryEntry.WeekOf
                        })
                       .OrderBy(x => x.Key.WeekOf)
                       .ThenBy(x => x.Key.Description)
                       .Select(x => new FacilitySystemDeliveryHistoryViewModel {
                            EntryType = x.Key.Description,
                            Date = x.Key.WeekOf,
                            Value = x.Sum(y => y.EntryValue)
                        })
                       .AsQueryable();
        }

        #endregion
    }

    public interface ISystemDeliveryFacilityEntryRepository : IRepository<SystemDeliveryFacilityEntry> {}
}
