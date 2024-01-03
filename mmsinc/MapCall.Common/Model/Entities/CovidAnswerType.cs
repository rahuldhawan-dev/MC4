using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class CovidAnswerType : EntityLookup
    {
        public struct Indices
        {
            public const int YES = 1, NO = 2, TBD = 3, CONTACT_TRACER = 4;
        }
    }
}
