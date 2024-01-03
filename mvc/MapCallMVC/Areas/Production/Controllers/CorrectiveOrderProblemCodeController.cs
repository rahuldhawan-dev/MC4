using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Production.Models.ViewModels;
using MMSINC;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Production.Controllers
{
    public class CorrectiveOrderProblemCodeController : ControllerBaseWithPersistence<IRepository<CorrectiveOrderProblemCode>, CorrectiveOrderProblemCode, User>
    {
        #region Constructors

        public CorrectiveOrderProblemCodeController(ControllerBaseWithPersistenceArguments<IRepository<CorrectiveOrderProblemCode>, CorrectiveOrderProblemCode, User> args) : base(args) { }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresAdmin]
        public ActionResult Search(SearchCorrectiveOrderProblemCode search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresAdmin]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresAdmin]
        public ActionResult Index(SearchCorrectiveOrderProblemCode search)
        {
            return this.RespondTo(formatter => {
                formatter.View(() => ActionHelper.DoIndex(search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresAdmin]
        public ActionResult New()
        {
            return ActionHelper.DoNew(ViewModelFactory.Build<CorrectiveOrderProblemCodeViewModel>());
        }

        [HttpPost, RequiresAdmin]
        public ActionResult Create(CorrectiveOrderProblemCodeViewModel model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresAdmin]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<CorrectiveOrderProblemCodeViewModel>(id, null, entity => { });
        }

        [HttpPost, RequiresAdmin]
        public ActionResult Update(CorrectiveOrderProblemCodeViewModel model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs());
        }

        #endregion

        #region ByEquipmentTypeId

        [HttpGet]
        public ActionResult ByEquipmentTypeId(int equipmentTypeId)
        {
            return new CascadingActionResult<CorrectiveOrderProblemCode, CorrectiveOrderProblemCodeDisplayItem>(Repository.Where(x => x.EquipmentTypes.Any(t => t.Id == equipmentTypeId)));
        }

        #endregion
    }
}