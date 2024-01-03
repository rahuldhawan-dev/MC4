using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Data;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class BacterialWaterSampleRepository : MapCallSecuredRepositoryBase<BacterialWaterSample>,
        IBacterialWaterSampleRepository
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.WaterQualityGeneral;

        #endregion

        #region Fields

        private readonly IPublicWaterSupplyRepository _pwsRepo;
        private readonly IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Properties

        public override RoleModules Role
        {
            get { return ROLE; }
        }

        #endregion

        #region Constructors

        public BacterialWaterSampleRepository(IRepository<AggregateRole> roleRepo, IPublicWaterSupplyRepository pwsRepo,
            IDateTimeProvider dateTimeProvider, ISession session, IAuthenticationService<User> authenticationService,
            IContainer container) :
            base(session, container, authenticationService, roleRepo)
        {
            _pwsRepo = pwsRepo;
            _dateTimeProvider = dateTimeProvider;
        }

        #endregion

        #region Reports

        #region Bacterial Samples High Low

        // This is only internal so unit tests can access it.
        internal IEnumerable<BactiSamplesChlorineHighLowViewModel> GetBactiSamplesChlorineHighLow(
            ISearchBactiSamplesChlorineHighLow search)
        {
            var criteria = GenerateCriteriaForSearchSet(search, null);

            return criteria
                  .CreateAlias("SS.PublicWaterSupply", "PWS")
                  .CreateAlias("SS.Town", "T")
                  .SetProjection(Projections.ProjectionList()
                                            .Add(Projections.Alias(Projections.GroupProperty("PWS.Identifier"),
                                                 "PublicWaterSupply"))
                                            .Add(Projections.Alias(Projections.GroupProperty("T.ShortName"), "Town"))
                                            .Add(Projections.Alias(Projections.GroupProperty("Month"), "Month"))
                                            .Add(Projections.Alias(Projections.GroupProperty("Year"), "Year"))
                                            .Add(Projections.Alias(Projections.Max("Cl2Free"), "Cl2FreeMax"))
                                            .Add(Projections.Alias(Projections.Min("Cl2Free"), "Cl2FreeMin"))
                                            .Add(Projections.Alias(Projections.Max("Cl2Total"), "Cl2TotalMax"))
                                            .Add(Projections.Alias(Projections.Min("Cl2Total"), "Cl2TotalMin")))
                  .SetResultTransformer(Transformers.AliasToBean<BactiSamplesChlorineHighLowViewModel>())
                  .List<BactiSamplesChlorineHighLowViewModel>();
        }

        public IEnumerable<BactiSamplesChlorineHighLowReportViewModel> SearchBactiSamplesChlorineHighLowReport(
            ISearchBactiSamplesChlorineHighLow search)
        {
            // Get Results
            search.EnablePaging = false;
            var results = GetBactiSamplesChlorineHighLow(search).ToList();

            var years = results.Select(x => x.Year).Distinct();
            var pwsids = results.Select(x => x.PublicWaterSupply).Distinct();
            var towns = results.Select(x => x.Town).Distinct();
            var types = new[] {"Cl2FreeMin", "Cl2FreeMax", "Cl2TotalMin", "Cl2TotalMax"};
            var totalRecords = years.Count() * pwsids.Count() * towns.Count() * types.Count();
            var report = new BactiSamplesChlorineHighLowReportViewModel[totalRecords];

            var counter = 0;
            foreach (var year in years)
            {
                foreach (var pwsid in pwsids)
                {
                    foreach (var town in towns)
                    {
                        if (counter >= totalRecords)
                            break;

                        var rowResults =
                            results.Where(x => x.Year == year && x.Town == town && x.PublicWaterSupply == pwsid)
                                   .ToArray();

                        if (rowResults.Any())
                        {
                            Func<int, string, decimal?> getVal = (month, val) => {
                                var first = rowResults.FirstOrDefault(x => x.Month == month);
                                return (first != null) ? (decimal?)first.GetPropertyValueByName(val) : (decimal?)null;
                            };

                            foreach (var type in types)
                            {
                                report[counter] = new BactiSamplesChlorineHighLowReportViewModel {
                                    Year = year,
                                    PublicWaterSupply = pwsid,
                                    Town = town,
                                    Type = Wordify.SpaceOutWordsFromCamelCase(type),
                                    Jan = getVal(1, type),
                                    Feb = getVal(2, type),
                                    Mar = getVal(3, type),
                                    Apr = getVal(4, type),
                                    May = getVal(5, type),
                                    Jun = getVal(6, type),
                                    Jul = getVal(7, type),
                                    Aug = getVal(8, type),
                                    Sep = getVal(9, type),
                                    Oct = getVal(10, type),
                                    Nov = getVal(11, type),
                                    Dec = getVal(12, type)
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

        #region Bacterial Sample Requirements

        public IEnumerable<BacterialWaterSampleRequirementViewModel> GetBacterialWaterSampleRequirements(
            ISearchBacterialWaterSampleRequirementViewModel search)
        {
            BacterialWaterSampleRequirementViewModel result = null;
            SampleSite sampleSite = null;
            PublicWaterSupply publicWaterSupply = null;
            BacterialSampleType bacterialSampleType = null;

            var query = Session.QueryOver<BacterialWaterSample>();
            query.JoinAlias(x => x.SampleSite, () => sampleSite, JoinType.LeftOuterJoin);
            query.JoinAlias(x => sampleSite.PublicWaterSupply, () => publicWaterSupply, JoinType.LeftOuterJoin);
            query.JoinAlias(x => x.BacterialSampleType, () => bacterialSampleType, JoinType.LeftOuterJoin);

            query.SelectList(x => x
                                 .SelectGroup(y => sampleSite.PublicWaterSupply)
                                 .WithAlias(() => result.PublicWaterSupply)
                                 .SelectGroup(y => y.BacterialSampleType).WithAlias(() => result.BacterialSampleType)
                                 .SelectGroup(y => y.Month).WithAlias(() => result.Month)
                                 .SelectGroup(y => y.Year).WithAlias(() => result.Year)
                                 .SelectCount(y => y.Id).WithAlias(() => result.Total)
            );

            query.TransformUsing(Transformers.AliasToBean<BacterialWaterSampleRequirementViewModel>());

            var results = Search(search, query);
            if (search.OnlyWithSamples == true)
                return results;

            // lets get the additional pwsid's that weren't selected and union them as this model class
            var otherResults = _pwsRepo.GetAll().Select(x => new BacterialWaterSampleRequirementViewModel
                {PublicWaterSupply = x});
            return results.Union(otherResults);
        }

        public IEnumerable<BacterialWaterSampleRequirementReportViewModel> GetBacterialWaterSampleRequirementsReport(
            ISearchBacterialWaterSampleRequirementViewModel search)
        {
            var results = GetBacterialWaterSampleRequirements(search)
                         .Where(x => x.PublicWaterSupply != null && x.PublicWaterSupply.Status.Id ==
                              PublicWaterSupplyStatus.Indices.ACTIVE).ToList();

            var years = results.Select(x => x.Year).Where(x => x != 0).Distinct().OrderBy(x => x);
            var pwsids = results.Select(x => x.PublicWaterSupply).Distinct().OrderBy(x => x.Description);
            var types = results.Select(x => x.BacterialSampleType).Where(x => x != null).Distinct();
            var totalRecords = years.Count() * pwsids.Count() * types.Count();
            totalRecords = totalRecords + (pwsids.Count() * years.Count());
            var report = new BacterialWaterSampleRequirementReportViewModel[totalRecords];

            var counter = 0;
            foreach (var year in years)
            {
                foreach (var pwsid in pwsids)
                {
                    if (counter >= totalRecords) break;
                    var rowResults = results.Where(x => x.Year == year && x.PublicWaterSupply == pwsid);
                    if (rowResults.Any())
                    {
                        Func<int, int, int?> getVal = (month, val) => {
                            var first =
                                rowResults.FirstOrDefault(x => x.Month == month && x.BacterialSampleType?.Id == val);
                            return (first != null) ? (int?)first.Total : (int?)null;
                        };

                        foreach (var type in types)
                        {
                            report[counter] = new BacterialWaterSampleRequirementReportViewModel {
                                Year = year,
                                PublicWaterSupply = pwsid,
                                BacterialSampleType = (type != null) ? type.Description : string.Empty,
                                Jan = getVal(1, type.Id),
                                Feb = getVal(2, type.Id),
                                Mar = getVal(3, type.Id),
                                Apr = getVal(4, type.Id),
                                May = getVal(5, type.Id),
                                Jun = getVal(6, type.Id),
                                Jul = getVal(7, type.Id),
                                Aug = getVal(8, type.Id),
                                Sep = getVal(9, type.Id),
                                Oct = getVal(10, type.Id),
                                Nov = getVal(11, type.Id),
                                Dec = getVal(12, type.Id)
                            };
                            counter++;
                        }
                    }

                    report[counter] = new BacterialWaterSampleRequirementReportViewModel {
                        Year = year,
                        PublicWaterSupply = pwsid,
                        BacterialSampleType = "Requirement",
                        Jan = pwsid.JanuaryRequiredBacterialWaterSamples,
                        Feb = pwsid.FebruaryRequiredBacterialWaterSamples,
                        Mar = pwsid.MarchRequiredBacterialWaterSamples,
                        Apr = pwsid.AprilRequiredBacterialWaterSamples,
                        May = pwsid.MayRequiredBacterialWaterSamples,
                        Jun = pwsid.JuneRequiredBacterialWaterSamples,
                        Jul = pwsid.JulyRequiredBacterialWaterSamples,
                        Aug = pwsid.AugustRequiredBacterialWaterSamples,
                        Sep = pwsid.SeptemberRequiredBacterialWaterSamples,
                        Oct = pwsid.OctoberRequiredBacterialWaterSamples,
                        Nov = pwsid.NovemberRequiredBacterialWaterSamples,
                        Dec = pwsid.DecemberRequiredBacterialWaterSamples,
                    };
                    counter++;
                }
            }

            return report.Where(x => x != null);
        }

        #endregion

        public IEnumerable<int> GetDistinctYears()
        {
            var results = (from x in base.Linq where x.SampleCollectionDTM.HasValue select x.Year).Distinct();
            return results;
        }

        public IEnumerable<BacterialWaterSample> GetRecentByPWSIDOfBacterialWaterSample(int bacterialWaterSampleId)
        {
            var that = Find(bacterialWaterSampleId);

            // BacterialWaterSample.SampleSite is a nullable field. If this is null, no results can be returned.
            if (that.SampleSite == null)
            {
                return Enumerable.Empty<BacterialWaterSample>();
            }

            var beginning = _dateTimeProvider.GetCurrentDate().AddHours(-120);

            return Linq.Where(s => s.Id != bacterialWaterSampleId &&
                                   s.SampleSite.PublicWaterSupply == that.SampleSite.PublicWaterSupply &&
                                   s.SampleCollectionDTM >= beginning).ToList();
        }

        #endregion

        #region Exposed Methods

        public IEnumerable<BacterialWaterSample> GetBySampleSiteIdWithBracketSites(int sampleSiteId)
        {
            // This needs to return all samples that are part of that SampleSite and that SampleSite's BracketSites.
            var sampleSite = Session.Query<SampleSite>().Single(x => x.Id == sampleSiteId);

            var samples = new List<BacterialWaterSample>();
            samples.AddRange(Linq.Where(x => x.SampleSite == sampleSite));

            foreach (var bracketSite in sampleSite.BracketSites)
            {
                samples.AddRange(Linq.Where(x => x.SampleSite == bracketSite.BracketSampleSite));
            }

            return samples.OrderBy(x => x.Id).ToList();
        }

        public override BacterialWaterSample Save(BacterialWaterSample entity)
        {
            entity = base.Save(entity);
            // Refresh needs to be called to update any formula fields that might be stale.
            Session.Refresh(entity);
            return entity;
        }

        #endregion
    }

    public interface IBacterialWaterSampleRepository : IRepository<BacterialWaterSample>
    {
        IEnumerable<BactiSamplesChlorineHighLowReportViewModel> SearchBactiSamplesChlorineHighLowReport(
            ISearchBactiSamplesChlorineHighLow search);

        IEnumerable<BacterialWaterSample> GetBySampleSiteIdWithBracketSites(int sampleSiteId);

        IEnumerable<BacterialWaterSampleRequirementViewModel> GetBacterialWaterSampleRequirements(
            ISearchBacterialWaterSampleRequirementViewModel search);

        IEnumerable<BacterialWaterSampleRequirementReportViewModel> GetBacterialWaterSampleRequirementsReport(
            ISearchBacterialWaterSampleRequirementViewModel search);

        IEnumerable<int> GetDistinctYears();
        IEnumerable<BacterialWaterSample> GetRecentByPWSIDOfBacterialWaterSample(int bacterialWaterSampleId);
    }

    public static class BacterialWaterSampleRepositoryExtensions
    {
        public static IEnumerable<BacterialWaterSample> GetSamplesWaitingForLIMSSubmission(
            this IRepository<BacterialWaterSample> repo)
        {
            // NOTE: Method is used by MapCall Scheduler.
            // For now we're only sending samples that are marked as "Ready to Send".
            // In the future we may want to retry sending samples that failed.

            return repo.Where(x => x.LIMSStatus.Id == LIMSStatus.Indices.READY_TO_SEND).ToList();
        }
    }
}
