using StructureMap;
using AwiaComplianceEntity = MapCall.Common.Model.Entities.AwiaCompliance;

namespace MapCallMVC.Areas.Engineering.Models.ViewModels.AwiaCompliance
{
    public class CreateAwiaCompliance : AwiaComplianceViewModel
    {
        #region Constructors

        public CreateAwiaCompliance(IContainer container) : base(container) { }

        #endregion
    }
}