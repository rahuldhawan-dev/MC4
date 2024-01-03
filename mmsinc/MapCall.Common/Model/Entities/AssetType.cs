using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;
using Newtonsoft.Json;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class AssetType : EntityLookup
    {
        #region Constants

        public struct Indices
        {
            public const int VALVE = 1,
                             HYDRANT = 2,
                             MAIN = 3,
                             SERVICE = 4,
                             SEWER_OPENING = 5,
                             SEWER_LATERAL = 6,
                             SEWER_MAIN = 7,
                             STORM_CATCH = 8,
                             EQUIPMENT = 9,
                             FACILITY = 11,
                             MAIN_CROSSING = 12;
        }

        protected int[] WithAssets = {
            Indices.VALVE,
            Indices.HYDRANT,
            Indices.SEWER_OPENING,
            Indices.EQUIPMENT,
            Indices.FACILITY,
            Indices.MAIN_CROSSING,
            Indices.STORM_CATCH
        };

        public struct StringLengths
        {
            public const int ONE_MAP_FEATURE_SOURCE = 255;
        }

        #endregion

        #region Properties

        public virtual IList<OperatingCenterAssetType> OperatingCenterAssetTypes { get; set; }
        public virtual bool IncludesCoordinate => WithAssets.Contains(Id);
        public virtual AssetTypeEnum AssetTypeEnum => (AssetTypeEnum)Id;

        [StringLength(StringLengths.ONE_MAP_FEATURE_SOURCE)]
        public virtual string OneMapFeatureSource { get; set; }

        #endregion

        #region Constructors

        public AssetType()
        {
            OperatingCenterAssetTypes = new List<OperatingCenterAssetType>();
        }

        #endregion
    }
}
