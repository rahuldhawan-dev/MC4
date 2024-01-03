using System;
using System.Collections.Generic;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class IncidentInvestigation : IEntity
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual Incident Incident { get; set; }

        [View("Root Cause Finding")]
        public virtual IncidentInvestigationRootCauseFindingType IncidentInvestigationRootCauseFindingType { get; set; }

        [View("Root Cause Level 1")]
        public virtual IncidentInvestigationRootCauseLevel1Type IncidentInvestigationRootCauseLevel1Type { get; set; }

        [View("Root Cause Level 2")]
        public virtual IncidentInvestigationRootCauseLevel2Type IncidentInvestigationRootCauseLevel2Type { get; set; }

        [View("Root Cause Level 3")]
        public virtual IncidentInvestigationRootCauseLevel3Type IncidentInvestigationRootCauseLevel3Type { get; set; }

        [View("Root Cause Finding Performed By")]
        public virtual IList<User> RootCauseFindingPerformedByUsers { get; set; }

        #endregion

        #region Constructor

        public IncidentInvestigation()
        {
            RootCauseFindingPerformedByUsers = new List<User>();
        }

        #endregion
    }
}
