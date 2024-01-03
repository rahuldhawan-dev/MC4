using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Models.ViewModels;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Controllers
{
    public class EquipmentGroupController : ControllerBaseWithPersistence<EquipmentGroup, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ProductionEquipment;

        #endregion

        #region Index

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchEquipmentGroup search)
        {
            search.EnablePaging = false;
            return ActionHelper.DoIndex(search);
        }

        #endregion
        
        public EquipmentGroupController(ControllerBaseWithPersistenceArguments<IRepository<EquipmentGroup>, EquipmentGroup, User> args) : base(args) { }
    }
}