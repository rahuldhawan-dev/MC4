using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ConsolidatedCustomerSideMaterial : IEntity
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual EPACode ConsolidatedEPACode { get; set; }
        public virtual EPACode CustomerSideEPACode { get; set; }
        public virtual EPACode CustomerSideExternalEPACode { get; set; }

        #endregion
    }
}
