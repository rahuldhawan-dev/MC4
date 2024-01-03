using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Data;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.BooleanExtensions;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class SystemDeliveryEntryRepository : MapCallSecuredRepositoryBase<SystemDeliveryEntry>, ISystemDeliveryEntryRepository
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ProductionSystemDeliveryEntry;

        #endregion

        #region Constructor

        public SystemDeliveryEntryRepository(ISession session, IContainer container, IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo) : base(session, container, authenticationService, roleRepo) { }

        #endregion

        #region Properties
        public override RoleModules Role => ROLE;

        public override ICriteria Criteria
        {
            get
            {
                var crit = base.Criteria
                               .CreateAlias("FacilityEntries", "criteriaFacilityEntries", JoinType.LeftOuterJoin)
                                // Need this since Entries will be empty on new/edit, user can still access the redirect to edit by using this alias for the OC filter
                               .CreateAlias("OperatingCenters", "critOc", JoinType.LeftOuterJoin); 

                if (!CurrentUserCanAccessAllTheRecords)
                {
                    var operatingCenterIds = GetUserOperatingCenterIds();
                    crit = crit.Add(Restrictions.In("critOc.Id", operatingCenterIds));
                }

                return crit;
            }
        }

        public override IQueryable<SystemDeliveryEntry> Linq
        {
            get
            {
                var linq = base.Linq;

                if (!CurrentUserCanAccessAllTheRecords)
                {
                    var operatingCenterIds = GetUserOperatingCenterIds();
                    linq = linq.Where(x => x.OperatingCenters.Any(y => operatingCenterIds.Contains(y.Id)));
                }

                return linq;
            }
        }

        #endregion

        #region SearchSystemDeliveryEntries

        public IEnumerable<SystemDeliveryEntrySearchResultViewModel> SearchSystemDeliveryEntries(SearchSet<SystemDeliveryEntry> search)
        {
            search.EnablePaging = false;
            var results = new List<SystemDeliveryEntrySearchResultViewModel>();
            var resultSearch = Search(search).DistinctBy(x => x.Id);

            foreach (var deliveryEntry in resultSearch)
            {
                if (deliveryEntry.FacilityEntries.Count > 0)
                {
                    AddResultWithEntries(deliveryEntry, results);
                }
                else
                {
                    AddResultWithoutEntries(deliveryEntry, results);
                }
            }

            return results.OrderBy(x => x.Id)
                          .ThenBy(x => x.EntryDate)
                          .ThenBy(x => x.Facility);
        }

        private void AddResultWithoutEntries(SystemDeliveryEntry deliveryEntry, List<SystemDeliveryEntrySearchResultViewModel> results) =>
            results.Add(new SystemDeliveryEntrySearchResultViewModel {
                Id = deliveryEntry.Id,
                SystemDeliveryType = deliveryEntry.SystemDeliveryType?.Description,
                EnteredBy = deliveryEntry.EnteredBy?.FullName,
                EntryDate = deliveryEntry.WeekOf
            });

        private void AddResultWithEntries(SystemDeliveryEntry deliveryEntry, List<SystemDeliveryEntrySearchResultViewModel> results)
        {
            results.AddRange(deliveryEntry.FacilityEntries.Select(entry => new SystemDeliveryEntrySearchResultViewModel {
                Id = deliveryEntry.Id,
                EntryDate = entry.EntryDate,
                OperatingCenter = entry.Facility.OperatingCenter.Description,
                SystemDeliveryType = entry.SystemDeliveryType.Description,
                Facility = entry.Facility.FacilityIdWithRegionalPlanningArea,
                PublicWaterSupply = entry.Facility.PublicWaterSupply?.Description,
                LegacyIdSd = entry.Facility.RegionalPlanningArea,

                // for a Transfer From, get BU from disabled matching entry type else get matching entry type
                BusinessUnit = entry.SystemDeliveryEntryType.Id == SystemDeliveryEntryType.Indices.TRANSFERRED_FROM ?
                    entry.Facility.FacilitySystemDeliveryEntryTypes
                         .FirstOrDefault(f => f.SystemDeliveryEntryType.Id == SystemDeliveryEntryType.Indices.TRANSFERRED_FROM 
                                              && !f.IsEnabled)?
                         .BusinessUnit : 
                    entry.Facility.FacilitySystemDeliveryEntryTypes
                         .FirstOrDefault(f => f.SystemDeliveryEntryType.Id == entry.SystemDeliveryEntryType.Id)?
                         .BusinessUnit,
                PurchaseSupplier =
                    entry.SystemDeliveryEntryType.Id == SystemDeliveryEntryType.Indices.PURCHASED_WATER &&
                    entry.Facility.FacilitySystemDeliveryEntryTypes.Any(x =>
                        x.SystemDeliveryEntryType.Id == SystemDeliveryEntryType.Indices.PURCHASED_WATER && x.IsEnabled)
                        ? entry.Facility.FacilitySystemDeliveryEntryTypes.Single(x =>
                                    x.SystemDeliveryEntryType.Id == SystemDeliveryEntryType.Indices.PURCHASED_WATER)
                               .PurchaseSupplier
                        : string.Empty,
                SystemDeliveryEntryType = entry.SystemDeliveryEntryType?.Description,
                Adjustment = entry.HasBeenAdjusted.ToString("yn"),
                OriginalEntry = entry.OriginalEntryValue ?? 0.00M,
                Value = entry.EntryValue,
                IsValidated = deliveryEntry.IsValidatedNotNull.ToString("yn"),
                IsInjection = entry.IsInjection.ToString("yn"),
                EnteredBy = entry.EnteredBy?.FullName,
                Comment = entry.AdjustmentComment,
                IsHyperionFileCreated = deliveryEntry.IsHyperionFileCreated.ToString("yn")
            }));
        }

        #endregion

        #region GetDataForSystemDeliveryEntryFileDump

        private string GetEntryDescription(int entryTypeId)
        {
            switch (entryTypeId)
            {
                case SystemDeliveryEntryType.Indices.DELIVERED_WATER:
                    return "SYS_NORMAL";
                case SystemDeliveryEntryType.Indices.PURCHASED_WATER:
                    return "SYS_PURCHASE";
                case SystemDeliveryEntryType.Indices.TRANSFERRED_FROM:
                    return "SYS_TRANSFER_FROM";
                case SystemDeliveryEntryType.Indices.TRANSFERRED_TO:
                    return "SYS_TRANSFER_TO";
                case SystemDeliveryEntryType.Indices.WASTEWATER_COLLECTED:
                    return "WW_WASTEWATER_COLLECTED";
                case SystemDeliveryEntryType.Indices.WASTEWATER_TREATED:
                    return "WW_TREATED";
                case SystemDeliveryEntryType.Indices.TREATED_EFF_DISCHARGED:
                    return "WW_TRT_EFF_DISCHARGE";
            }

            return String.Empty;
        }

        private string GetTotalValueAsString(decimal totalValue)
        {
            if (totalValue < 0)
            {
                return Math.Abs(totalValue).ToString() + "-"; // in the original CSV, negative values are appended at the end of the value not beginning.
            }

            return totalValue.ToString();
        }

        public IQueryable<int> GetEntryIds(DateTime startDate, IReadOnlyCollection<int> includedStates)
        {
            var entryIds = GetAll()
               .Where(x => x.UpdatedAt >= startDate &&
                           x.IsValidated == true &&
                           x.WeekOf >= startDate.AddWeeks(-2) &&
                           x.WeekOf <= startDate.GetEndOfMonth().Date);

            if (includedStates.Count > 0)
            {
                entryIds = entryIds.Where(x => x.OperatingCenters.Any(s => includedStates.Contains(s.State.Id)));
            }

            return entryIds.Select(x => x.Id);
        }

        public IQueryable<SystemDeliveryEntryFileDumpViewModel> GetDataForSystemDeliveryEntryFileDump(
            DateTime startDate, params int[] includedStates)
        {
            var entryIds = GetEntryIds(startDate, includedStates);
            var systemDeliveryFacilityEntryRepo = _container.GetInstance<IRepository<SystemDeliveryFacilityEntry>>();
            var fileDumpData = new List<SystemDeliveryEntryFileDumpViewModel>();
            var entries = systemDeliveryFacilityEntryRepo.Where(x => entryIds.Contains(x.SystemDeliveryEntry.Id));
            
            var facilityEntries = entries
                                 .Where(x => x.EntryDate >= startDate && x.EntryDate <= startDate.GetEndOfMonth())
                                 .GroupBy(x => new
                                      { x.Facility, SystemDeliveryEntryTypeId = x.SystemDeliveryEntryType.Id })
                                 .Select(group => new {
                                      group.Key.Facility,
                                      group.Key.SystemDeliveryEntryTypeId, 
                                      facilityEntries = group.ToList()
                                  }).ToList();

            foreach (var entry in facilityEntries)
            {
                var businessUnit = entry.Facility.FacilitySystemDeliveryEntryTypes.Any(x =>
                    x.SystemDeliveryEntryType.Id == entry.SystemDeliveryEntryTypeId)
                    ? entry.Facility.FacilitySystemDeliveryEntryTypes
                           .First(x => x.SystemDeliveryEntryType.Id == entry.SystemDeliveryEntryTypeId).BusinessUnit
                          ?.ToString()
                    : string.Empty;
                var totals = entry.facilityEntries.Where(x => x.EntryDate.Month == startDate.Month)
                                  .Sum(x => x.EntryValue);
                var asPostedDescription = "AS POSTED";
                var month = startDate.Month.ToString();
                var year = startDate.Year.ToString();
                var entryDescription = GetEntryDescription(entry.SystemDeliveryEntryTypeId);
                var facilityName = entry.Facility.FacilityName.ToUpper();
                var systemDeliveryDescription =
                    entry.Facility.SystemDeliveryType.Id == SystemDeliveryType.Indices.WASTE_WATER
                        ? "WASTE WATER"
                        : "SYSTEM DELIVERY";
                var totalValue = GetTotalValueAsString(totals);
                
                fileDumpData.Add(new SystemDeliveryEntryFileDumpViewModel {
                    Year = year,
                    Month = month,
                    BusinessUnit = businessUnit?.PadLeft(businessUnit.Length + 4, '0'),
                    FacilityName = facilityName,
                    EntryDescription = entryDescription,
                    AsPostedDescription = asPostedDescription,
                    SystemDeliveryDescription = systemDeliveryDescription,
                    TotalValue = totalValue
                });
            }

            return fileDumpData.AsQueryable().OrderBy(x => x.BusinessUnit);
        }

        #endregion
    }

    public interface ISystemDeliveryEntryRepository : IRepository<SystemDeliveryEntry>
    {
        IEnumerable<SystemDeliveryEntrySearchResultViewModel> SearchSystemDeliveryEntries(SearchSet<SystemDeliveryEntry> search);
        IQueryable<SystemDeliveryEntryFileDumpViewModel> GetDataForSystemDeliveryEntryFileDump(DateTime startDate, params int[] includedStates);
        IQueryable<int> GetEntryIds(DateTime startDate, IReadOnlyCollection<int> includedStates);
    }
}
