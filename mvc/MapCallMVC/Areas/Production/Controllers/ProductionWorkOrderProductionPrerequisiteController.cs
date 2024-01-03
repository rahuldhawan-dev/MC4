using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Production.Models.ViewModels;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Production.Controllers
{
    public class ProductionWorkOrderProductionPrerequisiteController : ControllerBaseWithPersistence<IRepository<ProductionWorkOrderProductionPrerequisite>, ProductionWorkOrderProductionPrerequisite, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ProductionWorkManagement;

        #endregion

        #region Exposed Methods

        [HttpGet, RequiresRole(ROLE,RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditProductionWorkOrderProductionPrerequisite>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditProductionWorkOrderProductionPrerequisite model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => RedirectToAction("Show", "ProductionWorkOrder", new { area = "Production", id = model.ProductionWorkOrder.Id })
            });
        }

        #endregion

        #region Constructors

        public ProductionWorkOrderProductionPrerequisiteController(ControllerBaseWithPersistenceArguments<IRepository<ProductionWorkOrderProductionPrerequisite>, ProductionWorkOrderProductionPrerequisite, User> args) : base(args) { }   

        #endregion
    }
}
