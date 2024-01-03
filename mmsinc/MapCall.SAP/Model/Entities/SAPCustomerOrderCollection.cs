using System;
using System.Collections.Generic;

namespace MapCall.SAP.Model.Entities
{
    public class SAPCustomerOrderCollection : IEnumerable<SAPCustomerOrder>
    {
        #region "Public Members"

        public List<SAPCustomerOrder> Items { get; set; }

        #endregion

        #region "Public Methods"

        public SAPCustomerOrderCollection()
        {
            Items = new List<SAPCustomerOrder>();
        }

        public IEnumerator<SAPCustomerOrder> GetEnumerator()
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
