using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Controllers;
using MMSINC.Results;

namespace MapCallMVC.Areas.Production.Controllers
{
    public class ScadaTagNameController : ControllerBaseWithPersistence<IScadaTagNameRepository,ScadaTagName, User>
    {
        public ScadaTagNameController(ControllerBaseWithPersistenceArguments<IScadaTagNameRepository, ScadaTagName, User> args) : base(args) { }

        #region ByPartialMatchName

        [HttpGet]
        public ActionResult ByPartialMatchName(string partialName)
        {
            var results = Repository.FindByPartialNameMatch(partialName);
            return new AutoCompleteResult(results, "Id", "Display");
        }

        #endregion
    }
}
