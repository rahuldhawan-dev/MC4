using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapCall.SAP.Model.Entities
{
    [Serializable]
    public class SAPCreateBPEM : SAPEntity, IHasStatus
    {
        #region Properties

        public virtual string CaseCategory { get; set; }
        public virtual string CaseNumber { get; set; }
        public virtual string CasePriority { get; set; }
        public virtual string AuthorizationGroup { get; set; }
        public virtual string ObjectType { get; set; }
        public virtual string Objectkey { get; set; }
        public virtual string OriginalDateofClarificationCase { get; set; }
        public virtual string CreationTimeofClarificationCase { get; set; }
        public virtual string Logicalsystem { get; set; }
        public virtual string CompanyCode { get; set; }
        public virtual string BusinessPartnerNumber { get; set; }
        public virtual string ContractAccountNumber { get; set; }
        public virtual string Premise { get; set; }

        #endregion

        #region WebService Response Properties

        public virtual string SAPStatus { get; set; }

        #endregion
    }
}
