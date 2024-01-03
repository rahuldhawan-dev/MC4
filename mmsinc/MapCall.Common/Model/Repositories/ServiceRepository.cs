using MapCall.Common.Data;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MapCall.Common.Model.Repositories
{
    public class ServiceRepository : MapCallSecuredRepositoryBase<Service>, IServiceRepository
    {
        #region Properties

        public override ICriteria Criteria
        {
            get
            {
                var crit = base.Criteria
                               .CreateAlias("Coordinate", "c", JoinType.LeftOuterJoin);
                if (!CurrentUserCanAccessAllTheRecords)
                {
                    var opCenterIds = GetUserOperatingCenterIds();
                    crit = crit.Add(Restrictions.In("OperatingCenter.Id", opCenterIds));
                }

                //// Performance critical for searches/indexes, make sure associations being displayed are eager loaded.
                crit = crit.SetFetchMode("Street", FetchMode.Eager)
                           .SetFetchMode("CrossStreet", FetchMode.Eager);
                return crit;
            }
        }

        public override IQueryable<Service> Linq
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

        public override RoleModules Role
        {
            get { return RoleModules.FieldServicesAssets; }
        }

        #endregion

        #region Exposed Methods

        public override Service Save(Service entity)
        {
            entity = base.Save(entity);
            Session.Refresh(entity);
            // Refresh needs to be called to update any formula fields that might be stale.
            return entity;
        }

        #region Lookups

        public IEnumerable<Service> FindManyByServiceNumberAndPremiseNumber(string serviceNumber, string premiseNumber)
        {
            return base.Linq.Where(x =>
                x.ServiceNumber.ToString() == serviceNumber && x.PremiseNumber == premiseNumber);
        }

        public IEnumerable<Service> FindManyByPremiseNumber(string premiseNumber)
        {
            return base.Linq.Where(x => x.PremiseNumber == premiseNumber);
        }

        public Service FindByPremiseNumberAndServiceNumber(string serviceNumber, string premiseNumber)
        {
            // Apparently service number and premise number aren't unique and so they want "the most recent"
            // one. bug-2141. -Ross 10/9/2014
            // base because this is a report and we aren't conerned with roles
            return base.Linq.Where(x => x.ServiceNumber.ToString() == serviceNumber && x.PremiseNumber == premiseNumber)
                       .OrderByDescending(x => x.Id)
                       .FirstOrDefault();
        }

        public Service FindByPremiseNumber(string premiseNumber)
        {
            // Apparently service number and premise number aren't unique and so they want "the most recent"
            // one. bug-2141. -Ross 10/9/2014
            // base because this is a report and we aren't conerned with roles
            return base.Linq.Where(x => x.PremiseNumber == premiseNumber)
                       .OrderByDescending(x => x.Id)
                       .FirstOrDefault();
        }

        public Service FindByOperatingCenterAndPremiseNumber(int operatingCenter, string premiseNumber, int id)
        {
            return
                base.Linq
                    .FirstOrDefault(
                         x => x.OperatingCenter.Id == operatingCenter && x.PremiseNumber == premiseNumber &&
                              x.Id != id);
        }

        public IEnumerable<Service> FindByStreetId(int streetId)
        {
            // base because this is a report and we aren't conerned with roles
            return base.Linq.Where(x => x.Street != null && x.Street.Id == streetId);
        }

        public long? GetNextServiceNumber(string operatingCenterCode)
        {
            //using default now; removed cases for "NJ3", "NJ4", "NJ5" - for MC 1762
            return (base.Linq.Count(x =>
                x.OperatingCenter.OperatingCenterCode == operatingCenterCode && x.ServiceNumber != null) == 0)
                ? 1
                : base.Linq.Where(x => x.OperatingCenter.OperatingCenterCode == operatingCenterCode)
                      .Where(x => x.ServiceNumber != null)
                      .Max(x => x.ServiceNumber) + 1;
        }

        private IQueryable<Service> ByInstallationNumberAndOperatingCenterAndSampleSites(
            string installation,
            int operatingCenterId)
        {
            return Linq.Where(x =>
                x.Installation != null && x.Installation != "" && x.Installation == installation &&
                x.OperatingCenter.Id == operatingCenterId && x.Premise != null && x.Premise.SampleSites.Any());
        }

        public bool AnyWithInstallationNumberAndOperatingCenterAndSampleSites(string installation,
            int operatingCenterId)
        {
            return ByInstallationNumberAndOperatingCenterAndSampleSites(installation, operatingCenterId).Any();
        }

        public IEnumerable<Service> FindByInstallationNumberAndOperatingCenterAndSampleSites(string installation,
            int operatingCenterId)
        {
            return ByInstallationNumberAndOperatingCenterAndSampleSites(installation, operatingCenterId);
        }

        public IEnumerable<Service> GetServicesWithoutPremiseLinked()
        {
            // base because this is to link premise and we aren't concerned with roles
            return from s in base.Linq
                   from p in Session.Query<Premise>()
                   where s.Installation == p.Installation &&
                         s.PremiseNumber == p.PremiseNumber &&
                         s.ServiceCategory != null &&
                         p.ServiceUtilityType != null &&
                         p.ServiceUtilityType.Id == s.ServiceCategory.ServiceUtilityType.Id &&
                         s.Premise == null
                   select s;
        }

        #endregion

        #region Reports

        public IEnumerable<int> GetDistinctYears()
        {
            var results = (from x in base.Linq where x.DateInstalled.HasValue select x.Year).Distinct();
            return results;
        }

        public IEnumerable<int> GetDistinctYearsRetired()
        {
            return (from x in base.Linq where x.RetiredDate.HasValue select x.YearRetired).Distinct();
        }

        #region GetServicesRenewed

        public IEnumerable<AggregatedService> GetServicesRenewed(ISearchSet<Service> search)
        {
            search.EnablePaging = false;
            // base because this is a report and we aren't conerned with roles
            var criteria = GenerateCriteriaForSearchSet(search, base.Criteria);
            //var criteria = Criteria.Add(Restrictions.Conjunction());
            var services = criteria.List<Service>()
                                   .GroupBy(x => new {x.OperatingCenter, x.Town, x.ServiceCategory, x.ServiceSize})
                                   .AsEnumerable()
                                   .Select(x => new AggregatedService {
                                        OperatingCenter = x.Key.OperatingCenter,
                                        Town = x.Key.Town,
                                        ServiceCategory = x.Key.ServiceCategory,
                                        ServiceSize = x.Key.ServiceSize,
                                        TotalFootage = (decimal)x.Sum(s => s.LengthOfService),
                                        ServiceCount = x.Count(),
                                        Services = x
                                    })
                                   .OrderBy(x => x.OperatingCenter.OperatingCenterCode)
                                   .ThenBy(x => x.Town.ShortName)
                                   .ThenBy(x => x.ServiceCategory.Description)
                                   .ThenBy(x => (x.ServiceSize == null) ? 0.0m : x.ServiceSize.Size);
            return services.ToList();
        }

        #endregion

        #region Monthly Services Installed By Category

        public IEnumerable<MonthlyServicesInstalledByCategoryViewModel> GetMonthlyServicesInstalledByCategory(
            ISearchMonthlyServicesInstalledByCategory search)
        {
            MonthlyServicesInstalledByCategoryViewModel result = null;
            OperatingCenter operatingCenter = null;
            ServiceCategory serviceCategory = null;

            var query = Session.QueryOver<Service>();
            query.JoinAlias(x => x.OperatingCenter, () => operatingCenter);
            query.SelectList(x => x
                                 .SelectGroup(y => y.OperatingCenter).WithAlias(() => result.OperatingCenter)
                                 .SelectGroup(y => y.ServiceCategory).WithAlias(() => result.ServiceCategory)
                                 .SelectGroup(y => y.Month).WithAlias(() => result.Month)
                                 .SelectGroup(y => y.Year).WithAlias(() => result.Year)
                                 .SelectCount(y => y.Id).WithAlias(() => result.Total)
            );
            query.TransformUsing(Transformers.AliasToBean<MonthlyServicesInstalledByCategoryViewModel>());

            var results = Search(search, query);
            return results;
        }

        public IEnumerable<MonthlyServicesInstalledByCategoryReportViewModel>
            GetMonthlyServicesInstalledByCategoryReport(ISearchMonthlyServicesInstalledByCategory search)
        {
            var results = GetMonthlyServicesInstalledByCategory(search);
            var operatingCenters =
                results.Select(x => x.OperatingCenter).Distinct().OrderBy(x => x.OperatingCenterCode);
            var categories = results.Select(x => x.ServiceCategory).Distinct().OrderBy(x => x.Description);
            var totalRecords = (operatingCenters.Count() * categories.Count()) + operatingCenters.Count();

            var report = new MonthlyServicesInstalledByCategoryReportViewModel[totalRecords];

            var counter = 0;
            foreach (var operatingCenter in operatingCenters)
            {
                if (counter >= totalRecords) break;
                var rowResults = results.Where(x => x.OperatingCenter == operatingCenter);
                if (rowResults.Any())
                {
                    Func<int, int, int?> getVal = (month, val) => {
                        var first = rowResults.FirstOrDefault(x => x.Month == month && x.ServiceCategory.Id == val);
                        return (first != null) ? first.Total : (int?)0;
                    };
                    int janTotal = 0,
                        febTotal = 0,
                        marTotal = 0,
                        aprTotal = 0,
                        mayTotal = 0,
                        junTotal = 0,
                        julTotal = 0,
                        augTotal = 0,
                        sepTotal = 0,
                        octTotal = 0,
                        novTotal = 0,
                        decTotal = 0;
                    foreach (var serviceCategory in categories)
                    {
                        report[counter] = new MonthlyServicesInstalledByCategoryReportViewModel {
                            Year = search.Year,
                            OperatingCenter = operatingCenter,
                            ServiceCategory = serviceCategory,
                            Jan = getVal(1, serviceCategory.Id),
                            Feb = getVal(2, serviceCategory.Id),
                            Mar = getVal(3, serviceCategory.Id),
                            Apr = getVal(4, serviceCategory.Id),
                            May = getVal(5, serviceCategory.Id),
                            Jun = getVal(6, serviceCategory.Id),
                            Jul = getVal(7, serviceCategory.Id),
                            Aug = getVal(8, serviceCategory.Id),
                            Sep = getVal(9, serviceCategory.Id),
                            Oct = getVal(10, serviceCategory.Id),
                            Nov = getVal(11, serviceCategory.Id),
                            Dec = getVal(12, serviceCategory.Id),
                        };
                        janTotal += report[counter].Jan.Value;
                        febTotal += report[counter].Feb.Value;
                        marTotal += report[counter].Mar.Value;
                        aprTotal += report[counter].Apr.Value;
                        mayTotal += report[counter].May.Value;
                        junTotal += report[counter].Jun.Value;
                        julTotal += report[counter].Jul.Value;
                        augTotal += report[counter].Aug.Value;
                        sepTotal += report[counter].Sep.Value;
                        octTotal += report[counter].Oct.Value;
                        novTotal += report[counter].Nov.Value;
                        decTotal += report[counter].Dec.Value;
                        counter++;
                    }

                    report[counter] = new MonthlyServicesInstalledByCategoryReportViewModel {
                        OperatingCenter = operatingCenter,
                        ServiceCategory = new ServiceCategory {Description = "Total"},
                        Jan = janTotal,
                        Feb = febTotal,
                        Mar = marTotal,
                        Apr = aprTotal,
                        May = mayTotal,
                        Jun = junTotal,
                        Jul = julTotal,
                        Aug = augTotal,
                        Sep = sepTotal,
                        Oct = octTotal,
                        Nov = novTotal,
                        Dec = decTotal
                    };
                    counter++;
                }
            }

            return report.Where(x => x != null);
        }

        #endregion

        #region BPU Report for Services

        public IEnumerable<BPUReportForServiceReportItem> GetBPUReportForServices(ISearchBPUReportForServices search)
        {
            BPUReportForServiceReportItem result = null;
            Service service = null;
            OperatingCenter operatingCenter = null;
            ServiceSize serviceSize = null;
            ServiceMaterial serviceMaterial = null;
            ServiceType serviceType = null;
            CategoryOfServiceGroup categoryOfServiceGroup = null;

            var query = Session.QueryOver(() => service);
            query.JoinAlias(x => x.OperatingCenter, () => operatingCenter);
            query.JoinAlias(x => x.ServiceSize, () => serviceSize);
            query.JoinAlias(x => x.ServiceMaterial, () => serviceMaterial);
            query.JoinAlias(x => x.ServiceType, () => serviceType);
            query.JoinAlias(x => serviceType.CategoryOfServiceGroup, () => categoryOfServiceGroup);
            query.Where(x => x.ServiceType != null);
            query.Where(x => serviceType.CategoryOfServiceGroup != null);

            var installedNew = QueryOver.Of<Service>()
                                        .JoinAlias(x => x.ServiceType, () => serviceType)
                                        .JoinAlias(x => serviceType.CategoryOfServiceGroup,
                                             () => categoryOfServiceGroup)
                                        .Where(s => categoryOfServiceGroup.Id == CategoryOfServiceGroup.Indices.NEW)
                                        .ToRowCountQuery();
            var installedRenewal = QueryOver.Of<Service>()
                                            .JoinAlias(x => x.ServiceType, () => serviceType)
                                            .JoinAlias(x => serviceType.CategoryOfServiceGroup,
                                                 () => categoryOfServiceGroup)
                                            .Where(s => categoryOfServiceGroup.Id ==
                                                        CategoryOfServiceGroup.Indices.RENEWAL).ToRowCountQuery();

            query.SelectList(x => x
                                 .SelectGroup(_ => service.OperatingCenter).WithAlias(() => result.OperatingCenter)
                                 .SelectGroup(_ => service.ServiceSize).WithAlias(() => result.InstalledSize)
                                 .SelectGroup(_ => service.ServiceMaterial).WithAlias(() => result.InstalledType)
                                 .SelectGroup(_ => service.Year).WithAlias(() => result.Year)
                                 .SelectSubQuery(installedNew.Where(y =>
                                      y.OperatingCenter == service.OperatingCenter
                                      && y.ServiceSize == service.ServiceSize
                                      && y.ServiceMaterial == service.ServiceMaterial
                                      && y.Year == service.Year)).WithAlias(() => result.InstalledNew)
                                 .SelectSubQuery(installedRenewal.Where(y =>
                                      y.OperatingCenter == service.OperatingCenter
                                      && y.ServiceSize == service.ServiceSize
                                      && y.ServiceMaterial == service.ServiceMaterial
                                      && y.Year == service.Year)).WithAlias(() => result.Replaced)
            );
            query.OrderBy(x => x.OperatingCenter).Asc();
            query.OrderBy(x => x.ServiceMaterial).Asc();
            query.OrderBy(x => x.ServiceSize).Asc();

            query.TransformUsing(Transformers.AliasToBean<BPUReportForServiceReportItem>());

            var results = Search(search, query).ToList();
            search.Count = results.Count();
            return results;
        }

        #endregion

        #region Services Retired

        public IEnumerable<ServicesRetiredReportItem> GetServicesRetired(ISearchServicesRetired search)
        {
            ServicesRetiredReportItem result = null;
            Service service = null;
            ServiceSize previousServiceSize = null;

            var query = Session.QueryOver(() => service);
            query.JoinAlias(x => x.PreviousServiceSize, () => previousServiceSize);
            query.SelectList(x => x
                                 .SelectGroup(_ => service.OperatingCenter).WithAlias(() => result.OperatingCenter)
                                 .SelectGroup(_ => service.Town).WithAlias(() => result.Town)
                                 .SelectGroup(_ => service.ServiceCategory).WithAlias(() => result.ServiceCategory)
                                 .SelectGroup(_ => service.PreviousServiceMaterial)
                                 .WithAlias(() => result.PreviousServiceMaterial)
                                 .SelectGroup(_ => previousServiceSize.Size).WithAlias(() => result.PreviousServiceSize)
                                 .SelectGroup(_ => previousServiceSize.ServiceSizeDescription)
                                 .WithAlias(() => result.PreviousServiceSizeDescription)
                                 .SelectGroup(_ => service.YearRetired).WithAlias(() => result.YearRetired)
                                 .SelectCount(_ => service.Id).WithAlias(() => result.Total)
            );
            query.OrderBy(x => x.OperatingCenter).Asc();
            query.OrderBy(x => x.Town).Asc();
            query.OrderBy(x => x.YearRetired).Asc();
            query.OrderBy(x => x.ServiceCategory).Asc();
            query.OrderBy(x => x.PreviousServiceMaterial).Asc();
            query.OrderBy(x => previousServiceSize.Size).Asc();

            query.TransformUsing(Transformers.AliasToBean<ServicesRetiredReportItem>());

            var results = Search(search, query).ToList();
            search.Count = results.Count;
            return results;
        }

        #endregion

        #region Services Renewed

        public IEnumerable<ServicesRenewedSummaryReportItem> GetServicesRenewedSummary(
            ISearchServicesRenewedSummary search)
        {
            ServicesRenewedSummaryReportItem result = null;
            Service service = null;
            ServiceCategory serviceCategory = null;
            Town town = null;

            var query = Session.QueryOver(() => service);

            query.JoinAlias(x => x.ServiceCategory, () => serviceCategory);
            query.JoinAlias(x => x.Town, () => town);

            query.SelectList(x => x
                                 .SelectGroup(_ => service.OperatingCenter).WithAlias(() => result.OperatingCenter)
                                 .SelectGroup(_ => town.ShortName).WithAlias(() => result.Town)
                                 .SelectGroup(_ => service.Year).WithAlias(() => result.Year)
                                 .SelectCount(_ => service.Id).WithAlias(() => result.Total)
            );

            query.AndRestrictionOn(x => serviceCategory.Id).IsIn(ServiceCategory.GetRenewalServiceCategories());

            query.TransformUsing(Transformers.AliasToBean<ServicesRenewedSummaryReportItem>());

            query.OrderBy(x => x.OperatingCenter).Asc();
            query.OrderBy(x => town.ShortName).Asc();
            query.OrderBy(x => x.Year).Asc();

            var results = Search(search, query).ToList();
            search.Count = results.Count;
            return results;
        }

        #endregion

        #region ServicesCompletedByCategory

        public IEnumerable<ServicesCompletedByCategoryReportItem> GetServicesCompletedByCategory(
            ISearchServicesCompletedByCategory search)
        {
            ServicesCompletedByCategoryReportItem result = null;
            Service service = null;

            var query = Session.QueryOver(() => service);
            query.SelectList(x => x
                                 .SelectGroup(_ => service.OperatingCenter).WithAlias(() => result.OperatingCenter)
                                 .SelectGroup(_ => service.ServiceCategory).WithAlias(() => result.ServiceCategory)
                                 .SelectCount(_ => service.Id).WithAlias(() => result.Total)
            );

            query.TransformUsing(Transformers.AliasToBean<ServicesCompletedByCategoryReportItem>());

            query.OrderBy(x => x.ServiceCategory).Asc();

            var results = Search(search, query).ToList();
            search.Count = results.Count;
            return results;
        }

        #endregion

        #region T/D Pending Services KPI

        /// <summary>
        /// This report queries the db for a number of different counts
        /// then sticks them all together into a report item class to be sorted out
        /// by the view.
        /// </summary>
        public IEnumerable<TDPendingServicesKPIReportItem> GetTDPendingServicesKPI(ISearchTDPendingServicesKPI search)
        {
            TDPendingServicesKPIReportItem result = null;
            Service service = null;
            ServiceInstallationPurpose serviceInstallationPurpose = null;
            ServiceRestorationContractor contractor = null;

            #region Renewed Permits Pending

            var rewewalsPermitsPending = Linq.Count(x => x.OperatingCenter.Id == search.OperatingCenter
                                                         && x.ServiceCategory.Id == ServiceCategory.Indices
                                                            .WATER_SERVICE_RENEWAL
                                                         && x.PermitSentDate.HasValue
                                                         && !x.PermitReceivedDate.HasValue
                                                         && !x.DateInstalled.HasValue
                                                         && (x.ServiceInstallationPurpose == null ||
                                                             x.ServiceInstallationPurpose.Description !=
                                                             "Main Replacement")
                                                         && x.IsActive);

            var rewewalsPermitsPendingResult = new TDPendingServicesKPIReportItem {
                Section = TDPendingServicesKPIReportItem.SECTION_SERVICE,
                ServicesContractor = TDPendingServicesKPIReportItem.Category.WATER_SERVICE_RENEWAL_PENDING_PERMITS,
                Total = rewewalsPermitsPending
            };

            #endregion

            #region Renewals Issued To Field

            var renewalsIssuedToField = Linq.Count(x =>
                x.OperatingCenter.Id == search.OperatingCenter
                && x.ServiceCategory.Id == ServiceCategory.Indices.WATER_SERVICE_RENEWAL
                && x.DateIssuedToField != null
                && x.DateInstalled == null
                && (x.ServiceInstallationPurpose == null ||
                    x.ServiceInstallationPurpose.Description != "Main Replacement")
                && x.IsActive);

            var renewalsIssuedToFieldResult = new TDPendingServicesKPIReportItem {
                Section = TDPendingServicesKPIReportItem.SECTION_SERVICE,
                ServicesContractor = TDPendingServicesKPIReportItem.Category.WATER_SERVICE_ISSUED_TO_FIELD,
                Total = renewalsIssuedToField
            };

            #endregion

            #region Approved Applications

            var servicesApprovedApplication = Linq.Count(x => x.OperatingCenter.Id == search.OperatingCenter
                                                              && ServiceCategory.GetWaterNewServiceCategories()
                                                                 .Contains(x.ServiceCategory.Id)
                                                              && (x.ServiceInstallationPurpose == null ||
                                                                  x.ServiceInstallationPurpose.Description !=
                                                                  "Main Replacement")
                                                              && x.DeveloperServicesDriven != true
                                                              && x.IsActive
                                                              && x.DateInstalled == null
                                                              && x.ApplicationApprovedOn != null
                                                              && x.PermitSentDate == null
                                                              && x.PermitReceivedDate == null
                                                              && x.DateIssuedToField == null
                                                              && (x.ServiceStatus == null || x.ServiceStatus.Id !=
                                                                  ServiceStatus.Indices.SITE_NOT_READY)
            );
            var servicesApprovedApplicationResult = new TDPendingServicesKPIReportItem {
                Section = TDPendingServicesKPIReportItem.SECTION_SERVICE,
                ServicesContractor = TDPendingServicesKPIReportItem.Category.NEW_WATER_SERVICES_APPROVED_APPLICATION,
                Total = servicesApprovedApplication
            };

            #endregion

            #region Permits Pending

            var servicesPermitsPending = Linq.Count(x => x.OperatingCenter.Id == search.OperatingCenter
                                                         && ServiceCategory.GetWaterNewServiceCategories()
                                                                           .Contains(x.ServiceCategory.Id)
                                                         && (x.ServiceInstallationPurpose == null ||
                                                             x.ServiceInstallationPurpose.Description !=
                                                             "Main Replacement")
                                                         && x.DeveloperServicesDriven != true
                                                         && x.IsActive
                                                         && x.DateInstalled == null
                                                         && x.PermitSentDate != null
                                                         && x.PermitReceivedDate == null
                                                         && x.DateIssuedToField == null
                                                         && (x.ServiceStatus == null || x.ServiceStatus.Id !=
                                                             ServiceStatus.Indices.SITE_NOT_READY)
            );
            var servicesPermitsPendingResult = new TDPendingServicesKPIReportItem {
                Section = TDPendingServicesKPIReportItem.SECTION_SERVICE,
                ServicesContractor = TDPendingServicesKPIReportItem.Category.NEW_WATER_SERVICES_PERMITS_PENDING,
                Total = servicesPermitsPending
            };

            #endregion

            #region Issued To Field

            var servicesIssuedToField = Linq.Count(x => x.OperatingCenter.Id == search.OperatingCenter
                                                        && ServiceCategory.GetWaterNewServiceCategories()
                                                                          .Contains(x.ServiceCategory.Id)
                                                        && (x.ServiceInstallationPurpose == null ||
                                                            x.ServiceInstallationPurpose.Description !=
                                                            "Main Replacement")
                                                        && x.DeveloperServicesDriven != true
                                                        && x.IsActive
                                                        && x.DateInstalled == null
                                                        && x.DateIssuedToField != null
                                                        && (x.ServiceStatus == null || x.ServiceStatus.Id !=
                                                            ServiceStatus.Indices.SITE_NOT_READY)
            );
            var servicesIssuedToFieldResult = new TDPendingServicesKPIReportItem {
                Section = TDPendingServicesKPIReportItem.SECTION_SERVICE,
                ServicesContractor = TDPendingServicesKPIReportItem.Category.NEW_WATER_SERVICES_ISSUED_TO_FIELD,
                Total = servicesIssuedToField
            };

            #endregion

            #region Site Not Ready

            var servicesSiteNotReady = Linq.Count(x => x.OperatingCenter.Id == search.OperatingCenter
                                                       && ServiceCategory.GetWaterNewServiceCategories()
                                                                         .Contains(x.ServiceCategory.Id)
                                                       && (x.ServiceInstallationPurpose == null ||
                                                           x.ServiceInstallationPurpose.Description !=
                                                           "Main Replacement")
                                                       && x.DeveloperServicesDriven != true
                                                       && x.IsActive
                                                       && x.DateInstalled == null
                                                       && (x.ServiceStatus != null && x.ServiceStatus.Id ==
                                                           ServiceStatus.Indices.SITE_NOT_READY)
            );

            var servicesSiteNotReadyResult = new TDPendingServicesKPIReportItem {
                Section = TDPendingServicesKPIReportItem.SECTION_SERVICE,
                ServicesContractor = TDPendingServicesKPIReportItem.Category.NEW_WATER_SERVICES_SITE_NOT_READY,
                Total = servicesSiteNotReady
            };

            #endregion

            #region Sewer Services

            var sewers = Linq.Count(x => x.OperatingCenter.Id == search.OperatingCenter
                                         && ServiceCategory.GetSewerCategories().Contains(x.ServiceCategory.Id)
                                         && x.DateIssuedToField != null
                                         && x.DateInstalled == null
                                         && x.DeveloperServicesDriven != true
                                         && x.IsActive
            );
            var sewersResult = new TDPendingServicesKPIReportItem {
                Section = TDPendingServicesKPIReportItem.SECTION_SERVICE,
                ServicesContractor = TDPendingServicesKPIReportItem.Category.SEWER_SERVICES,
                Total = sewers
            };

            #endregion

            #region Contractors

            // Contractor data is
            var query = Session.QueryOver(() => service);
            query.JoinAlias(_ => service.ServiceInstallationPurpose, () => serviceInstallationPurpose);
            query.JoinAlias(_ => service.WorkIssuedTo, () => contractor);
            query
               .SelectList(x => x
                               .SelectGroup(_ => service.OperatingCenter).WithAlias(() => result.OperatingCenter)
                               .Select(
                                    Projections.Constant(TDPendingServicesKPIReportItem.SECTION_CONTRACTOR)
                                               .WithAlias(() => result.Section))
                               .SelectGroup(_ => contractor.Contractor).WithAlias(() => result.ServicesContractor)
                               .SelectCount(_ => service.Id).WithAlias(() => result.Total)
                );
            query.Where(x =>
                x.DateIssuedToField != null
                && x.DateInstalled == null
                && serviceInstallationPurpose.Id != ServiceInstallationPurpose.Indices.MAIN_REPLACEMENT
                && x.DeveloperServicesDriven != true
                && x.WorkIssuedTo != null);
            query.AndRestrictionOn(x => x.ServiceCategory.Id).IsIn(ServiceCategory.GetNewServiceCategories());

            query.TransformUsing(Transformers.AliasToBean<TDPendingServicesKPIReportItem>());
            query.OrderBy(_ => service.WorkIssuedTo.Contractor);

            var results = Search(search, query).ToList();

            #endregion

            // join the contractors services with the 7 other records we manually selected
            search.Count = results.Count + 7;
            return results.Union(new[] {
                rewewalsPermitsPendingResult,
                renewalsIssuedToFieldResult,
                servicesApprovedApplicationResult,
                servicesPermitsPendingResult,
                servicesIssuedToFieldResult,
                servicesSiteNotReadyResult,
                sewersResult
            });
        }

        public IEnumerable<OpenIssuedServicesReportItem> GetOpenIssuedServices(
            ISearchSet<OpenIssuedServicesReportItem> model)
        {
            Service service = null;
            OpenIssuedServicesReportItem result = null;
            OperatingCenter opc = null;
            ServiceRestorationContractor contractor = null;
            ServiceCategory category = null;
            ServiceInstallationPurpose purpose = null;

            var query = Session.QueryOver(() => service)
                               .JoinAlias(_ => service.OperatingCenter, () => opc)
                               .JoinAlias(_ => service.WorkIssuedTo, () => contractor)
                               .JoinAlias(_ => service.ServiceCategory, () => category)
                               .JoinAlias(_ => service.ServiceInstallationPurpose, () => purpose);

            query.SelectList(x => x
                                 .Select(s => s.Id).WithAlias(() => result.Id)
                                 .Select(s => s.ServicePriority).WithAlias(() => result.ServicePriority)
                                 .Select(s => s.WorkIssuedTo).WithAlias(() => result.WorkIssuedTo)
                                 .Select(s => s.DateIssuedToField).WithAlias(() => result.DateIssuedToField)
                                 .Select(s => s.ServiceNumber).WithAlias(() => result.ServiceNumber)
                                 .Select(s => s.StreetAddress).WithAlias(() => result.CompleteStAddress)
                                 .Select(s => s.Town).WithAlias(() => result.Town)
                                 .Select(s => s.ServiceCategory).WithAlias(() => result.ServiceCategory)
                                 .Select(s => s.ServiceInstallationPurpose)
                                 .WithAlias(() => result.ServiceInstallationPurpose)
                                 .Select(s => s.CrossStreet).WithAlias(() => result.CrossStreet)
                                 .Select(s => s.PermitNumber).WithAlias(() => result.PermitNumber)
                                 .Select(s => s.TapOrderNotes).WithAlias(() => result.TapOrderNotes)
                                 .Select(s => s.PurchaseOrderNumber).WithAlias(() => result.PurchaseOrderNumber));

            query = query
                   .Where(
                        Restrictions.Not(Restrictions.On<Service>(_ => category.Description).IsLike("%Stub Service%")))
                   .Where(Restrictions.Not(Restrictions.On<Service>(_ => category.Description)
                                                       .IsLike("%Measurement Only%")))
                   .Where(_ => purpose.Description != "Main Replacement")
                   .Where(s => s.DateIssuedToField != null)
                   .Where(s => s.DateInstalled == null)
                   .Where(s => s.IsActive);

            query.TransformUsing(Transformers.AliasToBean<OpenIssuedServicesReportItem>());
            return Search(model, query);
        }

        #endregion

        #region ServicesQualityAssuranceReport

        public IEnumerable<ServiceQualityAssuranceReportItem> GetServicesQualityAssuranceReport(
            ISearchServiceQAReport search)
        {
            // NOTE: This report is annoying because it requires three completely separate queries. 
            // bug-3399 if you need to know where this comes from.

            search.EnablePaging = false;

            ServiceQualityAssuranceReportItem result = null;
            Service service = null;
            ServiceCategory serviceCategory = null;

            #region Service Renewals

            var renewQuery = Session.QueryOver(() => service);
            renewQuery.JoinAlias(x => x.ServiceCategory, () => serviceCategory);

            renewQuery.SelectList(x => x
                                      .SelectGroup(_ => service.OperatingCenter).WithAlias(() => result.OperatingCenter)
                                      .SelectGroup(_ => service.ServiceCategory).WithAlias(() => result.Category)
                                      .SelectCount(_ => service.Id).WithAlias(() => result.Total)
                                      .Select(Projections.Constant(true)).WithAlias(() => result.IsRenewal)
                                      .Select(Projections.Sum(Projections.Conditional(
                                           Restrictions.Eq(
                                               Projections.Property(() => service.PreviousServiceMaterial),
                                               ServiceMaterial.Indices.UNKNOWN),
                                           Projections.Constant(1),
                                           Projections.Constant(0)
                                       ))).WithAlias(() => result.PreviousServiceMaterialUnknown)
                                      .Select(Projections.Sum(Projections.Conditional(
                                           Restrictions.Eq(
                                               Projections.Property(() => service.CustomerSideMaterial),
                                               ServiceMaterial.Indices.UNKNOWN),
                                           Projections.Constant(1),
                                           Projections.Constant(0)
                                       ))).WithAlias(() => result.CustomerSideMaterialUnknown)
                                      .Select(Projections.Sum(Projections.Conditional(
                                           Restrictions.Eq(
                                               Projections.Property(() => service.ServiceMaterial),
                                               ServiceMaterial.Indices.UNKNOWN),
                                           Projections.Constant(1),
                                           Projections.Constant(0)
                                       ))).WithAlias(() => result.ServiceMaterialUnknown)
            );

            var renewalCats = new[] {
                ServiceCategory.Indices.FIRE_SERVICE_RENEWAL,
                ServiceCategory.Indices.IRRIGATION_RENEWAL,
                ServiceCategory.Indices.WATER_SERVICE_INCREASE_SIZE,
                ServiceCategory.Indices.WATER_SERVICE_RENEWAL,
                ServiceCategory.Indices.WATER_SERVICE_RENEWAL_CUST_SIDE,
                ServiceCategory.Indices.WATER_SERVICE_SPLIT,
                ServiceCategory.Indices.SEWER_RECONNECT,
                ServiceCategory.Indices.SEWER_SERVICE_INCREASE_SIZE,
                ServiceCategory.Indices.SEWER_SERVICE_RENEWAL,
                ServiceCategory.Indices.SEWER_SERVICE_SPLIT,
            };
            renewQuery.AndRestrictionOn(x => serviceCategory.Id).IsIn(renewalCats);

            renewQuery.TransformUsing(Transformers.AliasToBean<ServiceQualityAssuranceReportItem>());

            #endregion

            #region New Services

            var newQuery = Session.QueryOver(() => service);
            newQuery.JoinAlias(x => x.ServiceCategory, () => serviceCategory);

            newQuery.SelectList(x => x
                                    .SelectGroup(_ => service.OperatingCenter).WithAlias(() => result.OperatingCenter)
                                    .SelectGroup(_ => service.ServiceCategory).WithAlias(() => result.Category)
                                    .SelectCount(_ => service.Id).WithAlias(() => result.Total)
                                    .Select(Projections.Sum(Projections.Conditional(
                                         Restrictions.Eq(
                                             Projections.Property(() => service.CustomerSideMaterial),
                                             ServiceMaterial.Indices.UNKNOWN),
                                         Projections.Constant(1),
                                         Projections.Constant(0)
                                     ))).WithAlias(() => result.CustomerSideMaterialUnknown)
                                    .Select(Projections.Sum(Projections.Conditional(
                                         Restrictions.Eq(
                                             Projections.Property(() => service.ServiceMaterial),
                                             ServiceMaterial.Indices.UNKNOWN),
                                         Projections.Constant(1),
                                         Projections.Constant(0)
                                     ))).WithAlias(() => result.ServiceMaterialUnknown)
            );

            var newCats = new[] {
                ServiceCategory.Indices.FIRE_SERVICE_INSTALLATION,
                ServiceCategory.Indices.IRRIGATION_NEW,
                ServiceCategory.Indices.SEWER_SERVICE_NEW,
                ServiceCategory.Indices.WATER_SERVICE_NEW_COMMERCIAL,
                ServiceCategory.Indices.WATER_SERVICE_NEW_DOMESTIC,
            };
            newQuery.AndRestrictionOn(x => serviceCategory.Id).IsIn(newCats);

            newQuery.TransformUsing(Transformers.AliasToBean<ServiceQualityAssuranceReportItem>());

            #endregion

            #region Tap image count

            var tapQuery = Session.QueryOver(() => service);
            tapQuery.JoinAlias(x => x.ServiceCategory, () => serviceCategory);

            tapQuery.SelectList(x => x
                                    .SelectGroup(_ => service.OperatingCenter).WithAlias(() => result.OperatingCenter)
                                    .SelectCount(_ => service.Id).WithAlias(() => result.MissingTapImages)
                                    .Select(Projections.Constant(true)).WithAlias(() => result.IsTapImageReport)
            );

            tapQuery.AndRestrictionOn(x => serviceCategory.Id).IsIn(renewalCats.Concat(newCats).ToArray());
            tapQuery.WhereNot(x => x.HasTapImages);

            tapQuery.TransformUsing(Transformers.AliasToBean<ServiceQualityAssuranceReportItem>());

            #endregion

            var renewalResults = Search(search, renewQuery).ToList();
            var newResults = Search(search, newQuery).ToList();
            var tapResults = Search(search, tapQuery).ToList();

            // This is painful.
            foreach (var tr in tapResults)
            {
                var trOpc = tr.OperatingCenter;
                var renewalTotal = renewalResults.Where(x => x.OperatingCenter == trOpc).Sum(x => x.Total);
                var newTotal = newResults.Where(x => x.OperatingCenter == trOpc).Sum(x => x.Total);
                tr.Total = renewalTotal + newTotal;
            }

            var allResults = renewalResults.Concat(newResults).Concat(tapResults).ToList();
            search.Results = allResults;

            search.Count = allResults.Count;
            return search.Results;
        }

        #endregion

        #endregion

        #endregion

        public ServiceRepository(ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo) : base(session, container,
            authenticationService, roleRepo) { }
    }

    public interface IServiceRepository : IRepository<Service>
    {
        IEnumerable<AggregatedService> GetServicesRenewed(ISearchSet<Service> service);
        IEnumerable<Service> FindManyByServiceNumberAndPremiseNumber(string serviceNumber, string premiseNumber);
        IEnumerable<Service> FindManyByPremiseNumber(string premiseNumber);
        Service FindByPremiseNumberAndServiceNumber(string serviceNumber, string premiseNumber);
        Service FindByPremiseNumber(string premiseNumber);
        Service FindByOperatingCenterAndPremiseNumber(int operatingCenter, string premiseNumber, int id);
        IEnumerable<Service> FindByStreetId(int streetId);

        long? GetNextServiceNumber(string operatingCenterCode);

        IEnumerable<Service> FindByInstallationNumberAndOperatingCenterAndSampleSites(string installation,
            int operatingCenterId);

        bool AnyWithInstallationNumberAndOperatingCenterAndSampleSites(string installation, int operatingCenterId);

        IEnumerable<Service> GetServicesWithoutPremiseLinked();

        #region Reports

        IEnumerable<int> GetDistinctYears();
        IEnumerable<int> GetDistinctYearsRetired();

        IEnumerable<MonthlyServicesInstalledByCategoryViewModel> GetMonthlyServicesInstalledByCategory(
            ISearchMonthlyServicesInstalledByCategory search);

        IEnumerable<MonthlyServicesInstalledByCategoryReportViewModel> GetMonthlyServicesInstalledByCategoryReport(
            ISearchMonthlyServicesInstalledByCategory search);

        IEnumerable<BPUReportForServiceReportItem> GetBPUReportForServices(ISearchBPUReportForServices search);
        IEnumerable<ServicesRetiredReportItem> GetServicesRetired(ISearchServicesRetired search);
        IEnumerable<ServicesRenewedSummaryReportItem> GetServicesRenewedSummary(ISearchServicesRenewedSummary search);

        IEnumerable<ServicesCompletedByCategoryReportItem> GetServicesCompletedByCategory(
            ISearchServicesCompletedByCategory search);

        IEnumerable<TDPendingServicesKPIReportItem> GetTDPendingServicesKPI(ISearchTDPendingServicesKPI search);
        IEnumerable<OpenIssuedServicesReportItem> GetOpenIssuedServices(ISearchSet<OpenIssuedServicesReportItem> model);
        IEnumerable<ServiceQualityAssuranceReportItem> GetServicesQualityAssuranceReport(ISearchServiceQAReport search);

        #endregion
    }

    public static class ServiceRepositoryExtensions
    {
        public static IQueryable<Service> FindManyByWorkOrder(this IRepository<Service> that, WorkOrder workOrder)
        {
            var queryable = that.Where(x =>
                x.OperatingCenter == workOrder.OperatingCenter && x.Town == workOrder.Town &&
                x.PremiseNumber == workOrder.PremiseNumber);

            if (!string.IsNullOrWhiteSpace(workOrder.ServiceNumber))
            {
                queryable = queryable.Where(x => x.ServiceNumber.ToString() == workOrder.ServiceNumber);
            }

            return queryable;
        }
    }
}
