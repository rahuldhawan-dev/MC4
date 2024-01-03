using System;
using System.Collections.Generic;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class AtRiskBehaviorSection : EntityLookup
    {
        #region Properties

        public virtual int SectionNumber { get; set; }
        public virtual IList<AtRiskBehaviorSubSection> SubSections { get; set; }

        #endregion

        #region Constructors

        public AtRiskBehaviorSection()
        {
            SubSections = new List<AtRiskBehaviorSubSection>();
        }

        #endregion

        public override string ToString()
        {
            return string.Format("{0} - {1}", SectionNumber, Description);
        }
    }
}
