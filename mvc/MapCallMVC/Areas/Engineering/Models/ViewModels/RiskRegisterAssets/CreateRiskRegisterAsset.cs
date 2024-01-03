using MMSINC.Metadata;
using StructureMap;

namespace MapCallMVC.Areas.Engineering.Models.ViewModels.RiskRegisterAssets
{
    public class CreateRiskRegisterAsset : RiskRegisterAssetViewModel
    {
        #region Properties
        
        [DropDown("", "OperatingCenter", "ActiveByStateIdForEngineeringRiskRegisterAssets", DependsOn = nameof(State), PromptText = "Please select a State.")]
        public override int? OperatingCenter
        {
            get => base.OperatingCenter;
            set => base.OperatingCenter = value;
        }

        #endregion

        #region Constructors

        public CreateRiskRegisterAsset(IContainer container) : base(container) { }
        
        #endregion
    }
}
