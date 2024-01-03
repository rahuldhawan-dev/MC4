using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MapCall.SAP.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities.Users;

namespace MapCall.SAPTest.Model.Entities
{
    [TestClass()]
    public class SAPCreatePreventiveWorkOrderTest
    {
        #region Private Methods

        public SAPCreatePreventiveWorkOrder GetSAPCreatePreventiveWorkOrderTestForEmptyValues()
        {
            var SapCreatePreventiveWorkOrder = new SAPCreatePreventiveWorkOrder();
            SapCreatePreventiveWorkOrder.PlanningPlant = "";
            SapCreatePreventiveWorkOrder.OrderType = "";
            SapCreatePreventiveWorkOrder.CreatedOn = "";
            SapCreatePreventiveWorkOrder.CompanyCode = "";
            return SapCreatePreventiveWorkOrder;
        }

        public SAPCreatePreventiveWorkOrder GetSAPCreatePreventiveWorkOrderTestForPlanningPlat()
        {
            var SapCreatePreventiveWorkOrder = new SAPCreatePreventiveWorkOrder();
            SapCreatePreventiveWorkOrder.PlanningPlant = "P218";
            SapCreatePreventiveWorkOrder.OrderType = "20011";
            SapCreatePreventiveWorkOrder.CreatedOn = "";
            SapCreatePreventiveWorkOrder.CompanyCode = "";
            return SapCreatePreventiveWorkOrder;
        }

        public SAPCreatePreventiveWorkOrder GetSAPCreatePreventiveWorkOrderTestForCompanyCode()
        {
            var SapCreatePreventiveWorkOrder = new SAPCreatePreventiveWorkOrder();
            SapCreatePreventiveWorkOrder.PlanningPlant = "D217";
            SapCreatePreventiveWorkOrder.OrderType = "0040";
            SapCreatePreventiveWorkOrder.CreatedOn = "";
            SapCreatePreventiveWorkOrder.CompanyCode = "1018";
            return SapCreatePreventiveWorkOrder;
        }

        #endregion
    }
}
