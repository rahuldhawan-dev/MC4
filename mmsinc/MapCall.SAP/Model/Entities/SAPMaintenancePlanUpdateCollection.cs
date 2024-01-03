using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapCall.SAP.Model.Entities
{
    public class SAPMaintenancePlanUpdateCollection : IEnumerable<SAPMaintenancePlanUpdate>
    {
        #region "Public Members"

        public List<SAPMaintenancePlanUpdate> Items { get; set; }

        #endregion

        #region "Public Methods"

        public SAPMaintenancePlanUpdateCollection()
        {
            Items = new List<SAPMaintenancePlanUpdate>();
        }

        public IEnumerator<SAPMaintenancePlanUpdate> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
