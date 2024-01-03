using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Models.ViewModels.Streets;
using MMSINC;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Results;

namespace MapCallMVC.Controllers
{
    public class StreetController : ControllerBaseWithPersistence<IStreetRepository, Street, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesDataLookups;

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchStreet search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchStreet search)
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
            return ActionHelper.DoNew(ViewModelFactory.Build<CreateStreet>());
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateStreet model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditStreet>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditStreet model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Delete

        [HttpDelete, RequiresAdmin]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        #region Constructors

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            if (action == ControllerAction.Search)
            {
                this.AddDropDownData<State>(x => x.Id, x => x.Abbreviation);
            }
        }

        public StreetController(ControllerBaseWithPersistenceArguments<IStreetRepository, Street, User> args) : base(args) {}

        #endregion

        #region Ajaxie Actions

        [HttpGet]
        public ActionResult ByTownId(params int[] ids)
        {
            return new CascadingActionResult(Repository.GetByTown(ids), "FullStName", "Id");
        }

        public CascadingActionResult GetActiveByTownId(params int[] ids)
        {
            return new CascadingActionResult(Repository.GetByTown(ids).Where(x => x.IsActive), "FullStName", "Id");
        }

        #region Get by partial street name

        private ActionResult GetByTownIdAndPartialStreetName(string partial, int[] townId, bool onlyIncludeActiveStreets)
        {
            var results = Enumerable.Empty<Street>();
            
            if (!string.IsNullOrWhiteSpace(partial) && partial.Length > 2)
            {
                var resultQueryable = Repository.Where(s =>
                    townId.Contains(s.Town.Id)
                    && s.FullStName.ToLower().Contains(partial.ToLower()));

                if (onlyIncludeActiveStreets)
                {
                    resultQueryable = resultQueryable.Where(x => x.IsActive);
                }
                
                results = resultQueryable.OrderBy(x => x.Name).Take(20).ToList(); 
            }
                
            return new AutoCompleteResult(results, "Id", "FullStName");
        }

        [HttpGet]
        public ActionResult GetActiveByTownIdAndPartialStreetName(string partial, int[] townId)
        {
            return GetByTownIdAndPartialStreetName(partial, townId, true);
        }

        [HttpGet]
        public ActionResult GetByTownIdAndPartialStreetName(string partial, int[] townId)
        {
            return GetByTownIdAndPartialStreetName(partial, townId, false);
        }

        #endregion

        #endregion
    }
}
