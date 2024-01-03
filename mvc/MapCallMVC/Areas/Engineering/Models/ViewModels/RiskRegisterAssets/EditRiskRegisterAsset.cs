using MMSINC.Metadata;
using StructureMap;

namespace MapCallMVC.Areas.Engineering.Models.ViewModels.RiskRegisterAssets
{
    public class EditRiskRegisterAsset : RiskRegisterAssetViewModel
    {
        #region Properties

        [DropDown("", "OperatingCenter", "ByStateIdForEngineeringRiskRegisterAssets", DependsOn = nameof(State), PromptText = "Please select a State.")]
        public override int? OperatingCenter
        {
            get => base.OperatingCenter;
            set => base.OperatingCenter = value;
        }

        #endregion

        #region Constructors

        public EditRiskRegisterAsset(IContainer container) : base(container) { }

        #endregion
    }
}