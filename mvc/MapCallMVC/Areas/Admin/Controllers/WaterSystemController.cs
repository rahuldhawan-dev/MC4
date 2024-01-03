using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Admin.Models;
using MMSINC;
using MMSINC.Authentication;
using MMSINC.Controllers;
namespace MapCallMVC.Areas.Admin.Controllers
{
    public class WaterSystemController : ControllerBaseWithPersistence<IWaterSystemRepository, WaterSystem, User>
    {
        public const string NOT_FOUND = "Water System with the id '{0}' was not found.";

        #region Search/Index/Show
        [RequiresAdmin]
        [HttpGet]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchWaterSystem>();
        }
        [RequiresAdmin]
        [HttpGet]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }
        [RequiresAdmin]
        [HttpGet]
        public ActionResult Index(SearchWaterSystem model)
        {
            return ActionHelper.DoIndex(model);
        }

        #endregion

        #region New/Create
        [RequiresAdmin]
        [HttpGet]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateWaterSystem(_container));
        }
        [RequiresAdmin]
        [HttpPost]
        public ActionResult Create(CreateWaterSystem model)
        {
            return ActionHelper.DoCreate(model);
        }
        [RequiresAdmin]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditWaterSystem>(id);
        }
        [RequiresAdmin]
        [HttpPost]
        public ActionResult Update(EditWaterSystem model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region ByOperatingCenterId
        
        [HttpGet]
        public ActionResult ByOperatingCenterId(int operatingCenterId)
        {
            return new CascadingActionResult<WaterSystem, WaterSystemDisplayItem>(Repository.GetByOperatingCenterId(operatingCenterId), "Display", "Id");
        }
        
        #endregion

        public WaterSystemController(ControllerBaseWithPersistenceArguments<IWaterSystemRepository, WaterSystem, User> args) : base(args) { }
    }
}
