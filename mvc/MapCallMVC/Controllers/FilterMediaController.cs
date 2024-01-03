using System.ComponentModel;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Controllers
{
    [DisplayName("Filter Media")]
    public class FilterMediaController : ControllerBaseWithPersistence<FilterMedia, User>
    {
        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            switch (action)
            {
                case ControllerAction.Search:
                    this.AddDropDownData<IStateRepository, State>(r => r.GetWithFacilities(), s => s.Id,
                        s => s.Abbreviation);
                    goto default;
                case ControllerAction.Edit:
                case ControllerAction.New:
                    this.AddDynamicDropDownData<Facility, FacilityDisplayItem>();
                    goto default;
                default:
                    this.AddDropDownData<FilterMediaType>();
                    this.AddDropDownData<FilterMediaWashType>();
                    this.AddDropDownData<FilterMediaLevelControlMethod>();
                    this.AddDropDownData<FilterMediaFilterType>();
                    this.AddDropDownData<FilterMediaLocation>();
                    break;
            }

        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(RoleModules.ProductionFacilities)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(RoleModules.ProductionFacilities)]
        public ActionResult Search(SearchFilterMedia model)
        {
            return ActionHelper.DoSearch(model);
        }

        [HttpGet, RequiresRole(RoleModules.ProductionFacilities)]
        public ActionResult Index(SearchFilterMedia model)
        {
            return this.RespondTo(formatter => {
                formatter.View(() => ActionHelper.DoIndex(model));
                formatter.Excel(() => ActionHelper.DoExcel(model));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(RoleModules.ProductionFacilities, RoleActions.Add)]
        public ActionResult New(CreateFilterMedia model)
        {
            ModelState.Clear();
            model.MapSelf();
            return ActionHelper.DoNew(model);
        }

        [HttpPost, RequiresRole(RoleModules.ProductionFacilities, RoleActions.Add)]
        public ActionResult Create(CreateFilterMedia model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(RoleModules.ProductionFacilities, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<UpdateFilterMedia>(id);
        }

        [HttpPost, RequiresRole(RoleModules.ProductionFacilities, RoleActions.Edit)]
        public ActionResult Update(UpdateFilterMedia model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        public FilterMediaController(ControllerBaseWithPersistenceArguments<IRepository<FilterMedia>, FilterMedia, User> args) : base(args) {}
    }
}
