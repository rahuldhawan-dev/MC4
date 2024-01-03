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
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface ISewerMainCleaningRepository : IRepository<SewerMainCleaning>
    {
        IEnumerable<SewerMainCleaning> GetSewerMainCleaningsWithSapRetryIssues();
        IEnumerable<int> GetDistinctYearsCompleted();
        IEnumerable<SewerMainCleaningFootageReport> SearchSewerMainCleaningFootageReport(
            ISearchSewerMainCleaningFootageReport search);
    }

    public class SewerMainCleaningRepository : MapCallSecuredRepositoryBase<SewerMainCleaning>,
        ISewerMainCleaningRepository
    {
        #region Properties

        public override RoleModules Role
        {
            get { return RoleModules.FieldServicesAssets; }
        }

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

        public override IQueryable<SewerMainCleaning> Linq
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

        #endregion

        #region Exposed Methods

        public IEnumerable<SewerMainCleaning> GetSewerMainCleaningsWithSapRetryIssues()
        {
            return this.GetSewerMainCleaningsWithSapRetryIssuesImpl();
        }

        public IEnumerable<int> GetDistinctYearsCompleted()
        {
            return
                (from smc in base.Linq
                 where smc.Date.HasValue
                 select smc.Date.Value.Year
                ).Distinct();
        }

        public IEnumerable<SewerMainCleaningFootageReportItem> GetSewerMainCleaningFootageReportItems(
            ISearchSewerMainCleaningFootageReport search)
        {
            SewerMainCleaningFootageReportItem result = null;
            var query = Session.QueryOver<SewerMainCleaning>().Where(smc => smc.Year == search.Year);

            OperatingCenter opc = null;
            query.JoinAlias(x => x.OperatingCenter, () => opc, JoinType.InnerJoin);

            Town town = null;
            query.JoinAlias(x => x.Town, () => town, JoinType.InnerJoin);

            SewerMainInspectionType type = null;
            query.JoinAlias(x => x.InspectionType, () => type, JoinType.LeftOuterJoin);

            if (search.InspectionType.HasValue)
            {
                query = query.Where(smc => search.InspectionType == smc.InspectionType.Id);
            }
            if (search.OperatingCenter.HasValue)
            {
                query = query.Where(smc => search.OperatingCenter == smc.OperatingCenter.Id);
            }
            if (search.Town.HasValue)
            {
                query = query.Where(smc => search.Town == smc.Town.Id);
            }

            query.SelectList(x => x
                                 .SelectGroup(_ => opc.OperatingCenterCode).WithAlias(() => result.OperatingCenter)
                                 .SelectGroup(_ => town.ShortName).WithAlias(() => result.Town)
                                 .SelectGroup(_ => type.Description).WithAlias(() => result.InspectionType)
                                 .SelectGroup(h => h.Date.Value.Year).WithAlias(() => result.Year)
                                 .SelectGroup(h => h.Date.Value.Month).WithAlias(() => result.Month)
                                 .SelectSum(h => h.FootageOfMainInspected).WithAlias(() => result.Total)
            );

            query.TransformUsing(Transformers.AliasToBean<SewerMainCleaningFootageReportItem>());

            var resultList = query.List<SewerMainCleaningFootageReportItem>();
            return resultList.OrderBy(i => i.OperatingCenter);
        }

        public IEnumerable<SewerMainCleaningFootageReport> SearchSewerMainCleaningFootageReport(
            ISearchSewerMainCleaningFootageReport search)
        {
            var results = GetSewerMainCleaningFootageReportItems(search);
            var sewerMainCleaningFootageReportItems = results as SewerMainCleaningFootageReportItem[] ?? results.ToArray();
            if (!sewerMainCleaningFootageReportItems.Any())
            {
                return Enumerable.Empty<SewerMainCleaningFootageReport>();
            }

            var years = sewerMainCleaningFootageReportItems.Select(x => x.Year).Distinct().OrderByDescending(x => x);
            var types = sewerMainCleaningFootageReportItems.Select(x => x.InspectionType).Distinct();
            var opCenters = sewerMainCleaningFootageReportItems.Select(x => x.OperatingCenter).Distinct();
            var towns = sewerMainCleaningFootageReportItems.Select(x => x.Town).Distinct();
            var totalRecords = sewerMainCleaningFootageReportItems.Count();
            var report = new SewerMainCleaningFootageReport[totalRecords];

            var counter = 0;

            foreach (var year in years)
            {
                foreach (var opCenter in opCenters)
                {
                    foreach (var town in towns)
                    {
                        foreach (var type in types)
                        {
                            if (counter >= totalRecords)
                                break;

                            var rowResults =
                                sewerMainCleaningFootageReportItems.Where(x => x.Year == year && x.OperatingCenter == opCenter
                                    && x.Town == town && x.InspectionType == type && x.Total > 0).ToArray();
                            if (rowResults.Any())
                            {
                                float jan = rowResults.Where(x => x.Month == 1).Sum(row => row.Total);
                                float feb = rowResults.Where(x => x.Month == 2).Sum(row => row.Total);
                                float mar = rowResults.Where(x => x.Month == 3).Sum(row => row.Total);
                                float apr = rowResults.Where(x => x.Month == 4).Sum(row => row.Total);
                                float may = rowResults.Where(x => x.Month == 5).Sum(row => row.Total);
                                float jun = rowResults.Where(x => x.Month == 6).Sum(row => row.Total);
                                float jul = rowResults.Where(x => x.Month == 7).Sum(row => row.Total);
                                float aug = rowResults.Where(x => x.Month == 8).Sum(row => row.Total);
                                float sep = rowResults.Where(x => x.Month == 9).Sum(row => row.Total);
                                float oct = rowResults.Where(x => x.Month == 10).Sum(row => row.Total);
                                float nov = rowResults.Where(x => x.Month == 11).Sum(row => row.Total);
                                float dec = rowResults.Where(x => x.Month == 12).Sum(row => row.Total);
                                float total = rowResults.Sum(row => row.Total);

                                report[counter] = new SewerMainCleaningFootageReport {
                                    Year = year,
                                    OperatingCenter = opCenter,
                                    Town = town,
                                    InspectionType = type,
                                    Total = total,
                                    Jan = jan,
                                    Feb = feb,
                                    Mar = mar,
                                    Apr = apr,
                                    May = may,
                                    Jun = jun,
                                    Jul = jul,
                                    Aug = aug,
                                    Sep = sep,
                                    Oct = oct,
                                    Nov = nov,
                                    Dec = dec
                                };

                                counter++;
                            }
                        }
                    }
                }
            }

            return report.Where(x => x != null);
        }

        #endregion

        public SewerMainCleaningRepository(ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo) : base(session, container,
            authenticationService, roleRepo) { }
    }

    public static class ISewerMainCleaningRepositoryExtensions
    {
        public static IEnumerable<SewerMainCleaning> GetSewerMainCleaningsWithSapRetryIssuesImpl(
            this IRepository<SewerMainCleaning> that)
        {
            return that.Where(x =>
                x.SAPErrorCode != null && x.SAPErrorCode != "" && x.SAPErrorCode.StartsWith("RETRY"));
        }
    }
}
