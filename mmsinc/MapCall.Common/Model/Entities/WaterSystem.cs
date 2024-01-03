using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class WaterSystem : EntityLookup
    {
        #region Private Members

        private WaterSystemDisplayItem _display;

        #endregion

        #region Properties

        [StringLength(255)]
        public virtual string LongDescription { get; set; }

        public virtual string Display =>
            (_display ?? (_display =
                new WaterSystemDisplayItem {Description = Description, LongDescription = LongDescription})).Display;

        public virtual IList<OperatingCenter> OperatingCenters { get; set; }

        #endregion

        #region Constructors

        public WaterSystem()
        {
            OperatingCenters = new List<OperatingCenter>();
        }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return Display;
        }

        #endregion
    }

    [Serializable]
    public class WaterSystemDisplayItem : DisplayItem<WaterSystem>
    {
        #region Properties

        public string Description { get; set; }
        public string LongDescription { get; set; }

        public override string Display => $"{Description} - {LongDescription}";

        #endregion
    }
}
