using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapCallScheduler.JobHelpers.GIS.Models
{
    public class OperatingCenter
    {
        #region Properties

        public int Id { get; set; }
        public string OperatingCenterCode { get; set; }
        public string OperatingCenterName { get; set; }

        #endregion

        #region Exposed Methods

        public static OperatingCenter FromDbRecord(MapCall.Common.Model.Entities.OperatingCenter operatingCenter)
        {
            return operatingCenter == null ? null : new OperatingCenter { Id = operatingCenter.Id, OperatingCenterCode = operatingCenter.OperatingCenterCode, OperatingCenterName  = operatingCenter.OperatingCenterName };
        }

        #endregion
    }
}
