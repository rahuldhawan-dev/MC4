using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Production.Models.ViewModels;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;

namespace MapCallMVC.Areas.Production.Controllers
{
    public class ProductionWorkOrderMeasurementPointValueController : ControllerBaseWithPersistence<IRepository<ProductionWorkOrderMeasurementPointValue>, ProductionWorkOrderMeasurementPointValue, User>
    {
        public const RoleModules ROLE = RoleModules.ProductionWorkManagement;

        public ProductionWorkOrderMeasurementPointValueController(ControllerBaseWithPersistenceArguments<IRepository<ProductionWorkOrderMeasurementPointValue>, ProductionWorkOrderMeasurementPointValue, User> args) : base(args) { }

        [HttpPost, RequiresSecureForm, RequiresRole(ROLE)]
        public ActionResult Create(CreateProductionWorkOrderMeasurementPointValue model)
        {
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs {
                OnSuccess = () => RedirectToReferrerOr("Show", "ProductionWorkOrder", new { area = "Production", id = model.ProductionWorkOrder }, string.Empty)
            });
        }

        [HttpPost, RequiresSecureForm, RequiresRole(ROLE)]
        public ActionResult Update(EditProductionWorkOrderMeasurementPointValue model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => RedirectToReferrerOr("Show", "ProductionWorkOrder", new { area = "Production", id = model.ProductionWorkOrder }, string.Empty)
            });
        }
    }
}
