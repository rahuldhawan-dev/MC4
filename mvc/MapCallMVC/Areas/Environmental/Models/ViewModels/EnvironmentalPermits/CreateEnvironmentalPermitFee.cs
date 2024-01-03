using System.Linq;
using StructureMap;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalPermits
{
    public class CreateEnvironmentalPermitFee : EnvironmentalPermitFeeViewModel
    {
        #region Constructor

        public CreateEnvironmentalPermitFee(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override void SetDefaults()
        {
            base.SetDefaults();
            // This is done in SetDefaults for Create because Map won't be called.
            OperatingCenter = GetEnvironmentalPermit().OperatingCenters.Select(x => x.Id).ToArray();
        }

        #endregion
    }
}