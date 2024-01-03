using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallIntranet.Controllers
{
    [AllowAnonymous]
    public class MapIconController : ControllerBaseWithPersistence<MapIcon, User>
    {
        #region Properties

        public IIconSetRepository IconSetRepository
        {
            get { return _container.GetInstance<IIconSetRepository>(); }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet]
        public ActionResult Index(IconSets iconSet = IconSets.All)
        {
            var ret = IconSetRepository.Find(iconSet);
            return this.RespondTo(f => f.Fragment(() =>
                View("_Index", ret)));
        }

        #endregion

        #region Constructor

        public MapIconController(ControllerBaseWithPersistenceArguments<IRepository<MapIcon>, MapIcon, User> args) : base(args) { }

        #endregion
    }
}
