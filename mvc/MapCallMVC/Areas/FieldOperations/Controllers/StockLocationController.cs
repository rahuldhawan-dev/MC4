using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MMSINC.Authentication;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class StockLocationController : ControllerBaseWithPersistence<StockLocation, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesDataLookups;

        #endregion

        #region Constructors

        public StockLocationController(ControllerBaseWithPersistenceArguments<IRepository<StockLocation>, StockLocation, User> args) : base(args) { }

        #endregion

        #region Index/Search/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchStockLocation>();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchStockLocation search)
        {
            return ActionHelper.DoIndex(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresAdmin]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new StockLocationViewModel(_container));
        }

        [HttpPost, RequiresAdmin]
        public ActionResult Create(StockLocationViewModel model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresAdmin]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<StockLocationViewModel>(id);
        }

        [HttpPost, RequiresAdmin]
        public ActionResult Update(StockLocationViewModel model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion
    }
}