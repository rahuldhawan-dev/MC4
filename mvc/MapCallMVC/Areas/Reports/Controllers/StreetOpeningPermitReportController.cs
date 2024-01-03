using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Reports.Models;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using Permits.Data.Client.Entities;
using Permits.Data.Client.Repositories;
using County = MapCall.Common.Model.Entities.County;
using State = MapCall.Common.Model.Entities.State;

namespace MapCallMVC.Areas.Reports.Controllers
{
    public class StreetOpeningPermitReportController : ControllerBaseWithPersistence<OperatingCenter, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;

        #endregion

        #region Fields

        private readonly IPermitsRepositoryFactory _permitsRepositoryFactory;

        #endregion

        #region Private Methods

        private void GatherResults(SearchStreetOpeningPermit search)
        {
            var reportRepo =
                _permitsRepositoryFactory.GetRepository<CompanyReport>(AuthenticationService.CurrentUser
                   .DefaultOperatingCenter.PermitsUserName);
            var workOrderRepo = _container.GetInstance<IRepository<WorkOrder>>();
            var sopRepo = _container.GetInstance<IRepository<StreetOpeningPermit>>();
            var results = new List<SearchStreetOpeningPermit.ReportItem>();

            var searchParams = search.ToSearchParams();

            if (search.OperatingCenter.HasValue)
            {
                ApplyOperatingCenterParams(search, searchParams);
            }

            foreach (var permit in reportRepo.Search(searchParams))
            {
                var workOrder = string.IsNullOrWhiteSpace(permit.ArbitraryIdentifier) ||
                                !Regex.IsMatch(permit.ArbitraryIdentifier, @"^\d+$")
                    ? null
                    : workOrderRepo.Find(Int32.Parse(permit.ArbitraryIdentifier));

                if (workOrder != null && search.OperatingCenter.HasValue &&
                    workOrder.OperatingCenter.Id != search.OperatingCenter)
                {
                    continue;
                }

                var streetOpeningPermit = sopRepo.Find(permit.PermitId);
                results.Add(MapReportItem(permit, workOrder, streetOpeningPermit));
            }

            search.Results = results;
        }

        private static SearchStreetOpeningPermit.ReportItem MapReportItem(CompanyReport permit, WorkOrder workOrder, StreetOpeningPermit streetOpeningPermit)
        {
            return new SearchStreetOpeningPermit.ReportItem {
                PermitId = permit.PermitId.ToString(),
                WorkOrderId = workOrder?.Id.ToString() ?? permit.ArbitraryIdentifier,
                OperatingCenter = workOrder?.OperatingCenter?.Description ?? string.Empty,
                StreetAddress = permit.StreetAddress,
                NearestCrossStreet = workOrder?.NearestCrossStreet?.FullStName ?? string.Empty,
                CountyTown = string.IsNullOrWhiteSpace(permit.Town)
                    ? permit.County
                    : permit.Town,
                CreatedAt = permit.CreatedAt.ToString(),
                PermitReceivedDate = streetOpeningPermit?.DateIssued?.ToString() ?? string.Empty,
                PaymentReceivedAt = permit.PaymentReceivedAt.ToString(),
                SubmittedAt = permit.SubmittedAt.ToString(),
                TotalCharged = permit.TotalCharged.ToString(),
                PermitFee = permit.PermitFee.ToString(),
                InspectionFee = permit.InspectionFee.ToString(),
                BondFee = permit.BondFee.ToString(),
                Reconciled = !permit.Reconciled.HasValue || !permit.Reconciled.Value ? "no" : "yes",
                WorkDescription = workOrder?.WorkDescription?.Description ?? string.Empty,
                AccountCharged = workOrder?.AccountCharged ?? string.Empty,
                AccountingType = workOrder?.WorkDescription?.AccountingType?.Description ?? string.Empty,
                CanceledAt = permit.CanceledAt.ToString(),
                Refunded = permit.Refunded ? "yes" : "no",
            };
        }

        private void ApplyOperatingCenterParams(SearchStreetOpeningPermit search, NameValueCollection searchParams)
        {
            var operatingCenter = Repository.Find(search.OperatingCenter.Value);
            var counties =
                operatingCenter.Towns.Map<Town, County>(t => t.County).Distinct().ToList();
            var state = counties.Map<County, State>(c => c.State).Distinct().Single();

            searchParams["State"] = state.Abbreviation;

            foreach (var county in counties)
            {
                searchParams.Add("Counties", county.Name);
            }
        }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            if (action == ControllerAction.Search)
            {
                this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE);
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchStreetOpeningPermit search)
        {
            SetLookupData(ControllerAction.Search);
            return View(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchStreetOpeningPermit search)
        {
            if (string.IsNullOrWhiteSpace(AuthenticationService.CurrentUser.DefaultOperatingCenter.PermitsUserName))
            {
                DisplayErrorMessage(
                    $"Your user account's default operating center, {AuthenticationService.CurrentUser.DefaultOperatingCenter.OperatingCenterCode}, does not have a permits API user name associated with it.");
                return DoRedirectionToAction("Search", search);
            }
            if (!ModelState.IsValid)
            {
                DisplayModelStateErrors();
                return DoRedirectionToAction("Search", search);
            }

            GatherResults(search);

            return this.RespondTo(f => {
                f.View(() => View(search));
                f.Excel(() => this.Excel(search.Results));
            });
        }

        #endregion

        public StreetOpeningPermitReportController(
            ControllerBaseWithPersistenceArguments<IRepository<OperatingCenter>, OperatingCenter, User> args,
            IPermitsRepositoryFactory permitsRepositoryFactory) : base(args)
        {
            _permitsRepositoryFactory = permitsRepositoryFactory;
        }
    }
}