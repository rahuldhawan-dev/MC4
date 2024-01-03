using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data;
using System.Web.Mvc;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.ProjectManagement.Controllers
{
    public class PipeDataLookupValueController : ControllerBaseWithPersistence<PipeDataLookupValue, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesProjects;

        #endregion

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            if (action != ControllerAction.Show)
            {
                this.AddDropDownData<PipeDataLookupType>();
            }
        }

        #region Show	

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        #endregion

        #region Index

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchPipeDataLookupValue>();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchPipeDataLookupValue search)
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
            return ActionHelper.DoNew(new CreatePipeDataLookupValue(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreatePipeDataLookupValue model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditPipeDataLookupValue>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditPipeDataLookupValue model)
        {
            return ActionHelper.DoUpdate(model);
        }
		
        #endregion

		#region Constructors

        public PipeDataLookupValueController(ControllerBaseWithPersistenceArguments<IRepository<PipeDataLookupValue>, PipeDataLookupValue, User> args) : base(args) {}

		#endregion
    }
}