using System.Linq;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.WaterQuality.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using System.Web.Mvc;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.WaterQuality.Controllers
{
    public class WaterConstituentController : ControllerBaseWithPersistence<WaterConstituent, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.WaterQualityGeneral;

        #endregion

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            if (action == ControllerAction.Show)
            {
                this.AddDropDownData<UnitOfWaterSampleMeasure>("UnitOfMeasure");
            }
        }

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchWaterConstituent search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id, null, onModelFound: (constituent) => {
                var existingLimitedStates = constituent.StateLimits.Select(l => l.State.Id).ToArray();
                this.AddDropDownData<State>(r => r.Where(s => !existingLimitedStates.Contains(s.Id)), x => x.Id, x => x.Abbreviation);
            });
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchWaterConstituent search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateWaterConstituent(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateWaterConstituent model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditWaterConstituent>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditWaterConstituent model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion
        
		#region Constructors

        public WaterConstituentController(ControllerBaseWithPersistenceArguments<IRepository<WaterConstituent>, WaterConstituent, User> args) : base(args) {}

		#endregion
    }
}