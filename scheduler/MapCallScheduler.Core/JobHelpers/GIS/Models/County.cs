using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapCallScheduler.JobHelpers.GIS.Models
{
    public class County
    {
        #region Properties

        public int Id { get; set; }
        public string Name { get; set; }

        #endregion

        #region Exposed Methods

        public static County FromDbRecord(MapCall.Common.Model.Entities.County county)
        {
            return county == null ? null : new County { Id = county.Id, Name = county.Name };
        }

        #endregion
    }
}
