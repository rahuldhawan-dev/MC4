using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Facilities.Controllers
{
    public class CommunityRightToKnowController : ControllerBaseWithPersistence<IRepository<CommunityRightToKnow>, CommunityRightToKnow, User>
    {
        #region Constructors

        public CommunityRightToKnowController(ControllerBaseWithPersistenceArguments<IRepository<CommunityRightToKnow>, CommunityRightToKnow, User> args) : base(args) { }

        #endregion

        #region Constants

        public const RoleModules ROLE = RoleModules.ProductionFacilities;

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New(CreateCommunityRightToKnow model)
        {
            return ActionHelper.DoNew(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateCommunityRightToKnow model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchCommunityRightToKnow search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchCommunityRightToKnow search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditCommunityRightToKnow>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditCommunityRightToKnow model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Copy

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Copy(int id)
        {
            var form = Repository.Find(id);

            if (form == null)
            {
                return DoHttpNotFound($"Could not find form with id {id}.");
            }

            var viewModel = ViewModelFactory.Build<CreateCommunityRightToKnow>();

            viewModel.Facility = form.Facility.Id;

            return ActionHelper.DoNew(viewModel);
        }

        #endregion

        #region Delete

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion
    }
}