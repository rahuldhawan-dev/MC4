using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;

namespace MapCallMVC.Models.ViewModels
{
    public class AssetMapView : MapView
    {
        #region Properties

        //[Required]
        public int[] OperatingCenter { get; set; }

        #endregion
    }

    public class AssetCoordinateSearch : SearchSet<AssetCoordinate>, IAssetCoordinateSearch
    {
        #region Properties
        
        /// <remarks>
        /// Returns false and is not settable, because map coordinates shouldn't be paged.
        /// </remarks>
        public override bool EnablePaging
        {
            get => false;
            set { }
        }

        [Required]
        public int[] OperatingCenter { get; set; }

        [Required]
        public decimal? LatitudeMin { get; set; }

        [Required]
        public decimal? LatitudeMax { get; set; }

        [Required]
        public decimal? LongitudeMin { get; set; }

        [Required]
        public decimal? LongitudeMax { get; set; }
        
        #endregion
    }
}