using System.Collections;
using System.Collections.Generic;

namespace MapCall.SAP.Model.Entities
{
    public class SAPMaintenancePlanLookupCollection : IEnumerable<SAPMaintenancePlanLookup>
    {
        #region "Public Members"

        public List<SAPMaintenancePlanLookup> Items { get; set; }

        #endregion

        #region "Public Methods"

        public SAPMaintenancePlanLookupCollection()
        {
            Items = new List<SAPMaintenancePlanLookup>();
        }

        public IEnumerator<SAPMaintenancePlanLookup> GetEnumerator()
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
