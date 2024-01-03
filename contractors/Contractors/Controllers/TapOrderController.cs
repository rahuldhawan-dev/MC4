using System.Web.Mvc;
using Contractors.Data.DesignPatterns.Mvc;
using Contractors.Data.Models.Repositories;
using MapCall.Common.Model.Entities;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace Contractors.Controllers
{
    public class TapOrderController : ControllerBaseWithValidation<IServiceRepository, Service>
    {
        #region Search/Index/Show

        [HttpGet]
        public ActionResult Show(int id)
        {
            return this.RespondTo(x => {
                x.View(() => ActionHelper.DoShow(id));
                x.Pdf(() => ActionHelper.DoPdf(id, "Show"));
            });
        }

        #endregion

        public TapOrderController(
            ControllerBaseWithPersistenceArguments<IServiceRepository, Service, ContractorUser> args)
            : base(args) { }
    }
}
