using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class StormWaterAsset : IEntity, IAsset
    {
        #region Consts

        public struct StringLengths
        {
            public const int MAX_ASSET_NUMBER_LENGTH = 50,
                             MAX_CREATED_BY_LENGTH = 50,
                             MAX_TASK_NUMBER_LENGTH = 50;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        /// <summary>
        /// Gets/sets the UNIQUE asset number that identifies this StormWaterAsset.
        /// </summary>
        public virtual string AssetNumber { get; set; }

        public virtual string CreatedBy { get; set; }
        public virtual string TaskNumber { get; set; }

        public virtual StormWaterAssetType AssetType { get; set; }
        public virtual Coordinate Coordinate { get; set; }

        public virtual MapIcon Icon => Coordinate?.Icon;

        // TODO: Should this be renamed CrossStreet for consistancy?
        // public virtual Street IntersectingStreet { get; set; } Street model/map does not exist at the moment.
        public virtual OperatingCenter OperatingCenter { get; set; }

        // public virtual int OperatingCenterId { get; set; }
        // public virtual Street Street { get; set; } Street model/map does not exist at the moment.
        public virtual Town Town { get; set; }

        public virtual string Identifier => AssetNumber;

        #endregion

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override string ToString()
        {
            return AssetNumber;
        }
    }
}
