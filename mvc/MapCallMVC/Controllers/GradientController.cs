using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Models.ViewModels;
using MMSINC;
using MMSINC.Authentication;
using MMSINC.Controllers;

namespace MapCallMVC.Controllers
{
    public class GradientController : ControllerBaseWithPersistence<IGradientRepository, Gradient, User>
    {
        public GradientController(ControllerBaseWithPersistenceArguments<IGradientRepository, Gradient, User> args) : base(args) { }

        #region Search/Index/Show

        [RequiresAdmin]
        [HttpGet]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchGradient>();
        }

        [RequiresAdmin]
        [HttpGet]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [RequiresAdmin]
        [HttpGet]
        public ActionResult Index(SearchGradient search)
        {
            return ActionHelper.DoIndex(search);
        }

        #endregion

        #region New/Create

        [RequiresAdmin]
        [HttpGet]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new GradientViewModel(_container));
        }

        [RequiresAdmin]
        [HttpPost]
        public ActionResult Create(GradientViewModel model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [RequiresAdmin]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<GradientViewModel>(id);
        }

        [RequiresAdmin]
        [HttpPost]
        public ActionResult Update(GradientViewModel model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Ajaxie Actions

        [HttpGet]
        public ActionResult ByTownId(params int[] ids)
        {
            return new CascadingActionResult(Repository.GetByTown(ids), "Description", "Id");
        }

        #endregion
    }
}