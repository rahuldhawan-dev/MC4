using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using System.Web.Mvc;
using MapCallMVC.Models.ViewModels;
using MMSINC.Utilities;

namespace MapCallMVC.Controllers
{
    public class PublicWaterSupplyLicensedOperatorController : ControllerBaseWithPersistence<IRepository<PublicWaterSupplyLicensedOperator>, PublicWaterSupplyLicensedOperator, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.EnvironmentalGeneral;

        #endregion

        #region Constructors

        public PublicWaterSupplyLicensedOperatorController(ControllerBaseWithPersistenceArguments<IRepository<PublicWaterSupplyLicensedOperator>, PublicWaterSupplyLicensedOperator, User> args) : base(args) { }

        #endregion

        #region New

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreatePublicWaterSupplyLicensedOperator model)
        {
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs {
                OnSuccess = () => RedirectToAction("Show", "OperatorLicense", new { area = "", id = model.LicensedOperator}),
                OnError = () => RedirectToAction("Show", "OperatorLicense", new { area = "", id = model.LicensedOperator})
            });
        }

        #endregion

        #region Delete

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            // We want to redirect back to the PWS page, but we need the id first 
            var LoId = Repository.Find(id)?.LicensedOperator?.Id;
            return ActionHelper.DoDestroy(id, new ActionHelperDoDestroyArgs {
                OnSuccess = () => RedirectToAction("Show", "OperatorLicense", new { area = "", id = LoId }),
                OnError = () => RedirectToAction("Show", "OperatorLicense", new { area = "", id = LoId })
            });
        }

        #endregion

    }
}
