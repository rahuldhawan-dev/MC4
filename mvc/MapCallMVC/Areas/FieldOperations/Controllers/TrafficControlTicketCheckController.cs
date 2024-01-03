using MapCall.Common.Helpers;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using System.Web.Mvc;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class TrafficControlTicketCheckController : ControllerBaseWithPersistence<TrafficControlTicketCheck, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;
        public const string 
            STARTS_WITH_TICKET = "You must start a response from an existing ticket.",
            TICKET_NOT_FOUND = "Unable to locate the requested traffic control ticket.",
            DUPLICATE_CHECK_NUMBER = "Check Number: {0} is duplicated within the system. This can occur if a single check covers multiple payments.";

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchTrafficControlTicketCheck search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id, null, onModelFound: (check) => {
                if (!check.Unique)
                {
                    DisplayNotification(string.Format(DUPLICATE_CHECK_NUMBER, check.CheckNumber));
                }
            });
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchTrafficControlTicketCheck search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add), ActionBarVisible(false)]
        public ActionResult New(int? trafficControlTicketId)
        {
            if (!trafficControlTicketId.HasValue)
                return HttpNotFound(STARTS_WITH_TICKET);
            var ticket = _container.GetInstance<ITrafficControlTicketRepository>().Find(trafficControlTicketId.Value);
            if (ticket == null)
                return HttpNotFound(TICKET_NOT_FOUND);
            var model = new CreateTrafficControlTicketCheck(_container) {
                TrafficControlTicket = ticket.Id,
                TrafficControlTicketDisplay = ticket,
                Amount = ticket.InvoiceAmount
            };
            return ActionHelper.DoNew(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateTrafficControlTicketCheck model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditTrafficControlTicketCheck>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditTrafficControlTicketCheck model)
        {
            return ActionHelper.DoUpdate(model);
        }
		
        #endregion

        #region Delete

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

		#region Constructors

        public TrafficControlTicketCheckController(ControllerBaseWithPersistenceArguments<IRepository<TrafficControlTicketCheck>, TrafficControlTicketCheck, User> args) : base(args) {}

		#endregion
    }
}