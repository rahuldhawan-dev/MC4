using MMSINC.Data;
using MMSINC.Metadata;
using MapCall.Common.Model.Entities;

namespace MapCallMVC.Models.ViewModels
{
    public class SearchStormWaterAsset : SearchSet<StormWaterAsset>
    {
        #region Properties

        [DropDown]
        public int? OperatingCenter { get; set; }

        #endregion
    }
}