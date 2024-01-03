using System;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Models.ViewModels;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Controllers
{
    public class GrievanceController : ControllerBaseWithPersistence<IGrievanceRepository, Grievance, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.HumanResourcesUnion;

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            
            Action allActions = () => {
                this.AddDropDownData<GrievanceStatus>("Status");
            };

            switch (action)
            {
                case ControllerAction.New:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Add);
                    allActions();
                    break;
                case ControllerAction.Edit:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Edit);
                    allActions();
                    break;
                case ControllerAction.Search:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE);
                    allActions();
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchGrievance grievance)
        {
            return ActionHelper.DoSearch(grievance);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchGrievance grievance)
        {
            return ActionHelper.DoIndex(grievance);
            // TODO: If excel is added, you'll need to select each property manually if you 
            // want to exclude the IThingWithEmployeee Properties.
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(RoleModules.HumanResourcesUnion, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateGrievance(_container));
        }

        [HttpPost, RequiresRole(RoleModules.HumanResourcesUnion, RoleActions.Add)]
        public ActionResult Create(CreateGrievance model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(RoleModules.HumanResourcesUnion, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditGrievance>(id);
        }

        [HttpPost, RequiresRole(RoleModules.HumanResourcesUnion, RoleActions.Edit)]
        public ActionResult Update(EditGrievance grievance)
        {
            return ActionHelper.DoUpdate(grievance);
        }

        #endregion

        #region GrievanceAccountabilityAction

        [HttpGet]
        public ActionResult ByEmployeeId(int id)
        {
            var results = Repository.GetByEmployeeId(id);
            return new CascadingActionResult(results, "Id", "Id") { SortItemsByTextField = false };
        }

        #endregion

        public GrievanceController(ControllerBaseWithPersistenceArguments<IGrievanceRepository, Grievance, User> args) : base(args) {}
    }
}