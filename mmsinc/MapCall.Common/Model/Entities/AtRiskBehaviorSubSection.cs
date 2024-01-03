using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class AtRiskBehaviorSubSection : EntityLookup
    {
        #region Properties

        public virtual decimal SubSectionNumber { get; set; }
        public virtual AtRiskBehaviorSection Section { get; set; }

        #endregion

        public override string ToString()
        {
            return string.Format("{0} - {1}", SubSectionNumber, Description);
        }
    }
}
