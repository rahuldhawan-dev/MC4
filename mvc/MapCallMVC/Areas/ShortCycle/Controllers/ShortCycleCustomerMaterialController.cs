using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.ShortCycle.Models.ViewModels.ShortCycleCustomerMaterials;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.ShortCycle.Controllers
{
    public class ShortCycleCustomerMaterialController
        : ControllerBaseWithPersistence<
            IRepository<ShortCycleCustomerMaterial>,
            ShortCycleCustomerMaterial,
            User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesShortCycle;

        #endregion

        public ShortCycleCustomerMaterialController(
            ControllerBaseWithPersistenceArguments<
                IRepository<ShortCycleCustomerMaterial>,
                ShortCycleCustomerMaterial,
                User> args)
            : base(args) { }

        [RequiresRole(ROLE)]
        [HttpGet]
        public ActionResult Index(SearchShortCycleCustomerMaterial search)
        {
            return this.RespondTo(
                f => f.Fragment(
                    () => {
                        search.EnablePaging = false;
                        return ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                            IsPartial = true,
                            RedirectSingleItemToShowView = false,
                            OnNoResults = () => DoView("Index", search, true)
                        });
                    }));
        }
    }
}
