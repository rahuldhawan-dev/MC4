using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class StockLocation : EntityLookup
    {
        public new struct StringLengths
        {
            public const int SAP_STOCK_LOCATION = 50, DESCRIPTION = 25;
        }

        public virtual OperatingCenter OperatingCenter { get; set; }

        [StringLength(StringLengths.SAP_STOCK_LOCATION)]
        public virtual string SAPStockLocation { get; set; }

        public virtual bool IsActive { get; set; }

        public override string ToString()
        {
            return Description;
        }
    }
}
