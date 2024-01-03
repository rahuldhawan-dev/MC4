using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Models.ViewModels;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallMVC.Controllers
{
    [ActionHelperViewVirtualPathFormat("~/Views/ClassLocation/{0}.cshtml")]
    public class ClassLocationController : EntityLookupControllerBase<IRepository<ClassLocation>, ClassLocation, ClassLocationViewModel>
    {
        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            switch (action)
            {
                case ControllerAction.New:
                case ControllerAction.Edit:
                    this.AddOperatingCenterDropDownData();
                    break;
            }
        }

        #endregion

        public ClassLocationController(ControllerBaseWithPersistenceArguments<IRepository<ClassLocation>, ClassLocation, User> args) : base(args) {}
    }
}