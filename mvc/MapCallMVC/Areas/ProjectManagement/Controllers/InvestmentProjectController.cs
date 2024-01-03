using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Configuration;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.ProjectManagement.Controllers
{
    public class InvestmentProjectController : ControllerBaseWithPersistence<InvestmentProject, User>
    {
        #region Consts

        private const RoleModules ROLE = RoleModules.FieldServicesProjects;

        #endregion  

        #region Constructors
        
        public InvestmentProjectController(ControllerBaseWithPersistenceArguments<IRepository<InvestmentProject>, InvestmentProject, User> args) : base(args) {}
        
        #endregion

        #region Public Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            if (action == ControllerAction.Show)
            {
                return;
            }

            if (action == ControllerAction.New)
            {
                this.AddOperatingCenterDropDownData(x => x.IsActive);
            }
            else
            {
                this.AddOperatingCenterDropDownData();
            }

            // This is hacky but we don't want to query for employees four times. They do not cascade.
            this.AddDynamicDropDownData<IEmployeeRepository, Employee, EmployeeDisplayItem>("Employee");

            ViewData["AssetOwner"] = ViewData["Employee"];
            ViewData["ProjectManager"] = ViewData["Employee"];
            ViewData["ConstructionManager"] = ViewData["Employee"];
            ViewData["CompanyInspector"] = ViewData["Employee"];
            
            // Same hackiness as above
            this.AddDynamicDropDownData<Contractor, ContractorDisplayItem>();
            ViewData["EngineeringContractor"] = ViewData["Contractor"];
            ViewData["ConstructionContractor"] = ViewData["Contractor"];

            this.AddDynamicDropDownData<PublicWaterSupply, PublicWaterSupplyDisplayItem>();

            this.AddDropDownData<InvestmentProjectPhase>("Phase");
            this.AddDropDownData<InvestmentProjectCategory>("ProjectCategory");
            this.AddDropDownData<InvestmentProjectAssetCategory>("AssetCategory");
            this.AddDropDownData<InvestmentProjectApprovalStatus>("ApprovalStatus");
            this.AddDropDownData<InvestmentProjectStatus>("ProjectStatus");

            var years = new List<SelectListItem>();
            const int START_YEAR = 2015; // This is when this feature was requested so this value should always be the minimum.
            var curYear = DateTime.Now.Year;

            foreach (var y in Enumerable.Range(START_YEAR, 11 + (curYear - START_YEAR)))
            {
                years.Add(new SelectListItem {
                    Text = y.ToString(),
                    Value = y.ToString()
                });
            }

            ViewData["CPSReferenceYear"] = years;
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchInvestmentProject>();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchInvestmentProject search)
        {
            return this.RespondTo(f => {
                f.View(() => ActionHelper.DoIndex(search));
                f.Excel(() => ActionHelper.DoExcel(search));
                f.Map(() => _container.GetInstance<IMapResultFactory>().Build(ModelState, Repository, search));
            });
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return this.RespondTo(f => {
                f.View(() => ActionHelper.DoShow(id));
                f.Fragment(() => ActionHelper.DoShow(id, new MMSINC.Utilities.ActionHelperDoShowArgs {
                    ViewName = "_ShowPopup",
                    IsPartial = true 
                }));
            });
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateInvestmentProject(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateInvestmentProject model)
        {
            return ActionHelper.DoCreate(model);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<InvestmentProjectViewModel>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(InvestmentProjectViewModel model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpDelete, RequiresAdmin]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion
    }
}