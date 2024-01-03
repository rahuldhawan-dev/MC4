using MMSINC.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MapCall.SAP.Model.Entities;
using MMSINC.Data;

namespace MapCallMVC.Areas.SAP.Models.ViewModels
{
    public class SearchSAPPreventiveWorkOrdercs : SearchSet<SAPCreatePreventiveWorkOrder>
    {

        #region Properties

        public virtual string PlanningPlant { get; set; }
        public virtual string CreatedOn { get; set; }
        public virtual string CompanyCode { get; set; }
        public virtual string OrderType { get; set; }
        public virtual string OrderNumber { get; set; }
        public virtual string LastRunTime { get; set; }

        #endregion

        public SAPCreatePreventiveWorkOrder ToSearchPreventiveWorkOrder()
        {
            return new SAPCreatePreventiveWorkOrder
            {
                PlanningPlant = PlanningPlant ?? string.Empty,
                CreatedOn = CreatedOn ?? string.Empty,
                CompanyCode = CompanyCode ?? string.Empty,
                OrderType = OrderType?? string.Empty,
                OrderNumber = OrderNumber ?? string.Empty,
            };
        }
    }
}