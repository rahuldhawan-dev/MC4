using MMSINC.Data;
using System;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class PublicWaterSupplyLicensedOperator : IEntity
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual OperatorLicense LicensedOperator { get; set; }
        public virtual PublicWaterSupply PublicWaterSupply { get; set; }

        #endregion
    }
}
