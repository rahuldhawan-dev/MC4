using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Migrations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EstimatingProjectType : ReadOnlyEntityLookup
    {
        #region Constants

        public struct Indices
        {
            public const int FRAMEWORK = 1, NON_FRAMEWORK = 2;
        }

        #endregion

        #region Properties

        [Required, StringLength(CreateTablesForBug1774.StringLengths.ProjectTypes.DESCRIPTION)]
        public override string Description { get; set; }

        #endregion
    }
}
