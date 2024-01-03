using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.SAP.WBSElementWS;

namespace MapCall.SAP.Model.Entities
{
    public class SAPWBSElement
    {
        #region properties

        public virtual string WBSNumber { get; set; }
        public virtual string WBSDescription { get; set; }
        public virtual string ProjectDefintion { get; set; }
        public virtual string PlanningPlant { get; set; }
        public virtual string ProjectType { get; set; }
        public virtual string ProfitCentre { get; set; }
        public virtual string ProfitCentreFieldName { get; set; }
        public virtual string AssetCode { get; set; }
        public virtual string StartDate { get; set; }
        public virtual string EndDate { get; set; }
        public virtual string Year { get; set; }
        public virtual string SystemStatus { get; set; }
        public virtual string SAPErrorCode { get; set; }
        public virtual string Status { get; set; }

        #endregion

        #region Exposed methods

        public WBSElementQuery WBSElementRequest()
        {
            WBSElementQuery wbsRequest = new WBSElementQuery();
            wbsRequest.Record = new WBSElementQueryRecord();
            //wbsRequest.Record.AssetCode = AssetCode;
            wbsRequest.Record.PlanningPlant = PlanningPlant;
            wbsRequest.Record.ProfitCentre = ProfitCentre;
            wbsRequest.Record.ProfitCentreFieldName = ProfitCentreFieldName;
            wbsRequest.Record.ProjectDefinition = ProjectDefintion;
            wbsRequest.Record.ProjectType = ProjectType;
            wbsRequest.Record.SystemStatus = SystemStatus;
            wbsRequest.Record.WBSDescription = WBSDescription;
            wbsRequest.Record.WBSNumber = WBSNumber;
            wbsRequest.Record.Year = Year;

            return wbsRequest;
        }

        public SAPWBSElement() { }

        public SAPWBSElement(WBSElementInfoRecord wbsElementInfoRecord)
        {
            WBSNumber = wbsElementInfoRecord.WBSElement;
            WBSDescription = wbsElementInfoRecord.WBSDescription;
            StartDate = wbsElementInfoRecord.StartDate;
            EndDate = wbsElementInfoRecord.EndDate;
            Status = wbsElementInfoRecord.StatusCode == "Released" ? "Open" : "Complete";
            SAPErrorCode = "Successful";
        }

        #endregion
    }
}
