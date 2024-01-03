using System;
using System.Collections.Generic;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EquipmentLifespan : EntityLookup
    {
        #region Constants

        public struct Indices
        {
            public const int CHEMICAL_FEED_DRY = 1, GENERATOR = 9, ENGINE = 22, FILTER = 8;
        }

        public struct DisplayNames
        {
            public const string EXTENDED_LIFE_MAJOR = "Extended Life (Major)",
                                EXTENDED_LIFE_MINOR = "Extended Life (Minor)",
                                IS_ACTIVE = "Active";
        }

        #endregion

        #region Properties

        [View(DisplayName = DisplayNames.EXTENDED_LIFE_MAJOR)]
        public virtual decimal? ExtendedLifeMajor { get; set; }
       
        [View(DisplayName = DisplayNames.EXTENDED_LIFE_MINOR)]
        public virtual decimal? ExtendedLifeMinor { get; set; }
        
        public virtual decimal? EstimatedLifespan { get; set; }

        [View(DisplayName = DisplayNames.IS_ACTIVE)]
        public virtual bool? IsActive { get; set; }

        public virtual IList<EquipmentPurpose> EquipmentPurposes { get; set; }
        public virtual IList<TaskGroup> TaskGroups { get; set; }

        #endregion

        #region Constructors

        public EquipmentLifespan()
        {
            EquipmentPurposes = new List<EquipmentPurpose>();
        }

        #endregion
    }
}
