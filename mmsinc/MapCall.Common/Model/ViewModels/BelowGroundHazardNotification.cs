using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.ViewModels
{
    public class BelowGroundHazardNotification
    {
        #region Properties

        public BelowGroundHazard BelowGroundHazard { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string RecordUrl { get; set; }

        #endregion
    }
}
