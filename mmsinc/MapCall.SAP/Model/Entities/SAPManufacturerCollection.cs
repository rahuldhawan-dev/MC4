using System.Collections;
using System.Collections.Generic;

namespace MapCall.SAP.Model.Entities
{
    public class SAPManufacturerCollection : IEnumerable<SAPManufacturer>
    {
        #region "Public Members"

        public List<SAPManufacturer> Items { get; set; }

        #endregion

        #region "Public Methods"

        public SAPManufacturerCollection()
        {
            Items = new List<SAPManufacturer>();
        }

        public IEnumerator<SAPManufacturer> GetEnumerator()
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
