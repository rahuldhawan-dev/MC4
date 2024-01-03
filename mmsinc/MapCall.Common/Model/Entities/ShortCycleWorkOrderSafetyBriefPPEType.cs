using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMSINC.Data;
using MMSINC.Utilities.Json;
using Newtonsoft.Json;

namespace MapCall.Common.Model.Entities
{
    [Serializable, JsonConverter(typeof(ToStringJsonConverter))]
    public class ShortCycleWorkOrderSafetyBriefPPEType : EntityLookup { }
}
