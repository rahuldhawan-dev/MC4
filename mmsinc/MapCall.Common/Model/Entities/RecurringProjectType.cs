﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class RecurringProjectType : EntityLookup, IEntityWithCreationTimeTracking
    {
        #region Constants

        #endregion

        #region Properties

        [Required]
        public virtual string CreatedBy { get; set; }

        [Required]
        public virtual DateTime CreatedAt { get; set; }

        public virtual IList<RecurringProject> RecurringProjects { get; set; }

        #endregion

        #region Constructors

        public RecurringProjectType()
        {
            RecurringProjects = new List<RecurringProject>();
        }

        #endregion
    }
}
