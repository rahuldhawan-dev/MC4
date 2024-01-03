using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapCall.SAP.Model.Entities
{
    public class SAPTechnicalMasterAccountCollection : IEnumerable<SAPTechnicalMasterAccount>
    {
        #region "Public Members"

        public List<SAPTechnicalMasterAccount> Items { get; set; }

        #endregion

        #region "Public Methods"

        public SAPTechnicalMasterAccountCollection()
        {
            Items = new List<SAPTechnicalMasterAccount>();
        }

        public IEnumerator<SAPTechnicalMasterAccount> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        //System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        //{
        //    throw new NotImplementedException();
        //}

        //IEnumerator<SAPTechnicalMasterAccount> IEnumerable<SAPTechnicalMasterAccount>.GetEnumerator()
        //{
        //    throw new NotImplementedException();
        //}

        #endregion
    }
}
