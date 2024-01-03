using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SewerAuthorityCode : SapEntityLookup
    {
        public override string ToString()
        {
            return $"{SAPCode} {Description}";
        }
    }
}
