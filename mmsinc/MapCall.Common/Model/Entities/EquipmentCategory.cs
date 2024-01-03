using System;
using System.Collections.Generic;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EquipmentCategory : EntityLookup
    {
        #region Constants

        public struct Indices
        {
            public const int FLOW_METER = 3;
        }

        #endregion

        #region Properties

        public virtual IList<EquipmentPurpose> EquipmentPurposes { get; set; }

        #endregion

        #region Constructors

        public EquipmentCategory()
        {
            EquipmentPurposes = new List<EquipmentPurpose>();
        }

        #endregion
    }
}
