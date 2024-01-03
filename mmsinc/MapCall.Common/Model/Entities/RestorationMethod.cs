using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class RestorationMethod : EntityLookup
    {
        public virtual IList<RestorationType> RestorationTypes { get; set; }

        public RestorationMethod()
        {
            RestorationTypes = new List<RestorationType>();
        }
    }
}
