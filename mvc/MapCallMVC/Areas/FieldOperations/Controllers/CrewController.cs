using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Metadata;
using MMSINC.Utilities;
using System.Linq;
using System.Web.Mvc;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class CrewController : ControllerBaseWithPersistence<ICrewRepository, Crew, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;
        public const string CREW_NOT_FOUND = "Crew not found.";

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchCrew search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchCrew search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => {
                    return ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                        SearchOverrideCallback = () => Repository.Search(search)
                    });
                });
                formatter.Excel(() => {
                    search.EnablePaging = false;
                    var results = Repository.Search(search);
                    return this.Excel(results.Select(x => new {
                        CrewName = x.Description,
                        Availability_Hours = x.Availability,
                        x.OperatingCenter,
                        x.Active
                    }));
                });
            });
        }

        [HttpGet, NoCache, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult New(CreateCrew model)
        {
            ModelState.Clear();
            return ActionHelper.DoNew(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Create(CreateCrew model)
        {
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs {
                OnSuccess = () => RedirectToAction("Show", new { id = model.Id })
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit(id, new ActionHelperDoEditArgs<Crew, EditCrew> {
                IsPartial = false,
                NotFound = CREW_NOT_FOUND
            });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Update(EditCrew model)
        {
            var args = new ActionHelperDoUpdateArgs {
                OnSuccess = () => RedirectToAction("Show", new { id = model.Id }),
                OnError = () => RedirectToAction("Edit", new { id = model.Id }),
                OnNotFound = () => HttpNotFound(CREW_NOT_FOUND)
            };

            return ActionHelper.DoUpdate(model, args);
        }

        #endregion

        #region Constructors

        public CrewController(ControllerBaseWithPersistenceArguments<ICrewRepository, Crew, User> args) : base(args) { }

        #endregion

        [HttpGet]
        public ActionResult ByOperatingCenterOrAll(int? opc)
        {
            IQueryable<Crew> data = null;
            if (opc.HasValue)
            {
                data = Repository.GetAll().Where(x => x.OperatingCenter.Id == opc.Value && x.Active);
            }
            else
            {
                data = Repository.GetAllSorted(x => x.Id).Where(x => x.Active);
            }
            return new CascadingActionResult(data, "Description", "Id") {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult ShowAssignedWorkOrders(SearchCrewForWorkOrders search)
        {
            if (search.Id != null)
                search.Crew = Repository.Load(search.Id.Value);

            if (search.Crew == null)
                return HttpNotFound(CREW_NOT_FOUND);

            return this.RespondTo(f => {
                f.Fragment(() => ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                    ViewName = "_showWorkOrders",
                    IsPartial = true,
                    RedirectSingleItemToShowView = false,
                    OnNoResults = () => DoView("_showWorkOrders", search, true)
                }));
            });
        }

        #region ByOperatingCenterId

        [HttpGet]
        public ActionResult ByOperatingCenterId(int operatingCenterId)
        {
            return new CascadingActionResult(Repository.Where(x => x.OperatingCenter.Id == operatingCenterId));
        }

        #endregion
    }
}