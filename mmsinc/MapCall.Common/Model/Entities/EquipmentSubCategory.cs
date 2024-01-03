using System;
using System.Collections.Generic;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EquipmentSubCategory : EntityLookup
    {
        #region Constants

        public struct Indices
        {
            public const int DELIVERED_WATER = 36,
                             PURCHASED_WATER = 37,
                             TRANSFERRED_WATER = 38,
                             WASTE_WATER = 39;
        }

        public static readonly int[] SYSTEM_DELIVERY_SUB_CATEGORIES = {36, 37, 38, 39};

        #endregion

        #region Properties

        public virtual IList<EquipmentPurpose> EquipmentPurposes { get; set; }

        #endregion

        #region Constructors

        public EquipmentSubCategory()
        {
            EquipmentPurposes = new List<EquipmentPurpose>();
        }

        #endregion
    }
}
