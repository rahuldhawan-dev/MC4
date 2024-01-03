using System;
using System.Collections.Generic;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SafetyImplementationCategory : EntityLookup
    {
        #region Properties

        public virtual IList<BappTeamIdea> BappTeamIdeas { get; set; }

        #endregion

        #region Constructor

        public SafetyImplementationCategory()
        {
            BappTeamIdeas = new List<BappTeamIdea>();
        }

        #endregion
    }
}
