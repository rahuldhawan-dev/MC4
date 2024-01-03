using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;

namespace MapCall.SAP.Model.Entities
{
    public class SAPGoodsIssueCollection : IEnumerable<SAPGoodsIssue>
    {
        #region "Public Members"

        public List<SAPGoodsIssue> Items { get; set; }

        #endregion

        #region "Public Methods"

        public SAPGoodsIssueCollection()
        {
            Items = new List<SAPGoodsIssue>();
        }

        public IEnumerator<SAPGoodsIssue> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void MapToWorkOrder(ISapWorkOrder workOrder)
        {
            if (Items != null && Items.Any())
            {
                var s = Items.FirstOrDefault(i => !string.IsNullOrWhiteSpace(i.MaterialDocument))?.MaterialDocument;
                if (!string.IsNullOrWhiteSpace(s))
                {
                    workOrder.MaterialsDocID = s;
                }

                s = Items.FirstOrDefault(i => !string.IsNullOrWhiteSpace(i.Status))?.Status;
                if (!string.IsNullOrWhiteSpace(s))
                {
                    workOrder.SAPErrorCode = s;
                }
            }
        }

        #endregion
    }
}
