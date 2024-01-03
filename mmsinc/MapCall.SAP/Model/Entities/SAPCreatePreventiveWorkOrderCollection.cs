using System;
using System.Collections.Generic;

namespace MapCall.SAP.Model.Entities
{
    public class SAPCreatePreventiveWorkOrderCollection : IEnumerable<SAPCreatePreventiveWorkOrder>
    {
        #region "Public Members"

        public List<SAPCreatePreventiveWorkOrder> Items { get; set; }

        #endregion

        #region "Public Methods"

        public SAPCreatePreventiveWorkOrderCollection()
        {
            Items = new List<SAPCreatePreventiveWorkOrder>();
        }

        public IEnumerator<SAPCreatePreventiveWorkOrder> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
