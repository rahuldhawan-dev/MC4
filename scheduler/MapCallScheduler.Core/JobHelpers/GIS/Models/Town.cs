using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapCallScheduler.JobHelpers.GIS.Models
{
    public class Town
    {
        #region Properties

        public int Id { get; set; }
        public string FullName { get; set; }

        #endregion

        #region Exposed Methods

        public static Town FromDbRecord(MapCall.Common.Model.Entities.Town town)
        {
            return town == null ? null : new Town { Id = town.Id, FullName = town.FullName };
        }

        #endregion
    }
}
