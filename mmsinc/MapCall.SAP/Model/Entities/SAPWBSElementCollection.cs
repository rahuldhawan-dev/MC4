using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapCall.SAP.Model.Entities
{
    public class SAPWBSElementCollection : IEnumerable<SAPWBSElement>
    {
        #region "Public Members"

        public List<SAPWBSElement> Items { get; set; }

        #endregion

        #region "Public Methods"

        public SAPWBSElementCollection()
        {
            Items = new List<SAPWBSElement>();
        }

        public IEnumerator<SAPWBSElement> GetEnumerator()
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
