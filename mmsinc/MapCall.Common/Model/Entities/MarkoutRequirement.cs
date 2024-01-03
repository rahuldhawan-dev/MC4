using System;
using MapCall.Common.Utility;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class MarkoutRequirement : ReadOnlyEntityLookup
    {
        #region Consts

        public const int DESCRIPTION_MAX_LENGTH = 10;

        public enum Indices
        {
            NONE = 1,
            ROUTINE = 2,
            EMERGENCY = 3
        }

        #endregion

        /// <summary>
        /// MarkoutRequirementEnum value indicating the level of requirement.
        /// </summary>
        public virtual MarkoutRequirementEnum MarkoutRequirementEnum => (MarkoutRequirementEnum)Id;
    }
}
