using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapCall.SAP.Model.ViewModels.SAPDeviceDetail
{
    public class SAPStatus
    {
        public virtual string ReturnStatusType { get; set; }

        public virtual string ReturnStatusDescription { get; set; }
    }
}
