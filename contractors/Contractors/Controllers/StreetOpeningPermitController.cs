using System.Web.Mvc;
using System.Web.UI;
using Contractors.Data.DesignPatterns.Mvc;
using MapCall.Common.Model.Entities;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;

namespace Contractors.Controllers
{
    public class StreetOpeningPermitController : ControllerBaseWithValidation<StreetOpeningPermit>
    {
        #region Constants

        public const string NO_SUCH_PERMIT = "No such street opening permit.";

        #endregion

        #region Index/Show

        [HttpGet, NoCache]
        public ActionResult Index(int workOrderId)
        {
            var model = GetWorkOrder(workOrderId);
            if (model == null)
            {
                return NoSuchWorkOrder();
            }
            return PartialView("_Index", model);
        }

        [HttpGet, NoCache]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id, new MMSINC.Utilities.ActionHelperDoShowArgs {
                IsPartial = true,
                NotFound = NO_SUCH_PERMIT
            });
        }

        #endregion

        public StreetOpeningPermitController(ControllerBaseWithPersistenceArguments<IRepository<StreetOpeningPermit>, StreetOpeningPermit, ContractorUser> args) : base(args) {}
    }
}