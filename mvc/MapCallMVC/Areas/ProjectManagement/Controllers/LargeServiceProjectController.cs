using System;
using System.Collections.Generic;
using System.ComponentModel;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Configuration;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using System.Web.Mvc;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.ProjectManagement.Controllers
{
    [DisplayName("RP-Large Services")]
    public class LargeServiceProjectController : ControllerBaseWithPersistence<LargeServiceProject, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesProjects;

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            Action allActions = () => {
                this.AddDropDownData<AssetCategory>();
                this.AddDropDownData<AssetType>();
                this.AddDropDownData<PipeDiameter>("ProposedPipeDiameter", d => d.GetAllSorted(x => x.Diameter), d => d.Id, d => d.Diameter);
            };

            switch (action)
            {
                case ControllerAction.Search:
                    allActions();
                    this.AddDynamicDropDownData<OperatingCenter, OperatingCenterDisplayItem>(dataGetter: this.GetUserOperatingCentersFn(ROLE));
                    break;
                case ControllerAction.New:
                    allActions();
                    this.AddDynamicDropDownData<OperatingCenter, OperatingCenterDisplayItem>(dataGetter: this.GetUserOperatingCentersFn(ROLE, RoleActions.Add, extraFilterP: x => x.IsActive));
                    break;
                case ControllerAction.Edit:
                    allActions();
                    this.AddDynamicDropDownData<OperatingCenter, OperatingCenterDisplayItem>(dataGetter: this.GetUserOperatingCentersFn(ROLE, RoleActions.Edit));
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchLargeServiceProject>();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return this.RespondTo(x => {
                x.View(() => ActionHelper.DoShow(id));
                x.Fragment(() => ActionHelper.DoShow(id, new MMSINC.Utilities.ActionHelperDoShowArgs {
                    ViewName = "_ShowPopup",
                    IsPartial = true
                }));
                x.Map(() => {
                    var model = Repository.Find(id);
                    if (model == null)
                        return HttpNotFound();
                    return _container.With((IEnumerable<IThingWithCoordinate>)new [] {model}).GetInstance<MapResultWithCoordinates>();
                });
            });
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchLargeServiceProject search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Map(() => _container.GetInstance<IMapResultFactory>().Build(ModelState, Repository, search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateLargeServiceProject(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateLargeServiceProject model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditLargeServiceProject>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditLargeServiceProject model)
        {
            return ActionHelper.DoUpdate(model);
        }
		
        #endregion

        #region Delete

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        #region Constructors

        public LargeServiceProjectController(ControllerBaseWithPersistenceArguments<IRepository<LargeServiceProject>, LargeServiceProject, User> args) : base(args) {}

        #endregion
    }
}