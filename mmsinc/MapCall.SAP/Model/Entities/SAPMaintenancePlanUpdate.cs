using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.SAP.MaintenancePlanUpdateWS;

namespace MapCall.SAP.Model.Entities
{
    public class SAPMaintenancePlanUpdate : SAPEntity
    {
        #region Properties

        #region Request Properoties

        public virtual SAPFixCall SapFixCall { get; set; }
        public virtual SAPSkipCall SapSkipCall { get; set; }
        public virtual SAPManualCall SapManualCall { get; set; }
        public virtual IList<SAPAddRemoveItem> SapAddRemoveItem { get; set; }

        #endregion

        #region Response Properoties

        public string MaintenancePlan { get; set; }
        public string SAPErrorCode { get; set; }

        #endregion

        #endregion

        #region Exposed Methods

        public SAPMaintenancePlanUpdate()
        {
            SapAddRemoveItem = new List<SAPAddRemoveItem>();
        }

        public MaintenancePlan_Update Request()
        {
            var request = new MaintenancePlan_Update();
            if (SapFixCall != null)
            {
                request.FixCall = new MaintenancePlan_UpdateFixCall();
                request.FixCall.CallNumber = SapFixCall.CallNumber;
                request.FixCall.MaintenancePlan = SapFixCall.MaintenancePlan;
                request.FixCall.PlanDate = SapFixCall.PlanDate?.Date.ToString(SAP_DATE_FORMAT);
            }

            if (SapManualCall != null)
            {
                request.ManualCall = new MaintenancePlan_UpdateManualCall();
                request.ManualCall.MaintenancePlan = SapManualCall.MaintenancePlan;
                request.ManualCall.ManualCallDate = SapManualCall.ManualCallDate?.Date.ToString(SAP_DATE_FORMAT);
                request.ManualCall.MaintPack = SapManualCall.MaintenancePackage;
            }

            if (SapSkipCall != null)
            {
                request.SkipCall = new MaintenancePlan_UpdateSkipCall();
                request.SkipCall.MaintenancePlan = SapSkipCall.MaintenancePlan;
                request.SkipCall.CallNumber = SapSkipCall.CallNumber;
            }

            if (SapAddRemoveItem != null && SapAddRemoveItem.Any())
            {
                request.UpdateItemList = new MaintenancePlan_UpdateUpdateItemList[SapAddRemoveItem.ToList().Count];
                for (int i = 0; i < SapAddRemoveItem.ToList().Count; i++)
                {
                    request.UpdateItemList[i] = new MaintenancePlan_UpdateUpdateItemList();
                    request.UpdateItemList[i].MaintenancePlan = SapAddRemoveItem.ToList()[i].MaintenancePlan;
                    request.UpdateItemList[i].Item = SapAddRemoveItem.ToList()[i].Item;
                    request.UpdateItemList[i].FunctionalLocation = SapAddRemoveItem.ToList()[i].FunctionalLocation;
                    request.UpdateItemList[i].Action = SapAddRemoveItem.ToList()[i].SapAction;
                    request.UpdateItemList[i].Equipment = SapAddRemoveItem.ToList()[i].Equipment;
                }
            }

            return request;
        }

        #endregion
    }

    public class SAPAddRemoveItem
    {
        public virtual string MaintenancePlan { get; set; }
        public virtual string Item { get; set; }
        public virtual string Action { get; set; }
        public virtual string Equipment { get; set; }
        public virtual string FunctionalLocation { get; set; }

        #region Logical properties

        public virtual string SapAction
        {
            get
            {
                switch (Action)
                {
                    case "ADD":
                        return "AD";
                    case "REPLACE":
                        return "RE";
                    case "REMOVE":
                        return "RV";
                    default:
                        return string.Empty;
                }
            }
        }

        #endregion
    }

    public class SAPFixCall
    {
        public virtual string MaintenancePlan { get; set; }
        public virtual string CallNumber { get; set; }
        public virtual DateTime? PlanDate { get; set; }
    }

    public class SAPSkipCall
    {
        public virtual string MaintenancePlan { get; set; }
        public virtual string CallNumber { get; set; }
    }

    public class SAPManualCall
    {
        public virtual DateTime? ManualCallDate { get; set; }
        public virtual string MaintenancePlan { get; set; }
        public virtual string MaintenancePackage { get; set; }
    }
}
