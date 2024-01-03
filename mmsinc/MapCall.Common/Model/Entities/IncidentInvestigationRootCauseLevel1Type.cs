using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    // This is currently read-only so admin users don't go in and mess with things.
    // The Level2 and Level3 types depend on this and users don't have a way to
    // properly link those together on the site.
    [Serializable]
    public class IncidentInvestigationRootCauseLevel1Type : ReadOnlyEntityLookup { }
}
