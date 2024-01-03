using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallApi.Models;
using MMSINC.Controllers;
using MMSINC.Data;

namespace MapCallApi.Controllers
{
    public class GeneralLiabilityClaimController : ControllerBaseWithPersistence<IGeneralLiabilityClaimRepository, GeneralLiabilityClaim, User>
    {
        public const RoleModules ROLE = RoleModules.OperationsHealthAndSafety;

        #region Exposed Methods

        [RequiresRole(ROLE)]
        public ActionResult Index(SearchGeneralLiabilityClaim search)
        {
            if (search.IncidentDateTime == null || search.IncidentDateTime.Operator != RangeOperator.Between ||
                search.IncidentDateTime.End == null || search.IncidentDateTime.End.Value
                    .Subtract(search.IncidentDateTime.Start.Value).TotalDays > 30)
            {
                throw new InvalidOperationException(
                    "IncidentDateTime must be a 'between' search of a month or less.");
            }
            search.EnablePaging = false;
            return Json(Repository.Search(search).Select(c => new {
                c.Id,
                OperatingCenter = c.OperatingCenter.ToString(),
                CompanyContact = c.CompanyContact.ToString(),
                ClaimsRepresentative = c.ClaimsRepresentative.ToString(),
                c.ClaimNumber,
                c.Name,
                LiabilityType = c.LiabilityType.ToString(),
                c.Description,
                c.PhhContacted,
                c.IncidentDateTime,
                c.PoliceCalled,
                c.IncidentNotificationDate
            }), JsonRequestBehavior.AllowGet);
        }

        #endregion

        public GeneralLiabilityClaimController(ControllerBaseWithPersistenceArguments<IGeneralLiabilityClaimRepository, GeneralLiabilityClaim, User> args) : base(args) { }
    }
}