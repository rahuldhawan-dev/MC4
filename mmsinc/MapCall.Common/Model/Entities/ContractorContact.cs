using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ContractorContact : IEntity
    {
        #region Properties

        public virtual int Id { get; set; }

        public virtual Contractor Contractor { get; set; }

        public virtual Contact Contact { get; set; }

        public virtual ContactType ContactType { get; set; }

        #endregion
    }
}
