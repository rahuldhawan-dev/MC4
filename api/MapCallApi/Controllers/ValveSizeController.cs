using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallApi.Models;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallApi.Controllers
{
    public class ValveSizeController : ControllerBaseWithPersistence<ValveSize, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;

        #endregion
        
        #region Constructors

        public ValveSizeController(ControllerBaseWithPersistenceArguments<IRepository<ValveSize>, ValveSize, User> args) : base(args) { }

        #endregion

        #region Public Methods

        [HttpGet, RequiresRole(ROLE)]
        [OutputCache(Duration = 300)]
        public ActionResult Index(SearchValveSize searchSet)
        {
            searchSet.EnablePaging = false;
            
            return Json(_repository.Search(searchSet), JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
