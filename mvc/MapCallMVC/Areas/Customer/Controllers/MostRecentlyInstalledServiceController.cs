using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Customer.Controllers
{
    public class MostRecentlyInstalledServiceController
        : ControllerBaseWithPersistence<MostRecentlyInstalledService, User>
    {
        public MostRecentlyInstalledServiceController(
            ControllerBaseWithPersistenceArguments<
                IRepository<MostRecentlyInstalledService>, MostRecentlyInstalledService, User> args)
            : base(args) { }

        /// <summary>
        /// Attempt to locate the <see cref="MostRecentlyInstalledService"/> for a <see cref="Premise"/>
        /// with the supplied <see cref="Premise.Installation"/> and <see cref="OperatingCenter"/>.Id.
        /// If more or less than one match are found, the result data will be null.
        /// </summary>
        [HttpGet]
        public ActionResult ByInstallationNumberAndOperatingCenter(
            string installation,
            int operatingCenterId)
        {
            var query = Repository.ByInstallationNumberAndOperatingCenter(installation, operatingCenterId);

            return Json(
                new {
                    Data = query.Count() == 1
                        ? query.Select(s => MostRecentlyInstalledService.ToJSONObject(s)).Single()
                        : null
                },
                JsonRequestBehavior.AllowGet);
        }
    }
}
