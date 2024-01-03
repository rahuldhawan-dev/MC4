using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Utilities;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels.PreJobSafetyBriefs
{
    public abstract class CreateProductionPreJobSafetyBriefBase : ProductionPreJobSafetyBriefViewModelBase
    {
        #region Constructor
        
        public CreateProductionPreJobSafetyBriefBase(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override void SetDefaults()
        {
            base.SetDefaults();
            SafetyBriefDateTime = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
        }

        #endregion
    }
}