using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using System.Web.Mvc;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Controllers
{
    public class GISLayerUpdateController : ControllerBaseWithPersistence<GISLayerUpdate, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.OperationsManagement;

        #endregion
        
        #region Show	

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        #endregion
        
        #region Index

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index()
        {
            return ActionHelper.DoIndex();
        }

        #endregion
    
        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateGISLayerUpdate(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateGISLayerUpdate model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditGISLayerUpdate>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditGISLayerUpdate model)
        {
            return ActionHelper.DoUpdate(model);
        }
		
        #endregion

        public GISLayerUpdateController(ControllerBaseWithPersistenceArguments<IRepository<GISLayerUpdate>, GISLayerUpdate, User> args) : base(args) {}

    }
}