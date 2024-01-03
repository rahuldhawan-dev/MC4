using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Helpers;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Helpers;
using MMSINC.Utilities;
using StructureMap.Query;

namespace MapCallMVC.Areas.HealthAndSafety.Controllers
{
    public class ConfinedSpaceFormController : ControllerBaseWithPersistence<ConfinedSpaceForm, User>
    {
        #region Constants

        // TODO: I feel like this role should be renamed to OperationsForms maybe?
        public const RoleModules ROLE = RoleModules.OperationsLockoutForms;
        public const string FORM_LOCKED = "This permit has been cancelled and can no longer be edited. Please use the Notes tab for adding any additional information.";
        public const string TEST_OUT_OF_RANGE = "A valid pre-entry atmospheric test has not been added yet.";
        public const string ENTRY_MAY_COMMENCE_WITH_PERMIT = "Permit Authorized – Entry may commence";
        public const string ENTRY_MAY_COMMENCE_WITHOUT_PERMIT = "No Permit Required – Entry may commence";
        public const string FORM_LOCKED_COMPLETION = "The form has been completed and sections 1,2,3,4,5 can no longer be edited. If applicable, Employee Assignments can still be added";

        #endregion

        #region Constructors

        public ConfinedSpaceFormController(ControllerBaseWithPersistenceArguments<IRepository<ConfinedSpaceForm>, ConfinedSpaceForm, User> args) : base(args) { }

        #endregion

        #region Private Methods

        private void AddNoteForCancellation(ConfinedSpaceFormViewModel model)
        {
            if (!String.IsNullOrWhiteSpace(model.PermitCancellationNote) && model.IsPermitCancelledSectionSigned.Value)
            {
                _container.GetInstance<IRepository<Note>>().Save(new Note {
                    Text = model.PermitCancellationNote,
                    LinkedId = model.Id,
                    DataType = _container.GetInstance<IDataTypeRepository>()
                                         .GetByTableName(nameof(ConfinedSpaceForm) + "s").First(),
                    CreatedBy = AuthenticationService.CurrentUser.UserName
                });
            }
        }

        private void AddNoteForCancellationPostCompletion(PostCompletionConfinedSpaceForm model)
        {
            if (!String.IsNullOrWhiteSpace(model.PermitCancellationNote) && model.IsPermitCancelledSectionSigned.Value)
            {
                _container.GetInstance<IRepository<Note>>().Save(new Note {
                    Text = model.PermitCancellationNote,
                    LinkedId = model.Id,
                    DataType = _container.GetInstance<IDataTypeRepository>()
                                         .GetByTableName(nameof(ConfinedSpaceForm) + "s").First(),
                    CreatedBy = AuthenticationService.CurrentUser.UserName
                });
            }
        }

        private void ShowNotifications(ConfinedSpaceForm entity)
        {
            if (!entity.HasAtLeastOneValidPreEntryAtmosphericTest)
            {
                DisplayNotification(TEST_OUT_OF_RANGE);
            }

            if (entity.IsPermitCancelledSectionSigned)
            {
                DisplayNotification(FORM_LOCKED);
            }

            if (entity.IsCompleted)
            {
                DisplayNotification(FORM_LOCKED_COMPLETION);
                DisplayNotification(entity.IsSection5Completed
                    ? ENTRY_MAY_COMMENCE_WITH_PERMIT
                    : ENTRY_MAY_COMMENCE_WITHOUT_PERMIT);
            }
        }
        
        private void AddConfinedSpaceFormReadingCaptureTimes(bool? isCompleted)
        {
            if (isCompleted == true)
            {
                this.AddDropDownData<ConfinedSpaceFormReadingCaptureTime>("NewAtmosphericTests.ConfinedSpaceFormReadingCaptureTime");
            }
            else
            {
                this.AddDropDownData<ConfinedSpaceFormReadingCaptureTime>("NewAtmosphericTests.ConfinedSpaceFormReadingCaptureTime",
                    x => x.Where(ct => ct.Id == ConfinedSpaceFormReadingCaptureTime.Indices.PRE_ENTRY),
                    x => x.Id,
                    x => x.Description);
            }
        }

        #endregion

        #region Public Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            switch (action)
            {
                case ControllerAction.New:
                    AddConfinedSpaceFormReadingCaptureTimes(false);
                    this.AddDynamicDropDownData<GasMonitor, GasMonitorDisplayItem>(filter: x => x.Equipment.EquipmentStatus.Id != EquipmentStatus.Indices.RETIRED && x.Equipment.EquipmentStatus.Id != EquipmentStatus.Indices.CANCELLED);
                    break;
                case ControllerAction.Edit:
                    this.AddDynamicDropDownData<GasMonitor, GasMonitorDisplayItem>(filter: x => x.Equipment.EquipmentStatus.Id != EquipmentStatus.Indices.RETIRED && x.Equipment.EquipmentStatus.Id != EquipmentStatus.Indices.CANCELLED);
                    break;
            }

            this.AddDropDownData<ConfinedSpaceFormEntrantType>("NewEntrants.EntrantType");
        }

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchConfinedSpaceForm search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchConfinedSpaceForm search)
        {
            return ActionHelper.DoIndex(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id, new ActionHelperDoShowArgs<ConfinedSpaceForm> {
                OnSuccess = entity => {
                    ShowNotifications(entity);
                    // MC-2511: Needed for displaying in the view. They want to see all the hazard types
                    // even if they didn't select them for this particular CSF record.
                    ViewData["HazardTypes"] =
                        _container.GetInstance<IRepository<ConfinedSpaceFormHazardType>>().GetAll();
                    return this.RespondTo(x => {
                        x.View(() => ActionHelper.DoShow(id));
                        x.Pdf(() => {
                            // Needed for displaying all entrant types even if there aren't
                            // entrants entered for the specific type.
                            ViewData["EntrantTypes"] =
                                _container.GetInstance<IRepository<ConfinedSpaceFormEntrantType>>().GetAll();
                            return ActionHelper.DoPdf(id);
                        });
                    });
                }
            });
        }

        #endregion

        #region New/Create

        [ActionBarVisible(false)]
        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New(int? productionWorkOrderId = null, int? shortCycleWorkOrderNumber = null, int? workOrderId = null)
        {
            // NOTE: The short cycle parameter is used externally wherever FSRs are using this. The other two parameters are 
            // coming from views/links in MapCall. Also apparently they send us the SCWO number, not the id.
            if (productionWorkOrderId == null && shortCycleWorkOrderNumber == null && workOrderId == null)
            {
                return HttpNotFound();
            }
            var vm = ViewModelFactory.Build<CreateConfinedSpaceForm>();
            vm.ProductionWorkOrder = productionWorkOrderId;
            vm.ShortCycleWorkOrderNumber = shortCycleWorkOrderNumber;
            vm.WorkOrder = workOrderId;
            return ActionHelper.DoNew(vm);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateConfinedSpaceForm model)
        {
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    // Because of the way saving is required to enable a few sections,
                    // we're redirecting back to Edit to make users lives easier. Lori requested this
                    // for MC-2564. Merged conflict, adding note logic
                    AddNoteForCancellation(model);
                    return RedirectToAction("Edit", new {id = model.Id});
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            // TODO: ActionHelper.DoEdit needs proper override support for when
            // it finds the model but we need to redirect for reasons.
            var entity = Repository.Find(id);
            if (entity?.IsPermitCancelledSectionSigned == true)
            {
                return RedirectToAction("Show", new { id = id });
            }

            if (entity?.IsCompleted == true)
            {
                return RedirectToAction("PostCompletionEdit", new {id = id});
            }
            AddConfinedSpaceFormReadingCaptureTimes(entity?.IsCompleted);

            return ActionHelper.DoEdit<EditConfinedSpaceForm>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditConfinedSpaceForm model)
        {
            var modelHazards = model.Hazards;
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    // Because of the way saving is required to enable a few sections,
                    // we're redirecting back to Edit to make users lives easier. Lori requested this
                    // for MC-2564.
                    AddNoteForCancellation(model);
                    return RedirectToAction("Edit", new {id = model.Id});
                },
                // This is needed for the bug with server side validation, we lose the mapping for the hazards if theres any server side validation errors
                OnError = () => {
                    model.SetDefaults();
                    foreach (var hazard in modelHazards.Where(x => x.IsChecked == true))
                    {
                        var oldHazard = model.Hazards.Single(x => x.HazardType == hazard.HazardType);
                        oldHazard.IsChecked = hazard.IsChecked;
                        oldHazard.Notes = hazard.Notes;
                    }
                    return null;
                }
            });
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        #region PostCompletion

        [Crumb(Action = "Edit")]
        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult PostCompletionEdit(int id)
        {
            var entity = _repository.Find(id);
            ShowNotifications(entity);
            ViewData["HazardTypes"] = _container.GetInstance<IRepository<ConfinedSpaceFormHazardType>>().GetAll();
            AddConfinedSpaceFormReadingCaptureTimes(entity?.IsCompleted);
            return ActionHelper.DoEdit(id, new ActionHelperDoEditArgs<ConfinedSpaceForm, PostCompletionConfinedSpaceForm> {
                ViewName = "EditPostCompletion"
            });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult PostCompletionUpdate(PostCompletionConfinedSpaceForm model)
        {
            var entity = _repository.Find(model.Id);

            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    AddNoteForCancellationPostCompletion(model);
                    return RedirectToAction("Show", new {id = model.Id});
                }, 
                OnErrorView = "EditPostCompletion"
            });
        }

        #endregion

        #endregion
    }
}
