using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class HelpCategory : EntityLookup
    {
        public override string ToString()
        {
            return Description;
        }
    }
}
