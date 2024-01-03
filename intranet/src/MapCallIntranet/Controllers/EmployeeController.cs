using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Controllers;
using System.Linq;
using System.Web.Mvc;
using MMSINC;
using MMSINC.Results;

namespace MapCallIntranet.Controllers
{
    public class EmployeeController : ControllerBaseWithPersistence<IEmployeeRepository, Employee, User>
    {
        #region Properties

        private const int PAGE_SIZE = 50;

        #endregion

        #region GetEmployeeBy

        [HttpGet]
        public ActionResult GetEmployeeBy(string employeeIdStartsWith, int operatingCenterId)
        {
            var results = Repository.Where(e => e.EmployeeId.StartsWith(employeeIdStartsWith) && e.OperatingCenter.Id == operatingCenterId).Take(PAGE_SIZE);
            return new AutoCompleteResult(results, "Id", "EmployeeId");
        }

        #endregion

        #region ActiveEmployeesByOperatingCenterId

        [HttpGet]
        public ActionResult ActiveEmployeesByOperatingCenter(int operatingCenterId)
        {
            return new CascadingActionResult<Employee, EmployeeDisplayItem>(Repository.GetActiveEmployeesByOperatingCenterId(operatingCenterId));
        }

        #endregion

        #region Constructors

        public EmployeeController(ControllerBaseWithPersistenceArguments<IEmployeeRepository, Employee, User> args) : base(args) { }

        #endregion
    }
}