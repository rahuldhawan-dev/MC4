using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallIntranet.Controllers
{
    public class NearMissCategoryController : ControllerBaseWithPersistence<NearMissCategory, User>
    {
        #region Constructor

        public NearMissCategoryController(ControllerBaseWithPersistenceArguments<IRepository<NearMissCategory>, NearMissCategory, User> args) : base(args) { }

        #endregion

        #region GetByTypeId

        [HttpGet]
        public CascadingActionResult GetByTypeId(int typeId)
        {
            return new CascadingActionResult(Repository.Where(x => x.Type.Id == typeId), "Description", "Id");
        }

        #endregion
    }
}