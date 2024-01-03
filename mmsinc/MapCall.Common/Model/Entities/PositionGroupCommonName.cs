using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Migrations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class PositionGroupCommonName : EntityLookup
    {
        public PositionGroupCommonName()
        {
            TrainingRequirements = new List<TrainingRequirement>();
            PositionGroups = new List<PositionGroup>();
        }

        #region Properties

        public virtual IList<TrainingRequirement> TrainingRequirements { get; set; }
        public virtual IList<PositionGroup> PositionGroups { get; set; }

        #endregion
    }
}
