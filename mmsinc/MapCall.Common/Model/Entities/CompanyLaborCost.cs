using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class CompanyLaborCost : EntityLookup
    {
        public virtual string Unit { get; set; }

        [DisplayFormat(DataFormatString = "{0:c}")]
        public virtual decimal Cost { get; set; }

        public virtual string LaborItem { get; set; }
    }
}
