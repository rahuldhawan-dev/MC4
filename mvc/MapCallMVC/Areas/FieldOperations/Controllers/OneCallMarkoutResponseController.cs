using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Helpers;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class OneCallMarkoutResponseController : ControllerBaseWithPersistence<OneCallMarkoutResponse, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;
        public const string STARTS_WITH_TICKET = "You must start a response from an existing ticket.",
            TICKET_NOT_FOUND = "Unable to locate the requested one call markout ticket.";

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            this.AddOperatingCenterDropDownData();
            this.AddDropDownData<OneCallMarkoutResponseStatus>();
            this.AddDropDownData<OneCallMarkoutResponseTechnique>();
            this.AddDropDownData<OneCallMarkoutTicket>("CDCCode",
                _ => _container.GetInstance<IOneCallMarkoutTicketRepository>().GetDistinctCDCCodes().AsQueryable(), t => t.CDCCode,
                t => t.CDCCode);
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchOneCallMarkoutResponse search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchOneCallMarkoutResponse search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add), ActionBarVisible(false)]
        public ActionResult New(int? oneCallMarkoutTicketId, string indexQS)
        {
            if (!oneCallMarkoutTicketId.HasValue)
                return HttpNotFound(STARTS_WITH_TICKET);
            var ticket = _container.GetInstance<IRepository<OneCallMarkoutTicket>>().Find(oneCallMarkoutTicketId.Value);
            if (ticket == null)
            {
                return HttpNotFound(TICKET_NOT_FOUND);
            }
            var model = new CreateOneCallMarkoutResponse(_container) {
                OneCallMarkoutTicket = ticket.Id,
                OneCallMarkoutTicketDisplay = ticket,
                CompletedAt = _container.GetInstance<IDateTimeProvider>().GetCurrentDate()
            };
            ViewData[OneCallMarkoutTicketController.TICKET_INDEX_QS_KEY] = indexQS;
            return ActionHelper.DoNew(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateOneCallMarkoutResponse model)
        {
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs
            {
                OnSuccess = () =>
                {
                    if (string.IsNullOrWhiteSpace(model.IndexQS))
                    {
                        return null; // defer to default redirect
                    }
                    
                    return Redirect(Url.Action("Index", "OneCallMarkoutTicket", new { area = "FieldOperations" }) + model.IndexQS);
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditOneCallMarkoutResponse>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditOneCallMarkoutResponse model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        public OneCallMarkoutResponseController(ControllerBaseWithPersistenceArguments<IRepository<OneCallMarkoutResponse>, OneCallMarkoutResponse, User> args) : base(args) {}
    }
}