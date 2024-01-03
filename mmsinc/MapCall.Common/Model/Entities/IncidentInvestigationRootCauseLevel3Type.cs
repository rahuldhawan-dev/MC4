using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    // This is currently read-only so admin users don't go in and mess with things.
    // The Level3 types depend on this and users don't have a way to
    // properly link those together on the site.
    [Serializable]
    public class IncidentInvestigationRootCauseLevel3Type : ReadOnlyEntityLookup
    {
        #region Properties

        public virtual IncidentInvestigationRootCauseLevel2Type IncidentInvestigationRootCauseLevel2Type { get; set; }

        #endregion
    }
}
