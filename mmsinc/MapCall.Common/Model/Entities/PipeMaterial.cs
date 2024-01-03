using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class PipeMaterial : EntityLookup
    {
        #region Properties

        public virtual IList<RecurringProject> RecurringProjects { get; set; }

        #endregion

        #region Constructors

        public PipeMaterial()
        {
            RecurringProjects = new List<RecurringProject>();
        }

        #endregion
    }
}
