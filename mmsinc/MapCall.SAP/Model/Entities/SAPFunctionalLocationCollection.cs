using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapCall.SAP.Model.Entities
{
    public class SAPFunctionalLocationCollection : IEnumerable<SAPFunctionalLocation>
    {
        #region "Public Members"

        public List<SAPFunctionalLocation> Items { get; set; }

        #endregion

        #region "Public Methods"

        public SAPFunctionalLocationCollection()
        {
            Items = new List<SAPFunctionalLocation>();
        }

        public IEnumerator<SAPFunctionalLocation> GetEnumerator()
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
