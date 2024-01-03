using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Utilities;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels.WellTests
{
    public class CreateWellTestViewModel : WellTestViewModel
    {
        #region Constructors

        public CreateWellTestViewModel(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override void SetDefaults()
        {
            base.SetDefaults();
            DateOfTest = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            Employee = _container.GetInstance<IAuthenticationService<User>>().CurrentUser.Employee?.Id;
        }

        #endregion
    }
}
