using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Models.ViewModels;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Controllers
{
    [RequiresAdmin]
    public class FacilityKwhCostController : ControllerBaseWithPersistence<FacilityKwhCost, User>
    {
        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            switch (action)
            {
                case ControllerAction.New:
                    this.AddOperatingCenterDropDownData(x => x.IsActive);
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet]
        public ActionResult Search(SearchFacilityKwhCost search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet]
        public ActionResult Index(SearchFacilityKwhCost search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search , new MMSINC.Utilities.ActionHelperDoIndexArgs {
                    RedirectSingleItemToShowView = true
                }));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateFacilityKwhCost(_container));
        }

        [HttpPost]
        public ActionResult Create(CreateFacilityKwhCost model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditFacilityKwhCost>(id);
        }

        [HttpPost]
        public ActionResult Update(EditFacilityKwhCost model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        public FacilityKwhCostController(ControllerBaseWithPersistenceArguments<IRepository<FacilityKwhCost>, FacilityKwhCost, User> args) : base(args) {}
    }
}
