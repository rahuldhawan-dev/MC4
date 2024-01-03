using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapCall.Common.Model.Migrations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class PremiseType : EntityLookup
    {
        public new struct StringLengths
        {
            public const int DESCRIPTION = AddPremiseTypes.StringLengths.DESCRIPTION,
                             ABBREVIATION = AddPremiseTypes.StringLengths.ABBREVIATION;
        }

        public virtual string Abbreviation { get; set; }
    }
}
