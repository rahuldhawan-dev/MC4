using MapCall.Common.Model.Entities;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class EditAssetConditionReason : AssetConditionReasonViewModel
    {
        public EditAssetConditionReason(IContainer container) : base(container) { }
    }
}
