using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ConfinedSpaceFormHazard : IEntity
    {
        #region Consts

        public struct StringLengths
        {
            public const int NOTES = 255;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual ConfinedSpaceForm ConfinedSpaceForm { get; set; }
        public virtual ConfinedSpaceFormHazardType HazardType { get; set; }
        public virtual string Notes { get; set; }

        #endregion
    }
}
