using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.Reports.Controllers
{
    [DisplayName("Services Installed w/Footage")]
    public class ServicesInstalledController : ControllerBaseWithPersistence<IServiceRepository, Service, User>
    {
        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.Search:
                    this.AddOperatingCenterDropDownData();
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(RoleModules.FieldServicesAssets)]
        public ActionResult Search(SearchServicesInstalled search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(RoleModules.FieldServicesAssets)]
        public ActionResult Index(SearchServicesInstalled search)
        {
            return this.RespondTo(f =>
            {
                f.View(() => ActionHelper.DoIndex(search));
                f.Excel(() => {
                    search.EnablePaging = false;
                    var results = Repository.Search(search)
                        .Select(x => new {
                            x.Id, 
                            x.OperatingCenter, 
                            x.ServiceCategory,
                            x.ServiceSize,
                            x.LengthOfService,
                            x.ServiceNumber,
                            x.StreetAddress,
                            x.DateInstalled
                        });
                    return this.Excel(results);
                });
            });
        }

        #endregion

        public ServicesInstalledController(ControllerBaseWithPersistenceArguments<IServiceRepository, Service, User> args) : base(args) {}
    }
}