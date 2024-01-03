using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Helpers;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class OneCallMarkoutTicketController : ControllerBaseWithPersistence<IOneCallMarkoutTicketRepository, OneCallMarkoutTicket, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;
        public const string TICKET_INDEX_QS_KEY = "ticket index query string";

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            this.AddOperatingCenterDropDownData();
            this.AddDropDownData<OneCallMarkoutMessageType>("MessageType");
            this.AddDropDownData<OneCallMarkoutTicket>("CDCCode", _ => Repository.GetDistinctCDCCodes().AsQueryable(), t => t.CDCCode, t => t.CDCCode);
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchOneCallMarkoutTicket search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            if (Request.UrlReferrer != null && Request.UrlReferrer.AbsolutePath.EndsWith("FieldOperations/OneCallMarkoutTicket"))
            {
                ViewData[TICKET_INDEX_QS_KEY] = Request.UrlReferrer.Query;
            }
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchOneCallMarkoutTicket search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => {
                    search.EnablePaging = false;
                    var results = Repository.Search(search).Select(t => new {
                        t.Id,
                        t.MessageType,
                        t.NearestCrossStreet,
                        t.OperatingCenter,
                        t.RequestNumber,
                        t.RelatedRequestNumber,
                        t.RelatedRequest,
                        t.DateTransmitted,
                        t.DateReceived,
                        County = t.CountyText,
                        Town = t.TownText,
                        Street = t.StreetText,
                        t.NearestCrossStreetText,
                        t.TypeOfWork,
                        t.WorkingFor,
                        t.Excavator,
                        t.CDCCode,
                        t.FullText,
                        t.HasResponse
                    });
                    return this.Excel(results);
                });
            });
        }

        #endregion

        #region Edit/Update

        /// <summary>
        /// Notes/Docs rely on this method existing.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, RequiresRole(ROLE, RoleActions.Edit),ActionBarVisible(false)]
        public ActionResult Edit(int id)
        {
            return HttpNotFound();
        }

        #endregion

        #region GetTowns

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult GetCounties(int operatingCenter)
        {
            return new CascadingActionResult(Repository.GetDistinctCounties(operatingCenter), "CountyText", "CountyText");
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult GetTowns(int operatingCenter, string county)
        {
            return new CascadingActionResult(Repository.GetDistinctTowns(operatingCenter, county), "TownText", "TownText");
        }

        #endregion

        public OneCallMarkoutTicketController(ControllerBaseWithPersistenceArguments<IOneCallMarkoutTicketRepository, OneCallMarkoutTicket, User> args) : base(args) {}
    }
}