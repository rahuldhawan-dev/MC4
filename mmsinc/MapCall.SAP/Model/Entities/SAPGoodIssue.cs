using MapCall.Common.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.SAP.GoodsIssueWS;

namespace MapCall.SAP.Model.Entities
{
    [Serializable]
    public class SAPGoodsIssue : SAPEntity
    {
        #region Properties

        #region WebService Request Properties

        public IEnumerable<SAPProductionWorkOrderMaterialUsed> sapMaterialsUsed { get; set; }
        public virtual string AssetType { get; set; }
        public virtual string SAPWorkOrderNo { get; set; }
        public virtual string DateApproved { get; set; }
        public virtual string MaterialPostingDate { get; set; }
        public virtual string MaterialPlanningCompletedOn { get; set; }
        public virtual string DocumentDate { get; set; }

        //New field User Id included as per change request
        public virtual string UserID { get; set; }

        #region Logical properties

        public string PlanningPlan(MaterialUsed materialUsed)
        {
            switch (AssetType)
            {
                case "HYDRANT":
                case "VALVE":
                case "MAIN":
                case "SERVICE":
                    return materialUsed.StockLocation?.OperatingCenter?.DistributionPlanningPlant?.Code;
                case "SEWER OPENING":
                case "SEWER MAIN":
                case "SEWER LATERAL":
                    return materialUsed.StockLocation?.OperatingCenter?.SewerPlanningPlant?.Code;
                default:
                    return string.Empty;
            }
        }

        #endregion

        #endregion

        #region WebService Response Properties

        public virtual string MaterialDocument { get; set; }
        public virtual string OrderNumber { get; set; }
        public virtual string Status { get; set; }

        #endregion

        #endregion

        #region Constructors

        public SAPGoodsIssue(WorkOrder workOrder)
        {
            SAPWorkOrderNo = workOrder.SAPWorkOrderNumber?.ToString();
            AssetType = workOrder.AssetType?.Description.ToUpper();
            DateApproved = workOrder.DateCompleted?.Date.ToString(SAP_DATE_FORMAT);
            MaterialPostingDate = workOrder.MaterialPostingDate?.Date.ToString(SAP_DATE_FORMAT);

            //New field User Id included as per change request
            UserID = workOrder.UserId;

            DocumentDate = DateTime.Now.Date.ToString(SAP_DATE_FORMAT);

            if (AssetType == "MAIN CROSSING")
                AssetType = "MAIN";

            if (workOrder.MaterialsUsed != null && workOrder.MaterialsUsed.Any())
            {
                var MaterialsUsed = from m in workOrder.MaterialsUsed
                                    where m.Material?.PartNumber != null && m.Material?.PartNumber != ""
                                    select new SAPProductionWorkOrderMaterialUsed {
                                        Quantity = m.Quantity.ToString(),
                                        StcokLocation = m.StockLocation?.SAPStockLocation,
                                        PlanningPlan =
                                            PlanningPlan(m), //m.StockLocation?.OperatingCenter?.OperatingCenterCode,
                                        PartNumber = m.Material?.PartNumber,
                                        Description = m.Material?.Description ?? m.NonStockDescription,
                                    };

                sapMaterialsUsed = MaterialsUsed.ToList();
            }
        }

        public SAPGoodsIssue(GoodsIssueStatusGoodsIssueStatus GoodsIssueResponse)
        {
            MaterialDocument = GoodsIssueResponse.MaterialDocument;
            OrderNumber = GoodsIssueResponse.OrderNumber;
            Status = GoodsIssueResponse.Status;
        }

        public SAPGoodsIssue() { }

        #endregion

        #region Exposed Methods

        public GoodsIssueGoodsIssue[] ApproveGoodIssue()
        {
            GoodsIssueGoodsIssue[] goodsIssueGoodsIssue = new GoodsIssueGoodsIssue[sapMaterialsUsed.ToList().Count];

            if (sapMaterialsUsed != null && sapMaterialsUsed.Any())
            {
                for (int i = 0; i < sapMaterialsUsed.ToList().Count; i++)
                {
                    goodsIssueGoodsIssue[i] = new GoodsIssueGoodsIssue();
                    goodsIssueGoodsIssue[i].MaterialNumber = sapMaterialsUsed.ToList()[i].PartNumber;
                    goodsIssueGoodsIssue[i].Quantity = sapMaterialsUsed.ToList()[i].Quantity;
                    goodsIssueGoodsIssue[i].StorageLocation = sapMaterialsUsed.ToList()[i].StcokLocation;
                    goodsIssueGoodsIssue[i].Plant = sapMaterialsUsed.ToList()[i].PlanningPlan;
                    goodsIssueGoodsIssue[i].PostingDate = MaterialPostingDate;
                    goodsIssueGoodsIssue[i].DocumentDate = DocumentDate;
                    goodsIssueGoodsIssue[i].Order = SAPWorkOrderNo;
                    //New field User Id included as per change request
                    goodsIssueGoodsIssue[i].UserID = UserID;
                }
            }

            return goodsIssueGoodsIssue;
        }

        #endregion
    }
}
