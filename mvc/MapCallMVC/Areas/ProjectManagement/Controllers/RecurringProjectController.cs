using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Configuration;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;

namespace MapCallMVC.Areas.ProjectManagement.Controllers
{
    [DisplayName("RP-Mains\\Valves")]
    public class RecurringProjectController : ControllerBaseWithPersistence<RecurringProject, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesProjects;
        public const string CREATED_PURPOSE = "ProjectsRP New Record", COMPLETED_PURPOSE= "ProjectsRP Completed Record", GIS_INCORRECT_PURPOSE = "ProjectsRP GIS Data Incorrect";

        #endregion

        #region Private Methods
        
        private void SendNotification(RecurringProject project, string purpose, RecurringProjectViewModel viewModel, bool withOperatingCenter = true)
        {
            var notifier = _container.GetInstance<INotificationService>();
            dynamic model = project;
            if (purpose == GIS_INCORRECT_PURPOSE)
            {
                model = ViewModelFactory.BuildWithOverrides<RecurringProjectForGISNotification, RecurringProject>(project, new {
                    ModifiedBy = AuthenticationService.CurrentUser.FullName,
                    ModifiedByEmail = AuthenticationService.CurrentUser.Email,
                    RecordUrl = GetUrlForModel(model, "Show", "RecurringProject", "ProjectManagement")
                });
            }
            var args = new NotifierArgs {
                OperatingCenterId = (withOperatingCenter) ? project.OperatingCenter.Id : 0,
                Module = ROLE,
                Purpose = purpose,
                Data = model
            };
            if (viewModel.FileUpload != null && viewModel.FileUpload.HasBinaryData && purpose == GIS_INCORRECT_PURPOSE)
            {
                args.AddAttachment(viewModel.FileUpload.FileName, viewModel.FileUpload.BinaryData);
            }

            notifier.Notify(args);
        }

        /// <summary>
        /// This will add a limited set of values to the Recurring Status drop down 
        /// based on the roles a user belongs to. 
        /// </summary>
        /// <param name="id">Id of the entity being updated. The user needs to have the option
        /// to leave the value alone so it needs to be there.</param>
        private void AddRecurringProjectStatusesForUser(int id)
        {
            if (_container.GetInstance<IAuthenticationService>().CurrentUserIsAdmin)
            {
                this.AddDropDownData<RecurringProjectStatus>("Status");
                return;
            }

            var roleService = _container.GetInstance<IRoleService>();
            var items = new List<int>();
            if (roleService.CanAccessRole(ROLE))
            {
                items.Add(RecurringProjectStatus.Indices.SUBMITTED);
                items.Add(RecurringProjectStatus.Indices.CANCELED);
                items.Add(RecurringProjectStatus.Indices.PROPOSED);
            }

            if (roleService.CanAccessRole(RoleModules.FieldServicesLocalApproval))
            {
                items.Add(RecurringProjectStatus.Indices.MANAGER_ENDORSED);
                items.Add(RecurringProjectStatus.Indices.REVIEWED);
            }

            if (roleService.CanAccessRole(RoleModules.FieldServicesAssetPlanningEndorsement)) 
                items.Add(RecurringProjectStatus.Indices.AP_ENDORSED);

            if (roleService.CanAccessRole(RoleModules.FieldServicesAssetPlanningApproval))
                items.Add(RecurringProjectStatus.Indices.AP_APPROVED);

            if (roleService.CanAccessRole(RoleModules.FieldServicesCapitalPlanning))
                items.Add(RecurringProjectStatus.Indices.MUNICIPAL_RELOCATION_APPROVED);

            // users need to be able to also set it to the existing status
            var entity = _container.GetInstance<IRepository<RecurringProject>>().Find(id);
            if (entity != null && !items.Contains(entity.Status.Id))
                items.Add(entity.Status.Id);

            this.AddDropDownData("Status", _container.GetInstance<IRepository<RecurringProjectStatus>>().GetAllSorted(x => x.Description).Where(x => items.Contains(x.Id)), x=> x.Id, x => x.Description);
        }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            this.AddDropDownData<EndorsementStatus>();

            switch (action)
            {
                case ControllerAction.Search:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE);
                    this.AddDropDownData<RecurringProjectStatus>("Status"); // everyone can search for these
                    break;
                case ControllerAction.Edit:
                    this.AddDropDownData<HighCostFactor>("HighCostFactors");
                    this.AddDropDownData<GISDataInaccuracyType>("GISDataInaccuracies");
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Edit);
                    this.AddDropDownData<EndorsementStatus>();
                    this.AddDropDownData<AssetCategory>();
                    this.AddDropDownData<AssetType>();
                    this.AddDropDownData<RecurringProjectType>();
                    this.AddDropDownData<OverrideInfoMasterReason>();
                    this.AddDropDownData<PipeDiameter>("ProposedDiameter", d => d.GetAllSorted(x => x.Diameter), d => d.Id, d => d.Diameter);
                    this.AddDropDownData<PipeMaterial>("ProposedPipeMaterial");
                    this.AddDropDownData<AssetInvestmentCategory>("AcceleratedAssetInvestmentCategory");
                    this.AddDropDownData<AssetInvestmentCategory>("SecondaryAssetInvestmentCategory");
                    this.AddDropDownData<FoundationalFilingPeriod>();
                    this.AddDropDownData<RecurringProjectRegulatoryStatus>("RegulatoryStatus");
                    //AddRecurringProjectStatusesForUser(); // calling this directly in the edit action because we need the id
                    break;
                case ControllerAction.New:
                    this.AddDropDownData<HighCostFactor>("HighCostFactors");
                    this.AddDropDownData<GISDataInaccuracyType>("GISDataInaccuracies");
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Add, extraFilterP: x => x.IsActive);
                    this.AddDropDownData<EndorsementStatus>();
                    this.AddDropDownData<AssetCategory>();
                    this.AddDropDownData<AssetType>();
                    this.AddDropDownData<RecurringProjectType>();
                    this.AddDropDownData<OverrideInfoMasterReason>();
                    this.AddDropDownData<PipeDiameter>("ProposedDiameter", d => d.GetAllSorted(x => x.Diameter), d => d.Id, d => d.Diameter);
                    this.AddDropDownData<PipeMaterial>("ProposedPipeMaterial");
                    this.AddDropDownData<AssetInvestmentCategory>("AcceleratedAssetInvestmentCategory");
                    this.AddDropDownData<AssetInvestmentCategory>("SecondaryAssetInvestmentCategory");
                    this.AddDropDownData<FoundationalFilingPeriod>();
                    this.AddDropDownData<RecurringProjectRegulatoryStatus>("RegulatoryStatus");
                    // AddProjectStatusesForUser(); // All of them will be created with Proposed -- set by default?
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchRecurringProject search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return this.RespondTo(formatter => {
                formatter.View(() => ActionHelper.DoShow(id, onModelFound: rp => {
                    if (rp.RegulatoryStatus?.Id != RecurringProjectRegulatoryStatus.Indices.BPU_APPROVED)
                    {
                        DisplayErrorMessage("This project requires BPU notification before proceeding, notify your PM.");
                    }
                }));
                formatter.Fragment(() => ActionHelper.DoShow(id, new MMSINC.Utilities.ActionHelperDoShowArgs {
                    IsPartial = true,
                    ViewName = "_ShowPopup"
                }));
            });
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchRecurringProject search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
                formatter.Map(() => _container.GetInstance<IMapResultFactory>().Build(ModelState, Repository, search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateRecurringProject(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateRecurringProject model)
        {
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs
            {
                OnSuccess = () => {
                    var entity = Repository.Find(model.Id);
                    SendNotification(entity, CREATED_PURPOSE, model);
                    if (model.SendGISDataIncorrectOnSave)
                    {
                        SendNotification(entity, GIS_INCORRECT_PURPOSE, model, false);
                    }
                    return null; // defer to default
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditRecurringProject>(id, null, onModelFound: (entity) => {
                // TODO: This should be done via some sorta OnSuccess arg instead. It doesn't rely on the entity.
                AddRecurringProjectStatusesForUser(id);
            });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditRecurringProject model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs
            {
                OnSuccess = () => {
                    if (model.SendNotificationOnSave)
                    {
                        SendNotification(Repository.Find(model.Id), COMPLETED_PURPOSE, model);
                    }
                    if (model.SendGISDataIncorrectOnSave)
                    {
                        SendNotification(Repository.Find(model.Id), GIS_INCORRECT_PURPOSE, model, false);
                    }
                    return null;
                }
            });
        }
		
        #endregion

        #region Delete

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        #region AddEndorsement

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit), RequiresSecureForm]
        public ActionResult AddRecurringProjectEndorsement(AddRecurringProjectEndorsement model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Remove Endorsement

        [HttpDelete, RequiresRole(ROLE, RoleActions.Edit), RequiresSecureForm()]
        public ActionResult RemoveRecurringProjectEndorsement(RemoveRecurringProjectEndorsement model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Constructors

        public RecurringProjectController(ControllerBaseWithPersistenceArguments<IRepository<RecurringProject>, RecurringProject, User> args) : base(args) {}

		#endregion
    }
}