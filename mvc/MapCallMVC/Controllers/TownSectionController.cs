using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Models.ViewModels;
using MMSINC;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Controllers
{
    public class TownSectionController : ControllerBaseWithPersistence<ITownSectionRepository, TownSection, User>
    {
        #region Constructors

        public TownSectionController(ControllerBaseWithPersistenceArguments<ITownSectionRepository, TownSection, User> args) : base(args) { }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            if (action == ControllerAction.Search)
            {
                this.AddDropDownData<IStateRepository, State>(r => r.GetAll());
            }

            base.SetLookupData(action);
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresAdmin]
        public ActionResult Search(SearchTownSection search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresAdmin]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresAdmin]
        public ActionResult Index(SearchTownSection search)
        {
            return ActionHelper.DoIndex(search);
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresAdmin]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateTownSection(_container));
        }

        [HttpPost, RequiresAdmin]
        public ActionResult Create(CreateTownSection model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresAdmin]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditTownSection>(id);
        }

        [HttpPost, RequiresAdmin]
        public ActionResult Update(EditTownSection model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Ajaxie Actions

        [HttpGet]
        public ActionResult ByTownId(params int[] ids)
        {
            return new CascadingActionResult(Repository.GetByTown(ids).ToList(), "Description", "Id");
        }

        [HttpGet]
        public ActionResult ActiveByTownId(params int[] ids)
        {
            return new CascadingActionResult(Repository.GetActiveByTown(ids).ToList(), "Description", "Id");
        }

        #endregion
    }
}