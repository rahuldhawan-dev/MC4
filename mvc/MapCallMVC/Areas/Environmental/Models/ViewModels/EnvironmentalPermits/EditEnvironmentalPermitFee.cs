using System.Linq;
using MapCall.Common.Model.Entities;
using StructureMap;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalPermits
{
    public class EditEnvironmentalPermitFee : EnvironmentalPermitFeeViewModel
    {
        #region Constructor

        public EditEnvironmentalPermitFee(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override void Map(EnvironmentalPermitFee entity)
        {
            base.Map(entity);
            OperatingCenter = entity.EnvironmentalPermit.OperatingCenters.Select(x => x.Id).ToArray();
        }

        #endregion
    }
}