using System.Web.Mvc;
using Contractors.Data.DesignPatterns.Mvc;
using Contractors.Data.Models.Repositories;
using MapCall.Common.Model.Entities;
using MMSINC;
using MMSINC.Controllers;

namespace Contractors.Controllers
{
    public class CountyController : ControllerBaseWithValidation<ICountyRepository, County>
    {
        #region Constructors

        public CountyController(ControllerBaseWithPersistenceArguments<ICountyRepository, County, ContractorUser> args) : base(args) { }

        #endregion

        #region ByStateId

        [HttpGet]
        public ActionResult ByStateId(int stateId)
        {
            return new CascadingActionResult(Repository.GetByStateId(stateId), "Name", "Id");
        }

        #endregion
    }
}
