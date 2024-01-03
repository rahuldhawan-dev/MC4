using System;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ConfinedSpaceFormEntrant : IEntity
    {
        #region Constants

        public struct StringLengths
        {
            #region Constants

            public const int CONTRACTING_COMPANY = 255, CONTRACTOR_NAME = 255;

            #endregion
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual ConfinedSpaceForm ConfinedSpaceForm { get; set; }

        [View("Entry Assignment/Role")]
        public virtual ConfinedSpaceFormEntrantType EntrantType { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual string ContractingCompany { get; set; }
        public virtual string ContractorName { get; set; }

        #endregion
    }
}
