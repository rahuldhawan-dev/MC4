using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    // NOTE: This repository can't currently have properly secured Criteria/Linq methods due to different roles
    // being used to access this data. ie: WaterQualityGeneral uses this for cascades, but PublicWaterSupplyController
    // uses the EnvironmentalGeneral role.

    public class PublicWaterSupplyRepository : SecuredRepositoryBase<PublicWaterSupply, User>,
        IPublicWaterSupplyRepository
    {
        #region Fields

        private readonly IRepository<OperatingCenter> _operatingCenterRepository;

        #endregion

        #region Constructors

        public PublicWaterSupplyRepository(IRepository<OperatingCenter> operatingCenterRepository, ISession session,
            IContainer container, IAuthenticationService<User> authenticationService) : base(session,
            authenticationService, container)
        {
            _operatingCenterRepository = operatingCenterRepository;
        }

        #endregion

        #region Exposed Methods

        public override IQueryable<PublicWaterSupply> GetAllSorted()
        {
            return GetAll().OrderBy(x => x.Identifier).ThenBy(x => x.OperatingArea).ThenBy(x => x.System);
        }

        public IEnumerable<PublicWaterSupply> GetAllFilteredByWaterQualityGeneralRole()
        {
            // NOTE: This is not implemented as MapCallSecuredRepositoryBase because this filtering is 
            // not using the same role that PublicWaterSupplyController uses and is only being requested
            // for WaterQuality related things.
            var roleMatch = CurrentUser.GetCachedMatchingRoles(RoleModules.WaterQualityGeneral);

            if (CurrentUser.IsAdmin || roleMatch.HasWildCardMatch)
            {
                return Linq;
            }

            return Linq.Where(x =>
                x.OperatingCenterPublicWaterSupplies.Any(y =>
                    roleMatch.OperatingCenters.Contains(y.OperatingCenter.Id))).ToList();
        }

        public IQueryable<PublicWaterSupply> GetActiveByOperatingCenterId(params int[] operatingCenterIds)
        {
            var query = operatingCenterIds.Length == 0
                ? Linq
                : Linq.Where(x => x.OperatingCenterPublicWaterSupplies.Any(y => operatingCenterIds.Contains(y.OperatingCenter.Id)));

            return query.Where(x => x.Status.Id == PublicWaterSupplyStatus.Indices.ACTIVE)
                        .OrderBy(x => x.Identifier);
        }

        public IQueryable<PublicWaterSupply> GetActiveOrPendingByOperatingCenterId(params int[] operatingCenterIds)
        {
            var query = operatingCenterIds.Length == 0
                ? Linq
                : Linq.Where(x => x.OperatingCenterPublicWaterSupplies.Any(y => operatingCenterIds.Contains(y.OperatingCenter.Id)));

            return query.Where(x => x.Status.Id == PublicWaterSupplyStatus.Indices.ACTIVE ||
                                    x.Status.Id == PublicWaterSupplyStatus.Indices.PENDING ||
                                    x.Status.Id == PublicWaterSupplyStatus.Indices.PENDING_MERGER)
                        .OrderBy(x => x.Identifier);
        }

        public IQueryable<PublicWaterSupply> GetByOperatingCenterId(params int[] operatingCenterIds) =>
            operatingCenterIds.Length == 0
                ? Linq
                : Linq.Where(x => x.OperatingCenterPublicWaterSupplies.Any(y => operatingCenterIds.Contains(y.OperatingCenter.Id)))
                      .OrderBy(x => x.Identifier);

        public IEnumerable<PublicWaterSupply> GetAWOwnedByOperatingCenterIds(IEnumerable<int> operatingCenterIds)
        {
            // WO-193773 wants this to sort by identifier.
            return (from pws in Linq
                    where pws.AWOwned == true
                          && pws.OperatingCenterPublicWaterSupplies.Any(x =>
                              operatingCenterIds.Contains(x.OperatingCenter.Id))
                    select pws).OrderBy(x => x.Identifier).ToList();
        }

        public IQueryable<OperatingCenter> GetDistinctOperatingCentersByPublicWaterSupplies(int[] pwsids)
        {
            // BacterialWaterSample search needs the distinct operating centers based on any number of PWSIDS selected.

            if (pwsids == null || !pwsids.Any())
            {
                return Enumerable.Empty<OperatingCenter>().AsQueryable();
            }

            var result = Linq.Where(x => pwsids.Contains(x.Id))
                             .SelectMany(x => x.OperatingCenterPublicWaterSupplies)
                             .Select(x => x.OperatingCenter.Id)
                             .Distinct()
                             .ToArray(); // ToList this early. Calling Intersect before the query is executed isn't supported by NHibernate.

            var roleMatch = CurrentUser.GetCachedMatchingRoles(RoleModules.WaterQualityGeneral);

            if (CurrentUser.IsAdmin || roleMatch.HasWildCardMatch)
            {
                return _operatingCenterRepository.Where(oc => result.Contains(oc.Id));
            }

            return _operatingCenterRepository.Where(oc =>
                roleMatch.OperatingCenters.Contains(oc.Id) && result.Contains(oc.Id));
        }

        public IEnumerable<YearlyWaterSampleComplianceReportItem> SearchYearlyWaterSampleComplianceReport(
            ISearchYearlyWaterSampleComplianceReport model)
        {
            if (model.CertifiedYear == null)
            {
                // Model validation should be handling this already, this is just an extra precaution 
                // in case that gets removed accidentally.
                throw new InvalidOperationException("CertifiedYear is required.");
            }

            // This is being done in a weird way because we only need to do the basic PublicWaterSupply search.
            // However, we can't just pass the special search model and return it since it doesn't implement
            // ISearchSet<PublicWaterSupply> This is easier to do than writing up a special query that selects
            // all the extra PWSID stuff for display. The query join still makes this do a single query so
            // there's no performance problem with looping over the PublicWaterSupply.WaterSampleComplianceForms prop.

            var properSearchModel = new YearlySearchImplementation {
                State = model.State,
                OperatingCenter = model.OperatingCenter,
                EntityId = model.EntityId,
                EnablePaging =
                    model.EnablePaging, // This isn't a paged report, but the controller is responsible for setting this.
                AWOwned = true // This is hardcoded because this report should only include AW Owned PWSIDs.
            };

            var query = Session.QueryOver<PublicWaterSupply>();
            WaterSampleComplianceForm wscForm = null;
            query.JoinAlias(x => x.WaterSampleComplianceForms, () => wscForm,
                NHibernate.SqlCommand.JoinType.LeftOuterJoin);

            // Need to do a distinct because the join causes duplicate results to be returned.
            var pwsidResults = Search(properSearchModel, query).Distinct().ToList();

            var reportResults = new List<YearlyWaterSampleComplianceReportItem>();

            foreach (var result in pwsidResults)
            {
                var reportItem = new YearlyWaterSampleComplianceReportItem();
                reportItem.PublicWaterSupply = result;

                var formsForYear = result.WaterSampleComplianceForms.Where(x => x.CertifiedYear == model.CertifiedYear)
                                         .ToArray();

                reportItem.JanuaryForm = formsForYear.SingleOrDefault(x => x.CertifiedMonth == 1);
                reportItem.FebruaryForm = formsForYear.SingleOrDefault(x => x.CertifiedMonth == 2);
                reportItem.MarchForm = formsForYear.SingleOrDefault(x => x.CertifiedMonth == 3);
                reportItem.AprilForm = formsForYear.SingleOrDefault(x => x.CertifiedMonth == 4);
                reportItem.MayForm = formsForYear.SingleOrDefault(x => x.CertifiedMonth == 5);
                reportItem.JuneForm = formsForYear.SingleOrDefault(x => x.CertifiedMonth == 6);
                reportItem.JulyForm = formsForYear.SingleOrDefault(x => x.CertifiedMonth == 7);
                reportItem.AugustForm = formsForYear.SingleOrDefault(x => x.CertifiedMonth == 8);
                reportItem.SeptemberForm = formsForYear.SingleOrDefault(x => x.CertifiedMonth == 9);
                reportItem.OctoberForm = formsForYear.SingleOrDefault(x => x.CertifiedMonth == 10);
                reportItem.NovemberForm = formsForYear.SingleOrDefault(x => x.CertifiedMonth == 11);
                reportItem.DecemberForm = formsForYear.SingleOrDefault(x => x.CertifiedMonth == 12);
                reportResults.Add(reportItem);
            }

            model.Results = reportResults;
            model.Count = reportResults.Count;
            model.PageCount = properSearchModel.PageCount;
            model.PageNumber = properSearchModel.PageNumber;

            return model.Results;
        }

        public IEnumerable<PublicWaterSupply> FindByPartialIdMatch(string partialId)
        {
            if (string.IsNullOrWhiteSpace(partialId))
            {
                return Enumerable.Empty<PublicWaterSupply>();
            }

            var matches = from i in Linq
                          where i.Identifier
                                 .Contains(partialId)
                          select i;

            return matches;
        }

        public IQueryable<PublicWaterSupply> GetByStateId(params int[] stateIds) =>
            stateIds.Length == 0
                ? Linq
                : Linq.Where(x => stateIds.Contains(x.State.Id));

        public IQueryable<PublicWaterSupply> GetActiveByStateIdOrOperatingCenterId(int[] stateIds, int[] operatingCenterIds)
        {
            var results = Linq.Where(x => x.Status.Id == PublicWaterSupplyStatus.Indices.ACTIVE);

            if (stateIds != null && stateIds.Any())
            {
                results = results.Where(x => stateIds.Contains(x.State.Id));
            }

            if (operatingCenterIds != null && operatingCenterIds.Any())
            {
                results = results.Where(x => x.OperatingCenterPublicWaterSupplies.Any(y => operatingCenterIds.Contains(y.OperatingCenter.Id)));
            }

            return results;
        }

        #endregion

        #region Helper classes

        private class YearlySearchImplementation : SearchSet<PublicWaterSupply>, IBasicYearlyWaterSampleComplianceReport
        {
            public int? State { get; set; }
            public int[] OperatingCenter { get; set; }
            public int[] EntityId { get; set; }
            public int? CertifiedYear { get; set; }
            public bool AWOwned { get; set; }
        }

        #endregion
    }

    public interface IPublicWaterSupplyRepository : IRepository<PublicWaterSupply>
    {
        IEnumerable<PublicWaterSupply> GetAllFilteredByWaterQualityGeneralRole();

        IQueryable<PublicWaterSupply> GetActiveByOperatingCenterId(params int[] operatingCenterIds);

        IQueryable<PublicWaterSupply> GetActiveOrPendingByOperatingCenterId(params int[] operatingCenterIds);

        IQueryable<PublicWaterSupply> GetByOperatingCenterId(params int[] operatingCenterIds);

        IEnumerable<PublicWaterSupply> GetAWOwnedByOperatingCenterIds(IEnumerable<int> operatingCenterIds);

        IQueryable<OperatingCenter> GetDistinctOperatingCentersByPublicWaterSupplies(int[] pwsids);

        IEnumerable<YearlyWaterSampleComplianceReportItem> SearchYearlyWaterSampleComplianceReport(
            ISearchYearlyWaterSampleComplianceReport model);

        IEnumerable<PublicWaterSupply> FindByPartialIdMatch(string partialId);

        IQueryable<PublicWaterSupply> GetByStateId(params int[] stateIds);

        IQueryable<PublicWaterSupply> GetActiveByStateIdOrOperatingCenterId(int[] stateIds, int[] operatingCenterIds);
    }
}
