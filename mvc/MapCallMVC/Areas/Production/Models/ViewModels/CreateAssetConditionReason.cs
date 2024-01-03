using MapCall.Common.Model.Entities;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class CreateAssetConditionReason : AssetConditionReasonViewModel
    {
        public CreateAssetConditionReason(IContainer container) : base(container) { }
    }
}
